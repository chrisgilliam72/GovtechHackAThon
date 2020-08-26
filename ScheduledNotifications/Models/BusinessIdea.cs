using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class BusinessIdea
    {
        public BusinessIdea()
        {
            CaseInformation = new HashSet<CaseInformation>();
        }

        public int PkId { get; set; }
        public string WhatProblem { get; set; }
        public string How { get; set; }
        public string Who { get; set; }
        public string Impact { get; set; }

        public virtual ICollection<CaseInformation> CaseInformation { get; set; }
    }
}
