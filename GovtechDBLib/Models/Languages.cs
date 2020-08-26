using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class Languages
    {
        public Languages()
        {
            Applicant = new HashSet<Applicant>();
        }

        public int PkId { get; set; }
        public string Language { get; set; }

        public virtual ICollection<Applicant> Applicant { get; set; }
    }
}
