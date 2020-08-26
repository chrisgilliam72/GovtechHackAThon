using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class Provinces
    {
        public Provinces()
        {
            Company = new HashSet<Company>();
            Investor = new HashSet<Investor>();
            TeamMember = new HashSet<TeamMember>();
            User = new HashSet<User>();
        }

        public int PkId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Company> Company { get; set; }
        public virtual ICollection<Investor> Investor { get; set; }
        public virtual ICollection<TeamMember> TeamMember { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
