using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhadsCarRental
{
    [MetadataType(typeof(FleetCarMetadata))]
    public partial class FleetCar
    {
        public class FleetCarMetadata
        {

            [Display(Name = "License Plate")]
            [Required(ErrorMessage = "License Plate is required.")]
            [StringLength(7, ErrorMessage = "Maximum length is 7 characters.")]
            public string LicensePlate { get; set; }

            [Display(Name = "Car Model")]
            [Required(ErrorMessage = "Car Model is required.")]
            public int CarModelID { get; set; }

            [Display(Name = "Current Mileage")]
            [Required(ErrorMessage = "Current Mileage is required.")]
            public int CurrentMileage { get; set; }

            [Display(Name = "Image")]
            [Required(ErrorMessage = "Image is required.")]
            public string CarImage { get; set; }

            [Display(Name = "Is Proper")]
            [Required(ErrorMessage = "Propriety is required.")]
            public bool IsProper { get; set; }

        }
    }
}
