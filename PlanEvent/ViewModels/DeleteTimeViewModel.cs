using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.ViewModels
{
    public class DeleteTimeViewModel
    {
        //public int ActivityId { get; set; }
        public Guid Guid { get; set; }
        public int TimeProposedId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
