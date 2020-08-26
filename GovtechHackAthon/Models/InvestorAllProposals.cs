using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class InvestorProposals
    {
        public String Title { get; set; }
        public List<InvestorProposalDetails> Proposals{ get; set; }

       public InvestorProposals()
        {
            Proposals = new List<InvestorProposalDetails>();
        }
    }
}
