using Challenge.Core.Application.DTOs;
using Challenge.Core.CustomEntities;
using Challenge.Core.QueryFilter;
using System;
using System.Threading.Tasks;

namespace Challenge.Core.Domain.Interfaces
{
    public interface IMeetingService
    {
        Task<PagedList<MeetingDto>> GetMeetings(MeetingQueryFilter filters);

        Task<MeetingDto> GetMeeting(int id);

        Task<MeetingDto> InsertMeeting(MeetingDto meetingDto);

        Task<bool> UpdateMeeting(MeetingDto meetingDto, int adminId);

        Task<bool> SignOn(int meetingId, int userId);

        Task<bool> CheckIn(int meetingId, int userId);

        Task<bool> DeleteMeeting(int id);

        Task<int> GetAttendedCount(int meetingId);

        Task<int> GetBeerPacks(int meetingId);

        Task<bool> Invite(int meetingId, int[]guests);

        // se agrega este metodo para poder ser testeado
        int CalculateBeers(DateTime date, WeatherDto temps, int attendes, double totalDays);

    }
}