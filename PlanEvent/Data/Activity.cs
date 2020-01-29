using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.Data
{
    public class Activity
    {
        public int ActivityId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string AccountName { get; set; }

        public Guid Guid { get; set; }

        public ICollection<ActivityTimeInvitee> ActivityTimeInvitees { get; set; }

        public Activity()
        {
            ActivityTimeInvitees = new List<ActivityTimeInvitee>();
        }
    }
}
