using PlanEvent.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.InputModels
{
    public class AddTimeInputModel
    {
        //public int ActivityId { get; set; }
        public Guid Guid { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Please enter a start date")]
        [StartTimeValidation]
        public DateTime StartTime { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Please enter an end date")]
        [EndTimeValidation("StartTime")]
        public DateTime EndTime { get; set; }
    }
}
