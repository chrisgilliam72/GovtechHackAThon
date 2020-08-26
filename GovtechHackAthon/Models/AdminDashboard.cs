using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class AdminDashboard
    {
        public int TotalApplicants { get; set; }
        public int TotalApplications { get; set; }
        public int RevewiedApplications { get; set; }
        public int UnassignedApplications { get; set; }
    }
}
