using Challenge.Core.Application.DTOs;
using Challenge.Core.Application.Exceptions;
using Challenge.Core.CustomEntities;
using Challenge.Core.Domain.Entities;
using Challenge.Core.Domain.Interfaces;
using Challenge.Core.QueryFilter;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PaginationOptions _paginationOptions;
        public UserService(IUserRepository userRepository, IOptions<PaginationOptions> options)
        {
            _userRepository = userRepository;
            _paginationOptions = options.Value;
        }

        public async Task<PagedList<UserDto>> GetUsers(UserQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;
            try
            {
                var users = await _userRepository.GetUsers(filters);

                var usersDto = users.Select(u =>
                    new UserDto
                    {
                        UserId = u.UserId,
                        Name = u.Name,
                        Email = u.Email,
                        Role = u.Role
                    }).AsEnumerable();

                return PagedList<UserDto>.Create(usersDto, filters.PageNumber, filters.PageSize);
            }
            catch (Exception ex)
            {

                throw new ChallengeException(ex.Message);
            }
        }

        public async Task<UserDto> GetUser(int userId)
        {
            var user = await _userRepository.GetUser(userId);

            if (user == null)
            {
                throw new ChallengeException("El usuario no existe.");
            }

            var userDto =
                new UserDto
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                };

            return userDto;
        }

        public async Task<UserDto> InsertUser(NewUserDto newUserDto)
        {
            await Task.CompletedTask;
            throw new ChallengeException("No desarrollado por cuestiones de tiempo.");
        }

        public async Task<bool> UpdateUser(UserDto userDto)
        {
            await Task.CompletedTask;
            throw new ChallengeException("No desarrollado por cuestiones de tiempo.");
        }
        public async Task<bool> DeleteUser(int id)
        {
            await Task.CompletedTask;
            throw new ChallengeException("No desarrollado por cuestiones de tiempo.");
        }

        // TODO sacar a security service
        // el username es el email del usuario
        public async Task< UserDto> ValidateUser(UserLoginDto userLoginDto)
        {
            try
            {
                UserLogin user = new UserLogin
                {
                    Username = userLoginDto.Username,
                    Password = userLoginDto.Password
                };

                var logged = await _userRepository.ValidateUser(user);
                if (logged != null)
                {
                    var userDto = new UserDto
                    {
                        UserId = logged.UserId,
                        Name = logged.Name,
                        Email = logged.Email,
                        Role = logged.Role
                    };
                    return (userDto);
                }
                return null;
            }
            catch (Exception e)
            {

                throw new ChallengeException(e.Message);
            }
        }
    }
}
