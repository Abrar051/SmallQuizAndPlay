$(document).ready(function () {

    var url = new URL(window.location.href);

    let searchParams = new URLSearchParams(url.search);
    var invoice = searchParams.get('invoiceID');
    $.session.set('invoice', invoice);
    if (invoice != null) {
        $("#invoiceID").val(invoice);
    }
    var fbid = $.session.get("fbid");
    if (fbid != undefined) {
        window.location.href = BASE_URL + 'LandingPage/Index';
        return false;
    }

    $("form#data").submit(function (e) {
        e.preventDefault();
        var formData = new FormData(this);

        $.ajax({
            url: "https://wap.shabox.mobi/QuizPlayNew/api/Other/OtherData",
            type: 'POST',
            data: formData,
            success: function (res) {
                $.session.set("fbid", res.result);
                window.location.href = BASE_URL + 'LandingPage/Index';
                return false;
            },
            cache: false,
            contentType: false,
            processData: false
        });
    });

    $("#resend").on('click', function () {
        var msisdn = $.session.get('msisdn');

        var obj = {
            MSISDN: msisdn,
            hashString: "xyz",
            ServiceName: "QuizStar"
        };

        $.ajax({
            type: "POST",
            data: JSON.stringify(obj),
            url: "https://wap.shabox.mobi/QuizPlayNew/api/SMS/SendOtp",
            contentType: "application/json",
            success: function (res) {
                if (res.result === "Success") {
                    
                    $("#numberview").hide();
                    var otp = document.getElementById('otpview')
                    var otpvisi = otp.style.visibility;
                    otp.style.visibility = otpvisi == "visible" ? 'hidden' : "visible"
                    $("#msisdn").val(msisdn);
                }
            },
            failure: function (errMsg) {
                
            }
        });
    });

    $("#pro").on('click', function () {
        $('#file-input').trigger('click');
    });
});

var timeleft = 60;

function startTimer() {
    var downloadTimer = setInterval(function () {
        if (timeleft <= 0) {
            clearInterval(downloadTimer);
        }
        timeleft -= 1;
        document.querySelector('#time').textContent = timeleft;

    }, 1000);
}

function numberSubmit() {

    var msisdn = $("#number_input").val();
    $.session.set('msisdn', msisdn);

    var obj = {
        MSISDN: msisdn,
        hashString: "xyz",
        ServiceName: "QuizStar"
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(obj),
        url: "https://wap.shabox.mobi/QuizPlayNew/api/SMS/SendOtp",
        contentType: "application/json",
        success: function (res) {
            if (res.result === "Success") {
               
                $("#numberview").hide();
                var otp = document.getElementById('otpview')
                var otpvisi = otp.style.visibility;
                otp.style.visibility = otpvisi == "visible" ? 'hidden' : "visible"
                startTimer();
                $("#msisdn").val(msisdn);
            }

        },
        failure: function (errMsg) {
            
        }
    });
}


function onFileSelected(event) {
    var selectedFile = event.target.files[0];
    var reader = new FileReader();

    var imgtag = document.getElementById("pro");
    imgtag.title = selectedFile.name;

    reader.onload = function (event) {
        imgtag.src = event.target.result;
    };

    reader.readAsDataURL(selectedFile);
}

function otpSubmit() {

    var msisdn = $("#number_input").val();
    var otp = $("#otp_input").val();

    var obj = {
        MSISDN: msisdn,
        PinCode: otp,
        ServiceName: "QuizStar"
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(obj),
        url: "https://wap.shabox.mobi/QuizPlayNew/api/SMS/CheckPin",
        contentType: "application/json",
        success: function (res) {
            
            if (res.result === "Success") {
                $("#otpview").hide();
                var profile = document.getElementById('profileview')
                var otpvisi = profile.style.visibility;
                profile.style.visibility = otpvisi == "visible" ? 'hidden' : "visible"

                $("#msisdn").val(msisdn);
                $.session.set("islogin", 1);
            }
        },
        failure: function (errMsg) {
            
        }
    });
}

function phoneNumberLogin() {
    $("#homeview").hide();

    var node = document.getElementById('numberview')
    var visibility = node.style.visibility;
    node.style.visibility = visibility == "visible" ? 'hidden' : "visible"

}