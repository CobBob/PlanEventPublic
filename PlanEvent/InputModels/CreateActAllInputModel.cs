using PlanEvent.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PlanEvent.CustomAttributes;

namespace PlanEvent.InputModels
{
    public class CreateActAllInputModel
    {
        [DataType(DataType.Text)]
        [Required, MaxLength(40)]
        public string ActivityName { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(500)]
        public string Description { get; set; }

        [DataType(DataType.Text)]
        [Required, MaxLength(30)]
        public string OrganiserName { get; set; }
        
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Please enter a start date")]
        [StartTimeValidation]
        public DateTime StartTime {get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Please enter an end date")]
        [EndTimeValidation("StartTime")]
        public DateTime EndTime { get; set; }
    }
}
