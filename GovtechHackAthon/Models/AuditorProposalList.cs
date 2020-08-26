using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class AuditorProposalList
    {
        public int AuditorID { get; set; }
        public List<AuditorProposaListItem> Proposals { get; set; }

        public AuditorNotesList NotesList { get; set; }
        public List<AuditorProposaListItem> RankedProposals
        {
            get
            {
                return Proposals.OrderByDescending(x => x.TotalTotal).ToList();
            }

        }

        public AuditorProposalList()
        {
            Proposals = new List<AuditorProposaListItem>();
            NotesList = new AuditorNotesList();
        }
    }
}
