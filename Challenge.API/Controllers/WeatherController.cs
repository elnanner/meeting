using Challenge.API.Responses;
using Challenge.Core.Application.DTOs;
using Challenge.Core.Application.Services;
using Challenge.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Challenge.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IMeetingService _meetingService;

        public WeatherController(IWeatherService weatherService, IMeetingService meetingService)
        {
            _weatherService = weatherService;
            _meetingService = meetingService;
        }

        [HttpGet("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<WeatherDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWeatherByLocation(int meetingId)
        {
            var meeting = await _meetingService.GetMeeting(meetingId);

            var latitude = meeting.City.Latitude;
            var longitude = meeting.City.Longitude;

            var weatherDto = await _weatherService.GetWeatherByLocation(latitude, longitude);

            return Ok(new ApiResponse<WeatherDto>(weatherDto));
        }
    }
}