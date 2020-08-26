using GovtechDBLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class InvestorsTable
    {
        public List<InvestorDetails> InvestorList { get; set; }

        public bool UpdateSuccessful { get; set; }

        public async Task GetAllInvestors(GovtechHackathonContext ctx)
        {
            var dbInvestors = await ctx.Investor.Include("FkIndustry").Include("FkProvince").ToListAsync();
            InvestorList = dbInvestors.Select(x => (InvestorDetails)x).ToList();
        }

        public InvestorsTable()
        {
            UpdateSuccessful = false;
            InvestorList = new List<InvestorDetails>();
        }
    }
}
