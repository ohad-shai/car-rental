using System.Web.Security;

namespace OhadsCarRental
{
    /// <summary>
    /// Role provider class for all security operations in MVC.
    /// </summary>
    public class OhadsCarRentalRoleProvider : RoleProvider
    {
        private string _applicationName = "OhadsCarRental Website"; // The application name.
        private RolesLogic _rolesLogic = new RolesLogic(); // Business logic object.

        /// <summary>
        /// Application name.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                _applicationName = value;
            }
        }

        /// <summary>
        /// Add several users to serveral roles.
        /// </summary>
        /// <param name="usernames">All usernames to add to all given roles.</param>
        /// <param name="roleNames">All roles to add the usernames to.</param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            _rolesLogic.AddUsersToRoles(usernames, roleNames);
        }

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="roleName">The new role name.</param>
        public override void CreateRole(string roleName)
        {
            _rolesLogic.CreateRole(roleName);
        }

        /// <summary>
        /// Delete existing role.
        /// </summary>
        /// <param name="roleName">The role name to delete.</param>
        /// <param name="throwOnPopulatedRole">Whether to throw an exception if there are users attached to this role.</param>
        /// <returns>Returns true if succeeds to delete.</returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return _rolesLogic.DeleteRole(roleName, throwOnPopulatedRole);
        }

        /// <summary>
        /// Finds which users exists in given role by username partial match.
        /// </summary>
        /// <param name="roleName">The role to search in.</param>
        /// <param name="usernameToMatch">The partial or full username to search.</param>
        /// <returns>Returns all users match to given string and role name.</returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return _rolesLogic.FindUsersInRole(roleName, usernameToMatch);
        }

        /// <summary>
        /// Get all existing roles.
        /// </summary>
        /// <returns>Returns all existing role names.</returns>
        public override string[] GetAllRoles()
        {
            return _rolesLogic.GetAllRoleNames();
        }

        /// <summary>
        /// Get all roles for a specific user.
        /// </summary>
        /// <param name="username">The given username.</param>
        /// <returns>Returns all roles belongs to the given username.</returns>
        public override string[] GetRolesForUser(string username)
        {
            return _rolesLogic.GetRolesForUser(username);
        }

        /// <summary>
        /// Gets all usernames associated with a given role.
        /// </summary>
        /// <param name="roleName">The given role name.</param>
        /// <returns>Returns all user names associated with the given role.</returns>
        public override string[] GetUsersInRole(string roleName)
        {
            return _rolesLogic.GetUsersInRole(roleName);
        }

        /// <summary>
        /// Check if user is in a given role.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="roleName">The given role name to check.</param>
        /// <returns>Returns true if given username is associated with the given role name.</returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            return _rolesLogic.IsUserInRole(username, roleName);
        }

        /// <summary>
        /// Removes given users from a given roles.
        /// </summary>
        /// <param name="usernames">The usernames to remove from the given roles.</param>
        /// <param name="roleNames">The given roles to remove the given users from.</param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            _rolesLogic.RemoveUsersFromRoles(usernames, roleNames);
        }

        /// <summary>
        /// Check if a role exist.
        /// </summary>
        /// <param name="roleName">The given role name to check.</param>
        /// <returns>Returns true if given role name exist.</returns>
        public override bool RoleExists(string roleName)
        {
            return _rolesLogic.RoleExists(roleName);
        }

        /// <summary>
        /// Updates a user's role.
        /// </summary>
        /// <param name="username">The username to edit the role.</param>
        /// <param name="roleName">The user's new role.</param>
        public void UpdateRoleToUser(string username, string roleName)
        {
            _rolesLogic.UpdateRoleToUser(username, roleName);
        }

    }
}