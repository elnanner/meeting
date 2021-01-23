using Challenge.API.Controllers;
using Challenge.Core.Application.DTOs;
using Challenge.Core.Application.Services;
using Challenge.Core.CustomEntities;
using Challenge.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Challenge.UnitTests
{
    public class AuthenticationControllerTest
    {
        [Test]
        public async Task AuthenticateOk()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appSettings.json");

            IConfiguration configuration = configurationBuilder.Build();

            var repository = new MeetingRepository(configuration);
            var weatherService = new WeatherService();

            IOptions<PaginationOptions> options = Options.Create(new PaginationOptions());

            var meetingController = new MeetingController(
                new MeetingService(repository, weatherService, options));

            var user = new UserLoginDto
            {
                Username = "matsuoluciano@gmail.com",
                Password = "luciano1"
            };
            var userRepo = new UserRepository(configuration);
            var userService = new UserService(userRepo, options);

            var authController = new AuthenticationController(configuration, userService);

            var token = await authController.Authenticate(user);

            Assert.AreEqual(token.GetType(), typeof(OkObjectResult));

            await Task.CompletedTask;
        }
    }
}
