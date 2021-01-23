using Challenge.Core.Domain.Entities;
using Challenge.Core.Domain.Interfaces;
using Challenge.Core.QueryFilter;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Infrastructure.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly IConfiguration _configuration;

        private const string SP_GET_ALL = "[dbo].[spMeetingGetAll]";
        private const string SP_GET_BY_ID = "[dbo].[spMeetingGetById]";
        private const string SP_INSERT = "[dbo].[spMeetingInsert]";
        private const string SP_UPDATE = "[dbo].[spMeetingUpdate]";
        private const string SP_DELETE = "[dbo].[spMeetingDelete]";
        private const string SP_WILLATTEND = "[dbo].[spMeetingWillAttend]";
        private const string SP_ATTENDED = "[dbo].[spMeetingAttended]";
        private const string SP_GET_ATTENDED_COUNT = "[dbo].[spMeetingGetAttendedCount]";

        public MeetingRepository(IConfiguration configuration) => this._configuration = configuration;


        public async Task<IEnumerable<Meeting>> GetMeetings(MeetingQueryFilter filters)
        {
            var parameters = new DynamicParameters();
            parameters.Add("AdminId", filters.AdminId, DbType.Int32);
            parameters.Add("Date", filters.Date, DbType.DateTime);
            parameters.Add("Description", filters.Description, DbType.String);

            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var meetings = await connection.QueryAsync<Meeting, City, Meeting>(SP_GET_ALL,
                       (meeting, city) =>
                       {
                           meeting.City = city;
                           return meeting;
                       },
                       parameters,
                       null,
                       true,
                       "CityId",
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                return meetings;
            }
        }

        public async Task<Meeting> GetMeeting(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("MeetingId", id, DbType.Int32);
            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var meeting = await connection.QueryAsync<Meeting, City, Meeting>(SP_GET_BY_ID,
                       (m, city) =>
                       {
                           m.City = city;
                           return m;
                       },
                       parameters,
                       null,
                       true,
                       "CityId",
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                return meeting.FirstOrDefault();
            }
        }

        public async Task<Meeting> InsertMeeting(Meeting meeting)
        {

            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var insertedMeeting = await connection.QueryAsync<Meeting, City, Meeting>(SP_INSERT,
                         (m, city) =>
                         {
                             m.City = city;
                             return m;
                         },
                       new { AdminId = meeting.AdminId, Description = meeting.Description, Date = meeting.Date, MaxPeople = meeting.MaxPeople, CityId = meeting.City.CityId },
                       null,
                       true,
                       "CityId",
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                Debug.WriteLine($"Meeting insertada");
                meeting = insertedMeeting.FirstOrDefault();

                return meeting;

            }
        }

        public async Task<bool> UpdateMeeting(Meeting meeting)
        {
            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var meetingId = await connection.QueryAsync<int>(SP_UPDATE,
                       new { MeetingId = meeting.MeetingId, Description = meeting.Description, Date = meeting.Date, MaxPeople = meeting.MaxPeople, CityId = meeting.City.CityId },
                       null,
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                Debug.WriteLine($"Meeting actualizada.");

                return meetingId.Count() > 0;
            }
        }

        public async Task<bool> DeleteMeeting(int id)
        {
            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<int>(SP_DELETE,
                       new { MeetingId = id },
                       null,
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                Debug.WriteLine($"Meeting borrada.");

                return result.Count() > 0;
            }
        }

        public async Task<bool> SignOn(int meetingId, int userId)
        {
            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<int>(SP_WILLATTEND,
                       new { MeetingId = meetingId, UserId = userId, Status = (int)EStatus.WillAttend },
                       null,
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                Debug.WriteLine($"Usuario inscripto a la meeting.");

                return result.Count() > 0;
            }
        }

        public async Task<bool> CheckIn(int meetingId, int userId)
        {
            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<int>(SP_ATTENDED,
                       new { MeetingId = meetingId, UserId = userId, Status = (int)EStatus.Attended },
                       null,
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                Debug.WriteLine($"Usuario ha asistido a la meeting.");

                return result.Count() > 0;
            }
        }

        public async Task<int> GetAttendedCount(int meetingId)
        {
            using (var connection = new SqlConnection(this._configuration["AppSettings:DataBase:Meetup:ConnectionString"]))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<int>(SP_GET_ATTENDED_COUNT,
                       new { MeetingId = meetingId, Status = (int)EStatus.Attended },
                       null,
                       int.Parse(this._configuration["AppSettings:DataBase:Meetup:Timeout"]),
                       CommandType.StoredProcedure
                       );

                Debug.WriteLine($"Obtenidos la cantidad de inscriptos a la meeting.");

                return result.FirstOrDefault();
            }
        }

    }
}
