namespace Clubee.API.Contracts.Infrastructure.Telemetry
{
    public sealed class EventNames
    {
        #region Events

        /// <summary>
        /// Specific event search.
        /// </summary>
        public const string EventFind = "EVENT-FIND";

        /// <summary>
        /// Events search using location.
        /// </summary>
        public const string EventListLocation = "EVENT-LIST-LOCATION";

        /// <summary>
        /// Genre based events search.
        /// </summary>
        public const string EventListGenre = "EVENT-LIST-GENRE";

        /// <summary>
        /// Query based events search.
        /// </summary>
        public const string EventListQuery = "EVENT-LIST-QUERY";

        /// <summary>
        /// Date based events search.
        /// </summary>
        public const string EventListDate = "EVENT-LIST-DATE";

        #endregion

        #region Establishments

        /// <summary>
        /// Establishment based events search.
        /// </summary>
        public const string EventListEstablishment = "EVENT-LIST-ESTABLISHMENT";

        /// <summary>
        /// Specific establishment search.
        /// </summary>
        public const string EstablishmentFind = "ESTABLISHMENT-FIND";

        /// <summary>
        /// Establishments search using location.
        /// </summary>
        public const string EstablishmentListLocation = "ESTABLISHMENT-LIST-LOCATION";

        /// <summary>
        /// Type based establishments search.
        /// </summary>
        public const string EstablishmentListType = "ESTABLISHMENT-LIST-TYPE";

        /// <summary>
        /// Query based establishments search.
        /// </summary>
        public const string EstablishmentListQuery = "ESTABLISHMENT-LIST-QUERY"; 

        #endregion
    }
}
