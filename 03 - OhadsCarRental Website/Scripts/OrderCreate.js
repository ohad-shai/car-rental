/*••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••
  OrderCreate.js ==> Represents a page scripts for "Order Create" page:
  ---------------------------------------------------------------------
  This is the page script for the car order create (rental).
  Provides specific actions only for the "order create" page.
••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••*/


//==========================================================================================
// {Event: "On load"} : Operates actions that need to be called when the page loads:
//==========================================================================================
$(function () {

    //------------------------------------------------------------------------------------------------
    // [Server Operation] : Submits the main search bar, by AJAX request, and displays the returned search results.
    //------------------------------------------------------------------------------------------------
    $(window).load(function () {

        // Updates the start date, from the input, to the order "paper":
        ChangeDateIntoPaper('fieldset#orderForm #StartDate', '#sumStartDate');
        // Updates the return date, from the input, to the order "paper":
        ChangeDateIntoPaper('fieldset#orderForm #ReturnDate', '#sumReturnDate');

        // Updates the total price, and the total days, in the order "paper":
        UpdateTotalsToPaper();
    });


});


//------------------------------------------------------------------------------------------------
// [Helper] : Changes the date value in the order paper, according to the input.
// (Params) : inputSelector = The selector of the date input (Sender),
//            paperSelector = The selector of the order paper (Receiver).
//------------------------------------------------------------------------------------------------
function ChangeDateIntoPaper(inputSelector, paperSelector) {

    // Gets value of the date input to a correct string format:
    var paperStartDate = DisplayDate($(inputSelector).val());
    // Checks if it's a date value:
    if (paperStartDate != undefined) {
        // Changes the value of the date field in the "Paper":
        $(paperSelector).text(paperStartDate);
    }
    else {
        // If the value is not a date, then removes any text:
        $(paperSelector).text("");
    }
}


//------------------------------------------------------------------------------------------------
// [UI + Operation] : Updates the total price, and the total days, in the order "paper":
//------------------------------------------------------------------------------------------------
function UpdateTotalsToPaper() {

    var startDate = new Date($('fieldset#orderForm #StartDate').val()); // Holds the start date of the rental.
    var returnDate = new Date($('fieldset#orderForm #ReturnDate').val()); // Holds the return date of the rental.
    var dailyPrice = parseFloat($('#dailyPrice').text().replace("$", "")); // Holds the daily price for the rental.
    var totalPrice = 0; // Holds the total price for the rental.
    var totalDays = 0; // Holds the total days of rental.

    // Runs a loop for every day in the rental dates range: 
    for (var start = new Date(startDate) ; start <= Date.parse(returnDate) ; start.setDate(start.getDate() + 1)) {
        totalPrice += +dailyPrice;
        totalDays += 1;
    }

    // Displays the results to the "order paper":
    $("#totalPrice").text("$" + totalPrice.formatMoney(2));
    $("#totalDays").text(totalDays);
}


//==========================================================================================
//  {Change Events}
//==========================================================================================
$(function () {


    //------------------------------------------------------------------------------------------------
    // [Event: "Change"] of : Pick Up Date input, in the order form:
    //------------------------------------------------------------------------------------------------
    $('fieldset#orderForm #StartDate').change(function () {

        // Sends the selectors of the dates, to the validation helper to handle it:
        StartDateValidationHelper('fieldset#orderForm #StartDate', 'fieldset#orderForm #ReturnDate');

        // Updates the order "paper":
        ChangeDateIntoPaper('fieldset#orderForm #StartDate', '#sumStartDate');

        // Updates the total price, and the total days, in the order "paper":
        UpdateTotalsToPaper();
    });


    //------------------------------------------------------------------------------------------------
    // [Event: "Change"] of : Return Date input, in the order form:
    //------------------------------------------------------------------------------------------------
    $('fieldset#orderForm #ReturnDate').change(function () {

        // Sends the selectors of the dates, to the validation helper to handle it:
        ReturnDateValidationHelper('fieldset#orderForm #ReturnDate', 'fieldset#orderForm #StartDate');

        // Updates the order "paper":
        ChangeDateIntoPaper('fieldset#orderForm #ReturnDate', '#sumReturnDate');

        // Updates the total price, and the total days, in the order "paper":
        UpdateTotalsToPaper();
    });


});


