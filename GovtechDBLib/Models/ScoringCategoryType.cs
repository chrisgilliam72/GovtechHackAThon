using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class ScoringCategoryType
    {
        public ScoringCategoryType()
        {
            ScoringCategories = new HashSet<ScoringCategories>();
        }

        public int PkId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ScoringCategories> ScoringCategories { get; set; }
    }
}
