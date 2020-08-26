using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GovtechHackAthon.Helpers
{
    public class EmailConfirmation
    {
        public static void  SendEmailConfirmationLink (IConfiguration configuration, String confirmationLink,string email,String tempPassword)
        {
            var body = new StringBuilder();
            if (tempPassword!=null)
            {
                body.AppendLine(@"Your temporary password is : <br/>");
                body.AppendLine(tempPassword);
                body.AppendLine("<br/>");
            }

            body.AppendLine(@"Please verify your email by clicking on the link below:<br/>");
            body.AppendLine("<a href = \"" + confirmationLink + "\">Confirm</a></body></html>");
            //var body = "<html><body>Please verify your email by clicking on the link below:<br/>";
            //body += @"<a href = """ + confirmationLink+ @"""></a></body></html>";

            var mailHelper = new MailHelper(configuration);
            mailHelper.SendAsync(configuration["Gmail:Username"], new List<string>() { email }, "SITA Hackathon Email Confirmation", body.ToString(),false);
        }
    }
}
