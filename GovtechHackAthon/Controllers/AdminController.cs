using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GovtechDBLib.Models;
using GovtechHackAthon.Helpers;
using GovtechHackAthon.Models;
using GovtechHackAthon.Services;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace GovtechHackAthon.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly GovtechHackathonContext _govtechHackathonContext;
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly IConfiguration _configuration;
        private readonly DBAPI _dbAPI;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(GovtechHackathonContext govtechHackathonContext,
                               UserManager<IdentityUser> userManager,
                               RoleManager<IdentityRole> roleManger,
                               IConfiguration configuration,
                               DBAPI dbAPI)
        {
            _govtechHackathonContext = govtechHackathonContext;
            _roleManger = roleManger;
            _configuration = configuration;
            _dbAPI = dbAPI;
            _userManager = userManager;
        }

        public async Task<IActionResult> Calendar()
        {
            var model = new AdminCalendar();
            await model.Load(_govtechHackathonContext);
            return View(model);
        }

        public async Task<IActionResult> UpdateCalendar(AdminCalendar model)
        {
            var ctx = _govtechHackathonContext;
           foreach(var modelEvent in model.Events)
            {
                var dates = modelEvent.SplitDates();

                ctx.Dates.Attach(dates[0]);
                ctx.Entry(dates[0]).State = EntityState.Modified;
                ctx.Dates.Attach(dates[1]);
                ctx.Entry(dates[1]).State = EntityState.Modified;
                
            };
            await ctx.SaveChangesAsync();
            //var model = new AdminCalendar();
            //await model.Load(_govtechHackathonContext);
            model.ShowUpdateSuccess = true;
            return View("Calendar",model);
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = new AdminDashboard();

            var ctx = _govtechHackathonContext;
            var applicants = await ctx.Applicant.ToListAsync();
            var applications = await ctx.CaseInformation.Include("CaseCategoryScore").Include("FkStatus")
                                        .Include("CaseAssignments").Where(x => x.FkStatus.Name != "Canceled").ToListAsync();
            model.RevewiedApplications = applications.Where(x => x.CaseCategoryScore.Count > 0).Count();
            model.UnassignedApplications = applications.Where(x => x.CaseAssignments.Count == 0).Count();
            model.TotalApplicants = applicants.Count();
            model.TotalApplications = applications.Count();

            return View(model);
        }

        public IActionResult Analytics()
        {

            return View();
        }

        [AcceptVerbs("GET")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmailDoesntExist([Bind(Prefix = "Email")] string emailAddress, [Bind(Prefix = "UserID")] int userID)
        {
            if (emailAddress != null)
            {
                var ctx = _govtechHackathonContext;
                var dbUser = await ctx.User.FirstOrDefaultAsync(x => x.Email == emailAddress && x.PkId != userID && x.IsActive);
                if (dbUser != null)
                    return Json($"Email address already registered");

                return Json(true);
            }

            return Json(true);
        }

        public async Task<IActionResult> CaseDetail(int caseID)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<UserDetails>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;
                var model = await AdminCaseDetail.Load(caseID, currentUser.UserID,2, _govtechHackathonContext);
  
               
                return View(model);
            }

            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCaseResult(AdminApproveReject model, String submitCase)
        {
            if (ModelState.IsValid)
            {
                var ctx = _govtechHackathonContext;
                var adminAproved = new CaseAdminApproved()
                {
                    FkCaseId = model.CaseID,
                    Comment = model.Comment,
                    Approved = submitCase == "Approved"
                };


                ctx.CaseAdminApproved.Add(adminAproved);
                await ctx.SaveChangesAsync();

                if (model.EmailApplicant)
                {
                    var notificationMgr = new NotificationManager(_govtechHackathonContext,_configuration);
                    var caseInfo = ctx.CaseInformation.Include("FkUser").FirstOrDefault(x => x.PkId == model.CaseID);
                    var email = caseInfo.FkUser.EmailAddress;
                    var mailHelper = new MailHelper(_configuration);
                    var content = "Applicant: " + caseInfo.FkUser.FirstName + " " + caseInfo.FkUser.LastName + "<br>";
                    content += "Case : " + caseInfo.PkId + " " + caseInfo.Name + "<br>";
                    content += "Your application has been:"+ submitCase + "<br>";
                    content += "Reason : "+model.Comment;

                    await notificationMgr.NotifyTeam(caseInfo.PkId, "Your proposal has been "+ submitCase, content);
             
                }
                return RedirectToAction("AllCasesList"); 
            }
            return RedirectToAction("CaseDetail", model.CaseID);
        }

        public async Task<IActionResult> NextCase(int caseID)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<UserDetails>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;

                var dbCaseInfos = await ctx.CaseInformation.Include("FkStatus").Where(x => x.FkStatus.Name == "Submitted" || x.FkStatus.Name == "Deferred" || x.FkStatus.Name == "In Review").ToListAsync();
                var nextIDS = dbCaseInfos.OrderBy(x => x.PkId).Select(x => x.PkId).Where(x => x > caseID).ToList();
                if (nextIDS.Count > 0)
                {
                    return RedirectToAction("CaseDetail", new { CaseID = nextIDS.First() });
                }
                else
                {
                    var nextID = dbCaseInfos.First().PkId;
                    return RedirectToAction("CaseDetail", new { CaseID = nextID });
                }
              
            }

            return RedirectToAction("index", "home");
        }

        public async Task<ActionResult> PreviousCase(int caseID)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<UserDetails>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;

                var dbCaseInfos = await ctx.CaseInformation.Include("FkStatus").Where(x => x.FkStatus.Name == "Submitted" || x.FkStatus.Name == "Deferred" || x.FkStatus.Name== "In Review").ToListAsync();        
                var prevIDS = dbCaseInfos.OrderBy(x=>x.PkId).Select(x => x.PkId).Where(x => x < caseID).ToList();
                if (prevIDS.Count > 0)
                {
                    return RedirectToAction("CaseDetail", new { CaseID = prevIDS.Last() });
                }
                else
                {
                    var nextID = dbCaseInfos.Last().PkId;
                    return RedirectToAction("CaseDetail", new { CaseID = nextID });
                }

            }

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> UpdateGroup(GroupItem model)
        {
            var ctx = _govtechHackathonContext;
            var dbGroup = await ctx.Group.FirstOrDefaultAsync(x => x.PkId == model.GroupID);
            dbGroup.Name = model.Name;
            dbGroup.Active = model.IsActive;
            await ctx.SaveChangesAsync();
            return RedirectToAction("Groups");
        }

        public async Task<IActionResult> AddGroup(GroupItem model)
        {
            var ctx = _govtechHackathonContext;
            var dbGroup = new Group()
            {
                Name = model.Name,
                Active = model.IsActive
            };

            ctx.Group.Add(dbGroup);
            await ctx.SaveChangesAsync();
            return RedirectToAction("Groups");
        }

        public async Task<IActionResult> AddUsersToGroup(UserListSelection model)
        {
            var ctx = _govtechHackathonContext;
            var group = await ctx.Group.FirstOrDefaultAsync(x => x.PkId == model.GroupID);
            foreach (var user in model.SelectedUsers)
            {
                var userInGroup = new UsersInGroup()
                {
                    FkGroupId = model.GroupID,
                    FkUserId = user.UserID
                };

                ctx.UsersInGroup.Add(userInGroup);
            }

            await ctx.SaveChangesAsync();
            return RedirectToAction("Groups");
        }


        public async Task<IActionResult> RemoveUsersGroup(UserListSelection model)
        {
            var ctx = _govtechHackathonContext;
            var selectedIDs = model.SelectedUsers.Select(x => x.UserID).ToList();

            var usersInGroups = await ctx.UsersInGroup.Where(x => selectedIDs.Contains(x.FkUserId) && x.FkGroupId == model.GroupID).ToListAsync();
            ctx.UsersInGroup.RemoveRange(usersInGroups);
            await ctx.SaveChangesAsync();
            return RedirectToAction("Groups");
        }

        public async Task<IActionResult> Groups()
        {
            var ctx = _govtechHackathonContext;
            var model = new Groups();
            var dbGroups = await ctx.Group.Include("UsersInGroup").Include("UsersInGroup.FkUser").ToListAsync();
            var grpItems = dbGroups.Select(x => (GroupItem)x).ToList();
            model.GroupsList.AddRange(grpItems);

            return View(model);
        }

        public async Task<IActionResult> Investors()
        {
            var model = new InvestorsTable();
            await  model.GetAllInvestors(_govtechHackathonContext);
            return View(model);
        }

        public async Task<IActionResult> Scoreboard()
        {
            var model = new Scoreboard();
            var ctx = _govtechHackathonContext;
            var groupUsers = await ctx.UsersInGroup.Include("FkGroup").Include("FkUser").ToListAsync();

            var allCases = await ctx.CaseInformation.Include("CaseAssignments").
                                Include("CaseCategoryScore").Include("CaseCategoryScore.FkUser").
                                Include("CaseCategoryScore.FkCategory").ToListAsync();

            foreach (var caseItem in allCases)
            {
                if (caseItem.CaseAssignments.Count>0)
                {
                    var caseGrpID = caseItem.CaseAssignments.FirstOrDefault().FkGroupId;
                    var usersInGrp = groupUsers.Where(x => x.FkGroupId == caseGrpID);
                    foreach (var usrGrp in usersInGrp)
                    {

                        var scoreboardItem = new ScoreboardItem()
                        {
                            CaseName = caseItem.Name,
                            Adjudicator = usrGrp.FkUser.FirstName + " " + usrGrp.FkUser.Surname,
                            Group = usrGrp.FkGroup.Name
                        };

                        var scores = caseItem.CaseCategoryScore.Where(x => x.FkUserId == usrGrp.FkUserId);
                        foreach (var score in scores)
                        {
                            var scoreboardScore = new ScoreboardCategoryScore()
                            {
                                CategoryID= score.FkCategory.PkId,
                                CategoryName = score.FkCategory.Description,
                                Score = score.Score
                            };

                            scoreboardItem.Scores.Add(scoreboardScore);
                        }

                        model.Items.Add(scoreboardItem);
                    }
                }
             
            }
            model.Rank();
            return View(model);
        }

        public async Task<IActionResult> Users()
        {
            var userList = new UserList();
            userList.Users.AddRange(await _dbAPI.GetUserListAsync());
            return View(userList);
        }

        [HttpGet]
        public async Task<IActionResult> AllCasesList()
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            var model = new AllCasesList();
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;
                var dbCaseInfos = await ctx.CaseInformation.Include("FkStatus").Include("CaseAdminApproved").Where(x => x.FkStatus.Name == "Submitted" ).ToListAsync();
                var dbCaseAssignments = await ctx.CaseAssignments.Include("FkCase").Include("FkCase.CaseAdminApproved").ToListAsync();
                var assignments = dbCaseInfos.Select(x => (CaseAssignmentEntry)x).ToList();
                model.CaseAssignments.AddRange(assignments);

                assignments = dbCaseAssignments.Select(x => (CaseAssignmentEntry)x).ToList();
                model.CaseAssignments.AddRange(assignments);

            }
            return View(model);
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> UpdateCaseAssignments(AllCasesList model)
        {
            if (model.CaseAssignments.Count > 0)
            {
                var ctx = _govtechHackathonContext;
                var caseIDs = model.CaseAssignments.Where(x => x.AssignedtoGroupID != 0).Select(x => x.CaseID).ToList();
                var assigmentsToRemove = ctx.CaseAssignments.Where(x => caseIDs.Contains(x.FkCaseId));
                ctx.CaseAssignments.RemoveRange(assigmentsToRemove);
                await ctx.SaveChangesAsync();

                var InReviewStatus = await ctx.CaseStatus.FirstAsync(x => x.Name == "In Review");
                var casesToUpdate = await ctx.CaseInformation.Where(x => caseIDs.Contains(x.PkId)).ToListAsync();
                casesToUpdate.ForEach(x => x.FkStatusId = InReviewStatus.PkId);

                var assignedCases = model.CaseAssignments.Where(x => x.AssignedtoGroupID > 0).ToList();
                foreach (var caseEntry in assignedCases)
                {
                    var caseAssignment = new GovtechDBLib.Models.CaseAssignments()
                    {
                        FkGroupId = caseEntry.AssignedtoGroupID,
                        FkCaseId = caseEntry.CaseID,
                        Comment = ""
                    };

                    ctx.CaseAssignments.Add(caseAssignment);

                }
                await ctx.SaveChangesAsync();
            }

            return RedirectToAction("AllCasesList", "Admin");
        }

        public async Task<IActionResult> DeactivateUser(IFormCollection frmData)
        {
            var userList = new UserList();
            int userID = Convert.ToInt32(frmData.First().Value);
            var ctx = _govtechHackathonContext;
            var dbUser = await ctx.User.FirstOrDefaultAsync(x => x.PkId == userID);
            if (dbUser != null)
            {
                dbUser.IsActive = false;
                await ctx.SaveChangesAsync();

            }
            userList.Users.AddRange( await _dbAPI.GetUserListAsync());
            userList.UpdateSuccessful = true;
            return PartialView("/Views/Admin/_UserTable.cshtml", userList);
        }

        public async Task<IActionResult>ActivateUser(IFormCollection frmData)
        {
            var userList = new UserList();
            int userID = Convert.ToInt32(frmData.First().Value);
            var ctx = _govtechHackathonContext;
            var dbUser = await ctx.User.FirstOrDefaultAsync(x => x.PkId == userID);
            if (dbUser != null)
            {
                dbUser.IsActive = true;
                await ctx.SaveChangesAsync();

            }
            userList.Users.AddRange(await _dbAPI.GetUserListAsync());
            userList.UpdateSuccessful = true;
            return PartialView("/Views/Admin/_UserTable.cshtml", userList);
        }

        public async Task<IActionResult> UpdateUser(UserDetails model)
        {
            var userList = new UserList();
            if (ModelState.IsValid)
            {
                var ctx = _govtechHackathonContext;
                var userDB = await ctx.User.FirstOrDefaultAsync(x => x.PkId == model.UserID);
                if (userDB != null)
                {
                    userDB.FirstName = model.FirstName;
                    userDB.Surname = model.Surname;
                    userDB.Email = model.Email;
                    userDB.ContactNo = model.ContactNo;
                    userDB.FkProvinceId = model.fkProvinceID;
                    userDB.FkUserLevelId = model.fkUserLevelID;
                    userDB.CanAssignCases = model.CanAssignCases;
                    userDB.CanEditCases = model.CanEditCases;
                    await ctx.SaveChangesAsync();
                }
            }
            userList.Users.AddRange(await _dbAPI.GetUserListAsync());
            userList.UpdateSuccessful = true;
            return PartialView("/Views/Admin/_UserTable.cshtml", userList);
        }

        public async Task<IActionResult> AddUser(UserDetails model)
        {
            var ctx = _govtechHackathonContext;
            var userList = new UserList();
          

            if (ModelState.IsValid)
            {

                var userLevel = await ctx.UserLevel.FirstAsync(x => x.PkId == model.fkUserLevelID);
                var user = (User)model;
                user.IsActive = true;
                ctx.User.Add(user);
                await ctx.SaveChangesAsync();
                var identityUser = new IdentityUser { UserName = model.Email, Email = model.Email };
                var password = PasswordGenerator.GeneratePassword(_userManager.Options.Password);
                var result = await _userManager.CreateAsync(identityUser, password);
                if (result.Succeeded)
                {
                    userList.UpdateSuccessful = true;
                    await _userManager.AddToRoleAsync(identityUser, userLevel.Name);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    var confirmationLink = Url.Action("ConfirmEmail", "Registration", new { userId = identityUser.Id, token }, Request.Scheme);
                    EmailConfirmation.SendEmailConfirmationLink(_configuration, confirmationLink, model.Email, password);
                }
                else
                    userList.UpdateSuccessful = false;

                userList.Users.AddRange(await _dbAPI.GetUserListAsync());
            }


            return PartialView("/Views/Admin/_UserTable.cshtml", userList);
        }

    }

}
