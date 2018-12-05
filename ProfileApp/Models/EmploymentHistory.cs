using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfileApp.Models
{
    public class EmploymentHistory
    {
        public int EmpDetailID { get; set; }
        public string EmployerName { get; set; }
        public int UserID { get; set; }
        public int TypeID { get; set; }
        public string Desgignation { get; set; }
        public string JobProfile { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
    }
}