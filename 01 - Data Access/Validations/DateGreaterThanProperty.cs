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
    /// Validates that the property date value is greater than a provided property name of a date value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class DateGreaterThanPropertyAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Holds the property name, that holds the lesser date value.
        /// </summary>
        public string LesserPropName { get; private set; }
        /// <summary>
        /// Holds an indicator if to allow equal dates.
        /// </summary>
        public bool EqualDates { get; private set; }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="lesserPropName">The property name, that holds the lesser date value.</param>
        /// <param name="allowEqualDates">Indicator if to allow equal dates.</param>
        public DateGreaterThanPropertyAttribute(string lesserPropName, bool allowEqualDates = false)
            : base("{0} is invalid.")
        {
            LesserPropName = lesserPropName;
            EqualDates = allowEqualDates;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is DateTime)
            {
                // Gets the date value of the property:
                DateTime propDate = (DateTime)value; // DateTime.Parse(value.ToString());

                // Gets information about the provided lesser property name:
                var lesserPropInfo = validationContext.ObjectType.GetProperty(LesserPropName);
                // Checks if lesser property exists:
                if (lesserPropInfo == null)
                    return new ValidationResult(string.Format("Unknown property {0}.", LesserPropName));

                // Gets the value of the provided lesser property name:
                var lesserPropVal = lesserPropInfo.GetValue(validationContext.ObjectInstance, null);

                // Checks if there's a value in the lesser date property:
                if (lesserPropVal != null && lesserPropVal is DateTime)
                {
                    // Gets the date value of the property:
                    DateTime lesserPropDate = (DateTime)lesserPropVal;

                    // Check 1: if the property is greater than the LesserDate:
                    // Check 2: if the property is equal to the LesserDate, and equal dates is not allowed:
                    if ((propDate.Date < lesserPropDate.Date) ||
                        (propDate.Date == lesserPropDate.Date && !EqualDates))
                    {
                        // If so, the date is invalid:
                        var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                        return new ValidationResult(errorMessage);
                    }
                }
            }

            return ValidationResult.Success;
        }

        // For the client side validation:
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "dategreaterthanprop", // This is the name jQuery adapter will use!
                ErrorMessage = ErrorMessage // This is the error message the client shows!
            };

            // The parameters the client side validation uses:
            rule.ValidationParameters["lesserpropname"] = LesserPropName;
            rule.ValidationParameters["equaldates"] = EqualDates;

            yield return rule;
        }
    }
}
