using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhadsCarRental
{
    [MetadataType(typeof(CarModelMetadata))]
    public partial class CarModel
    {
        public class CarModelMetadata
        {

            [Display(Name = "Manufacturer Model")]
            [Required(ErrorMessage = "Manufacturer Model is required.")]
            public object ManufacturerModelID { get; set; }

            [Display(Name = "Production Year")]
            [Required(ErrorMessage = "Production Year is required.")]
            [Range(1900, 3000, ErrorMessage = "Must be between 1900 - 3000.")]
            public object ProductionYear { get; set; }

            [Display(Name = "Gear")]
            [Required(ErrorMessage = "Gear is required.")]
            public object ManualGear { get; set; }

            [Display(Name = "Daily Price")]
            [Required(ErrorMessage = "Daily Price is required.")]
            [DataType(DataType.Currency)]
            public object DailyPrice { get; set; }

            [Display(Name = "Day Delay Price")]
            [Required(ErrorMessage = "Day Delay Price is required.")]
            [DataType(DataType.Currency)]
            public object DayDelayPrice { get; set; }

        }
    }
}
