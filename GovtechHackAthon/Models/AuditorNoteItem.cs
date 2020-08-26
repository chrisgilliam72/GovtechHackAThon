using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class AuditorNoteItem
    {

        public int AuditorID { get; set; }
        [Required]
        public String Auditor { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public String Date { get; set; }
        [Required]
        public String Note { get; set; }

        public static explicit operator AuditorNoteItem (AuditorNote auditNote)
        {
            var noteItem = new AuditorNoteItem();
            noteItem.Auditor = auditNote.FkAuditor.FirstName + " " + auditNote.FkAuditor.Surname;
            noteItem.Date = auditNote.NoteDate.ToShortDateString();
            noteItem.Note = auditNote.Note;
            return noteItem;
        }
    }
}
