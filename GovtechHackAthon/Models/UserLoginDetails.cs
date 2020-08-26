using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class UserLoginDetails
    {
        [Required]
        //[Remote(action: "VerifyUsername", controller: "Registration")]
        public String UserName { get; set; }
        [Required]
        //[Remote(action: "VerifyPassword", controller: "Registration", AdditionalFields = nameof(UserName))]
        public String Password { get;set; }
    }
}
