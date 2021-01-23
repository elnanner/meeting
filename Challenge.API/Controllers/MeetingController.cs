using Challenge.API.Responses;
using Challenge.Core.Application.DTOs;
using Challenge.Core.CustomEntities;
using Challenge.Core.Domain.Interfaces;
using Challenge.Core.QueryFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        /// <summary>
        /// Get all Meetings. Can be filtered.
        /// </summary>
        /// <returns>Ok(List of all Meets) or NotContent()</returns>
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<PagedList<MeetingDto>>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMeetings([FromQuery]MeetingQueryFilter filters)
        {
            var meetingsDto = await _meetingService.GetMeetings(filters);

            var metadata = new Metadata
            {
                TotalCount = meetingsDto.TotalCount,
                PageSize = meetingsDto.PageSize,
                CurrentPage = meetingsDto.CurrentPage,
                TotalPages = meetingsDto.TotalPages,
                HasNextPage = meetingsDto.HasNextPage,
                HasPreviousPage = meetingsDto.HasPreviousPage,
            };

            if (meetingsDto.Count > 0)
            {
                var response = new ApiResponse<IEnumerable<MeetingDto>>(meetingsDto) { Metadata = metadata };

                // Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(response);
            }
            return NoContent();
        }

        /// <summary>
        /// Get a meeting by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<MeetingDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMeeting(int id)
        {
            var meetingDto = await _meetingService.GetMeeting(id);
            if (meetingDto != null)
            {
                return Ok(new ApiResponse<MeetingDto>(meetingDto));
            }
            return NotFound();
        }

        /// <summary>
        /// Create a meeting.
        /// </summary>
        /// <param name="meetingDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<MeetingDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody]MeetingDto meetingDto)
        {
            meetingDto.AdminId = int.Parse(HttpContext.User.Claims.FirstOrDefault(x=>x.Type == "UserId").Value);
            var newMeeting = await _meetingService.InsertMeeting(meetingDto);

            return Ok(new ApiResponse<MeetingDto>(meetingDto));
        }

        /// <summary>
        /// Update a meeting.
        /// </summary>
        /// <param name="meetingDto"></param>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(MeetingDto meetingDto)
        {
            var adminId = int.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);

            var result = await _meetingService.UpdateMeeting(meetingDto, adminId);

            return Ok(new ApiResponse<bool>(result));
        }

        /// <summary>
        /// Delete a meeting by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        [HttpDelete("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _meetingService.DeleteMeeting(id);

            return Ok(new ApiResponse<bool>(result));
        }

        /// <summary>
        /// Sign on to a meeting.
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [HttpPost("{meetingId}/signon")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<PagedList<MeetingDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignOn(int meetingId)
        {
            int userId;

            if(!int.TryParse(HttpContext.User.FindFirst("UserId").Value, out userId))
            {
                return StatusCode(500, "No se pudo obtener le identifcador para el usuario actual.");
            }

            var result = await _meetingService.SignOn(meetingId, userId);

            return Ok(new ApiResponse<bool>(result));
        }

        /// <summary>
        /// Check in to a meeting.
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [HttpPost("{meetingId}/checkin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Checkin(int meetingId)
        {
            int userId;

            if (!int.TryParse(HttpContext.User.FindFirst("UserId").Value, out userId))
            {
                return StatusCode(500, "No se pudo obtener le identifcador para el usuario actual.");
            }

            var result = await _meetingService.CheckIn(meetingId, userId);

            return Ok(new ApiResponse<bool>(result));
        }

        /// <summary>
        /// Get beer count.
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("{meetingId}/beerPacks")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<int>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBeerPacks(int meetingId)
        {
            var beerCount = await _meetingService.GetBeerPacks(meetingId);

            return Ok(new ApiResponse<int>(beerCount));
        }

        /// <summary>
        /// Invite to meeting
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("{meetingId}/invite")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Invite(int meetingId, int[]guests)
        {

            var done = await _meetingService.Invite(meetingId, guests);

            return Ok(new ApiResponse<bool>(done));
        }
    }
}