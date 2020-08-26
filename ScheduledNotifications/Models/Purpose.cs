using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class Purpose
    {
        public Purpose()
        {
            CaseInformation = new HashSet<CaseInformation>();
        }

        public int PkId { get; set; }
        public string Motivation { get; set; }
        public string CoreValues { get; set; }

        public virtual ICollection<CaseInformation> CaseInformation { get; set; }
    }
}
