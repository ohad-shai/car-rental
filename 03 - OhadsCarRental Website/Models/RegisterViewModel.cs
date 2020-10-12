using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OhadsCarRental.Validations;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents a view model for the user registration.
    /// </summary>
    public class RegisterViewModel
    {

        [Display(Name = "Username *")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^.{4,}$", ErrorMessage = "Minimum length is 4 characters.")]
        public string Username { get; set; }

        [Display(Name = "Password *")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^.{4,}$", ErrorMessage = "Minimum length is 4 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password *")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [RegularExpression(@"^.{4,}$", ErrorMessage = "Minimum length is 4 characters.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name *")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name *")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Display(Name = "Identity Number *")]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid Identity Number.")]
        [Required(ErrorMessage = "Identity Number is required.")]
        [IdentityNumberValidation(ErrorMessage = "Invalid Identity Number.")] // My custom validation :)
        public int IdentityNumber { get; set; }

        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [BirthDateValidation(18, 150, ErrorMessage = "You must be over 18 to register.")] // My custom validation :)
        public DateTime? BirthDate { get; set; }

    }
}