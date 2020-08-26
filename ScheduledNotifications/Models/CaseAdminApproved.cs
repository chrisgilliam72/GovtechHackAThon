using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class CaseAdminApproved
    {
        public int PkId { get; set; }
        public bool Approved { get; set; }
        public string Comment { get; set; }
        public int FkCaseId { get; set; }

        public virtual CaseInformation FkCase { get; set; }
    }
}
