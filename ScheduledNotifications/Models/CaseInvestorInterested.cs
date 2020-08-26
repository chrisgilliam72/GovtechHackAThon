using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class CaseInvestorInterested
    {
        public int PkId { get; set; }
        public int FkCaseId { get; set; }
        public int FkInvestorId { get; set; }

        public virtual CaseInformation FkCase { get; set; }
        public virtual Investor FkInvestor { get; set; }
    }
}
