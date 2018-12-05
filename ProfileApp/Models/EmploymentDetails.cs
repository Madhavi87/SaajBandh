using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfileApp.Models
{
    public class EmploymentDetails
    {
        public int EmpDetailID { get; set; }
        public string EmployerName { get; set; }
        public string Designation { get; set; }
        public int UserID { get; set; }
        public int TypeID { get; set; }
        public string Type { get; set; }
        public DateTime DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }
        public string Gap { get; set; }
    }
}