using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents a view model for a car model management.
    /// </summary>
    public class CarModelViewModel
    {

        /// <summary>
        /// Holds the "Manufacturer" and "ManufacturerModel" DropDownLists.
        /// </summary>
        public ManufacturerModelDDLs DDLs = new ManufacturerModelDDLs();

        /// <summary>
        /// Holds the "Gear" DropDownList.
        /// </summary>
        public GearDDL GearDDL = new GearDDL();

        /// <summary>
        /// Holds the car model object.
        /// </summary>
        public CarModel CarModel { get; set; }

    }
}