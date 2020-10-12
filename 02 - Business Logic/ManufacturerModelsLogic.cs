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
    /// Represents the logic for the CRUD of the manufacturer models.
    /// </summary>
    public class ManufacturerModelsLogic : BaseLogic
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
        /// Gets all the manufacturer models.
        /// </summary>
        /// <returns>List of all the manufacturer models.</returns>
        public List<ManufacturerModel> GetAllManufacturerModels()
        {
            return DB.ManufacturerModels.Include(c => c.Manufacturer)
                                        .OrderBy(m => m.Manufacturer.ManufacturerName)
                                        .ThenBy(m => m.ManufacturerModelName)
                                        .ToList();
        }

        /// <summary>
        /// Gets the manufacturer models for a manufacturer.
        /// </summary>
        /// <param name="manufacturerID">The ID of the manufacturer to find the models.</param>
        /// <returns>List of all manufacturer models for the manufacturer.</returns>
        public List<ManufacturerModel> GetModelsForManufacturer(int manufacturerID)
        {
            return DB.ManufacturerModels.Where(m => m.Manufacturer.ManufacturerID == manufacturerID)
                                        .OrderBy(m => m.Manufacturer.ManufacturerName)
                                        .ThenBy(m => m.ManufacturerModelName)
                                        .ToList();
        }

        /// <summary>
        /// Gets a manufacturer model by ID.
        /// </summary>
        /// <param name="id">The ID of the manufacturer model.</param>
        /// <returns>The manufacturer model details.</returns>
        public ManufacturerModel GetManufacturerModelByID(int id)
        {
            return DB.ManufacturerModels.Find(id);
        }

        /// <summary>
        /// Inserts a new manufacturer model.
        /// </summary>
        /// <param name="manufacturerModel">The manufacturer model to insert.</param>
        public void InsertManufacturerModel(ManufacturerModel manufacturerModel)
        {
            if (manufacturerModel == null)
                throw new ArgumentNullException();

            DB.ManufacturerModels.Add(manufacturerModel);
            DB.SaveChanges();
        }

        /// <summary>
        /// Updates a manufacturer model.
        /// </summary>
        /// <param name="manufacturerModel">The manufacturer model to update.</param>
        public void UpdateManufacturerModel(ManufacturerModel manufacturerModel)
        {
            if (manufacturerModel == null)
                throw new ArgumentNullException();

            DB.Entry(manufacturerModel).State = EntityState.Modified;
            DB.SaveChanges();
        }

        /// <summary>
        /// Deletes a manufacturer model.
        /// </summary>
        /// <param name="manufacturerModel">The manufacturer model to delete.</param>
        /// <param name="isCollective">Indicator if to perform a collective delete, if to delete all it's related data.</param>
        public void DeleteManufacturerModel(ManufacturerModel manufacturerModel, bool isCollective = false)
        {
            if (manufacturerModel == null)
                throw new ArgumentNullException();

            // Checks if to perform a collective delete:
            if (isCollective)
            {
                // Begins to delete any related data, to this manufacturer model:

                // Deletes all the rentals, related to this manufacturer model:
                List<Rental> orders = DB.Rentals.Where(r => r.FleetCar.CarModel.ManufacturerModel.ManufacturerModelID == manufacturerModel.ManufacturerModelID).ToList();
                foreach (Rental r in orders)
                    DB.Rentals.Remove(r);

                // Deletes all the fleet cars, related to this manufacturer model:
                List<FleetCar> fleet = DB.FleetCars.Where(f => f.CarModel.ManufacturerModel.ManufacturerModelID == manufacturerModel.ManufacturerModelID).ToList();
                foreach (FleetCar f in fleet)
                    DB.FleetCars.Remove(f);

                // Deletes all the car models, related to this manufacturer model:
                List<CarModel> carModels = DB.CarModels.Where(c => c.ManufacturerModel.ManufacturerModelID == manufacturerModel.ManufacturerModelID).ToList();
                foreach (CarModel c in carModels)
                    DB.CarModels.Remove(c);
            }

            // Deletes this manufacturer model:
            DB.ManufacturerModels.Remove(manufacturerModel);
            DB.SaveChanges();
        }

        /// <summary>
        /// Checks if a manufacturer model exists.
        /// </summary>
        /// <param name="manufacturerModel">The manufacturer model to check.</param>
        /// <returns>True or False, if there's already a manufacturer model.</returns>
        public bool IsManufacturerModelExists(ManufacturerModel manufacturerModel)
        {
            if (manufacturerModel == null)
                throw new ArgumentNullException();

            return DB.ManufacturerModels.Any(m => m.ManufacturerModelName == manufacturerModel.ManufacturerModelName &&
                                             m.Manufacturer.ManufacturerID == manufacturerModel.ManufacturerID);
        }

    }
}
