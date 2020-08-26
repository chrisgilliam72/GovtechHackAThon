using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class Company
    {
        public Company()
        {
            CaseInformation = new HashSet<CaseInformation>();
        }

        public int PkId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int FkProvinceId { get; set; }

        public virtual Provinces FkProvince { get; set; }
        public virtual ICollection<CaseInformation> CaseInformation { get; set; }
    }
}
