/*===================================================================================
  DateGreaterThanProperty.js ==> Represents a Client Side Validation:
  -------------------------------------------------------------------
  Validates that the property date value is greater than
  a provided property name of a date value.
===================================================================================*/

$.validator.unobtrusive.adapters.add("dategreaterthanprop", ["lesserpropname", "equaldates"], function (options) {

    // Adapts the HTML attribute values into jQuery:
    options.rules["dategreaterthanprop"] = {
        other: options.params.other,
        lesserpropname: options.params.lesserpropname,
        equaldates: options.params.equaldates
    };
    options.messages["dategreaterthanprop"] = options.message; // The error message
});


$.validator.addMethod("dategreaterthanprop", function (value, element, params) {

    var propDate = new Date(value); // Gets the date of the property.
    var lesserDateVal = $("#" + params.lesserpropname).val(); // Gets the date string, from the property name.
    var allowEqualDates = params.equaldates; // Gets indicator if to allow equal dates.

    // Checks if the lesser property has a value:
    if (lesserDateVal) {
        // Creates a date object from the lesser date string:
        var lesserDate = new Date(lesserDateVal);

        if ((propDate < lesserDate) || (propDate == lesserDate && allowEqualDates == false))
            return false; // Invalid validation.
    }

    return true; // Valid.
});

