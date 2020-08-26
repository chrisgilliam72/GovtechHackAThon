using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class Race
    {
        public Race()
        {
            Applicant = new HashSet<Applicant>();
            TeamMember = new HashSet<TeamMember>();
        }

        public int PkId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Applicant> Applicant { get; set; }
        public virtual ICollection<TeamMember> TeamMember { get; set; }
    }
}
