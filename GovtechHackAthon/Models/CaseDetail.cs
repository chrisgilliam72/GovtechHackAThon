using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class CaseDetail
    {
        public int CaseID { get; set; }
        public bool IsStudent { get; set; }
        public bool HasCompany { get; set; }
        public String CaseName { get; set; }
        public CompanyDetails Company { get; set; }
        public StudentDetails StudentInfo { get; set; }
        public ApplicantData Applicant { get; set; }
        public List<TeamMemberData> TeamMembers { get; set; }
        public List<String> OutComes { get; set; }
        public String SituationStatement { get; set; }
        public String SituationOption { get; set; }
        public String CompleteIdea { get; set; }
        public int CategoryID { get; set; }
        public int Year { get; set; }

        public CaseDetail()
        {
            TeamMembers = new List<TeamMemberData>();
            OutComes = new List<string>();
        }
    }
}
