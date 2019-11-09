namespace Clubee.API.Models.Event
{
    public class EventFindLocationDTO
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Street { get; set; }
        public uint Number { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
