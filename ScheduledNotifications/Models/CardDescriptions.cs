using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class CardDescriptions
    {
        public int PkId { get; set; }
        public string WhatProblem { get; set; }
        public string HowSolve { get; set; }
        public string WhoGot { get; set; }
        public string WhatImpact { get; set; }
        public string WhatMotivates { get; set; }
        public string WhatCoreValues { get; set; }
    }
}
