using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Provides pages for errors that may occur in the site.
    /// </summary>
    public class ErrorController : Controller
    {

        /// <summary>
        /// Error page for: General.
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Error page for: 403 - Forbidden.
        /// </summary>
        public ActionResult HttpError403()
        {
            return View();
        }

        /// <summary>
        /// Error page for: 404 - Page not found.
        /// </summary>
        public ActionResult HttpError404()
        {
            return View();
        }

        /// <summary>
        /// Error page for: 500 - Internal Server Error.
        /// </summary>
        public ActionResult HttpError500()
        {
            return View("Index"); // Shows general error.
        }

    }
}
