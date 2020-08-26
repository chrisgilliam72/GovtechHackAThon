using GovtechDBLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class AdminCalendar
    {
        public bool ShowUpdateSuccess { get; set; }
        public String Year
        {
            get
            {
                return DateTime.Today.Year.ToString();
            }
        }

        public List<AdminCalendarEvent> Events { get; set; }


        public AdminCalendar()
        {
            Events = new List<AdminCalendarEvent>();
        }

        public async Task Load(GovtechHackathonContext ctx)
        {
            var datesLst = await ctx.Dates.ToListAsync();
            var stages = datesLst.GroupBy(x => x.Stage).ToList();
            foreach (var stage in stages)
            {
                var calendarEvent = new AdminCalendarEvent();
                calendarEvent.SetEvent(stage);
                Events.Add(calendarEvent);
            }
        }
    }
}
