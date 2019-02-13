using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TablessV1._0.Models
{
    public class UserToken
    {
        public static string Token { get; set; }
        public static string Otoken { get; set; }
        public static string Overifier { get; set; }
        public static string TokenSecert { get; set; }
        public static int flag { get; set; }
        public static int flag2 { get; set; }
        public UserToken()
        {
            flag = 0;
            flag2 = 0;
        }

    }
}