using Microsoft.EntityFrameworkCore;
using PlanEvent.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.Data
{
    public class Repository : IRepository
    {
        private readonly EventDbContext _context;
        public Repository(EventDbContext context)
        {
            _context = context;
        }


        public async Task<ICollection<Activity>> GetActivities(string accountName)
        {
            return await _context.Activities
                .Where(a => a.AccountName == accountName)
                .ToListAsync();
        }

        public void InsertActivityTimeInvitee(ActivityTimeInvitee ati)
        {
            _context.Add(ati);
            _context.SaveChanges();
        }

        public async Task<bool> TryUpdateActivityAtis(Guid guid, string[] atis)
        {
            var activity = await GuidToActivityAsync(guid);

            try
            {
                //Get all the ati's from the context
                var allAtiFromActivity = _context.ActivityTimeInvitees
                    .Where(ati => ati.ActivityId == activity.ActivityId)
                    .ToList();

                //loop through the collectio of ati's, and set availability to 0
                foreach (var ati in allAtiFromActivity)
                {
                    ati.Availability = AVAILABILITY.X;
                }

                //Gather the ati's from Post
                for (int i = 0; i < atis.Length; i++)
                {
                    if (HtmlParseHelpers.TryParseStringActivityTimeInviteeTableIds(
                        atis[i],
                        out int tempInviteeId,
                        out int tempTimeId,
                        out AVAILABILITY availability))
                    {
                        allAtiFromActivity[allAtiFromActivity.FindIndex(ati => ati.InviteeId == tempInviteeId
                        && ati.TimeProposedId == tempTimeId)].Availability = AVAILABILITY.Y;
                    }
                    else
                    {
                        return false;
                    }
                }

                _context.UpdateRange(allAtiFromActivity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(activity.ActivityId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public async void UpdateActivityDescription(Guid guid, string description)
        {
            var activity = await GuidToActivityAsync(guid);

            activity.Description = description;
            _context.Activities.Update(activity);

        }
        public void DeleteActivity(Guid guid)
        {
            //TODO: Test if invitees and timeproposeds and activities are also deleted when including them query below
            var activityTimeInvitees = _context.ActivityTimeInvitees
                .Where(ati => ati.Activity.Guid == guid)
                .Include(ati => ati.Invitee)
                .Include(ati => ati.TimeProposed)
                .ToList();

            _context.Invitees.RemoveRange(activityTimeInvitees.Select(ati => ati.Invitee));
            _context.TimeProposeds.RemoveRange(activityTimeInvitees.Select(ati => ati.TimeProposed));
            _context.Remove(activityTimeInvitees.First().Activity);
            _context.RemoveRange(activityTimeInvitees);

            _context.SaveChanges();
        }

        public async Task<ICollection<ActivityTimeInvitee>> GetATIsfromGuid(Guid guid)
        {
            return await _context.ActivityTimeInvitees
                .Include(ati => ati.Activity)
                .Include(ati => ati.Invitee)
                .Include(ati => ati.TimeProposed)
                .Where(ati => ati.Activity.Guid == guid)
                .ToListAsync();
        }



        public void AddActivityTime(Guid guid, TimeProposed timeProposed)
        {
            var inviteeKeys = _context.ActivityTimeInvitees
                .Where(ati => ati.Activity.Guid == guid)
                .Select(ati => ati.InviteeId)
                .Distinct()
                .ToList();

            var activity = GuidToActivityAsync(guid).Result;

            foreach (var key in inviteeKeys)
            {
                timeProposed.ActivityTimeInvitees.Add(new ActivityTimeInvitee()
                {
                    ActivityId = activity.ActivityId,
                    InviteeId = key,
                    Availability = AVAILABILITY.X
                });
            }

            _context.Add(timeProposed);
            _context.SaveChanges();
        }

        public async Task<TimeProposed> GetTimeProposedFromTimeId(int timeProposedId)
        {
            return await _context.TimeProposeds
                .FirstOrDefaultAsync(m => m.TimeProposedId == timeProposedId);
        }

        public Guid GetGuidFromTimeId(int timeProposedId)
        {
            return _context.ActivityTimeInvitees
                .Include(ati => ati.Activity)
                .FirstOrDefault(ati => ati.TimeProposedId == timeProposedId)
                .Activity
                .Guid;
        }

        public async Task<int> NumberOfTimeProposeds(Guid guid)
        {
            return await _context.ActivityTimeInvitees
                .Where(ati => ati.Activity.Guid == new Guid())
                .Select(ati => ati.TimeProposedId)
                .Distinct()
                .CountAsync();
        }

        public void DeleteTimeProposed(TimeProposed timeProposed)
        {
            _context.TimeProposeds.Remove(timeProposed);
            _context.SaveChanges();
        }



        public void AddActivityInvitee(Guid guid, Invitee invitee)
        {
            var timeKeys = _context.ActivityTimeInvitees
                .Where(ati => ati.Activity.Guid == guid)
                .Select(ati => ati.TimeProposedId)
                .Distinct()
                .ToList();

            var activity = GuidToActivityAsync(guid).Result;

            foreach (var key in timeKeys)
            {
                invitee.ActivityTimeInvitees.Add(new ActivityTimeInvitee()
                {
                    ActivityId = activity.ActivityId,
                    TimeProposedId = key,
                    Availability = AVAILABILITY.X
                });
            }

            _context.Add(invitee);
            _context.SaveChanges();
        }

        public Guid GetGuidFromInviteeId(int inviteeId)
        {
            return _context.ActivityTimeInvitees
                .Include(ati => ati.Activity)
                .FirstOrDefault(ati => ati.InviteeId == inviteeId)
                .Activity
                .Guid;
        }

        public async Task<int> NumberOfInvitees(Guid guid)
        {
            return await _context.ActivityTimeInvitees
                .Where(ati => ati.Activity.Guid == guid)
                .Select(ati => ati.InviteeId)
                .Distinct()
                .CountAsync();
        }

        public async Task<Invitee> GetInviteeFromInviteeId(int inviteeId)
        {
            return await _context.Invitees.FindAsync(inviteeId);
        }

        public void DeleteInvitee(Invitee invitee)
        {
            _context.Invitees.Remove(invitee);
            _context.SaveChanges();
        }



        public async Task<Activity> GuidToActivityAsync(Guid guid)
        {
            return await _context.Activities
                .Where(a => a.Guid == guid)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsGuidValid(Guid guid)
        {
            return await _context.Activities.Where(a => a.Guid == guid).CountAsync() == 1;
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.ActivityId == id);
        }
    }
}
