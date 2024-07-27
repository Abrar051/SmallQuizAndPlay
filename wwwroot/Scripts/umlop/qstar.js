var fbid;

$(document).ready(function () {

    if (sessionStorage.getItem("islogin") === undefined || sessionStorage.getItem("islogin") === null) {
        window.location.href = BASE_URL + 'LandingPage/BkashPayment';
        return false;
    }
    $("#btncoin").click(function () {
        $('#modalcoinbuy').modal('show');
    });
    $("#btnlife").click(function () {
        $('#modallifebuy').modal('show');
    });

    fbid = $.session.get("fbid");

    var urlParams = new URLSearchParams(window.location.search);
    var myParam = urlParams.get('spTransID');

    if (myParam != undefined) {
        $.get(bkashCheck + "?spTransID=" + myParam, function (res) {
            if (res.status === "success") {
                var am = parseInt(res.amount);
                var data = { "Tkamount": am, "MSISDN": "", "FBID": fbid, "UsdAmount": "" };

                $.ajax({
                    type: "POST",
                    url: BUY_COIN,
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        $("#quiz_coin_res").modal('show');
                    },
                    error: function (errMsg) {
                    }
                });
            }
        });
    }

    getProf();
    checkdailybonus();

    $("#logout").on('click', function () {
        $.session.remove("fbid");
        $.session.remove("islogin");
        window.location.href = BASE_URL + 'LandingPage/Index';
        return false;
    });

    $("#settings").on('click', function () {
        $("#mod").empty();
        $("#mod").text("সেটিংস");
        $(".modal-body").empty();
        $(".modal-body").append("<small style=\"text-align: center; color: #5E4C52 \">আনসাবস্ক্রাইব করতে নিচের বাটনে ক্লিক করুন</small></br></br><button class=\"btn btn-danger\" onclick=\"unsubscribe()\">আনসাবস্ক্রাইব</button> </br></br>");

        $("#submodal").modal('show');
        closeNav();
    });

});

function unsubscribe() {

    $("#mod").empty();
    $("#mod").text("কনফার্মেশন");
    $(".modal-body").empty();
    $(".modal-body").append("<small style=\"text-align: center; color: #5E4C52 \">আপনি কি শিউর আপনি আনসাবস্ক্রাইব করতে চাচ্ছেন?</small></br></br><button class=\"btn btn-secondary\" onclick=\"closes()\">না</button> <button class=\"btn btn-danger\" onclick=\"ok()\">ওকে</button> </br></br>");

    $("#submodal").modal('show');
}

function closes() {
    $("#submodal").modal('hide');
}

function ok() {

    var fbid = $.session.get('fbid');
    $.get("https://wap.shabox.mobi/apibkash/api/genericsub/cancel?fbid=" + fbid + "&app=quizstar", function (res) {
        closes();
    });
}

function getProf() {
    $.get(PROFILE + "?fbid=" + fbid, function (res) {
        var user = res.result.FbInfo.liveUserInfoList[0];
        $("#img_prof").attr('src', user.FbImageUrl);
        $("#db_img").attr('src', user.FbImageUrl);
        $("#name").text(user.FbName);
        $("#coinlife").text("Coin: " + user.Coin + "     -        Life: " + user.Life);
        $("#winlose").text("Win: " + user.Win + "       -     Lose: " + user.Lose);

        $.session.set('life', user.Life);
    });
}

function payment(amn) {
    var appName = "Quizstar";
    var userMobile = "";
    var other = "";
    var mobile = "";

    var data = { "amount": amn, "userId": fbid, "appName": appName, "userMobile": userMobile, "other": other, "mobile": mobile };

    $.ajax({
        type: "POST",
        url: bkash,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            window.location.href = "../Home/BkashPayment";
            return false;
        },
        error: function (errMsg) {
        }
    });
}

function buyLife(amn) {
    $.ajax({
        type: "GET",
        url: BUY_LIFE + '?fbid=' +fbid + '&coin=' + amn,
        success: function (res) {
            location.reload();
            return false;
        },
        error: function (errMsg) {
        }
    });
}

function subscription(cy) {
    var appName = "Quizstar";
    var userMobile = "";
    var other = "";
    var mobile = "";

    var data = { "cycle": cy, "package": 'quizstar', "reference": fbid };
    $.ajax({
        type: "POST",
        url: bsub,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            window.location.href = "../Home/BkashPayment";
            return false;
        },
        error: function (errMsg) {
        }
    });
}

function checkdailybonus() {

    var number = sessionStorage.getItem("fbid");

    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=Check_Daily_Bonus&srcurl=" + window.location.href, function (rData) { });

    //$.ajax({

    //    url: "../LandingPage/SaveThemeInfo?msisdn=" + number + "&theme=" + "Check_Daily_Bonus", success: function (res) {


    //    }
    //});

    $.get(DAILY_BONUS + "?fbid=" + fbid, function (res) {
        if (res.result === "success") {
            $('#modaldailybonus').modal('show');
        }
    });
}

function openNav() {
    var number = sessionStorage.getItem("fbid");

    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=Open_Nav&srcurl=" + window.location.href, function (rData) { });

    //$.ajax({

    //    url: "../LandingPage/SaveThemeInfo?msisdn=" + number + "&theme=" + "Open_Nav", success: function (res) {


    //    }
    //});
    document.getElementById("mySidenav").style.width = "250px";
}

function closeNav() {
    var number = sessionStorage.getItem("fbid");

    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=Close_Nav&srcurl=" + window.location.href, function (rData) { });

    //$.ajax({

    //    url: "../LandingPage/SaveThemeInfo?msisdn=" + number + "&theme=" + "Close_Nav", success: function (res) {


    //    }
    //});
    document.getElementById("mySidenav").style.width = "0";
}

//Play Daily Quiz
function p_daily_q() {
    var type = "dailyquiz";

    $.get(CHECK_PLAY_STATUS + "?fbid=" + fbid + "&type=" + type, function (res) {
        if (res.statusCode === 1002) {
            $('#m_d_q').modal('show');
            $(".modal-body").empty();
            $(".modal-body").append("<small style=\"text-align: center; \" class=\"deeporange\">নিউ ইয়ার কুইজ খেলতে ৫০ কয়েন ব্যবহার করুন</small>");
        } else if (res.statusCode === 1007) {
            $('#quiz_sub').modal('show');
        }
        else {
            $.session.set('isRegistered', 1);
            window.location.href = BASE_URL + 'LandingPage/PlayBoard';
            return;
        }
    });
}

//Daily Quiz Registration
function confirm_dqrg() {
    var type = "dailyquiz";

    $.get(REGISTER_PLAY_STATUS + "?fbid=" + fbid + "&type=" + type, function (res) {
        if (res.statusCode === 1002) {
            $("#mod").empty();
            $("#mod").text("নোটিফিকেশান");
            $(".modal-body").empty();
            $(".modal-body").append("<small style=\"text-align: center; color: #5E4C52 \">আপনার পর্যাপ্ত পরিমাণ কয়েন নেই। তাই এখনি কয়েন কিনুন বিকাশ অথবা রবি নম্বরের মাধ্যমে। </small></br></br><hr><button class=\"btn btn-primary\" onclick=\"closes()\" style=\"float: right\">ওকে</button> </br>");

            $("#submodal").modal('show');
        } else {
            window.location.href = BASE_URL + 'LandingPage/PlayBoard';
            return;
        }
    });
}

//Play Jhotpot Round
function p_jhotpot() {
    var type = "jhotpot";

    $.get(CHECK_PLAY_STATUS + "?fbid=" + fbid + "&type=" + type, function (res) {
        if (res.statusCode === 1002) {
            $('#m_jh').modal('show');
            $(".modal-body").empty();
            $(".modal-body").append("<small style=\"text-align: center; \" class=\"deeporange\">ঝটপট রাউন্ড খেলতে ৫০ কয়েন ব্যবহার করুন</small>");
        } else if (res.statusCode === 1007) {
            $('#quiz_sub').modal('show');
        }
        else {
            window.location.href = BASE_URL + 'LandingPage/JhotpotRound';
            return;
        }
    });
}

function confirm_j() {
    var type = "jhotpot";

    $.get(REGISTER_PLAY_STATUS + "?fbid=" + fbid + "&type=" + type, function (res) {
        if (res.statusCode === 1002) {
            $("#mod").empty();
            $("#mod").text("নোটিফিকেশান");
            $(".modal-body").empty();
            $(".modal-body").append("<small style=\"text-align: center; color: #5E4C52 \">আপনার পর্যাপ্ত পরিমাণ কয়েন নেই। তাই এখনি কয়েন কিনুন বিকাশ অথবা রবি নম্বরের মাধ্যমে। </small></br></br><hr><button class=\"btn btn-primary\" onclick=\"closes()\" style=\"float: right\">ওকে</button> </br>");

            $("#submodal").modal('show');
        } else {
            window.location.href = BASE_URL + 'LandingPage/JhotpotRound';
            return false;
        }
    });
}

function play() {

    var number = sessionStorage.getItem("fbid");


    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=Play&srcurl=" + window.location.href, function (rData) { });


    //$.ajax({

    //    url: "../LandingPage/SaveThemeInfo?msisdn=" + number + "&theme=" + "Play", success: function (res) {


    //    }
    //});

    $(".modal-body").empty();
    $(".modal-body").append("<small style=\"text-align: center; margin-bottom: 10px\" class=\"deeporange\">চ্যালেঞ্জ কুইজ খেলতে অ্যাপ ডাউনলোড করুন প্লে স্টোর থেকে</small></br><a style=\"float:right\" target=\"_blank\" href=\"https://play.google.com/store/apps/details?id=biz.vumobile.quizstarapps&hl=en&gl=US\"> <img src=\"" + BASE_URL +"Content/themes/img/play_google.png\" height=\"40\" width=\"100\" /></a>");
    $("#mod_challenge").modal('show');
}

function LeaderBoard() {
    var number = sessionStorage.getItem("fbid");

    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=Leader_Board&srcurl=" + window.location.href, function (rData) { });


    //$.ajax({

    //    url: "../LandingPage/SaveThemeInfo?msisdn=" + number + "&theme=" + "Leader_Board", success: function (res) {


    //    }
    //});
    window.location.href = BASE_URL + "LandingPage/LeaderBoard";
}

function Terms() {

    var number = sessionStorage.getItem("fbid");

    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=Terms&srcurl=" + window.location.href, function (rData) { });


    //$.ajax({

    //    url: "../LandingPage/SaveThemeInfo?msisdn=" + number + "&theme=" + "Terms", success: function (res) {


    //    }
    //});
    window.location.href = BASE_URL + "LandingPage/Terms";
}