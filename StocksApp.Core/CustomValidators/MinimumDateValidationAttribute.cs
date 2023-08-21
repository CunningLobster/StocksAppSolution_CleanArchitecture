using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.CustomValidators
{
    internal class MinimumDateValidationAttribute : ValidationAttribute
    {
        public string MinDate { get; set; }
        public string DefaultErrorMessage { get; set; } = "The date and time should not be older then {0}";

        public MinimumDateValidationAttribute(string minDate)
        {
            MinDate = minDate;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            { 
                DateTime date = (DateTime)value;
                if (date < Convert.ToDateTime(MinDate))
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, MinDate));
                else
                    return ValidationResult.Success;
            }
            return null;
        }
    }
}
