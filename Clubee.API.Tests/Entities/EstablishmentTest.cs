using Clubee.API.Contracts.Enums;
using Clubee.API.Entities;
using FluentAssertions;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using Xunit;

namespace Clubee.API.Tests.Entities
{
    public class EstablishmentTest
    {
        [Fact]
        public void Should_create_entity_correctly()
        {
            // Act.
            Establishment establishment = new Establishment(
                "Vitrola Bar",
                "www.vitrolabar.com.br",
                "www.vitrolabar.com.br",
                "Melhor bar da cidade",
                new GeoJson2DGeographicCoordinates(
                    -47.5541214, 
                    7.5425421
                ),
                string.Empty,
                new[] 
                { 
                    EstablishmentTypeEnum.Bar, 
                    EstablishmentTypeEnum.CasaShows 
                },
                new []
                {
                    new Availability(
                        DayOfWeekEnum.Friday,
                        new TimeSpan(20, 0, 0),
                        new TimeSpan(4, 0, 0)
                    ),
                    new Availability(
                        DayOfWeekEnum.Saturday,
                        new TimeSpan(20, 0, 0),
                        new TimeSpan(5, 0, 0)
                    )
                }
            );

            // Assert.
            establishment.Name.Should().NotBeNullOrEmpty();
            establishment.Image.Should().Contain("vitrola");
            establishment.ImageThumbnail.Should().Contain("vitrola");
            establishment.Availabilities.Should().HaveCount(2);
            establishment.EstablishmentTypes.Should().HaveCount(2);
            establishment.Location.Latitude.Should().NotBe(default);
            establishment.Location.Longitude.Should().NotBe(default);
        }
    }
}
