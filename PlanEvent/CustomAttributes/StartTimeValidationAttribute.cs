using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.CustomAttributes
{
    public class StartTimeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime now = DateTime.Now;
            DateTime entered = (DateTime)value;

            if(entered < now.AddDays(-2))
            {
                return new ValidationResult(errorMessage:
                    "The start time cannot be more than 2 days into the past");
            }

            if (entered > now.AddYears(1))
            {
                return new ValidationResult(errorMessage:
                    "The startDate cannot be more than a year into the future");
            }

            return ValidationResult.Success;
        }
    }
}
