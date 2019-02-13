using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TablessV1._0.Filters;

namespace TablessV1._0.Models
{
    public class FacebookFriendsViewModel
    {
        [Required]
        [FacebookMapping("id")]
        public string TaggingId { get; set; }


        [Required]
        [Display(Name = "Friend's Name")]
        [FacebookMapping("name")]
        public string Name { get; set; }

        [FacebookMapping("url", parent = "picture")]
        public string ImageURL { get; set; }
    }
}