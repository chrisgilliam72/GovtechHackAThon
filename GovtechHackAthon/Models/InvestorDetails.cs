using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class InvestorDetails
    {
        public int ID { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Surname { get; set; }
        public String FullName
        {
            get
            {
                return Name + " " + Surname;
            }
        }

        public bool IsNew { get; set; }

        [Required]
        public String Password { get; set; }

        // Code to compare password field
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirm password fields do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public String Email { get; set; }
        [Required]
        [DisplayName("Contact number")]
        public String Phone { get; set; }
        [Required]
        [DisplayName("Company Name")]
        public String Company { get; set; }
        [Required]
        public String City { get; set; }
        [Required(ErrorMessage = "Province is required")]
        public int ProvinceID { get; set; }
        public String Province { get; set; }
        [Required(ErrorMessage = "Industry is required")]
        public int IndustryID { get; set; }
        public String Industry { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public List<String> AuthErrors { get; set; }

        public bool AcceptedTermsConditions { get; set; }

        public bool ShowUpdateSuccessful { get; set; }

        public InvestorDetails()
        {
            IsNew = true;
            ShowUpdateSuccessful = false;
            AuthErrors =new List<String>();
            Password = "password";
            ConfirmPassword = "password";
        }

        public static explicit operator InvestorDetails (Investor dbInvestor)
        {
            var investor = new InvestorDetails()
            {
                ID = dbInvestor.PkId,
                AcceptedTermsConditions=dbInvestor.AcceptedTerms,
                Name = dbInvestor.Name,
                Surname = dbInvestor.Surname,
                City = dbInvestor.City,
                Email = dbInvestor.Email,
                IsApproved = dbInvestor.Approved,
                IsActive = dbInvestor.Active,
                Phone = dbInvestor.Phone,
                Company = dbInvestor.Company,
                IndustryID = dbInvestor.FkIndustry.PkId,
                Industry = dbInvestor.FkIndustry.Description,
                Province = dbInvestor.FkProvince.Name,
                ProvinceID = dbInvestor.FkProvince.PkId,
                IsNew = false,
            };

            return investor;
        }

    }

}
