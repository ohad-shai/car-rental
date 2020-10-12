using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents a DropDownList for: Gear (Transmission), for a car model.
    /// (By clicking the gear select list, it shows "Automatic" or "Manual" gear).
    /// </summary>
    public class GearDDL : BaseLogic
    {

        private List<SelectListItem> _gear = new List<SelectListItem>();

        [Required(ErrorMessage = "Gear is required.")]
        public string SelectedGear { get; set; }

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