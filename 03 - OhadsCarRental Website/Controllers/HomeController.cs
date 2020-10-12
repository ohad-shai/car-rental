using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Provides general pages for the website.
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        /// Page displays: The Homepage of the site.
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Page displays: The About page of the site.
        /// </summary>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Page displays: Help page for the site.
        /// </summary>
        public ActionResult Help()
        {
            return View();
        }

        /// <summary>
        /// Page displays: Privacy information page for the site.
        /// </summary>
        public ActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Page displays: Terms and Conditions page for the site.
        /// </summary>
        public ActionResult Terms()
        {
            return View();
        }

    }
}
