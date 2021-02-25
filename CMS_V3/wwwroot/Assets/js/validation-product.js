var validationMessage = {
    ProductMainImage_Empty: "Ảnh đại diện không được để trống",
    ProductIllustration_Empty: "Ảnh chi tiết không được để trống",
    ProductIllustration_Max: "Quá giới hạn cho phép 20 ảnh",
    ProductCategory_Empty: "Chủng loại không được để trống",
    Name_Empty: "Tiêu đề không được để trống",
    Name_MaxLength: "Tiêu đề không được để trống",
    Name_Specal: "Tiêu đề không được nhập ký tự đặc biệt",
    ProductManufacture_Empty: "Hãng sản xuất không được để trống",
    ProductModel_Empty: "Model không được để trống",
    ProductModel_Duplicate: "Model đã tồn tại",
    ProductModel_Add_Failed: "Thêm model không thành công",
    ProductModel_Add_Successed: "Thêm model thành công",
    Country_Empty: "Xuất xứ không được để trống",
    Price_MaxLength: "Giá chỉ được nhập tối đa 15 ký tự",
    StatusMachine_Empty: "Tình trạng không được để trống",
    YearManufacture_Empty: "Năm sản xuất không được để trống",
    HoursOfWork_MaxLength: "Số giờ hoạt động / số km đã đi phải ít hơn 10 ký tự",
    Content_MinLength: "Bạn phải nhập nội dung tối thiểu 10 ký tự trở lên",
    Content_MaxLength: "Nội dung quá giới hạn 2000 ký tự",
    SerialNumber_MaxLength: "Bạn phải nhập serial ít hơn 20 ký tự",
    RequiredInput: "Chưa nhập dữ liệu",
    Input_Invalid: "Dữ liệu không hợp lệ hoặc chứa các ký tự đặc biệt",
    Permission_Invalid: "Bạn không đủ quyền",
    MaterialName_Empty: "Tên vật tư không được để trống",
    MaterialName_MaxLength: "Tên vật tư quá 100 ký tự cho phép",
    Label_Maxlength: "Nhãn hiệu vượt quá 20 ký tự cho phép",
    SerialNumber_MaxLength: "Ký hiệu vượt quá 20 ký tự cho phép",
    PartNumber_MaxLength: "Số hiệu/Part-Number vượt quá 20 ký tự cho phép",
    MaterialManufactureName_Empty: "Hãng sản xuất không được để trống",
    MaterialManufactureName_MaxLength: "Hãng sản xuất vượt quá 50 ký tự cho phép",
    AccessoriesName_Empty: "Tiêu đề bài đăng không được để trống",
    AccessoriesName_MaxLength: "Tiêu đề bài đăng không được vượt quá 70 ký tự",
    Serial_Special: "Số Serial/ Part-number không nhập kí tự có dấu hoặc ký tự đặc biệt",

};

function checkSpecialCharacter(data) {
    var check = true;
    console.log("data:" + data);
    var iChars = "!#$%^&*+=[]\\\';/{}|\":<>?~_";
    for (var i = 0; i < data.length; i++) {
        console.log("dataitem:" + data.charAt(i));
        if (iChars.indexOf(data.charAt(i)) != -1) {
            console.log("Your string has special characters. \n These are not allowed.");
            check = false;
            break;
        }
    }
    return check;
}

function checkSpecialCharacter_New(data) {
    var check = true;
    console.log("data:" + data);
    var iChars = "~`!@#$^&*_+={}[];:'\"|<>";

    for (var i = 0; i < data.length; i++) {
        console.log("dataitem:" + data.charAt(i));
        if (iChars.indexOf(data.charAt(i)) != -1) {
            console.log("Your string has special characters. \n These are not allowed.");
            check = false;
            break;
        }
    }
    return check;
}

function validateAddNewModel(modelName) {
    var check = true;
    var modelTrim = modelName.trim();
    if (modelTrim == "") {
        toastr["error"]("Model không được để trống");
        check = false;
    } else if (!checkSpecialCharacter(modelTrim)) {
        toastr["error"]("Sai định dạng");
        check = false;
    }
    else if (modelTrim.length> 20) {
        toastr["error"]("Model nhập quá 20 ký tự cho phép");
        check = false;
    }
    return check;
}