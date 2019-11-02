using Microsoft.Extensions.Configuration;
using System;

namespace Clubee.API.Contracts.Extensions
{
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// Try get value from configuration source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetValue<T>(this IConfiguration configuration, string key, out T value)
        {
            value = default;

            try
            {
                if (configuration[key] != null)
                {
                    value = (T)Convert.ChangeType(configuration[key], typeof(T));
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
