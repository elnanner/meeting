using Challenge.Core.Domain.Entities;
using Challenge.Core.QueryFilter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.Core.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers(UserQueryFilter filters);

        Task<User> GetUser(int userId);

        Task<User> InsertUser(NewUser user);

        Task<bool> UpdateUser(NewUser user);

        Task<bool> DeleteUser(int id);

        Task<User> ValidateUser(UserLogin userLogin);
    }
}
