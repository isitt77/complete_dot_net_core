// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//let styleLink = document.getElementById("dark-light-bootst-style");
let styleLinkRefAttribute = document.getElementById("dark-light-bootst-style").attributes[2];

let darkStyleUrl = "/lib/bootstrap/dist/css/dark/bootstrap.min.css";
let lightStyleUrl = "/lib/bootstrap/dist/css/light/bootstrap.min.css";

let getStyle = localStorage.getItem(styleLinkRefAttribute.name);
//console.log(`getStyle: ${getStyle}`);

let isDark = getStyle == darkStyleUrl;
//console.log(`isDark: ${isDark}`);

let lightDarkCheckbox = document.querySelector("input[name=lightDarkCheck]");

let label = lightDarkCheckbox.nextElementSibling;
//console.log(`label: ${label.innerHTML}`);


// SweetAlert Dark/Light variables
//let layoutHeadTag = document.getElementById("layoutHeadTag");

//let sweetAlertStyle = document.getElementById("dark-light-swal2-style");
//console.log(sweetAlertStyle);

let sweetAlertScript = document.getElementById("dark-light-swal2-script");
console.log(sweetAlertScript);

let sweetAlertScriptSrc = document.getElementById("dark-light-swal2-script").attributes[1];
console.log(sweetAlertScriptSrc);

let darkSweetAlertScriptSrc = "//cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.js";
let lightSweetAlertScriptSrc = "//cdn.jsdelivr.net/npm/sweetalert2@11";

let getSweetAlertScript = localStorage.getItem(sweetAlertScriptSrc.name);
let isDarkSweetAlert = getSweetAlertScript == darkSweetAlertScriptSrc;

SetTheme();

lightDarkCheckbox.addEventListener('change', LightDarkToggle);


function SetTheme() {
    if (isDark && isDarkSweetAlert) {
        lightDarkCheckbox.setAttribute("checked", "");
        //console.log("Checkbox is checked..");

        styleLinkRefAttribute.value = darkStyleUrl;
        let darkStyleValue = styleLinkRefAttribute.value;

        sweetAlertScriptSrc.value = darkSweetAlertScriptSrc;
        let darkSweetAlertScriptValue = sweetAlertScriptSrc.value;

        localStorage.setItem(styleLinkRefAttribute.name, darkStyleValue);
        localStorage.setItem(sweetAlertScriptSrc.name, darkSweetAlertScriptValue)

        label.innerHTML = "Dark";

        //console.log(`isDark: ${isDark}`);
    } else {
        lightDarkCheckbox.removeAttribute("checked");
        //console.log("Checkbox is not checked..");

        styleLinkRefAttribute.value = lightStyleUrl;
        let lightStyleValue = styleLinkRefAttribute.value;

        sweetAlertScriptSrc.value = lightSweetAlertScriptSrc;
        let lightSweetAlertScriptValue = sweetAlertScriptSrc.value;

        localStorage.setItem(styleLinkRefAttribute.name, lightStyleValue);
        localStorage.setItem(sweetAlertScriptSrc.name, lightSweetAlertScriptValue);

        label.innerHTML = "Light";

        //console.log(`isDark: ${isDark}`);
    }
}


function LightDarkToggle() {
    isDark = !isDark;
    isDarkSweetAlert = !isDarkSweetAlert;
    SetTheme();
}



//var checkbox = document.querySelector("input[name=checkbox]");

//checkbox.addEventListener('change', function () {
//    if (this.checked) {
//        console.log("Checkbox is checked..");
//    } else {
//        console.log("Checkbox is not checked..");
//    }
//});
