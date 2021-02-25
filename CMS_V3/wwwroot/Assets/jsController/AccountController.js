

$('#login').on('submit', function (e) {
    e.preventDefault();
    var username = $("#username").val();
    var pwd = $("#pwd").val();
    if (jQuery.trim(username) == "") {
        toastr["error"]("Tài khoản không được nhập toàn bộ là khoảng trắng");
        return false;
    }
    if (jQuery.trim(pwd) == "") {
        toastr["error"]("Mật khẩu không được nhập toàn bộ là khoảng trắng");
        return false;
    }
    if (username == "") {
        toastr["error"]("Tài khoản không được để trống!");
        $("#username").focus();
        return;
    }
    if (pwd == "") {
        toastr["error"]("Mật khẩu không được để trống!");
        $("#pwd").focus();
        return;
    }
    var model = {
        "Email": $("#username").val(),
        "Password": $("#pwd").val()
    };
    $.ajax({
        url: "/Account/Login",
        type: "POST",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        success: function (response) {
            var result = JSON.parse(response);            
            if (result.ErrorCode == "00") {                
                $('#popup').modal('hide');
                window.location.href = "/tai-khoan";
            }
            else {
                toastr["error"](result.Message);
            }
        },
        error: function () {
            console.log('Login Fail!!!');
        }
    });

});




//Register 
$('#registerStep1').on('submit', function (e) {
    e.preventDefault();
    var username = $("#username").val();
    
    if (username == "") {
        toastr["error"]("Số điện thoại không được để trống!");
        $("#username").focus();
        return;
    };
    if (jQuery.trim(username) == "") {
        toastr["error"]("Số điện thoại không được nhập toàn bộ là khoảng trắng");
        return false;
    }
    if (username.length < 10) {
        toastr["error"]("Số điện thoại không đúng định dạng!");
        $("#username").focus();
        return;
    };
    $("#loading").removeClass('hideMe');
    
    var model = {
        "email": username
    }
    $.ajax({
        url: "/Account/Register",
        type: "POST",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        beforeSend: function () {
            $("#loading").removeClass('hideMe');                 
        },
        success: function (response) {
            var result = JSON.parse(response);
            if (result.ErrorCode == "00") {    
                $("#loading").addClass('hideMe');
                $("#modalRegister1").addClass('hideMe');                
                $("#modalRegister2").removeClass('hideMe');                 
            }
            else {
                $("#loading").addClass('hideMe');                
                toastr["error"](result.Message);
            }
        },
        error: function () {
            console.log('Login Fail!!!');
        }
    });
   
});

//Register Step 2 (OTP)
$('#registerStep2').on('submit', function (e) {
    e.preventDefault();
    var username = $("#username").val();
    var verifyCode = $("#verifyCode").val();

    if (verifyCode.length < 6) {
        toastr["error"]("Mã code không hợp lệ");
        $("#verifyCode").focus();
        return;
    };
    
    $.ajax({
        url: "/Account/VerifyOTP?EmailOrPhone=" + username + "&code=" + verifyCode ,
        type: "GET",      
        dataType: "json",
        contentType: "application/json",
        beforeSend: function () {
            $("#loading").removeClass('hideMe');
        },
        success: function (response) {
            var result = JSON.parse(response);
            if (result.ErrorCode == "00") {
                $("#loading").addClass('hideMe');
                $("#modalRegister1").addClass('hideMe');
                $("#modalRegister2").addClass('hideMe');
                $("#modalRegister3").removeClass('hideMe');
            }
            else {
                $("#loading").addClass('hideMe');
                toastr["error"](result.Message);
            }
        },
        error: function () {
            console.log('Verify Fail!!!');
        }
    });

});

//Register Step 3 (Input passsword)
$('#registerStep3').on('submit', function (e) {
    e.preventDefault();
    var username = $("#username").val();
    var pwd = $("#pwd").val();
    var repwd = $("#repwd").val();

    if (pwd.length < 6) {
        toastr["error"]("Mật khẩu phải lớn hơn 6 kí tự");
        $("#pwd").focus();
        return;
    };
    if (pwd !== repwd ) {
        toastr["error"]("Nhập lại mật khẩu không chính xác!");
        $("#repwd").focus();
        return;
    };
    var model = {
        "emailOrPhone": username,
        "newPassword": pwd,
        "confirmPassword": repwd
    }
    $.ajax({
        url: "/Account/SetPassword",
        type: "POST",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        beforeSend: function () {
            $("#loading").removeClass('hideMe');
        },
        success: function (response) {
            var result = JSON.parse(response);
            if (result.ErrorCode == "00") {
                $("#loading").addClass('hideMe');                
                $("#modalRegister2").addClass('hideMe');
                $("#modalRegister3").addClass('hideMe');
                $("#modalLogin").removeClass('hideMe');
                toastr["success"]("Đăng ký tài khoản thành công");
                             
            }
            else {
                toastr["error"](result.Message);
            }
        },
        error: function () {
            console.log('Login Fail!!!');
        }
    });

});

//timeDown
function timedown() {
    var time = 0; /* how long the timer runs for */
    var i = 120;
    var interval = setInterval(function () {
        $('#timedown').text('(' + i + ')');
        document.getElementById('timedown').style.display = 'inline-block';
        if (i == time) {
            document.getElementById('timedown').style.display = 'none';
            document.getElementById('resend').disabled = false;
            clearInterval(interval);
        }
        i -= 1;
    }, 1000);
}
//register resend OTP
function resendOTP() {
    var username = $("#username").val();    
    var model = {
        "emailOrPhone": username
    }
    $.ajax({
        url: "/Account/ResendCode",
        type: "POST",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        beforeSend: function () {
            $("#loading").removeClass('hideMe');
        },
        success: function (response) {
            var result = JSON.parse(response);
            if (result.ErrorCode == "00") {
                toastr["error"]("Đã gửi code xác nhận!");
                document.getElementById('resend').disabled = true;
                timedown();
            }
            else {
                toastr["error"](result.Message);
            }
        },
        error: function () {
            console.log('Login Fail!!!');
        }
    });

}
//Forgot Step 1 (sent OTP)
$('#forgotStep1').on('submit', function (e) {
    e.preventDefault();
    var username = $("#username").val();    

    if (username == "") {
        toastr["error"]("Số điện thoại không được để trống!");
        $("#username").focus();
        return;
    };
    if (jQuery.trim(username) == "") {
        toastr["error"]("Số điện thoại không được nhập toàn bộ là khoảng trắng");
        return false;
    }
    if (username.length < 10) {
        toastr["error"]("Số điện thoại không đúng định dạng!");
        $("#username").focus();
        return;
    };

    var model = {
        "emailOrPhone": username
    }

    $.ajax({
        url: "/Account/ResendCode",
        type: "POST",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        beforeSend: function () {
            $("#loading").removeClass('hideMe');
        },
        success: function (response) {
            var result = JSON.parse(response);
            if (result.ErrorCode == "00") {
                $("#loading").addClass('hideMe');
                $("#modalForgot1").addClass('hideMe');
                $("#modalForgot2").removeClass('hideMe');
            }
            else {
                $("#loading").removeClass('hideMe');
                toastr["error"](result.Message);
            }
        },
        error: function () {
            console.log('Login Fail!!!');
        }
    });

});

//Forgot Step 2 (verify OTP)

$('#forgotStep2').on('submit', function (e) {
    e.preventDefault();
    var username = $("#username").val();
    var verifyCode = $("#verifyCode").val();

    if (verifyCode.length < 6) {
        toastr["error"]("Mã code không hợp lệ");
        $("#username").focus();
        return;
    };

    $.ajax({
        url: "/Account/VerifyOTP?EmailOrPhone=" + username + "&code=" + verifyCode ,
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        beforeSend: function () {
            $("#loading").removeClass('hideMe');
        },
        success: function (response) {
            var result = JSON.parse(response);
            if (result.ErrorCode == "00") {
                $("#loading").addClass('hideMe');
                $("#modalForgot1").addClass('hideMe');
                $("#modalForgot2").addClass('hideMe');
                $("#modalForgot3").removeClass('hideMe');
            }
            else {
                $("#loading").removeClass('hideMe');
                toastr["error"](result.Message);
            }
        },
        error: function () {
            console.log('Verify Fail!!!');
        }
    });

});

//Forgot Step 3 (Input passsword)
$('#forgotStep3').on('submit', function (e) {
    e.preventDefault();
    var username = $("#username").val();
    var pwd = $("#pwd").val();
    var repwd = $("#repwd").val();

    if (pwd.length < 6) {
        toastr["error"]("Mật khẩu phải lớn hơn 6 kí tự");
        $("#pwd").focus();
        return;
    };
    if (pwd !== repwd) {
        toastr["error"]("Nhập lại mật khẩu không chính xác!");
        $("#repwd").focus();
        return;
    };
    var model = {
        "emailOrPhone": username,
        "newPassword": pwd,
        "confirmPassword": repwd
    }
    $.ajax({
        url: "/Account/SetPassword",
        type: "POST",
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json",
        beforeSend: function () {
            $("#loading").removeClass('hideMe');
        },
        success: function (response) {
            var result = JSON.parse(response);
            if (result.ErrorCode == "00") {
                $("#loading").addClass('hideMe');
                $("#modalForgot1").addClass('hideMe');
                $("#modalForgot2").addClass('hideMe');
                $("#modalForgot3").addClass('hideMe');
                $("#modalLogin").removeClass('hideMe');
                toastr["success"]("Cập nhật mật khẩu thành công");                
            }
            else {
                toastr["error"](result.Message);
            }
        },
        error: function () {
            console.log('Set password Fail!!!');
        }
    });

});

