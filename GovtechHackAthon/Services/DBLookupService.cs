using GovtechDBLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Services
{
    public class DBLookupService
    {
        private readonly GovtechHackathonContext govtechHackathonContext;

        public DBLookupService(GovtechHackathonContext govtechHackathonContext)
        {
            this.govtechHackathonContext = govtechHackathonContext;
        }

        public IEnumerable<CaseCategory> GetCategories()
        {
            var ctx = govtechHackathonContext;
            return ctx.CaseCategory.OrderBy(x => x.Name).ToList();
        }
        public IEnumerable<ScoringCategories> GetScoringCategories()
        {
            var ctx = govtechHackathonContext;
            return ctx.ScoringCategories.Include("FkType").OrderBy(x=>x.PkId).ToList();
        }


        public IEnumerable<InvestorIndustry> GetInvestorIndustries()
        {
            var ctx = govtechHackathonContext;
            return ctx.InvestorIndustry.OrderBy(x => x.Description);
        }

        public IEnumerable<Genders> GetGenders()
        {
            var ctx = govtechHackathonContext;
            using (ctx)
            {
                return ctx.Genders.ToList();
            }

        }

        public IEnumerable<Provinces> GetProvinces()
        {
            var ctx = govtechHackathonContext;
            using (ctx)
            {
                var provinces = ctx.Provinces.OrderBy(x => x.Name).ToList();
                var NAProvince = provinces.FirstOrDefault(x => x.Name == "N/A");
                if (NAProvince!=null)
                {
                    provinces.Remove(NAProvince);
                    provinces.Add(NAProvince);
                }

                return provinces;
            }
        }

        public IEnumerable<Race> GetRaces()
        {
            var ctx = govtechHackathonContext;
            using (ctx)
            {
                return ctx.Race.OrderBy(x => x.Name).ToList();
            }
        }

        public IEnumerable<SituationOptions> GetSituationOptions()
        {
            var ctx = govtechHackathonContext;
            using (ctx)
            {
                return ctx.SituationOptions.ToList();
            }
        }

        public IEnumerable<SituationStatements> GetSituationStatements()
        {
            var ctx =  govtechHackathonContext;
            using (ctx)
            {
                return ctx.SituationStatements.ToList();
            }
        }

        public IEnumerable<Outcomes> GetOutcomes()
        {
            var ctx = govtechHackathonContext;
            using (ctx)
            {
                return ctx.Outcomes.ToList();
            }
        }

        public IEnumerable<Languages> GetLanguages()
        {
            var ctx = govtechHackathonContext;
            using (ctx)
            {
                return ctx.Languages.OrderBy(x=>x.Language).ToList();
            }
        }

        public IEnumerable<Countries> GetCountries()
        {
            var ctx = govtechHackathonContext;
            using (ctx)
            {
                return ctx.Countries.OrderBy(x => x.Name).ToList();
            }
        }

        public IEnumerable<UserLevel> GetUserLevels()
        {
            return govtechHackathonContext.UserLevel.ToList(); ;
        }
    }
}
