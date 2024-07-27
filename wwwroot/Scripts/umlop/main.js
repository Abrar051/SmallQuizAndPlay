


//var BASE_URL = "../";
//var BASE_URL = window.location.origin +'/quizmaster'; // Test
var BASE_URL = window.location.origin; //Live

//var BASE_URL = "https://quizmaster.shabox.mobi/";
var API_BASE_URL = "https://wap.shabox.mobi/quizplaynew";
//var API_BASE_URL = "http://localhost:3470";

var API_BASE_SERVICE_URL = "https://wap.shabox.mobi/serviceapi";
var BKASH = "https://wap.shabox.mobi/apibkash";
var BKASHOLD = "https://wap.shabox.mobi/bkashapi";


var DAILY_BONUS = API_BASE_URL + "/api/jhotpot/DailyBonus";
var CHECK_PLAY_STATUS = API_BASE_URL + "/api/jhotpot/CheckPlayerPlayStatus";
var REGISTER_PLAY_STATUS = API_BASE_URL + "/api/jhotpot/RegisterUser";
//var JHOTPOT_QUESTIONS = API_BASE_URL + "/api/jhotpot/GetJhotpotQuestions";
var JHOTPOT_QUESTIONS = BASE_URL + "/api/jhotpot/GetJhotpotThemeQuestionsNew";
var JhotpotThemeQuestionsForSpecialQuizes = BASE_URL + "/api/jhotpot/GetJhotpotThemeQuestionsForSpecialQuizes";
//var GetJhotpotThemeQuestionsNew = API_BASE_URL + "/api/jhotpot/GetJhotpotThemeQuestionsNew";


var JHOTPOT_ANSWER = BASE_URL + "/api/jhotpot/JhotpotAnswerWithTimeBkash";

var JHOTPOT_ANSWER_WC = BASE_URL + "/api/jhotpot/JhotpotAnswerWithTimeBkashWC";

var JHOTPOT_ANSWER_BreakTime = BASE_URL + "/api/jhotpot/BreaktimequizAnswer";



var JHOTPOT_BreakTime = BASE_URL + "/api/landingpage/BreaktimeQuizQuestion";






var JhotpotAnswerWithTimeBkashForSpecialQuiz = BASE_URL + "/api/jhotpot/JhotpotAnswerWithTimeBkashForSpecialQuiz";
var JhotpotAnswerWithTimeBkashForLiveVideoQuiz = BASE_URL + "/api/jhotpot/JhotpotAnswerWithTimeBkashForLiveVideoQuiz";

var MY_SCORE = API_BASE_URL + "/api/wifi/GetDailtsResult";
var PROFILE = API_BASE_URL + "/api/master/GetUserInfo";

var CLAIM_COIN = API_BASE_URL + "/api/master/ClaimMoney";

var LEADERBOARD = API_BASE_URL + "/api/wifi/GetDailtsResult";

var ENTERTAINMENT = API_BASE_SERVICE_URL + "/api/bdtube/categorywise";

var INS = API_BASE_URL + "/api/terms/DailyRulesChallange";
var VID = API_BASE_URL + "/api/Terms/video";


var BUY_COIN = API_BASE_URL + "/api/coin/BuyCoin";

//BuyLifeUsing coin

var BUY_LIFE = API_BASE_URL + "/api/Life/BuyLifeUsingCoin";

//BKASH
var bkash = BKASHOLD + "/api/QuizstarSubscription/BuyOnDemand";
var bkashCheck = BKASHOLD + "/api/CheckPayment/Get";

//BkashSub

var bsub = BKASH + "/api/GenericSub/buy";

//

//var firebaseConfig = {
//    apiKey: "AIzaSyClHVv0oOa7IoLV2P8JJeIdM_kFkso4eFU",
//    authDomain: "playnwin-ffa32.firebaseapp.com",
//    databaseURL: "https://playnwin-ffa32.firebaseio.com",
//    projectId: "playnwin-ffa32",
//    storageBucket: "playnwin-ffa32.appspot.com",
//    messagingSenderId: "475972065411",
//    appId: "1:475972065411:web:ee0f1c8a931946b06523b6"
//};


//// Initialize Firebase
//firebase.initializeApp(firebaseConfig);