using GovtechDBLib.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class TeamMemberData 
    {
        public int TeamMemberID { get; set; }
        public int CaseID { get; set; }
        public int Index { get; set; }
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String Surname { get; set; }
        [Required]
        public String PhoneNumber { get; set; }

        [Required]
        public int CountryID { get; set; }
        public String Country { get; set; }

        [Required]
        public int RaceID { get; set; }
        public String Race { get; set; }

        [Required]
        public int GenderID { get; set; }
        public String Gender { get; set; }
        public int ProvinceID { get; set; }
        public String Province { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        //[Remote(action: "VerifyIDDoesntExist1", controller: "Registration")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ID must be 13 digits long")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "ID must be numeric")]
        [Remote(action: "IDUniqueOverTeam", controller: "Registration", AdditionalFields = "TeamMemberID")]
        public String ID { get; set; }
        [Required]
        public String City { get; set; }

        [Required(ErrorMessage ="Email field required")]
        [Remote(action: "TeamEmailCanBeShared", controller: "Registration", AdditionalFields = "IsStudent,TeamMemberID")]
        public String  Email {get;set;}

        public bool IsStudent { get; set; }

        public bool IsNew { get; set; }

        public bool ShowDetails { get; set; }

        public int IDSouthAfrica { get; set; }

        [Display(Name = "Date of Birth")]
        public String DateOfBirthDisplayString 
        {
            get
            {
                if (!String.IsNullOrEmpty(ID))
                {
                    DateTime dateOfBirth;
                    var year = ID.Substring(0, 2);
                    var month = ID.Substring(2, 2);
                    var day = ID.Substring(4, 2);

                    int yearPart = Convert.ToInt32(year) + 2000;
                    int monthPart = Convert.ToInt32(month);
                    int dayPart = Convert.ToInt32(day);

                    if (monthPart < 13 && monthPart > 0 && dayPart > 0)
                    {
                        int daysInMonth = DateTime.DaysInMonth(yearPart, monthPart);
                        if (dayPart <= daysInMonth)
                            dateOfBirth = new DateTime(yearPart, monthPart, dayPart);
                        else
                            dateOfBirth = new DateTime(2000, 01, 01);
                    }
                    else
                        dateOfBirth = new DateTime(2000, 01, 01);

                    if (dateOfBirth > DateTime.Today)
                        dateOfBirth = new DateTime(Convert.ToInt32(year) + 1900, monthPart, dayPart);
                    return dateOfBirth.ToShortDateString();
                }
                return "";
            }
        }

        public String FullName
        {
            get
            {
                return FirstName + " " + Surname;
            }
        }
        public TeamMemberData()
        {
            IsNew = true;
            ShowDetails = false;
            DateOfBirth = new DateTime(1990, 01, 01);
        }

        public TeamMemberData(int caseID)
        {
            IsNew = true;
            CaseID = caseID;
            DateOfBirth = new DateTime(1990, 01, 01);
        }

        public static explicit operator TeamMember (TeamMemberData teamMember)
        {
            var dbTeamMemberData = new TeamMember()
            {
                FkCaseId=teamMember.CaseID,
                EmailAddress = teamMember.Email,
                FirstName = teamMember.FirstName,
                LastName = teamMember.Surname,
                PhoneNumber = teamMember.PhoneNumber,
                City= teamMember.City,
                FkGenderId = teamMember.GenderID,
                FkRaceId = teamMember.RaceID,
                FkProvinceId = teamMember.ProvinceID != 0 ? teamMember.ProvinceID : (int?)null,
                Identity = teamMember.ID,
                FkCountryId = teamMember.CountryID,
                IsStudent = teamMember.IsStudent,
                DateOfBirth= teamMember.DateOfBirth
            };

            return dbTeamMemberData;
        }

        public static explicit operator TeamMemberData(TeamMember dbTeamMember)
        {
            var TeamMemberData = new TeamMemberData()
            {
                TeamMemberID = dbTeamMember.PkId,
                RaceID = dbTeamMember.FkRaceId,
                GenderID = dbTeamMember.FkGenderId,
                FirstName = dbTeamMember.FirstName,
                Surname = dbTeamMember.LastName,
                Email = dbTeamMember.EmailAddress,
                Race = dbTeamMember.FkRace.Name,
                Gender = dbTeamMember.FkGender.Name,
                City = dbTeamMember.City,
                Country = dbTeamMember.FkCountry.Name,
                Province = dbTeamMember.FkProvince != null ? dbTeamMember.FkProvince.Name : "",
                ID = dbTeamMember.Identity,
                CaseID = dbTeamMember.FkCaseId,
                PhoneNumber = dbTeamMember.PhoneNumber,
                IsStudent =dbTeamMember.IsStudent,
                CountryID=dbTeamMember.FkCountryId,
                ProvinceID=dbTeamMember.FkProvinceId ?? 0,
                DateOfBirth=dbTeamMember.DateOfBirth ?? new DateTime(2000,01,01),
                IsNew =false
            };

            return TeamMemberData;
        }


    }
}

