// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/10.13.1/firebase-app.js";
import { getAuth, createUserWithEmailAndPassword} from "https://www.gstatic.com/firebasejs/10.13.1/firebase-auth.js";
import { getDatabase, ref, set, get, child} from "https://www.gstatic.com/firebasejs/10.13.1/firebase-database.js";
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
const database = getDatabase(app);

    const signUp = document.getElementById("btnSignUp");
    signUp.addEventListener("click", function(event) {
    event.preventDefault()

    const nameRegister = document.getElementById("nameRe").value;
    const emailRegister = document.getElementById("emailRe").value;
    const passwordRegister = document.getElementById("passwordRe").value;

    createUserWithEmailAndPassword(auth, emailRegister, passwordRegister)
    .then((userCredential) => {
        const user = userCredential.user;
        set(ref(database, 'user/' + nameRegister),
        {
        User: nameRegister,
        Email: emailRegister,
        Password: passwordRegister
        })
        alert("Dang ky thanh cong");
        window.location.href = "../user/Login.html";
    })
    .catch((error)=>{
        const errorCode = error.code;
        const errorMessage = error.message;
        alert(errorMessage)
    })
})
