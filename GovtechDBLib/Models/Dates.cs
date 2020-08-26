using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class Dates
    {
        public int PkId { get; set; }
        public string DateName { get; set; }
        public DateTime Date { get; set; }
        public int Stage { get; set; }
        public string StageName { get; set; }
    }
}
