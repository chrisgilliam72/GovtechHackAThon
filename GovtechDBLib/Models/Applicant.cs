using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class Applicant
    {
        public Applicant()
        {
            CaseInformation = new HashSet<CaseInformation>();
        }

        public int PkId { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Idnumber { get; set; }
        public string City { get; set; }
        public int FkProvinceId { get; set; }
        public int FkGenderId { get; set; }
        public bool AgreedToTerms { get; set; }
        public int FkHomeLanguageId { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string LinkedIn { get; set; }
        public string Position { get; set; }
        public byte[] ProfileImage { get; set; }
        public string Company { get; set; }
        public string ProfileImageType { get; set; }
        public int? FkRaceId { get; set; }
        public string Twitter { get; set; }

        public virtual Languages FkHomeLanguage { get; set; }
        public virtual Race FkRace { get; set; }
        public virtual ICollection<CaseInformation> CaseInformation { get; set; }
    }
}
