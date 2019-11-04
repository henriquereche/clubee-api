using System.Collections.Generic;
using System.Linq;

namespace Clubee.API.Contracts.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// IEnumerable is null or hasnt any elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> value)
        {
            return value == null || !value.Any();
        }
    }
}
