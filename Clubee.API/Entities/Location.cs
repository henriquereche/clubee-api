using MongoDB.Driver.GeoJsonObjectModel;

namespace Clubee.API.Entities
{
    public class Location
    {
        public Location(
            string street, 
            uint number, 
            string state, 
            string country, 
            string city, 
            GeoJson2DGeographicCoordinates coordinates
            )
        {
            this.Street = street;
            this.Number = number;
            this.State = state;
            this.Country = country;
            this.City = city;
            this.Coordinates = coordinates;
        }

        public string Street { get; set; }
        public uint Number { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public GeoJson2DGeographicCoordinates Coordinates { get; set; }
    }
}
