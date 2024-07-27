
var fbid;

(function () {

    if (sessionStorage.getItem("islogin") === undefined || sessionStorage.getItem("islogin") === null) {
        window.location.href = BASE_URL + 'LandingPage/Index';
        return false;
    }

    fbid = $.session.get('fbid');
    var type = "myscore";

    $.get(PROFILE + "?fbid=" + fbid, function (res) {
        var user = res.result.FbInfo.liveUserInfoList[0];
        $("#coin").text(user.Coin);
        $("#winlose").text(user.Win + "/" + user.Lose);
        $("#life").text(user.Life);
    });

    $.get(MY_SCORE + "?fbid=" + fbid + "&type=" + type, function (res) {

        var i = 0;

        if (res.result.length == 0) {
            $("#parrent_layout").hide();
        } else {
            document.getElementById("parrent_layout").style.visibility = "visible"; 
            res.result.forEach(function (item) {
                i += 1;

                $("#layout").append(' <div class="row mb-2 myscore-layout">' +
                    '<div class="col-1" style = "margin-top:5px;"> <span>' + i + '</span></div >' +
                    '<div class="col-1"><img src="' + item.FbImageUrl + '"height="30" width="30" style="border-radius: 50px;" /></div>' +
                    '<div class="col-5" style="margin-top:5px;"><span>' + item.FbName + '</span></div>' +
                    ' <div class="col-2" style="margin-top:5px;"><span> ' + item.Totalamount + ' </span></div>' +
                    '<div class="col-2" style="margin-top:5px;"><span>পয়েন্ট</span></div>' +
                    '</div >');
            }); 


        }

        
    });


})();

function claimCoin() {
    var coin = $("#coin").text();
    $.get(CLAIM_COIN + "?fbid=" + fbid + "&coin=" + coin, function (res) {
        $("#m_cl").modal('show');
    });
}