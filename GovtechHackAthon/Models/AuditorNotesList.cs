using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class AuditorNotesList
    {
        public List<AuditorNoteItem> Notes { get; set; }

        public AuditorNotesList()
        {
            Notes = new List<AuditorNoteItem>();
        }
    }
}
