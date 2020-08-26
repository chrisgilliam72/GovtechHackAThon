using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class ApplicantCaseList
    {
        public int ApplicantID { get; set; }
        public List<CaseListItem> MyCases { get; set; }
        public List<CaseListItem> TeamCases { get; set; }


        public ApplicantCaseList()
        {
            MyCases = new List<CaseListItem>();
            TeamCases = new List<CaseListItem>();
        }
    }
}
