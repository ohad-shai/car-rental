using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Provides pages for car models management.
    /// </summary>
    [Authorize(Roles = "Admin, Manager")]
    public class CarModelsController : Controller
    {

        #region Private Fields

        /// <summary>
        /// Holds the logic for the CRUD of the car models.
        /// </summary>
        private CarModelsLogic logic = new CarModelsLogic();

        #endregion

        /// <summary>
        /// Page displays: All the car models in a list.
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                return View(logic.GetAllCarModels());
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new List<CarModel>());
            }
        }

        /// <summary>
        /// Page displays: The details of a car model.
        /// </summary>
        public ActionResult Details(int id = 0)
        {
            try
            {
                CarModel carModel = logic.GetCarModelByID(id);
                if (carModel == null)
                    return HttpNotFound();

                return View(carModel);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new CarModel());
            }
        }

        /// <summary>
        /// Page displays: A form to add a new car model.
        /// </summary>
        public ActionResult Create()
        {
            return View(new CarModelViewModel());
        }

        /// <summary>
        /// Page displays: A form to add a new car model. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarModelViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                // Checks if car model already exists:
                if (logic.IsCarModelExists(model.CarModel))
                {
                    ViewBag.ErrorMessage = "Car model already exists.";
                    return View(model);
                }
                else
                {
                    // Car model is OK to insert:
                    logic.InsertCarModel(model.CarModel);
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
        /// Page displays: A form to edit a car model.
        /// </summary>
        public ActionResult Edit(int id = 0)
        {
            try
            {
                // Gets the car model by the ID:
                CarModel carModel = logic.GetCarModelByID(id);
                if (carModel == null)
                    return HttpNotFound();

                // Creates the model object:
                CarModelViewModel model = new CarModelViewModel();
                model.CarModel = carModel;
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new CarModelViewModel());
            }
        }

        /// <summary>
        /// Page displays: A form to edit a car model. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarModelViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                logic.UpdateCarModel(model.CarModel);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(model);
            }
        }

        /// <summary>
        /// Page displays: A car model to delete.
        /// </summary>
        public ActionResult Delete(int id = 0)
        {
            try
            {
                CarModel carModel = logic.GetCarModelByID(id);
                if (carModel == null)
                    return HttpNotFound();

                return View(carModel);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new CarModel());
            }
        }

        /// <summary>
        /// Page displays: A car model to delete. (POST)
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                logic.DeleteCarModel(logic.GetCarModelByID(id));
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                // If can't delete, because it has a relationship with other records, redirects to "DeleteFailed" action:
                return RedirectToAction("DeleteFailed", new { id });
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(logic.GetCarModelByID(id));
            }
        }

        /// <summary>
        /// Page displays: Proceed option, after delete has failed.
        /// </summary>
        public ActionResult DeleteFailed(int id = 0)
        {
            try
            {
                CarModel carMdl = logic.GetCarModelByID(id);
                if (carMdl == null)
                    return HttpNotFound();

                return View(carMdl);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new CarModel());
            }
        }

        /// <summary>
        /// Page displays: Proceed option, after delete has failed. (POST)
        /// </summary>
        [HttpPost, ActionName("DeleteFailed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFailedProceed(int id)
        {
            return RedirectToAction("DeleteCollective", new { id });
        }

        /// <summary>
        /// Page displays: A car model to delete collective.
        /// </summary>
        public ActionResult DeleteCollective(int id = 0)
        {
            try
            {
                CarModel carMdl = logic.GetCarModelByID(id);
                if (carMdl == null)
                    return HttpNotFound();

                return View(carMdl);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(logic.GetCarModelByID(id));
            }
        }

        /// <summary>
        /// Page displays: A car model to delete collective. (POST)
        /// </summary>
        [HttpPost, ActionName("DeleteCollective")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCollectiveConfirmed(int id)
        {
            try
            {
                logic.DeleteCarModel(logic.GetCarModelByID(id), true);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(logic.GetCarModelByID(id));
            }
        }

        #region AJAX Requests

        /// <summary>
        /// Gets a manufacturer's ID, and returns all of it's car models.
        /// </summary>
        /// <param name="manufacturerID">The ID of the manufacturer to find it's car models.</param>
        /// <returns>Returns a select list with all of the car models. If failed, returns null.</returns>
        public JsonResult GetCarModelsForManufacturer(string manufacturerID)
        {
            try
            {
                int id = int.Parse(manufacturerID); // Parses the ID of the manufacturer.

                // Gets the CarModels for the Manufacturer from the repository:
                List<CarModel> carModels = logic.GetCarModelsForManufacturer(id);

                List<SelectListItem> selectList = new List<SelectListItem>(); // Creates a new select list

                // Adds the car models to the SelectList:
                foreach (CarModel car in carModels)
                {
                    // Creates the car model text format: (Manufacturer ManufacturerModel ProductionYear Gear):
                    string carModelText = string.Format("{0} {1} {2} {3}", car.ManufacturerModel.Manufacturer.ManufacturerName,
                                                                           car.ManufacturerModel.ManufacturerModelName,
                                                                           car.ProductionYear,
                                                                           car.ManualGear ? "Manual" : "Automatic");
                    selectList.Add(new SelectListItem() { Text = carModelText, Value = car.CarModelID.ToString() });
                }

                return Json(selectList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
