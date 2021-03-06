﻿var upload = new FileUploadWithPreview("cover-img", {
    text: {
        chooseFile: "",
        browse: "Chọn ảnh đại diện"
    },
});
var upload1 = new FileUploadWithPreview("thumb-img", {
    maxFileCount: 20,
    text: {
        chooseFile: "",
        browse: "Ảnh mô tả"
    },
});

function onSelectMain() {
    
    var mainImg = $('#mainimagedemand').val();
    if (mainImg.files != "") {
        var files = $('#mainimagedemand')[0].files;
        var type = files[0].name.split('.').pop()
        var acceptedFiles = ["jpg", "JPG", "jpeg", "JPEG", "png", "PNG"]
        var isAcceptedImageFormat = ($.inArray(type, acceptedFiles)) != -1
        if (!isAcceptedImageFormat) {
            toastr["error"]("Ảnh chỉ chấp nhận các định dạng JPG, JPEG, PNG");
            return false;
        }
    }
    
}

function onSelectSub() {
    var files2 = document.getElementById("subimagedemand");
    for (i = 0; i < files2.files.length; i++) {
        var type = files2.files[i].name.split('.').pop();
        var acceptedFiles = ["jpg", "JPG", "jpeg", "JPEG", "png", "PNG"]
        var isAcceptedImageFormat = ($.inArray(type, acceptedFiles)) != -1
        if (!isAcceptedImageFormat) {
            toastr["error"]("Ảnh chỉ chấp nhận các định dạng JPG, JPEG, PNG");
            return false;
        }
    }

    if (files2.files.length > 20) {
        toastr.error("Quá 20 ảnh cho phép.");
        return false;
    }
}