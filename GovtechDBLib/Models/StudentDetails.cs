using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class StudentDetails
    {
        public StudentDetails()
        {
            CaseInformation = new HashSet<CaseInformation>();
        }

        public int PkId { get; set; }
        public string Course { get; set; }
        public string Instituion { get; set; }

        public virtual ICollection<CaseInformation> CaseInformation { get; set; }
    }
}
