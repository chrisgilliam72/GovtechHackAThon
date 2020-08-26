using GovtechDBLib.Models;
using GovtechHackAthon.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class CaseAssignmentEntry
    {
        public int CaseID { get; set; }
        public String CaseName { get; set; }
        public String DateSubmitted { get; set; }
       public String Status
        {
            get
            {
                if (AdminApproved.HasValue)
                {
                    return AdminApproved.Value ? "Approved" : "Rejected";
                }
                else
                    return "N/A";
            }
        }
        public String AssignedTo { get; set; }
        public int  AssignedtoGroupID { get; set; }

        public bool? AdminApproved { get; set; }

        public static explicit operator CaseAssignmentEntry(CaseInformation caseInfo)
        {
            var caseAssignmentEntry = new CaseAssignmentEntry()
            {
                CaseID = caseInfo.PkId,
                CaseName = caseInfo.Name,
                DateSubmitted = caseInfo.SubmittedDate.ToShortDateString(),
                AdminApproved = caseInfo.CaseAdminApproved.FirstOrDefault()!=null  ? caseInfo.CaseAdminApproved.FirstOrDefault().Approved : (bool?)null 

            };

            return caseAssignmentEntry;
        }

        public static explicit operator CaseAssignmentEntry(CaseAssignments caseAssignmentInfo)
        {
            if (caseAssignmentInfo!=null)
            {
                var caseAssignmentEntry = new CaseAssignmentEntry()
                {
                    CaseID = caseAssignmentInfo.FkCase.PkId,
                    CaseName = caseAssignmentInfo.FkCase.Name,
                    DateSubmitted = caseAssignmentInfo.FkCase.SubmittedDate.ToShortDateString(),
                    AssignedtoGroupID = caseAssignmentInfo.FkGroupId,
                    AdminApproved = caseAssignmentInfo.FkCase.CaseAdminApproved.FirstOrDefault() != null ? caseAssignmentInfo.FkCase.CaseAdminApproved.FirstOrDefault().Approved : (bool?)null


                };

                return caseAssignmentEntry;
            }

            return new CaseAssignmentEntry();
        }
    }


}
