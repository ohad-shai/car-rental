using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Provides pages for the Contact Us management.
    /// </summary>
    public class ContactsController : Controller
    {

        #region Private Fields

        /// <summary>
        /// Holds the logic for the Contact Us management.
        /// </summary>
        private ContactsLogic logic = new ContactsLogic();

        #endregion

        /// <summary>
        /// Page displays: List of all the Contact Us messages sent to the site.
        /// </summary>
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Watch()
        {
            try
            {
                return View(logic.GetAllContacts().OrderByDescending(c => c.DateTime).ToList());
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View();
            }
        }

        /// <summary>
        /// Page displays: The form to send the Contact Us message.
        /// </summary>
        public ActionResult Create()
        {
            return View();
        }

        #region AJAX Requests

        /// <summary>
        /// Submits a new Contact Us message.
        /// </summary>
        /// <param name="contact">The contact us message to submit.</param>
        /// <returns>True or False, if contact submission succeeded.</returns>
        [HttpPost]
        public JsonResult SubmitContact(Contact contact)
        {
            if (!ModelState.IsValid)
                return Json(false);

            try
            {
                logic.InsertContactMessage(contact);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        /// <summary>
        /// Updates unread contact message.
        /// </summary>
        /// <param name="contactID">The ID of the contact message to update.</param>
        /// <returns>True or False, if unread contact update succeeded.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public JsonResult UpdateUnreadContact(int contactID)
        {
            try
            {
                logic.UpdateUnreadContact(contactID);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        #endregion

    }
}
