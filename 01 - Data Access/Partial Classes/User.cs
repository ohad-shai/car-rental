using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OhadsCarRental.Validations;

namespace OhadsCarRental
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public class UserMetadata
        {

            [Display(Name = "Username *")]
            [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
            [Required(ErrorMessage = "Username is required.")]
            [RegularExpression(@"^.{4,}$", ErrorMessage = "Minimum length is 4 characters.")]
            public object Username { get; set; }

            [Display(Name = "Password *")]
            [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
            [Required(ErrorMessage = "Password is required.")]
            [RegularExpression(@"^.{4,}$", ErrorMessage = "Minimum length is 4 characters.")]
            [DataType(DataType.Password)]
            public object Password { get; set; }

            [Display(Name = "First Name *")]
            [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
            [Required(ErrorMessage = "First Name is required.")]
            public object FirstName { get; set; }

            [Display(Name = "Last Name *")]
            [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
            [Required(ErrorMessage = "Last Name is required.")]
            public object LastName { get; set; }

            [Display(Name = "Identity Number *")]
            [Range(0, int.MaxValue, ErrorMessage = "Invalid Identity Number.")]
            [Required(ErrorMessage = "Identity Number is required.")]
            [IdentityNumberValidation(ErrorMessage = "Invalid Identity Number.")] // My custom validation :)
            public object IdentityNumber { get; set; }

            [Display(Name = "Email")]
            [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
            [DataType(DataType.EmailAddress)]
            [EmailAddress(ErrorMessage = "Invalid Email Address.")]
            public object Email { get; set; }

            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            [BirthDateValidation(18, 150, ErrorMessage = "You must be over 18 to register.")] // My custom validation :)
            public object BirthDate { get; set; }

        }
    }

}
