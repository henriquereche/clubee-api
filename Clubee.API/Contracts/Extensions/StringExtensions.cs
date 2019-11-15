namespace Clubee.API.Contracts.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Transform string to camel case.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return $"{char.ToLower(value[0])}{value.Substring(1)}";
        }
    }
}
