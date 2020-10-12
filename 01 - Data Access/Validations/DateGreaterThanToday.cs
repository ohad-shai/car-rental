using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OhadsCarRental.Validations
{
    /// <summary>
    /// Validates that the property's date value is greater than today's date.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class DateGreaterThanTodayAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Holds an indicator if to allow equal dates.
        /// </summary>
        public bool EqualDates { get; private set; }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="lesserDateString">The string date value, that must be lesser than the property value.</param>
        /// <param name="allowEqualDates">Indicator if to allow equal dates.</param>
        public DateGreaterThanTodayAttribute(bool allowEqualDates = false)
            : base("{0} is invalid.")
        {
            EqualDates = allowEqualDates;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is DateTime)
            {
                // Gets the date value of the property:
                DateTime propDate = DateTime.Parse(value.ToString());
                DateTime today = DateTime.Today;

                // Check 1: if the property is greater than today's date:
                // Check 2: if the property is equal to today's date, and equal dates is not allowed:
                if ((propDate.Date < today) ||
                    (propDate.Date == today && !EqualDates))
                {
                    // If so, the date is invalid:
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }

        // For the client side validation:
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "dategreaterthantoday", // This is the name jQuery adapter will use!
                ErrorMessage = ErrorMessage // This is the error message the client shows!
            };

            // The parameters the client side validation uses:
            rule.ValidationParameters["equaldates"] = EqualDates;

            yield return rule;
        }
    }
}
