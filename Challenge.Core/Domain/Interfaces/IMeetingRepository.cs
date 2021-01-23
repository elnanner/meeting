using Challenge.Core.Domain.Entities;
using Challenge.Core.QueryFilter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.Core.Domain.Interfaces
{
    public interface IMeetingRepository
    {
        Task<IEnumerable<Meeting>> GetMeetings(MeetingQueryFilter filters);

        Task<Meeting> GetMeeting(int id);

        Task<Meeting> InsertMeeting(Meeting meeting);

        Task<bool> UpdateMeeting(Meeting meeting);

        Task<bool> DeleteMeeting(int id);

        Task<bool> SignOn(int meetingId, int userId);

        Task<bool> CheckIn(int meetingId, int userId);

        Task<int> GetAttendedCount(int meetingId);

    }
}
