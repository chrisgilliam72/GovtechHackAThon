using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GovtechHackAthon.Helpers
{
    public class MailHelper
    {
        private IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendAsync(string from, List<String> toAddresses, string subject, string content, bool ccSender)
        {
            Task.Run(() =>
            {
                Send(from, toAddresses, subject, content,ccSender);
            });
        }


        public bool Send(string from, List<String> toAddresses, string subject, string content, bool ccSender)
        {
            try
            {
                var host = _configuration["Gmail:Host"];
                var port = int.Parse(_configuration["Gmail:Port"]);
                var username = _configuration["Gmail:Username"];
                var password = _configuration["Gmail:Password"];
                var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);

                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(username, password)
                };

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(from);
                foreach (var address in toAddresses)
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

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
