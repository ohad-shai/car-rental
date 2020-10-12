/*===================================================================================
  IdentityNumberValidation.js ==> Represents a Client Side Validation:
  --------------------------------------------------------------------
  Validates if the identity number is valid, by an algorithm to check IDs.
===================================================================================*/

$.validator.unobtrusive.adapters.add("idnumber", ["other"], function (options) {
    options.rules["idnumber"] = "#" + options.params.other;
    options.messages["idnumber"] = options.message; // The error message
});

$.validator.addMethod("idnumber", function (value, element, params) {

    // Checks if the identity number is valid:
    if (!ValidateID(value))
        return false; // Invalid IdentityNumber!

    return true; // Valid IdentityNumber!
});

// Validates the ID number!
function ValidateID(str) {
    //INPUT VALIDATION

    // Just in case -> convert to string
    var IDnum = String(str);

    // Validate correct input:
    if ((IDnum.length > 9) || (IDnum.length < 5))
        return false; // Invalid id
    if (isNaN(IDnum))
        return false; // Invalid id

    // If the number is too short, add leading 0000:
    if (IDnum.length < 9) {
        while (IDnum.length < 9) {
            IDnum = '0' + IDnum;
        }
    }

    // CHECKS THE ID NUMBER:
    var mone = 0, incNum;
    for (var i = 0; i < 9; i++) {
        incNum = Number(IDnum.charAt(i));
        incNum *= (i % 2) + 1;
        if (incNum > 9)
            incNum -= 9;
        mone += incNum;
    }
    if (mone % 10 == 0)
        return true; // Valid id
    else
        return false; // Invalid id
}

