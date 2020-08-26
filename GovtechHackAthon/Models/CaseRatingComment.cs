using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class CaseRatingComment
    {
        public int CaseID { get; set; }
        public String Adjudicator { get; set; }
        public String Comment { get; set; }

        public static explicit operator CaseRatingComment(CaseComment dbComment)
        {
            var caseRatingComment = new CaseRatingComment()
            {
                CaseID=dbComment.FkCaseId,
                Adjudicator = dbComment.FkUser.FirstName + " " + dbComment.FkUser.Surname,
                Comment=dbComment.Comment
            };

            return caseRatingComment;
        }
    }


}
