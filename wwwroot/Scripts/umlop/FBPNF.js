///Firebase Push Notification Feature Start

        $(function () {

            var token = null;
            var config = {
                apiKey: "AIzaSyCgxfNVhoQD4IHFiaeKd0qP67goFbBlPUE",
                authDomain: "push-notification-test-77c9d.firebaseapp.com",
                projectId: "push-notification-test-77c9d",
                storageBucket: "push-notification-test-77c9d.appspot.com",
                messagingSenderId: "575417553672",
                appId: "1:575417553672:web:71da03329f96426b8492bd",
                measurementId: "G-1EXXNFN9L5"
            };
            firebase.initializeApp(config);

            const messaging = firebase.messaging();


            //differtentAlertsBasedOnFeature(messaging);


            
            
            //setTimeout(function () {
            //    HomeAlertsBasedOnTime(messaging);
            //}, 5000); //milliseconds
        
            
        });

function HomeAlertsBasedOnTime(messaging) {
    var pathName = window.location.pathname;

    if (pathName == "/") {

        if (Notification.permission == "default") {

            Swal.fire({
                icon: 'success',
                /*imageUrl: '../Assets/Leaderboard 2.png',*/
                //title: 'দুঃখিত',
                //position: 'top-start',
                //position: 'top',
                text: 'কুইজ মাস্টারের নতুন ফিচার ও আপডেট পেতে পরবর্তী নোটিফিকেশন এ Allow ক্লিক করুন ',
                confirmButtonText: 'ওকে',
                confirmButtonColor: '#e3126e',
                showCancelButton: false,
                //cancelButtonColor: '#efaf11',
                showCloseButton: true,
                cancelButtonColor: '#033da3',
                cancelButtonText: 'No',
                allowOutsideClick: true,
                //background:'#033da3'
                //width: '60vw'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=PushNotiPopUp_Home_Yes&srcurl=" + window.location.href, function (rData) { });
                    messaging.requestPermission()
                        .then((permission) => {
                            console.log("granted");
                            console.log(permission);
                            if (isTokenSentToServer()) {
                                console.log("already granted");
                            } else {
                                getRegtoken(messaging);
                            }
                        }).catch((err) => {
                            if (err.message.includes('messaging/permission-blocked')) {
                                saveDeniedNotificationRequest(0);
                            } else {
                                saveDeniedNotificationRequest(3);
                            }
                            console.log(err);
                        });

                } else {
                    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=PushNotiPopUp_Home_No&srcurl=" + window.location.href, function (rData) { });
                }
            });
        }

    }

}

function differtentAlertsBasedOnFeature(messaging) {
    var pathName = window.location.pathname;

    //if (pathName == "/") {

    //    if (Notification.permission == "default") {

    //        Swal.fire({
    //            icon: 'success',
    //            /*imageUrl: '../Assets/Leaderboard 2.png',*/
    //            //title: 'দুঃখিত',
    //            //position: 'top-start',
    //            //position: 'top',
    //            text: 'কুইজ মাস্টারের নতুন ফিচার ও আপডেট পেতে পরবর্তী নোটিফিকেশন এ Allow ক্লিক করুন ',
    //            confirmButtonText: 'ওকে',
    //            confirmButtonColor: '#e3126e',
    //            showCancelButton: false,
    //            //cancelButtonColor: '#efaf11',
    //            showCloseButton: true,
    //            cancelButtonColor: '#033da3',
    //            cancelButtonText: 'No',
    //            allowOutsideClick: true,
    //            //background:'#033da3'
    //            //width: '60vw'
    //        }).then((result) => {
    //            if (result.isConfirmed) {
    //                $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=PushNotiPopUp_Home_Yes&srcurl=" + window.location.href, function (rData) { });
    //                messaging.requestPermission()
    //                    .then((permission) => {
    //                        console.log("granted");
    //                        console.log(permission);
    //                        if (isTokenSentToServer()) {
    //                            console.log("already granted");
    //                        } else {
    //                            getRegtoken();
    //                        }
    //                    }).catch((err) => {
    //                        if (err.message.includes('messaging/permission-blocked')) {
    //                            saveDeniedNotificationRequest(0);
    //                        } else {
    //                            saveDeniedNotificationRequest(3);
    //                        }
    //                        console.log(err);
    //                    });

    //            } else {
    //                $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=PushNotiPopUp_Home_No&srcurl=" + window.location.href, function (rData) { });
    //            }
    //        });
    //    }

    //}
    //else
        if (pathName.indexOf("/Tab_layout") !== -1) {

        if (Notification.permission == "default") {

            Swal.fire({
                imageUrl: '../Assets/Leaderboard 2.png',
                //position: 'top',
                text: 'স্কোর আর উইনার আপডেট পেতে পরবর্তী মেসেজ এ Allow ক্লিক  করো ',
                confirmButtonText: 'Okay',
                confirmButtonColor: '#e3126e',
                showCancelButton: false,
                cancelButtonColor: '#efaf11',
                cancelButtonText: 'No',
                showCloseButton: true,
                allowOutsideClick: true
            }).then((result) => {
                if (result.isConfirmed) {
                    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=PushNotiPopUp_LeaderBoard_Yes&srcurl=" + window.location.href, function (rData) { });
                    messaging.requestPermission()
                        .then((permission) => {
                            console.log("granted");
                            console.log(permission);
                            if (isTokenSentToServer()) {
                                console.log("already granted");
                            } else {
                                getRegtoken(messaging);
                            }
                        }).catch((err) => {
                            if (err.message.includes('messaging/permission-blocked')) {
                                saveDeniedNotificationRequest(0);
                            } else {
                                saveDeniedNotificationRequest(3);
                            }
                            console.log(err);
                        });


                } else {
                    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=PushNotiPopUp_LeaderBoard_No&srcurl=" + window.location.href, function (rData) { });
                }
            });
        }

    }
    //else if (pathName.indexOf("/PaymentStatusModal") !== -1) {

    //    if (Notification.permission == "default") {

    //        Swal.fire({
    //            imageUrl:"../Assets/test101.png",
    //            //position: 'top-start',
    //            text: 'পরবর্তী নোটিফিকেশন এ Allow ক্লিক করে পেমেন্ট আপডেট জেনে নাও নিয়মিত ',
    //            confirmButtonText: 'Okay',
    //            confirmButtonColor: '#e3126e',
    //            showCancelButton: false,
    //            cancelButtonColor: '#efaf11',
    //            cancelButtonText: 'No',
    //            showCloseButton: true,
    //            allowOutsideClick: true
    //        }).then((result) => {
    //            if (result.isConfirmed) {
    //                $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=PushNotiPopUp_PaymentStatus_Yes&srcurl=" + window.location.href, function (rData) { });
    //                messaging.requestPermission()
    //                    .then((permission) => {
    //                        console.log("granted");
    //                        console.log(permission);
    //                        if (isTokenSentToServer()) {
    //                            console.log("already granted");
    //                        } else {
    //                            getRegtoken();
    //                        }
    //                    }).catch((err) => {
    //                        if (err.message.includes('messaging/permission-blocked')) {
    //                            saveDeniedNotificationRequest(0);
    //                        } else {
    //                            saveDeniedNotificationRequest(3);
    //                        }
    //                        console.log(err);
    //                    });


    //            } else {
    //                $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=PushNotiPopUp_PaymentStatus_No&srcurl=" + window.location.href, function (rData) { });
    //            }
    //        });
    //    }

    //}
    else if (pathName.indexOf("/Prizes") !== -1) {

        if (Notification.permission == "default") {

            Swal.fire({
                //icon: 'success',
                //position: 'top-start',
                imageUrl: "../Assets/PrizePushPop.png",
                text: 'প্রাইজ সম্পর্কে জানতে আগ্রহী? তবে পরবর্তী নোটিফিকেশন এ Allow ক্লিক করো',
                confirmButtonText: 'Okay',
                confirmButtonColor: '#e3126e',
                showCancelButton: false,
                cancelButtonColor: '#efaf11',
                cancelButtonText: 'No',
                allowOutsideClick: true,
                showCloseButton: true,
                //width: '60vw'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=PushNotiPopUp_Prizes_Yes&srcurl=" + window.location.href, function (rData) { });

                    messaging.requestPermission()
                        .then((permission) => {
                            console.log("granted");
                            if (isTokenSentToServer()) {
                                console.log("already granted");
                            } else {
                                getRegtoken(messaging);
                            }
                        }).catch((err) => {
                            if (err.message.includes('messaging/permission-blocked')) {
                                saveDeniedNotificationRequest(0);
                            } else {
                                saveDeniedNotificationRequest(3);
                            }
                            console.log(err);
                        });

                } else {
                    $.get("../LandingPage/SaveCapmLog?msisdn=" + sessionStorage.getItem("fbid") + "&ckey=" + sessionStorage.getItem("ckey") + "&cname=" + "KidStar" + "&theme=PushNotiPopUp_Prizes_No&srcurl=" + window.location.href, function (rData) { });

                }
            });
        }

    }

}




function getRegtoken(messaging) {
    messaging.getToken().then((currentToken) => {
        if (currentToken) {
            console.log(currentToken);
            setTokenSentToServer(true);
            sessionStorage.setItem("FCT", currentToken);
            $("#PushNotificationToken").text(currentToken);
            
            saveToken(currentToken);
        } else {
            console.log('No Instance ID token available. Request permission to generate one.');
            setTokenSentToServer(false);
        }
    }).catch((err) => {
        console.log('An error occurred while retrieving token. ', err);
        setTokenSentToServer(false);
    });

}

function setTokenSentToServer(sent) {
    window.localStorage.setItem('sentToServer', sent ? 1 : 0);
}
function saveToken(currentToken) {
    $.get("../LandingPage/SavePushNotificationAllowedLogs?Token=" + currentToken + "&AllowedType=" + 1 + "&SourceUrl=" + window.location.href, function (rData) { });

    $.get("../LandingPage/SaveToken?Token=" + currentToken, function (rData) { });

    var msisdn = sessionStorage.getItem("fbid");
    if (msisdn != null && msisdn != "") {
        var data = { "MSISDN": msisdn, "Name": "", "SourceUrl": window.location.href, 'CKey': '', "FPToken": currentToken };

        $.ajax({
            type: "POST",
            url: '../Home/SaveUserInfo',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                //if (res.response == "true") {

                //}
                //if (res === false) {
                //    alert("প্রথমে প্রোফাইল সেট আপ করুন ");
                //}
            },
            error: function (errMsg) {
            }
        });
    }

}

function isTokenSentToServer() {
    return window.localStorage.getItem('sentToServer') === '1';
}
function saveDeniedNotificationRequestLog(status) {
    $.get("../LandingPage/SavePushNotificationAllowedLogs?Token=" + "" + "&AllowedType=" + status + "&SourceUrl=" + window.location.href, function (rData) { });
}

function saveDeniedNotificationRequest(status) {
    saveDeniedNotificationRequestLog(status);
}

        /////////  Firebase Push Notification Feature End