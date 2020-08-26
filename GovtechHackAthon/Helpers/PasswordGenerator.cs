using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GovtechHackAthon.Helpers
{
    public class PasswordGenerator
    {

    
        public static string GeneratePassword(PasswordOptions passwordOptions)
        {
           
            int length = passwordOptions.RequiredLength;
            bool nonAlphanumeric = passwordOptions.RequireNonAlphanumeric;
            bool digit = passwordOptions.RequireDigit;
            bool lowercase = passwordOptions.RequireLowercase;
            bool uppercase = passwordOptions.RequireUppercase;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }
    }
}
