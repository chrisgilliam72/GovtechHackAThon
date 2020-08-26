using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class AuditorProposaListItem
    {
        public int ProposalID { get; set; }
        public String ProposalName { get; set; }
        public DateTime DateSubmitted { get; set; }
        public String GroupName { get; set; }
        public int GroupAdjudicatorCount { get; set; }

        public int AdjudicatorScoredCount { get; set; }

        public int TotalTotal
        {
            get
            {
               return ScoringTotals.Sum(x => x.ScoreValue);
            }
        }
        public List<AuditorScoreTotalItem> ScoringTotals { get; set; }

        public int GetScoreTotal(int scoreCatagoryID)
        {
            var scores = ScoringTotals.Where(x => x.ScoreItemID == scoreCatagoryID).ToList();
            if (scores.Count>0)
                return scores.Sum(x => x.ScoreValue);
            return 0;
        }


        public String DateSubmittedString
        {
            get
            {
                return DateSubmitted.ToShortDateString();
            }
        }

        public AuditorProposaListItem()
        {
            ScoringTotals = new List<AuditorScoreTotalItem>();
        }

        public static explicit operator AuditorProposaListItem (CaseAssignments caseAssignement)
        {
            var auditItem = new AuditorProposaListItem();

            auditItem.ProposalID = caseAssignement.FkCase.PkId;
            auditItem.ProposalName = caseAssignement.FkCase.Name;
            auditItem.DateSubmitted = caseAssignement.FkCase.SubmittedDate;
            auditItem.GroupName = caseAssignement.FkGroup.Name;
            var scores = caseAssignement.FkCase.CaseCategoryScore.ToList();
            foreach (var score in scores)
            {
                var scoreTotalItem = new AuditorScoreTotalItem();
                scoreTotalItem.ScoreItemID = score.FkCategoryId;
                scoreTotalItem.ScoreValue = score.Score;
                auditItem.ScoringTotals.Add(scoreTotalItem);
            }
            auditItem.AdjudicatorScoredCount= scores.GroupBy(x => x.FkUserId).Count();
            auditItem.GroupAdjudicatorCount = caseAssignement.FkGroup.UsersInGroup.Count;
            return auditItem;
        }

    }
}
