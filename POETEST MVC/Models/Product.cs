using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POETEST_MVC.Models
{
    public class Product
    {

        public int productID { get; set; }
        [Required]
        public string product { get; set; }
        [Required]
        public string type { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        [Required]
        public int farmerID { get; set; }

    }
}