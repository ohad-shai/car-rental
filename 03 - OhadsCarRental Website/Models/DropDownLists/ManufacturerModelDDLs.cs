using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents a DropDownList for: Manufacturers and ManufacturerModels.
    /// (By selecting a manufacturer, it shows all of the manufacturer models).
    /// </summary>
    public class ManufacturerModelDDLs : BaseLogic
    {

        private List<SelectListItem> _manufacturers = new List<SelectListItem>();
        private List<SelectListItem> _manufacturerModels = new List<SelectListItem>();

        [Required(ErrorMessage = "Manufacturer is required.")]
        public string SelectedManufacturer { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        public string SelectedManufacturerModel { get; set; }

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

    }
}