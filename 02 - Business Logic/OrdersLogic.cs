using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents the logic for the CRUD of the orders (rentals).
    /// </summary>
    public class OrdersLogic : BaseLogic
    {

        /// <summary>
        /// Gets all the rentals.
        /// </summary>
        /// <returns>List of all the rentals.</returns>
        public List<Rental> GetAllRentals()
        {
            return DB.Rentals.Include(c => c.FleetCar).ToList();
        }

        /// <summary>
        /// Gets all the rentals that their car can be returned.
        /// </summary>
        /// <returns>List of all the rentals that their car can be returned.</returns>
        public List<Rental> GetRentalsToCarReturn()
        {
            return DB.Rentals.Include(c => c.FleetCar)
                             .Where(r => r.ActualReturnDate == null)
                             .OrderBy(r => r.ReturnDate)
                             .ToList();
        }

        /// <summary>
        /// Gets all the rentals for a user, history of user's orders.
        /// </summary>
        /// <param name="username">The username to get his rentals.</param>
        /// <returns>Returns a list of all the user's rentals.</returns>
        public List<Rental> GetRentalsForUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException();

            return DB.Rentals.Where(r => r.User.Username.ToLower() == username.ToLower())
                             .OrderByDescending(r => r.RentalID)
                             .ToList();
        }

        /// <summary>
        /// Gets a rental by ID.
        /// </summary>
        /// <param name="id">The ID of the rental.</param>
        /// <returns>The rental details.</returns>
        public Rental GetRentalByID(int id)
        {
            return DB.Rentals.Find(id);
        }

        /// <summary>
        /// Inserts a new rental.
        /// </summary>
        /// <param name="licenseNumber">The identifier of the car.</param>
        /// <param name="startDate">The start date of the rental.</param>
        /// <param name="returnDate">The return date of the rental.</param>
        /// <param name="username">The username of the user that orders the car.</param>
        /// <returns>The order number of the rental (ID).</returns>
        public int InsertRental(string licenseNumber, DateTime startDate, DateTime returnDate, string username)
        {
            if (string.IsNullOrWhiteSpace(licenseNumber) || string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException();

            // Creates the rental object:
            Rental rental = new Rental();
            rental.LicensePlate = licenseNumber;
            rental.PickUpDate = startDate;
            rental.ReturnDate = returnDate;
            rental.UserID = DB.Users.Where(u => u.Username.ToLower() == username.ToLower()).FirstOrDefault().UserID;

            DB.Rentals.Add(rental);
            DB.SaveChanges();

            // Gets the rental values after inserted to the DB, in order to return the order number (ID):
            DB.Entry(rental).GetDatabaseValues();
            return rental.RentalID;
        }

        /// <summary>
        /// Updates the actual return date, when a car is returned.
        /// </summary>
        /// <param name="rentalID">The identifier of the rental. (Receipt Number).</param>
        public void UpdateCarReturn(int rentalID)
        {
            Rental rental = this.GetRentalByID(rentalID); // Gets the rental object from the rentalID.
            rental.ActualReturnDate = DateTime.Today; // Sets the actual return date to today's date.

            DB.Entry(rental).State = EntityState.Modified;
            DB.SaveChanges();
        }

    }
}
