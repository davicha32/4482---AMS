// clone tfoot like thead
$("#editItems").append(
    $('<tfoot/>').append($("#editItems thead tr").clone())
);

// Setup - add a text input to each footer cell
$('#editItems tfoot th').each(function () {
    // var title = $(this).text().trim();
    $(this).html('<input type="text" class="colSearch" style="width:7em" placeholder="Search" />');
});

// Create DataTable
var table = $("#editItems").DataTable({
    colReorder: false
});

if ($(".dataTables_empty").length) {
    $("#editItems tbody").hide();
}

// Move column search fields to top
$('#editItems tfoot tr').appendTo('#editItems thead');


$("#editItems").click(function (e) {
    e = e || window.event;
    t = e.target || e.srcElement;
    var obj = $(this).data("object");
    var act = $(this).data("action");
    if (t.tagName === "TD") {
        var par = $(t).parent();
        //alert(par.data("id"));
        var addy = obj + "/" + act + "/" + par.data("id");
        window.location.href = addy;
    }
});


$("#editItem").click(function (e) {
    e = e || window.event;
    t = e.target || e.srcElement;
    var obj = $(this).data("object");
    var act = $(this).data("action");
    if (t.tagName === "TD") {
        var par = $(t).parent();
        //alert(par.data("id"));
        var addy = "../../" + obj + "/" + act + "/" + par.data("id");
        window.location.href = addy;
    }
});

// Apply the search
table.columns().every(function () {
    var that = this;

    $('input', this.footer()).on('keyup change', function () {
        if (that.search() !== this.value) {
            that
                .search(this.value)
                .draw();
        }
    });
});