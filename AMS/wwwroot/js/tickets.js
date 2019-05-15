// ----------------------------------------------------------------------------------------------------------------------
// Add New Note
// Used in the ticket details view
// ----------------------------------------------------------------------------------------------------------------------
var openNote = false;

//This adds a new row to the top of the notes table in the tickets details page when the "Add New Note" button is pressed
var newButton = document.getElementById("btnNewNote");
if (typeof newButton !== "undefined" && newButton !== null) {
    newButton.addEventListener("click", function (e) {

        if (openNote !== true) {
            openNote = true;
            var table = document.getElementById("tblNotes");
            var tbody = table.tbody;
            var tr = table.insertRow(0);

            var currentDate = getCurrentDateTime();

            var td1 = tr.insertCell(0);
            td1.innerHTML = currentDate;
            addClass(td1, "tdDateCreated");

            var name = document.getElementById("userName").value;

            var td2 = tr.insertCell(1);
            td2.innerHTML = name;
            addClass(td2, "tdUserFirstName");

            var td3 = tr.insertCell(2);
            td3.innerHTML = '<textarea name="Description" id="newNote" class="newNote"></textarea><tr><td><input type="button" value="Submit" id="noteSubmit"/></td></tr>';
            addClass(td3, "tdNote");

            document.getElementById("newNote").focus();

            // add an event listener to the text area that was created
            // submits the note when the note is blurred 
            document.getElementById("noteSubmit").addEventListener("click", function (e) {
                document.getElementById("newNote").blur();
                openNote = false;
                var form = document.getElementsByTagName("form")[0];
                form.submit();

            }, false);

        } else {
            alert("A note is still open");
        }
    }, false);
}

//Adds a new class to an element
//Parameters: element and a string for the class name
function addClass(element, name) {
    var arr = element.className.split(" ");
    if (arr.indexOf(name) === -1) {
        element.className += " " + name;
    }
}

// Returns the current date: "mm/dd/yyyy hh:mm:ss AM/PM"
function getCurrentDateTime() {
    var today = new Date();
    var date = (today.getMonth() + 1) + '/' + today.getDate() + '/' + today.getFullYear();
    var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
    var AMorPM;

    if (today.getHours() < 12) {
        AMorPM = "AM";
    } else {
        AMorPM = "PM";
    }


    var dateTime = date + ' ' + time + ' ' + AMorPM;

    return dateTime;
}


var assetsToRemove;
var assetsToAdd;
AssetsToRemoveElement = document.getElementById("AssetsToRemove");
AssetsToAddElement = document.getElementById("AssetsToAdd");


//Adds an event listener to the Assets Table in the edit tickets page.
//When pressed it hides the row of the asset, and it adds the id to the list of
// assets to remove.
var delButton = document.getElementById("AssetTable");
if (typeof delButton !== "undefined" && delButton !== null) {
    delButton.addEventListener("click", function (e) {
        e = e || window.event;
        t = e.target || e.srcElement;
        e.stopPropagation();
        if (t.defaultValue === "Remove") {
            row = t.parentNode.parentNode;

            row.style.display = "none";

            if (assetsToRemove === undefined) {
                assetsToRemove = t.id;
            } else {
                assetsToRemove += "," + t.id;
            }

            AssetsToRemoveElement.value = assetsToRemove;
        }
    }, false);
}

// ----------------------------------------------------------------------------------------------------------------------
// end of Remove Asset From Ticket
// ----------------------------------------------------------------------------------------------------------------------