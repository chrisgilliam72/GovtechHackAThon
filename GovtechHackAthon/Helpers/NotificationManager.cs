using GovtechDBLib.Models;
using GovtechHackAthon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GovtechHackAthon.Helpers
{
    public class NotificationManager
    {
        private readonly GovtechHackathonContext _govtechHackathonContext;
        private readonly IConfiguration _configuration;

        public NotificationManager(GovtechHackathonContext govtechHackathonContext,
                                    IConfiguration configuration)
        {
            _govtechHackathonContext = govtechHackathonContext;
            _configuration = configuration;
        }

        public async Task<bool> NotifyApplicantsIdeasNotSubmitted()
        {
            var ctx = _govtechHackathonContext;
            int currentYear = DateTime.Today.Year;
            var mailHelper = new MailHelper(_configuration);

            var savedCases = await ctx.CaseInformation.Include("FkUser").
                                                        Include("FkStatus").
                                                        Where(x => x.FkStatus.Name == "saved" && x.Year == currentYear).ToListAsync();
            if (savedCases!=null && savedCases.Count>0)
            {
                var applicantEmails = savedCases.Select(x => x.FkUser.EmailAddress).ToList();
                mailHelper.Send(_configuration["Gmail:Username"], applicantEmails, "You have not submitted your proposal(s)","You have proposals which have not yet been submitted.", false) ;
                
            }

            return true;
        }

        public async Task<bool> NotifyNewTeammembers(int caseID)
        {
            var ctx = _govtechHackathonContext;
            var mailHelper = new MailHelper(_configuration);
            var caseInfo = await ctx.CaseInformation.Include("FkUser").FirstAsync(x => x.PkId == caseID);
            var teamMembers = await ctx.TeamMember.Where(x => x.FkCaseId == caseID).ToListAsync();

            var teamLeader = caseInfo.FkUser.FirstName + " " + caseInfo.FkUser.LastName;
           
            var leaderEmail = caseInfo.FkUser.EmailAddress;
            var teamEmails = teamMembers.Select(x => x.EmailAddress).ToList();

            var subject = teamLeader + " has added you as team member";
            var body = new StringBuilder();
            body.AppendLine(@"<html><body>" + teamLeader + "(" + leaderEmail + ")" + " has added you as a team member to his proposal <br/>");
            body.AppendLine(caseInfo.Name+" ID=("+ caseInfo.PkId+ @")</body></html>");

            mailHelper.Send(_configuration["Gmail:Username"], teamEmails, subject, body.ToString(), false);

            return true;
        }

        public async Task<bool> NotifyTeam(int caseID, String subject, String message )
        {
            var ctx = _govtechHackathonContext;
            var mailHelper = new MailHelper(_configuration);
            var caseInfo = await ctx.CaseInformation.Include("FkUser").FirstAsync(x => x.PkId == caseID);
            var teamMembers = await ctx.TeamMember.Where(x => x.FkCaseId == caseID).ToListAsync();

            var leaderEmail = caseInfo.FkUser.EmailAddress;
            var teamEmails = teamMembers.Select(x => x.EmailAddress).ToList();

            mailHelper.Send(_configuration["Gmail:Username"], teamEmails, subject, message, false);
            return true;
        }
    }
}
