using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace OhadsCarRental
{
    /// <summary>
    /// Logic for Roles table.
    /// </summary>
    public class RolesLogic : BaseLogic
    {

        /// <summary>
        /// Add user to a specific role.
        /// </summary>
        /// <param name="username">The username to add to the role.</param>
        /// <param name="roleName">The role to add the user to.</param>
        public void AddUser2Role(string username, string roleName)
        {
            Role role2Add = DB.Roles.First(r => r.RoleName == roleName);
            User user2Add = DB.Users.First(u => u.Username == username);
            user2Add.Roles.Add(role2Add);
            DB.SaveChanges();
        }

        /// <summary>
        /// Add several users to serveral roles.
        /// </summary>
        /// <param name="usernames">All usernames to add to all given roles.</param>
        /// <param name="roleNames">All roles to add the usernames to.</param>
        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (string username in usernames)
                foreach (string roleName in roleNames)
                    AddUser2Role(username, roleName);
        }

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="roleName">The new role name.</param>
        public void CreateRole(string roleName)
        {
            Role role = new Role { RoleName = roleName };
            DB.Roles.Add(role);
            DB.SaveChanges();
        }

        /// <summary>
        /// Update existing role.
        /// </summary>
        /// <param name="roleID">The role ID.</param>
        /// <param name="roleName">The role's new name.</param>
        public void UpdateRole(int roleID, string roleName)
        {
            Role role = DB.Roles.First(r => r.RoleID == roleID);
            role.RoleName = roleName;
            DB.SaveChanges();
        }

        /// <summary>
        /// Delete existing role.
        /// </summary>
        /// <param name="roleName">The role name to delete.</param>
        /// <param name="throwOnPopulatedRole">Whether to throw an exception if there are users attached to this role.</param>
        /// <returns>Returns true if succeeds to delete.</returns>
        public bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            try
            {
                Role role = DB.Roles.First(r => r.RoleName == roleName);
                DB.Roles.Remove(role);
                DB.SaveChanges();
                return true;
            }
            catch
            {
                if (throwOnPopulatedRole)
                    throw;
                return false;
            }
        }

        /// <summary>
        /// Finds which users exists in given role by username partial match.
        /// </summary>
        /// <param name="roleName">The role to search in.</param>
        /// <param name="usernameToMatch">The partial or full username to search.</param>
        /// <returns>Returns all users match to given string and role name.</returns>
        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return DB.Users.Where(u => u.Username.Contains(usernameToMatch) && u.Roles.Any(r => r.RoleName == roleName)).Select(u => u.Username).ToArray();
        }

        /// <summary>
        /// Gets all existing role names.
        /// </summary>
        /// <returns>Returns all existing role names.</returns>
        public string[] GetAllRoleNames()
        {
            return DB.Roles.Select(r => r.RoleName).ToArray();
        }

        /// <summary>
        /// Get all roles for a specific user.
        /// </summary>
        /// <param name="username">The given username.</param>
        /// <returns>Returns all roles belongs to the given username.</returns>
        public string[] GetRolesForUser(string username)
        {
            return DB.Users.First(u => u.Username == username).Roles.Select(r => r.RoleName).ToArray();
        }

        /// <summary>
        /// Gets all usernames associated with a given role.
        /// </summary>
        /// <param name="roleName">The given role name.</param>
        /// <returns>Returns all user names associated with the given role.</returns>
        public string[] GetUsersInRole(string roleName)
        {
            return DB.Roles.First(r => r.RoleName == roleName).Users.Select(u => u.Username).ToArray();
        }

        /// <summary>
        /// Check if user is in a given role.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="roleName">The given role name to check.</param>
        /// <returns>Returns true if given username is associated with the given role name.</returns>
        public bool IsUserInRole(string username, string roleName)
        {
            return DB.Roles.First(r => r.RoleName == roleName).Users.Any(u => u.Username == username);
        }

        /// <summary>
        /// Removes given users from a given role.
        /// </summary>
        /// <param name="usernames">The usernames to remove from the given role.</param>
        /// <param name="roleName">The given role to remove the given users from.</param>
        private void RemoveUsersFromRole(string[] usernames, string roleName)
        {
            Role role = DB.Roles.First(r => r.RoleName == roleName);
            foreach (string username in usernames)
                DB.Users.First(u => u.Username == username).Roles.Remove(role);
            DB.SaveChanges();
        }

        /// <summary>
        /// Removes given users from a given roles.
        /// </summary>
        /// <param name="usernames">The usernames to remove from the given roles.</param>
        /// <param name="roleNames">The given roles to remove the given users from.</param>
        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (string roleName in roleNames)
                RemoveUsersFromRole(usernames, roleName);
        }

        /// <summary>
        /// Check if a role exist.
        /// </summary>
        /// <param name="roleName">The given role name to check.</param>
        /// <returns>Returns true if given role name exist.</returns>
        public bool RoleExists(string roleName)
        {
            return DB.Roles.Any(r => r.RoleName == roleName);
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>Returns all roles.</returns>
        public List<Role> GetAllRoles()
        {
            return DB.Roles.ToList();
        }

        /// <summary>
        /// Updates a user's role.
        /// </summary>
        /// <param name="username">The username to edit the role.</param>
        /// <param name="roleName">The user's new role.</param>
        public void UpdateRoleToUser(string username, string roleName)
        {
            Role role = DB.Roles.First(r => r.RoleName == roleName);
            User user = DB.Users.First(u => u.Username == username);
            user.Roles.Clear();
            user.Roles.Add(role);

            DB.Entry(user).State = EntityState.Modified;
            DB.SaveChanges();
        }

    }
}