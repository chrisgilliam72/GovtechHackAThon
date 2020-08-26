using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class Auditor
    {
        public int PkId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
    }
}
