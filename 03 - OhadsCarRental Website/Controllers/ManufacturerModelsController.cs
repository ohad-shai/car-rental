using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Provides pages for manufacturer models management.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class ManufacturerModelsController : Controller
    {

        #region Private Fields

        /// <summary>
        /// Holds the logic for the CRUD of the manufacturer models.
        /// </summary>
        private ManufacturerModelsLogic logic = new ManufacturerModelsLogic();

        #endregion

        /// <summary>
        /// Page displays: All the manufacturer models in a list.
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                return View(logic.GetAllManufacturerModels());
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new List<ManufacturerModel>());
            }
        }

        /// <summary>
        /// Page displays: The details of a manufacturer model.
        /// </summary>
        public ActionResult Details(int id = 0)
        {
            try
            {
                ManufacturerModel manufacturerModel = logic.GetManufacturerModelByID(id);
                if (manufacturerModel == null)
                    return HttpNotFound();

                return View(manufacturerModel);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new ManufacturerModel());
            }
        }

        /// <summary>
        /// Page displays: A form to add a new manufacturer model.
        /// </summary>
        public ActionResult Create()
        {
            try
            {
                ViewBag.ManufacturerID = new SelectList(logic.GetAllManufacturers(), "ManufacturerID", "ManufacturerName");
                return View();
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View();
            }
        }

        /// <summary>
        /// Page displays: A form to add a new manufacturer model. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ManufacturerModel manufacturerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ManufacturerID = new SelectList(logic.GetAllManufacturers(), "ManufacturerID", "ManufacturerName", manufacturerModel.ManufacturerID);
                    return View(manufacturerModel);
                }

                // Checks if manufacturer model already exists:
                if (logic.IsManufacturerModelExists(manufacturerModel))
                {
                    ViewBag.ErrorMessage = "Manufacturer model already exists.";
                    ViewBag.ManufacturerID = new SelectList(logic.GetAllManufacturers(), "ManufacturerID", "ManufacturerName", manufacturerModel.ManufacturerID);
                    return View(manufacturerModel);
                }
                else
                {
                    // Manufacturer model is OK to insert:
                    logic.InsertManufacturerModel(manufacturerModel);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(manufacturerModel);
            }
        }

        /// <summary>
        /// Page displays: A form to edit a manufacturer model.
        /// </summary>
        public ActionResult Edit(int id = 0)
        {
            try
            {
                ManufacturerModel manufacturerModel = logic.GetManufacturerModelByID(id);
                if (manufacturerModel == null)
                    return HttpNotFound();

                ViewBag.ManufacturerID = new SelectList(logic.GetAllManufacturers(), "ManufacturerID", "ManufacturerName", manufacturerModel.ManufacturerID);
                return View(manufacturerModel);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new ManufacturerModel());
            }
        }

        /// <summary>
        /// Page displays: A form to edit a manufacturer model. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManufacturerModel manufacturerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ManufacturerID = new SelectList(logic.GetAllManufacturers(), "ManufacturerID", "ManufacturerName", manufacturerModel.ManufacturerID);
                    return View(manufacturerModel);
                }

                logic.UpdateManufacturerModel(manufacturerModel);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(manufacturerModel);
            }
        }

        /// <summary>
        /// Page displays: A manufacturer model to delete.
        /// </summary>
        public ActionResult Delete(int id = 0)
        {
            try
            {
                ManufacturerModel manufacturerModel = logic.GetManufacturerModelByID(id);
                if (manufacturerModel == null)
                    return HttpNotFound();

                return View(manufacturerModel);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new ManufacturerModel());
            }
        }

        /// <summary>
        /// Page displays: A manufacturer model to delete. (POST)
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                logic.DeleteManufacturerModel(logic.GetManufacturerModelByID(id));
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
                return View(logic.GetManufacturerModelByID(id));
            }
        }

        /// <summary>
        /// Page displays: Proceed option, after delete has failed.
        /// </summary>
        public ActionResult DeleteFailed(int id = 0)
        {
            try
            {
                ManufacturerModel mfrMdl = logic.GetManufacturerModelByID(id);
                if (mfrMdl == null)
                    return HttpNotFound();

                return View(mfrMdl);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new ManufacturerModel());
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
        /// Page displays: A manufacturer model to delete collective.
        /// </summary>
        public ActionResult DeleteCollective(int id = 0)
        {
            try
            {
                ManufacturerModel mfrMdl = logic.GetManufacturerModelByID(id);
                if (mfrMdl == null)
                    return HttpNotFound();

                return View(mfrMdl);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(logic.GetManufacturerModelByID(id));
            }
        }

        /// <summary>
        /// Page displays: A manufacturer model to delete collective. (POST)
        /// </summary>
        [HttpPost, ActionName("DeleteCollective")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCollectiveConfirmed(int id)
        {
            try
            {
                logic.DeleteManufacturerModel(logic.GetManufacturerModelByID(id), true);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(logic.GetManufacturerModelByID(id));
            }
        }

        #region AJAX Requests

        /// <summary>
        /// Gets a manufacturer's ID, and returns all of it's manufacturer models.
        /// </summary>
        /// <param name="manufacturerID">The ID of the manufacturer to find it's models.</param>
        /// <returns>Returns a select list with all of the manufacturer models. If failed, returns null.</returns>
        [AllowAnonymous]
        public JsonResult GetModelsForManufacturer(string manufacturerID)
        {
            try
            {
                int id = int.Parse(manufacturerID); // Parses the ID of the manufacturer.

                // Gets the ManufacturerModels for the Manufacturer from the repository:
                List<ManufacturerModel> manufacturerModels = logic.GetModelsForManufacturer(id);

                List<SelectListItem> selectList = new List<SelectListItem>(); // Creates a new select list

                // Adds the manufacturer models to the SelectList:
                foreach (ManufacturerModel model in manufacturerModels)
                    selectList.Add(new SelectListItem() { Text = model.ManufacturerModelName, Value = model.ManufacturerModelID.ToString() });

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
