using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class VwCaseGroups
    {
        public int PkId { get; set; }
        public string Name { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
