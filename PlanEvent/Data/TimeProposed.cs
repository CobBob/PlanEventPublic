using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.Data
{
    public class TimeProposed
    {
        public int TimeProposedId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ICollection<ActivityTimeInvitee> ActivityTimeInvitees { get; set; }

        public TimeProposed()
        {
            ActivityTimeInvitees = new List<ActivityTimeInvitee>();
        }
    }
}
