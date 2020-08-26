using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class CaseRatingInfo
    {
        [Display(Name = "Stage")]
        public int Round { get; set; }
        public int CaseID { get; set; }
        public int UserID { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public String Comment { get; set; }
        public List<CaseRatingCriteriaItem> RatingCriteria { get; set; }
        public bool ShowToast { get; set; }
        public int TotalRating { get; set; }
        public int CategoryID { get; set; }
        public bool ViewOnly { get; set; }
        public bool IsSubmitted
        {
            get
            {
                return RatingCriteria.Any(x => x.Submitted == true);
            }
        }

        public CaseRatingInfo(int currentAdjudicationRound )
        {
            ViewOnly = false;
            Round = currentAdjudicationRound;
            RatingCriteria = new List<CaseRatingCriteriaItem>();
            ShowToast = false;
        }
    }


}
