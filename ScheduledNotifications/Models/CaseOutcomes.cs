using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class CaseOutcomes
    {
        public int PkId { get; set; }
        public int FkCaseId { get; set; }
        public int FkOutcomeId { get; set; }

        public virtual CaseInformation FkCase { get; set; }
        public virtual Outcomes FkOutcome { get; set; }
    }
}
