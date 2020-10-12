/*==============================================================
    Car.js ==> Represents a car object (Logic).
==============================================================*/

// C'tor Function:
function Car(licensePlate, manufacturer, model, year, gear, dailyPrice, dayDelayPrice, carImage) {

    // Properties:
    this.licensePlate = licensePlate;
    this.manufacturer = manufacturer;
    this.model = model;
    this.year = year;
    this.gear = gear;
    this.dailyPrice = dailyPrice;
    this.dayDelayPrice = dayDelayPrice;
    this.carImage = carImage;
}
Car.prototype.title = function ()
{ return this.manufacturer + " " + this.model };
Car.prototype.transmission = function ()
{ return this.gear == 0 ? "Automatic" : "Manual" };
