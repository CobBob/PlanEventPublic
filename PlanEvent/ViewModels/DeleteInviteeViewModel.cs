using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.ViewModels
{
    public class DeleteInviteeViewModel
    {
        //public int ActivityId { get; set; }
        public Guid Guid { get; set; }
        public int InviteeId { get; set; }
        public string Name { get; set; }
    }
}
