using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.Data
{
    public class Invitee
    {
        public int InviteeId { get; set; }

        public string Name { get; set; }

        public ICollection<ActivityTimeInvitee> ActivityTimeInvitees { get; set; }

        public Invitee()
        {
            ActivityTimeInvitees = new List<ActivityTimeInvitee>();
        }
    }
}
