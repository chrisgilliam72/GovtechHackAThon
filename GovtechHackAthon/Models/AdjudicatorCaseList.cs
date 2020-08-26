using GovtechDBLib.Models;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class AdjudicatorCaseList
    {
        public bool TechnicalAdjudication { get; set; }
        public int OwnerID { get; set; }
        public List<CaseListItem> Cases { get; set; }

        ResetPassword PasswordReset { get; set; }

        public void FilterList()
        {
            foreach (var caseItem in Cases)
            {
                caseItem.RatingScores = caseItem.RatingScores.Where(x => x.AdjudicatorID == OwnerID).ToList();
                caseItem.TotalRating = caseItem.RatingScores.Sum(x => x.ActualScore.Value);
            }

        }

        public AdjudicatorCaseList()
        {
            Cases = new List<CaseListItem>();
            PasswordReset = new ResetPassword();
            TechnicalAdjudication = true;
        }
    }

}
