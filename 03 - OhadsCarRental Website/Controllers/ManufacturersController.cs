using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Provides pages for manufacturers management.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class ManufacturersController : Controller
    {

        #region Private Fields

        /// <summary>
        /// Holds the logic for the CRUD of the manufacturers.
        /// </summary>
        private ManufacturersLogic logic = new ManufacturersLogic();

        #endregion

        /// <summary>
        /// Page displays: All the manufacturers in a list.
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                return View(logic.GetAllManufacturers());
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new List<Manufacturer>());
            }
        }

        /// <summary>
        /// Page displays: The details of a manufacturer.
        /// </summary>
        public ActionResult Details(int id = 0)
        {
            try
            {
                Manufacturer manufacturer = logic.GetManufacturerByID(id);
                if (manufacturer == null)
                    return HttpNotFound();

                return View(manufacturer);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new Manufacturer());
            }
        }

        /// <summary>
        /// Page displays: A form to add a new manufacturer.
        /// </summary>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Page displays: A form to add a new manufacturer. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Manufacturer manufacturer)
        {
            if (!ModelState.IsValid)
                return View(manufacturer);

            try
            {
                // Checks manufacturer already exists:
                if (logic.IsManufacturerExists(manufacturer.ManufacturerName))
                {
                    ViewBag.ErrorMessage = "Manufacturer already exists.";
                    return View(manufacturer);
                }
                else
                {
                    // Manufacturer is OK to insert:
                    logic.InsertManufacturer(manufacturer);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(manufacturer);
            }
        }

        /// <summary>
        /// Page displays: A form to edit a manufacturer.
        /// </summary>
        public ActionResult Edit(int id = 0)
        {
            try
            {
                Manufacturer manufacturer = logic.GetManufacturerByID(id);
                if (manufacturer == null)
                    return HttpNotFound();

                return View(manufacturer);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new Manufacturer());
            }
        }

        /// <summary>
        /// Page displays: A form to edit a manufacturer. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Manufacturer manufacturer)
        {
            if (!ModelState.IsValid)
                return View(manufacturer);

            try
            {
                logic.UpdateManufacturer(manufacturer);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(manufacturer);
            }
        }

        /// <summary>
        /// Page displays: A manufacturer to delete.
        /// </summary>
        public ActionResult Delete(int id = 0)
        {
            try
            {
                Manufacturer manufacturer = logic.GetManufacturerByID(id);
                if (manufacturer == null)
                    return HttpNotFound();

                return View(manufacturer);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new Manufacturer());
            }
        }

        /// <summary>
        /// Page displays: A manufacturer to delete. (POST)
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                logic.DeleteManufacturer(logic.GetManufacturerByID(id));
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
                return View(logic.GetManufacturerByID(id));
            }
        }

        /// <summary>
        /// Page displays: Proceed option, after delete has failed.
        /// </summary>
        public ActionResult DeleteFailed(int id = 0)
        {
            try
            {
                Manufacturer manufacturer = logic.GetManufacturerByID(id);
                if (manufacturer == null)
                    return HttpNotFound();

                return View(manufacturer);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new Manufacturer());
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
        /// Page displays: A manufacturer to delete collective.
        /// </summary>
        public ActionResult DeleteCollective(int id = 0)
        {
            try
            {
                Manufacturer manufacturer = logic.GetManufacturerByID(id);
                if (manufacturer == null)
                    return HttpNotFound();

                return View(manufacturer);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(logic.GetManufacturerByID(id));
            }
        }

        /// <summary>
        /// Page displays: A manufacturer to delete collective. (POST)
        /// </summary>
        [HttpPost, ActionName("DeleteCollective")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCollectiveConfirmed(int id)
        {
            try
            {
                logic.DeleteManufacturer(logic.GetManufacturerByID(id), true);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(logic.GetManufacturerByID(id));
            }
        }

    }
}
