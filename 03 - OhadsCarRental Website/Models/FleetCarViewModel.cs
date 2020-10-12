using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents a view model for a fleet car management.
    /// </summary>
    public class FleetCarViewModel
    {

        /// <summary>
        /// Holds the "Manufacturer" and "CarModel" DropDownLists.
        /// </summary>
        public ManufacturerCarModelDDLs DDLs = new ManufacturerCarModelDDLs();

        /// <summary>
        /// Holds the fleet car object.
        /// </summary>
        public FleetCar FleetCar { get; set; }

    }
}