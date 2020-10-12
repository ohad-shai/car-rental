using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhadsCarRental
{
    [MetadataType(typeof(ManufacturerModelMetadata))]
    public partial class ManufacturerModel
    {
        public class ManufacturerModelMetadata
        {

            [Display(Name = "Manufacturer Model Name")]
            [Required(ErrorMessage = "Manufacturer Model Name is required.")]
            [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
            public object ManufacturerModelName { get; set; }

            [Display(Name = "Manufacturer")]
            [Required(ErrorMessage = "Manufacturer is required.")]
            public object ManufacturerID { get; set; }

        }
    }
}
