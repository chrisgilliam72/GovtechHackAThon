using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class CaseComment
    {
        public int PkId { get; set; }
        public string Comment { get; set; }
        public int FkCaseId { get; set; }
        public int FkUserId { get; set; }
        public DateTime DateStamp { get; set; }

        public virtual CaseInformation FkCase { get; set; }
        public virtual User FkUser { get; set; }
    }
}
