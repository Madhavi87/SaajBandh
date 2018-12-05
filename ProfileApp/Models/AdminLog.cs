using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProfileApp.Models
{
    public class AdminLog
    {
        [Required(ErrorMessage="Enter UserName")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }

        public string ErrMsg { get; set; }
    }
}