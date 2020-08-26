using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class AuditorNote
    {
        public int PkId { get; set; }
        public int FkAuditorId { get; set; }
        public int AuditYear { get; set; }
        public DateTime NoteDate { get; set; }
        public string Note { get; set; }

        public virtual User FkAuditor { get; set; }
    }
}
