using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OhadsCarRental.Validations;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents a view model for a car order (rental).
    /// </summary>
    public class OrderViewModel
    {

        public string CarModelName { get; set; }

        public string CarImage { get; set; }

        public decimal DailyPrice { get; set; }

        public decimal DayDelayPrice { get; set; }

        [Display(Name = "License Plate")]
        [Required(ErrorMessage = "License Plate is required.")]
        [RegularExpression(@"^[a-zA-Z\d]{7}$", ErrorMessage = "Invalid license plate.")]
        public string LicensePlate { get; set; }

        [Display(Name = "Pick Up Date")]
        [Required(ErrorMessage = "Pick Up Date is required.")]
        [DataType(DataType.Date)]
        [DateGreaterThanToday(true, ErrorMessage = "Invalid pick up date.")] // My custom validation :D
        public DateTime StartDate { get; set; }

        [Display(Name = "Return Date")]
        [Required(ErrorMessage = "Return Date is required.")]
        [DataType(DataType.Date)]
        [DateGreaterThanToday(true, ErrorMessage = "Invalid return date.")] // My custom validation :D
        [DateGreaterThanProperty("StartDate", true, ErrorMessage = "Must be greater than Pick Up Date.")] // My custom validation :D
        public DateTime ReturnDate { get; set; }

    }
}