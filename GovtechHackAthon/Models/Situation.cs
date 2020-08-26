using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class SituationOption
    {
        public String Name { get; set; }

        public bool IsTrue { get; set; }

        public String DisplayID{ get; set; }

        public static explicit operator SituationOption(SituationOptions dbSituation)
        {
            var situationOption = new SituationOption()
            {
                Name = dbSituation.DisplayString,
                DisplayID = dbSituation.VisualId,
            };

            return situationOption;
        }
    }
}
