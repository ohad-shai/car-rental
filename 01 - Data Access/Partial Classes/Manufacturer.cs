using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhadsCarRental
{
    [MetadataType(typeof(ManufacturerMetadata))]
    public partial class Manufacturer
    {
        public class ManufacturerMetadata
        {

            [Display(Name = "Manufacturer Name")]
            [Required(ErrorMessage = "Manufacturer Name is required.")]
            [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
            public object ManufacturerName { get; set; }

        }
    }
}