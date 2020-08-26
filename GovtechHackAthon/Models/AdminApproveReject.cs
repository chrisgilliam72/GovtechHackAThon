using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public enum AdminApprovalState { Approved, Rejected, Unevaluated}
    public class AdminApproveReject
    {
        [Required]
        public int CaseID { get; set; }
        [Required]
        public String Comment { get; set; }

        public bool EmailApplicant { get; set; }
        public AdminApprovalState ApprovalState { get; set; }

        public AdminApproveReject()
        {
            ApprovalState = AdminApprovalState.Unevaluated;
            EmailApplicant = true;
        }
    }

}
