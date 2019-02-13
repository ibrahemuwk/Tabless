using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using TablessV1._0.Filters;

namespace TablessV1._0.Models
{
    public class FacebookProfileViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        [FacebookMapping("first_name")]
        public dynamic FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [FacebookMapping("last_name")]
        public dynamic LastName { get; set; }

        [FacebookMapping("name")]
        public dynamic Fullname { get; set; }


        public dynamic ImageURL { get; internal set; }

        [FacebookMapping("link")]
        public dynamic LinkUrl { get; set; }

        [FacebookMapping("locale")]
        public dynamic Locale { get; set; }

        [FacebookMapping("email")]
        public dynamic email { get; set; }

        [FacebookMapping("age_range")]
        public dynamic age_range { get; set; }

        [FacebookMapping("gender")]
        public dynamic gender { get; set; }

        [FacebookMapping("name", parent = "location")]
        public dynamic Location { get; set; }
        public static int iflogedTwitter { get; set; }
        public static string message { get; set; }

        public FacebookProfileViewModel()
        {
            iflogedTwitter = 0;
        }
    }
    
}