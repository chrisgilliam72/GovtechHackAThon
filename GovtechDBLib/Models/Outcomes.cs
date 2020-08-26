using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class Outcomes
    {
        public Outcomes()
        {
            CaseOutcomes = new HashSet<CaseOutcomes>();
        }

        public int PkId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CaseOutcomes> CaseOutcomes { get; set; }
    }
}
