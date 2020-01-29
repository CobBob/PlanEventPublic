using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.Data
{
    public enum AVAILABILITY
    {
        X = 0,
        Y = 1,
        M = 2,
    }

    public class ActivityTimeInvitee
    {
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

        public int TimeProposedId { get; set; }
        public TimeProposed TimeProposed { get; set; }

        public int InviteeId { get; set; }
        public Invitee Invitee { get; set; }

        public AVAILABILITY Availability { get; set; }
    }
}
