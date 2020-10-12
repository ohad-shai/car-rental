/*===================================================================================
  DateGreaterThanToday.js ==> Represents a Client Side Validation:
  ----------------------------------------------------------------
  Validates that the property's date value is greater than today's date.
===================================================================================*/

$.validator.unobtrusive.adapters.add("dategreaterthantoday", ["equaldates"], function (options) {

    // Adapts the HTML attribute values into jQuery:
    options.rules["dategreaterthantoday"] = {
        other: options.params.other,
        equaldates: options.params.equaldates
    };
    options.messages["dategreaterthantoday"] = options.message; // The error message
});


$.validator.addMethod("dategreaterthantoday", function (value, element, params) {

    var propDate = new Date(value); // Gets the date of the property.
    var today = new Date(GetTodaysDate()); // Gets today's date.
    var allowEqualDates = params.equaldates; // Gets indicator if to allow equal dates.

    if ((propDate < today) || (propDate == today && allowEqualDates == false))
        return false; // Invalid validation.

    return true; // Valid.
});


