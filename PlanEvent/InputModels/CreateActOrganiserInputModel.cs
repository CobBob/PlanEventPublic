using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.InputModels
{
    public class CreateActOrganiserInputModel
    {
        //TODO: the name and other things can be validated in the input models, but what if a user tries to circumvent the validation 
        //by doing wierd stuff with the url, prevent stuff stored in database through an additional validation
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter your (nick) name")]
        [MaxLength(30, ErrorMessage = "Please enter a name that is smaller than 30 Characters")]
        public string OrganiserName { get; set; }
    }
}
