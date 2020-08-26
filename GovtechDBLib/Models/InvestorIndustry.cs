using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class InvestorIndustry
    {
        public InvestorIndustry()
        {
            Investor = new HashSet<Investor>();
        }

        public int PkId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Investor> Investor { get; set; }
    }
}
