using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using GovtechDBLib.Models;
using GovtechHackAthon.Helpers;
using GovtechHackAthon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;

namespace GovtechHackAthon.Controllers
{
    [Authorize]
    public class AdjudicatorController : Controller
    {
        private readonly GovtechHackathonContext _govtechHackathonContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AdjudicatorController(GovtechHackathonContext govtechHackathonContext,
                                     UserManager<IdentityUser> userManager,
                                     IConfiguration configuration)
        {
            _govtechHackathonContext = govtechHackathonContext;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordPartial(ChangePassword model)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<UserDetails>("CurrentUser");
            if (currentUser != null)
            {
                var user = await _userManager.FindByEmailAsync(currentUser.Email);
                if (user != null)
                {
                    if (model.ConfirmPassword != model.Password)
                    {
                        model.HasErrors = true;
                        ModelState.AddModelError("", "Password and confirmation fields do not match");
                        return PartialView("/Views/Shared/_ChangePassword.cshtml", model);
                    }
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
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
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> DeferCase(AdjudicatorDeferralReasonData model)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<UserDetails>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;
                var deferredStatus = await ctx.CaseStatus.FirstOrDefaultAsync(x => x.Name == "Deferred");
                var caseInfo = await ctx.CaseInformation.FirstOrDefaultAsync(x => x.PkId == model.CaseID);

                caseInfo.FkStatusId = deferredStatus.PkId;

                var deferalReason = new CaseAdjudicatorReferalReason()
                {
                    FkCaseId = model.CaseID,
                    Reason = model.Reason,
                    ResubmissionDate = model.ResubmissionDate
                };

                ctx.CaseAdjudicatorReferalReason.Add(deferalReason);
                await ctx.SaveChangesAsync();

                var body = new StringBuilder();
                body.AppendLine(@"<html><body>Your proposal : " + caseInfo.Name + " ID= " + caseInfo.PkId + " has been deferred and must be resubmitted again by " + model.ResubmissionDate.ToShortDateString() + ". </body></html>");

                var notificationMgr = new NotificationManager(_govtechHackathonContext, _configuration);
                await notificationMgr.NotifyTeam(caseInfo.PkId, "Proposal Submitted", body.ToString());

                return RedirectToAction("Dashboard");

            }

            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> NextCase(int caseID)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<UserDetails>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;
                var grpIds = currentUser.Groups;
                if (grpIds.Count > 0)
                {
                    var caseAssignments = await ctx.CaseAssignments.Where(x => grpIds.Contains(x.FkGroupId)).OrderBy(x => x.FkCaseId).ToListAsync();
                    var nextIDS = caseAssignments.Select(x => x.FkCaseId).Where(x => x > caseID).ToList();
                    if (nextIDS.Count > 0)
                    {
                        return RedirectToAction("CaseDetail", new { CaseID = nextIDS.First() });
                    }
                    else
                    {
                        var nextID = caseAssignments.FirstOrDefault().FkCaseId;
                        return RedirectToAction("CaseDetail", new { CaseID = nextID });
                    }
                }
                else
                {
                    return RedirectToAction("CaseDetail", new { CaseID = caseID });

                }
                          
            }

            return RedirectToAction("index", "home");
        }

        public async Task<ActionResult>PreviousCase(int caseID)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<UserDetails>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;
                var grpIds = currentUser.Groups;
                if (grpIds.Count > 0)
                {
                    var caseAssignments = await ctx.CaseAssignments.Where(x => grpIds.Contains(x.FkGroupId)).OrderByDescending(x => x.FkCaseId).ToListAsync();
                    var prevIDS = caseAssignments.Select(x => x.FkCaseId).Where(x => x < caseID).ToList();
                    if (prevIDS.Count > 0)
                    {
                        return RedirectToAction("CaseDetail", new { CaseID = prevIDS.First() });
                    }
                    else
                    {
                        var nextID = caseAssignments.First().FkCaseId;
                        return RedirectToAction("CaseDetail", new { CaseID = nextID });
                    }
                }
                else
                {
                    return RedirectToAction("CaseDetail", new { CaseID = caseID });

                }

            }

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> CaseDetail(int caseID)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<UserDetails>("CurrentUser");
            if (currentUser != null)
            {
                var model = await AdjucatorCaseDetail.Load(caseID, currentUser.UserID,2, _govtechHackathonContext);
                model.CurrentRound = 2;
                return View(model);
            }

            return RedirectToAction("index", "home");
        }



        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var model = new AdjudicatorCaseList();
            var currentUser = HttpContext.Session.GetObjectFromJson<UserDetails>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;
                var usrGroups = await ctx.UsersInGroup.Where(x => x.FkUserId == currentUser.UserID).ToListAsync();
                var usrGrpIDs = usrGroups.Select(x => x.FkGroupId).ToList();

                var dbcaseAssignments = await ctx.CaseAssignments.Include("FkCase").Include("FkCase.FkStatus").
                                                                 Include("FkCase.CaseCategoryScore").
                                                                    Include("FkCase.CaseCategoryScore.FkCategory.FkType").
                                                                  Include("FkCase.CaseComment").
                                                                 Include("FkCase.CaseComment.FkUser").
                                                                 Include("FkGroup").
                                                                  Where(x=> usrGrpIDs.Contains(x.FkGroupId) ).
                                                                  ToListAsync();

                model.OwnerID = currentUser.UserID;
                var caseitems = dbcaseAssignments.Select(x => (CaseListItem)x).ToList();
                model.Cases.AddRange(caseitems);

                foreach (var caseItem in caseitems)
                {
                    var ratingScores = caseItem.RatingScores.Where(x => x.AdjudicatorID == currentUser.UserID);
                    if (ratingScores.Count() > 0)
                        caseItem.Submitted = ratingScores.All(x => x.Submitted == true) == true ? "Yes" : "No";
                    else
                        caseItem.Submitted = "No";
                }

                model.FilterList();
                return View(model);
            }

            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> AddActionPartial(CaseActionData model)
        {
            var ctx = _govtechHackathonContext;
            var caseAction = new CaseAction()
            {
                ActionDate = model.Date,
                Description = model.Action,
                FkAdjudicatorId = model.AdjudicatorID,
                FkCaseId = model.CaseID
            };

            ctx.CaseAction.Add(caseAction);
            await ctx.SaveChangesAsync();
            var dbActions = await ctx.CaseAction.Include("FkAdjudicator").Where(x => x.FkCaseId == model.CaseID).ToListAsync();
            var actionsList = new CaseActionsList();
            actionsList.AdjudicatorID = model.AdjudicatorID;
            actionsList.CaseID = model.CaseID;
            foreach (var dbAction in dbActions)
            {
                var action = (CaseActionData)dbAction;
                actionsList.Actions.Add(action);
            }
            ModelState.Clear();
            return PartialView("/Views/Adjudicator/_ActionsTable.cshtml", actionsList) ;
        }


        [HttpPost]
        public async Task<IActionResult> RemoveActionPartial([Bind(Prefix = "action.ID")] int  actionID,
                                                            [Bind(Prefix = "action.CaseID")]  int caseID,
                                                             [Bind(Prefix = "action.AdjudicatorID")] int adjudicatorID)
        {
            var ctx = _govtechHackathonContext;
            var caseAction = await ctx.CaseAction.FirstOrDefaultAsync(x => x.PkId == actionID);
            if (caseAction != null)
            {
                ctx.CaseAction.Remove(caseAction);
                await ctx.SaveChangesAsync();
            }

            var dbActions = await ctx.CaseAction.Where(x => x.FkCaseId == caseID).ToListAsync();
            var actionsList = new CaseActionsList();
            actionsList.AdjudicatorID = adjudicatorID;
            actionsList.CaseID = caseID;
            foreach (var dbAction in dbActions)
            {
                var action = (CaseActionData)dbAction;
                actionsList.Actions.Add(action);
            }
            ModelState.Clear();
            return PartialView("/Views/Adjudicator/_ActionsTable.cshtml", actionsList);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateRatingPartial(CaseRatingInfo model, String UpdateRating)
        {
            bool hasErrors = false;
            var criteriaCount = model.RatingCriteria.Count;
            for (int i=0;i<criteriaCount;i++)
            {
                var category = model.RatingCriteria[i];
                if (category.ActualScore<0 || category.ActualScore > category.MaxScore)
                {
                    hasErrors = true;
                    var errorKey = "RatingCriteria[" + i+ "].ActualScore";
                    ModelState.AddModelError(errorKey, "value out of range");
                }
            }
            if (hasErrors)
            {
                return PartialView("/Views/Adjudicator/_AdjudicationReview.cshtml", model);
            }

            var ctx = _govtechHackathonContext;
            var categories = await ctx.CaseCategoryScore.Where(x => x.FkCaseId == model.CaseID && x.FkUserId == model.UserID).ToListAsync();
            var caseInfo = await ctx.CaseInformation.FirstOrDefaultAsync(x => x.PkId == model.CaseID);
            ctx.CaseCategoryScore.RemoveRange(categories);
            await ctx.SaveChangesAsync();

            model.ShowToast = true;
            foreach (var category in model.RatingCriteria)
            {


                var score = new CaseCategoryScore()
                {
                    FkCategoryId = category.CategoryID,
                    Score = category.ActualScore ?? 0,
                    FkCaseId = model.CaseID,
                    FkUserId = model.UserID,
                    Round = 1,
                    Submitted = UpdateRating == "Submit"
                };
                ctx.CaseCategoryScore.Add(score);

                category.Submitted = UpdateRating == "Submit";
            }
            var caseComment = new CaseComment()
            {
                FkCaseId = model.CaseID,
                FkUserId = model.UserID,
                Comment = model.Comment,
                DateStamp = DateTime.Now

            };
            caseInfo.FkCategoryId = model.CategoryID;

            ctx.CaseComment.Add(caseComment);
            caseInfo.IsAdjudicatorSubmitted = UpdateRating == "Submit";
            await ctx.SaveChangesAsync();
            return PartialView("/Views/Adjudicator/_AdjudicationReview.cshtml", model);
        }

    }
}
