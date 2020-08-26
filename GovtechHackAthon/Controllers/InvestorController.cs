using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GovtechDBLib.Models;
using GovtechHackAthon.Helpers;
using GovtechHackAthon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GovtechHackAthon.Controllers
{
    [Authorize]
    public class InvestorController : Controller
    {
        private readonly GovtechHackathonContext _govtechHackathonContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public InvestorController(GovtechHackathonContext govtechHackathonContext,
                                    UserManager<IdentityUser> userManager,
                                    IConfiguration configuration)
        {
            _govtechHackathonContext = govtechHackathonContext;
            _userManager = userManager;
            _configuration = configuration;
        }

        public IActionResult InvestorProfile()
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<InvestorDetails>("CurrentInvestor");
            if (currentUser != null)
            {
                currentUser.IsNew = false;
                return View("InvestorDetails", currentUser);
            }
            else
                return RedirectToAction("index", "home");
        }
        [AllowAnonymous]
        public IActionResult InvestorDetails()
        {
            var investorDetails = new InvestorDetails();
            return View(investorDetails);
        }
        public ActionResult TermsConditions()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NextInterestedCase(int caseID)
        {
            var ctx = _govtechHackathonContext;

            var dbCaseInfos = await ctx.CaseAdminApproved.Include("FkCase").Where(x => x.Approved).OrderBy(x => x.FkCaseId).ToListAsync();
            var nextIDS = dbCaseInfos.OrderBy(x => x.FkCaseId).Select(x => x.FkCaseId).Where(x => x > caseID).ToList();
            if (nextIDS.Count > 0)
            {
                return RedirectToAction("CaseDetail", new { CaseID = nextIDS.First() });
            }
            else
            {
                var nextID = dbCaseInfos.First().FkCaseId;
                return RedirectToAction("CaseDetail", new { CaseID = nextID });
            }


        }

        [HttpPost]
        public async Task<ActionResult> PreviousInterestedCase(int caseID)
        {
            var ctx = _govtechHackathonContext;
            var dbCaseInfos = await ctx.CaseAdminApproved.Include("FkCase").Where(x => x.Approved).OrderBy(x => x.FkCaseId).ToListAsync();
            var prevIDS = dbCaseInfos.OrderBy(x => x.FkCaseId).Select(x => x.FkCaseId).Where(x => x < caseID).ToList();
            if (prevIDS.Count > 0)
            {
                return RedirectToAction("CaseDetail", new { CaseID = prevIDS.Last() });
            }
            else
            {
                var nextID = dbCaseInfos.Last().FkCaseId;
                return RedirectToAction("CaseDetail", new { CaseID = nextID });
            }

        }

        [HttpPost]
        public async Task<IActionResult> NextCase(int caseID)
        {
            var ctx = _govtechHackathonContext;

            var dbCaseInfos = await ctx.CaseAdminApproved.Include("FkCase").Where(x => x.Approved).OrderBy(x => x.FkCaseId).ToListAsync();
            var nextIDS = dbCaseInfos.OrderBy(x => x.FkCaseId).Select(x => x.FkCaseId).Where(x => x > caseID).ToList();
            if (nextIDS.Count > 0)
            {
                return RedirectToAction("CaseDetail", new { CaseID = nextIDS.First() });
            }
            else
            {
                var nextID = dbCaseInfos.First().FkCaseId;
                return RedirectToAction("CaseDetail", new { CaseID = nextID });
            }


        }

        [HttpPost]
        public async Task<ActionResult> PreviousCase(int caseID)
        {
            var ctx = _govtechHackathonContext;
            var dbCaseInfos = await ctx.CaseAdminApproved.Include("FkCase").Where(x => x.Approved).OrderBy(x => x.FkCaseId).ToListAsync();
            var prevIDS = dbCaseInfos.OrderBy(x => x.FkCaseId).Select(x => x.FkCaseId).Where(x => x < caseID).ToList();
            if (prevIDS.Count > 0)
            {
                return RedirectToAction("CaseDetail", new { CaseID = prevIDS.Last() });
            }
            else
            {
                var nextID = dbCaseInfos.Last().FkCaseId;
                return RedirectToAction("CaseDetail", new { CaseID = nextID });
            }

        }

        [HttpPost]
        public async Task<ActionResult> TermsAndConditionsAccepted(TermsConditions model)
        {

            var currentUser = HttpContext.Session.GetObjectFromJson<InvestorDetails>("CurrentInvestor");
            if (currentUser != null)
            {
                if (ModelState.IsValid)
                {
                    var ctx = _govtechHackathonContext;
                    var applicantDBObj = ctx.Investor.FirstOrDefault(x => x.PkId == currentUser.ID);
                    applicantDBObj.AcceptedTerms = true;
                    await ctx.SaveChangesAsync();
                    currentUser.AcceptedTermsConditions = true;
                    HttpContext.Session.SetObjectAsJson("CurrentInvestor", currentUser);
                    return RedirectToAction("AllProposals");
                }

            }

            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> CaseDetail(int caseID)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<InvestorDetails>("CurrentInvestor");
            if (currentUser != null)
            {
                var model = await InvestorCaseDetail.Load(caseID, currentUser.ID, _govtechHackathonContext);
                return View(model);
            }
            else
                return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Interested(int caseId, int investorID)
        {
            var ctx = _govtechHackathonContext;
            var interestedObj = new CaseInvestorInterested()
            {
                FkCaseId = caseId,
                FkInvestorId = investorID
            };
            ctx.CaseInvestorInterested.Add(interestedObj);
            await ctx.SaveChangesAsync();

            return RedirectToAction("MyProposals");
        }


        public async Task<IActionResult> NotInterested(int caseId, int investorID)
        {
            var ctx = _govtechHackathonContext;
            var caseInterest = await ctx.CaseInvestorInterested.FirstOrDefaultAsync(x => x.FkCaseId == caseId && x.FkInvestorId == investorID);
            if (caseInterest != null)
            {
                ctx.CaseInvestorInterested.Remove(caseInterest);
                await ctx.SaveChangesAsync();
            }

            return RedirectToAction("MyProposals");
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> MyProposals()
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<InvestorDetails>("CurrentInvestor");
            if (currentUser != null)
            {
                if (currentUser.AcceptedTermsConditions)
                {
                    var model = new InvestorProposals();
                    model.Title = "Proposals I am interested in";
                    var ctx = _govtechHackathonContext;
                    var dbCaseInfos = await ctx.CaseInvestorInterested.Include("FkCase").Include("FkCase.FkCategory").
                                        Include("FkCase.FkStatus").Where(x => x.FkInvestorId == currentUser.ID).ToListAsync();
                    model.Proposals.AddRange(dbCaseInfos.Select(x => (InvestorProposalDetails)x).ToList());
                    return View("Proposals", model);
                }
                else
                    return RedirectToAction("TermsConditions", "Investor");
            }
            else
                return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> AllProposals()
        {
            var model = new InvestorProposals();
            model.Title = "All proposals";
            var ctx = _govtechHackathonContext;
            var dbCaseInfos = await ctx.CaseAdminApproved.Include("FkCase").Include("FkCase.FkCategory").Include("FkCase.FkStatus").Where(x => x.Approved == true).ToListAsync();
            model.Proposals.AddRange(dbCaseInfos.Select(x => (InvestorProposalDetails)x).ToList());
            return View("Proposals", model);
        }

        public async Task<IActionResult> ApproveInvestor(IFormCollection frmData)
        {
            var ctx = _govtechHackathonContext;
            var model = new InvestorsTable();
            int investorID = Convert.ToInt32(frmData.First().Value);
            var dbInvestor = await ctx.Investor.FirstOrDefaultAsync(x => x.PkId == investorID);
            dbInvestor.Approved = true;
            dbInvestor.Active = true;
            await ctx.SaveChangesAsync();
            await model.GetAllInvestors(_govtechHackathonContext);
            model.UpdateSuccessful = true;
            return PartialView("/Views/Investor/_InvestorsTable.cshtml", model);
        }

        public async Task<IActionResult> ActivateInvestor(IFormCollection frmData)
        {
            var ctx = _govtechHackathonContext;
            var model = new InvestorsTable();
            int investorID = Convert.ToInt32(frmData.First().Value);
            var dbInvestor = await ctx.Investor.FirstOrDefaultAsync(x => x.PkId == investorID);
            dbInvestor.Active = true;
            await ctx.SaveChangesAsync();
            await model.GetAllInvestors(_govtechHackathonContext);
            model.UpdateSuccessful = true;
            return PartialView("/Views/Investor/_InvestorsTable.cshtml", model);
        }

        public async Task<IActionResult> DeActivateInvestor(IFormCollection frmData)
        {
            var ctx = _govtechHackathonContext;
            var model = new InvestorsTable();
            int investorID = Convert.ToInt32(frmData.First().Value);
            var dbInvestor = await ctx.Investor.FirstOrDefaultAsync(x => x.PkId == investorID);
            dbInvestor.Active = false;
            await ctx.SaveChangesAsync();
            await model.GetAllInvestors(_govtechHackathonContext);
            model.UpdateSuccessful = true;
            return PartialView("/Views/Investor/_InvestorsTable.cshtml", model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> SaveInvestorDetails(InvestorDetails model)
        {
            try
            {
                var ctx = _govtechHackathonContext;
                if (ModelState.IsValid)
                {
                    var identityUser = new IdentityUser { UserName = model.Email, Email = model.Email };
                    if (model.IsNew)
                    {

                        var result = await _userManager.CreateAsync(identityUser, model.Password);
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(identityUser, "Investor");

                            var dbInvestor = new Investor()
                            {
                                Name = model.Name,
                                Surname = model.Surname,
                                City = model.City,
                                Email = model.Email,
                                Phone = model.Phone,
                                FkProvinceId = model.ProvinceID,
                                FkIndustryId = model.IndustryID,
                                Company = model.Company
                            };

                            ctx.Investor.Add(dbInvestor);
                            await ctx.SaveChangesAsync();

                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                            var confirmationLink = Url.Action("ConfirmEmail", "Registration", new { userId = identityUser.Id, token }, Request.Scheme);

                            var body = new StringBuilder();
                            body.AppendLine(@"Please verify your email by clicking on the link below:<br/>");
                            body.AppendLine("<a href = \"" + confirmationLink + "\">Confirm</a></body></html>");
                            //var body = "<html><body>Please verify your email by clicking on the link below:<br/>";
                            //body += @"<a href = """ + confirmationLink+ @"""></a></body></html>";

                            var mailHelper = new MailHelper(_configuration);
                            mailHelper.SendAsync(_configuration["Gmail:Username"], new List<string>() { identityUser.Email }, "SITA Hackathon Email Confirmation", body.ToString(), false);

                            return RedirectToAction("Login", "Registration");
                        }
                        else
                        {

                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(error.Code, error.Description);
                                model.AuthErrors.Add(error.Description);
                            }

                            return View("InvestorDetails", model);



                        }
                    }
                    else
                    {
                        var dbIndvestor = ctx.Investor.First(x => x.PkId == model.ID);
                        dbIndvestor.FkProvinceId = model.ProvinceID;
                        dbIndvestor.FkIndustryId = model.IndustryID;
                        dbIndvestor.Company = model.Company;
                        dbIndvestor.Name = model.Name;
                        dbIndvestor.Surname = model.Surname;
                        dbIndvestor.Phone = model.Phone;
                        await ctx.SaveChangesAsync();

                        if (model.Email != dbIndvestor.Email)
                        {
                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                            var confirmationLink = Url.Action("ConfirmEmail", "Registration", new { userId = identityUser.Id, token }, Request.Scheme);

                            var body = new StringBuilder();
                            body.AppendLine(@"Please verify your email by clicking on the link below:<br/>");
                            body.AppendLine("<a href = \"" + confirmationLink + "\">Confirm</a></body></html>");
                            //var body = "<html><body>Please verify your email by clicking on the link below:<br/>";
                            //body += @"<a href = """ + confirmationLink+ @"""></a></body></html>";

                            var mailHelper = new MailHelper(_configuration);
                            mailHelper.SendAsync(_configuration["Gmail:Username"], new List<string>() { identityUser.Email }, "SITA Hackathon Email Confirmation", body.ToString(), false);


                        }

                        model.ShowUpdateSuccessful = true;
                    }
                }

                return View("InvestorDetails", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("An error has occured:", ex.Message);
                model.AuthErrors.Add(ex.Message);
                return View("InvestorDetails", model);
            }
        }
    }
}
