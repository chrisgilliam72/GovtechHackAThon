using System;
using System.Collections.Generic;

namespace ScheduledNotifications.Models
{
    public partial class TeamMember
    {
        public int PkId { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Identity { get; set; }
        public string AccessLevel { get; set; }
        public string City { get; set; }
        public decimal? ZipCode { get; set; }
        public int? FkProvinceId { get; set; }
        public int FkCaseId { get; set; }
        public int FkRaceId { get; set; }
        public int FkGenderId { get; set; }
        public int FkCountryId { get; set; }
        public bool IsStudent { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public virtual CaseInformation FkCase { get; set; }
        public virtual Countries FkCountry { get; set; }
        public virtual Genders FkGender { get; set; }
        public virtual Provinces FkProvince { get; set; }
        public virtual Race FkRace { get; set; }
    }
}
