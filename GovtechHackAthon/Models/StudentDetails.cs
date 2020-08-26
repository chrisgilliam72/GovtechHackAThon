using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class RegistrationStudentDetails
    {
        [Required]
        public String Course { get; set; }
        [Required]
        public String Institution { get; set; }
    }
}
