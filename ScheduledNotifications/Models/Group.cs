using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class Group
    {
        public Group()
        {
            CaseAssignments = new HashSet<CaseAssignments>();
            UsersInGroup = new HashSet<UsersInGroup>();
        }

        public int PkId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<CaseAssignments> CaseAssignments { get; set; }
        public virtual ICollection<UsersInGroup> UsersInGroup { get; set; }
    }
}
