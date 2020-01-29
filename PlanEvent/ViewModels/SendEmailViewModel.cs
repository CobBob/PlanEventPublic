using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PlanEvent.ViewModels
{
    public class SendEmailViewModel
    {
        public Guid Guid { get; set; }

        public string ToEmail { get; set; }

        public string AdditionalTextMessage { get; set; }
    }
}
