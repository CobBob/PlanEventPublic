using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.Data
{
    public interface IRepository
    {
        void InsertActivityTimeInvitee(ActivityTimeInvitee ati);

        Task<ICollection<Activity>> GetActivities(string accountName);

        void UpdateActivityDescription(Guid guid, string description);

        Task<bool> TryUpdateActivityAtis(Guid guid, string[] atis);

        void AddActivityTime(Guid guid, TimeProposed timeProposed);

        Task<TimeProposed> GetTimeProposedFromTimeId(int timeProposedId);

        Guid GetGuidFromTimeId(int timeProposedId);

        Task<int> NumberOfTimeProposeds(Guid guid);

        void DeleteTimeProposed(TimeProposed timeProposed);

        void AddActivityInvitee(Guid guid, Invitee invitee);

        Task<bool> IsGuidValid(Guid guid);

        Guid GetGuidFromInviteeId(int inviteeId);

        Task<int> NumberOfInvitees(Guid guid);

        Task<Invitee> GetInviteeFromInviteeId(int inviteeId);

        void DeleteInvitee(Invitee invitee);

        void DeleteActivity(Guid guid);

        Task<Activity> GuidToActivityAsync(Guid guid);

        Task<ICollection<ActivityTimeInvitee>> GetATIsfromGuid(Guid guid);
    }
}
