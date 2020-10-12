using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OhadsCarRental
{
    /// <summary>
    /// Provides pages for account management.
    /// </summary>
    public class AccountController : Controller
    {

        #region Private Fields

        /// <summary>
        /// Holds the logic for the User!
        /// </summary>
        private UsersLogic logic = new UsersLogic();

        #endregion

        /// <summary>
        /// Page displays: A form to register a new user.
        /// </summary>
        public ActionResult Register()
        {
            // If the user is already logged in, redirects him to his profile:
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("UserProfile");

            return View();
        }

        /// <summary>
        /// Page displays: A form to register a new user. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Checks if the username is already taken:
                if (logic.IsUsernameTaken(model.Username))
                {
                    ViewBag.ErrorMessage = "Username is already taken by someone else.";
                    return View();
                }
                else // New user is OK to register:
                {
                    logic.Register(model.Username,
                                   model.Password,
                                   model.FirstName,
                                   model.LastName,
                                   model.IdentityNumber,
                                   model.Email,
                                   model.BirthDate);

                    FormsAuthentication.SetAuthCookie(model.Username, false); // Sets the user already logged in!
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred while register. please try again later.";
                return View();
            }
        }

        /// <summary>
        /// Page displays: A form to login a user.
        /// </summary>
        public ActionResult Login(string returnUrl)
        {
            // If the user is already logged in, redirects him to his profile:
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("UserProfile");

            ViewBag.ReturnUrl = SetReturnUrl(returnUrl);
            return View();
        }

        /// <summary>
        /// Page displays: A form to login a user. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = SetReturnUrl(returnUrl);
                return View(model);
            }

            try
            {
                // Checks if the login is OK:
                if (logic.IsUserExist(model.Username, model.Password))
                {
                    // Successful login!
                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                    return RedirectToReturnUrl(returnUrl);
                }

                // Login failed:
                ViewBag.ReturnUrl = SetReturnUrl(returnUrl);
                ViewBag.ErrorMessage = "Username or Password is incorrect.";
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred while login. please try again later.";
                return View();
            }
        }

        /// <summary>
        /// Page performs: A logout operation to a logged in user.
        /// </summary>
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToReturnUrl(null);
        }

        /// <summary>
        /// Page displays: The user's profile details.
        /// </summary>
        [Authorize]
        public ActionResult UserProfile()
        {
            try
            {
                return View(logic.GetUserByUsername(User.Identity.Name));
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View();
            }
        }

        /// <summary>
        /// Page displays: Forgotten password information page.
        /// </summary>
        public ActionResult ForgottenPassword()
        {
            return View();
        }

        #region (Private) Action Helpers

        /// <summary>
        /// Sets the return URL after the Login, so that the user will be redirected to the correct last URL.
        /// </summary>
        /// <param name="returnUrl">The url from last session</param>
        /// <returns>The last URL the user needs to be redirected to.</returns>
        private string SetReturnUrl(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl)) // If there's already a return url, keeps on saving the URL
            {
                return returnUrl; // Returns the saved URL
            }
            else if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString())) // If there's a last request referrer
            {
                return Request.UrlReferrer.ToString(); // Returns the last URL
            }

            // Default:
            return "/Home/Index"; // HomePage
        }

        /// <summary>
        /// Redirects to the last URL the user came from, by the returnUrl.
        /// </summary>
        /// <param name="returnUrl">The last URL the user came from.</param>
        /// <returns>Redirection to the last url according to the return URL string.</returns>
        private ActionResult RedirectToReturnUrl(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl)) // If there's a return url
            {
                return Redirect(returnUrl);
            }
            else if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString())) // If there's a last request referrer
            {
                return Redirect(Request.UrlReferrer.ToString());
            }

            // Default:
            return RedirectToAction("Index", "Home"); // If there's nothing --> HomePage
        }

        #endregion

    }
}
