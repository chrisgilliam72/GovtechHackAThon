using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class CaseActionData
    {
        public int ID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        public String Action { get; set; }
        public String Adjudicator { get; set; }

        public int CaseID { get; set; }
        public int AdjudicatorID { get; set; }

        public CaseActionData(int caseID, int adjudicatorID)
        {
            CaseID = caseID;
            AdjudicatorID = adjudicatorID;
            Date = DateTime.Today;
        }
        public CaseActionData()
        {
            Date = DateTime.Today;
        }

        public static explicit operator CaseActionData(CaseAction dbcaseAction)
        {
            var caseActionData = new CaseActionData()
            {
                ID=dbcaseAction.PkId,
                Date=dbcaseAction.ActionDate,
                Action= dbcaseAction.Description,
                CaseID= dbcaseAction.FkCaseId,
                AdjudicatorID= dbcaseAction.FkAdjudicatorId,
                Adjudicator = dbcaseAction.FkAdjudicator!=null  ? dbcaseAction.FkAdjudicator.FirstName+" "+dbcaseAction.FkAdjudicator.Surname : ""
            };

            return caseActionData;
        }
    }
}
