using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class User
    {
        public User()
        {
            CaseAction = new HashSet<CaseAction>();
            CaseCategoryScore = new HashSet<CaseCategoryScore>();
            CaseComment = new HashSet<CaseComment>();
            UserComments = new HashSet<UserComments>();
            UsersInGroup = new HashSet<UsersInGroup>();
        }

        public int PkId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int FkProvinceId { get; set; }
        public string ContactNo { get; set; }
        public string City { get; set; }
        public bool CanAssignCases { get; set; }
        public bool CanEditCases { get; set; }
        public int FkUserLevelId { get; set; }
        public bool IsActive { get; set; }

        public virtual Provinces FkProvince { get; set; }
        public virtual UserLevel FkUserLevel { get; set; }
        public virtual ICollection<CaseAction> CaseAction { get; set; }
        public virtual ICollection<CaseCategoryScore> CaseCategoryScore { get; set; }
        public virtual ICollection<CaseComment> CaseComment { get; set; }
        public virtual ICollection<UserComments> UserComments { get; set; }
        public virtual ICollection<UsersInGroup> UsersInGroup { get; set; }
    }
}
