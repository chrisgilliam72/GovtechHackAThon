using GovtechDBLib.Models;
using GovtechHackAthon.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Services
{
    public class DBAPI
    {
        private readonly GovtechHackathonContext govtechHackathonContext;

        public DBAPI(GovtechHackathonContext govtechHackathonContext)
        {
            this.govtechHackathonContext = govtechHackathonContext;
        }

        public IEnumerable<UserDetails> GetUserList()
        {
            var userList = new UserList();
            var ctx = govtechHackathonContext;
            var dbUser = ctx.User.Include("FkProvince").Include("FkUserLevel").ToList();
            return dbUser.Select(x => (UserDetails)x).ToList();
        }

        public async Task<IEnumerable<UserDetails>> GetUserListAsync()
        {
           
            var ctx = govtechHackathonContext;
            var dbUser = await ctx.User.Include("FkProvince").Include("FkUserLevel").ToListAsync();
            return dbUser.Select(x => (UserDetails)x).ToList();
        }

        public IEnumerable<UserDetails> GetAllUsers()
        {
            var ctx = govtechHackathonContext;
            var dbUsers = ctx.User.Where(x => x.IsActive).ToList();
            var lstUsers = dbUsers.Select(x => (UserDetails)x).ToList();
            return lstUsers;
        }

        public IEnumerable<UserDetails> GetUnnassignedUsers()
        {
            var ctx = govtechHackathonContext;
            var dbUsers = ctx.User.Include("UsersInGroup").Where(x => x.IsActive && x.UsersInGroup.Count==0).ToList();
            var lstUsers = dbUsers.Select(x => (UserDetails)x).ToList();
            return lstUsers;
        }

        public IEnumerable<GroupItem> GetAllGroups()
        {
            var ctx = govtechHackathonContext;
            var dbUsers = ctx.Group.Where(x => x.Active).ToList();
            var lstGrps = dbUsers.Select(x => (GroupItem)x).ToList();
            return lstGrps;
        }


       public IEnumerable<TeamMemberData> GetTeamMembers()
        {
            var ctx = govtechHackathonContext;
            var dbTeamList = ctx.TeamMember.Include("FkRace").
                                Include("FkGender").
                                Include("FkCountry").ToList();
            var list = dbTeamList.Select(x => (TeamMemberData)x).ToList();
            return list;
        }
    }
}
