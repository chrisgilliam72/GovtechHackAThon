using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage ="Email required")]
        [EmailAddress]
        public String EmailAddress { get; set; }
    }
}
