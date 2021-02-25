
function GetVideoDataPagging(currentPage, pageSize) {
    $.ajax({
        type: "GET",
        url: "/Video/GetPaggingVideo?page="+currentPage + "&pageSize=" + pageSize,
        dataType: "html",
        success: function (response) {
            $('#data-video').html(response);
        },
        failure: function (response) {
            
        },
        error: function (response) {
            
        }
    })
}
function OnChangePageSize( pageSize) {
    $.ajax({
        type: "GET",
        url: "/Video/GetPaggingVideo?page=" + '1' + "&pageSize=" + pageSize,
        dataType: "html",
        success: function (response) {
            $('#data-video').html(response);
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    })
}


window.onload = function () {
    $.ajax({
        type: "GET",
        url: "/Video/GetPaggingVideo?page=" + currentPage + "&pageSize=" + pageSize,
        dataType: "html",
        success: function (response) {
            $('#data-video').html(response);
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });
}
