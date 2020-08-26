using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class ScoreboardItem
    {
        public String CaseName { get; set; }

        public String Adjudicator { get; set; }

        public String Group { get; set; }

        public int Rank { get; set; }

        public int Total { get; set; }

        public List<ScoreboardCategoryScore> Scores { get; set; }



        public ScoreboardItem()
        {
            Scores = new List<ScoreboardCategoryScore>();
        }

        public static explicit operator ScoreboardItem(CaseCategoryScore caseScore)
        {
            var scoreboardItem = new ScoreboardItem();
            scoreboardItem.Adjudicator = caseScore.FkUser.FirstName + " " + caseScore.FkUser.Surname;
            scoreboardItem.CaseName = caseScore.FkCase.Name;
            scoreboardItem.Group = caseScore.FkUser.UsersInGroup.FirstOrDefault().FkGroup.Name;
            return scoreboardItem;
        }
    }
}
