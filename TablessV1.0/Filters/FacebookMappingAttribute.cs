using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TablessV1._0.Filters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FacebookMapping : Attribute
    {
        private string name;
        public string parent;
        public FacebookMapping(string name)
        {
            this.name = name;
            this.parent = string.Empty;
        }
        public string GetName()
        {
            return this.name;
        }
    }
}