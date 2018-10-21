using EmployeeApp.Models.EmployeeDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models.ViewModel
{
    public class VMEmployee
    {
      
        public int EmployeeID { get; set; }

        [Display(Name = "Country")]
        public int CountryID { get; set; }

        [Display(Name = "City")]
        public int CityID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Display(Name = "Salary")]
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^\(?([0-9]{0,11})\)?[-. ]?([0-9]{0,11})[-. ]?([0-9]{0,11})$", ErrorMessage = "Not valid salary")]
        public string Salary { get; set; }


        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        public List<Country> CountryList { get; set; }
        public List<City> CityList { get; set; }
    }
}