using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class CompanyDetails
    {
        [Required]
        [DisplayName("Company Name")]
        public String Name { get; set; }
        [Required]
        public String ZipCode { get; set; }
        [Required]
        public int ProvinceID { get; set; }
        [Required]
        public String City { get; set; }

        public String Province { get; set; }
    }
}
