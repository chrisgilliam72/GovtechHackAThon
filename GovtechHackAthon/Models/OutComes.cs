using GovtechDBLib.Models;
using GovtechHackAthon.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class RegistrationOutcomes
    {
        public String TmpText { get; set; }
        public Dictionary<Outcomes, bool> OutComeSelections { get; set; }

        public bool AtLeastOneOutcomeSelected
        {
            get
            {
                var selected = OutComeSelections.Where(x => x.Value).ToList();

                return selected.Count > 0;
            }
        }

        public RegistrationOutcomes()
        {
            OutComeSelections = new Dictionary<Outcomes, bool>();
        }
    }
}
