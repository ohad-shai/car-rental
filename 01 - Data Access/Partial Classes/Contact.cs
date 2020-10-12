using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhadsCarRental
{
    [MetadataType(typeof(ContactMetadata))]
    public partial class Contact
    {
        public class ContactMetadata
        {
            [Display(Name = "First Name *")]
            [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
            [Required(ErrorMessage = "First Name is required.")]
            public object FirstName { get; set; }

            [Display(Name = "Last Name *")]
            [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
            [Required(ErrorMessage = "Last Name is required.")]
            public object LastName { get; set; }

            [Display(Name = "Email *")]
            [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
            [Required(ErrorMessage = "Email is required.")]
            [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address.")]
            [EmailAddress(ErrorMessage = "Invalid Email Address.")]
            public object Email { get; set; }

            [DataType(DataType.PhoneNumber)]
            [StringLength(24, ErrorMessage = "Maximum length is 24 characters.")]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone Number.")]
            public object Phone { get; set; }

            [Display(Name = "Text *")]
            [Required(ErrorMessage = "Text is required.")]
            public object Text { get; set; }

        }
    }
}
