using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class UserComments
    {
        public int PkId { get; set; }
        public string Comment { get; set; }
        public DateTime DateStamp { get; set; }
        public int FkUserId { get; set; }
        public int FkCaseId { get; set; }

        public virtual CaseInformation FkCase { get; set; }
        public virtual User FkUser { get; set; }
    }
}
