
//Logic for the adding a brand with a modal
$(document).ready(function () {
    $("#btnAddbrand").click(function () {
        $("#brandModal").modal('show');
    });

    $("#btnSubmitbrandModal").click(function () {
        //Getting elements and their values
        var divDevices = document.getElementById("divDevices");
        var brandNameInput = document.getElementById("brandName");
        var slBrands = document.getElementById("slBrands");
        var divModels = document.getElementById("divModels");


        //Sending the created model to the DAL
        $.ajax({
            url: "/Base/AddBrand",
            type: 'POST',
            data: { name: brandNameInput.value },
            success: function () {

                $.ajax({
                    type: "GET",
                    url: "/Base/getAllBrands",
                    data: "{}",
                    success: function (data) {
                        var BrandOptions = '<option selected disabled hidden>Please Select a Brand</option>';
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].brandName === brandNameInput.value) {
                                BrandOptions += '<option value="' + data[i].brandID + '"selected>' + data[i].brandName + '</option>';
                            } else {
                                BrandOptions += '<option value="' + data[i].brandID + '">' + data[i].brandName + '</option>';
                            }
                        }

                        //Fills slBrands then unhides divModels
                        $("#slBrands").empty();
                        $("#slDevices").empty();
                        $("#slModels").empty();
                        $("#slBrands").html(BrandOptions);

                        var DeviceOptions = '<option selected disabled hidden>Please Select a Device</option>';
                        $("#slDevices").empty();
                        $("#slModels").empty();
                        $("#slDevices").html(DeviceOptions);
                        divDevices.style.display = "Block";
                        divModels.style.display = "none";

                    }
                });





                //var selected = $("#slBrands option:selected");

                //$.ajax({
                //    type: "POST",
                //    url: "/Base/getDevicesByBrandID",
                //    data: { id: selected.val() },
                //    success: function (data) {
                //        var s = '<option selected disabled hidden>Please Select a Device</option>';
                //        for (var i = 0; i < data.length; i++) {
                //            s += '<option value="' + data[i].deviceID + '">' + data[i].deviceName + '</option>';
                //        }
                //        // Fills the select list for Models with Models based on device selected
                //        $("#slDevices").empty();
                //        $("#slModels").empty();
                //        $("#slDevices").html(s);
                //        divDevices.style.display = "block";


                //    },
                //    error: function () {
                //        alert("There was a problem with Devices");
                //    }
                //});
            },
            error: function () {
                alert("There was a problem with Brands");
            }
        });

        $("#brandModal").modal('hide');
    });
    //end of ("#btnSubmitbrandModal").click

    $("#btnClosebrandModal").click(function () {
        $("#brandModal").modal('hide');
    });
});

//Logic for the adding a device with a modal
$(document).ready(function () {
    $("#btnAdddevice").click(function () {
        $("#deviceModal").modal('show');
    });

    $("#btnSubmitdeviceModal").click(function () {
        //Getting elements and their values
        var divModels = document.getElementById("divModels");
        var modelDeviceBrandInput = document.getElementById("Model_Device_BrandID");
        var slBrands = document.getElementById("slBrands");
        var currentlySelectedBrandValue = slBrands.options[slBrands.selectedIndex].value;
        var BrandNameModel = slBrands.options[slBrands.selectedIndex].innerText;
        modelDeviceBrandInput.value = currentlySelectedBrandValue;
        var deviceNameInput = document.getElementById("deviceName");

        var BrandID = $("#slBrands option:selected").val();


        $.ajax({
            url: "/Base/AddDevice",
            type: 'POST',
            data: { name: deviceNameInput.value, brandID: BrandID },
            success: function () {

                $.ajax({
                    type: "POST",
                    url: "/Base/getDevicesByBrandID",
                    data: { id: BrandID },
                    success: function (data) {
                        var DeviceOptions = '<option selected disabled hidden>Please Select a Device</option>';
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].deviceName === deviceNameInput.value) {
                                DeviceOptions += '<option value="' + data[i].deviceID + '"selected>' + data[i].deviceName + '</option>';
                            } else {
                                DeviceOptions += '<option value="' + data[i].deviceID + '">' + data[i].deviceName + '</option>';
                            }
                        }


                        divDevices.style.display = "block";
                        $("#slDevices").empty();
                        $("#slDevices").html(DeviceOptions);

                        var ModelOptions = '<option selected disabled hidden>Please Select a Model</option>';
                        $("#slModels").empty();
                        $("#slModels").html(ModelOptions);
                        divModels.style.display = "Block";

                    }
                });

                //var selected = $("#slDevices option:selected");

                //$.ajax({
                //    type: "POST",
                //    url: "/Base/getModelsByDeviceID",
                //    data: { id: selected.val() },
                //    success: function (data) {
                //        var s = '<option selected disabled hidden>Please Select a Model</option>';
                //        for (var i = 0; i < data.length; i++) {
                //            s += '<option value="' + data[i].modelID + '">' + data[i].modelName + '</option>';
                //        }
                //        // Fills the select list for Models with Models based on Model selected
                //        $("#slModels").empty();
                //        $("#slModels").html(s);

                //        divModels.style.display = "block";


                //    },
                //    error: function () {
                //        alert("There was a problem with Models");
                //    }
                //});
            },
            error: function () {
                alert("There was a problem with adding Device");
            }
        });

        $("#deviceModal").modal('hide');
    });

    $("#btnClosedeviceModal").click(function () {
        $("#deviceModal").modal('hide');
    });
});

//Logic for the adding a model with a modal
$(document).ready(function () {
    $("#btnAddmodel").click(function () {
        $("#modelModal").modal('show');
    });

    $("#btnSubmitmodelModal").click(function () {
        //Getting elements and their values
        var divModels = document.getElementById("divModels");
        var modelDeviceInput = document.getElementById("Model_Device_ID");
        var slDevices = document.getElementById("slDevices");
        var currentlySelectedDeviceValue = slDevices.options[slDevices.selectedIndex].value;
        var DeviceNameModel = slDevices.options[slDevices.selectedIndex].innerText;
        modelDeviceInput.value = currentlySelectedDeviceValue;
        var modelNameInput = document.getElementById("modelName");


        var DeviceID = $("#slDevices option:selected").val();


        $.ajax({
            url: "/Base/AddModel",
            type: 'POST',
            data: { Name: modelNameInput.value, id: DeviceID },
            success: function () {

                $.ajax({
                    type: "POST",
                    url: "/Base/getModelsByDeviceID",
                    data: { id: DeviceID },
                    success: function (data) {
                        var s = '<option selected disabled hidden>Please Select a Model</option>';
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].modelName === modelNameInput.value) {
                                s += '<option value="' + data[i].modelID + '"selected>' + data[i].modelName + '</option>';
                            } else {
                                s += '<option value="' + data[i].modelID + '">' + data[i].modelName + '</option>';
                            }
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
                        alert("There was a problem with getting Models");
                    }
                });
                $("#modelModal").modal('hide');
            },
            error: function () {
                alert("There was a problem with adding Model");
            }

        });

        $("#btnClosemodelModal").click(function () {
            $("#modelModal").modal('hide');
        });

    });
});

//Logic for the adding a state with a modal
$(document).ready(function () {
    $("#btnAddstate").click(function () {
        $("#stateModal").modal('show');
    });

    $("#btnSubmitstateModal").click(function () {
        //Getting elements and their values

        var StateNameInput = document.getElementById("stateName");
        var slStates = document.getElementById("slStates");

        //Sending the created model to the DAL
        $.ajax({
            url: "/Base/AddState",
            type: 'POST',
            data: { name: StateNameInput.value },
            success: function () {
                
                $.ajax({
                    type: "GET",
                    url: "/Base/getAllStates",
                    data: "{}",
                    success: function (data) {
                        var StateOptions = '<option selected disabled hidden>Please Select a State</option>';
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].stateName === StateNameInput.value) {
                                StateOptions += '<option value="' + data[i].stateID + '"selected>' + data[i].stateName + '</option>';
                            } else {
                                StateOptions += '<option value="' + data[i].stateID + '">' + data[i].stateName + '</option>';
                            }
                        }
                        
                        //Fills slStates then unhides divModels
                        $("#slStates").html(StateOptions);
                    }
                });
                $("#stateModal").modal('hide');
            },
            error: function () {
                alert("There was a problem with States");
            }
        });

        $("#btnClosestateModal").click(function () {
            $("#stateModal").modal('hide');
        });
    });
});

//Logic for the adding a location with a modal
$(document).ready(function () {
    $("#btnAddlocation").click(function () {
        $("#locationModal").modal('show');
    });

    $("#btnSubmitlocationModal").click(function () {
        //Getting elements and their values
        var LocationNameInput = document.getElementById("locationName");
        var slLocations = document.getElementById("slLocations");

        var newOption = document.createElement("option");
        var numberOfOptions = slLocations.options.length;
        newOption.value = numberOfOptions;
        newOption.innerText = LocationNameInput.value;
        slLocations.appendChild(newOption);
        slLocations.selectedIndex = newOption.value;

        //Creating a model based off of user inputs
        var LocationModel = {
            Name: LocationNameInput.value
        };

        //Sending the created model to the DAL
        $.ajax({
            url: "/BaseController/AddLocation",
            type: 'POST',
            data: LocationModel
        });
        $("#locationModal").modal('hide');
    });

    $("#btnCloselocationModal").click(function () {
        $("#locationModal").modal('hide');
    });
});
