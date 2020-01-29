using PlanEvent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.ViewModels
{
    public class ActivityEditViewModel
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsOrganiser { get; set; }

        public ICollection<Invitee> Invitees { get; set; }
        public ICollection<TimeProposed> TimeProposeds { get; set; }
        public ICollection<ActivityTimeInvitee> ActivityTimeInvitees { get; set; }

        public ActivityEditViewModel()
        {
            Invitees = new List<Invitee>();
            TimeProposeds = new List<TimeProposed>();
            ActivityTimeInvitees = new List<ActivityTimeInvitee>();
        }
    }
}
