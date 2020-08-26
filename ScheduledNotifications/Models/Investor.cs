using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class Investor
    {
        public Investor()
        {
            CaseInvestorInterested = new HashSet<CaseInvestorInterested>();
        }

        public int PkId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public int FkProvinceId { get; set; }
        public int FkIndustryId { get; set; }
        public bool AcceptedTerms { get; set; }
        public bool Approved { get; set; }
        public bool Active { get; set; }

        public virtual InvestorIndustry FkIndustry { get; set; }
        public virtual Provinces FkProvince { get; set; }
        public virtual ICollection<CaseInvestorInterested> CaseInvestorInterested { get; set; }
    }
}
