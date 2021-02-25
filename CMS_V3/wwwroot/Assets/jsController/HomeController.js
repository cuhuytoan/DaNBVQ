function getHomeCarousel() {
    $.ajax({
        type: "GET",
        url: "/Home/GetTopProductBrand",
        dataType: "html",
        cache: true,
        success: function (response) {
            $('#body_section_top_product_brand').html(response);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function loadData() {
    $.ajax({
        url: '/Service/LoadDataByCateAsync',
        type: 'GET',
        data: {
            page: 1,
            pageSize: 15
        },
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                var data = response.data;
                var html = '';
                var template = $('#data-template').html();
               
                $('#tblData').html(html);
                paging(response.total, function () {
                    loadData();
                });
            }
        }
    })
}

function paging(totalRow, callback) {
    var totalPage = Math.ceil(totalRow / homeconfig.pageSize);
    $('#pagination').twbsPagination({
        totalPages: totalPage,
        first: "Đầu",
        next: "Tiếp",
        last: "Cuối",
        prev: "Trước",
        visiblePages: 10,
        onPageClick: function (event, page) {
            homeconfig.pageIndex = page;
            setTimeout(callback, 200);
        }
    });
}

