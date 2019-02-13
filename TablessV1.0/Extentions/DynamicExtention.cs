using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using TablessV1._0.Filters;

namespace TablessV1._0.Extentions
{
    public class propertycontainer
    {
        public string facebookfield { get; set; }
        public string facebookparent { get; set; }
        public PropertyInfo facebookMappedProperty { get; set; }
    }
    public static class DynamicExtention
    {
        public static T ToStatic<T>(object dynamicObject)
        {
            //create new instance of the static
            var entity = Activator.CreateInstance<T>();

            //convert dynamic object as an implemented directory
            var properties = dynamicObject as IDictionary<string, object>;
            if (properties == null)
            {
                return entity;
            }
            Dictionary<string, propertycontainer> PropertyLookup = new Dictionary<string, propertycontainer>();
            var destFBMapplingProperties = (from PropertyInfo property in entity.GetType().GetProperties()
                                            where property.GetCustomAttributes(typeof(FacebookMapping), true).Length > 0
                                            select property).ToList();
            foreach (PropertyInfo pi in destFBMapplingProperties)
            {
                foreach (object attribute in pi.GetCustomAttributes(typeof(FacebookMapping)))
                {
                    FacebookMapping facebookMappingAttribute = attribute as FacebookMapping;
                    if (facebookMappingAttribute != null)
                    {
                        string facebookLookupKey = string.Empty;
                        if (string.IsNullOrEmpty(facebookMappingAttribute.parent))
                        {
                            facebookLookupKey = facebookMappingAttribute.GetName();

                        }
                        else
                        {
                            facebookLookupKey = facebookMappingAttribute.parent;
                        }
                        try
                        {
                            PropertyLookup.Add(facebookLookupKey,
                            new propertycontainer
                            {
                                facebookfield = facebookMappingAttribute.GetName(),
                                facebookparent = facebookMappingAttribute.parent,
                                facebookMappedProperty = pi
                            });

                        }
                        catch (Exception)
                        {


                        }

                    }

                }
            }
            foreach (var entry in properties)
            {
                if (PropertyLookup.ContainsKey(entry.Key))
                {
                    var DestinationPropertyInfo = PropertyLookup[entry.Key];
                    if (DestinationPropertyInfo != null)
                    {
                        object mappedValue;
                        if (entry.Value.GetType().Name == "JsonObject")
                        {
                            //var childProperties = entry.Value as IDictionary<string, object>;
                            //mappedValue = (from KeyValuePair<string, object> item in childProperties
                            //               where item.Key == DestinationPropertyInfo.facebookfield
                            //               select item.Value).FirstOrDefault();
                            mappedValue = FindMatchingChildProperitesRecursively(entry, DestinationPropertyInfo);
                            if (mappedValue == null)
                            {
                                mappedValue = entry.Value;

                            }
                        }
                        else
                        {
                            mappedValue = entry.Value;
                        }
                        if (DestinationPropertyInfo.facebookMappedProperty.PropertyType.Name == "DateTime")
                        {
                            DestinationPropertyInfo.facebookMappedProperty.SetValue(entity, System.DateTime.Parse(mappedValue.ToString()), null);
                        }
                        else
                        {
                            DestinationPropertyInfo.facebookMappedProperty.SetValue(entity, mappedValue.ToString(), null);
                        }
                    }
                }

            }
            return entity;
        }
        private static object FindMatchingChildProperitesRecursively(
            KeyValuePair<string, object> entry,
            propertycontainer DestinationProperityInfo)
        {
            object mappedValue = null;
            var childproperties = entry.Value as IDictionary<string, object>;
            mappedValue = (from KeyValuePair<string, object> item in childproperties
                           where item.Key == DestinationProperityInfo.facebookfield
                           select item.Value).FirstOrDefault();
            if (mappedValue == null)
            {
                foreach (KeyValuePair<string, object> item in childproperties)
                {
                    if (item.Value.GetType().Name == "JsonObject")
                    {
                        mappedValue = FindMatchingChildProperitesRecursively(item, DestinationProperityInfo);

                    }

                }

            }
            return mappedValue;
        }
    }
}