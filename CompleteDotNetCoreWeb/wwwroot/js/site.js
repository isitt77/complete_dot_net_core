// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



//var checkbox = document.querySelector("input[name=checkbox]");

//checkbox.addEventListener('change', function () {
//    if (this.checked) {
//        console.log("Checkbox is checked..");
//    } else {
//        console.log("Checkbox is not checked..");
//    }
//});

let styleLink = document.getElementById("dark-light-bootst-style");
let styleLinkRefAttribute = document.getElementById("dark-light-bootst-style").attributes[2];
let styleLinkRefAttributes = document.getElementById("dark-light-bootst-style").attributes;

let darkStyleUrl = "/lib/bootstrap/dist/css/dark/bootstrap.min.css";
let lightStyleUrl = "/lib/bootstrap/dist/css/light/bootstrap.min.css";

let getStyle = localStorage.getItem(styleLinkRefAttribute.name);
console.log(`getStyle: ${getStyle}`);

let isDark = getStyle == darkStyleUrl;
console.log(`isDark: ${isDark}`);

let lightDarkCheckbox = document.querySelector("input[name=lightDarkCheck]");

let isChecked = lightDarkCheckbox.checked;


SetTheme();

lightDarkCheckbox.addEventListener('change', LightDarkToggle);


function SetTheme() {
    if (isDark) {
        lightDarkCheckbox.setAttribute("checked", "");
        console.log("Checkbox is checked..");

        styleLinkRefAttribute.value = darkStyleUrl;
        let darkStyleValue = styleLinkRefAttribute.value;

        localStorage.setItem(styleLinkRefAttribute.name, darkStyleValue);

        console.log(`isDark: ${isDark}`);
    } else {
        lightDarkCheckbox.removeAttribute("checked");
        console.log("Checkbox is not checked..");

        styleLinkRefAttribute.value = lightStyleUrl;
        let lightStyleValue = styleLinkRefAttribute.value;

        localStorage.setItem(styleLinkRefAttribute.name, lightStyleValue);

        console.log(`isDark: ${isDark}`);
    }
}


function LightDarkToggle() {
    isDark = !isDark;
    SetTheme();
}

