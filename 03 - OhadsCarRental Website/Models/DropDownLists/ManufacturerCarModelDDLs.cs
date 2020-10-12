using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents a DropDownList for: Manufacturers and CarModels.
    /// (By selecting a manufacturer, it shows all of the car models).
    /// </summary>
    public class ManufacturerCarModelDDLs : BaseLogic
    {

        private List<SelectListItem> _manufacturers = new List<SelectListItem>();
        private List<SelectListItem> _carModels = new List<SelectListItem>();

        [Required(ErrorMessage = "Manufacturer is required.")]
        public string SelectedMfrToCarMdl { get; set; }

        [Required(ErrorMessage = "Car Model is required.")]
        public string SelectedCarMdlFromMfr { get; set; }

        /// <summary>
        /// Holds the DropDownList for the CarModels.
        /// </summary>
        public List<SelectListItem> CarModels
        {
            get { return _carModels; }
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
                    _manufacturers.Add(new SelectListItem()
                    {
                        Text = manufacturer.ManufacturerName,
                        Value = manufacturer.ManufacturerID.ToString()
                    });
                }
                return _manufacturers;
            }
        }

    }
}