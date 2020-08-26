using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class CaseRatingCriteriaItem
    {
        public int ScoreID { get; set; }
        public int CategoryID { get; set; }
        public int AdjudicatorID { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public int? ActualScore { get; set; }
        public int MaxScore { get; set; }
        public bool Submitted { get; set; }
        public String Type { get; set; }
        public static explicit operator CaseRatingCriteriaItem(CaseCategoryScore caseCatagoryScore)
        {
            var ratingItem = new CaseRatingCriteriaItem()
            {
                ScoreID = caseCatagoryScore.PkId,
                CategoryID= caseCatagoryScore.FkCategory.PkId,
                AdjudicatorID = caseCatagoryScore.FkUserId,
                ActualScore = caseCatagoryScore.Score,
                Submitted = caseCatagoryScore.Submitted,
                MaxScore = caseCatagoryScore.FkCategory != null ? caseCatagoryScore.FkCategory.MaxScore : 0,
                Type=caseCatagoryScore.FkCategory.FkType.Description
            };

            return ratingItem;
        }
    }

    
}
