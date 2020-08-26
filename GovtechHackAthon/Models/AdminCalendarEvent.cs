using GovtechDBLib.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class AdminCalendarEvent
    {
        public int StartEventID { get; set; } 
        public int EndEventID { get; set; }
        public int StageNo { get; set; }
        public String Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateStart { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateEnd { get; set; }
        
        public void SetEvent(IGrouping<int, Dates> groupStage)
        {
            StartEventID = groupStage.OrderBy(x => x.Date).FirstOrDefault().PkId;
            EndEventID = groupStage.OrderByDescending(x => x.Date).FirstOrDefault().PkId;
            Name = groupStage.OrderBy(x => x.Date).FirstOrDefault().StageName;
            StageNo = groupStage.OrderBy(x => x.Date).FirstOrDefault().Stage;
            DateStart = groupStage.OrderBy(x => x.Date).FirstOrDefault().Date;
            DateEnd= groupStage.OrderByDescending(x => x.Date).FirstOrDefault().Date;

        }

        public Dates[] SplitDates()
        {
            var datesArray = new Dates[2];
            datesArray[0].PkId = StartEventID;
            datesArray[0].Date = DateStart;
            datesArray[0].StageName = Name;
            datesArray[0].Stage = StageNo;

            datesArray[1].PkId = EndEventID;
            datesArray[1].Date = DateEnd;
            datesArray[1].StageName = Name;
            datesArray[1].Stage = StageNo;

            return datesArray;
        }
    }
}
