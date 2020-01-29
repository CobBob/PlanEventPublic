using PlanEvent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.ViewModels
{
    public class FullActivityViewModel
    {
        public string ActivityName { get; set; }
        public string Description { get; set; }

        public string OrganiserName { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
