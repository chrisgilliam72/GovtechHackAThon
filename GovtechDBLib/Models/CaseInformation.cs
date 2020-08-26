using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class CaseInformation
    {
        public CaseInformation()
        {
            CaseAction = new HashSet<CaseAction>();
            CaseAdjudicatorReferalReason = new HashSet<CaseAdjudicatorReferalReason>();
            CaseAdminApproved = new HashSet<CaseAdminApproved>();
            CaseAssignments = new HashSet<CaseAssignments>();
            CaseCategoryScore = new HashSet<CaseCategoryScore>();
            CaseComment = new HashSet<CaseComment>();
            CaseInvestorInterested = new HashSet<CaseInvestorInterested>();
            CaseOutcomes = new HashSet<CaseOutcomes>();
            TeamMember = new HashSet<TeamMember>();
            UserComments = new HashSet<UserComments>();
        }

        public int PkId { get; set; }
        public int FkUserId { get; set; }
        public string Name { get; set; }
        public int FkSituationOptionId { get; set; }
        public int Outcomes { get; set; }
        public DateTime SubmittedDate { get; set; }
        public bool ForGovernment { get; set; }
        public int Year { get; set; }
        public int FkSituationStatementId { get; set; }
        public int FkStatusId { get; set; }
        public int? FkPurposeId { get; set; }
        public int? FkBusinessIdeaId { get; set; }
        public int? FkCompanyId { get; set; }
        public int? FkStudentInfoId { get; set; }
        public bool IsAdjudicatorSubmitted { get; set; }
        public int? FkCategoryId { get; set; }
        public int? LastModified { get; set; }

        public virtual BusinessIdea FkBusinessIdea { get; set; }
        public virtual CaseCategory FkCategory { get; set; }
        public virtual Company FkCompany { get; set; }
        public virtual Purpose FkPurpose { get; set; }
        public virtual SituationOptions FkSituationOption { get; set; }
        public virtual SituationStatements FkSituationStatement { get; set; }
        public virtual CaseStatus FkStatus { get; set; }
        public virtual StudentDetails FkStudentInfo { get; set; }
        public virtual Applicant FkUser { get; set; }
        public virtual ICollection<CaseAction> CaseAction { get; set; }
        public virtual ICollection<CaseAdjudicatorReferalReason> CaseAdjudicatorReferalReason { get; set; }
        public virtual ICollection<CaseAdminApproved> CaseAdminApproved { get; set; }
        public virtual ICollection<CaseAssignments> CaseAssignments { get; set; }
        public virtual ICollection<CaseCategoryScore> CaseCategoryScore { get; set; }
        public virtual ICollection<CaseComment> CaseComment { get; set; }
        public virtual ICollection<CaseInvestorInterested> CaseInvestorInterested { get; set; }
        public virtual ICollection<CaseOutcomes> CaseOutcomes { get; set; }
        public virtual ICollection<TeamMember> TeamMember { get; set; }
        public virtual ICollection<UserComments> UserComments { get; set; }
    }
}
