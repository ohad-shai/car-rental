using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents a view model for the user login.
    /// </summary>
    public class LoginViewModel
    {

        [Display(Name = "Username")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^.{4,}$", ErrorMessage = "Minimum length is 4 characters.")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [StringLength(50, ErrorMessage = "The field has maximum length of 50.")]
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^.{4,}$", ErrorMessage = "Minimum length is 4 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Keep me logged in")]
        public bool RememberMe { get; set; }

    }
}