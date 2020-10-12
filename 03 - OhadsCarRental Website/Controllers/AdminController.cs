using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhadsCarRental
{
    /// <summary>
    /// Provides pages for admin management.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        #region Private Fields

        /// <summary>
        /// Holds the logic for the Roles!
        /// </summary>
        private RolesLogic rolesLogic = new RolesLogic();

        /// <summary>
        /// Holds the logic for the Users!
        /// </summary>
        private UsersLogic usersLogic = new UsersLogic();

        #endregion

        /// <summary>
        /// Page displays: All the admin management options.
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Page displays: (Users management option) displays a list of all the users to manage.
        /// </summary>
        public ActionResult AllUsers()
        {
            try
            {
                return View(usersLogic.GetAllUsers());
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(new List<User>());
            }
        }

        /// <summary>
        /// Page displays: A form to edit the role of a user.
        /// </summary>
        public ActionResult EditUserRole(int id = 0)
        {
            try
            {
                User user = usersLogic.GetUserByID(id);
                if (user == null)
                    return HttpNotFound();

                EditUserRoleViewModel model = new EditUserRoleViewModel();
                model.Username = user.Username;
                model.RoleName = user.Roles.FirstOrDefault().RoleName;

                ViewBag.RoleName = new SelectList(rolesLogic.GetAllRoles(), "RoleName", "RoleName", model.RoleName);
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                ViewBag.RoleName = new SelectList(new List<Role>(), "RoleName", "RoleName");
                return View(new EditUserRoleViewModel());
            }
        }

        /// <summary>
        /// Page displays: A form to edit the role of a user. (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserRole(EditUserRoleViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Some of the fields are invalid.";
                    ViewBag.RoleName = new SelectList(rolesLogic.GetAllRoles(), "RoleName", "RoleName", model.RoleName);
                    return View(model);
                }

                rolesLogic.UpdateRoleToUser(model.Username, model.RoleName);
                return RedirectToAction("AllUsers");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                ViewBag.RoleName = new SelectList(new List<Role>(), "RoleName", "RoleName");
                return View(model);
            }
        }

        /// <summary>
        /// Page displays: A user to delete.
        /// </summary>
        public ActionResult DeleteUser(int id = 0)
        {
            try
            {
                User user = usersLogic.GetUserByID(id);
                if (user == null)
                    return HttpNotFound();

                return View(user);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(usersLogic.GetUserByID(id));
            }
        }

        /// <summary>
        /// Page displays: A user to delete. (POST)
        /// </summary>
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(int id)
        {
            try
            {
                usersLogic.DeleteUser(usersLogic.GetUserByID(id));
                return RedirectToAction("AllUsers");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error has occurred. please try again later.";
                return View(usersLogic.GetUserByID(id));
            }
        }

    }
}
