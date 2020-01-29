using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.ViewModels
{
    public class ActivityTimeCreateViewModel
    {
        public int TimeProposedId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
    }
}
