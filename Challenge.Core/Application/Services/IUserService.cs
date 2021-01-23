using Challenge.Core.Application.DTOs;
using Challenge.Core.CustomEntities;
using Challenge.Core.QueryFilter;
using System.Threading.Tasks;

namespace Challenge.Core.Application.Services
{
    public interface IUserService
    {
        Task<PagedList<UserDto>> GetUsers(UserQueryFilter filters);

        Task<UserDto> GetUser(int userId);

        Task<UserDto> InsertUser(NewUserDto newUserDto);

        Task<bool> UpdateUser(UserDto userDto);

        Task<bool> DeleteUser(int id);

        Task<UserDto> ValidateUser(UserLoginDto userLoginDto);
    }
}
