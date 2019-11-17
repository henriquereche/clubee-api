using Microsoft.ApplicationInsights;

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
        public static void TrackEvent(this TelemetryClient client, string eventName, object properties)
        {
            client.TrackEvent(
                eventName, 
                properties.ToDictionary(true)
            );
        }
    }
}
