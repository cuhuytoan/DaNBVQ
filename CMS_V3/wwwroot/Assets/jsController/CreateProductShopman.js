async function readFileAsDataURL(file) {
    let result_base64 = await new Promise((resolve) => {
        let fileReader = new FileReader();
        fileReader.onload = (e) => resolve(fileReader.result);
        fileReader.readAsDataURL(file);
    });
    var dataImage = {
        FileName: file.name,
        ExtensionType: file.type,
        Base64: result_base64
    }
    return dataImage;
}

async function readSubFileAsDataURL(file) {
    let result_base64 = await new Promise((resolve) => {
        let fileReader = new FileReader();
        fileReader.onload = (e) => resolve(fileReader.result);
        fileReader.readAsDataURL(file);
    });
    var dataImage = {
        FileName: file.name,
        ExtensionType: file.type,
        Base64: result_base64
    }
    return dataImage;
}

async function CreateSellingProduct() {
    if ($("#mainimagedemand").val() == "") {
        toastr["error"]("Ảnh đại diện không được để trống!");
        return false;
    }
    if ($("#subimagedemand").val() == "") {
        toastr["error"]("Ảnh mô tả không được để trống!");
        return false;
    }

    //Lấy data
    var NameProduct = $("#Name").val();
    var productcateId = $("#ProductCategoryId").val();
    var ProductManufactureId = $("#ProductManufactureId").val();
    var ProductModelId = $("#ProductModelId").val();
    var SaleLocationId = $("#SaleLocationId").val();
    var StatusMachine = $("#StatusMachine").val();
    var HoursOfWork = $("#HoursOfWork").val();
    var SerialNumber = $("#SerialNumber").val();
    var MadeCountryId = $("#MadeCountryId").val();
    var Price = $("#Price").val();
    var Unit = $("#Unit").val();
    var YearManufacture = $("#YearManufacture").val();
    var HourOfWorkType = $("#HourOfWorkType").val();
    var content = $("#Content").val();

    if (NameProduct == "") {
        toastr["error"]("Tiêu đề bài đăng không được để trống");
        return false;
    }
    if (!checkSpecialCharacter(NameProduct)) {
        toastr["error"]("Tiêu đề bài đăng được nhập các ký tự đặc biệt");
        return false;
    }
    if (NameProduct.length > 70) {
        toastr["error"]("Tiêu đề không được quá 70 ký tự");
        return false;
    }
    
    if (productcateId < 1) {
        toastr["error"]("Loại máy không được để trống");
        return false;
    }
    if (isNaN(productcateId)) {
        toastr["error"]("Loại máy không tồn tại");
        return false;
    }

    if (ProductManufactureId < 1) {
        toastr["error"]("Hãng sản xuất không được để trống");
        return false;
    }
    if (isNaN(ProductManufactureId)) {
        toastr["error"]("Hãng sản xuất không tồn tại");
        return false;
    }

    if (ProductModelId < 1) {
        toastr["error"]("Model không được để trống");
        return false;
    }
    if (isNaN(ProductModelId)) {
        toastr["error"]("Model không tồn tại");
        return false;
    }

    if (SaleLocationId < 1) {
        toastr["error"]("Địa điểm đặt máy không được để trống");
        return false;
    }
    if (isNaN(SaleLocationId)) {
        toastr["error"]("Địa điểm đặt máy không tồn tại");
        return false;
    }

    if (StatusMachine.length < 1) {
        toastr["error"](validationMessage.StatusMachine_Empty);
        return false;
    }
    if (HoursOfWork.length > 10) {
        toastr["error"](validationMessage.HoursOfWork_MaxLength);
        return false;
    }

    if (!checkSpecialCharacter(SerialNumber)) {
        toastr["error"]("Số serial không được nhập các ký tự đặc biệt");
        return false;
    }
    if (SerialNumber == null || SerialNumber.length > 20) {
        toastr["error"](validationMessage.SerialNumber_MaxLength);
        return false;
    }

    if (/^[a-zA-Z0-9- ]*$/.test(SerialNumber) == false) {
        toastr["error"](validationMessage.Serial_Special);
        return false;
    }

    if (content == "") {
        toastr["error"]("Nội dung chi tiết không để trống");
        return false;
    }

    if (content.length < 10) {
        toastr["error"]("Nội dung chi tiết không được nhỏ hơn 10 ký tự");
        return false;
    }

    if (content.length > 4000) {
        toastr["error"]("Nội dung chi tiết không được lớn hơn 4000 ký tự");
        return false;
    }

    var objMainImage = null;
    var subImageList = [];
    $('input[name^="SubImageUpload"]').each(function () {
        subImageList.push($(this).val());
    });

    var subImageNameList = [];
    $('input[name^="SubImageFileName"]').each(function () {
        subImageNameList.push($(this).val());
    });

    var mainImg = $('#mainimagedemand').val();
    if (mainImg.files != "") {
        var files = $('#mainimagedemand')[0].files;
        objMainImage = await readFileAsDataURL(files[0]);
    }

    var objProduct = {
        "ProductCategoryId": productcateId,
        "Name": NameProduct,
        "ProductManufactureId": ProductManufactureId,
        "ProductModelId": ProductModelId,
        "SerialNumber": SerialNumber,
        "MadeCountryId": MadeCountryId,
        "Price": Price,
        "Unit": Unit,
        "StatusMachine": StatusMachine,
        "YearManufacture": YearManufacture,
        "HoursOfWork": HoursOfWork,
        "HourOfWorkType": HourOfWorkType,
        "SaleLocationId": SaleLocationId,
        "Content": content
    }
    var model = {
        "Product": objProduct,
        "MainImage": {},
        "SubImageUpload": subImageList,
        "SubImageFileName": subImageNameList,
        "typeForm": 1
    };
    model['MainImage'] = objMainImage;

    $.ajax({
        type: 'POST',
        url: "/Shopman/Product/CreateProduct",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            toastr["success"](data);
            toastr.options.fadeOut = 4000;
            setTimeout(
                function () {
                    window.location.href = "/Shopman/Product/";
                },
                4000);
        }
    });
}