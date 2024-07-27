
(function () {
    if (sessionStorage.getItem("islogin") === undefined || sessionStorage.getItem("islogin") === null) {
        window.location.href = BASE_URL + 'Home/BkashPayment';
        return;
    }

    //var fbid = sessionStorage.getItem("fbid");
    var fbid = "2347033605551885";
    var type = "monthly";
    $.get(LEADERBOARD + "?fbid=" + fbid + "&type=" + type, function (res) {
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