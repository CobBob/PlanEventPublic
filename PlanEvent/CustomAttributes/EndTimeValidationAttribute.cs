using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanEvent.CustomAttributes
{
    public class EndTimeValidationAttribute : ValidationAttribute
    {
        private string _startTimePropertyName;

        public EndTimeValidationAttribute(string startTimePropertyName)
        {
            _startTimePropertyName = startTimePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var StartTimeTestedInfo = validationContext.ObjectType.GetProperty(this._startTimePropertyName);
            if (StartTimeTestedInfo == null)
            {
                return new ValidationResult(string.Format("unknown property {0}", this._startTimePropertyName));
            }

            var startTimeValue = (DateTime) StartTimeTestedInfo.GetValue(validationContext.ObjectInstance, null);

            DateTime endTime = (DateTime)value;
            DateTime now = DateTime.Now;

            if (endTime < startTimeValue)
            {
                return new ValidationResult(errorMessage:
    "The end time has to come after the startTime");
            }

            if (endTime > startTimeValue.AddMonths(2))
            {
                return new ValidationResult(errorMessage:
    "Your activity cannot last more than 2 months");
            }

            return ValidationResult.Success;
        }
    }
}
