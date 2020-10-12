using System.Linq;
using System.Collections.Generic;
using System.Web.Security;
using System;
using System.Data;
using System.Data.Entity;

#pragma warning disable 618

namespace OhadsCarRental
{
    /// <summary>
    /// Users logic - contains logic for the users table.
    /// </summary>
    public class UsersLogic : BaseLogic
    {

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="username">The username for the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <param name="identityNumber">The user's identity number.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="birthDate">The user's date of birth.</param>
        public void Register(string username, string password, string firstName, string lastName, int identityNumber, string email, DateTime? birthDate)
        {
            // Need to add reference to: System.Web (Framework 4.0)
            string encryptPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");

            // Creates the new user object:
            User user = new User();
            user.Username = username;
            user.Password = encryptPassword;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.IdentityNumber = identityNumber;
            user.Email = email;
            user.BirthDate = birthDate.HasValue ? birthDate.Value.Date : birthDate;

            // Saves to DB:
            DB.Roles.FirstOrDefault(r => r.RoleID == 3).Users.Add(user); // Set this user the "User" Role (RoleID = 3).
            DB.Users.Add(user);
            DB.SaveChanges();
        }

        /// <summary>
        /// Check if username already taken.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>Returns true if username already taken.</returns>
        public bool IsUsernameTaken(string username)
        {
            return DB.Users.Any(u => u.Username == username);
        }

        /// <summary>
        /// Check if user exists.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>Returns true if user exist.</returns>
        public bool IsUserExist(string username, string password)
        {
            string encryptPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
            return DB.Users.Any(u => u.Username == username && u.Password == encryptPassword);
        }

        /// <summary>
        /// Gets all the usernames.
        /// </summary>
        /// <returns>Returns all usernames.</returns>
        public string[] GetAllUsernames()
        {
            return DB.Users.Select(u => u.Username).ToArray();
        }

        /// <summary>
        /// Gets all the users.
        /// </summary>
        /// <returns>Returns a list of all users.</returns>
        public List<User> GetAllUsers()
        {
            List<User> users = DB.Users.Include(r => r.Roles).ToList();

            foreach (User user in users)
                user.Password = ""; // Clears the hashed password.

            return users;
        }

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>Returns the user object.</returns>
        public User GetUserByID(int id)
        {
            User user = DB.Users.Find(id);
            if (user != null)
                user.Password = ""; // Clears the hashed password.

            return user;
        }

        /// <summary>
        /// Gets a user by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>Returns the user object.</returns>
        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException();

            User user = DB.Users.Where(u => u.Username.ToLower() == username.ToLower()).FirstOrDefault();
            if (user != null)
                user.Password = ""; // Clears the hashed password.

            return user;
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        public void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException();

            // Begins to delete any related data, to this user:

            // Deletes all the rentals, related to this user:
            List<Rental> orders = DB.Rentals.Where(r => r.UserID == user.UserID).ToList();
            foreach (Rental r in orders)
                DB.Rentals.Remove(r);

            // Deletes the role of the user in the RoleUsers table:
            DB.Database.ExecuteSqlCommand(string.Format("DELETE FROM [dbo].[RoleUsers] WHERE [UserID] = {0}", user.UserID));

            // Deletes this user:
            DB.Users.Remove(user);
            DB.SaveChanges();
        }

    }
}