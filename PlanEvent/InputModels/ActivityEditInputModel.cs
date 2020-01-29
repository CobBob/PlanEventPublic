using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.InputModels
{
    public class ActivityEditInputModel
    {
        //TODO: is it secure to be able to pass this thing around ActivityId?
        //public int ActivityId { get; set; }
        public Guid Guid { get; set; }

        //[DataType(DataType.Text)]
        //[Required(ErrorMessage = "Please enter your a name For the activity")]
        //[MaxLength(40, ErrorMessage = "Please enter a name that is smaller than 40 Characters")]
        //public string Name { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(500, ErrorMessage = "Description must be under 500 characters")]
        public string Description { get; set; }

        //TODO: maybe validate this: ActivityTimeInviteeTableIds in inputmodel
        public string[] ActivityTimeInviteeTableIds { get; set; }

        public ActivityEditInputModel()
        {
            ActivityTimeInviteeTableIds = new string[0];
        }
    }
}
