
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ScheduledNotifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledNotifications
{
    class Program
    {
        public static void SendAsync(string from, List<String> toAddresses, string subject, string content, bool ccSender,
                                    IConfiguration configuration)
        {
            Task.Run(() =>
            {
                Send(from, toAddresses, subject, content, ccSender, configuration);
            });
        }


        public static bool Send(string from, List<String> toAddresses, string subject, string content, bool ccSender,
                          IConfiguration configuration)
        {
            try
            {
                var host = configuration["Gmail:Host"];
                var port = int.Parse(configuration["Gmail:Port"]);
                var username = configuration["Gmail:Username"];
                var password = configuration["Gmail:Password"];
                var enable = bool.Parse(configuration["Gmail:SMTP:starttls:enable"]);

                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(username, password)
                };


                foreach (var address in toAddresses)
                {
                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(from);
                    mailMessage.To.Add(address);

                    if (ccSender)
                    {
                        var ccAddress = new MailAddress(from);
                        mailMessage.CC.Add(ccAddress);
                    }
                    mailMessage.Subject = subject;
                    mailMessage.Body = content;
                    mailMessage.IsBodyHtml = true;

                    smtpClient.Send(mailMessage);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static  async Task<bool> NotifyApplicantsIdeasNotSubmitted(GovtechHackathonContext ctx, 
                                                                          IConfiguration configuration)
        {
  
            int currentYear = DateTime.Today.Year;

            var dates = await ctx.Dates.ToListAsync();
            var closingDate = dates.First(x => x.DateName == "Applications Close").Date;
            var daysLeft = (closingDate - DateTime.Today).TotalDays;

            var savedCases = await ctx.CaseInformation.Include("FkUser").
                                                        Include("FkStatus").
                                                        Where(x => x.FkStatus.Name == "saved" && x.Year == currentYear).ToListAsync();

            var body = new StringBuilder();
            body.AppendLine(@"<html><body>You have proposals which have not yet been submitted.<br/>");
            body.AppendLine("You have " + daysLeft + " days left to submit them</body></html>");

            if (savedCases != null && savedCases.Count > 0)
            {
                var applicantEmails = savedCases.Select(x => x.FkUser.EmailAddress).ToList();
               Send(configuration["Gmail:Username"], applicantEmails, "You have not submitted your proposal(s)",
                                    body.ToString(), false, configuration);

            }

            return true;
        }

        static async Task Main(string[] args)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();


                var section = configuration.GetSection("ConnectionStrings:GovtechDBConnection");
                var dbcontext = new GovtechHackathonContext();
               await NotifyApplicantsIdeasNotSubmitted(dbcontext, configuration);


                Console.WriteLine("Hello World!");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}



