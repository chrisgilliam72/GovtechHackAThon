using GovtechDBLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{

    public class CaseDashboard
    {
        [Required]
        public int OwnerID { get; set; }
        public Case CaseInfo { get; set; }
        public List<TeamMemberData> TeamMembers { get; set; }
        public RegistrationPurpose Purpose { get; set; }
        public RegistrationBusinessIdea Idea { get; set; }
        public List<String> Outcomes { get; set; }
        public bool CanSubmit { get; set; }
        public bool ViewOnlyMode { get; set; }
        public bool ReadOnly
        {
            get
            {
                return ((CaseInfo.Status != "Saved"  && CaseInfo.Status!="Deferred") || ViewOnlyMode);
            }
        }

        public bool IsTeamMember (String applicantEmail)
        {
            return TeamMembers.Any(x => x.Email == applicantEmail);
        }

        public bool IsDefered { get; set; }
        public String DeferalReason { get; set; }
        public DateTime DeferedUntil { get; set; }

        public bool CollapseTeamMembers { get; set; }
        public bool CollapsePurpose { get; set; }
        public bool CollapseIdea { get; set; }

        public CaseDashboard()
        {
            CaseInfo = new Case();
            Purpose = new RegistrationPurpose();
            Idea = new RegistrationBusinessIdea();
            TeamMembers = new List<TeamMemberData>();
            Outcomes = new List<String>();
            ViewOnlyMode = false;
            CollapseTeamMembers = true;
            CollapsePurpose = true;
            CollapseIdea = true;
        }

    }
}
