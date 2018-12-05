using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfileApp.Models
{
    public class AdvSearchResult
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ImagePath { get; set; }
        public string City { get; set; }
    }
}