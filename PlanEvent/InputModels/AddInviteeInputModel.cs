using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.InputModels
{
    public class AddInviteeInputModel
    {
        //public int ActivityId { get; set; }
        public Guid Guid { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter your (nick) name")]
        [MaxLength(30, ErrorMessage = "Please enter a name that is smaller than 30 Characters")]
        public string Name { get; set; }
    }
}
