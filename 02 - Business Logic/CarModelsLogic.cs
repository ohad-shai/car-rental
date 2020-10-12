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
    /// Represents the logic for the CRUD of the car models.
    /// </summary>
    public class CarModelsLogic : BaseLogic
    {

        /// <summary>
        /// Gets all the car models.
        /// </summary>
        /// <returns>List of all the car models.</returns>
        public List<CarModel> GetAllCarModels()
        {
            return DB.CarModels.Include(c => c.ManufacturerModel)
                               .OrderBy(c => c.ManufacturerModel.Manufacturer.ManufacturerName)
                               .ThenBy(c => c.ManufacturerModel.ManufacturerModelName)
                               .ThenBy(c => c.ProductionYear)
                               .ToList();
        }

        /// <summary>
        /// Gets a car model by ID.
        /// </summary>
        /// <param name="id">The ID of the car model.</param>
        /// <returns>The car model details.</returns>
        public CarModel GetCarModelByID(int id)
        {
            return DB.CarModels.Find(id);
        }

        /// <summary>
        /// Inserts a new car model.
        /// </summary>
        /// <param name="carModel">The car model to insert.</param>
        public void InsertCarModel(CarModel carModel)
        {
            if (carModel == null)
                throw new ArgumentNullException();

            DB.CarModels.Add(carModel);
            DB.SaveChanges();
        }

        /// <summary>
        /// Updates a car model.
        /// </summary>
        /// <param name="carModel">The car model to update.</param>
        public void UpdateCarModel(CarModel carModel)
        {
            if (carModel == null)
                throw new ArgumentNullException();

            DB.Entry(carModel).State = EntityState.Modified;
            DB.SaveChanges();
        }

        /// <summary>
        /// Deletes a car model.
        /// </summary>
        /// <param name="carModel">The car model to delete.</param>
        /// <param name="isCollective">Indicator if to perform a collective delete, if to delete all it's related data.</param>
        public void DeleteCarModel(CarModel carModel, bool isCollective = false)
        {
            if (carModel == null)
                throw new ArgumentNullException();

            // Checks if to perform a collective delete:
            if (isCollective)
            {
                // Begins to delete any related data, to this car model:

                // Deletes all the rentals, related to this car model:
                List<Rental> orders = DB.Rentals.Where(r => r.FleetCar.CarModelID == carModel.CarModelID).ToList();
                foreach (Rental r in orders)
                    DB.Rentals.Remove(r);

                // Deletes all the fleet cars, related to this car model:
                List<FleetCar> fleet = DB.FleetCars.Where(f => f.CarModelID == carModel.CarModelID).ToList();
                foreach (FleetCar f in fleet)
                    DB.FleetCars.Remove(f);
            }
            
            // Deletes this car model:
            DB.CarModels.Remove(carModel);
            DB.SaveChanges();
        }

        /// <summary>
        /// Checks if a car model exists.
        /// </summary>
        /// <param name="carModel">The car model to check.</param>
        /// <returns>True or False, if there's already a car model.</returns>
        public bool IsCarModelExists(CarModel carModel)
        {
            if (carModel == null)
                throw new ArgumentNullException();

            return DB.CarModels.Any(m => m.ManufacturerModelID == carModel.ManufacturerModelID &&
                                    m.ProductionYear == carModel.ProductionYear &&
                                    m.ManualGear == carModel.ManualGear);
        }

        /// <summary>
        /// Gets all the car models for a manufacturer.
        /// </summary>
        /// <param name="manufacturerID">The ID of the manufacturer.</param>
        /// <returns>List of all the car models for the manufacturer. Ordered by manufacturer model name, then by production year.</returns>
        public List<CarModel> GetCarModelsForManufacturer(int manufacturerID)
        {
            return DB.CarModels.Where(c => c.ManufacturerModel.Manufacturer.ManufacturerID == manufacturerID)
                               .OrderBy(c => c.ManufacturerModel.Manufacturer.ManufacturerName)
                               .ThenBy(c => c.ManufacturerModel.ManufacturerModelName)
                               .ThenBy(c => c.ProductionYear)
                               .ToList();
        }

    }
}
