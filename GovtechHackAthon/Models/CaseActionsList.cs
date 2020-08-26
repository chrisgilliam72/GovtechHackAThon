using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class CaseActionsList
    {
        public int CaseID { get; set; }
        public int AdjudicatorID { get; set; }
        public List<CaseActionData> Actions { get; set; }

        public bool ViewOnly { get; set; }

        public CaseActionsList()
        {
            ViewOnly = false;
            Actions = new List<CaseActionData>();
        }
    }
}
