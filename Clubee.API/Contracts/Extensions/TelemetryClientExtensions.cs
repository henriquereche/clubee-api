using Microsoft.ApplicationInsights;
using System.Collections.Generic;
using System.Reflection;

namespace Clubee.API.Contracts.Extensions
{
    public static class TelemetryClientExtensions
    {
        /// <summary>
        /// Tracks event using dynamic property values.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="eventName"></param>
        /// <param name="properties"></param>
        public static void TrackEvent(this TelemetryClient client, string eventName, dynamic properties)
        {
            IDictionary<string, string> propertiesDictionary = new Dictionary<string, string>();
            PropertyInfo[] propertiesDescriptor = properties.GetType().GetProperties();

            foreach (PropertyInfo property in propertiesDescriptor)
                propertiesDictionary[property.Name.ToCamelCase()] = property.GetValue(properties).ToString();

            client.TrackEvent(eventName, propertiesDictionary);
        }
    }
}
