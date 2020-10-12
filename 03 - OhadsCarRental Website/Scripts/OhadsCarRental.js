/*••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••
  OhadsCarRental.js ==> Represents the general script for the whole website:
  --------------------------------------------------------------------------
  This is the main script for the website. Loads in every page in the site. (Located in the layout).
  Provides general actions, operations, helpers, for many pages in the website.
••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••*/


//==========================================================================================
//  [General Helpers]:
//==========================================================================================
// (String Helper) : Starts With - Checks if a string starts with a string:
if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (str) {
        return this.substring(0, str.length) === str;
    }
};

// (Number Helper) : Formats a number to money:
// (Params) : c = The number of characters, in a decimal number after the dot.
Number.prototype.formatMoney = function (c) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

//------------------------------------------------------------------------------------------------
// [Validation Helper] for : 'Start Date' input. Helps the validation of rental dates to be in a safe range, 
//                            by limiting the date values according to the validation.
// (Params) : startDateSelector = The selector of the start date field,
//            returnDateSelector = The selector of the return date field.
//------------------------------------------------------------------------------------------------
function StartDateValidationHelper(startDateSelector, returnDateSelector) {

    var today = GetTodaysDate(); // Today's date string value.
    var startVal = $(startDateSelector).val(); // Start Date string value.
    var returnVal = $(returnDateSelector).val(); // Return Date string value.
    var startDate = new Date(startVal); // Date object of Start Date.
    var returnDate = new Date(returnVal); // Date object of Return Date. 

    // If there's a value in the Start Date field, AND - it's a correct date:
    if (startVal != "" && startDate instanceof Date) {

        // If the user assigns Start Date lower than today's date.
        if (startVal < today) {
            // Sets the value of Start Date to today's date:
            $(startDateSelector).val(today);
        }

        // If Start Date is lower than Return Date:
        if (startDate < returnDate) {
            return; // It's OK, that's the correct use. 
        }

        // If the user assigns the Start Date for the first time.
        // Or, if the user assigns Start Date bigger than Return Date.

        // Sets the value of Return Date to Start Date:
        $(returnDateSelector).val($(startDateSelector).val());
    }
}

//------------------------------------------------------------------------------------------------
// [Validation Helper] for : 'Return Date' input. Helps the validation of rental dates to be in a safe range, 
//                            by limiting the date values according to the validation.
// (Params) : returnDateSelector = The selector of the return date field,
//            startDateSelector = The selector of the start date field.
//------------------------------------------------------------------------------------------------
function ReturnDateValidationHelper(returnDateSelector, startDateSelector) {

    var today = GetTodaysDate(); // Today's date string value.
    var startVal = $(startDateSelector).val(); // Start Date string value.
    var returnVal = $(returnDateSelector).val(); // Return Date string value.
    var startDate = new Date(startVal); // Date object of Start Date.
    var returnDate = new Date(returnVal); // Date object of Return Date. 

    // If there's a value in the Return Date field, AND - it's a correct date:
    if (returnVal != "" && returnDate instanceof Date) {

        if (returnVal < today) {
            // If the user assigns Return Date lower than today's date.
            // Sets the value of Return Date to today's date:
            $(returnDateSelector).val(today);
        }

        if (startDate > returnDate) {
            // If Return Date is lower than Start Date, sets the Return Date to Start Date:
            $(returnDateSelector).val($(startDateSelector).val());
        }
    }
}


//------------------------------------------------------------------------------------------------
//  [DateTime Helper] - Gets today's date, in a correct format.
//------------------------------------------------------------------------------------------------
function GetTodaysDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; // January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }

    if (mm < 10) {
        mm = '0' + mm;
    }

    return yyyy + '-' + mm + '-' + dd; // Returns the needed format of the date.
}

//------------------------------------------------------------------------------------------------
//  [DateTime Helper] - Displays a date in a correct format:
//------------------------------------------------------------------------------------------------
function DisplayDate(date) {
    date = new Date(date);
    var dd = date.getDate();
    var mm = date.getMonth() + 1; // January is 0!
    var yyyy = date.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }

    if (mm < 10) {
        mm = '0' + mm;
    }

    if (!yyyy || !mm || !dd) {
        return undefined;
    }

    return mm + '/' + dd + '/' + yyyy; // Returns the needed format of the date.
}


//==========================================================================================
//  GUI Loading Spinner
//  - When called, shows a loading spinner, or hides it. In order to illustrate operations.
//==========================================================================================
// Shows the GUI Loading Spinner in the selector:
function ShowLoading(selector) {
    $(selector).append('<div class="loading-wrapper"><div class="loading-spinner"></div></div>');
}

// Hides the GUI Loading Spinner in the selector:
function HideLoading(selector) {
    $(selector).children(".loading-wrapper").remove();
}


//==========================================================================================
//  Popup Message Alert
//  - When called, shows a pop up message on the page, with the text provided to display.
//  (html) = to provide the html to insert to the popup message.
//==========================================================================================
var popupMsg = function (html) {

    var popMsg_wrapper = $('<div />').addClass('popMsg-wrapper'),
      popMsg_cell = $('<div />').addClass('popMsg-cell')
      .appendTo(popMsg_wrapper),
      popMsg_bar = $('<div />').addClass('popMsg-bar')
      .appendTo(popMsg_cell).animate({ opacity: "toggle" }, 500),
      popMsg_content = $('<div />').addClass('popMsg-content')
      .html(html)
      .appendTo(popMsg_bar),
      button = $('<button />').appendTo(popMsg_content)
      .text("OK")
      .click(function () {
          popMsg_wrapper.remove();
      });

    $('body').append(popMsg_wrapper);

}


//==========================================================================================
//  Sub Window
//  - Represents a sub window object.
//    Shows a new window that contains some content, over the page.
//==========================================================================================
// C'tor Function:
function SubWindow(bgColor, html) {

    // Properties:
    this.bgColor = bgColor;
    this.html = html;
}
SubWindow.prototype.Show = function () {
    var _wrapper = $('<div />').addClass('subWindow-wrapper');
    var _window = $('<div />').addClass('subWindow-window').css("background-color", this.bgColor)
                              .appendTo(_wrapper).animate({ opacity: "toggle" }, 500);
    var _close = $('<button />').addClass('subWindow-close').appendTo(_window).click(function () {
        _window.animate({ height: '+=400px', opacity: "toggle" }, 600, function () {
            _wrapper.animate({ opacity: "toggle" }, 600, function () {
                _wrapper.remove();
            });
        });

    });
    var _content = $('<div />').addClass('subWindow-content').html(this.html).appendTo(_window);

    $('section.actualContent').append(_wrapper);
};


//==========================================================================================
//  (Layout) - Header Buttons
//==========================================================================================
// Menu Button: toggles the sidebar.
$(function () {
    $("#btn-menu").on("click", function () {

        // Makes the button active:
        $(this).toggleClass("active");

        // Tells the content block to share space with the sidebar:
        $("div.content").toggleClass("menu-open");

        // Closes the login form:
        $("#btn-sign-in").removeClass("active");
        $("#login").removeClass("visible");
    });
});

// Login Button: displays login form, or navigation for logged in users.
$(function () {
    $("#btn-sign-in").on("click", function () {

        // Makes the button active:
        $(this).toggleClass("active");

        // Opens the login block:
        $("#login").toggleClass("visible");

        // Closes the sidebar:
        $("#btn-menu").removeClass("active");
        $("div.content").removeClass("menu-open");
    });
});


//==========================================================================================
//  Contacts Us Form Submission
//==========================================================================================
$(function () {

    // Submits Contact Us form:
    $("#contactUsForm").submit(function (e) {
        e.preventDefault();

        // Acts only if the form is valid:
        if ($(this).valid()) {
            // Shows loading spinner to the GUI:
            ShowLoading("#contact-us-block");

            // Creating the contact object:
            var dataObject = JSON.stringify({
                'FirstName': $('#FirstName').val(),
                'LastName': $('#LastName').val(),
                'Email': $('#Email').val(),
                'Phone': $('#Phone').val(),
                'Text': $('#Text').val()
            });

            // Setting 1 second delay to show the cool GUI loading spinner:
            setTimeout(function () {
                // After 1 second:
                // Sending through AJAX:
                $.ajax({
                    type: 'POST',
                    url: '/Contacts/SubmitContact',
                    contentType: 'application/json',
                    data: dataObject,
                    success: function (isSubmitted) {
                        HideLoading("#contact-us-block"); // Hides loading spinner from the GUI.

                        if (isSubmitted === true) {
                            $("#contact-us-block").html('<p id="submissionStatus" class="form-submitted"></p>');
                            $("#submissionStatus").html("We have received your contact. Thank you.");
                        }
                        else {
                            $("#submissionStatus").html("An error has occurred while contact submission, please try again later.");
                        }
                    },
                    error: function () {
                        HideLoading("#contact-us-block"); // Hides loading spinner from the GUI.
                        $("#submissionStatus").html("An error has occurred while contact submission, please try again later.");
                    },
                    fail: function () {
                        HideLoading("#contact-us-block"); // Hides loading spinner from the GUI.
                        $("#submissionStatus").html("An error has occurred while contact submission, please try again later.");
                    },
                    timeout: 6000
                });

            }, 1000);
        }
    });

});


//==========================================================================================
//  Contacts Watch
//==========================================================================================
$(function () {

    // Click on the table's row:
    $("div.model-tbl-tr.contacts").on("click", function () {

        // Read Update - if unread, (Sending by AJAX):
        if ($(this).hasClass("unread")) {
            // Removes the unread class:
            $(this).removeClass("unread");

            // Updates the unread contact message to read by AJAX:
            $.ajax({
                type: 'POST',
                url: '/Contacts/UpdateUnreadContact',
                data: { contactID: $(this).children("input[type='hidden']").val() }
            });
        }

    });

});


//==========================================================================================
//  model-tbl (Model Table) - Row open function
//==========================================================================================
$(function () {

    // Click on the table's row:
    $("div.model-tbl-tr").on("click", function () {
        // Opens or Closes the next element, details block:
        $(this).next().animate({ left: "+=50", height: "toggle" }, 500).toggleClass("active");
    });

});


//==========================================================================================
//  [UI] : Back Button (Goes back in the history)
//==========================================================================================
$(function () {

    // Click on the Back Button:
    $("#backBtn").on("click", function () {
        // Goes back in the history:
        window.history.back();
    });

});


//------------------------------------------------------------------------------------------
//                               v v v Temporary Test v v v
//------------------------------------------------------------------------------------------



