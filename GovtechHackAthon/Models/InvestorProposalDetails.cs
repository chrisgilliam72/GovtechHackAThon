using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class InvestorProposalDetails
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Category { get; set; }

        public String DaeSubmitted { get; set; }

        public static explicit operator InvestorProposalDetails(CaseInvestorInterested caseInterested)
        {
            var proposal = new InvestorProposalDetails()
            {
                ID = caseInterested.FkCase.PkId,
                Name = caseInterested.FkCase.Name,
                DaeSubmitted = caseInterested.FkCase.SubmittedDate.ToShortDateString(),
                Category = caseInterested.FkCase.FkCategory != null ? caseInterested.FkCase.FkCategory.Name : "N/A"

            };
            return proposal;
        }

        public static explicit operator InvestorProposalDetails(CaseAdminApproved approvedCase)
        {
            var proposal = new InvestorProposalDetails()
            {
                ID = approvedCase.FkCase.PkId,
                Name = approvedCase.FkCase.Name,
                DaeSubmitted = approvedCase.FkCase.SubmittedDate.ToShortDateString(),
                Category = approvedCase.FkCase.FkCategory != null ? approvedCase.FkCase.FkCategory.Name : "N/A"

            };
            return proposal;
        }

        public static explicit operator InvestorProposalDetails(CaseInformation caseInfo)
        {
            var proposal = new InvestorProposalDetails()
            {
                ID = caseInfo.PkId,
                Name = caseInfo.Name,
                DaeSubmitted=caseInfo.SubmittedDate.ToShortDateString(),
                Category = caseInfo.FkCategory != null ? caseInfo.FkCategory.Name : "N/A"

            };
            return proposal;
        }
    }
}
