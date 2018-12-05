using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfileApp.Models
{
    public class ActiveDeactiveUser
    {
        public int UserID { get; set; } 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
    }
}