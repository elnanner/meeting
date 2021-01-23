using Challenge.Core.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Challenge.Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly IConfiguration _configuration;

        private const string SP_GET_ALL = "[dbo].[spCityGetAll]";
        public async Task<IEnumerable<City>> GetAll()
        {
            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<City>(SP_GET_ALL,
                    null,
                    null,
                    int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                    CommandType.StoredProcedure);
            }
        }
    }
}
