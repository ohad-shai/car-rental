using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents the logic for the CRUD of the cars in the fleet.
    /// </summary>
    public class FleetCarsLogic : BaseLogic
    {

        /// <summary>
        /// Gets all the cars in the fleet.
        /// </summary>
        /// <returns>List of all the cars in the fleet.</returns>
        public List<FleetCar> GetAllFleetCars()
        {
            return DB.FleetCars.Include(f => f.CarModel)
                               .OrderBy(f => f.CarModel.ManufacturerModel.Manufacturer.ManufacturerName)
                               .ThenBy(f => f.CarModel.ManufacturerModel.ManufacturerModelName)
                               .ThenBy(f => f.CarModel.ProductionYear)
                               .ToList();
        }

        /// <summary>
        /// Gets a car in the fleet by the license plate.
        /// </summary>
        /// <param name="id">The license plate of the car in the fleet.</param>
        /// <returns>The car in the fleet details.</returns>
        public FleetCar GetFleetCarByLicensePlate(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                return null;

            return DB.FleetCars.Where(c => c.LicensePlate == licensePlate).FirstOrDefault();
        }

        /// <summary>
        /// Inserts a new car to the fleet.
        /// </summary>
        /// <param name="fleetCar">The car to insert.</param>
        public void InsertFleetCar(FleetCar fleetCar)
        {
            if (fleetCar == null)
                throw new ArgumentNullException();

            DB.FleetCars.Add(fleetCar);
            DB.SaveChanges();
        }

        /// <summary>
        /// Updates a car in the fleet.
        /// </summary>
        /// <param name="fleetCar">The car in the fleet to update.</param>
        public void UpdateFleetCar(FleetCar fleetCar)
        {
            if (fleetCar == null)
                throw new ArgumentNullException();

            DB.Entry(fleetCar).State = EntityState.Modified;
            DB.SaveChanges();
        }

        /// <summary>
        /// Deletes a car in the fleet.
        /// </summary>
        /// <param name="fleetCar">The car in the fleet to delete.</param>
        /// <param name="isCollective">Indicator if to perform a collective delete, if to delete all it's related data.</param>
        public void DeleteFleetCar(FleetCar fleetCar, bool isCollective = false)
        {
            if (fleetCar == null)
                throw new ArgumentNullException();

            // Checks if to perform a collective delete:
            if (isCollective)
            {
                // Begins to delete any related data, to this fleet car:

                // Deletes all the rentals, related to this fleet car:
                List<Rental> orders = DB.Rentals.Where(r => r.LicensePlate == fleetCar.LicensePlate).ToList();
                foreach (Rental r in orders)
                    DB.Rentals.Remove(r);
            }

            // Deletes this fleet car:
            DB.FleetCars.Remove(fleetCar);
            DB.SaveChanges();
        }

        /// <summary>
        /// Checks if a car in the fleet exists.
        /// </summary>
        /// <param name="licensePlate">The license plate of the car in the fleet to check.</param>
        /// <returns>True or False, if there's already a car in the fleet.</returns>
        public bool IsFleetCarExists(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                return false;

            return DB.FleetCars.Any(m => m.LicensePlate == licensePlate);
        }

        /// <summary>
        /// Checks an availability of a car, according to the rental dates provided.
        /// </summary>
        /// <param name="licensePlate">The license plate of the car (identifier).</param>
        /// <param name="startDate">The date to start the rental.</param>
        /// <param name="returnDate">The date to end the rental.</param>
        /// <returns>Indicator if the car is available to rent, true or false.</returns>
        public bool CheckCarAvailability(string licensePlate, DateTime startDate, DateTime returnDate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                throw new ArgumentException();

            // Declares the output parameter from the query:
            ObjectParameter isAvailable = new ObjectParameter("isAvailable", false);

            // Calls the stored procedure that checks availability:
            DB.CarCheckAvailability(licensePlate, startDate, returnDate, isAvailable);

            return (bool)isAvailable.Value;
        }

        /// <summary>
        /// Searches for cars, according to the provided parameters, that satisfies a condition.
        /// </summary>
        /// <param name="manufacturerID">The ID of the manufacturer of the car.</param>
        /// <param name="manufacturerModelID">The ID of the manufacturer model of the car.</param>
        /// <param name="productionYear">The production year of the car.</param>
        /// <param name="manualGear">Indicator if the car is manual or automatic.</param>
        /// <param name="startDate">The start date of the rental.</param>
        /// <param name="returnDate">The return date of the rental.</param>
        /// <param name="freeText">A free text to search in the manufacturer name, and the manufacturer model name.</param>
        /// <returns>Returns a list of cars, that satifies the search condition, as a result.</returns>
        public List<CarsSearch_Result> SearchCars(int? manufacturerID, int? manufacturerModelID, int? productionYear,
                                                  bool? manualGear, DateTime? startDate, DateTime? returnDate, string freeText)
        {
            // Calls the stored procedure of the search:
            ObjectResult<CarsSearch_Result> spResults = DB.CarsSearch(manufacturerID, manufacturerModelID,  productionYear,
                                                                    manualGear, startDate, returnDate, freeText);

            // Declares the returned list of the search:
            List<CarsSearch_Result> results = new List<CarsSearch_Result>();

            // Appends the returned list, from the list returned by the stored procedure:
            foreach (var result in spResults)
                results.Add(result);

            return results;
        }

    }
}
