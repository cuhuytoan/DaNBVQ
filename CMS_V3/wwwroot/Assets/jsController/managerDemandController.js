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
var listLocal = [];
var txtLocalWork = "";
var txtLocalWork2 = "";
var sttx = 0;
$("#LocalWork").change(function () {
    listLocal.push($("#LocalWork").val());
    if (listLocal.length > 3) {
        toastr["error"]("Mong muốn làm việc tại (tối đa 3)!");
        return false;
    } else {
        if (listLocal.length == 1) {
            txtLocalWork = "" + listLocal[0];
        }
        if (listLocal.length == 2) {
            txtLocalWork = "";
            txtLocalWork = "" + listLocal[1];
        }
        if (listLocal.length == 3) {
            txtLocalWork = "";
            txtLocalWork = "" + listLocal[2];
        }
    }
});

//Cần mua thiết bị
async function CreateDemand(e) {
    if ($("#mainimagedemand").val() == "") {
        toastr["error"]("Ảnh đại diện không được để trống!");
        return false;
    }

    if ($("#subimagedemand").val() == "") {
        toastr["error"]("Ảnh mô tả không được để trống!");
        return false;
    }

    var NameProduct = $("#Name").val();
    var productcateId = $("#ProductCategoryId").val();
    var ProductManufactureId = $("#ProductManufactureId").val();
    var SaleLocationId = $("#SaleLocationId").val();
    var ProductCategoryIdParent = $("#ProductCategoryIdParent").val();

    if (NameProduct == "") {
        toastr["error"]("Tiêu đề bài đăng không được để trống");
        $("#Name").focus();
        return false;
    }
    if (checkSpecialCharacter_New(NameProduct) == false) {
        toastr["error"](validationMessage.Name_Specal);
        $("#Name").focus();
        return false;
    }
    if (NameProduct.length > 70) {
        toastr["error"]("Tiêu đề không được quá 70 ký tự");
        $("#Name").focus();
        return false;
    }

    if (ProductCategoryIdParent < 1) {
        toastr["error"]("Chủng loại máy không được để trống");
        $("#ProductCategoryId");
        return false;
    }
    if (isNaN(ProductCategoryIdParent)) {
        toastr["error"]("Chủng loại máy không tồn tại");
        $("#ProductCategoryId")
        return false;
    }

    if (productcateId < 1) {
        toastr["error"]("Loại máy không được để trống");
        $("#ProductCategoryId");
        return false;
    }
    if (isNaN(productcateId)) {
        toastr["error"]("Loại máy không tồn tại");
        $("#ProductCategoryId")
        return false;
    }

    if (ProductManufactureId < 1) {
        toastr["error"]("Hãng sản xuất không được để trống");
        $("#ProductManufactureId").focus();
        return false;
    }
    if (isNaN(ProductManufactureId)) {
        toastr["error"]("Hãng sản xuất không tồn tại");
        $("#ProductManufactureId").focus();
        return false;
    }

    if (SaleLocationId < 1) {
        toastr["error"]("Địa chỉ không được để trống");
        $("#SaleLocationId").focus();
        return false;
    }
    if (isNaN(SaleLocationId)) {
        toastr["error"]("Địa chỉ không tồn tại");
        $("#SaleLocationId").focus();
        return false;
    }

    var content = $("#content_val").val();

    if (content == "") {
        toastr["error"]("Nội dung chi tiết không để trống");
        $("#content_val").focus();
        return false;
    }

    if (content.length < 10) {
        toastr["error"]("Nội dung chi tiết không được nhỏ hơn 10 ký tự");
        $("#content_val").focus();
        return false;
    }

    if (content.length > 4000) {
        toastr["error"]("Nội dung chi tiết không được lớn hơn 4000 ký tự");
        $("#content_val").focus();
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
        "SaleLocationId": SaleLocationId,
        "Content": content
    }
    var model = {
        "Product": objProduct,
        "MainImage": {},
        "SubImageUpload": subImageList,
        "SubImageFileName": subImageNameList,
        "typeForm": 2
    };
    model['MainImage'] = objMainImage;
    document.getElementById("buttonCreate").disabled = true;
    $.ajax({
        type: 'POST',
        url: "/ManagerDemand/CreateDemand",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            toastr["success"](data);
            toastr.options.fadeOut = 2000;
            setTimeout(
                function () {
                    window.location.href = "/tai-khoan?id=2";
                },
                2000);
        }
    });
    e = e || window.event;
    e.preventDefault();
}

//Cần mua vật tư
async function CreateMaterial() {
    if ($("#mainimagedemand").val() == "") {
        toastr["error"]("Ảnh đại diện không được để trống!");
        return false;
    }

    if ($("#subimagedemand").val() == "") {
        toastr["error"]("Ảnh mô tả không được để trống!");
        return false;
    }

    var NameProduct = $("#Name").val();
    var productcateId = $("#ProductCategoryId").val();

    if (NameProduct == "") {
        toastr["error"]("Tiêu đề bài đăng không được để trống");
        $("#Name").focus();
        return false;
    }
    if (checkSpecialCharacter_New(NameProduct) == false) {
        toastr["error"](validationMessage.Name_Specal);
        $("#Name").focus();
        return false;
    }
    if (NameProduct.length > 100) {
        toastr["error"]("Tiêu đề không được quá 100 ký tự");
        $("#Name").focus();
        return false;
    }

    if (productcateId < 1) {
        toastr["error"]("Vật tư cần mua không được để trống");
        $("#ProductCategoryId").focus();
        return false;
    }
    if (isNaN(productcateId)) {
        toastr["error"]("Vật tư cần mua không tồn tại");
        $("#ProductCategoryId").focus();
        return false;
    }

    var content = $("#content_val").val();

    if (content == "") {
        toastr["error"]("Nội dung chi tiết không để trống");
        $("#content_val").focus();
        return false;
    }

    if (content.length < 10) {
        toastr["error"]("Nội dung chi tiết không được nhỏ hơn 10 ký tự");
        $("#content_val").focus();
        return false;
    }

    if (content.length > 4000) {
        toastr["error"]("Nội dung chi tiết không được lớn hơn 4000 ký tự");
        $("#content_val").focus();
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
        "Content": content
    }
    var model = {
        "Product": objProduct,
        "MainImage": {},
        "SubImageUpload": subImageList,
        "SubImageFileName": subImageNameList,
        "typeForm": 8
    };
    model['MainImage'] = objMainImage;
    document.getElementById("buttonCreate").disabled = true;
    $.ajax({
        type: 'POST',
        url: "/ManagerDemand/CreateDemand",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            toastr["success"](data);
            toastr.options.fadeOut = 3000;
            setTimeout(
                function () {
                    window.location.href = "/tai-khoan?id=2";
                },
                2000);
        }
    });

}

//Cần thuê thiết bị
async function CreateEmployDemand() {
    if ($("#mainimagedemand").val() == "") {
        toastr["error"]("Ảnh đại diện không được để trống!");
        return false;
    }

    if ($("#subimagedemand").val() == "") {
        toastr["error"]("Ảnh mô tả không được để trống!");
        return false;
    }

    var NameProduct = $("#Name").val();
    var productcateId = $("#ProductCategoryId").val();
    var ProductManufactureId = $("#ProductManufactureId").val();

    if (NameProduct == "") {
        toastr["error"]("Tiêu đề bài đăng không được để trống");
        $("#Name").focus();
        return false;
    }
    if (checkSpecialCharacter_New(NameProduct) == false) {
        toastr["error"](validationMessage.Name_Specal);
        $("#Name").focus();
        return false;
    }
    if (NameProduct.length > 70) {
        toastr["error"]("Tiêu đề không được quá 70 ký tự");
        $("#Name").focus();
        return false;
    }

    if (productcateId < 1) {
        toastr["error"]("Loại máy không được để trống");
        $("#ProductCategoryId").focus();
        return false;
    }
    if (isNaN(productcateId)) {
        toastr["error"]("Loại máy không tồn tại");
        $("#ProductCategoryId").focus();
        return false;
    }

    if (ProductManufactureId < 1) {
        toastr["error"]("Hãng sản xuất không được để trống");
        $("#ProductManufactureId").focus();
        return false;
    }
    if (isNaN(ProductManufactureId)) {
        toastr["error"]("Hãng sản xuất không tồn tại");
        $("#ProductManufactureId").focus();
        return false;
    }

    var content = $("#content_val").val();

    if (content == "") {
        toastr["error"]("Nội dung chi tiết không để trống");
        $("#content_val").focus();
        return false;
    }

    if (content.length < 10) {
        toastr["error"]("Nội dung chi tiết không được nhỏ hơn 10 ký tự");
        $("#content_val").focus();
        return false;
    }

    if (content.length > 4000) {
        toastr["error"]("Nội dung chi tiết không được lớn hơn 4000 ký tự");
        $("#content_val").focus();
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
        "Content": content
    }
    var model = {
        "Product": objProduct,
        "MainImage": {},
        "SubImageUpload": subImageList,
        "SubImageFileName": subImageNameList,
        "typeForm": 4
    };
    model['MainImage'] = objMainImage;
    document.getElementById("buttonCreate").disabled = true;
    $.ajax({
        type: 'POST',
        url: "/ManagerDemand/CreateDemand",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            toastr["success"](data);
            toastr.options.fadeOut = 2000;
            setTimeout(
                function () {
                    window.location.href = "/tai-khoan?id=2";
                },
                2000);
        }
    });
}

//Cần thuê dịch vụ
async function CreateEmployService() {
    if ($("#mainimagedemand").val() == "") {
        toastr["error"]("Ảnh đại diện không được để trống!");
        return false;
    }

    if ($("#subimagedemand").val() == "") {
        toastr["error"]("Ảnh mô tả không được để trống!");
        return false;
    }

    var NameProduct = $("#Name").val();
    var productcateId = $("#ProductCategoryId").val();
    var ProductCategoryIdParent = $("#ProductCategoryIdParent").val();
    var SaleLocationId = $("#SaleLocationId").val();
    var RelatedCategoryId = $("#RelatedCategoryId").val()

    if (NameProduct == "") {
        toastr["error"]("Tiêu đề bài đăng không được để trống");
        $("#Name").focus();
        return false;
    }
    if (checkSpecialCharacter_New(NameProduct) == false) {
        toastr["error"](validationMessage.Name_Specal);
        $("#Name").focus();
        return false;
    }
    if (NameProduct.length > 100) {
        toastr["error"]("Tiêu đề không được quá 100 ký tự");
        $("#Name").focus();
        return false;
    }

    if (productcateId < 1) {
        toastr["error"]("Loại dịch vụ không được để trống");
        $("#ProductCategoryId").focus();
        return false;
    }
    if (isNaN(productcateId)) {
        toastr["error"]("Loại dịch vụ không tồn tại");
        $("#ProductCategoryId").focus();
        return false;
    }

    if (ProductCategoryIdParent < 1) {
        toastr["error"]("Chủng loại không được để trống");
        $("#ProductCategoryIdParent").focus();
        return false;
    }
    if (isNaN(ProductCategoryIdParent)) {
        toastr["error"]("Chủng loại không tồn tại");
        $("#ProductCategoryIdParent").focus();
        return false;
    }

    var content = $("#content_val").val();

    if (content == "") {
        toastr["error"]("Nội dung chi tiết không để trống");
        $("#content_val").focus();
        return false;
    }

    if (content.length < 10) {
        toastr["error"]("Nội dung chi tiết không được nhỏ hơn 10 ký tự");
        $("#content_val").focus();
        return false;
    }

    if (content.length > 4000) {
        toastr["error"]("Nội dung chi tiết không được lớn hơn 4000 ký tự");
        $("#content_val").focus();
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
        "RelatedCategoryId": RelatedCategoryId,
        "SaleLocationId": SaleLocationId,
        "Content": content
    }
    var model = {
        "Product": objProduct,
        "MainImage": {},
        "SubImageUpload": subImageList,
        "SubImageFileName": subImageNameList,
        "typeForm": 12
    };
    model['MainImage'] = objMainImage;
    document.getElementById("buttonCreate").disabled = true;
    $.ajax({
        type: 'POST',
        url: "/ManagerDemand/CreateDemand",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            toastr["success"](data);
            toastr.options.fadeOut = 2000;
            setTimeout(
                function () {
                    window.location.href = "/tai-khoan?id=2";
                },
                2000);
        }
    });
}


async function CreateRecruitment(e) {
    if ($("#MainImage").val() == "") {
        toastr["error"]("Ảnh đại diện không được để trống!");
        return false;
    }

    if ($("#SubImage").val() == "") {
        toastr["error"]("Ảnh mô tả không được để trống!");
        return false;
    }
    var Name = $("#Name").val();
    var RecruitmentCategoryId = $("#RecruitmentCategoryId").val();
    var RequireOfCV = $("#RequireOfCV").val();
    var Location_ID = $("#Location_ID").val();
    var DeadlineOfCV = $("#DeadlineOfCV").val();
    var Description = $("#Description").val();
    var ProductCateChildRelate = $("#ProductCateChildRelate").val();
    var RequireExp = $("#RequireExp").val();
    var RecruimentNumber = $("#RecruimentNumber").val();
    var PriceFrom = $("#PriceFrom").val();

    var CompanyName = $("#CompanyName").val();
    var ContactAddress = $("#ContactAddress").val();
    var CompanyWebsite = $("#CompanyWebsite").val();
    var ContactName = $("#ContactName").val();
    var ContactEmail = $("#ContactEmail").val();
    var ContactPhone = $("#ContactPhone").val();
    var CompanyBusiness = $("#CompanyBusiness").val();

    if (CompanyName == "") {
        toastr["error"]("Tên công ty không được để trống");
        $("#CompanyName").focus();
        return false;
    }

    if (CompanyName.length > 100) {
        toastr["error"]("Tên công ty không được quá 100 ký tự");
        $("#CompanyName").focus();
        return false;
    }

    if (ContactAddress == "") {
        toastr["error"]("Địa chỉ không được để trống");
        $("#ContactAddress").focus();
        return false;
    }

    if (ContactAddress.length > 100) {
        toastr["error"]("Địa chỉ không được quá 100 ký tự");
        $("#CompanyName").focus();
        return false;
    }

    if (ContactName == "") {
        toastr["error"]("Người liên hệ không được để trống");
        $("#ContactName").focus();
        return false;
    }

    if (ContactName.length > 30) {
        toastr["error"]("Người liên hệ không được quá 30 ký tự");
        $("#ContactName").focus();
        return false;
    }

    if (ContactEmail.length > 30) {
        toastr["error"]("Email không được quá 30 ký tự");
        $("#ContactEmail").focus();
        return false;
    }

    if (ContactPhone == "") {
        toastr["error"]("Số điện thoại không được để trống");
        $("#ContactPhone").focus();
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

    var mainImg = $('#MainImage').val();
    if (mainImg.files != "") {
        var files = $('#MainImage')[0].files;
        objMainImage = await readFileAsDataURL(files[0]);
    }

    var objProduct = {
        "Name": Name,
        "RecruitmentCategoryId": RecruitmentCategoryId,
        "RequireOfCV": RequireOfCV,
        "Location_ID": Location_ID,
        "DeadlineOfCV": DeadlineOfCV,
        "Description": Description,
        "ProductCateChildRelate": ProductCateChildRelate,
        "RequireExp": RequireExp,
        "RecruimentNumber": RecruimentNumber,
        "PriceFrom": PriceFrom,
        "CompanyName": CompanyName,
        "ContactAddress": ContactAddress,
        "CompanyWebsite": CompanyWebsite,
        "ContactName": ContactName,
        "ContactEmail": ContactEmail,
        "ContactPhone": ContactPhone,
        "CompanyBusiness": CompanyBusiness
    }
    var model = {
        "Rec": objProduct,
        "MainImage": {},
        "SubImageUpload": subImageList,
        "SubImageFileName": subImageNameList
    };
    model['MainImage'] = objMainImage;
    document.getElementById("buttonCreate").disabled = true;
    $.ajax({
        type: 'POST',
        url: "/ManagerDemand/CreateRecruitment",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            toastr["success"](data);
            toastr.options.fadeOut = 2000;
            setTimeout(
                function () {
                    window.location.href = "/tai-khoan?id=2";
                },
                2000);
        }
    });
    e = e || window.event;
    e.preventDefault();
}



async function EditRecruitment(e) {
    var RecruitmentId = $("#RecruitmentId").val();
    var Name = $("#Name").val();
    var RecruitmentCategoryId = $("#RecruitmentCategoryId").val();
    var RequireOfCV = $("#RequireOfCV").val();
    var Location_ID = $("#Location_ID").val();
    var DeadlineOfCV = $("#DeadlineOfCV").val();
    var Description = $("#Description").val();
    var ProductCateChildRelate = $("#ProductCateChildRelate").val();
    var RequireExp = $("#RequireExp").val();
    var RecruimentNumber = $("#RecruimentNumber").val();
    var PriceFrom = $("#PriceFrom").val();

    var CompanyName = $("#CompanyName").val();
    var ContactAddress = $("#ContactAddress").val();
    var CompanyWebsite = $("#CompanyWebsite").val();
    var ContactName = $("#ContactName").val();
    var ContactEmail = $("#ContactEmail").val();
    var ContactPhone = $("#ContactPhone").val();
    var CompanyBusiness = $("#CompanyBusiness").val();

    if (CompanyName == "") {
        toastr["error"]("Tên công ty không được để trống");
        $("#CompanyName").focus();
        return false;
    }

    if (ContactAddress == "") {
        toastr["error"]("Địa chỉ không được để trống");
        $("#ContactAddress").focus();
        return false;
    }

    if (ContactName == "") {
        toastr["error"]("Người liên hệ không được để trống");
        $("#ContactName").focus();
        return false;
    }

    if (ContactPhone == "") {
        toastr["error"]("Số điện thoại không được để trống");
        $("#ContactPhone").focus();
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

    var mainImg = $('#MainImage').val();
    if (mainImg != "") {
        var files = $('#MainImage')[0].files;
        objMainImage = await readFileAsDataURL(files[0]);
    }

    var objProduct = {
        "RecruitmentId": RecruitmentId,
        "Name": Name,
        "RecruitmentCategoryId": RecruitmentCategoryId,
        "RequireOfCV": RequireOfCV,
        "Location_ID": Location_ID,
        "DeadlineOfCV": DeadlineOfCV,
        "Description": Description,
        "ProductCateChildRelate": ProductCateChildRelate,
        "RequireExp": RequireExp,
        "RecruimentNumber": RecruimentNumber,
        "PriceFrom": PriceFrom,
        "CompanyName": CompanyName,
        "ContactAddress": ContactAddress,
        "CompanyWebsite": CompanyWebsite,
        "ContactName": ContactName,
        "ContactEmail": ContactEmail,
        "ContactPhone": ContactPhone,
        "CompanyBusiness": CompanyBusiness
    }
    var model = {
        "Rec": objProduct,
        "MainImage": {},
        "SubImageUpload": subImageList,
        "SubImageFileName": subImageNameList
    };
    model['MainImage'] = objMainImage;
    document.getElementById("buttonCreate").disabled = true;
    $.ajax({
        type: 'POST',
        url: "/ManagerDemand/EditRecruitment",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            toastr["success"](data.Message);
            toastr.options.fadeOut = 2000;
            setTimeout(
                function () {
                    window.location.href = "/tai-khoan?id=2";
                },
                2000);
        }
    });
    e = e || window.event;
    e.preventDefault();
}


async function CreateCV(e) {
    if ($("#mainimagedemand").val() == "") {
        toastr["error"]("Ảnh đại diện không được để trống!");
        return false;
    }

    if ($("#subimagedemand").val() == "") {
        toastr["error"]("Ảnh mô tả không được để trống!");
        return false;
    }
    var FullName = $("#FullName2").val();
    var Phone = $("#Phone1").val();
    var DOB = $("#DOB").val();
    var Gender = document.getElementById("Gender2").value;
    var HomeTown = $("#HomeTown").val();
    var CareerCategoryId = $("#CareerCategoryId").val();
    var Certificate = $("#Certificate").val();
    var ProductCateRelate = $("#ProductCateRelate").val();
    var YearsOfExperience = $("#YearsOfExperience").val();
    var Salary = $("#Salary").val();

    var LocalWork = $("#LocalWork").val();
    var IntroInfomation = $("#IntroInfomation").val();
    if (FullName == "") {
        toastr["error"]("Họ và tên không được để trống");
        $("#FullName").focus();
        return false;
    }

    if (DOB == "") {
        toastr["error"]("Ngày sinh không được để trống");
        $("#DOB").focus();
        return false;
    }

    if (Gender == "") {
        toastr["error"]("Giới tính không được để trống");
        $("#Gender").focus();
        return false;
    }

    if (HomeTown == "") {
        toastr["error"]("Địa chỉ không được để trống");
        $("#HomeTown").focus();
        return false;
    }

    if (Phone == "") {
        toastr["error"]("Số điện thoại không được để trống");
        $("#Phone1").focus();
        return false;
    }

    if (CareerCategoryId < 1) {
        toastr["error"]("Ngành nghề không được để trống");
        $("#CareerCategoryId").focus();
        return false;
    }


    if (LocalWork == "") {
        toastr["error"]("Mong muốn làm việc tại không được để trống");
        $("#LocalWork").focus();
        return false;
    }

    

    if (IntroInfomation == "") {
        toastr["error"]("Giới thiệu bản thân không được để trống");
        $("#content_val").focus();
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
        "FullName": FullName,
        "DOB": DOB,
        "Gender": Gender,
        "HomeTown": HomeTown,
        "Phone": Phone,
        "CareerCategoryId": CareerCategoryId,
        "Certificate": Certificate,
        "ProductCateRelate": ProductCateRelate,
        "YearsOfExperience": YearsOfExperience,
        "Salary": Salary,
        "LocalWork": txtLocalWork,
        "IntroInfomation": IntroInfomation
    }
    var model = {
        "CV": objProduct,
        "MainImage": {},
        "SubImageUpload": subImageList,
        "SubImageFileName": subImageNameList
    };
    model['MainImage'] = objMainImage;
    document.getElementById("buttonCreate").disabled = true;
    $.ajax({
        type: 'POST',
        url: "/ManagerDemand/CreateCV",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            toastr["success"](data);
            toastr.options.fadeOut = 2000;
            setTimeout(
                function () {
                    window.location.href = "/tai-khoan?id=2";
                },
                2000);
        }
    });
    e = e || window.event;
    e.preventDefault();
}



async function EditCV(e) {
    var Id = $("#Id").val();
    var FullName = $("#FullName2").val();
    var DOB = $("#DOB").val();
    var Gender = document.getElementById("Gender2").value;
    var HomeTown = $("#HomeTown").val();
    var Phone = $("#Phone1").val();
    var CareerCategoryId = $("#CareerCategoryId").val();
    var Certificate = $("#Certificate").val();
    var ProductCateRelate = $("#ProductCateRelate").val();
    var YearsOfExperience = $("#YearsOfExperience").val();
    var Salary = $("#Salary").val();

    var LocalWork = $("#LocalWork").val();
    var IntroInfomation = $("#content_val").val();
    if (FullName == "") {
        toastr["error"]("Họ và tên không được để trống");
        $("#FullName").focus();
        return false;
    }

    if (DOB == "") {
        toastr["error"]("Ngày sinh không được để trống");
        $("#DOB").focus();
        return false;
    }

    if (Gender == "") {
        toastr["error"]("Giới tính không được để trống");
        $("#Gender").focus();
        return false;
    }

    if (HomeTown == "") {
        toastr["error"]("Địa chỉ không được để trống");
        $("#HomeTown").focus();
        return false;
    }

    if (Phone == "") {
        toastr["error"]("Số điện thoại không được để trống");
        $("#Phone1").focus();
        return false;
    }

    if (CareerCategoryId < 1) {
        toastr["error"]("Ngành nghề không được để trống");
        $("#CareerCategoryId").focus();
        return false;
    }


    if (LocalWork == "") {
        toastr["error"]("Mong muốn làm việc tại không được để trống");
        $("#LocalWork").focus();
        return false;
    }

    if (IntroInfomation == "") {
        toastr["error"]("Giới thiệu bản thân không được để trống");
        $("#content_val").focus();
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

    var mainImg = $('#MainImage').val();
    if (mainImg != "") {
        var files = $('#MainImage')[0].files;
        objMainImage = await readFileAsDataURL(files[0]);
    }

    var objProduct = {
        "Id": Id,
        "FullName": FullName,
        "DOB": DOB,
        "Gender": Gender,
        "HomeTown": HomeTown,
        "Phone": Phone,
        "CareerCategoryId": CareerCategoryId,
        "Certificate": Certificate,
        "ProductCateRelate": ProductCateRelate,
        "YearsOfExperience": YearsOfExperience,
        "Salary": Salary,
        "LocalWork": LocalWork,
        "IntroInfomation": IntroInfomation
    }
    var model = {
        "CV": objProduct,
        "MainImage": {},
        "SubImageUpload": subImageList,
        "SubImageFileName": subImageNameList
    };
    model['MainImage'] = objMainImage;
    document.getElementById("buttonCreate").disabled = true;
    $.ajax({
        type: 'POST',
        url: "/ManagerDemand/EditCV",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            toastr["success"](data.Message);
            toastr.options.fadeOut = 2000;
            setTimeout(
                function () {
                    window.location.href = "/tai-khoan?id=2";
                },
                2000);
        }
    });
}
