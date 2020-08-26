using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class CaseListItem
    {
        public int CaseID { get; set; }
        public String CaseName { get; set; }
        public String DateSubmitted { get; set; }
        public String Status { get; set; }
        public String Comment { get; set; }
        public String AssignedTo { get; set; }
        public List<CaseRatingComment> CommentList { get; set; }
        public List<CaseRatingCriteriaItem> RatingScores { get; set; }
        public int TotalRating { get; set; }

        public String Submitted { get; set; }
        public bool CanDelete
        {
            get
            {
                return  Status=="Saved";
            }
        }

   

        public CaseListItem()
        {
            Submitted = "No";
            CommentList = new List<CaseRatingComment>();
            RatingScores = new List<CaseRatingCriteriaItem>();
        }

        public static explicit operator CaseListItem(CaseInformation caseInfo)
        {
            var item = new CaseListItem()
            {
                CaseID = caseInfo.PkId,
                CaseName = caseInfo.Name,
                DateSubmitted = caseInfo.SubmittedDate.ToShortDateString(),
                Status = caseInfo.FkStatus.Name
            };
            return item;
        }


        public static explicit operator CaseListItem(CaseAssignments caseAssignment)
        {
            var item = new CaseListItem()
            {
                CaseID = caseAssignment.FkCaseId,
                CaseName = caseAssignment.FkCase.Name,
                DateSubmitted = caseAssignment.FkCase.SubmittedDate.ToShortDateString(),
                Status = caseAssignment.FkCase.FkStatus.Name,
                AssignedTo = caseAssignment.FkGroup.Name
            };

            var ratingScores = caseAssignment.FkCase.CaseCategoryScore.Select(x=> (CaseRatingCriteriaItem)x).ToList();
            var ratingComments = caseAssignment.FkCase.CaseComment.Select(x => (CaseRatingComment)x).ToList();
            item.CommentList.AddRange(ratingComments);
            item.RatingScores.AddRange(ratingScores.OrderBy(x => x.CategoryID).ToList());


            return item;
        }
    }
}
