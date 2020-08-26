using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class SituationOptions
    {
        public SituationOptions()
        {
            CaseInformation = new HashSet<CaseInformation>();
        }

        public int PkId { get; set; }
        public string DisplayString { get; set; }
        public int DisplayOrder { get; set; }
        public string VisualId { get; set; }

        public virtual ICollection<CaseInformation> CaseInformation { get; set; }
    }
}
