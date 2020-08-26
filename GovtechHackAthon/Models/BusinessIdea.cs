using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class RegistrationBusinessIdea
    {
        public int CaseID { get; set; }
        public int IdeaID { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public String What { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public String Who { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public String How { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public String Impact { get; set; }

        public bool NeedsHow { get; set; }
        public bool NeedsWhat { get; set; }
        public bool NeedsWho { get; set; }
        public bool NeedsImpact { get; set; }

        public ToastMessage Toast {get;set;}

        public bool ShowToast { get; set; }

        public RegistrationBusinessIdea()
        {
            IdeaID = -999;
            NeedsWhat = true;
            NeedsHow = false;
            NeedsWho = false;
            NeedsImpact = false;
            ShowToast = false;

            Toast = new ToastMessage("Success", "Data saved!");
        }

    }
}
