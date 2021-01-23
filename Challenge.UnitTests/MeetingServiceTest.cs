using Challenge.Core.Application.DTOs;
using Challenge.Core.Application.Services;
using Challenge.Core.CustomEntities;
using Challenge.Core.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Challenge.UnitTests
{
    [TestFixture]
    public class MeetingServiceTest
    {
        IMeetingService _meetingService;


        [Test]
        public void GetMeetingsType()
        {

            // Arrange none

            // Act
            var expected = typeof(PagedList<MeetingDto>).Name;
            var actual = _meetingService.GetMeetings(null).Result.GetType().Name;

            // Assert
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void CalculateBeerPacks()
        {
            // real service

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appSettings.json");

            IConfiguration configuration = configurationBuilder.Build();

            IOptions<PaginationOptions> options = Options.Create(new PaginationOptions());

            var realMeetingService = new MeetingService(null, null, options);


            // calculara en base a 24.01 grados, o sea 2 por persona
            var weatherDto = new WeatherDto
            {
                Hourly = new HourlyDto
                {
                    data = new List<DatumDto>
                    {
                         new DatumDto
                        {
                            //solo necetiso la hora y la temp
                            time = 1611183600,// representa el 20/01/201 20:00hs en EPOCH time
                            temperature = 24.01d// deberia calcular 2 per cápita
                        }
                    }
                }

            };

            //meeting 2(mock) es para la plata, dentro de 1 dia
            var meeting2 = _meetingService.GetMeeting(2).Result;

            var totalDays = 1;// faltaria un dia para la meeting

            // cantidad de invitado qeu hicieron checkin, en este caso seran 10
            int attendes = _meetingService.GetAttendedCount(meeting2.MeetingId).Result;

            int result = realMeetingService.CalculateBeers(meeting2.Date, weatherDto, attendes, totalDays);

            Assert.AreEqual(4, result);

        }


        #region Setup mock-sacar a otra clase
        [SetUp]
        public void SetupMeetingMock()
        {

            var meetingsDto = new List<MeetingDto> {
                new MeetingDto{
                    MeetingId = 1,
                    AdminId = 1,
                    Description = "meeting mock 1, ya pasó",
                    Date = DateTime.Now.AddDays(-10),
                    MaxPeople = 10,
                    City = new CityDto{
                        CityId = 1,
                        Name = "La Plata",
                        Latitude = -34.9214,
                        Longitude =  -57.9544
                    }
                },new MeetingDto{
                    MeetingId = 2,
                    AdminId = 1,
                    Description = "meeting mock 2, es mañana",
                    Date = new DateTime(2021, 1, 20, 20, 0, 0, 0),
                    MaxPeople = 10,
                    City = new CityDto{
                        CityId = 1,
                        Name = "La Plata",
                        Latitude = -34.9214,
                        Longitude =  -57.9544
                    }
                },new MeetingDto{
                    MeetingId = 3,
                    AdminId = 1,
                    Description = "meeting mock 3, dentro de 10 dias",
                    Date = DateTime.Now.AddDays(10),
                    MaxPeople = 10,
                    City = new CityDto{
                        CityId = 1,
                        Name = "La Plata",
                        Latitude = -34.9214,
                        Longitude =  -57.9544
                    }
                },new MeetingDto{
                    MeetingId = 4,
                    AdminId = 1,
                    Description = "meeting mock 1, dentro de 4 dias",
                    Date = DateTime.Now.AddDays(4),
                    MaxPeople = 10,
                    City = new CityDto{
                        CityId = 1,
                        Name = "La Plata",
                        Latitude = -34.9214,
                        Longitude =  -57.9544
                    }
                },
            };

            var pagedList = PagedList<MeetingDto>.Create(meetingsDto, 1, 2);// son 2 paginas de 2 elementos

            var mock = new Mock<IMeetingService>();
            mock.Setup(m => m.GetMeetings(null)).ReturnsAsync(pagedList);
            mock.Setup(m => m.GetMeeting(2)).ReturnsAsync(meetingsDto.Where(
                x => x.MeetingId == 2).FirstOrDefault());
            mock.Setup(m => m.GetAttendedCount(2)).ReturnsAsync(10);
            _meetingService = mock.Object;
        }

        #endregion

    }
}
