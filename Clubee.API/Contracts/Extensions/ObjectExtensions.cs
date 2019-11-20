using System.Collections.Generic;
using System.Reflection;

namespace Clubee.API.Contracts.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Transforms object properties into keyValue dictionary.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="camelCaseConvention"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ToDictionary(this object value, bool camelCaseConvention = false)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            PropertyInfo[] propertiesDescriptor = value.GetType().GetProperties();

            foreach (PropertyInfo property in propertiesDescriptor)
            {
                object propertyValue = property.GetValue(value);
                string propertyName = camelCaseConvention 
                    ? property.Name.ToCamelCase() 
                    : property.Name;

                if (propertyValue != null)
                {
                    if (propertyValue.GetType().IsEnum)
                        propertyValue = (int)propertyValue;

                    dictionary[propertyName] = propertyValue.ToString();
                }
            }

            return dictionary;
        }
    }
}
