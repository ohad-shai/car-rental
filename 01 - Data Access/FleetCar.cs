//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class FleetCar
    {
        public FleetCar()
        {
            this.Rentals = new HashSet<Rental>();
        }
    
        public string LicensePlate { get; set; }
        public int CarModelID { get; set; }
        public int CurrentMileage { get; set; }
        public string CarImage { get; set; }
        public bool IsProper { get; set; }
    
        public virtual CarModel CarModel { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; }
    }
}
