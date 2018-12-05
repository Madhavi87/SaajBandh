using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProfileApp.Models
{
    public class UserRegVM
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please Enter FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter MiddleName")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

      
        public string Password { get; set; }


        public string BirthDateDisplay { get; set; }
        public string ConfPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile No")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Please Select Terms and Conditions")]
        public bool TermsAndConditions { get; set; }

        //[Required(ErrorMessage = "Please Select Country")]
        //public int CountryID { get; set; }

        //[Required(ErrorMessage = "Please Select State")]
        //public int StateID { get; set; }

        //[Required(ErrorMessage = "Please Select City")]
        //public int CityID { get; set; }

        //[Required(ErrorMessage = "Please Select City")]
        [Required(ErrorMessage = "Please Select Gender")]
        public int Gender { get; set; }

        //   [Required(ErrorMessage = "Please Select Caste")]
        public string Caste { get; set; }

        public string SubCaste { get; set; }
        public string City { get; set; }

        public string ErrMsg { get; set; }

        public string MyAppId { get; set; }
    }
}