using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POETEST_MVC.Models
{
    public class Farmer
    {
        public int farmerID { get; set; }
        [Required(ErrorMessage = "The farmer name field is required.")]
        public string FarmerName { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public int UserID { get; set; }

    }
}