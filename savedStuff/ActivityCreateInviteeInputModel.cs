using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.InputModels
{
    public class ActivityCreateInviteeInputModel
    {
        public int InviteeId { get; set; }
        public string Name { get; set; }
        public int ActivityId { get; set; }
    }
}
