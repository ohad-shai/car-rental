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
    /// Validates if the identity number is valid, by an algorithm to check IDs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class IdentityNumberValidationAttribute : ValidationAttribute, IClientValidatable
    {

        /// <summary>
        /// Validates if identity number is valid!
        /// </summary>
        /// <param name="idNum">The identity number to validate.</param>
        /// <returns>True of False, if ID is valid or not.</returns>
        private bool ValidateID(string idNum)
        {
            // Validates correct input:
            if (!System.Text.RegularExpressions.Regex.IsMatch(idNum, @"^\d{5,9}$"))
                return false; // Identity Number is invalid!

            // If the number is too short, adds leading 0000:
            if (idNum.Length < 9)
            {
                while (idNum.Length < 9)
                {
                    idNum = '0' + idNum;
                }
            }

            // CHECKS THE ID NUMBER:
            int counter = 0;
            int incNum;
            for (int i = 0; i < 9; i++)
            {
                incNum = Convert.ToInt32(idNum[i].ToString());
                incNum *= (i % 2) + 1;
                if (incNum > 9)
                    incNum -= 9;
                counter += incNum;
            }
            if (counter % 10 == 0)
                return true; // Identity Number is valid!
            else
                return false; // Identity Number is invalid!
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (!ValidateID(value.ToString())) // Checks the ID if not valid...
                {
                    // If so, the Identity Number is invalid!
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
                ValidationType = "idnumber", // This is the name jQuery adapter will use!
                ErrorMessage = ErrorMessage // This is the error message the client shows!
            };

            yield return rule;
        }
    }
}
