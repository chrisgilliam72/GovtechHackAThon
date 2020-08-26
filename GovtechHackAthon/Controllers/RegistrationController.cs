using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GovtechDBLib.Models;
using GovtechHackAthon.Helpers;
using GovtechHackAthon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
//using GovtechHackAthon.Models.EFModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GovtechHackAthon.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly GovtechHackathonContext _govtechHackathonContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManger;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public RegistrationController(GovtechHackathonContext govtechHackathonContext,
                                      UserManager<IdentityUser> userManager,
                                      RoleManager<IdentityRole> roleManger,
                                      SignInManager<IdentityUser> signInManager,
                                      IConfiguration configuration)
        {
            _govtechHackathonContext = govtechHackathonContext;
            this.userManager = userManager;
            this.roleManger = roleManger;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyUsername(string username)
        {
            return Json($"{username} could not be found.");

        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPassword(string password, string username)
        {
            return Json($"Invalid password");

        }
 

        public async Task<ActionResult> IDUniqueOverTeam(String ID, int TeamMemberID)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (!String.IsNullOrEmpty(ID))
            {
                var ctx = _govtechHackathonContext;
                using (ctx)
                {
                    var teamMembers = await ctx.TeamMember.Where(x => x.FkCaseId == currentUser.Case.CaseID && x.Identity == ID && x.PkId != TeamMemberID).ToListAsync();
                    if (teamMembers.Count>0)
                        return Json($"ID must be unique for all team members");
                }
            }
            return Json(true);
        }

        public async Task<IActionResult> TeamEmailCanBeShared([Bind(Prefix = "Email")]string TeamEmail, bool IsStudent,int TeamMemberID)
        {
           var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (!String.IsNullOrEmpty(TeamEmail))
            {
                var ctx = _govtechHackathonContext;
                using (ctx)
                {
                    var teamMembers = await ctx.TeamMember.Where(x => x.FkCaseId == currentUser.Case.CaseID && x.EmailAddress == TeamEmail && x.PkId!= TeamMemberID).ToListAsync();
                    if (teamMembers.Count == 0)
                        return Json(true);
                    foreach (var member in teamMembers)
                    {
                        if (member.EmailAddress==TeamEmail && (!member.IsStudent || !IsStudent))
                            return Json($"Only students may share an email address");
                    }
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
        public IActionResult VerifyIDDoesntExist([Bind(Prefix = "ID")]string IDNumber)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (IDNumber != null)
            {
                var ctx = _govtechHackathonContext;
                using (ctx)
                {
                    var usrDetails = ctx.Applicant.FirstOrDefault(x => x.Idnumber == IDNumber);
                    if (usrDetails != null && currentUser == null)
                        return Json($"ID number already registered");
                    if ((usrDetails != null && currentUser != null && usrDetails.PkId != currentUser.RegistrationData.UserID))
                        return Json($"ID number already registered");
                    return Json(true);
                }
            }

            return Json(true);
        }

        //Verification for Team Member ID
        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyIDDoesntExist1([Bind(Prefix = "ID")]string IDNumber)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (IDNumber != null)
            {
                var ctx = _govtechHackathonContext;
                using (ctx)
                {
                    var usrDetails = await ctx.TeamMember.FirstOrDefaultAsync(x => x.Identity == IDNumber);
                    if (usrDetails != null && currentUser == null)
                        return Json($"ID number already registered");
                    if ((usrDetails != null && currentUser != null && usrDetails.PkId != currentUser.RegistrationData.UserID))
                        return Json($"ID number already registered");
                    return Json(true);
                }
            }

            return Json(true);
        }

        [AcceptVerbs("GET")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmailDoesntExist([Bind(Prefix = "Email")]string emailAddress, [Bind(Prefix = "UserID")] int userID)
        {


            if (emailAddress != null)
            {
                var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
                if (currentUser != null && emailAddress == currentUser.RegistrationData.Email)
                    return Json(true);

                var usrDetails = await userManager.FindByEmailAsync(emailAddress);
                if (usrDetails != null)
                    return Json($"Email address already registered");


                return Json(true);

            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyCity(String TeamMemberCity, int CountryID)
        {
            if (TeamMemberCity != null)
            {

                var ctx = _govtechHackathonContext;
                var country = await ctx.Countries.FirstOrDefaultAsync(x => x.PkId == CountryID);
                if (country != null && country.Name == "South Africa" && TeamMemberCity == "")
                    return Json($"city must be entered");

            }

            return Json(true);
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
            //return Redirect("/Dashboard/Home");
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            HttpContext.Session.SetObjectAsJson("CurrentUser", null);
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.EmailAddress);
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var confirmationLink = Url.Action("ResetPassword", "Registration", new { email = model.EmailAddress, token }, Request.Scheme);
                    var confirmationView = View("Confirm");
                    confirmationView.ViewData["ResultMessage"] = "A password reset link has been sent to this email address.";
                    var body = new StringBuilder();
                    body.AppendLine(@"<html><body>Please reset your SITA Hackathon password by clicking on the link below:<br/>");
                    body.AppendLine("<a href = \"" + confirmationLink + "\">Reset Password</a></body></html>");
                    var mailHelper = new MailHelper(configuration);
                    mailHelper.SendAsync(configuration["Gmail:Username"], new List<String>() { model.EmailAddress }, "SITA Hackathon password reset", body.ToString(), false) ;
                    return confirmationView;
                }
                else
                {
                    ModelState.AddModelError("", "EMail Address not found");
                }
            }

            return View("ForgotPassword");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {

                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {

                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        var confirmView = View("Confirm");
                        confirmView.ViewData["ResultMessage"] = "Thank you.Your password has been successfully changed.";
                        return confirmView;
                    }

                    else
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError("", error.Description);
                    }
                }
                else
                    ModelState.AddModelError("", "Unable to reset password. User not found");

            }
            return View("ChangePassword", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordPartial(ChangePassword model)
        {
            var currentUserID = userManager.GetUserId(HttpContext.User);
            var user = await userManager.FindByIdAsync(currentUserID);
            if (user != null)
                {
                if (model.ConfirmPassword != model.Password)
                {
                    model.HasErrors = true;
                    ModelState.AddModelError("", "Password and confirmation fields do not match");
                    return PartialView("/Views/Shared/_ChangePassword.cshtml", model);
                }
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                if (result.Succeeded)
                {
                    model = new ChangePassword();
                    ModelState.Clear();
                    model.Sucessful = true;
                    return PartialView("/Views/Shared/_ChangePassword.cshtml", model);
                }
                else
                {
                    model.HasErrors = true;
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);

                }
                return PartialView("/Views/Shared/_ChangePassword.cshtml", model);
            }
            else
            {
                model.HasErrors = true;
                ModelState.AddModelError("", "Unable to reset password. User not found");
                return PartialView("/Views/Shared/_ChangePassword.cshtml", model);
            }

        }

        [HttpGet]
        public async Task<ActionResult> ChangePassword()
        {
            var currentUserID = userManager.GetUserId(HttpContext.User);
            if (!String.IsNullOrEmpty(currentUserID))
            {
                var user = await userManager.FindByIdAsync(currentUserID);
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var model = new ResetPassword();

                    model.Email = user.Email;
                    model.Token = token;
                    string referer = Request.Headers["Referer"].ToString();
                    return View("ChangePassword",model);
                }
            }


            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }

            return View("ChangePassword");

            //var resetPassword = new ResetPassword();
            //resetPassword.Email = email;
            //resetPassword.Token = token;
            //return View("ChangePassword",resetPassword);
        }

   

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userID, String token)
        {
            if (userID == null || token == null)
                return RedirectToAction("index", "home");


            var user = await userManager.FindByIdAsync(userID);
            if (user != null)
            {
                var confirmView = View("Confirm");
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result == IdentityResult.Success)
                    confirmView.ViewData["ResultMessage"] = "Thank you.Your email address has been verified. Please login to continue.";
                else
                {
                    confirmView.ViewData["ResultMessage"] = "Email verification failed . Reason :" + Environment.NewLine;
                    foreach (var error in result.Errors)
                    {
                        confirmView.ViewData["ResultMessage"] += error.Description;
                        confirmView.ViewData["ResultMessage"] += Environment.NewLine;
                    }
                }

                return confirmView;
            }
            else
                return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLoginDetails model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName,
            model.Password, false, lockoutOnFailure: true);

            if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
            {

                var ctx = _govtechHackathonContext;
                var user = new ApplicantInfo();
                var identityUser = await userManager.FindByEmailAsync(model.UserName);
                if (await userManager.IsInRoleAsync(identityUser, "Applicant"))
                {     
                    var usrDetails = await ctx.Applicant.Include("CaseInformation")
                                                                .FirstOrDefaultAsync(x => x.EmailAddress == model.UserName);
                    if (usrDetails == null)
                    {
                        ModelState.AddModelError("Error", "Invalid username");
                        return View();
                    }
                    else
                    {

                        user.UserRole = "Applicant";
                        var userModel = (ApplicantData)usrDetails;
                        user.RegistrationData = userModel;

                        HttpContext.Session.SetObjectAsJson("CurrentUser", user);

                        if (!usrDetails.AgreedToTerms)
                            return RedirectToAction("TermsConditions");

                        if (usrDetails.ProfileImage==null)
                            return RedirectToAction("ProfileView", "applicant");
                        return RedirectToAction("CaseList", "CaseList");
                    }
                }
                else if(await userManager.IsInRoleAsync(identityUser, "Adjudicator"))
                {
                    user.UserRole = "Adjudicator";
                    var userData = ctx.User.Include("UsersInGroup"). First(x => x.Email == identityUser.Email);
                    var userDetails = (UserDetails)userData;
                    HttpContext.Session.SetObjectAsJson("CurrentUser", userDetails);
                    return RedirectToAction("Dashboard", "Adjudicator");
                }
                else if (await userManager.IsInRoleAsync(identityUser, "Admin"))
                {
                    user.UserRole = "Admin";
                    HttpContext.Session.SetObjectAsJson("CurrentUser", user);
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (await userManager.IsInRoleAsync(identityUser, "Auditor"))
                {
                    user.UserRole = "Auditor";
                    var userData = ctx.User.Include("UsersInGroup").First(x => x.Email == identityUser.Email);
                    var userDetails = (UserDetails)userData;
                    HttpContext.Session.SetObjectAsJson("CurrentUser", userDetails);
                    return RedirectToAction("Dashboard", "Auditor");
                }
                else if (await userManager.IsInRoleAsync(identityUser,"Investor"))
                {
                    var dbInvestor = ctx.Investor.Include("FkIndustry").Include("FkProvince").First(x => x.Email == identityUser.Email);
                    if (dbInvestor.Approved)
                    {
                        var investor = (InvestorDetails)dbInvestor;
                        HttpContext.Session.SetObjectAsJson("CurrentInvestor", investor);
                        return RedirectToAction("MyProposals", "Investor");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "account not approved");
                        return View();
                    }

                }
                ModelState.AddModelError("Error", "Invalid Role");
                return View();
            }
            else if (result == Microsoft.AspNetCore.Identity.SignInResult.NotAllowed)
            {
                ModelState.AddModelError("Error", "Email not confirmed");
                return View();
            }
            else if (result == Microsoft.AspNetCore.Identity.SignInResult.LockedOut)
            {
                ModelState.AddModelError("Error", "Account locked");
                return View();
            }
            else
            {
                ModelState.AddModelError("Error", "Invalid username or password");
                return View();
            }

            //}

        }

        public ActionResult UserRegistrationData()
        {
            return View(new ApplicantData());
        }



        [HttpPost]
        public async Task<ActionResult> RegistrationDataCompleted(ApplicantData model)
        {
            if (ModelState.IsValid)
            {
                model.AuthErrors.Clear();
                var identityUser = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(identityUser, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(identityUser, "Applicant");
                    var ctx = _govtechHackathonContext;
                    var user = new ApplicantInfo();
                    user.RegistrationData = model;
                    user.Teams.AddTeamMembers(2);
                    var dbObject = (Applicant)model;
                    ctx.Applicant.Add(dbObject);
                    await ctx.SaveChangesAsync();
                    model.UserID = dbObject.PkId;

                    var token = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    var confirmationLink = Url.Action("ConfirmEmail", "Registration", new { userId = identityUser.Id, token }, Request.Scheme);
                    await signInManager.SignInAsync(identityUser, isPersistent: false);
                    HttpContext.Session.SetString("Confirmlink", confirmationLink);
                    HttpContext.Session.SetObjectAsJson("CurrentUser", user);

                    var body = new StringBuilder();
                    body.AppendLine(@"Please verify your email by clicking on the link below:<br/>");
                    body.AppendLine("<a href = \"" + confirmationLink + "\">Confirm</a></body></html>");
                    //var body = "<html><body>Please verify your email by clicking on the link below:<br/>";
                    //body += @"<a href = """ + confirmationLink+ @"""></a></body></html>";

                    var mailHelper = new MailHelper(configuration);
                    mailHelper.SendAsync(configuration["Gmail:Username"], new List<string>() { identityUser.Email }, "SITA Hackathon Email Confirmation", body.ToString(),false);


                    return RedirectToAction("TermsConditions");
                }


                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                    model.AuthErrors.Add(error.Description);
                }


            }

            return View("UserRegistrationData", model);
        }

        [HttpPost]
        public async Task<ActionResult> TermsAndConditionsAccepted(TermsConditions model)
        {

            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                if (ModelState.IsValid)
                {
                    var ctx = _govtechHackathonContext;
                    var applicantDBObj = ctx.Applicant.FirstOrDefault(x => x.PkId == currentUser.RegistrationData.UserID);
                    applicantDBObj.AgreedToTerms = true;
                    await ctx.SaveChangesAsync();

                    var teamsLst = await ctx.TeamMember.Where(x => x.EmailAddress == applicantDBObj.EmailAddress).ToListAsync();
                    if (teamsLst.Count==0)
                    {
                        HttpContext.Session.SetObjectAsJson("CurrentUser", currentUser);
                        return RedirectToAction("CaseInfo");
                    }
                    else
                    {
                        return RedirectToAction("CaseList", "CaseList");
                    }
                }

            }

            return RedirectToAction("UserRegistrationData", "Registration");
        }

        [Authorize]
        public ActionResult CaseInfo()
        {
            var caseModel = new Case();

            return View(caseModel);
        }

        [HttpPost]
        public async Task<ActionResult> CaseDetailsSubmitted(Case model)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {

                if (!String.IsNullOrEmpty(model.CaseName))
                {

                    var selectedOutComes = model.OutComeSelections.Where(x => x.Value);

                    var ctx = _govtechHackathonContext;
                    var savedSatus = await ctx.CaseStatus.FirstOrDefaultAsync(x => x.Name == "Saved");
                    var canceledStatus= await ctx.CaseStatus.FirstOrDefaultAsync(x => x.Name == "Canceled");
                    var situationStatements = await ctx.SituationStatements.ToListAsync();


                    var caseInfo = new CaseInformation();
                    caseInfo.Name = model.CaseName;

                    if (model.SituationStatementID == 0 || model.SituationOptionID == 0)
                    {
                        if (model.SituationStatementID == 0)
                            model.NeedsSituationStatement = true;
                        ModelState.AddModelError("SituationStatementID", "Situation statement is required");

                        if (model.SituationOptionID == 0)
                        {
                            model.NeedsSituationOption = true;
                            ModelState.AddModelError("SituationOptionID", "Situation option is required");
                        }
                        return View("CaseInfo", model);
                    }

                    var situationStatement = situationStatements.FirstOrDefault(x => x.PkId == model.SituationStatementID);
                    if (situationStatement != null && situationStatement.RequiresStudentInfo)
                    {
                        if (String.IsNullOrEmpty(model.StudentInfo.Course) || String.IsNullOrEmpty(model.StudentInfo.Institution))
                        {
                            model.NeedsSituationStatement = true;
                            model.HasStudentInfo = true;
                            return View("CaseInfo", model);
                        }
                        else
                        {
                            var studentDetails = new StudentDetails()
                            {
                                Course = model.StudentInfo.Course,
                                Instituion = model.StudentInfo.Institution
                            };
                            ctx.StudentDetails.Add(studentDetails);
                            caseInfo.FkStudentInfo = studentDetails;
                        }
                    }

                    caseInfo.FkSituationOptionId = model.SituationOptionID;
                    caseInfo.FkSituationStatementId = model.SituationStatementID;
                    caseInfo.FkUserId = currentUser.RegistrationData.UserID;
                    caseInfo.FkStatusId = savedSatus.PkId;
                    caseInfo.SubmittedDate = DateTime.Today;
                    caseInfo.ForGovernment = model.WorksForGovernment;
                    caseInfo.Year = DateTime.Today.Year;
                    ctx.CaseInformation.Add(caseInfo);

                    if (model.Company.Name != null)
                        if (model.Company.ProvinceID > 0 && !String.IsNullOrEmpty(model.Company.City))
                        {
                            var company = new Company();
                            company.Name = model.Company.Name;
                            company.City = model.Company.City;
                            company.FkProvinceId = model.Company.ProvinceID;
                            ctx.Company.Add(company);
                            caseInfo.FkCompany = company;
                        }
                        else
                        {
                            model.NeedsSituationStatement = true;
                            return View();
                        }


                    await ctx.SaveChangesAsync();
                
                    foreach (var selOutcome in selectedOutComes)
                    {
                        var dbCaseOutcome = new CaseOutcomes();
                        dbCaseOutcome.FkCaseId = caseInfo.PkId;
                        dbCaseOutcome.FkOutcomeId = selOutcome.Key;
                        ctx.CaseOutcomes.Add(dbCaseOutcome);
                    }


                    if (currentUser.Case!=null)
                    {
                        var caseID = currentUser.Case.CaseID;
                        var prevCaseInfo = await ctx.CaseInformation.FirstOrDefaultAsync(x => x.PkId == caseID);
                        prevCaseInfo.FkStatusId = canceledStatus.PkId;
                    }

                    await ctx.SaveChangesAsync();


                    currentUser.Case = model;
                    currentUser.Case.CaseID = caseInfo.PkId;
                    HttpContext.Session.SetObjectAsJson("CurrentUser", currentUser);


                    return RedirectToAction("Teams");
                }
                else
                    return View();
            }

            return RedirectToAction("UserRegistrationData", "Registration");
        }
        [Authorize]
        public ActionResult Purpose()
        {
            var model = new RegistrationPurpose();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> PurposeSubmitted(RegistrationPurpose model)
        {

            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                if (ModelState.IsValid)
                {
                    var ctx = _govtechHackathonContext;
                    var caseInfo = await ctx.CaseInformation.Include("FkPurpose").FirstOrDefaultAsync(x => x.PkId == currentUser.Case.CaseID);
                    if (caseInfo.FkPurpose==null)
                    {
                        var purpose = new Purpose();
                        purpose.CoreValues = model.CoreValues;
                        purpose.Motivation = model.Motivation;
                        caseInfo.FkPurpose = purpose;
                        ctx.Purpose.Add(purpose);
                    }
                    else
                    {
                        caseInfo.FkPurpose.CoreValues= model.CoreValues;
                        caseInfo.FkPurpose.Motivation = model.Motivation;
                    }

                    await ctx.SaveChangesAsync();

                }
                else
                {
                    model.NeedsMotivation = String.IsNullOrEmpty(model.Motivation);
                    model.NeedsCoreValues = String.IsNullOrEmpty(model.CoreValues);
                    return View("Purpose", model);
                }
            }
            return RedirectToAction("Businessidea");

        }

        [Authorize]
        public ActionResult BusinessIdea()
        {
            var model = new RegistrationBusinessIdea();
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> BusinessIdeaSubmitted(RegistrationBusinessIdea model)
        {

            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                if (!String.IsNullOrEmpty(model.What) && !String.IsNullOrEmpty(model.How) &&
                    !String.IsNullOrEmpty(model.Who) && !String.IsNullOrEmpty(model.Impact))
                {
                    var caseID = currentUser.Case.CaseID;

                    var ctx = _govtechHackathonContext;
                    var caseInfo = await ctx.CaseInformation.Include("FkBusinessIdea").FirstOrDefaultAsync(x => x.PkId == caseID);
                    if (caseInfo!=null)
                    {
                        if (caseInfo.FkBusinessIdea==null)
                        {
                            var businessIdea = new BusinessIdea();
                            businessIdea.How = model.How;
                            businessIdea.Who = model.Who;
                            businessIdea.WhatProblem = model.What;
                            businessIdea.Impact = model.Impact;
                            caseInfo.FkBusinessIdea= businessIdea;
                            ctx.BusinessIdea.Add(businessIdea);
                        }
                        else
                        {
                            caseInfo.FkBusinessIdea.How = model.How;
                            caseInfo.FkBusinessIdea.Who = model.Who;
                            caseInfo.FkBusinessIdea.WhatProblem = model.What;
                            caseInfo.FkBusinessIdea.Impact = model.Impact;
                        }
                    }
    
                    await ctx.SaveChangesAsync();
                    return RedirectToAction("Case", "Applicant", new { CaseID = currentUser.Case.CaseID });
                }
                else
                {
                    model.NeedsWhat = String.IsNullOrEmpty(model.What);
                    model.NeedsHow = String.IsNullOrEmpty(model.How);
                    model.NeedsWho = String.IsNullOrEmpty(model.Who);
                    model.NeedsImpact = String.IsNullOrEmpty(model.Impact);
                    return View("BusinessIdea", model);
                }

            }
            return RedirectToAction("UserRegistrationData", "Registration");

        }

        [Authorize]
        public async Task<ActionResult> Teams()
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser !=null)
            {
                var ctx = _govtechHackathonContext;
                var dbTeamMembers = await ctx.TeamMember.Where(x => x.FkCaseId == currentUser.Case.CaseID).ToListAsync();
                if (dbTeamMembers != null && dbTeamMembers.Count > 0)
                {
                    ctx.TeamMember.RemoveRange(dbTeamMembers);
                    await ctx.SaveChangesAsync();
                }

                var model = currentUser.Teams;
                return View(model);
            }
            return RedirectToAction("UserRegistrationData", "Registration");
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult> TeamsSubmitted(RegistrationTeam model, String btnUpdate)
        {
            bool Errors = false;
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");

            if (currentUser != null && currentUser.Case != null)
            {
                if (btnUpdate == "Delete")
                {
                    if (currentUser != null && currentUser.Teams.MemberCount > 2)
                    {
                        ModelState.Clear();
                        model.RemoveTeamMember(model.IndexToDelete);
                        currentUser.Teams = model;
                        HttpContext.Session.SetObjectAsJson("CurrentUser", currentUser);
                    }

                    return RedirectToAction("Teams");
                }

                if (btnUpdate == "Add Team Member")
                {
                    ModelState.Clear();
                    if (currentUser.Teams.MemberCount < 4)
                    {
                        model.AddTeamMember();
                        currentUser.Teams = model;
                        HttpContext.Session.SetObjectAsJson("CurrentUser", currentUser);
                        return RedirectToAction("Teams");
                    }
                }
                else
                {
                    var ctx = _govtechHackathonContext;

                    var SAID = await ctx.Countries.FirstOrDefaultAsync(x => x.Name == "South Africa");
                    var dbTeamMembers = await ctx.TeamMember.Where(x => x.FkCaseId == currentUser.Case.CaseID).ToListAsync();
                    if (dbTeamMembers != null && dbTeamMembers.Count > 0)
                        ctx.TeamMember.RemoveRange(dbTeamMembers);

                    foreach (var teamMember in model.Members)
                    {
                        if (!String.IsNullOrEmpty(teamMember.ID))
                        {
                            if (currentUser.RegistrationData.ID == teamMember.ID)
                            {
                                var errorKey = "Members[" + teamMember.Index + "].ID";
                                ModelState.AddModelError(errorKey, "ID must be unique");
                                Errors = true;
                            }

                            if (!model.IDIsUnique(teamMember.ID, teamMember.Index))
                            {
                                var errorKey = "Members[" + teamMember.Index + "].ID";
                                ModelState.AddModelError(errorKey, "ID must be unique");
                                Errors = true;
                            }
                        }

                        if (!teamMember.IsStudent)
                        {
                            if (teamMember.Email == currentUser.RegistrationData.Email)
                            {
                                var errorKey = "Members[" + teamMember.Index + "].Email";
                                ModelState.AddModelError(errorKey, "Team email address may not be shared");
                                Errors = true;
                            }

                            if (model.EmailShared(teamMember.Email, teamMember.Index))
                            {
                                var errorKey = "Members[" + teamMember.Index + "].Email";
                                ModelState.AddModelError(errorKey, "Team email address may not be shared");
                                Errors = true;
                            }

                        }
                        else
                        {
                            if (!model.CanShareEmail(teamMember.Email, teamMember.Index))
                            {
                                var errorKey = "Members[" + teamMember.Index + "].Email";
                                ModelState.AddModelError(errorKey, "Team email address may not be shared");
                                Errors = true;
                            }

                        }
                        if (teamMember.CountryID == SAID.PkId && String.IsNullOrEmpty(teamMember.City))
                        {
                            var errorKey = "Members[" + teamMember.Index + "].City";
                            ModelState.AddModelError(errorKey, "City is required");
                            Errors = true;
                        }

                        var dbTeamMember = (TeamMember)teamMember;
                        dbTeamMember.FkCaseId = currentUser.Case.CaseID;
                        ctx.TeamMember.Add(dbTeamMember);


                    }

                    if (Errors)
                        return View("Teams", model);
                    await ctx.SaveChangesAsync();

                    var notificationManager = new NotificationManager(_govtechHackathonContext, configuration);
                    await notificationManager.NotifyNewTeammembers(currentUser.Case.CaseID);
                    return RedirectToAction("Purpose");
                }

            }

            return RedirectToAction("UserRegistrationData", "Registration");

        }
        [Authorize]
        public ActionResult TermsConditions()
        {
            return View();
        }

    }
}