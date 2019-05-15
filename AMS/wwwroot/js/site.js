$(document).ready(function () {
    var userName = document.getElementById("userName");

    if (document.contains(userName)){
        $.ajax({
            url: "/Base/getCurrentUser",
            type: 'GET',
            data: {},
            success: function (data) {
                userName.innerHTML = data.email;


                $("#userName").click(function (e) {
                    window.location.href = "../../User/Edit/" + data.id;
                });
            }
        });
    }
});