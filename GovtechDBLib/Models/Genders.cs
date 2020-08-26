using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class Genders
    {
        public Genders()
        {
            TeamMember = new HashSet<TeamMember>();
        }

        public int PkId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TeamMember> TeamMember { get; set; }
    }
}
