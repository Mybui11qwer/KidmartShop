// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/10.13.1/firebase-app.js";
import { getAuth, signInWithEmailAndPassword} from "https://www.gstatic.com/firebasejs/10.13.1/firebase-auth.js";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
    apiKey: "AIzaSyBhjmZZczZcSEtjywL97VU1A78yeMU_e-c",
    authDomain: "child-cloth.firebaseapp.com",
    databaseURL: "https://child-cloth-default-rtdb.firebaseio.com",
    projectId: "child-cloth",
    storageBucket: "child-cloth.appspot.com",
    messagingSenderId: "112408298170",
    appId: "1:112408298170:web:360049c71fabc8cc4a7883",
    measurementId: "G-WDKZD6NZK0"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const auth = getAuth(app);

    const signUp = document.getElementById("btnSignIn");
    signUp.addEventListener("click", function(event) {
    event.preventDefault()

    const emailLogin = document.getElementById("emailLog").value;
    const passwordLogin = document.getElementById("passwordLog").value;

    signInWithEmailAndPassword(auth, emailLogin, passwordLogin)
    .then((userCredential) => {
        const user = userCredential.user;
        window.location.href = "../user/Index.html";
    })
    .catch((error)=>{
        const errorCode = error.code;
        const errorMessage = error.message;
        alert(errorMessage)
    })
})
