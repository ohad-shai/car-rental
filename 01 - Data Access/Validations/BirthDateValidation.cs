using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;

namespace OhadsCarRental.Validations
{
    /// <summary>
    /// Validates if the date of birth is valid, by finding the age from the date, 
    /// and checking if the age is valid in a specific range of age!
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class BirthDateValidationAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Holds the minimum valid age!
        /// </summary>
        public int MinAge { get; private set; }
        /// <summary>
        /// Holds the maximum valid age!
        /// </summary>
        public int MaxAge { get; private set; }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="minAge">The minimum valid age.</param>
        /// <param name="maxAge">The maximum valid age.</param>
        public BirthDateValidationAttribute(int minAge, int maxAge)
            : base("{0} is invalid.")
        {
            MinAge = minAge;
            MaxAge = maxAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (!(value is DateTime))
                    throw new Exception("The value is not DateTime for the BirthDateValidation.");

                DateTime birthDate = Convert.ToDateTime(value); // Gets the BirthDate of the user
                DateTime today = DateTime.Today; // Gets today's date
                int age = today.Year - birthDate.Year; // Calculates age

                // Checks if age is lower than minimum, or higher than maximum:
                if (age < MinAge || age > MaxAge)
                {
                    // If so, the BirthDate is invalid!
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
                ValidationType = "birthdate", // This is the name jQuery adapter will use!
                ErrorMessage = ErrorMessage // This is the error message the client shows!
            };

            // The parameters the client side validation uses:
            rule.ValidationParameters["minage"] = MinAge;
            rule.ValidationParameters["maxage"] = MaxAge;

            yield return rule;
        }
    }
}
