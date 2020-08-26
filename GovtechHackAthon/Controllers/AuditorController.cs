using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovtechDBLib.Models;
using GovtechHackAthon.Helpers;
using GovtechHackAthon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GovtechHackAthon.Controllers
{
    public class AuditorController : Controller
    {
        private readonly GovtechHackathonContext _govtechHackathonContext;

        public AuditorController(GovtechHackathonContext govtechHackathonContext )
        {
            _govtechHackathonContext = govtechHackathonContext;
        }


        public async Task<IActionResult> AddNotePartial(AuditorNoteItem model)
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<UserDetails>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;
                var auditNotes = await ctx.AuditorNote.Include("FkAuditor").Where(x => x.FkAuditorId == currentUser.UserID && x.AuditYear == DateTime.Today.Year).ToListAsync();
                var notesList = new AuditorNotesList();
                return PartialView("/Views/Auditor/_Notes.cshtml", notesList);
            }

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Dashboard()
        {
            var currentUser = HttpContext.Session.GetObjectFromJson<UserDetails>("CurrentUser");
            if (currentUser != null)
            {
                var ctx = _govtechHackathonContext;

                var dbcaseAssignments = await ctx.CaseAssignments.Include("FkCase").Include("FkGroup").
                                                                Include("FkGroup.UsersInGroup").Include("FkCase.CaseCategoryScore")
                                                                .Where(x => x.FkCase.Year == DateTime.Today.Year).ToListAsync();

                var auditNotes = await ctx.AuditorNote.Include("FkAuditor").Where(x => x.FkAuditorId == currentUser.UserID && x.AuditYear == DateTime.Today.Year).ToListAsync();
                var auditNoteItems = auditNotes.Select(x => (AuditorNoteItem)x).ToList();
                var proposalItems = dbcaseAssignments.Select(x => (AuditorProposaListItem)x).ToList();
                var model = new AuditorProposalList();
                model.AuditorID = currentUser.UserID;
                model.NotesList.Notes.AddRange(auditNoteItems);
                model.Proposals.AddRange(proposalItems);
                return View(model);
            }

            return RedirectToAction("index", "home");
        }
    }
}
