using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace PlanEvent.ViewModels
{
    public class LoginInputModel
    {
        [Required]
        [MinLength(4, ErrorMessage = "Username must have a length of at least 4 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Your username can only consist of letters and numbers")]
        public string Username { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password must contain at least: 1 lower case letter, 1 upper case letter, and one symbol")]
        public string Password { get; set; }
    }
}
