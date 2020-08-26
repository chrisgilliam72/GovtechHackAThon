using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovtechDBLib.Models;
using GovtechHackAthon.Helpers;
using GovtechHackAthon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GovtechHackAthon.Controllers
{
    [Authorize]
    public class CaseListController : Controller
    {
        private readonly GovtechHackathonContext _govtechHackathonContext;

        public CaseListController(GovtechHackathonContext govtechHackathonContext)
        {
            _govtechHackathonContext = govtechHackathonContext;
        }


        [HttpPost]
        public async Task<IActionResult> CancelCase(int applicantID, [Bind(Prefix = "caseItem.CaseID")]int caseID)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                if (applicantID == currentUser.RegistrationData.UserID)
                {
                    var ctx = _govtechHackathonContext;
                    var caseInfo = await ctx.CaseInformation.FirstOrDefaultAsync(x => x.PkId == caseID);
                    var canceledStatus = await ctx.CaseStatus.FirstOrDefaultAsync(x => x.Name == "Canceled");
                    if (caseInfo != null && canceledStatus != null)
                    {
                        caseInfo.FkStatusId = canceledStatus.PkId;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction("CaseList", "CaseList");
                    }
                }
                return RedirectToAction("CaseList", "CaseList");
            }

            return RedirectToAction("Login", "Registration");
        }

        [HttpGet]
        public IActionResult NewCase()
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                currentUser.Case = null;
                currentUser.Teams.Clear();
                currentUser.Teams.AddTeamMembers(2);
                HttpContext.Session.SetObjectAsJson("CurrentUser", currentUser);
                return RedirectToAction("CaseInfo", "Registration");
            }
            return RedirectToAction("Login", "Registration");
        }


        [HttpGet]
        public async Task<IActionResult> CaseList()
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<ApplicantInfo>("CurrentUser");
            if (currentUser != null)
            {
                var model = new ApplicantCaseList();
                model.ApplicantID = currentUser.RegistrationData.UserID;
                if (currentUser != null)
                {
                    var ctx = _govtechHackathonContext;
                    var cases = await ctx.CaseInformation.Include("FkStatus")
                                    .Where(x => x.FkUserId == currentUser.RegistrationData.UserID && x.FkStatus.Name != "Canceled").ToListAsync();
                    model.MyCases.AddRange(cases.Select(x => (CaseListItem)x));

                    var teams = await ctx.TeamMember.Where(x => x.EmailAddress == currentUser.RegistrationData.Email).ToListAsync();
                    if (teams!=null && teams.Count>0)
                    {
                        var caseIDS = teams.Select(x => x.FkCaseId).ToList();
                        cases = await ctx.CaseInformation.Include("FkStatus")
                                    .Where(x=> caseIDS.Contains(x.PkId) && x.FkStatus.Name != "Canceled").ToListAsync();
                        model.TeamCases.AddRange(cases.Select(x => (CaseListItem)x));
                    }


                }
                return View(model);
            }
            return RedirectToAction("Login", "Registration");
        }
    }
}