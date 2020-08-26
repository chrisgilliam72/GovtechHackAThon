using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class CaseStatus
    {
        public CaseStatus()
        {
            CaseInformation = new HashSet<CaseInformation>();
        }

        public int PkId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CaseInformation> CaseInformation { get; set; }
    }
}
