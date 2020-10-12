using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents a DropDownList for: Manufacturers and ManufacturerModels and ProductionYears.
    /// (By selecting a manufacturer, it shows all of the manufacturer models, then all existed production years).
    /// </summary>
    public class ManufacturerModelYearGearDDLs : BaseLogic
    {

        private List<SelectListItem> _manufacturers = new List<SelectListItem>();
        private List<SelectListItem> _manufacturerModels = new List<SelectListItem>();
        private List<SelectListItem> _years = new List<SelectListItem>();
        private List<SelectListItem> _gear = new List<SelectListItem>();

        public string SelectedManufacturer { get; set; }
        public string SelectedManufacturerModel { get; set; }
        public string SelectedYear { get; set; }
        public string SelectedGear { get; set; }

        /// <summary>
        /// Holds the DropDownList for the ManufacturerModels.
        /// </summary>
        public List<SelectListItem> ManufacturerModels
        {
            get { return _manufacturerModels; }
        }

        /// <summary>
        /// Holds the DropDownList for the Manufacturers.
        /// </summary>
        public List<SelectListItem> Manufacturers
        {
            get
            {
                List<Manufacturer> manufacturers = DB.Manufacturers.OrderBy(m => m.ManufacturerName).ToList(); // List of all manufacturers
                foreach (Manufacturer manufacturer in manufacturers)
                {
                    // Adds every manufacturer to the drop down list:
                    _manufacturers.Add(new SelectListItem() { Text = manufacturer.ManufacturerName,
                                                              Value = manufacturer.ManufacturerID.ToString() });
                }
                return _manufacturers;
            }
        }

        /// <summary>
        /// Holds the DropDownList for the Years.
        /// </summary>
        public List<SelectListItem> Years
        {
            get
            {
                List<CarModel> cars = DB.CarModels.GroupBy(c => c.ProductionYear)
                                                           .Select(grp => grp.FirstOrDefault())
                                                           .ToList(); // List of all distinct production years
                foreach (CarModel car in cars)
                {
                    // Adds every production year to the drop down list:
                    _years.Add(new SelectListItem()
                    {
                        Text = car.ProductionYear.ToString(),
                        Value = car.ProductionYear.ToString()
                    });
                }
                return _years;
            }
        }

        /// <summary>
        /// Holds the DropDownList for the Gear.
        /// </summary>
        public List<SelectListItem> Gear
        {
            get
            {
                _gear.Add(new SelectListItem() { Text = "Automatic", Value = "false" });
                _gear.Add(new SelectListItem() { Text = "Manual", Value = "true" });

                return _gear;
            }
        }

    }
}