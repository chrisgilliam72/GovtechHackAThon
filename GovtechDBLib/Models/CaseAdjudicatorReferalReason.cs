using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class CaseAdjudicatorReferalReason
    {
        public int PkId { get; set; }
        public int FkCaseId { get; set; }
        public string Reason { get; set; }
        public DateTime ResubmissionDate { get; set; }

        public virtual CaseInformation FkCase { get; set; }
    }
}
