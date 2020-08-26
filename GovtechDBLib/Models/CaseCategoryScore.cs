using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class CaseCategoryScore
    {
        public int PkId { get; set; }
        public int Score { get; set; }
        public int Round { get; set; }
        public int FkCategoryId { get; set; }
        public int FkCaseId { get; set; }
        public int FkUserId { get; set; }
        public bool Submitted { get; set; }

        public virtual CaseInformation FkCase { get; set; }
        public virtual ScoringCategories FkCategory { get; set; }
        public virtual User FkUser { get; set; }
    }
}
