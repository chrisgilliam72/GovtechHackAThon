using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GovtechDBLib.Models;
using GovtechHackAthon.Helpers;
using GovtechHackAthon.Models;
using GovtechHackAthon.Services;
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
    public class ApplicantController : Controller
    {
        private readonly DBLookupService dBLookupService;
        private readonly GovtechHackathonContext _govtechHackathonContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        private CaseDashboard LoadDashboard(int caseID)
        {
            var model = new CaseDashboard();
            var ctx = _govtechHackathonContext;
            var caseInfo = ctx.CaseInformation.Include("FkSituationOption").
                                                Include("CaseAdjudicatorReferalReason").
                                                Include("FkSituationStatement").
                                                Include("FkCompany").
                                                Include("FkStudentInfo").
                                                Include("FkCompany.FkProvince").
                                                Include("FkStatus").
                                                Include("TeamMember").
                                                Include("TeamMember.FkRace").
                                                Include("TeamMember.FkGender").
                                                Include("TeamMember.FkCountry").
                                                Include("CaseOutcomes").
                                                Include("CaseOutcomes.FkOutcome").
                                                Include("FkBusinessIdea").
                                                Include("FkPurpose").
                                                FirstOrDefault(x => x.PkId == caseID);
            if (caseInfo != null)
            {
                model.OwnerID = caseInfo.FkUserId;
                model.CaseInfo.CaseName = caseInfo.Name;
                model.CaseInfo.SituationStatementID = caseInfo.FkSituationStatementId;
                model.CaseInfo.SituationOptionID = caseInfo.FkSituationOptionId;
                model.CaseInfo.HasCompany = caseInfo.FkSituationStatement.RequiresCompany;
                model.CaseInfo.HasStudentInfo = caseInfo.FkSituationStatement.RequiresStudentInfo;
                model.CaseInfo.Status = caseInfo.FkStatus.Name;
                model.CaseInfo.DateSubmitted = caseInfo.SubmittedDate;
                model.CaseInfo.CaseID = caseInfo.PkId;

                if (caseInfo.CaseOutcomes != null)
                {
                    foreach (var outcome in caseInfo.CaseOutcomes)
                        model.CaseInfo.OutComeIDS.Add(outcome.FkOutcomeId);
                }

                int index = 1;
                foreach (var dbTeamMember in caseInfo.TeamMember)
                {
                    var teamMember = (TeamMemberData)dbTeamMember;
                    teamMember.Index = index++;
                    teamMember.CaseID = dbTeamMember.FkCaseId;
                    model.TeamMembers.Add(teamMember);
                }

                model.Idea.CaseID = caseInfo.PkId;
                if (caseInfo.FkBusinessIdea != null)
                {

                    model.Idea.IdeaID = caseInfo.FkBusinessIdea.PkId;
                    model.Idea.How = caseInfo.FkBusinessIdea.How;
                    model.Idea.What = caseInfo.FkBusinessIdea.WhatProblem;
                    model.Idea.Who = caseInfo.FkBusinessIdea.Who;
                    model.Idea.Impact = caseInfo.FkBusinessIdea.Impact;

                }
                model.Purpose.CaseID = caseInfo.PkId;
                if (caseInfo.FkPurpose != null)
                {
                    model.Purpose.PurposeID = caseInfo.FkPurpose.PkId;
                    model.Purpose.Motivation = caseInfo.FkPurpose.Motivation;
                    model.Purpose.CoreValues = caseInfo.FkPurpose.CoreValues;
                   
                }
                if (caseInfo.FkCompany != null)
                {
                    model.CaseInfo.Company = new CompanyDetails()
                    {
                        Name = caseInfo.FkCompany.Name,
                        Province = caseInfo.FkCompany.FkProvince.Name,
                        ProvinceID = caseInfo.FkCompany.FkProvinceId,
                        City = caseInfo.FkCompany.City
                    };


                }
                if (caseInfo.FkStudentInfo != null)
                {
                    model.CaseInfo.StudentInfo = new RegistrationStudentDetails()
                    {
                        Course = caseInfo.FkStudentInfo.Course,
                        Institution = caseInfo.FkStudentInfo.Instituion
                    };


                }

                if (caseInfo.CaseAdjudicatorReferalReason!=null && caseInfo.CaseAdjudicatorReferalReason.Count() > 0)
                {
                    var deferalReason = caseInfo.CaseAdjudicatorReferalReason.First();
                    model.IsDefered = true;
                    model.DeferalReason = deferalReason.Reason;
                    model.DeferedUntil = deferalReason.ResubmissionDate;
                }
            }
            return model;

        }

        public ApplicantController(DBLookupService dBLookupService, GovtechHackathonContext govtechHackathonContext,
                                   UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.dBLookupService = dBLookupService;
            _govtechHackathonContext = govtechHackathonContext;
            _userManager = userManager;
            _configuration = configuration;
        }


        public IActionResult Home()
        {
            return View();
        }

        public IActionResult ProfileView()
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                return View("Profile", currentUser.RegistrationData);
            }

            return RedirectToAction("login", "Registration");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View("/Registration/ChangePassword");
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfileData(ApplicantData model, String btnSave)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                if (btnSave == "Upload" && model.ImageFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(memoryStream);
                        model.ImageSrc = memoryStream.ToArray();
                        model.ImageType = model.ImageFile.ContentType;
                    }
                }
                else
                {
                    var usrData = await _govtechHackathonContext.Applicant.FirstAsync(x => x.PkId == model.UserID);
                    usrData.FirstName = model.FirstName;
                    usrData.LastName = model.Surname;
                    usrData.Idnumber = model.ID;
                    usrData.FkProvinceId = model.ProvinceID;
                    usrData.Company = model.Company;
                    usrData.Position = model.Position;
                    usrData.ProfileImage = model.ImageSrc;
                    usrData.ProfileImageType = model.ImageType;
                    usrData.City = model.City;
                    usrData.Address = model.Address;
                    usrData.ZipCode = model.ZipCode;
                    usrData.ContactNumber = model.ContactNumber;
                    usrData.LinkedIn = model.LinkedIn;
                    usrData.Twitter = model.Twitter;
                    model.ShowToast = true;
                    if (model.Email != usrData.EmailAddress)
                    {
                        var user = await _userManager.FindByEmailAsync(usrData.EmailAddress);
                        if (user != null)
                        {
                            user.Email = model.Email;
                            user.UserName = model.Email;
                            user.EmailConfirmed = false;
                            await _userManager.UpdateAsync(user);
                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var confirmationLink = Url.Action("ConfirmEmail", "Registration", new { userId = user.Id, token }, Request.Scheme);
                            EmailConfirmation.SendEmailConfirmationLink(_configuration, confirmationLink, model.Email,null);
                            usrData.EmailAddress = model.Email;
                        }

                    }
                    await _govtechHackathonContext.SaveChangesAsync();
                    currentUser.RegistrationData = model;
                    HttpContext.Session.SetObjectAsJson("CurrentUser", currentUser);
                }

                ModelState.Clear();
                return View("Profile", model);
            }

            return RedirectToAction("login", "Registration");

        }

        [HttpGet]
        public async Task<IActionResult> Case(int caseID)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                var model = LoadDashboard(caseID);

                if (model.OwnerID==currentUser.RegistrationData.UserID || model.IsTeamMember(currentUser.RegistrationData.Email))
                {
                    currentUser.Case = model.CaseInfo;
                    HttpContext.Session.SetObjectAsJson("CurrentUser", currentUser);

                    if (model.OwnerID== currentUser.RegistrationData.UserID)
                    {
                        if (currentUser.RegistrationData != null)
                        {
                            var user = await _userManager.FindByEmailAsync(currentUser.RegistrationData.Email);
                            if (user != null)
                                model.CanSubmit = user.EmailConfirmed;
                        }
                    }
                    else
                    {
                        model.ViewOnlyMode = true;
                    }


                    return View(model);
                }

                RedirectToAction("CaseList", "CaseList");
            }

            return RedirectToAction("login", "Registration");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCase([Bind(Prefix = "CaseInfo.CaseID")]int caseID)
        {
            bool hasErrors = false;
            var submittedStatus = await _govtechHackathonContext.CaseStatus.FirstAsync(x => x.Name == "Submitted");
            var caseModel = LoadDashboard(caseID);
            var notificationMngr = new NotificationManager(_govtechHackathonContext,_configuration);

            if (caseModel.TeamMembers.Count<2)
            {
                hasErrors = true;
                caseModel.CollapseTeamMembers = false;
                ModelState.AddModelError("TeamMembersErrors", "Additional team members are needed");
            }

            if (String.IsNullOrEmpty(caseModel.Purpose.Motivation))
            {
                hasErrors = true;
                caseModel.CollapsePurpose = false;
                ModelState.AddModelError("Motivation", "Please complete this section");
            }

            if (String.IsNullOrEmpty(caseModel.Purpose.CoreValues))
            {
                hasErrors = true;
                caseModel.CollapsePurpose = false;
                ModelState.AddModelError("CoreValues", "Please complete this section");
            }

            if (String.IsNullOrEmpty(caseModel.Idea.Impact))
            {
                hasErrors = true;
                caseModel.CollapseIdea = false;
                ModelState.AddModelError("Impact", "Please complete this section");
            }

            if (String.IsNullOrEmpty(caseModel.Idea.How))
            {
                hasErrors = true;
                caseModel.CollapseIdea = false;
                ModelState.AddModelError("How", "Please complete this section");
            }

            if (String.IsNullOrEmpty(caseModel.Idea.Who))
            {
                hasErrors = true;
                caseModel.CollapseIdea = false;
                ModelState.AddModelError("Who", "Please complete this section");
            }

            if (String.IsNullOrEmpty(caseModel.Idea.What))
            {
                hasErrors = true;
                caseModel.CollapseIdea = false;
                ModelState.AddModelError("What", "Please complete this section");
            }

            if (hasErrors)
            {
                caseModel.CanSubmit = true;
                return View("Case", caseModel);
            }
            else
            {
                var caseInfo = await _govtechHackathonContext.CaseInformation.FirstAsync(x => x.PkId == caseID);
                caseInfo.FkStatusId = submittedStatus.PkId;
                _govtechHackathonContext.Attach(caseInfo);
                _govtechHackathonContext.Entry(caseInfo).State = EntityState.Modified;
                
                if (caseModel.IsDefered)
                {
                    var deferalReason = await _govtechHackathonContext.CaseAdjudicatorReferalReason.Where(x => x.FkCaseId == caseID).ToListAsync();
                    _govtechHackathonContext.CaseAdjudicatorReferalReason.RemoveRange(deferalReason);
                }

               
                await _govtechHackathonContext.SaveChangesAsync();

                var body = new StringBuilder();
                body.AppendLine(@"<html><body>Your proposal : "+ caseInfo.Name+" ID= "+caseInfo.PkId+ " has been submitted </body></html>");

                var notificationMgr = new NotificationManager(_govtechHackathonContext, _configuration);
                await notificationMgr.NotifyTeam(caseInfo.PkId, "Proposal Submitted", body.ToString());

            }

          

            return RedirectToAction("CaseList", "CaseList");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCaseInfoPartial(Case model)
        {
            ModelState.Clear();
            var ctx = _govtechHackathonContext;

            var caseInfo = await ctx.CaseInformation.Include("CaseOutcomes").Include("FkCompany").FirstAsync(x => x.PkId == model.CaseID);
            var situations = await ctx.SituationStatements.ToListAsync();
            if (caseInfo != null)
            {
                var selectedOutComes = model.OutComeSelections.Where(x => x.Value);

                caseInfo.Name = model.CaseName;
                caseInfo.FkSituationOptionId = model.SituationOptionID;
                caseInfo.FkSituationStatementId = model.SituationStatementID;
                var situation = situations.First(x => x.PkId == model.SituationStatementID);
                if (caseInfo.FkCompany != null)
                {
                    ctx.Company.Remove(caseInfo.FkCompany);
                    caseInfo.FkCompany = null;
                }

                if (caseInfo.FkStudentInfo != null)
                {
                    ctx.StudentDetails.Remove(caseInfo.FkStudentInfo);
                    caseInfo.FkStudentInfo = null;
                }

                if (situation.RequiresStudentInfo && model.StudentInfo.Course != null && model.StudentInfo.Institution != null)
                {
                    var studentInfo = new StudentDetails()
                    {
                        Course = model.StudentInfo.Course,
                        Instituion = model.StudentInfo.Institution
                    };
                    ctx.StudentDetails.Add(studentInfo);
                    caseInfo.FkStudentInfo = studentInfo;
                    model.HasStudentInfo = true;
                }

                if (situation.RequiresCompany && model.Company.Name != null && model.Company.ProvinceID > 0 && !String.IsNullOrEmpty(model.Company.City))
                {
                    var company = new Company()
                    {
                        Name = model.Company.Name,
                        City = model.Company.City,
                        FkProvinceId = model.Company.ProvinceID
                    };

                    ctx.Company.Add(company);
                    caseInfo.FkCompany = company;
                    model.HasCompany = true;
                }




                ctx.CaseOutcomes.RemoveRange(caseInfo.CaseOutcomes);

                foreach (var selOutcome in selectedOutComes)
                {
                    var dbCaseOutcome = new CaseOutcomes();
                    dbCaseOutcome.FkCaseId = caseInfo.PkId;
                    dbCaseOutcome.FkOutcomeId = selOutcome.Key;
                    ctx.CaseOutcomes.Add(dbCaseOutcome);
                }

                await ctx.SaveChangesAsync();

                foreach (var outcome in caseInfo.CaseOutcomes)
                    model.OutComeIDS.Add(outcome.FkOutcomeId);

            }

            model.ShowToast = true;
            return PartialView("/Views/Shared/_CaseInfoDetails.cshtml", model);

        }



        [HttpPost]
        public async Task<IActionResult> UpdateBusinessIdeaPartial(RegistrationBusinessIdea model)
        {
            ModelState.Clear();
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;
                var caseInfo = await ctx.CaseInformation.Include("FkBusinessIdea").FirstOrDefaultAsync(x => x.PkId == model.CaseID);
                if (caseInfo != null)
                {

                    if (caseInfo.FkBusinessIdea == null)
                    {
                        var businessIdea = new BusinessIdea()
                        {
                            How = model.How,
                            WhatProblem = model.What,
                            Impact = model.Impact,
                            Who = model.Who
                        };
                        caseInfo.FkBusinessIdea = businessIdea;
                        ctx.BusinessIdea.Add(businessIdea);
                        await ctx.SaveChangesAsync();
                        model.IdeaID = businessIdea.PkId;

                    }
                    else
                    {
                        caseInfo.FkBusinessIdea.How = model.How;
                        caseInfo.FkBusinessIdea.WhatProblem = model.What;
                        caseInfo.FkBusinessIdea.Impact = model.Impact;
                        caseInfo.FkBusinessIdea.Who = model.Who;
                        await ctx.SaveChangesAsync();
                    }


                }
            }
            model.ShowToast = true;
            return PartialView("/Views/Shared/_BusinessIdea.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePurposePartial(RegistrationPurpose model)
        {
            ModelState.Clear();
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;
                var caseInfo = await ctx.CaseInformation.Include("FkPurpose").FirstOrDefaultAsync(x => x.PkId == model.CaseID);
                if (caseInfo != null)
                {

                    if (caseInfo.FkPurpose == null)
                    {
                        var purpose = new Purpose()
                        {
                            CoreValues = model.CoreValues,
                            Motivation = model.Motivation,

                        };
                        caseInfo.FkPurpose = purpose;
                        ctx.Purpose.Add(purpose);
                        await ctx.SaveChangesAsync();
                        model.PurposeID = purpose.PkId;
                    }

                    else
                    {
                        caseInfo.FkPurpose.CoreValues = model.CoreValues;
                        caseInfo.FkPurpose.Motivation = model.Motivation;
                        await ctx.SaveChangesAsync();
                    }

                }
            }
            model.ShowToast = true;
            return PartialView("/Views/Shared/_purpose.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> AskQuestion(CaseQuestion model)
        {
            var ctx = _govtechHackathonContext;
            var configSettings= ctx.ConfigSettings.FirstOrDefault();
            var caseInfo = await ctx.CaseInformation.FirstOrDefaultAsync(x => x.PkId == model.CaseID);
            var applicantInfo = await ctx.Applicant.FirstOrDefaultAsync(x => x.PkId == model.ApplicantID);
            var mailHelper = new MailHelper(_configuration);
            var content = "Applicant: " + applicantInfo.FirstName + " " + applicantInfo.LastName + "<br>";
            content += "Case : " + caseInfo.PkId +" "+ caseInfo.Name + "<br>";
            content += "Question:" + "<br>";
            content += model.Question;

            mailHelper.SendAsync(applicantInfo.EmailAddress, new List<String>() { configSettings.AdminEmail }, "Automated email from Govtech Hackathon website", content,true);

            model.Successful = true;
          
            return PartialView("/Views/Shared/_AskQuestion.cshtml",model );
        }

            [HttpPost]
        public async Task<IActionResult> UpdateTeamMember(TeamMemberData model)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;
                var teamMember = new TeamMember()
                {
                    PkId = model.TeamMemberID,
                    FkCaseId = model.CaseID,
                    Identity = model.ID,
                    FirstName = model.FirstName,
                    LastName = model.Surname,
                    EmailAddress = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FkRaceId = model.RaceID,
                    FkGenderId = model.GenderID,
                    FkCountryId = model.CountryID,
                    IsStudent = model.IsStudent,
                    DateOfBirth=model.DateOfBirth
                };

                ctx.TeamMember.Attach(teamMember);
                ctx.Entry(teamMember).State = EntityState.Modified;
                await ctx.SaveChangesAsync();

                ModelState.Clear();

                //var caseModel = CaseDashboard.LoadDashboard(model.CaseID);
                //caseModel.CollapseTeamMembers = false;
                //return View("Case", caseModel);
                var caseModel = LoadDashboard(model.CaseID);
                return PartialView("/Views/Shared/_TeamMembers.cshtml", caseModel);
            }
            else
                return RedirectToAction("Login", "Registration");
        }




        [HttpPost]
        public async Task<IActionResult> DeleteTeamMember([Bind(Prefix = "teamMember.TeamMemberID")]int teamMemberID,
                                                          [Bind(Prefix = "teamMember.CaseID")]int caseID)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;
                var teamMember = new TeamMember() { PkId = teamMemberID };
                ctx.TeamMember.Attach(teamMember);
                ctx.TeamMember.Remove(teamMember);
                await ctx.SaveChangesAsync();


                ModelState.Clear();
                var caseModel = LoadDashboard(caseID);
                return PartialView("/Views/Shared/_TeamMembers.cshtml", caseModel);
            }
            else
                return RedirectToAction("Login", "Registration");

        }

        [HttpPost]
        public IActionResult AddTeamMember(TeamMemberData model)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            int pkIDSouthAfrica = dBLookupService.GetCountries().FirstOrDefault(x => x.Name == "South Africa").PkId;
            if (model.CountryID != pkIDSouthAfrica)
            {
                ModelState.Remove("City");
                ModelState.Remove("ProvinceID");
            }
            if (ModelState.IsValid)
            {
                if (currentUser != null)
                {

                    var ctx = _govtechHackathonContext;
                    var dbTeamMember = (TeamMember)model;
                    ctx.TeamMember.Add(dbTeamMember);
                    ctx.SaveChanges();

                    ModelState.Clear();
                    var caseModel = LoadDashboard(model.CaseID);
                    return PartialView("/Views/Shared/_TeamMembers.cshtml", caseModel);

                    //var caseModel = CaseDashboard.LoadDashboard(model.CaseID);
                    //caseModel.CollapseTeamMembers = false;
                    //return View("Case", caseModel);
                }

                else
                    return RedirectToAction("Login", "Registration");
            }

            return PartialView("/Views/Shared/_TeamMembers.cshtml", model);
        }


    }
}