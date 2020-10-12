/*===================================================================================
  BirthDateValidation.js ==> Represents a Client Side Validation:
  ---------------------------------------------------------------
  Validates if the date of birth is valid, by finding the age from the date, 
  and checking if the age is valid in a specific range of age!
===================================================================================*/

$.validator.unobtrusive.adapters.add("birthdate", ["minage", "maxage"], function (options) {

    // Adapts the HTML attribute values into jQuery:
    options.rules["birthdate"] = {
        other: options.params.other,
        minage: options.params.minage,
        maxage: options.params.maxage
    };
    options.messages["birthdate"] = options.message; // The error message
});


$.validator.addMethod("birthdate", function (value, element, params) {

    var birthDate = new Date(value); // Gets the BirthDate of the user
    var today = new Date(); // Gets today's date
    var age = today.getFullYear() - birthDate.getFullYear(); // Calculates age

    // Checks if age is lower than minimum, or higher than maximum:
    if (age < params.minage || age > params.maxage)
        return false; // Invalid BirthDate

    return true; // Valid BirthDate
});


