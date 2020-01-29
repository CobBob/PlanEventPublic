using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.InputModels
{
    public class SendEmailInputModel
    {
        public Guid Guid { get; set; }


        [Required, Display(Name = "Invitee Email"), EmailAddress]
        public string ToEmail { get; set; }

        public string AdditionalTextMessage { get; set; }
    }
}
