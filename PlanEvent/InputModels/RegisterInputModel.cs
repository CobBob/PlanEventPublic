using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.ViewModels
{
    public class RegisterInputModel : LoginInputModel
    {
        [Compare("Password", ErrorMessage = "Confirm Password doesn't match, Type again!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
