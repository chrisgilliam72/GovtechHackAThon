using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Helpers
{
    public class CountryList
    {
        private static List<String> _countryLst;
        public static List<String> ListCountries
        {
            get
            {
                if (_countryLst==null)
                    _countryLst = BuildCountryList().OrderBy(x => x).ToList();
                return _countryLst;
            }
        }

        public static List<String> Provinces
        {
            get
            {
                return new List<String> { "Eastern Cape", "Free State", "Gauteng", "KZN", "Limpopo", "Mpumalanga",
                                          "Northern Cape", "North West", "Western Cape" };
            }
        }

        public static List<string> BuildCountryList()
        {
            //create a new Generic list to hold the country names returned
            List<string> cultureList = new List<string>();

            //create an array of CultureInfo to hold all the cultures found, these include the users local cluture, and all the
            //cultures installed with the .Net Framework
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);

            //loop through all the cultures found
            foreach (CultureInfo culture in cultures)
            {
                try
                {
                    //pass the current culture's Locale ID (http://msdn.microsoft.com/en-us/library/0h88fahh.aspx)
                    //to the RegionInfo contructor to gain access to the information for that culture
                    RegionInfo region = new RegionInfo(culture.LCID);

                    //make sure out generic list doesnt already
                    //contain this country
                    if (!(cultureList.Contains(region.EnglishName)))
                        //not there so add the EnglishName (http://msdn.microsoft.com/en-us/library/system.globalization.regioninfo.englishname.aspx)
                        //value to our generic list
                        cultureList.Add(region.EnglishName);
                }
                catch (Exception)
                {

                }
            }
            return cultureList;
        }
    }
}
