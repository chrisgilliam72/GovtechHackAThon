using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class UsersInGroup
    {
        public int PkId { get; set; }
        public int FkGroupId { get; set; }
        public int FkUserId { get; set; }

        public virtual Group FkGroup { get; set; }
        public virtual User FkUser { get; set; }
    }
}
