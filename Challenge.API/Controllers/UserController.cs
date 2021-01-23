using Challenge.API.Responses;
using Challenge.Core.Application.DTOs;
using Challenge.Core.Application.Services;
using Challenge.Core.CustomEntities;
using Challenge.Core.Domain.Entities;
using Challenge.Core.Domain.Interfaces;
using Challenge.Core.QueryFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users. Can be filtered.
        /// </summary>
        /// <returns>Ok(List of all Meets) or NotContent()</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<PagedList<UserDto>>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers([FromQuery]UserQueryFilter filters)
        {
            var usersDto = await _userService.GetUsers(filters);

            var metadata = new Metadata
            {
                TotalCount = usersDto.TotalCount,
                PageSize = usersDto.PageSize,
                CurrentPage = usersDto.CurrentPage,
                TotalPages = usersDto.TotalPages,
                HasNextPage = usersDto.HasNextPage,
                HasPreviousPage = usersDto.HasPreviousPage,
            };

            var response = new ApiResponse<IEnumerable<UserDto>>(usersDto) { Metadata = metadata };

            // Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        /// <summary>
        /// Get a user by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(int id)
        {
            var userDto = await _userService.GetUser(id);

            return Ok(new ApiResponse<UserDto>(userDto));
        }

        /// <summary>
        /// Create a meeting.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody]NewUserDto newUserDto)
        {

            var userDto = await _userService.InsertUser(newUserDto);

            return Ok(new ApiResponse<UserDto>(userDto));
        }

        /// <summary>
        /// Update a user.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put([FromBody]UserDto userDto)
        {
            var result = await _userService.UpdateUser(userDto);

            return Ok(new ApiResponse<bool>(result));
        }

        /// <summary>
        /// Delete a user by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            await _userService.DeleteUser(id);

            return Ok();
        }
    }
}