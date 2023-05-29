using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POETEST_MVC.Models
{
    public class User
    {
        public string userID { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
