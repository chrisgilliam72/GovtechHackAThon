using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class ApplicantInfo
    {
        public ApplicantData RegistrationData { get; set; }
        public Case Case { get; set; }

        public RegistrationPurpose Purpose { get; set; }

        public String UserRole { get; set; }

        public RegistrationTeam Teams { get; set; }


        public ApplicantInfo()
        {
            Teams = new RegistrationTeam();
        }
    }

}
