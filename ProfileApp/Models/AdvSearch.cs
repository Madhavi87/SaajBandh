using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfileApp.Models
{
    public class AdvSearch
    {

        public string FullName { get; set; }

        public string City { get; set; }
        public int CityID { get; set; }
        public int Gender { get; set; }
        public int Education { get; set; }
        public int MaritalStatus { get; set; }
    }
}