/*••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••
  Home.js ==> Represents a page scripts for HomePage of the site:
  ---------------------------------------------------------------
  Provides specific actions only for the HomePage.
••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••*/


//==========================================================================================
//  {Change Events}
//==========================================================================================
$(function () {


    //------------------------------------------------------------------------------------------------
    // [Event: "Change"] of : Pick Up Date input, in the order form:
    //------------------------------------------------------------------------------------------------
    $('#sStartDate').change(function () {

        // Sends the selectors of the dates, to the validation helper to handle it:
        StartDateValidationHelper('#sStartDate', '#sReturnDate');
    });


    //------------------------------------------------------------------------------------------------
    // [Event: "Change"] of : Return Date input, in the order form:
    //------------------------------------------------------------------------------------------------
    $('#sReturnDate').change(function () {

        // Sends the selectors of the dates, to the validation helper to handle it:
        ReturnDateValidationHelper('#sReturnDate', '#sStartDate');
    });


});







