using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class AllCasesList
    {
        public List<CaseAssignmentEntry> CaseAssignments {get;set;}

        public AllCasesList()
        {
            CaseAssignments = new List<CaseAssignmentEntry>();
    }
    }

 
}
