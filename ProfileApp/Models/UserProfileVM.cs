using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProfileApp.Models
{
    public class UserProfileVM
    {
        public int UserID { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public int WorkID { get; set; }
        public int Gender { get; set; }
        public int MaritalStatusID { get; set; }
        public string RAddress { get; set; }
        public string ImagePath { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string MyAppID { get; set; }
        public string Password { get; set; }
        public int BloodGroupID { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Please Select State")]
        public int StateID { get; set; }

        [Required(ErrorMessage = "Please Select City")]
        public int CityID { get; set; }
        public int BirthYear { get; set; }
        public string BirthDateDisplay { get; set; }
        public string BirthName { get; set; }
        public string BirthPlace { get; set; }
        public string BirthTime { get; set; }
        public string Gotra { get; set; }
        public int SubCasteID { get; set; }

        public int ComplexionID { get; set; }
        public int HeightID { get; set; }

        public int WeightID { get; set; }
        public int PhysicalChalengeID { get; set; }
        public int SpectacleID { get; set; }

        public int SchoolID { get; set; }
        public string EducationDetail { get; set; }
        public int ServiceBusinessID { get; set; }
        public string ServiceDetail { get; set; }
        public float SalaryMonthly { get; set; }
        public string SalaryAnual { get; set; }

        public string MulGaon { get; set; }
        public int TalukaID { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }

        public string FatherOccupation { get; set; }
        public string FatherContactNo { get; set; }
        public string MamaSurName { get; set; }
        public string MamaPlace { get; set; }

        public string DegreeName { get; set; }
        public string GenderName { get; set; }
        public string MaritalStatusName { get; set; }
        public string SubCasteName { get; set; }
        public string ComplexionName { get; set; }
        public string HeightName { get; set; }
        public string BloodGroupName { get; set; }
        public string PhysicalyChalengedName { get; set; }
        public string SpectacleName { get; set; }
        public string CarrerTypeName { get; set; }
        public string StateName { get; set; }
        public string CitiName { get; set; }
        public string TalukaName { get; set; }
        public string WeightName { get; set; }


    }
}