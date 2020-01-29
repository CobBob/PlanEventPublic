using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.InputModels
{
    public class CreateActActivityInputModel
    {
        public string OrganiserName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter your a name For the activity")]
        [MaxLength(40, ErrorMessage = "Please enter a name that is smaller than 40 Characters")]
        public string ActivityName { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(500, ErrorMessage = "Description must be under 500 characters")]
        public string Description { get; set; }
    }
}
