using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Provides pages for orders management.
    /// </summary>
    [Authorize]
    public class OrdersController : Controller
    {

        #region Private Fields

        /// <summary>
        /// Holds the logic for the CRUD of the orders management.
        /// </summary>
        private OrdersLogic logic = new OrdersLogic();

        #endregion

        /// <summary>
        /// Page displays: A form to create a new rental.
        /// </summary>
        public ActionResult Create(string licensePlate, DateTime startDate, DateTime returnDate)
        {
            try
            {
                // Gets the car object, according to the license plate:
                FleetCarsLogic carsLogic = new FleetCarsLogic();
                FleetCar car = carsLogic.GetFleetCarByLicensePlate(licensePlate);

                // Checks if the car exists to order:
                if (car == null)
                    return new HttpNotFoundResult("Car is not found."); // If car not exists, returns error 404 not found.

                // Car exists, so creates the order view model object:
                OrderViewModel model = new OrderViewModel();
                model.LicensePlate = licensePlate;
                model.CarModelName = string.Format("{0} {1} {2} {3}", car.CarModel.ManufacturerModel.Manufacturer.ManufacturerName,
                                                                      car.CarModel.ManufacturerModel.ManufacturerModelName,
                                                                      car.CarModel.ProductionYear,
                                                                      car.CarModel.ManualGear ? "Manual" : "Automatic");
                model.CarImage = car.CarImage;
                model.DailyPrice = car.CarModel.DailyPrice;
                model.DayDelayPrice = car.CarModel.DayDelayPrice;
                model.StartDate = startDate.Date;
                model.ReturnDate = returnDate.Date;

                return View(model);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new OrderViewModel());
            }
        }

        /// <summary>
        /// Page displays: A form to create a new rental. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                FleetCarsLogic carsLogic = new FleetCarsLogic(); // Holds the logic for the cars in the fleet.

                // Checks if the car is available in the specified rental dates:
                if (carsLogic.CheckCarAvailability(model.LicensePlate, model.StartDate, model.ReturnDate))
                {
                    // Car is available, so:
                    // Inserts the rental to the database, and assigns the order number (ID):
                    int id = logic.InsertRental(model.LicensePlate, model.StartDate, model.ReturnDate, User.Identity.Name);

                    // After rental inserted, redirects to Order Receipt action, with the order number:
                    return RedirectToAction("OrderReceipt", new { id });
                }
                else
                {
                    // Car is not available:
                    ViewBag.ErrorMessage = "Car is not available in the specified dates.";
                    return View(model);
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(model);
            }
        }

        /// <summary>
        /// Page displays: A success message after ordering a car, with the order number,
        ///                Also, suggests a redirect options like: orders history, cars list, homepage.
        /// </summary>
        public ActionResult OrderReceipt(int id)
        {
            return View(id);
        }

        /// <summary>
        /// Page displays: A list of all the orders of the current user. (Rentals History).
        /// </summary>
        public ActionResult History()
        {
            try
            {
                return View(logic.GetRentalsForUser(User.Identity.Name));
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new List<Rental>());
            }
        }

        /// <summary>
        /// Page displays: A list of all the orders from all the users in the website.
        /// (Available only for Manager, and Admin).
        /// </summary>
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Watch()
        {
            try
            {
                return View(logic.GetAllRentals().OrderByDescending(r => r.RentalID));
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new List<Rental>());
            }
        }

        /// <summary>
        /// Page displays: A list of all the cars an employee can return.
        /// (Available only for Employee, and Admin).
        /// </summary>
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult CarsToReturn()
        {
            try
            {
                return View(logic.GetRentalsToCarReturn());
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new List<Rental>());
            }
        }

        /// <summary>
        /// Page displays: A list of all the cars an employee can return. (POST) - (For search by receipt option).
        /// (Available only for Employee, and Admin).
        /// </summary>
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        public ActionResult CarsToReturn(int receiptNum = 0)
        {
            try
            {
                List<Rental> model = new List<Rental>();
                Rental rental = logic.GetRentalByID(receiptNum); // Gets the rental by the receipt (ID).

                // Checks if a recipt is found:
                if (rental != null)
                    model.Add(rental);

                return View(model);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new List<Rental>());
            }
        }

        /// <summary>
        /// Page displays: A confirm message for returning a car, with all the details of the order,
        ///                so the employee can confirm the return of the car.
        /// (Available only for Employee, and Admin).
        /// </summary>
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult CarReturn(int id = 0)
        {
            try
            {
                return View(logic.GetRentalByID(id)); // Gets the details of the order.
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new Rental());
            }
        }

        /// <summary>
        /// Page displays: A confirm message for returning a car, with all the details of the order,
        ///                so the employee can confirm the return of the car. (POST)
        /// (Available only for Employee, and Admin).
        /// </summary>
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost, ActionName("CarReturn")]
        [ValidateAntiForgeryToken]
        public ActionResult CarReturnConfirmed(int rentalID)
        {
            try
            {
                logic.UpdateCarReturn(rentalID); // Updates that the car returned, and sets the actual return date to today.

                return RedirectToAction("CarsToReturn"); // Redirects to the view of the list of all the cars to return.
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new Rental());
            }
        }

    }
}
