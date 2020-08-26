using GovtechDBLib.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class UserDetails
    {
        public int UserID { get; set; }
        [Required]
        public String FirstName { get;set; }
        [Required]
        public String Surname { get; set; }
        [Required]
        [Remote(action: "VerifyEmailDoesntExist", controller: "Admin", AdditionalFields = "UserID")]
        public String Email { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Contact No must be numeric")]
        public String ContactNo { get; set; }
        [Required]
        public String City { get; set; }
        [Required(ErrorMessage = "Province is required")]
        public int fkProvinceID { get; set; }
        public String Province { get; set; }
        [Required(ErrorMessage = "User level is required")]
        public int fkUserLevelID { get; set; }
        public String UserLevel { get; set; }
        public bool CanEditCases { get; set; }
        public bool CanAssignCases { get; set; }
        public List<int> Groups { get; set; }
        public bool IsActive { get; set; }
        public String FullName
        {
            get
            {
                return FirstName+ " " + Surname;
            }
        }

        public String CanEditCasesDispay
        {
            get
            {
                return CanEditCases ? "Yes" : "No";
            }
        }


        public String CanAssignCasesDisplay
        {
            get
            {
                return CanAssignCases ? "Yes" : "No";
            }
        }


        public UserDetails()
        {
            Groups = new List<int>();
        }

        public static explicit operator UserDetails(User dbUser)
        {
            var userDetails = new UserDetails()
            {
                UserID = dbUser.PkId,
                FirstName = dbUser.FirstName,
                Surname = dbUser.Surname,
                Email = dbUser.Email,
                City = dbUser.City,
                fkProvinceID = dbUser.FkProvinceId,
                Province = dbUser.FkProvince != null ? dbUser.FkProvince.Name : "",
                fkUserLevelID = dbUser.FkUserLevelId,
                UserLevel = dbUser.FkUserLevel != null ? dbUser.FkUserLevel.Name : "",
                ContactNo = dbUser.ContactNo,
                CanEditCases=dbUser.CanEditCases,
                CanAssignCases=dbUser.CanAssignCases,
                IsActive=dbUser.IsActive
            };

            if (dbUser.UsersInGroup!=null)
            {
                var grps = dbUser.UsersInGroup.Select(x => x.FkGroupId).ToList();
                userDetails.Groups.AddRange(grps);
            }

            return userDetails;
        }

        public static explicit operator User(UserDetails userDetails)
        {
            var user = new User()
            {
                FirstName = userDetails.FirstName,
                Surname = userDetails.Surname,
                ContactNo = userDetails.ContactNo,
                Email = userDetails.Email,
                City = userDetails.City,
                FkProvinceId = userDetails.fkProvinceID,
                FkUserLevelId = userDetails.fkUserLevelID,
                CanAssignCases = userDetails.CanAssignCases,
                CanEditCases = userDetails.CanEditCases,
                IsActive= userDetails.IsActive
            };

            return user;
        }
    }


}
