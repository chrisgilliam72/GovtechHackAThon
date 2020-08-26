using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class CaseQuestion
    {
        public int CaseID { get; set; }
        public int ApplicantID { get; set; }
        public String Question { get; set; }
        public bool Successful { get; set; }

        public CaseQuestion()
        {

        }

        public CaseQuestion(int caseID, int applicantID)
        {
            Successful = false;
            CaseID = caseID;
            ApplicantID = applicantID;
        }
    }
}
