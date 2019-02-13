using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TablessV1._0.Filters;

namespace TablessV1._0.Models
{
    public class FacebookPostViewModel
    {
        [Required]
        [FacebookMapping("id")]
        public string Id { get; set; }

        [FacebookMapping("created_time")]
        public DateTime Created_Time { get; set; }

        [FacebookMapping("id", parent = "from")]
        public string From_Id { get; set; }

        [FacebookMapping("name", parent = "from")]
        public string From_Name { get; set; }

        [FacebookMapping("url", parent = "from")]
        public string From_Picture_Url { get; set; }

        [FacebookMapping("story")]
        public string Story { get; set; }

        [FacebookMapping("message")]
        public string Message { get; set; }

        [FacebookMapping("picture")]
        public string Picture_Url { get; set; }

        [FacebookMapping("link")]
        public string Link { get; set; }

        [FacebookMapping("description")]
        public string Description { get; set; }

        [FacebookMapping("likes")]
        public string Likes { get; set; }

        [FacebookMapping("comments")]
        public string Comments { get; set; }
    }
}