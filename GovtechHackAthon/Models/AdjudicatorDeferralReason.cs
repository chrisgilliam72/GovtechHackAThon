using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class AdjudicatorDeferralReasonData
    {
        public int CaseID { get; set; }

        [Required]
        public String Reason { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ResubmissionDate { get; set; }


        public AdjudicatorDeferralReasonData()
        {
            ResubmissionDate = DateTime.Today;
        }
        public AdjudicatorDeferralReasonData(int caseID)
        {
            CaseID = caseID;
        }
    }
}
