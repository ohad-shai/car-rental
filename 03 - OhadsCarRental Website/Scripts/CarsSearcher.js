/*••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••
  CarsSearcher.js ==> Represents a page scripts for "Search" page:
  ----------------------------------------------------------------
  This is the page script for the search of cars.
  Provides specific actions only for the search page.
••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••*/


//==========================================================================================
// Global Variables:
//==========================================================================================
// (Global) - Holds the list of cars returned from the search results.
var CarsList = new Array();


//==========================================================================================
// [Car Object] : Represents a car object (Logic)
//==========================================================================================
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


//==========================================================================================
// {Event: "On load"} : Operates actions that need to be called when the page loads:
//==========================================================================================
$(function () {

    //------------------------------------------------------------------------------------------------
    // [Server Operation] : Submits the main search bar, by AJAX request, and displays the returned search results.
    //------------------------------------------------------------------------------------------------
    $(window).load(function () {

        // Submits the main search bar:
        $("form#searchForm").submit();

        // Checks if there's a search history for the user:
        if (IsSearchHistoryExists()) {
            // Displays the search history:
            DisplaySearchHistory();
        }

    });


});


//==========================================================================================
// [UI] : 'Main Search Bar' Buttons (toggle buttons):
//==========================================================================================
$(function () {


    //------------------------------------------------------------------------------------------------
    // "More search" toggle button:
    //------------------------------------------------------------------------------------------------
    $("#moreSearch").toggle(function () {
        $(this).val("Less"); // Changes the text to "less"
        // Shows and animates the more search content:
        $("div.search-more-content").animate({ left: "+=50", height: "toggle" }, 400).toggleClass("active");
    }, function () {
        $(this).val("More"); // Changes the text to "more"
        // Hides and animates the more search content:
        $("div.search-more-content").animate({ left: "+=50", height: "toggle" }, 400).toggleClass("active");
    });


    //------------------------------------------------------------------------------------------------
    // "Search History" toggle button:
    //------------------------------------------------------------------------------------------------
    $("#searchHistory").click(function () {
        // Toggles the search history block, open of close:
        $("div.basic-form.history").animate({ height: "toggle", marginTop: "toggle", borderTopWidth: "toggle", borderBottomWidth: "toggle", opacity: "toggle" }, 600);
    });


});


//==========================================================================================
// [Control Bar] : "Search" Filter Form Submission (Server operation by AJAX)
//==========================================================================================
$(function () {

    //------------------------------------------------------------------------------------------------
    // [UI] Search Button. (appears in the main search bar).
    //------------------------------------------------------------------------------------------------
    $('#searchCars').click(function () {
        // Saves the current search to the local storage:
        SaveCurrentSearchToLocal();
    });


    //------------------------------------------------------------------------------------------------
    // Search filter form submission: (Sends the search filters to the server, in order to get the results,
    //                                 and displays them):
    //------------------------------------------------------------------------------------------------
    $("#searchForm").submit(function (e) {
        e.preventDefault();

        // Checks if the search form is valid:
        if ($("#searchForm").valid()) {

            // Creates the search model (search filters):
            var dataObject = {
                ManufacturerID: $('#DDLs_SelectedManufacturer').val(),
                ManufacturerModelID: $('#DDLs_SelectedManufacturerModel').val(),
                ProductionYear: $('#DDLs_SelectedYear').val(),
                ManualGear: $('#DDLs_SelectedGear').val(),
                FreeText: $('div.basic-control-bar.search #FreeText').val(),
                StartDate: $('#StartDate').val(),
                ReturnDate: $('#ReturnDate').val()
            };

            // Sends an AJAX request to the server, with the search model:
            $.ajax({
                type: 'GET',
                url: '/FleetCars/AdvancedSearch',
                data: dataObject,
                contentType: "application/json;charset=utf-8",
                dataType: 'json',
                success: function (results) {
                    ProcessSearchResults(results);
                },
                error: function (error) {
                    popupMsg("<div>An error has occurred.</div><div>Code: " + error.status + ", " + error.statusText + "</div>");
                },
                fail: function () {
                    popupMsg("<div>Failed to send the request.</div>");
                },
                timeout: 6000
            });
        }
        else {
            popupMsg($('div#validation-hidden').html());
        }
    });


    //------------------------------------------------------------------------------------------------
    // Processing the results that returned from the AJAX search request. (Expected: list of cars,)
    // [Parameter Situations: "results"] ==> (Expected: list of cars object),
    //                                       (Error: "error" string),
    //                                       (Invalid search filter model: "invalid" string).
    //------------------------------------------------------------------------------------------------
    function ProcessSearchResults(results) {

        if (results === "INVALID") {
            // Displays a popup message for invalid search filter values:
            popupMsg("<div>Invalid search filter values.</div>" + $('div#validation-hidden').html());
        }
        else if (results === "ERROR") {
            // Displays a popup message to inform an error has occurred:
            popupMsg("<div>An error has occurred. please try again later.</div>");
        }
        else {
            // Checks if there's no results:
            if (results == null || results.length == 0) {
                // Checks if results block is already displayed:
                if ($('#searchSection div').hasClass('results')) {
                    // If results block already displayed, clears all the content, and appends the "No results" title:
                    $('#searchSection div.results').empty().append('<div class="search-no-results">No results.</div>');
                }
                else {
                    // If "results block" not already displayed, appends the results block, with the "No results" title:
                    $('#searchSection').append('<div class="basic-form big results"><div class="search-no-results">No results.</div></div>');
                }
            }
            else {
                // --- Here, there's some content in the results! ---

                // Creates a list of car objects, and assigns it to the global variable 'CarsList':
                CarsList = CreateCarsListFromResults(results);

                // Displays the list of car objects:
                DisplayCurrentCarsList();
            }
        }
    }


    //------------------------------------------------------------------------------------------------
    // Creates a javascript list of cars, that converted from the list of the results (JSON).
    // (Params) : carsResults = the list of cars result returned from the server.
    // * Returns : List of car objects.
    //------------------------------------------------------------------------------------------------
    function CreateCarsListFromResults(carsResults) {
        var carList = new Array(); // Creates the list.
        for (var i = 0; i < carsResults.length; i++) {

            var car = CreateCarFromResult(carsResults[i]); // Creates a car object from the result.
            carList.push(car); // Adds the car object to the list of cars.
        }
        return carList; // Returns the list of car objects.
    }


    //------------------------------------------------------------------------------------------------
    // Creates a car object, from a single car result in the results.
    // (Params) : carResult = a car result object returned from the server.
    // * Returns : A car object (javascript object).
    //------------------------------------------------------------------------------------------------
    function CreateCarFromResult(carResult) {
        var car = new Car();
        car.licensePlate = carResult["LicensePlate"];
        car.manufacturer = carResult["Manufacturer"];
        car.model = carResult["ManufacturerModel"];
        car.year = carResult["Year"];
        car.gear = carResult["Gear"];
        car.dailyPrice = carResult["DailyPrice"];
        car.dayDelayPrice = carResult["DayDelayPrice"];
        car.carImage = carResult["CarImage"];

        return car;
    }


    //------------------------------------------------------------------------------------------------
    // [UI] : Displays all the cars in the list returned from the search, to the results block.
    //------------------------------------------------------------------------------------------------
    function DisplayCurrentCarsList() {
        // Displays only if there's car objects to display:
        if (CarsList != null && CarsList.length > 0) {
            // Checks if results block is already displayed:
            if ($('#searchSection div').hasClass('results')) {
                // If results block already displayed, clears all the content, and appends the cars:
                $('#searchSection div.results').empty();
                $('#searchSection div.results').append(HtmlStringToCarsList(CarsList));
            }
            else {
                // If "results block" not already displayed, appends the results block with the cars:
                $('#searchSection').append('<div class="basic-form big results">' + HtmlStringToCarsList(CarsList) + '</div>');
            }
        }
    }


    //------------------------------------------------------------------------------------------------
    // Generates an HTML string in order to display the cars list.
    // (Params) : carsList = The list of car objects, in order to display their content.
    // * Returns : The HTML string created to display the list of cars.
    //------------------------------------------------------------------------------------------------
    function HtmlStringToCarsList(carsList) {
        var html = "";
        html += '<div class="section-results">'; // <A> List Wrapper (Open -->

        // Display for each car in the list:
        for (var i = 0; i < carsList.length; i++) {

            html += '<div class="vehicle-wrapper">'; // <B> Car Wrapper (Open -->
            html += '<input type="hidden" class="vehicle-id" value="' + carsList[i].licensePlate + '">'; // <Empty> Hidden Car Identifier
            html += '<div class="vehicle-content">'; // <C> Car Content (Open -->
            html += '<div class="vehicle-img">'; // <D> Car Image (Open -->
            html += '<img src="/Content/images/' + carsList[i].carImage + '" alt="' + carsList[i].title() + '">'; // <Empty> Car Image
            html += '</div>'; // </D> Car Image <-- Close)
            html += '<h4>' + carsList[i].title() + '</h4>'; // <E> Car Title (Open & Close)
            html += '<p class="vehicle-year">' + carsList[i].year + '</p>'; // <F> Car Year (Open & Close)
            html += '<p class="vehicle-gear">' + carsList[i].transmission() + ' Transmission</p>'; // <G> Car Transmission (Open & Close)
            html += '<div class="vehicle-price">'; // <H> Car Price (Open -->
            html += '<p>Daily Price: <span>$' + carsList[i].dailyPrice + '</span></p>'; // <Empty> Car Price
            html += '</div>'; // </H> Car Price <-- Close)
            html += '</div>'; // </C> Car Content <-- Close)
            html += '<input type="button" class="chk-availability-btn" value="Check Availability" />'; // <Empty> Check Availability Button
            html += '<input type="button" class="order-now-btn" value="Order Now" />'; // <Empty> Order Now Button
            html += '<div class="vehicle-view-details">'; // <I> Car View Details (Open -->
            html += '<a href="/FleetCars/CarDetails/' + carsList[i].licensePlate + '">View Details</a>'; // <Empty> Order Now Button
            html += '</div>'; // </I> Car View Details <-- Close)
            html += '</div>'; // </B> Car Wrapper <-- Close)
        }
        html += '</div>'; // </A> List Wrapper <-- Close)

        return html;
    }


    //------------------------------------------------------------------------------------------------
    // Sorts the cars list, according to a sort keyword provided.
    // (Params) : carsList = The list of car objects, in order to display their content,
    //            sortBy = The sort keyword, which tells the function the sort to apply.
    //            (sortBy keywords: "Manufacturer", "Model", "Year", "Price")
    // * Returns : The sorted cars list.
    //------------------------------------------------------------------------------------------------
    function SortCarsList(carsList, sortBy) {

        carsList.sort(function (a, b) {
            // Sorts the cars list according to the sort keyword:
            if (sortBy === "Manufacturer") {
                if (a.manufacturer < b.manufacturer)
                    return -1;
                else if (a.manufacturer > b.manufacturer)
                    return 1;
                else
                    return 0;
            }
            else if (sortBy === "Model") {
                if (a.model < b.model)
                    return -1;
                else if (a.model > b.model)
                    return 1;
                else
                    return 0;
            }
            else if (sortBy === "Year") {
                if (a.year < b.year)
                    return -1;
                else if (a.year > b.year)
                    return 1;
                else
                    return 0;
            }
            else if (sortBy === "Price") {
                if (a.dailyPrice < b.dailyPrice)
                    return -1;
                else if (a.dailyPrice > b.dailyPrice)
                    return 1;
                else
                    return 0;
            }
        });

        return carsList; // Returns the sorted cars list.
    }


});


//==========================================================================================
// [Results Block] : Actions for every car result: (Check Availability), (Order Now).
//==========================================================================================
$(function () {


    //------------------------------------------------------------------------------------------------
    // [Helper] : Generates an HTML for providing rental dates:
    // (Params) : html = The HTML object to insert the HTML for the sub window.
    //            licenseNumber = The license number of the selected car.
    //            callback = A callback function, that operates after dates provided correctly,
    //                       then shows the car's availability. *(Params: html, licenseNumber)*
    //------------------------------------------------------------------------------------------------
    function GenerateHtmlForProvidingDates(html, licenseNumber, callback) {

        // Title:
        var _provideTitle = $('<div />').addClass('subWindow-title')
                                        .text("We need you to provide your rental dates.")
                                        .appendTo(html);
        // Start Date Field:
        var _startField = $('<div />').addClass('subWindow-field').appendTo(html);
        var _startDateLabel = $('<label />').attr("for", "sub-startDate").text("Start Date: ").appendTo(_startField);
        var _startDateInput = $('<input />').attr({
            type: "date",
            id: "sub-startDate"
        }).appendTo(_startField);

        // Return Date Field:
        var _returnField = $('<div />').addClass('subWindow-field').appendTo(html);
        var _returnDateLabel = $('<label />').attr("for", "sub-returnDate").text("Return Date: ").appendTo(_returnField);
        var _returnDateInput = $('<input />').attr({
            type: "date",
            id: "sub-returnDate"
        }).appendTo(_returnField);

        // Next Button:
        var _nextBtn = $('<input />').attr({
            type: "button",
            value: "Next"
        }).addClass('avail-window-next').appendTo(html).click(function () {

            // Checks if the user entered the dates correctrly:
            if (CheckSubWindowRentalDates()) {
                // OK! the user provided the rental dates correctrly.
                // Closes the 'rental dates provide' form, 
                html.animate({ opacity: "toggle" }, 300, function () {
                    html.empty(); // Removes last html.
                    html.animate({ opacity: "toggle" }, 600); // Returns the html to be visible.
                    callback(html, licenseNumber); // Calls the callback function after dates provided correctly.
                });
            }
        });
    }


    //------------------------------------------------------------------------------------------------
    // [Helper] : Checks that the user provided the rental dates:
    //------------------------------------------------------------------------------------------------
    function CheckRentalDatesProvided() {

        var startDate = $('#StartDate').val();
        var returnDate = $('#ReturnDate').val();

        if (!startDate || !returnDate) {
            return false;
        }

        return true;
    }


    //------------------------------------------------------------------------------------------------
    // [Helper] : Checks that the user provided the rental dates correctly in the Sub-Window:
    // - If dates entered correctly, saves the dates to the search form, for future use.
    // - If not entered correctly, displays a popup message with the specific error.
    // * Returns: true = if dates are valid.
    //            false = if dates are not valid.
    //------------------------------------------------------------------------------------------------
    function CheckSubWindowRentalDates() {

        // Gets the values from the inputs:
        var startDate = $('#sub-startDate').val();
        var returnDate = $('#sub-returnDate').val();
        var today = GetTodaysDate();

        if (!startDate || !returnDate) {
            // User didn't enter rental dates: (user is playing games...)
            popupMsg("<div>Please enter rental dates.</div>");
            return false;
        }
        else if (startDate < today) {
            // User didn't enter rental dates: (user is playing games...)
            popupMsg("<div>You can't return to the past.</div>");
            return false;
        }
        else if (returnDate < startDate) {
            // User entered return date lower than start date:
            popupMsg("<div>Return Date can't be lower than Start Date.</div>");
            return false;
        }

        // Dates OK!
        // Saves the dates to the search form, for future use:
        $('form#searchForm #StartDate').val(startDate);
        $('form#searchForm #ReturnDate').val(returnDate);
        return true;
    }


    //••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••


    //------------------------------------------------------------------------------------------------
    // [Check Availability Button]
    //------------------------------------------------------------------------------------------------
    $('.chk-availability-btn').live('click', function () {

        // Holds the car identifier:
        var licenseNumber = $(this).parent().children(".vehicle-id").val();

        // Creates the sub window for the "check availability":
        var window = new SubWindow("#5ab4ff", function () {
            // Creates the HTML object to insert the HTML for the sub window:
            var _html = $('<div />').addClass('chk-availability-window');

            // Checks if the user didn't provide the rental dates:
            if (!CheckRentalDatesProvided()) {
                // Generates an HTML for providing rental dates:
                GenerateHtmlForProvidingDates(_html, licenseNumber, CheckAvailabilityCallBack);
            }
            else {
                // Dates are provided, checks the availability of the car:
                CheckAvailabilityCallBack(_html, licenseNumber);
            }

            return _html; // Returns all the generated html to the argument, so it will be added to the sub window.
        });
        window.Show(); // Displays the Sub Window.
    });


    //------------------------------------------------------------------------------------------------
    // [CallBack function] operates after : Dates provided correctly, then shows the car's availability.
    // (Params) : html = The HTML object to insert the HTML for the sub window,
    //            licensePlate = The identifier of the car.
    //------------------------------------------------------------------------------------------------
    function CheckAvailabilityCallBack(html, licenseNumber) {

        // Checks the availability of the car:
        var checkResult = CheckCarAvailability(licenseNumber,
                                               $('form#searchForm #StartDate').val(),
                                               $('form#searchForm #ReturnDate').val());
        GenerateHtmlForAvailabilityResult(checkResult, html); // Displays the availability of the car.
    }


    //------------------------------------------------------------------------------------------------
    // [Server Operation] : Checks car's availability by start date and return date.
    // (Params) : licensePlate = The identifier of the car,
    //            startDate = The start date of the car rental,
    //            returnDate = The return date of the car rental.
    // * Returns - true = if the car is available in the dates.
    //             false = if the car is not available in the dates.
    //------------------------------------------------------------------------------------------------
    function CheckCarAvailability(licensePlate, startDate, returnDate) {

        var _availability = false; // Holds the car availability indicator.

        // Creates the check availability model object:
        var dataObject = {
            LicensePlate: licensePlate,
            StartDate: startDate,
            ReturnDate: returnDate
        };

        // Sends an AJAX request to the server, with the check availability model:
        $.ajax({
            type: 'GET',
            url: '/FleetCars/CheckCarAvailability',
            data: dataObject,
            contentType: "application/json;charset=utf-8",
            dataType: 'json',
            success: function (availability) {
                if (availability === "ERROR") {
                    popupMsg("<div>An error has occurred. please try again later.</div>");
                    return;
                }
                _availability = availability; // Assigns to the car availability indicator.
            },
            error: function (error) {
                popupMsg("<div>An error has occurred.</div><div>Code: " + error.status + ", " + error.statusText + "</div>");
            },
            fail: function () {
                popupMsg("<div>Failed to send the request.</div>");
            },
            async: false,
            timeout: 2000
        });

        return _availability;
    }


    //------------------------------------------------------------------------------------------------
    // [UI] : Displays the availability result, to the sub window.
    // (Params) : availability = The availability indicator - true or false - if available or not,
    //            html = The HTML object to insert the HTML for the sub window.
    //------------------------------------------------------------------------------------------------
    function GenerateHtmlForAvailabilityResult(availability, html) {

        var _availability = availability ? "available" : "not available";


        var _title = $('<div />').addClass('subWindow-title')
                                        .text("The car is " + _availability + ".")
                                        .appendTo(html);

        var _subtitle = $('<h2 />').text("During:")
                                .appendTo(html);

        // Start Date Field:
        var _startField = $('<div />').addClass('subWindow-field').appendTo(html);
        var _startDateLabel = $('<label />').text("Start Date: ").appendTo(_startField);
        var _startDate = $('<label />').text($('form#searchForm #StartDate').val()).appendTo(_startField);

        // Return Date Field:
        var _returnField = $('<div />').addClass('subWindow-field').appendTo(html);
        var _returnDateLabel = $('<label />').text("Return Date: ").appendTo(_returnField);
        var _returnDate = $('<label />').text($('form#searchForm #ReturnDate').val()).appendTo(_returnField);

        return _title;
    }


    //••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••


    //------------------------------------------------------------------------------------------------
    // [Order Now Button]
    //------------------------------------------------------------------------------------------------
    $('.order-now-btn').live('click', function () {

        // Gets the order details object (the selected car to rent):
        // Holds the order details (car details):
        var orderDetails = {
            "LicenseNumber": $(this).parent().children(".vehicle-id").val(),
            "CarModel": $(this).parent().children(".vehicle-content").children("h4").text() + " " +
                        $(this).parent().children(".vehicle-content").children("p.vehicle-year").text() + " " +
                        $(this).parent().children(".vehicle-content").children("p.vehicle-gear").text().replace(" Transmission", ""),
            "DailyPrice": $(this).parent().children(".vehicle-content").children(".vehicle-price").children("p").children("span").text().replace("$", ""),
        };

        // Creates the sub window for the "check availability":
        var window = new SubWindow("#ed0", function () {
            // Creates the HTML object to insert the HTML for the sub window:
            var _html = $('<div />').addClass('order-now-window');

            // Checks if the user didn't provide the rental dates:
            if (!CheckRentalDatesProvided()) {
                // Generates an HTML for providing rental dates:
                GenerateHtmlForProvidingDates(_html, orderDetails.LicenseNumber, OrderNowCallBack);
            }
            else {
                // Dates are provided, displays car order details:
                OrderNowCallBack(_html, orderDetails.LicenseNumber);
            }

            return _html; // Returns all the generated html to the argument, so it will be added to the sub window.
        });
        window.Show(); // Displays the Sub Window.
    });

    //------------------------------------------------------------------------------------------------
    // [CallBack function] operates after : Rental dates provided correctly, then shows car order details.
    // (Params) : html = The HTML object to insert the HTML for the sub window,
    //            licenseNumber = The identifier of the car.
    //------------------------------------------------------------------------------------------------
    function OrderNowCallBack(html, licenseNumber) {

        // Holds the car context selector (the car details area to get the details):
        var carContext = $('input[value="' + licenseNumber + '"]').parent().children(".vehicle-content");

        // Holds the details of the selected car for rental:
        var carDetails = {
            "LicenseNumber": licenseNumber,
            "CarModel": carContext.children("h4").text() + " " +
                        carContext.children("p.vehicle-year").text() + " " +
                        carContext.children("p.vehicle-gear").text().replace(" Transmission", ""),
            "DailyPrice": carContext.children(".vehicle-price").children("p").children("span").text().replace("$", ""),
        };

        // Checks the availability of the car:
        var carAvailability = CheckCarAvailability(licenseNumber,
                                               $('form#searchForm #StartDate').val(),
                                               $('form#searchForm #ReturnDate').val());

        if (carAvailability === true) {
            // Car is available:
            GenerateHtmlForOrderDetails(html, carDetails);
        }
        else {
            // Car is not available:
            GenerateHtmlForAvailabilityResult(false, html); // Displays the availability of the car.
        }
    }


    //------------------------------------------------------------------------------------------------
    // [UI] : Displays the order details, to the sub window.
    // (Params) : html = The HTML object to insert the HTML for the sub window,
    //            carDetails = the details of the selected car for rental.
    //------------------------------------------------------------------------------------------------
    function GenerateHtmlForOrderDetails(html, carDetails) {

        var startDate = $('form#searchForm #StartDate').val(); // Holds the rental's Start Date.
        var returnDate = $('form#searchForm #ReturnDate').val(); // Holds the rental's Return Date.
        var totalPrice = CalculateRentalTotalPrice(carDetails.DailyPrice, startDate, returnDate); // Holds the rental's total price.

        // Starts to build the sub window's html:

        // Title:
        var _title = $('<div />').addClass('subWindow-title')
                                        .text("Order Details:")
                                        .appendTo(html);

        // The block of the order details:
        var _detailsBlock = $('<div />').addClass('subWindow-detailsBlock').appendTo(html);

        // Car Model Detail:
        var _carModelField = $('<div />').addClass('subWindow-field').appendTo(_detailsBlock);
        var _carModelLabel = $('<label />').text("Car Model: ").appendTo(_carModelField);
        var _carModelValue = $('<label />').text(carDetails.CarModel).appendTo(_carModelField);

        // Daily Price Detail:
        var _dailyPriceField = $('<div />').addClass('subWindow-field').appendTo(_detailsBlock);
        var _dailyPriceLabel = $('<label />').text("Daily Price: ").appendTo(_dailyPriceField);
        var _dailyPriceValue = $('<label />').addClass('price-val').text("$" + carDetails.DailyPrice).appendTo(_dailyPriceField);

        // Separator:
        var _separator = $('<hr />').appendTo(_detailsBlock);

        // Start Date Field:
        var _startDateField = $('<div />').addClass('subWindow-field').appendTo(_detailsBlock);
        var _startDateLabel = $('<label />').text("Start Date: ").appendTo(_startDateField);
        var _startDateValue = $('<label />').text(startDate).appendTo(_startDateField);

        // Return Date Field:
        var _returnDateField = $('<div />').addClass('subWindow-field').appendTo(_detailsBlock);
        var _returnDateLabel = $('<label />').text("Return Date: ").appendTo(_returnDateField);
        var _returnDateValue = $('<label />').text(returnDate).appendTo(_returnDateField);

        // Separator:
        var _separator = $('<hr />').appendTo(_detailsBlock);

        // Total Price Field:
        var _totalPriceField = $('<div />').addClass('subWindow-field').appendTo(_detailsBlock);
        var _totalPriceLabel = $('<label />').text("Total Price: ").appendTo(_totalPriceField);
        var _totalPriceValue = $('<label />').addClass('price-val').text("$" + totalPrice).appendTo(_totalPriceField);

        // Order Now Button:
        var _orderBtn = $('<input />').attr({
            type: "button",
            value: "Order Now"
        }).addClass('sub-order-now').appendTo(html).click(function () {
            // Redirects to order page:
            window.location.replace("/Orders/Create/" + carDetails.LicenseNumber + "/" + startDate + "/" + returnDate);
        });

        return _title;
    }


    //------------------------------------------------------------------------------------------------
    // [Helper] : Calculates the total price of the car rental according to the rental dates:
    // (Params) : dailyPrice = the price for each day,
    //            startDate = the date the rental starts,
    //            returnDate = the date the rental ends.
    //------------------------------------------------------------------------------------------------
    function CalculateRentalTotalPrice(dailyPrice, startDate, returnDate) {

        var totalPrice = 0; // Holds the total price for the car rental

        // Runs a loop for every day in the rental dates range: 
        for (var start = new Date(startDate) ; start <= Date.parse(returnDate) ; start.setDate(start.getDate() + 1)) {
            totalPrice += +dailyPrice;
        }

        return totalPrice;
    }


});


//==========================================================================================
//  {Change Events}
//==========================================================================================
$(function () {


    //------------------------------------------------------------------------------------------------
    // [Event: "Change"] of : Every input field in the main search form bar, sends a search request.
    //------------------------------------------------------------------------------------------------
    $('form#searchForm input').change(function () {
        $("form#searchForm").submit();
    });


    //------------------------------------------------------------------------------------------------
    // [Event: "Input"] of : FreeText input field in the main search form bar, sends a search request.
    //------------------------------------------------------------------------------------------------
    $('form#searchForm #FreeText').on('input', function () {
        $("form#searchForm").submit();
    });


    //------------------------------------------------------------------------------------------------
    // [Event: "Change"] of : Start Date input, in the main search form:
    //------------------------------------------------------------------------------------------------
    $('form#searchForm #StartDate').change(function () {

        // Sends the selectors of the dates, to the validation helper to handle it:
        StartDateValidationHelper('form#searchForm #StartDate', 'form#searchForm #ReturnDate');
    });


    //------------------------------------------------------------------------------------------------
    // [Event: "Change"] of : Return Date input, in the main search form:
    //------------------------------------------------------------------------------------------------
    $('form#searchForm #ReturnDate').change(function () {

        // Sends the selectors of the dates, to the validation helper to handle it:
        ReturnDateValidationHelper('form#searchForm #ReturnDate', 'form#searchForm #StartDate');
    });


    //------------------------------------------------------------------------------------------------
    // [Event: "Change"] of : Start Date input, in the main search form:
    //------------------------------------------------------------------------------------------------
    $("#sub-startDate").live("change", function () {

        // Sends the selectors of the dates, to the validation helper to handle it:
        StartDateValidationHelper('#sub-startDate', '#sub-returnDate');
    });


    //------------------------------------------------------------------------------------------------
    // [Event: "Change"] of : Return Date input, in the main search form:
    //------------------------------------------------------------------------------------------------
    $("#sub-returnDate").live("change", function () {

        // Sends the selectors of the dates, to the validation helper to handle it:
        ReturnDateValidationHelper('#sub-returnDate', '#sub-startDate');
    });


});


//••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••
//  [Local Storage] : Related to saving the search filters to the local storage, or reading it.
//••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••

//------------------------------------------------------------------------------------------------
// [Client Operation] : Saves the current search (in the main search bar), to the search history, in the local storage:
//------------------------------------------------------------------------------------------------
function SaveCurrentSearchToLocal() {

    // Serializes a JSON string for the search object: 
    var currentSearch = JSON.stringify({
        'Manufacturer': $('#DDLs_SelectedManufacturer option[value="' + $('#DDLs_SelectedManufacturer').val() + '"]').text(),
        'Model': $('#DDLs_SelectedManufacturerModel option[value="' + $('#DDLs_SelectedManufacturerModel').val() + '"]').text(),
        'Year': $('#DDLs_SelectedYear').val(),
        'Gear': $('#DDLs_SelectedGear option[value="' + $('#DDLs_SelectedGear').val() + '"]').text(),
        'FreeText': $('div.basic-control-bar.search #FreeText').val(),
        'StartDate': $('#StartDate').val(),
        'ReturnDate': $('#ReturnDate').val()
    });

    // Checks if the new search to save, has totally empty values:
    if (IsSearchObjectEmpty(JSON.parse(currentSearch))) {
        return; // Not saving empty search values!
    }

    // Appends the new search to local by FILO (First In Last Out):
    // ------------------------------------------------------------

    // Gets an array of search history:
    var searchHistory = GetSearchHistoryFromLocal();

    // Checks if the history array does not contain a search like the new search:
    if (searchHistory.indexOf(currentSearch) === -1) {
        // Adds the new search, at the beginning of the array:
        searchHistory.unshift(currentSearch);

        // Checks if there's more than 5 searches in the history: (limit is 5 searches saving)
        if (searchHistory.length > 5) {
            searchHistory.pop(); // If more than 5, removes the latest search.
        }

        // Saves the new search history array, to the local storage:
        for (var i = 0; i < searchHistory.length; i++) {
            localStorage.setItem("search-history-" + (+i + 1), searchHistory[i]);
        }
    }
}


//------------------------------------------------------------------------------------------------
// [Helper] : Gets an array of search history, from the local storage:
// * Returns: Array of search history (Array of JSON.stringify() 'search' objects).
//------------------------------------------------------------------------------------------------
function GetSearchHistoryFromLocal() {

    var history = []; // Holds the array of search history.

    for (var i = 0; i < localStorage.length; i++) {
        // Checks if the local storage cell is "search-history-(number)":
        if (localStorage.key(i).startsWith("search-history-")) {
            // If the cell is search-history, then adds the search to the history array:
            history.push(localStorage.getItem(localStorage.key(i)));
        }
    }

    return history; // Returns the array of search history.
}


//------------------------------------------------------------------------------------------------
// [Helper] : Indicates if there's a search history to the user, in the local storage:
// * Returns: Indicator true or false, if search history exists or not.
//------------------------------------------------------------------------------------------------
function IsSearchHistoryExists() {

    for (var i = 0; i < localStorage.length; i++) {
        // Checks if the local storage cell is "search-history-(number)":
        if (localStorage.key(i).startsWith("search-history-")) {
            return true; // Search history exists.
        }
    }

    return false; // Search history doesn't exists.
}


//------------------------------------------------------------------------------------------------
// [Helper] : Indicates if a search object is empty, which means no field has a value in it:
// * Returns: Indicator true or false, if search object is empty or not.
//------------------------------------------------------------------------------------------------
function IsSearchObjectEmpty(searchObj) {

    // For every property in the search object:
    for (var prop in searchObj) {
        // Checks if it's actual property of the object, and not prototype:
        if (searchObj.hasOwnProperty(prop)) {

            // Checks if it has a certain value:
            if (searchObj[prop] != "" && searchObj[prop] != "All") {
                return false; // Search object has a value in a property. It's not empty.
            }
        }
    }
    return true; // Here, search object is empty.
}


//------------------------------------------------------------------------------------------------
// [UI] : Gets an array of search history, from the local storage:
//------------------------------------------------------------------------------------------------
function DisplaySearchHistory() {

    // (GUI): Generates the HTML of the search history to display:
    var _wrapper = $('<div />').addClass('basic-form big history');
    var _topControl = $('<div />').addClass('top-control').appendTo(_wrapper);
    var _closeBtn = $('<button />').addClass('history-close').appendTo(_topControl).click(function () {
        _wrapper.animate({ height: "toggle", marginTop: "toggle", borderTopWidth: "toggle", borderBottomWidth: "toggle", opacity: "toggle" }, 600);
    });
    var _content = $('<div />').addClass('s-history-content').appendTo(_wrapper);
    var _title = $('<h4 />').text("Your last searches:").appendTo(_content);
    var _ul = $('<ul />').appendTo(_content);

    // Gets an array of search history:
    var searchHistory = GetSearchHistoryFromLocal();

    // For every search object, in the array of search history:
    for (var i = 0; i < searchHistory.length; i++) {

        // Deserializes the search object, from the JSON.stringify():
        var searchObj = JSON.parse(searchHistory[i]);

        // The list item, that displays one search row in the history:
        var _li = $('<li />').appendTo(_ul);
        var _a = $('<a />').text("Search: " + InitSearchHistoryRowDisplay(searchObj))
                                                .appendTo(_li)
                                                .on("click", { value: searchObj }, function (event) {
                                                    // Assigns the selected search history, to the main search bar:
                                                    AssignSearchObjectToSearchBar(event.data.value);

                                                    // Then, triggers submit event of the main search bar, to get search results:
                                                    $("form#searchForm").submit();
                                                });
    }

    // Appends the search history HTML to the section in the page:
    $('#searchSection').append(_wrapper);
}


//------------------------------------------------------------------------------------------------
// [Helper] : Assigns a search object to the main search bar:
//------------------------------------------------------------------------------------------------
function AssignSearchObjectToSearchBar(searchObj) {

    // Changes the value of the Manufacturer DDL, to the value of the manufacturer in the search object,
    $('#DDLs_SelectedManufacturer').val($('#DDLs_SelectedManufacturer option:contains("' + searchObj['Manufacturer'] + '")').val());

    // Updates the models for the manufacturer, (Not async),
    // So the value of the needed model, will be available for the next command:
    UpdateManufacturerModels(false);

    // Changes the value of the ManufacturerModel DDL, to the value of the model in the search object,
    $('#DDLs_SelectedManufacturerModel').val($('#DDLs_SelectedManufacturerModel option:contains("' + searchObj['Model'] + '")').val());

    // Assigns other search object properties:
    $('#DDLs_SelectedYear').val(searchObj['Year']);
    $('#DDLs_SelectedGear').val($('#DDLs_SelectedGear option:contains("' + searchObj['Gear'] + '")').val());
    $('div.basic-control-bar.search #FreeText').val(searchObj['FreeText']);
    $('#StartDate').val(searchObj['StartDate']);
    $('#ReturnDate').val(searchObj['ReturnDate']);
}


//------------------------------------------------------------------------------------------------
// [Helper + Server Operation] : Updates the manufacturer models select list (DDLs_SelectedManufacturerModel), 
//                               according to the selected manufacturer in the manufacturer select list (DDLs_SelectedManufacturer).
// (Params) : isAsync = Indicator if to perform as asynchronous, or not, true or false. 
//------------------------------------------------------------------------------------------------
function UpdateManufacturerModels(isAsync) {
    // Sends AJAX request, every select index change, to get the manufacturer models for the manufacturer:
    $.ajax({
        type: 'GET',
        url: '/ManufacturerModels/GetModelsForManufacturer',
        data: { manufacturerID: $("#DDLs_SelectedManufacturer").val() },
        async: isAsync,
        success: function (models) {
            var ddlSelectedManufacturerModel = $("#DDLs_SelectedManufacturerModel"); // Gets the DropDownList of the ManufacturerModels

            // Clears all previous options:
            $("#DDLs_SelectedManufacturerModel > option").remove();
            $("#DDLs_SelectedManufacturerModel").append($('<option></option>').val(null).text('All'));

            // If there are Manufacturer Models, populates them to the select list:
            if (models != null) {
                for (i = 0; i < models.length; i++) {
                    ddlSelectedManufacturerModel.append($("<option />").val(models[i].Value).text(models[i].Text));
                }
            }
        }
    });
}


//------------------------------------------------------------------------------------------------
// [Helper] : Gets an array of search history, from the local storage:
// * Returns : (string) - the format of the search row to display.
//------------------------------------------------------------------------------------------------
function InitSearchHistoryRowDisplay(searchObj) {

    var display = []; // Holds the array of search values to display, from the searchObj row.

    // For every property in the search row object:
    for (var prop in searchObj) {
        // Checks if it's actual property of the object, and not prototype:
        if (searchObj.hasOwnProperty(prop)) {

            // Adds the property to the display array, only if it has a certain value:
            if (searchObj[prop] != "" && searchObj[prop] != "All") {

                // Checks if the property is "FreeText", or "StartDate", or "ReturnDate", changes the format a little:
                if (prop == "FreeText") {
                    display.push('Text: "' + searchObj[prop] + '"');
                }
                else if (prop == "StartDate") {
                    display.push("Start Date: " + searchObj[prop]);
                }
                else if (prop == "ReturnDate") {
                    display.push("Return Date: " + searchObj[prop]);
                }
                else {
                    // Any other property:
                    display.push(searchObj[prop]);
                }
            }
        }
    }

    return display.join(", ") + "."; // Returns the format of search row display.
}

