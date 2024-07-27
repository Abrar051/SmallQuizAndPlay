// Import and configure the Firebase SDK
// These scripts are made available when the app is served or deployed on Firebase Hosting
// If you do not serve/host your project using Firebase Hosting see https://firebase.google.com/docs/web/setup
//importScripts('https://www.gstatic.com/firebasejs/6.4.2/firebase-app.js');
//importScripts('https://www.gstatic.com/firebasejs/6.4.2/firebase-messaging.js');
//importScripts('https://www.gstatic.com/firebasejs/7.17.1/firebase-app.js');
//importScripts('https://www.gstatic.com/firebasejs/7.17.1/firebase-messaging.js');
importScripts('https://www.gstatic.com/firebasejs/8.10.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.10.1/firebase-messaging.js');


//firebase.initializeApp({
//    'messagingSenderId': '240808501219'
//});
//const messaging = firebase.messaging();

///**
// * Here is is the code snippet to initialize Firebase Messaging in the Service
// * Worker when your app is not hosted on Firebase Hosting.

// // [START initialize_firebase_in_sw]
// // Give the service worker access to Firebase Messaging.
// // Note that you can only use Firebase Messaging here, other Firebase libraries
// // are not available in the service worker.
 

// // Initialize the Firebase app in the service worker by passing in the
// // messagingSenderId.
 

// // Retrieve an instance of Firebase Messaging so that it can handle background
// // messages.
// const messaging = firebase.messaging();
// // [END initialize_firebase_in_sw]
// **/


//// If you would like to customize notifications that are received in the
//// background (Web app is closed or not in browser focus) then you should
//// implement this optional method.
//// [START background_handler]
//messaging.setBackgroundMessageHandler(function (payload) {
//    console.log('[firebase-messaging-sw.js] Received background message ', payload);
//    // Customize notification here
//    const notificationTitle = 'Background Message Title';
//    const notificationOptions = {
//        body: 'Background Message body.',
//        icon: '/firebase-logo.png'
//    };

//    return self.registration.showNotification(notificationTitle,
//        notificationOptions);
//});
//// [END background_handler]


firebase.initializeApp({
    apiKey: "AIzaSyCgxfNVhoQD4IHFiaeKd0qP67goFbBlPUE",
    authDomain: "push-notification-test-77c9d.firebaseapp.com",
    projectId: "push-notification-test-77c9d",
    storageBucket: "push-notification-test-77c9d.appspot.com",
    messagingSenderId: "575417553672",
    appId: "1:575417553672:web:71da03329f96426b8492bd",
    measurementId: "G-1EXXNFN9L5"
});

// Retrieve an instance of Firebase Messaging so that it can handle background
// messages.
const messaging = firebase.messaging();
  // [END messaging_init_in_sw]


function onBackgroundMessage() {
    const messaging = firebase.messaging();

    // [START messaging_on_background_message]
    messaging.onBackgroundMessage((payload) => {
        console.log('[firebase-messaging-sw.js] Received background message ', payload);
        // Customize notification here
        const notificationTitle = 'Background Message Title';
        const notificationOptions = {
            body: 'Background Message body.',
            icon: '/firebase-logo.png'
        };

        self.registration.showNotification(notificationTitle,
            notificationOptions);

        //self.addEventListener('notificationclick', function (event) {
        //    event.notification.close();
        //    //if (event.action === 'archive') {
        //    //    // User selected the Archive action.
        //    //    archiveEmail();
        //    //} else {
        //    //    // User selected (e.g., clicked in) the main body of notification.
        //    //    clients.openWindow('/inbox');
        //    //}
        //    clients.openWindow('/Leaderboard');
        //    //window.location.href = "../LandingPage/Leaderboard";
        //}, false);
    });
}