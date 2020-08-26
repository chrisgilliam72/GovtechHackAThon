using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class UserLevel
    {
        public UserLevel()
        {
            User = new HashSet<User>();
        }

        public int PkId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
