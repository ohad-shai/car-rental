﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OhadsCarRental
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class OhadsCarRentalEntities : DbContext
    {
        public OhadsCarRentalEntities()
            : base("name=OhadsCarRentalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<FleetCar> FleetCars { get; set; }
        public DbSet<ManufacturerModel> ManufacturerModels { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    
        public virtual int CarCheckAvailability(string licensePlate, Nullable<System.DateTime> startDate, Nullable<System.DateTime> returnDate, ObjectParameter isAvailable)
        {
            var licensePlateParameter = licensePlate != null ?
                new ObjectParameter("LicensePlate", licensePlate) :
                new ObjectParameter("LicensePlate", typeof(string));
    
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("StartDate", startDate) :
                new ObjectParameter("StartDate", typeof(System.DateTime));
    
            var returnDateParameter = returnDate.HasValue ?
                new ObjectParameter("ReturnDate", returnDate) :
                new ObjectParameter("ReturnDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CarCheckAvailability", licensePlateParameter, startDateParameter, returnDateParameter, isAvailable);
        }
    
        public virtual ObjectResult<CarsSearch_Result> CarsSearch(Nullable<int> manufacturerID, Nullable<int> manufacturerModelID, Nullable<int> productionYear, Nullable<bool> manualGear, Nullable<System.DateTime> startDate, Nullable<System.DateTime> returnDate, string freeText)
        {
            var manufacturerIDParameter = manufacturerID.HasValue ?
                new ObjectParameter("ManufacturerID", manufacturerID) :
                new ObjectParameter("ManufacturerID", typeof(int));
    
            var manufacturerModelIDParameter = manufacturerModelID.HasValue ?
                new ObjectParameter("ManufacturerModelID", manufacturerModelID) :
                new ObjectParameter("ManufacturerModelID", typeof(int));
    
            var productionYearParameter = productionYear.HasValue ?
                new ObjectParameter("ProductionYear", productionYear) :
                new ObjectParameter("ProductionYear", typeof(int));
    
            var manualGearParameter = manualGear.HasValue ?
                new ObjectParameter("ManualGear", manualGear) :
                new ObjectParameter("ManualGear", typeof(bool));
    
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("StartDate", startDate) :
                new ObjectParameter("StartDate", typeof(System.DateTime));
    
            var returnDateParameter = returnDate.HasValue ?
                new ObjectParameter("ReturnDate", returnDate) :
                new ObjectParameter("ReturnDate", typeof(System.DateTime));
    
            var freeTextParameter = freeText != null ?
                new ObjectParameter("FreeText", freeText) :
                new ObjectParameter("FreeText", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CarsSearch_Result>("CarsSearch", manufacturerIDParameter, manufacturerModelIDParameter, productionYearParameter, manualGearParameter, startDateParameter, returnDateParameter, freeTextParameter);
        }
    
        public virtual ObjectResult<UpdateAvailabilityOfCars_Result> UpdateAvailabilityOfCars()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UpdateAvailabilityOfCars_Result>("UpdateAvailabilityOfCars");
        }
    }
}
