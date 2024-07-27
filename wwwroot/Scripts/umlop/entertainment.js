(function () {

    if (sessionStorage.getItem("islogin") === undefined || sessionStorage.getItem("islogin") === null) {
        window.location.href = BASE_URL + 'LandingPage/Index';
        return;
    }

    var CatCode = "NV";
    var page = 1;

    $.get(ENTERTAINMENT + "?CatCode=" + CatCode + "&PageTotal=" + page, function (res) {

        if (res.length == 0) {
            $("#parrent_layout").hide();
        } else {
            document.getElementById("parrent_layout").style.visibility = "visible";
            res.forEach(function (item) {
                $("#layout").append('<div class="row bg-white" style="border-radius:10px; margin-bottom:5px;" onclick="loadVideo(\'https://wap.shabox.mobi/CMS/Assets/video/' + item.PhysicalFileName + '.mp4\')"><div class= "col-4 m-0 p-0">' +
                    '<img class= "entertainment" src = "' + item.BigPreview + '" /> </div><div class="col-8" style="font-size:13px; padding-top:8px;">' + item.ContentTitle + '</div></div >');
            });
        }
    });

    $("#layout").bind('scroll', function () {
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight-10) {
            page++;
            $.get(ENTERTAINMENT + "?CatCode=" + CatCode + "&PageTotal=" + page, function (res) {
                if (res.length == 0) {
                } else {
                    document.getElementById("parrent_layout").style.visibility = "visible";
                    res.forEach(function (item) {
                        $("#layout").append('<div class="row bg-white" style="border-radius:10px; margin-bottom:5px;" onclick="loadVideo(\'https://wap.shabox.mobi/CMS/Assets/video/' + item.PhysicalFileName + '.mp4\')"><div class= "col-4 m-0 p-0">' +
                            '<img class= "entertainment" src = "' + item.BigPreview + '" /> </div><div class="col-8" style="font-size:13px; padding-top:8px;">' + item.ContentTitle + '</div></div >');
                    });
                }
            });
        }
    });

})();

function loadVideo(vid) {
    $("#vid").attr('src', vid);
}