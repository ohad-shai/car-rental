using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents the logic for the CRUD of the manufacturers!
    /// </summary>
    public class ManufacturersLogic : BaseLogic
    {

        /// <summary>
        /// Gets all the manufacturers!
        /// </summary>
        /// <returns>List of all the manufacturers.</returns>
        public List<Manufacturer> GetAllManufacturers()
        {
            return DB.Manufacturers.OrderBy(m => m.ManufacturerName).ToList();
        }

        /// <summary>
        /// Gets a manufacturer by ID!
        /// </summary>
        /// <param name="id">The ID of the manufacturer.</param>
        /// <returns>The manufacturer details.</returns>
        public Manufacturer GetManufacturerByID(int id)
        {
            return DB.Manufacturers.Find(id);
        }

        /// <summary>
        /// Inserts a new manufacturer!
        /// </summary>
        /// <param name="manufacturer">The manufacturer to insert.</param>
        public void InsertManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException();

            DB.Manufacturers.Add(manufacturer);
            DB.SaveChanges();
        }

        /// <summary>
        /// Updates a manufacturer!
        /// </summary>
        /// <param name="manufacturer">The manufacturer to update.</param>
        public void UpdateManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException();

            DB.Entry(manufacturer).State = EntityState.Modified;
            DB.SaveChanges();
        }

        /// <summary>
        /// Deletes a manufacturer!
        /// </summary>
        /// <param name="manufacturer">The manufacturer to delete.</param>
        /// <param name="isCollective">Indicator if to perform a collective delete, if to delete all it's related data.</param>
        public void DeleteManufacturer(Manufacturer manufacturer, bool isCollective = false)
        {
            if (manufacturer == null)
                throw new ArgumentNullException();

            // Checks if to perform a collective delete:
            if (isCollective)
            {
                // Begins to delete any related data, to this manufacturer:

                // Deletes all the rentals, related to this manufacturer:
                List<Rental> orders = DB.Rentals.Where(r => r.FleetCar.CarModel.ManufacturerModel.Manufacturer.ManufacturerID == manufacturer.ManufacturerID).ToList();
                foreach (Rental r in orders)
                    DB.Rentals.Remove(r);

                // Deletes all the fleet cars, related to this manufacturer:
                List<FleetCar> fleet = DB.FleetCars.Where(f => f.CarModel.ManufacturerModel.Manufacturer.ManufacturerID == manufacturer.ManufacturerID).ToList();
                foreach (FleetCar f in fleet)
                    DB.FleetCars.Remove(f);

                // Deletes all the car models, related to this manufacturer:
                List<CarModel> carModels = DB.CarModels.Where(c => c.ManufacturerModel.Manufacturer.ManufacturerID == manufacturer.ManufacturerID).ToList();
                foreach (CarModel c in carModels)
                    DB.CarModels.Remove(c);

                // Deletes all the manufacturer models, related to this manufacturer:
                List<ManufacturerModel> mfrModels = DB.ManufacturerModels.Where(m => m.Manufacturer.ManufacturerID == manufacturer.ManufacturerID).ToList();
                foreach (ManufacturerModel m in mfrModels)
                    DB.ManufacturerModels.Remove(m);
            }
            
            // Deletes this manufacturer:
            DB.Manufacturers.Remove(manufacturer);
            DB.SaveChanges();
        }

        /// <summary>
        /// Checks if a manufacturer exists.
        /// </summary>
        /// <param name="manufacturerName">The manufacturer name to check.</param>
        /// <returns>True of False, if there's already a manufacturer with the name.</returns>
        public bool IsManufacturerExists(string manufacturerName)
        {
            if (string.IsNullOrWhiteSpace(manufacturerName))
                throw new ArgumentException();

            return DB.Manufacturers.Any(m => m.ManufacturerName == manufacturerName);
        }

    }
}
