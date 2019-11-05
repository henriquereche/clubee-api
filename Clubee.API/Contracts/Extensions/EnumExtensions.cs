using Clubee.API.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Clubee.API.Contracts.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get description off all field on specifyed enum.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static IEnumerable<EnumDescriptionModel> GetEnumDescriptions(this Type enumType)
        {
            if (enumType.IsEnum)
            {
                object instance = Activator.CreateInstance(enumType);

                foreach (FieldInfo field in enumType.GetFields())
                {
                    DescriptionAttribute description = field.GetCustomAttribute<DescriptionAttribute>();

                    if (description != null)
                        yield return new EnumDescriptionModel(
                            (int)field.GetValue(instance), 
                            description.Description
                        );
                }
            }
        }
    }
}
