using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OhadsCarRental
{
    /// <summary>
    /// Provides pages for the management of the cars in the fleet.
    /// </summary>
    [Authorize(Roles = "Admin, Manager")]
    public class FleetCarsController : Controller
    {

        #region Private Fields

        /// <summary>
        /// Holds the logic for the CRUD of the cars in the fleet.
        /// </summary>
        private FleetCarsLogic logic = new FleetCarsLogic();

        #endregion

        #region CRUD for Admins/Managers

        /// <summary>
        /// Page displays: All the cars in the fleet in a list.
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                return View(logic.GetAllFleetCars());
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new List<FleetCar>());
            }
        }

        /// <summary>
        /// Page displays: The details of a car in the fleet.
        /// </summary>
        public ActionResult Details(string licensePlate)
        {
            try
            {
                FleetCar fleetCar = logic.GetFleetCarByLicensePlate(licensePlate);
                if (fleetCar == null)
                    return HttpNotFound();

                return View(fleetCar);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new FleetCar());
            }
        }

        /// <summary>
        /// Page displays: A form to add a new car to the fleet.
        /// </summary>
        public ActionResult Create()
        {
            return View(new FleetCarViewModel());
        }

        /// <summary>
        /// Page displays: A form to add a new car to the fleet. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FleetCarViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                // Checks if a car in the fleet already exists:
                if (logic.IsFleetCarExists(model.FleetCar.LicensePlate))
                {
                    ViewBag.ErrorMessage = "Car already exists in the fleet.";
                    return View(model);
                }
                else
                {
                    // Car in the fleet is OK to insert:
                    logic.InsertFleetCar(model.FleetCar);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(model);
            }
        }

        /// <summary>
        /// Page displays: A form to edit a car in the fleet.
        /// </summary>
        public ActionResult Edit(string licensePlate)
        {
            try
            {
                FleetCar fleetCar = logic.GetFleetCarByLicensePlate(licensePlate);
                if (fleetCar == null)
                    return HttpNotFound();

                FleetCarViewModel model = new FleetCarViewModel();
                model.FleetCar = fleetCar;
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new FleetCarViewModel());
            }
        }

        /// <summary>
        /// Page displays: A form to edit a car in the fleet. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FleetCarViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                logic.UpdateFleetCar(model.FleetCar);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(model);
            }
        }

        /// <summary>
        /// Page displays: A car in the fleet to delete.
        /// </summary>
        public ActionResult Delete(string licensePlate)
        {
            try
            {
                FleetCar fleetCar = logic.GetFleetCarByLicensePlate(licensePlate);
                if (fleetCar == null)
                    return HttpNotFound();

                return View(fleetCar);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new FleetCar());
            }
        }

        /// <summary>
        /// Page displays: A car in the fleet to delete. (POST)
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string licensePlate)
        {
            try
            {
                logic.DeleteFleetCar(logic.GetFleetCarByLicensePlate(licensePlate));
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                // If can't delete, because it has a relationship with other records, redirects to "DeleteFailed" action:
                return RedirectToAction("DeleteFailed", new { licensePlate });
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(logic.GetFleetCarByLicensePlate(licensePlate));
            }
        }

        /// <summary>
        /// Page displays: Proceed option, after delete has failed.
        /// </summary>
        public ActionResult DeleteFailed(string licensePlate)
        {
            try
            {
                FleetCar fCar = logic.GetFleetCarByLicensePlate(licensePlate);
                if (fCar == null)
                    return HttpNotFound();

                return View(fCar);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new FleetCar());
            }
        }

        /// <summary>
        /// Page displays: Proceed option, after delete has failed. (POST)
        /// </summary>
        [HttpPost, ActionName("DeleteFailed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFailedProceed(string licensePlate)
        {
            return RedirectToAction("DeleteCollective", new { licensePlate });
        }

        /// <summary>
        /// Page displays: A car in the fleet to delete collective.
        /// </summary>
        public ActionResult DeleteCollective(string licensePlate)
        {
            try
            {
                FleetCar fCar = logic.GetFleetCarByLicensePlate(licensePlate);
                if (fCar == null)
                    return HttpNotFound();

                return View(fCar);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(logic.GetFleetCarByLicensePlate(licensePlate));
            }
        }

        /// <summary>
        /// Page displays: A car in the fleet to delete collective. (POST)
        /// </summary>
        [HttpPost, ActionName("DeleteCollective")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCollectiveConfirmed(string licensePlate)
        {
            try
            {
                logic.DeleteFleetCar(logic.GetFleetCarByLicensePlate(licensePlate), true);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(logic.GetFleetCarByLicensePlate(licensePlate));
            }
        }

        #endregion

        /// <summary>
        /// Page displays: The fleet of cars.
        /// </summary>
        [AllowAnonymous]
        public ActionResult Watch()
        {
            // Returns the 'search' page, to show all the cars:
            return View("Search", new SearchViewModel());
        }

        /// <summary>
        /// Page displays: Details of a car in the fleet.
        /// </summary>
        [AllowAnonymous]
        public ActionResult CarDetails(string licensePlate)
        {
            try
            {
                FleetCar fleetCar = logic.GetFleetCarByLicensePlate(licensePlate);
                if (fleetCar == null)
                    return HttpNotFound();

                return View(fleetCar);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new FleetCar());
            }
        }

        /// <summary>
        /// Page displays: Search for cars in the fleet.
        /// * Additional Information: This action gets the provided parameters in the route,
        ///                           and searches according to the provided parameters.
        ///                           Also, parameters are given only in a simple search operation.
        /// </summary>
        [AllowAnonymous]
        public ActionResult Search(string freeText, DateTime? startDate, DateTime? returnDate)
        {
            SearchViewModel viewModel = new SearchViewModel();

            // Checks the provided search filters, and assigns it to the view model of search:
            // -------------------------------------------------------------------------------
            if (!string.IsNullOrEmpty(freeText))
                viewModel.FreeText = freeText;

            if (startDate.HasValue)
                viewModel.StartDate = startDate.Value;

            if (returnDate.HasValue)
                viewModel.ReturnDate = returnDate.Value;
            // -------------------------------------------------------------------------------

            return View(viewModel);
        }

        #region AJAX Requests

        /// <summary>
        /// Searches for cars in the fleet according to the search filters provided.
        /// </summary>
        /// <param name="model">The model of search filter values.</param>
        /// <returns>All the cars in the fleet that satisfies the condition.</returns>
        [AllowAnonymous]
        [HttpGet]
        public JsonResult AdvancedSearch(SearchViewModel model)
        {
            if (!ModelState.IsValid)
                return Json("INVALID", JsonRequestBehavior.AllowGet);

            try
            {
                // After getting search results, creates an anonymous list, in order to send it through JSON:
                var results = logic.SearchCars(model.ManufacturerID, model.ManufacturerModelID, model.ProductionYear,
                                               model.ManualGear, model.StartDate, model.ReturnDate, model.FreeText)
                                               .Select(c => new
                                               {
                                                   LicensePlate = c.LicensePlate,
                                                   Manufacturer = c.ManufacturerName,
                                                   ManufacturerModel = c.ManufacturerModelName,
                                                   Year = c.ProductionYear,
                                                   Gear = c.ManualGear,
                                                   DailyPrice = c.DailyPrice,
                                                   DayDelayPrice = c.DayDelayPrice,
                                                   CarImage = c.CarImage
                                               });

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Checks an availability of a car, according to the rental dates provided.
        /// </summary>
        /// <returns>Returns indicator if available, true or false.</returns>
        [AllowAnonymous]
        [HttpGet]
        public JsonResult CheckCarAvailability(string licensePlate, DateTime startDate, DateTime returnDate)
        {
            try
            {
                var availability = logic.CheckCarAvailability(licensePlate, startDate, returnDate);
                return Json(availability, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
