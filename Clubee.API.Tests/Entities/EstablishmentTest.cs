//using Clubee.API.Contracts.Enums;
//using Clubee.API.Entities;
//using MongoDB.Driver.GeoJsonObjectModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace Clubee.API.Tests.Entities
//{
//    public class EstablishmentTest
//    {
//        [Fact]
//        public void Should_create_entity_correctly()
//        {
//            // Act.
//            Establishment establishment = new Establishment(
//                "Vitrola Bar",
//                new Image("", "", ""),
//                new Image("", "", ""),
//                "www.vitrolabar.com.br",
//                new Location("", 1, "", "", "", 
//                    new GeoJson2DGeographicCoordinates(
//                        -47.5541214,
//                        7.5425421
//                    )
//                ),
//                new[]
//                {
//                    EstablishmentTypeEnum.Bar,
//                    EstablishmentTypeEnum.CasaShows
//                },
//                new[]
//                {
//                    new Availability(
//                        DayOfWeekEnum.Friday,
//                        new TimeSpan(20, 0, 0),
//                        new TimeSpan(4, 0, 0)
//                    ),
//                    new Availability(
//                        DayOfWeekEnum.Saturday,
//                        new TimeSpan(20, 0, 0),
//                        new TimeSpan(5, 0, 0)
//                    ),
//                    new Availability(
//                        DayOfWeekEnum.Sunday,
//                        new TimeSpan(20, 0, 0),
//                        new TimeSpan(5, 0, 0)
//                    ),
//                    new Availability(
//                        DayOfWeekEnum.Tuesday,
//                        new TimeSpan(20, 0, 0),
//                        new TimeSpan(5, 0, 0)
//                    ),
//                }
//            );

//            IEnumerable<Availability> availabilities = new List<Availability>();


//            DateTime initialDate = new DateTime(2019, 11, 24, 10, 0, 0);
//            DateTime endDate = new DateTime(2019, 11, 29, 21, 0, 0);

//            int initialDayOfWeek = (int)initialDate.DayOfWeek + 1;
//            int endDayOfWeek = (int)endDate.DayOfWeek + 1;

//            if (initialDayOfWeek == endDayOfWeek && endDate.Subtract(initialDate).TotalDays >= 7)
//            {
//                initialDayOfWeek = 1;
//                endDayOfWeek = 7;
//            }

//            if (initialDayOfWeek == (endDayOfWeek - 1) && (endDate.Subtract(initialDate).TotalHours < 24))
//            {
//                endDayOfWeek = initialDayOfWeek;
//            }

//            if (initialDayOfWeek == endDayOfWeek)
//            {
//                availabilities = establishment.Availabilities.Where(x =>
//                    x.OpenTime >= initialDate.TimeOfDay
//                        && x.CloseTime <= endDate.TimeOfDay
//                        && (int)x.DayOfWeek == initialDayOfWeek);
//            } else
//            {
//                IEnumerable<int> dateRange = Enumerable.Range(initialDayOfWeek, 7 - initialDayOfWeek);
//                IEnumerable<int> freeDate = dateRange.Where(x => x != initialDayOfWeek && x != endDayOfWeek);

//                availabilities = establishment.Availabilities.Where(x =>
//                    freeDate.Contains((int)x.DayOfWeek)
//                    || (
//                        (int)x.DayOfWeek == initialDayOfWeek 
//                            ? (x.OpenTime >= initialDate.TimeOfDay)
//                            : (int)x.DayOfWeek == endDayOfWeek && x.OpenTime <= endDate.TimeOfDay 
//                        )
//                    );
//            }

//            Console.WriteLine(availabilities);
//        }
//    }
//}