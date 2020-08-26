using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class ResetPassword
    {
        [Required]
        [EmailAddress]
        public String Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        //[Remote(action: "VerifyPassword", controller: "AdjudicatorController", AdditionalFields = "ConfirmPassword")]
        public String Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirm password fields do not match.")]
        public String ConfirmPassword { get; set; }

        public String Token { get; set; }

        public ResetPassword()
        {
            Password = "";
            ConfirmPassword = "";
        }

    }
}
