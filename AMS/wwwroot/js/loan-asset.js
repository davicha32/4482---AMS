
var btnReturnAsset = document.getElementsByClassName("btnReturnAsset")[0];


if (document.contains(btnReturnAsset)) {
    $(".btnReturnAsset").click(function (e) {
        var obj = $(this).data("object");
        var act = $(this).data("action");
        var id = $(this).data("id");
        var addy = "../../" + obj + "/" + act + "/" + id;
        window.location.href = addy;
    });
}

var btnLoanAsset = document.getElementsByClassName("btnLoanAsset")[0];

if (document.contains(btnLoanAsset)) {
    $(".btnLoanAsset").click(function (e) {
        var obj = $(this).data("object");
        var act = $(this).data("action");
        var id = $(this).data("id");
        var addy = "../../" + obj + "/" + act + "/" + id;
        window.location.href = addy;
    });
}


