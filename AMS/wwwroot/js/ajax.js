// ----------------------------------------------------------------------------------------------------------------------
// Start of AJAX calls for assets, Brands, Models, Devices, categories, states, statuses, and locations
// ----------------------------------------------------------------------------------------------------------------------

var divAssets = document.getElementById("divAssets");

//Fills the Assets Div if it exists
if (document.contains(divAssets)) {
    //$.ajax({
    //    type: "GET",
    //    url: "/Base/getAllAssets",
    //    data: "{}",
    //    success: function (data) {
    //        var s = '<option selected disabled hidden>Please Select an Asset</option>';
    //        for (var i = 0; i < data.length; i++) {
    //            s += '<option value="' + data[i].assetID + '">' + data[i].assetNumber + " | " + data[i].assetBrand + " | " + data[i].assetDevice + " | " + data[i].assetModel + '</option>';
    //        }

    //        //Fills slAssets
    //        $("#slAssets").html(s);
    //    },
    //    error: function () {
    //        alert("There was a problem with Assets");
    //    }
    //});
}

var divCategories = document.getElementById("divCategories");

//Fills the Categories Div if it exists
if (document.contains(divCategories)) {
    $.ajax({
        type: "GET",
        url: "/Base/getAllCategories",
        data: "{}",
        success: function (data) {
            var s = '<option selected disabled hidden>Please Select a Category</option>';
            if (document.contains(document.getElementById("hdCategoryID"))) {
                var hdCategoryID = document.getElementById("hdCategoryID").defaultValue;

                for (var i = 0; i < data.length; i++) {
                    if (data[i].categoryID == hdCategoryID) {
                        s += '<option value="' + data[i].categoryID + '" selected>' + data[i].categoryName + '</option>';
                    } else {
                        s += '<option value="' + data[i].categoryID + '">' + data[i].categoryName + '</option>';
                    }
                }

            } else {
                for (i = 0; i < data.length; i++) {
                    s += '<option value="' + data[i].categoryID + '">' + data[i].categoryName + '</option>';
                }
            }

            //Fills slCategories
            $("#slCategories").html(s);
        },
        error: function () {
            alert("There was a problem with Categories");
        }
    });
}

var divStates = document.getElementById("divStates");

//Fills the states Div if it exists
if (document.contains(divStates)) {
    $.ajax({
        type: "GET",
        url: "/Base/getAllStates",
        data: "{}",
        success: function (data) {
            var s = '<option selected disabled hidden>Please Select a State</option>';
            if (document.contains(document.getElementById("hdStateID"))) {
                var hdStateID = document.getElementById("hdStateID").defaultValue;
                for (var i = 0; i < data.length; i++) {
                    if (data[i].stateID == hdStateID) {
                        s += '<option value="' + data[i].stateID + '" selected>' + data[i].stateName + '</option>';
                    } else {
                        s += '<option value="' + data[i].stateID + '">' + data[i].stateName + '</option>';
                    }
                }

            } else {
                for (i = 0; i < data.length; i++) {
                    s += '<option value="' + data[i].stateID + '">' + data[i].stateName + '</option>';
                }
            }

            //Fills slStates
            $("#slStates").html(s);
        },
        error: function () {
            alert("There was a problem with States");
        }
    });
}

var divStatuses = document.getElementById("divStatuses");

//Fills the Statuses Div if it exists
if (document.contains(divStatuses)) {
    $.ajax({
        type: "GET",
        url: "/Base/getAllStatuses",
        data: "{}",
        success: function (data) {
            var s = '<option selected disabled hidden>Please Select a Status</option>';
            if (document.contains(document.getElementById("hdStatusID"))) {
                var hdStatusID = document.getElementById("hdStatusID").defaultValue;
                for (var i = 0; i < data.length; i++) {
                    if (data[i].statusID == hdStatusID) {
                        s += '<option value="' + data[i].statusID + '" selected>' + data[i].statusName + '</option>';
                    } else {
                        s += '<option value="' + data[i].statusID + '">' + data[i].statusName + '</option>';
                    }
                }

            } else {
                for (i = 0; i < data.length; i++) {
                    s += '<option value="' + data[i].statusID + '">' + data[i].statusName + '</option>';
                }
            }

            //Fills slStatuses
            $("#slStatuses").html(s);
        },
        error: function () {
            alert("There was a problem with Statuses");
        }
    });
}

var divLocations = document.getElementById("divLocations");

//Fills the Locations Div if it exists
if (document.contains(divLocations)) {
    $.ajax({
        type: "GET",
        url: "/Base/getAllLocations",
        data: "{}",
        success: function (data) {
            var s = '<option selected disabled hidden>Please Select a Location</option>';
            if (document.contains(document.getElementById("hdLocationID"))) {
                var hdLocationID = document.getElementById("hdLocationID").defaultValue;
                for (var i = 0; i < data.length; i++) {
                    if (data[i].locationID == hdLocationID) {
                        s += '<option value="' + data[i].locationID + '" selected>' + data[i].locationName + '</option>';
                    } else {
                        s += '<option value="' + data[i].locationID + '">' + data[i].locationName + '</option>';
                    }
                }

            } else {
                for (i = 0; i < data.length; i++) {
                    s += '<option value="' + data[i].locationID + '">' + data[i].locationName + '</option>';
                }
            }

            //Fills slLocations
            $("#slLocations").html(s);
            $("#slLocations").trigger("chosen:updated");
        },
        error: function () {
            alert("There was a problem with Locations");
        }
    });
}

var divBrands = document.getElementById("divBrands");

//Checks if a select list of brands exists then it fills it
if (document.contains(divBrands)) {

    if (document.contains(document.getElementById("hdBrandID"))) {
        var BrandID = document.getElementById("hdBrandID").value;
        var ModelID = document.getElementById("hdModelID").value;
        var DeviceID = document.getElementById("hdDeviceID").value;

        $.ajax({
            type: "GET",
            url: "/Base/getAllBrands",
            data: "{}",
            success: function (data) {
                var s = '<option disabled hidden>Please Select a Brand</option>';
                for (var i = 0; i < data.length; i++) {
                    if (data[i].brandID == BrandID) {
                        s += '<option value="' + data[i].brandID + '" selected>' + data[i].brandName + '</option>';
                    } else {
                        s += '<option value="' + data[i].brandID + '">' + data[i].brandName + '</option>';
                    }
                }

                //Fills slBrands then unhides divModels
                $("#slBrands").empty();
                $("#slBrands").html(s);

                var BrandSelected = $("#slBrands option:selected");
                $.ajax({
                    type: "POST",
                    url: "/Base/getDevicesByBrandID",
                    data: { id: BrandSelected.val() },
                    success: function (data) {
                        var s = '<option disabled hidden>Please Select a Device</option>';
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].deviceID == DeviceID) {
                                s += '<option value="' + data[i].deviceID + '" selected>' + data[i].deviceName + '</option>';
                            } else {
                                s += '<option value="' + data[i].deviceID + '">' + data[i].deviceName + '</option>';
                            }
                        }

                        //Fills slDevices then unhides divModels
                        $("#slDevices").empty();
                        $("#slDevices").html(s);

                        var DeviceSelected = $("#slDevices option:selected");
                        $.ajax({
                            type: "POST",
                            url: "/Base/getModelsByDeviceID",
                            data: { id: DeviceSelected.val() },
                            success: function (data) {
                                var s = '<option disabled hidden>Please Select a Model</option>';
                                for (var i = 0; i < data.length; i++) {
                                    if (data[i].modelID == ModelID) {
                                        s += '<option value="' + data[i].modelID + '" selected>' + data[i].modelName + '</option>';
                                    } else {
                                        s += '<option value="' + data[i].modelID + '">' + data[i].modelName + '</option>';
                                    }
                                }

                                //Fills slModels then unhides divModels
                                $("#slModels").empty();
                                $("#slModels").html(s);
                            }, error: function () {
                                alert("Issue with Models list");
                            }
                        });
                    }, error: function () {
                        alert("Issue with Devices list");
                    }
                });
            }, error: function () {
                alert("Issue with Brands list");
            }
        });
    }
    //Hide the Model and Device divs
    var divModels = document.getElementById("divModels");
    var slModels = document.getElementById("slModels");
    var divDevices = document.getElementById("divDevices");
    var slDevices = document.getElementById("slDevices");
    var divBrands = document.getElementById("divBrands");
    var slBrands = document.getElementById("slBrands");
    var btnSubmitbrandModal = document.getElementById("btnSubmitbrandModal");

    if ($(".assetCreate")[0]) {
        divDevices.style.display = "none";
        divModels.style.display = "none";

        $.ajax({
            type: "GET",
            url: "/Base/getAllBrands",
            data: "{}",
            success: function (data) {
                var s = '<option selected disabled hidden>Please Select a Brand</option>';
                for (var i = 0; i < data.length; i++) {
                    s += '<option value="' + data[i].brandID + '">' + data[i].brandName + '</option>';
                }

                //Fills slBrands then unhides divModels
                $("#slBrands").empty();
                $("#slBrands").html(s);
            }, error: function () {
                alert("There was a problem with Brands");
            }

        });
    }

    //When there is a change in the slBrand
    slBrands.addEventListener("change", function () {
        $("#slDevices").empty();
        divDevices.style.display = "none";
        $("#slModels").empty();
        divModels.style.display = "none";
        var selected = $("#slBrands option:selected");



        $.ajax({
            type: "POST",
            url: "/Base/getDevicesByBrandID",
            data: { id: selected.val() },
            success: function (data) {
                var s = '<option selected disabled hidden>Please Select a Device</option>';
                for (var i = 0; i < data.length; i++) {
                    s += '<option value="' + data[i].deviceID + '">' + data[i].deviceName + '</option>';
                }

                //Fills slBrands then unhides divDevices
                $("#slDevices").empty();
                $("#slDevices").html(s);
                if (selected.text() !== "Please Select a Brand") {
                    divDevices.style.display = "block";
                } else {
                    divDevices.style.display = "none";
                }


                //When there is a change in the slModels
                slDevices.addEventListener("change", function () {
                    divModels.style.display = "none";
                    var selected = $("#slDevices option:selected");

                    $.ajax({
                        type: "POST",
                        url: "/Base/getModelsByDeviceID",
                        data: { id: selected.val() },
                        success: function (data) {
                            var s = '<option selected disabled hidden>Please Select a Model</option>';
                            for (var i = 0; i < data.length; i++) {
                                s += '<option value="' + data[i].modelID + '">' + data[i].modelName + '</option>';
                            }
                            // Fills the select list for Models with Models based on device selected
                            $("#slModels").empty();
                            $("#slModels").html(s);
                            if (selected.text() !== "Please Select a Device") {
                                divModels.style.display = "block";
                            } else {
                                divModels.style.display = "none";

                            }


                        },
                        error: function () {
                            alert("There was a problem with Models");
                        }
                    });

                }, false);


            },
            error: function () {
                alert("There was a problem with Devices");
            }
        });

    }, false);
}


var divUsers = document.getElementById("divUsers");

//Fills the Users Div if it exists
if (document.contains(divUsers)) {
    $.ajax({
        type: "GET",
        url: "/Base/getAllUsers",
        data: "{}",
        success: function (data) {
            var s = $("#slUsers").html();
            for (var i = 0; i < data.length; i++) {
                s += '<option value="' + data[i].userID + '">' + data[i].userFirstName + ' ' + data[i].userLastName + '</option>';
            }

            //Fills slUsers
            $("#slUsers").empty();
            $("#slUsers").html(s);
            $("#slUsers").trigger("chosen:updated");
        },
        error: function () {
            alert("There was a problem with Users");
        }
    });
}
// ----------------------------------------------------------------------------------------------------------------------
// End of AJAX calls for Brands, Models, and Devices
// ----------------------------------------------------------------------------------------------------------------------

// ----------------------------------------------------------------------------------------------------------------------
// Remove Asset From Ticket
// Used in the Tickets Edit View
// ----------------------------------------------------------------------------------------------------------------------