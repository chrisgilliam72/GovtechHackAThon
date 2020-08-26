//using GovtechHackAthon.Models.EFModels;
using GovtechDBLib.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class ApplicantData
    {
        public int UserID { get; set; }
        [Required]
        [Remote(action: "VerifyEmailDoesntExist", controller: "Registration", AdditionalFields = "UserID")]
        virtual public String Email { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public String Surname { get; set; }
        [Required]
        [Remote(action: "VerifyIDDoesntExist", controller: "Registration")]
        [StringLength(13, MinimumLength = 13, ErrorMessage ="ID must be 13 digits long")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "ID must be numeric")]
        public String ID { get; set; }
        [Required]
        public String ContactNumber { get; set; }
        
        public String Password { get; set; }

        // Code to compare password field
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirm password fields do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Language is required")]
        public int LanguageID { get; set; }
        [Required(ErrorMessage ="Race is required")]
        public int RaceID { get; set; }
        [Required (ErrorMessage = "Gender is required")]
        public int GenderID { get; set; }
        [Required]
        public String City { get; set; }
        [Required (ErrorMessage ="Province is required")]
        public int ProvinceID { get; set; }
        public String Province { get; set; }
        public String Gender { get; set; }
        public String Race { get; set; }
        public String Country { get; set; }
        [Display(Name="Date of Birth")]
        public String DateOfBirth
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

                    if (monthPart<13 && monthPart>0 && dayPart>0)
                    {
                        int daysInMonth = DateTime.DaysInMonth(yearPart, monthPart);
                        if (dayPart<=daysInMonth)
                            dateOfBirth = new DateTime(yearPart, monthPart, dayPart);
                        else
                            dateOfBirth = new DateTime(2000, 01, 01);
                    }
                    else
                        dateOfBirth = new DateTime(2000, 01, 01);

                    if (dateOfBirth> DateTime.Today)
                        dateOfBirth = new DateTime(Convert.ToInt32(year) + 1900, monthPart, dayPart);
                    return dateOfBirth.ToShortDateString();
                }
                return "";
            }
        }
        public List<String> AuthErrors { get; set; }
        public String Company { get; set; }
        public String Position { get; set; }
        public String Address { get; set; }
        public String LinkedIn { get; set; }
        public String Twitter { get; set; }
        public String ZipCode { get; set; }
        public byte[] ImageSrc { get; set; }
        public String ImageType { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }
        public bool ShowToast { get; set; }

        public bool NeedsPhoto
        {
            get
            {
                return (ImageFile == null);
            }
        }
        public String FullName
        {
            get
            {
                return FirstName + " " + Surname;
            }
        }

        public bool ViewOnlyMode { get; set; }

        public ApplicantData()
        {
            ViewOnlyMode = false;
            ShowToast = false;
            AuthErrors = new List<string>();
        }
        public ClaimsPrincipal GetClaimsPrincipal()
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", UserID.ToString()),
                new Claim("UserName", FullName)
            };
            var principal = new ClaimsPrincipal();
            principal.AddIdentity(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            return principal;
        }

        public static explicit operator ApplicantData(Applicant dbObject)
        {
            var modelObject = new ApplicantData()
            {
                UserID = dbObject.PkId,
                Email = dbObject.EmailAddress,
                FirstName = dbObject.FirstName,
                Surname = dbObject.LastName,
                ID = dbObject.Idnumber,
                ContactNumber = dbObject.ContactNumber,
                Company = dbObject.Company,
                Position = dbObject.Position,
                LinkedIn = dbObject.LinkedIn,
                Twitter = dbObject.Twitter,
                LanguageID = dbObject.FkHomeLanguageId,
                ProvinceID = dbObject.FkProvinceId,
                GenderID = dbObject.FkGenderId,
                ImageSrc = dbObject.ProfileImage,
                ImageType=dbObject.ProfileImageType,
                Address = dbObject.Address,
                ZipCode = dbObject.ZipCode,
                City = dbObject.City
                
            };


            return modelObject;
        }

        public static explicit operator Applicant(ApplicantData modelObject)
        {
            var dbObject = new Applicant();
            dbObject.PkId = modelObject.UserID;
            dbObject.EmailAddress = modelObject.Email;
            dbObject.FirstName = modelObject.FirstName;
            dbObject.LastName = modelObject.Surname;
            dbObject.Idnumber = modelObject.ID;
            var genderpart = modelObject.ID.Substring(6, 4);
            var genderID = Convert.ToInt32(genderpart);
            dbObject.FkGenderId = genderID >=5000 ? 1 : 0;
            dbObject.ContactNumber = modelObject.ContactNumber;
            dbObject.FkHomeLanguageId = modelObject.LanguageID;
            dbObject.FkProvinceId = modelObject.ProvinceID;
            dbObject.FkHomeLanguageId = modelObject.LanguageID;
            dbObject.FkRaceId = modelObject.RaceID;
            dbObject.Company = modelObject.Company;
            dbObject.Position = modelObject.Position;
            dbObject.LinkedIn = modelObject.LinkedIn;
            dbObject.Twitter = modelObject.Twitter;
            dbObject.ProfileImage = modelObject.ImageSrc;
            dbObject.ProfileImageType = modelObject.ImageType;
            dbObject.City = modelObject.City;
            return dbObject;
        }
    }
}
