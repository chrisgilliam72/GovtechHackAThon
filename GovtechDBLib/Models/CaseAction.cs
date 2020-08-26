using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class CaseAction
    {
        public int PkId { get; set; }
        public string Description { get; set; }
        public int FkAdjudicatorId { get; set; }
        public int FkCaseId { get; set; }
        public DateTime ActionDate { get; set; }

        public virtual User FkAdjudicator { get; set; }
        public virtual CaseInformation FkCase { get; set; }
    }
}
