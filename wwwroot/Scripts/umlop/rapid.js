//const { time } = require("modernizr");

var questionArray = [];
var index = 0;
var right = 0;
var wrong = 0;
var flag = false;
var timetaken = 0;
var fbid;

var nameWC = sessionStorage.getItem("Page");
var serviceName = sessionStorage.getItem("serviceName");
/*var nameNQZ = sessionStorage.getItem("nameNQZ");*/

$(document).ready(function () {
    if (window.performance && window.performance.navigation.type === window.performance.navigation.TYPE_BACK_FORWARD) {
        window.location.href = '../';
    }
    else {
        if (sessionStorage.getItem("islogin") === undefined || sessionStorage.getItem("islogin") === null) {
            window.location.href = 'LandingPage/Index';
            return false;
        }



        $("#rt").html(right);
        $("#ng").html(wrong);
        fbid = sessionStorage.getItem('fbid');
        FFPressed = sessionStorage.getItem("FFPressed");
        EQPressed = sessionStorage.getItem("EQPressed");


        $.get("../home/BkashQuizMasterQuizTimeChecking?quiztype=WinTime", function (rdata) {


            if (rdata.response != null) {
                console.log(rdata.response);
                if (new Date().toString().includes("Fri ") == true && FFPressed == 1 && EQPressed == 0) {

                    $.get("../home/SpecialQuizPlayQountChecking?quiztype=FunFriday&msisdn=" + fbid, function (res) {
                        console.log(res.response);
                        if (res != null && res != 0) {

                            $("#TotalQuesReg").text("10");
                            $(".CurrentQuizType").text("FunFriday");
                            sessionStorage.setItem("QT", "SQ");
                            sessionStorage.removeItem("FFPressed");
                            GetSpecialQuiz("FunFriday");

                        }
                        
                    });
                }
                else {
                    var dt = new Date();//current Date that gives us current Time also

                    var startTime = rdata.response[0].startTime;
                    var endTime = rdata.response[0].endTime;

                    //var s = startTime.split(':');
                    var dt1 = new Date(dt.getFullYear() + '/' + (dt.getMonth() + 1) + '/' + dt.getDate() + ' ' +
                        startTime);

                    //var e = endTime.split(':');
                    var dt2 = new Date(dt.getFullYear() + '/' + (dt.getMonth() + 1) + '/' + dt.getDate() + ' ' +
                        endTime);



                    //alert((dt >= dt1 && dt <= dt2) ? 'Current time is between startTime and endTime' :
                    //    'Current time is NOT between startTime and endTime');
                    //alert('dt = ' + dt + ',  dt1 = ' + dt1 + ', dt2 =' + dt2)


                    if (dt >= dt1 && dt <= dt2) {
                        sessionStorage.setItem("QT", "SQ");
                        sessionStorage.removeItem("FFPressed");
                        console.log('Current time is between startTime and endTime');
                        //GetSpecialQuiz("EkusheyQuiz");
                        $(".CurrentQuizType").text("WorldCupQuiz");
                        GetSpecialQuiz("WorldCupQuiz");
                    } else {
                        sessionStorage.removeItem("QT");
                        $("#TotalQuesReg").text("60");
                        GetNormalQuiz();
                    }
                }





                
            }
            else {
                var challangeQuiz = sessionStorage.getItem("scs");
                if (challangeQuiz != null && challangeQuiz != "") {
                    GetChallangeQuiz();
                } else {
                    $("#TotalQuesReg").text("60");
                    GetNormalQuiz();
                }
            }
        });

        //var challangeQuiz = sessionStorage.getItem("scs");
        //if (challangeQuiz != null && challangeQuiz != "") {
        //    GetChallangeQuiz();
        //} else {
        //    $("#TotalQuesReg").text("60");
        //    GetNormalQuiz();
        //}


        //Old
        //if (new Date().toString().includes("Fri ") == true && FFPressed == 1) { //Need to add false for testing except friday // For Fun Friday Special Quiz Logic
        //    $("#TotalQuesReg").text("10");
        //    sessionStorage.setItem("QT", "SQ");
        //    sessionStorage.removeItem("FFPressed");
        //    GetSpecialQuiz();
        //}
        //else {
        //    sessionStorage.removeItem("QT");
        //    var challangeQuiz = sessionStorage.getItem("scs");
        //    if (challangeQuiz != null && challangeQuiz != "") {
        //        GetChallangeQuiz();
        //    } else {
        //        $("#TotalQuesReg").text("60");
        //        GetNormalQuiz();
        //    }
        //}
        

    }
});





function GetChallangeQuiz() {
    var challangeQuiz = atob(atob(sessionStorage.getItem("scs")));
    console.log(challangeQuiz);
    console.log("scs");
    sessionStorage.removeItem("scs");


}

function GetSpecialQuiz(quiztype) {
    //$.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=Quiz_Khelun_FunFriday&srcurl=" + window.location.href, function (rData) { });
    //var nameWC = sessionStorage.getItem("nameWC");
    console.log(serviceName);
    if (serviceName === "KidStar-Math" || serviceName === "KidStar-GuessWord") {
        JhotpotThemeQuestionsForSpecialQuizes = window.location.origin + "/api/jhotpot/GetJhotpotQuestionsSpellingAndCine";
    } else if (serviceName === "QuizMaster") {
        JhotpotThemeQuestionsForSpecialQuizes = window.location.origin + "/api/jhotpot/GetJhotpotThemeQuestionsNew";
    }
   
    if (typeof JhotpotThemeQuestionsForSpecialQuizes === "undefined") {

        if (serviceName === "KidStar-Math" || serviceName === "KidStar-GuessWord") {
            JhotpotThemeQuestionsForSpecialQuizes = window.location.origin + "/api/jhotpot/GetJhotpotQuestionsSpellingAndCine";
        } else if (serviceName === "QuizMaster") {
            JhotpotThemeQuestionsForSpecialQuizes = window.location.origin + "/api/jhotpot/GetJhotpotThemeQuestionsNew";
        }
       
    }

    $.get(JhotpotThemeQuestionsForSpecialQuizes + "?fbid=" + fbid + "&ServiceType=" + quiztype + "&serviceName=" + serviceName, function (res) {
        
        var dd = res.result.isPlayed === "1";
        if (!dd) {
            var node = document.getElementById('gg')
            var visibility = node.style.visibility;
            node.style.visibility = visibility == "visible" ? 'hidden' : "visible"
            questionArray = res.result.qlist;
            //console.log(res.result.Qlist);
            //var fortySeconds = 180;
            var fortySeconds = res.result.timeDuration;
            var display = document.getElementById("countdowntimer");
            startTimer(fortySeconds, display);

            var data = questionArray[0];
            updateQuestionForSpecialQuiz(data, 0);
        } else {
            /*   $("#m_jh_pl").modal('show');*/
            window.location.href = "../Home/SorryPage?SQ=1";
        }
    });
}


function updateQuestionForSpecialQuiz(data, index) {

    $("#questionImage").hide();

    $("#quesid").text(index + 1);

    $("#question").text(data.question);
    $("#qindex").val(index);

    if (data.questionImage == null) {
        data.questionImage = "whitebg.jpg";
    }

    if (data.questionImage != null) {

        if (serviceName === "KidStar-GuessWord") {
            var imageUrl = "https://wap.shabox.mobi/cmsnew/assets/images/KidStar/" + data.questionImage;
        }
        else if (serviceName === "KidStar-Math") {
            var imageUrl = "https://wap.shabox.mobi/cmsnew/assets/images/mathologic/" + data.questionImage;
        }
        $("#questionImage").show();
        $("#questionImage").val("");
        $("#questionImage").text("");
        $("#questionImage").append('<a><img src="' + imageUrl + '" id="edit-save" style="width: 30vw;height: 15vh;margin-bottom: 3%;"/></a>');
        if (serviceName === "KidStar-Math") {
            document.getElementById('edit-save').style.width = '78vw';
        }

    }

   

    $("#option_layout").append('<div class="option-btn"><div class= "row" onclick="submitQuestionForSpecialQuiz(\'Option1\')"><div class="col-2" style=""></div><div class="col-8" style="11px;">' + data.option1 + '</div></div ></div >' +
        '<div class="option-btn"><div class="row" onclick="submitQuestionForSpecialQuiz(\'Option2\')"> <div class="col-2" style=""></div><div class="col-8" style="11px;">' + data.option2 + '</div></div> </div >' +
        '<div class="option-btn"><div class="row" onclick="submitQuestionForSpecialQuiz(\'Option3\')"><div class="col-2" style=""></div><div class="col-8" style="11px;">' + data.option3 + '</div> </div></div >');
}


function submitQuestionForSpecialQuiz(option) {
    var quiztype = $(".CurrentQuizType").text();
    var myAudio = document.getElementById('aa');
    if (myAudio.duration > 0 && !myAudio.paused) {

    } else {
        myAudio.play();
        //myAudio.load();
    }

    //console.log(option);
    var currentindexx = $("#qindex").val();
    if (questionArray.length === index + 1) {
        if (!flag) {
            var data = questionArray[currentindexx];
            if (data.Answer === option) {
                right += 1;
                $("#rt").text(right);
            } else {
                wrong += 1;
                $("#ng").text(wrong);
            }
            let fbid = $.session.get('fbid');

            $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=Submit_SpecialQuiz_Question&srcurl=" + window.location.href, function (rData) { });

            ////Ratul
            //if (nameWC === "WC" && nameNQZ !== "NQZ") {
            //    $.get(JhotpotAnswerWithTimeBkashWC + "?fbid=" + $.session.get('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=QuizMaster&ServiceType=" + quiztype, function (res) {

            //    });
            //} else if (nameWC !== "WC" && nameNQZ === "NQZ") {
            //    $.get(JhotpotAnswerWithTimeBkashForSpecialQuiz + "?fbid=" + $.session.get('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=QuizMaster&ServiceType=" + quiztype, function (res) {

            //    });
            //}

            $.get(JhotpotAnswerWithTimeBkash + "?fbid=" + sessionStorage.getItem('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=" + sessionStorage.getItem('serviceName') + "&ServiceType=" + quiztype, function (res) {

            });


            //$.get(JhotpotAnswerWithTimeBkashForSpecialQuiz + "?fbid=" + $.session.get('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=QuizMaster&ServiceType=" + quiztype, function (res) {
                
         //});
            timetaken = 0;
            flag = true;
        }
        showResult();
    } else {
        var currentindex = $("#qindex").val();
        var data = questionArray[currentindex];
        if (data.answer === option) {
            right += 1;
            $("#rt").text(right);
        } else {
            wrong += 1;
            $("#ng").text(wrong);
        }
        //if (nameWC === "Page2") {
        //    $.get(JhotpotAnswerWithTimeBkashWC + "?fbid=" + $.session.get('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=QuizMaster&ServiceType=" + quiztype, function (res) {

        //    });
        //} else if (nameWC === "Page1") {
        //    $.get(JhotpotAnswerWithTimeBkashForSpecialQuiz + "?fbid=" + $.session.get('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=QuizMaster&ServiceType=" + quiztype, function (res) {

        //    });
        //}

        $.get(JhotpotAnswerWithTimeBkash + "?fbid=" + sessionStorage.getItem('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=" + sessionStorage.getItem('serviceName') + "&ServiceType=" + quiztype, function (res) {

        });

        //$.get(JhotpotAnswerWithTimeBkashForSpecialQuiz + "?fbid=" + $.session.get('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=QuizMaster&ServiceType=" + quiztype, function (res) {
            
        //});
        timetaken = 0;


        index += 1;
        if (questionArray.length != index) {
            $("#option_layout").html('');
            data = questionArray[index];
            updateQuestionForSpecialQuiz(data, index);
            $("#qindex").val(index);
        }
    }
}


function GetNormalQuiz() {
    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=Played_Quiz&srcurl=" + window.location.href, function (rData) { });
    console.log(serviceName);
    if (serviceName === "KidStar-Math" || serviceName === "KidStar-GuessWord") {
        console.log('Multi Service');
        JhotpotThemeQuestionsForSpecialQuizes = window.location.origin + "/api/jhotpot/GetJhotpotQuestionsSpellingAndCine";
    } else if (serviceName === "QuizMaster") {
        console.log('Quiz Master');
        JhotpotThemeQuestionsForSpecialQuizes = window.location.origin + "/api/jhotpot/GetJhotpotThemeQuestionsNew";
    }
    
    //$.ajax({

    //    url: "../LandingPage/SaveThemeInfo?msisdn=" + fbid + "&theme=" + "Quiz_Khelun", success: function (res) {


    //    }
    //});
    //$.get("https://wap.shabox.mobi/quizplaynew/api/jhotpot/GetJhotpotQuestionsTheme?fbid=" + fbid + "&theme=112", function (res) {
    if (typeof JHOTPOT_QUESTIONS === "undefined") {


        if (serviceName === "KidStar-Math" || serviceName === "KidStar-GuessWord") {
            JHOTPOT_QUESTIONS = BASE_URL + "/api/jhotpot/GetJhotpotQuestionsSpellingAndCine";
        } else if (serviceName === "QuizMaster") {
            JHOTPOT_QUESTIONS = BASE_URL + "/api/jhotpot/GetJhotpotThemeQuestionsNew";
        }
       

        ////JHOTPOT_QUESTIONS = "https://wap.shabox.mobi/quizplaynew/api/jhotpot/GetJhotpotThemeQuestionsNew";
        //JHOTPOT_QUESTIONS = BASE_URL + "/api/jhotpot/GetJhotpotThemeQuestionsNew";
    }

    $.get(JhotpotThemeQuestionsForSpecialQuizes + "?fbid=" + fbid + "&serviceName=" + sessionStorage.getItem('serviceName'), function (res) {

        var dd = res.result.isPlayed === "1";
        if (!dd) {
            var node = document.getElementById('gg')
            var visibility = node.style.visibility;
            node.style.visibility = visibility == "visible" ? 'hidden' : "visible"
            questionArray = res.result.qlist;
            //console.log(res.result.Qlist);
            var three_min = 3 * 60;
            var display = document.getElementById("countdowntimer");
            startTimer(three_min, display);

            var data = questionArray[0];
            updateQuestion(data, 0);
        } else {
            /*   $("#m_jh_pl").modal('show');*/
            window.location.href = "../Home/SorryPage";
        }
    });
}


function closeModal() {
    $("#m_jh_pl").modal('hide');
    window.location.href = BASE_URL + '/LandingPage/index';
    return false;
}

function updateQuestion(data, index) {
    $("#questionImage").hide();

    $("#quesid").text(index+1);

    $("#question").text(data.question);
    $("#qindex").val(index);

    if (data.questionImage != null) {
        if (serviceName === "KidStar-GuessWord") {
            var imageUrl = "https://wap.shabox.mobi/cmsnew/assets/images/KidStar/" + data.questionImage;
        }
        else if (serviceName === "KidStar-Math") {
            var imageUrl = "https://wap.shabox.mobi/cmsnew/assets/images/mathologic/" + data.questionImage;
        }

        $("#questionImage").show();
        $("#questionImage").val("");
        $("#questionImage").text("");
        $("#questionImage").append('<a><img src="' + imageUrl + '" id="edit-save" style="width: 30vw;height: 15vh;margin-bottom: 3%;"/></a>');
        if (serviceName === "KidStar-Math") {
            document.getElementById('edit-save').style.width = '78vw';
        }
        
    }

    $("#option_layout").append('<div class="option-btn"><div class= "row" onclick="submitQuestion(\'Option1\')"><div class="col-2" style=""></div><div class="col-8" style="11px;">' + data.option1 + '</div></div ></div >' +
        '<div class="option-btn"><div class="row" onclick="submitQuestion(\'Option2\')"> <div class="col-2" style=""></div><div class="col-8" style="11px;">' + data.option2 + '</div></div> </div >' +
        '<div class="option-btn"><div class="row" onclick="submitQuestion(\'Option3\')"><div class="col-2" style=""></div><div class="col-8" style="11px;">' + data.option3 + '</div> </div></div >');
}

function submitQuestion(option) {

    var myAudio = document.getElementById('aa');
    if (myAudio.duration > 0 && !myAudio.paused) {

    } else {
        myAudio.play();
        //myAudio.load();
    }

    //console.log(option);()
    var currentindexx = $("#qindex").val();
    if (questionArray.length === index + 1) {
        if (!flag) {
            var data = questionArray[currentindexx];
            if (data.Answer === option) {
                right += 1;
                $("#rt").text(right);
            } else {
                wrong += 1;
                $("#ng").text(wrong);
            }
            let fbid = $.session.get('fbid');

            $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=Submit_Question&srcurl=" + window.location.href, function (rData) { });

            //$.ajax({

            //    url: "../LandingPage/SaveThemeInfo?msisdn=" + fbid + "&theme=" + "Submit_Question", success: function (res) {


            //    }
            //});


            ///Ratul
            //if (nameWC === "Page2") {
            //    $.get(JHOTPOT_ANSWER_WC + "?fbid=" + $.session.get('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=QuizMasterWCup", function (res) {
            //        //console.log("From else: "+res.result)
            //    });
            //} else if (nameWC === "Page1") {
            //    $.get(JHOTPOT_ANSWER + "?fbid=" + $.session.get('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=QuizMaster", function (res) {
            //        //console.log("From else: "+res.result)
            //    });
            //}

            $.get(JHOTPOT_ANSWER + "?fbid=" + sessionStorage.getItem('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=" + sessionStorage.getItem('serviceName') , function (res) {

            });

            //$.get(JHOTPOT_ANSWER + "?fbid=" + $.session.get('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=QuizMaster", function (res) {
            //    //console.log("From else: "+res.result)
            //});
            timetaken = 0;
            flag = true;
        }
        showResult();
    } else {
        var currentindex = $("#qindex").val();
        var data = questionArray[currentindex];
        if (data.answer === option) {
            right += 1;
            $("#rt").text(right);
        } else {
            wrong += 1;
            $("#ng").text(wrong);
        }



        $.get(JHOTPOT_ANSWER + "?fbid=" + sessionStorage.getItem('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=" + sessionStorage.getItem('serviceName'), function (res) {

        });


        //$.get(JHOTPOT_ANSWER + "?fbid=" + $.session.get('fbid') + "&qid=" + data.id + "&answer=" + option + "&timetaken=" + timetaken + "&ServiceName=QuizMaster", function (res) {
        //    //console.log("From else: "+res.result)
        //});
        timetaken = 0;


        index += 1;
        if (questionArray.length!= index) {
            $("#option_layout").html('');
            data = questionArray[index];
            updateQuestion(data, index);
            $("#qindex").val(index);
        }
    }
}

function showResult() {
    var QuizType = sessionStorage.getItem('serviceName'); 

    if (nameWC === "Page2") {
        if (QuizType != null && typeof QuizType != "undefined" && QuizType == "SQ") {
            /*sessionStorage.removeItem("QT");*/

            var src1 = BASE_URL + '/Assets/PrizePushPop.png';
            $("#result_pic").attr("src", src1);
            var cssObject = $('#result_pic').prop('style'); cssObject.removeProperty('width');
            //$("#result_title").text("আপনি বিজয়ি হয়েছেন");
            $("#result_content").text("বিজয়ীদের নাম দেখুন ");
            $("#result_content2").text("লিডারবোর্ডে ");
            $("#modal_buttonPlay").text("প্রাইজ");
            $("#modal_button").text("ওকে");
            $("#modal_buttonPlay").attr("onclick", "RedirectOnPrize()");
            $("#modal_button").attr("onclick", "RedirectOnOkay()");

        } else {
            if (right >= 40) {
             

                var src1 = BASE_URL + '/Assets/WinnerCeleb.png';
                $("#result_pic").attr("src", src1);
                //$("#result_title").text("আপনি বিজয়ি হয়েছেন");
                $("#result_content").text("বিশ্বকাপ কুইজ খেলুন ");
                $("#result_content2").text("সঠিক হয়েছে document.getElementById('rt').textContent টি ভুল হয়েছে document.getElementById('ng').textContent টি...");
                $("#modal_buttonPlay").text("আবার খেলুন");
                $("#modal_button").text("ওকে");
                $("#modal_buttonPlay").attr("onclick", "reloadUI()");
                $("#modal_button").attr("onclick", "RedirectOnOkay()");
            } else {
    

                var src1 = BASE_URL + '/Assets/';
                $("#result_pic").attr("src", src1);
       
                $("#result_content").text("বিশ্বকাপ কুইজ খেলুন ");
                $("#result_content2").text("৭০ হাজার টাকা পর্যন্ত ক্যাশ প্রাইজ জিতুন");
                $("#modal_buttonPlay").text("আবার খেলুন");
                $("#modal_button").text("ওকে");
                $("#modal_buttonPlay").attr("onclick", "reloadUI()");
                $("#modal_button").attr("onclick", "RedirectOnOkay()");
            }
        }

        $("#m_jh_result").modal('show');
        $(".CurrentQuizType").text("");
        return;
    }
    else if (nameWC === "Page1") {
        if (QuizType != null && typeof QuizType != "undefined" && QuizType == "SQ") {
            /*sessionStorage.removeItem("QT");*/

            var src1 = BASE_URL + '/Assets/PrizePushPop.png';
            $("#result_pic").attr("src", src1);
            var cssObject = $('#result_pic').prop('style'); cssObject.removeProperty('width');
            //$("#result_title").text("আপনি বিজয়ি হয়েছেন");
            $("#result_content").text("বিজয়ীদের নাম দেখুন ");
            $("#result_content2").text("লিডারবোর্ডে ");
            $("#modal_buttonPlay").text("প্রাইজ");
            $("#modal_button").text("ওকে");
            $("#modal_buttonPlay").attr("onclick", "RedirectOnPrize()");
            $("#modal_button").attr("onclick", "RedirectOnOkay()");

        } else {
            if (right >= 40) {
                //var src1 = BASE_URL + 'Assets/birthday.png';
                //$("#result_pic").attr("src", src1);
                //$("#result_title").text("অভিনন্দন");
                //$("#result_content").text("আপনি ৩০ টি প্রশ্নের সঠিক উত্তর দিয়ে বিজয়ি হয়েছেন।");
                //$("#modal_button").text("ওকে");
                //$("#modal_button").attr("onclick", "reloadUI()");

                var src1 = BASE_URL + '/Assets/WinnerCeleb.png';
                $("#result_pic").attr("src", src1);
                //$("#result_title").text("আপনি বিজয়ি হয়েছেন");
                $("#result_content2").text("সঠিক হয়েছে " + document.getElementById('rt').textContent + " টি ভুল হয়েছে "+ document.getElementById('ng').textContent + " টি");
                $("#modal_buttonPlay").text("আবার খেলুন");
                $("#modal_button").text("ওকে");
                $("#modal_buttonPlay").attr("onclick", "reloadUI()");
                $("#modal_button").attr("onclick", "RedirectOnOkay()");
            } else {
              
                var src1 = BASE_URL + '/Assets/';
                $("#result_pic").attr("src", src1);
         
                $("#result_content2").text("সঠিক হয়েছে " + document.getElementById('rt').textContent + " টি ভুল হয়েছে " + document.getElementById('ng').textContent + " টি");
                $("#modal_buttonPlay").text("আবার খেলুন");
                $("#modal_button").text("ওকে");
                $("#modal_buttonPlay").attr("onclick", "reloadUI()");
                $("#modal_button").attr("onclick", "RedirectOnOkay()");
            }
        }

        $("#m_jh_result").modal('show');
        $(".CurrentQuizType").text("");
        return;
    }

    
}


function showResultAfterTimeFinished() {

    if (nameWC === "Page2") {
        if ($('#m_jh_result').is(':visible')) {

        } else {
            var src1 = BASE_URL + '/Assets/TimeUp.png';
            $("#result_pic").attr("src", src1);
            /*$("#result_title").text("আপনি হেরে গিয়েছেন?");*/
            $("#result_content").text("বিশ্বকাপ কুইজ খেলুন ");
            $("#result_content2").text("ক্যাশ প্রাইজ জিতুন");
            $("#modal_buttonPlay").text("আবার খেলুন");
            $("#modal_button").text("ওকে");
            $("#modal_buttonPlay").attr("onclick", "reloadUI()");
            $("#modal_button").attr("onclick", "RedirectOnOkay()");

            $("#m_jh_result").modal('show');
        }
        return;
    }
    else if (nameWC === "Page1") {
        if ($('#m_jh_result').is(':visible')) {

        } else {
            var src1 = BASE_URL + '/Assets/TimeUp.png';
            $("#result_pic").attr("src", src1);
         
            $("#result_content").text("সঠিক হয়েছে " + document.getElementById('rt').textContent + " টি ভুল হয়েছে " + document.getElementById('ng').textContent + " টি");
            $("#modal_buttonPlay").text("আবার খেলুন");
            $("#modal_button").text("ওকে");
            $("#modal_buttonPlay").attr("onclick", "reloadUI()");
            $("#modal_button").attr("onclick", "RedirectOnOkay()");

            $("#m_jh_result").modal('show');
        }
        return;
    }

  
}

function reloadUI() {
    window.location.reload();
    return true;
}

function GetBackToHome() {
    //window.location.href = BASE_URL + 'LandingPage/Index';
    var loc = BASE_URL + '/LandingPage/Index';
    window.location.href = loc;
    //window.location.replace(loc);
    //return true;
}

function exit() {
    let fbid = $.session.get('fbid');

    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=Exit&srcurl=" + window.location.href, function (rData) { });


    //$.ajax({

    //    url: "../LandingPage/SaveThemeInfo?msisdn=" + fbid + "&theme=" + "Exit", success: function (res) {


    //    }
    //});
    window.location.href = BASE_URL + '/LandingPage/Index';
    return false;
}

//new timer
function get_elapsed_time_string(total_seconds) {
    function pretty_time_string(num) {
        return (num < 10 ? "0" : "") + num;
    }

    var hours = Math.floor(total_seconds / 3600);
    total_seconds = total_seconds % 3600;

    var minutes = Math.floor(total_seconds / 60);
    total_seconds = total_seconds % 60;

    var seconds = Math.floor(total_seconds);

    // Pad the minutes and seconds with leading zeros, if required
    hours = pretty_time_string(hours);
    minutes = pretty_time_string(minutes);
    seconds = pretty_time_string(seconds);

    if (minutes == 3 && seconds > 3) {
        window.location = "";
    }

    // Compose the string for display
    var currentTimeString = hours + ":" + minutes + ":" + seconds;

    return currentTimeString;
}

function startTimer(duration, display) {
    var start = new Date();
    var timer = duration, minutes, seconds;
    var totalDuration = duration * 1000; //converting second to milisec
    var TimeInterval = setInterval(function () {
        var current = new Date();
        if (((current.getTime() - start.getTime()) / 1000) < (duration + 2)) { //(duration + 2) is added for the 1st and last missing sec of javascript, [forbidden]

            var s = Math.floor((totalDuration / 1000)) % 60;
            var m = Math.floor((totalDuration / 60000)) % 60;

            m = m < 10 ? "0" + m : m;
            s = s < 10 ? "0" + s : s;
            display.textContent = m + ":" + s;

            if (((current.getTime() - start.getTime()) / 1000) >= (duration + 2)) { //(duration + 2) is added for the 1st and last missing sec of javascript, [forbidden]
                totalDuration = 0;
                //clearInterval(this);
                clearInterval(TimeInterval);
                TimeInterval = 0;
                showResultAfterTimeFinished();
                return;
            } else {
                totalDuration = totalDuration - 1000;
                timetaken += 1;
            }
        } else {
            totalDuration = 0;
            //clearInterval(this);
            clearInterval(TimeInterval);
            TimeInterval = 0;
            showResultAfterTimeFinished();

            return;
        }
    }, 1000);
    //setInterval(function () {
    //    var current = new Date();
    //    var count = +current - +start;

    //    var s = Math.floor((count / 1000)) % 60;
    //    var m = Math.floor((count / 60000)) % 60;

    //    //minutes = parseInt(timer / 60, 10);
    //    //seconds = parseInt(timer % 60, 10);

    //    //minutes = minutes < 10 ? "0" + minutes : minutes;
    //    //seconds = seconds < 10 ? "0" + seconds : seconds;
    //    m = m < 10 ? "0" + m : m;
    //    s = s < 10 ? "0" + s : s;

    //    //display.textContent = minutes + ":" + seconds;
    //    display.textContent = m + ":" + s;


    //    if (((current.getTime() - start.getTime()) / 1000) >= 180) {
    //        clearInterval(this);
    //        showResult();
    //    } else {
    //        timetaken += 1;
    //    }

    //    //if (--timer < 0) {
    //    //    timer = duration;
    //    //    clearInterval(this);
    //    //    showResult();
    //    //} else {
    //    //    timetaken += 1;
    //    //}
    //}, 1000);
}

function RedirectOnOkay() {
    window.location.href = BASE_URL;
}

function RedirectOnPrize() {
    window.location.href = BASE_URL +"/Landingpage/Prizes";
}

function OldstartTimer(duration, display) {
    var timer = duration, minutes, seconds;
    setInterval(function () {
        minutes = parseInt(timer / 60, 10);
        seconds = parseInt(timer % 60, 10);

        minutes = minutes < 10 ? "0" + minutes : minutes;
        seconds = seconds < 10 ? "0" + seconds : seconds;

        display.textContent = minutes + ":" + seconds;

        if (--timer < 0) {
            timer = duration;
            clearInterval(this);
            showResult();
        } else {
            timetaken += 1;
        }
    }, 1000);
}

