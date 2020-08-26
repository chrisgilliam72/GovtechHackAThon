using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class Scoreboard
    {
        public List<ScoreboardItem> Items { get; set; }

        public void Rank()
        {
            foreach (var item in Items)
                item.Total = item.Scores.Sum(x => x.Score);

            Items = Items.OrderByDescending(x => x.Total).ToList();
            int rank = 1;
            int lastTotal = 0;
            foreach (var item in Items)
            {
                if (item.Total != lastTotal)
                {
                    lastTotal = item.Total;
                    item.Rank = rank++;
                }
                else
                    item.Rank = rank;

            }

        }

        public Scoreboard()
        {
            Items = new List<ScoreboardItem>(); 
        }
    }
}
