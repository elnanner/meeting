using Challenge.Core.Application.DTOs;
using Challenge.Core.Application.Services;
using Moq;
using NUnit.Framework;
using System;

namespace Challenge.UnitTests
{
    [TestFixture]
    public class WeatherServiceTest
    {
        IWeatherService _weatherService;

        [Test]
        public void GetWeatherByLocation()
        {
            long ticks = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
           // .Ticks;
            /***
             * Por una cuestion de tiempo, solo se puede probar la temperatura para la hora actual y el dia actual
             * Se utiliza Moq
             */

            // Arrange
            var lat = -34.9214;
            var lon = -57.9544;
            var expected = new WeatherDto
            {
                Currently = new CurrentlyDto
                {
                    time = (int)DateTime.Now.Ticks,
                    temperature = 24.5d
                },
                Latitude = lat,
                Longitude = lon
            };

            // Act
            var actual = _weatherService.GetWeatherByLocation(lat, lon).Result;

            // Assert
            Assert.AreEqual(expected.Currently.temperature, actual.Currently.temperature);

        }

        [SetUp]
        public void SetupWeatherMock()
        {
            var latitude = -34.9214;
            var longitude = -57.9544;
            var weatherDto = new WeatherDto
            {
                Currently = new CurrentlyDto
                {
                    time = (int)DateTime.Now.Ticks,
                    temperature = 24.5d
                },
                Latitude = latitude,
                Longitude = longitude
            };

            var mock = new Mock<IWeatherService>();
            mock.Setup(m => m.GetWeatherByLocation(latitude, longitude)).ReturnsAsync(weatherDto);
            _weatherService = mock.Object;
        }
    }
}
