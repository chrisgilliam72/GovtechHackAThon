using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class CaseAssignments
    {
        public int PkId { get; set; }
        public int FkCaseId { get; set; }
        public int FkGroupId { get; set; }
        public string Comment { get; set; }

        public virtual CaseInformation FkCase { get; set; }
        public virtual Group FkGroup { get; set; }
    }
}
