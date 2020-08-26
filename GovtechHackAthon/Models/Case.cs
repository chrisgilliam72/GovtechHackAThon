using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    enum CaseStatus { NoCompany,DevelopingCompany,DevelopingPartOfCompany,PlayingAround  }
    public class Case
    {

        public int CaseID { get; set; }
        [Required]
        public String CaseName { get; set; }

        public DateTime DateSubmitted { get; set; }

        public String Status { get; set; }

        public bool HasCompany { get; set; }
        public bool HasStudentInfo { get; set; }

        public CompanyDetails Company { get; set; }
    
        public RegistrationStudentDetails StudentInfo { get; set; }

        public String Situation { get; set; }

        public bool SituationOpen { get; set; }
        public bool OptionsOpen { get; set; }
        public bool OutcomesOpen { get; set; }

        public bool NeedsSituationStatement {get;set;}
        public bool NeedsSituationOption { get; set; }
        public bool NeedsOutcomes { get; set; }

        [Required(ErrorMessage ="Situation option is required")]
        public int SituationOptionID { get; set; }
        [Required(ErrorMessage = "Situation statement is required")]
        public int SituationStatementID { get; set; }

        public Dictionary<int, bool> OutComeSelections { get; set; }

        public List<int> OutComeIDS { get; set; }
        public bool WorksForGovernment { get; set;  }

        public bool ShowToast { get; set; }
        public String DateSubmittedString
        {
            get
            {
                return DateSubmitted.ToShortDateString();
            }
        }

        public Case(Case srcCase)
        {
            HasCompany = false;
            OutComeSelections = new Dictionary<int, bool>();
            OutComeIDS = new List<int>();

            CaseID = srcCase.CaseID;
            CaseName = srcCase.CaseName;
            DateSubmitted = srcCase.DateSubmitted;
            Status = srcCase.Status;
            HasCompany = srcCase.HasCompany;
            HasStudentInfo = srcCase.HasStudentInfo;
            Situation = srcCase.Situation;
            SituationOptionID = srcCase.SituationOptionID;
            SituationStatementID = srcCase.SituationStatementID;

            foreach (var outComeID in srcCase.OutComeIDS)
                OutComeIDS.Add(outComeID);

            if (srcCase.Company!=null)
            {
                Company = new CompanyDetails()
                {
                    Name = srcCase.Company.Name,
                    ZipCode = srcCase.Company.ZipCode,
                    City = srcCase.Company.City,
                    Province = srcCase.Company.Province,
                    ProvinceID = srcCase.Company.ProvinceID
                };
            }

            if (srcCase.StudentInfo!=null)
            {
                StudentInfo = new RegistrationStudentDetails()
                {
                    Course = srcCase.StudentInfo.Course,
                    Institution = srcCase.StudentInfo.Institution
                };
            }
          }

        public Case()
        {
            NeedsSituationStatement = false;
            NeedsSituationOption = false;
            NeedsOutcomes = false;
            HasCompany = false;
            HasStudentInfo = false;
            OutComeSelections = new Dictionary<int, bool>() ;
            OutComeIDS = new List<int>();
        }
    }
}