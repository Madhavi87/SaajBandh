//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Education
    {
        public int EduDetailID { get; set; }
        public int UserID { get; set; }
        public int EduTypeID { get; set; }
        public int EducationDegreeID { get; set; }
        public string University { get; set; }
        public int PassYear { get; set; }
        public decimal Marks { get; set; }
    }
}