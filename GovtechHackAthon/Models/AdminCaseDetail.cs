using GovtechDBLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class AdminCaseDetail
    {
        public AdjucatorCaseDetail AdjudicatorDetails { get; set; }
        public AdminApproveReject AdminApprovalDetails { get; set; }

        public AdminCaseDetail()
        {
            AdminApprovalDetails = new AdminApproveReject();
        }

        public static async Task<AdminCaseDetail> Load(int caseID, int UserID,int adjudicationRound, GovtechHackathonContext ctx)
        {
            var adminCaseDetail = new AdminCaseDetail();
            adminCaseDetail.AdjudicatorDetails = await AdjucatorCaseDetail.Load(caseID, UserID, adjudicationRound, ctx);
            adminCaseDetail.AdjudicatorDetails.Rating.ViewOnly = true;
            adminCaseDetail.AdjudicatorDetails.ActionList.ViewOnly = true;
            var adminAssigned = await ctx.CaseAdminApproved.FirstOrDefaultAsync(x => x.FkCaseId == caseID);
            if (adminAssigned != null)
            {
                adminCaseDetail.AdminApprovalDetails.CaseID = adminAssigned.FkCaseId;
                adminCaseDetail.AdminApprovalDetails.Comment = adminAssigned.Comment;
                adminCaseDetail.AdminApprovalDetails.ApprovalState = adminAssigned.Approved ? AdminApprovalState.Approved : AdminApprovalState.Rejected;
            }
            else
                adminCaseDetail.AdminApprovalDetails.CaseID = caseID;
            return adminCaseDetail;
        }
    }
}
