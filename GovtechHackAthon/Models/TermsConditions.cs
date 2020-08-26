using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GovtechHackAthon.Helpers;

namespace GovtechHackAthon.Models
{
    public class TermsConditions
    {
        [Range(typeof(bool), "true", "true", ErrorMessage ="You must agree to terms and conditions")]
        public bool AcceptedTermsAndConditions { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to privacy policy")]
        public bool AcceptedPrivacy { get; set; }
    }
}
