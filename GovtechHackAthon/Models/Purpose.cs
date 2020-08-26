using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GovtechHackAthon.Models
{
    public partial class RegistrationPurpose
    {
        public int CaseID { get; set; }
        public int PurposeID { get; set; }
        [Required]
        public string Motivation { get; set; }
        [Required]
        public string CoreValues { get; set; }

        public bool NeedsMotivation { get; set; }
        public bool NeedsCoreValues { get; set; }

        public bool ShowToast { get; set; }
        public RegistrationPurpose()
        {
            NeedsMotivation = true;
            NeedsCoreValues = false;
            PurposeID = -999;
        }

    }
}
