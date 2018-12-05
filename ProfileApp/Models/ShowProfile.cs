using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProfileApp.Models
{
    public class ShowProfile
    {
        public int UserID { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Work { get; set; }
        public string Gender { get; set; }
        public string MritalStatus { get; set; }
        public string RAddress { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ImagePath { get; set; }
        public string MyAppID { get; set; }
        public string BloodGroup { get; set; }
        public bool IsActive { get; set; }

    }
}