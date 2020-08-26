using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class SituationStatements
    {
        public SituationStatements()
        {
            CaseInformation = new HashSet<CaseInformation>();
        }

        public int PkId { get; set; }
        public string DisplayString { get; set; }
        public string VisualId { get; set; }
        public bool RequiresCompany { get; set; }
        public int DisplayOrder { get; set; }
        public bool RequiresStudentInfo { get; set; }

        public virtual ICollection<CaseInformation> CaseInformation { get; set; }
    }
}
