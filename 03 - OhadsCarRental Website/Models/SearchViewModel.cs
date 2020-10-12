using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents a view model for a search.
    /// </summary>
    public class SearchViewModel
    {

        /// <summary>
        /// Holds all of the DropDownLists the view model needs.
        /// </summary>
        public ManufacturerModelYearGearDDLs DDLs = new ManufacturerModelYearGearDDLs();

        /// <summary>
        /// Holds the manufacturer ID of a car, to search.
        /// </summary>
        public int? ManufacturerID { get; set; }

        /// <summary>
        /// Holds the manufacturer model ID of a car, to search.
        /// </summary>
        public int? ManufacturerModelID { get; set; }

        /// <summary>
        /// Holds the production year of a car, to search.
        /// </summary>
        public int? ProductionYear { get; set; }

        /// <summary>
        /// Holds an indicator if the transmission (gear) of the car is manual or automatic, to search.
        /// </summary>
        public bool? ManualGear { get; set; }

        /// <summary>
        /// Holds the start date of the rental, to search available cars.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Holds the return date of the rental, to search available cars.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// Holds a free text to search in the manufacturer name or model name of a car.
        /// </summary>
        public string FreeText { get; set; }

    }
}