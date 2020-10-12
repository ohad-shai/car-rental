/*••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••
  DropDownLists.js ==> Represents actions with DDLs in the site:
  --------------------------------------------------------------
  This script handles all the drop down lists in the site.
••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••*/


//==========================================================================================
//  DropDownLists in 'Car Model' Management:
//==========================================================================================
$(function () {

    // 'Change' event for: Manufacturer Model Select List
    // When changing the select option in the DDL, updates the hidden "ManufacturerModelID" property:
    $("#DDLs_SelectedManufacturerModel").change(function () {

        // Gets the value, from the selected option, in the 'Models' list:
        var modelID = $(this).find("option:selected").attr("value");

        // Assigns the ModelID from the DDL, to the hidden 'ManufacturerModelID':
        $('#CarModel_ManufacturerModelID').val(modelID);
    });

    // 'Change' event for: Gear Select List
    // When changing the select option in the DDL, updates the hidden "ManualGear" property:
    $("#GearDDL_SelectedGear").change(function () {

        // Gets the value, from the selected option, in the 'Gear' list:
        var value = $(this).find("option:selected").attr("value");

        // Assigns the ModelID to the hidden ManufacturerModelID:
        $('#CarModel_ManualGear').val(value);
    });

    // 'Ready' event for: Gear Select List
    $("#GearDDL_SelectedGear").ready(function () {
        // Only if defined:
        if ($('#CarModel_ManualGear').val() != undefined) {
            // Gets the value of the gear from the hidden element:
            var gearValue = $('#CarModel_ManualGear').val().toLowerCase();

            // Selects the option according to the Gear hidden element:
            $('#GearDDL_SelectedGear option[value="' + gearValue + '"]').attr('selected', 'selected');
        }
    });

});


//==========================================================================================
//  DropDownLists in 'Fleet Car' Management:
//==========================================================================================
$(function () {

    // 'Change' event for: Manufacturer Select List
    // When changing the select option of the "Manufacturer" DDL, sends AJAX request in order to fill the car models DDL:
    $("#DDLs_SelectedMfrToCarMdl").change(function () {
        // Sends AJAX request, every select index change, to get the manufacturer models for the manufacturer:
        $.ajax({
            type: 'GET',
            url: '/CarModels/GetCarModelsForManufacturer',
            data: { manufacturerID: $(this).val() },
            success: function (models) {
                var ddlCarModel = $("#DDLs_SelectedCarMdlFromMfr"); // Gets the DropDownList of the CarModels.

                // Gets the default option for the DDL:
                var defaultOption = $("#DDLs_SelectedCarMdlFromMfr > option").first().text();
                // Clears all previous options:
                $("#DDLs_SelectedCarMdlFromMfr > option").remove();
                // Appends the default option:
                $("#DDLs_SelectedCarMdlFromMfr").append($('<option></option>').val(null).text(defaultOption));

                // If there are Manufacturer Models, populates them to the select list:
                if (models != null) {
                    for (i = 0; i < models.length; i++) {
                        ddlCarModel.append($("<option />").val(models[i].Value).text(models[i].Text));
                    }
                }

                // Checks if hidden "FleetCar_CarModelID" has a value, in order to select it's option in the list:
                var hiddenCarID = $('#FleetCar_CarModelID').val();
                if (hiddenCarID != "") {
                    // Selects the option in the DDL, according to the CarModelID hidden element:
                    $('#DDLs_SelectedCarMdlFromMfr option[value="' + hiddenCarID + '"]').attr('selected', 'selected');
                }
            }
        });
    });

    // 'Ready' event for: Manufacturer Select List
    // Initializes some values for the DDL:
    $("#DDLs_SelectedMfrToCarMdl").ready(function () {

        // Gets the value of the Manufacturer from the hidden element:
        var mfrValue = $('#CarModelMfr').val();

        // Selects the option in the DDL, according to the Manufacturer hidden element:
        $('#DDLs_SelectedMfrToCarMdl option[value="' + mfrValue + '"]').attr('selected', 'selected');

        // Triggers 'change' event, in order to perform an AJAX operation that fills the car models DDL:
        $('#DDLs_SelectedMfrToCarMdl').change();
    });

    // 'Change' event for: Car Model Select List
    // When changing the select option in the DDL, updates the hidden "CarModelID" property:
    $("#DDLs_SelectedCarMdlFromMfr").change(function () {

        // Gets the value, from the selected option, in the 'Models' list:
        var carModelID = $(this).find("option:selected").attr("value");

        // Assigns the carModelID from the DDL, to the hidden 'CarModelID':
        $('#FleetCar_CarModelID').val(carModelID);
    });

});


//==========================================================================================
//  DropDownLists in 'Search' page:
//==========================================================================================
$(function () {

    // 'Change' event for: Manufacturer Select List
    // When changing the select option of the "Manufacturer" DDL, sends AJAX request in order to fill the models DDL:
    $("#DDLs_SelectedManufacturer").change(function () {
        // Sends AJAX request, every select index change, to get the manufacturer models for the manufacturer:
        $.ajax({
            type: 'GET',
            url: '/ManufacturerModels/GetModelsForManufacturer',
            data: { manufacturerID: $(this).val() },
            success: function (models) {
                var ddlSelectedManufacturerModel = $("#DDLs_SelectedManufacturerModel"); // Gets the DropDownList of the ManufacturerModels

                // Gets the default option for the DDL:
                var defaultOption = $("#DDLs_SelectedManufacturerModel > option").first().text();
                // Clears all previous options:
                $("#DDLs_SelectedManufacturerModel > option").remove();
                // Appends the default option:
                $("#DDLs_SelectedManufacturerModel").append($('<option></option>').val(null).text(defaultOption));

                // If there are Manufacturer Models, populates them to the select list:
                if (models != null) {
                    for (i = 0; i < models.length; i++) {
                        ddlSelectedManufacturerModel.append($("<option />").val(models[i].Value).text(models[i].Text));
                    }
                }
            }
        });
    });

});

