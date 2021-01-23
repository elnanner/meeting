using Challenge.Core.Domain.Entities;
using Challenge.Core.Domain.Interfaces;
using Challenge.Core.QueryFilter;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        private const string SP_GET_ALL = "[dbo].[spUserGetAll]";
        private const string SP_GET_BY_ID = "[dbo].[spUserGetById]";
        private const string SP_INSERT = "[dbo].[spUserInsert]";// TODO
        private const string SP_UPDATE = "[dbo].[spUserUpdate]";// TODO
        private const string SP_DELETE = "[dbo].[spUserDelete]";// TODO
        private const string SP_VALIDATE = "[dbo].[spUserValidate]";

        public UserRepository(IConfiguration configuration) => this._configuration = configuration;

        public async Task<IEnumerable<User>> GetUsers(UserQueryFilter filters)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", filters.Name, DbType.String);
            parameters.Add("Email", filters.Email, DbType.String);
            parameters.Add("Role", filters.Role, DbType.String);

            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var users = await connection.QueryAsync<User>(SP_GET_ALL,
                       parameters,
                       null,
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                return users;
            }
        }

        public async Task<User> GetUser(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);

            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var user = await connection.QueryAsync<User>(SP_GET_BY_ID,
                       parameters,
                       null,
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                return user.FirstOrDefault();
            }
        }

        public async Task<User> InsertUser(NewUser user)
        {
            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var insertedUser = await connection.QueryAsync<int>(SP_INSERT,
                       new { Name = user.Name, Email = user.Email, Role = user.Role},
                       null,
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                return new User { UserId = insertedUser.FirstOrDefault(), Name = user.Name, Email = user.Email, Role = user.Role };
            };

        }

        public async Task<bool> UpdateUser(NewUser user)
        {
            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var userId = await connection.QueryAsync<int>(SP_UPDATE,
                       new { Name = user.Name, Email = user.Email, Role = user.Role },
                       null,
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                return userId.Count() > 0;
            }
        }
        public async Task<bool> DeleteUser(int id)
        {
            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<int>(SP_DELETE,
                       new { UserId = id },
                       null,
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                return result.Count() > 0;
            }
        }

        public async Task<User> ValidateUser(UserLogin userLogin)
        {
            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<User>(SP_VALIDATE,
                       new { Username = userLogin.Username, Password = userLogin.Password },
                       null,
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                return result.FirstOrDefault();
            }
        }
    }
}
