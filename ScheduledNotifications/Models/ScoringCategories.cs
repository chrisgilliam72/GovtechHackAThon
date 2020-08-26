using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class ScoringCategories
    {
        public ScoringCategories()
        {
            CaseCategoryScore = new HashSet<CaseCategoryScore>();
        }

        public int PkId { get; set; }
        public string Description { get; set; }
        public int MaxScore { get; set; }

        public virtual ICollection<CaseCategoryScore> CaseCategoryScore { get; set; }
    }
}
