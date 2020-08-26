using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{

    public class UserSelectionItem
    {
        public int UserID { get; set; }
        public String DisplayName { get; set; }
        public bool Selected { get; set; }
    }
}
