using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProfileApp.Models
{
    public class FeedBackModel
    {
      

       [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Mobile")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Enter Email")]
        public string Email { get; set; }

        public string Comments { get; set; }
    }
}