// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



//function MakeDark() {

//}

//function MakeLight() {

//}

//let styleLink = document.getElementById("dark-light-bootst-style");

//let styleLinkRefAttribute = document.getElementById("dark-light-bootst-style").attributes[2];
//console.log(styleLinkRefAttribute.value);
//console.log(`Key ${styleLinkRefAttribute.name}`);

//styleLink.addEventListener("change" function (LightDarkToggle) {

//});

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

//console.log(`styleLink: ${styleLink}`);
//console.log(`Initial style Key: ${styleLinkRefAttribute.name}`);
//console.log(`Initial style Value: ${styleLinkRefAttribute.value}`);
//console.log(styleLinkRefAttributes);

let lightDarkCheckbox = document.querySelector("input[name=lightDarkCheck]");

let isChecked = lightDarkCheckbox.checked;

//let isDark = true;

//lightDarkCheckbox.addEventListener('change', SetTheme);
//console.log(`isDark: ${isDark}`);

//isDark = localStorage.getItem(styleLinkRefAttribute.value) == darkStyleValue ? false : true;
//SetTheme();

//isDark = localStorage.getItem(styleLinkRefAttribute.name) == "false" ? false : true;
//SetTheme();



//if (localStorage.getItem(styleLinkRefAttribute.value) == darkStyleUrl) {
//    isChecked = true;
//}
//else {
//    isChecked = false;
//}

SetTheme();

lightDarkCheckbox.addEventListener('change', LightDarkToggle);


function SetTheme() {
    //localStorage.setItem(styleLinkRefAttribute.name, isDark ? "false" : "true");
    if (isDark) {
        lightDarkCheckbox.setAttribute("checked", "");
        console.log("Checkbox is checked..");

        styleLinkRefAttribute.value = darkStyleUrl;
        let darkStyleValue = styleLinkRefAttribute.value;

        localStorage.setItem(styleLinkRefAttribute.name, darkStyleValue);

        console.log(`isDark: ${isDark}`);
        //isDark = true;
        //let getDarkStyle = localStorage.getItem(styleLinkRefAttribute.name);
        //console.log(`getDarkStyle: ${getDarkStyle}`);
        //console.log(`Get Local Storage: ${getDarkStyle}`);

        //return getDarkStyle;
    } else {
        lightDarkCheckbox.removeAttribute("checked");
        console.log("Checkbox is not checked..");

        styleLinkRefAttribute.value = lightStyleUrl;
        let lightStyleValue = styleLinkRefAttribute.value;

        localStorage.setItem(styleLinkRefAttribute.name, lightStyleValue);

        console.log(`isDark: ${isDark}`);
        //isDark = false;
        //let getLightStyle = localStorage.getItem(styleLinkRefAttribute.name);
        //console.log(`getLightStyle: ${getLightStyle}`);
        //console.log(`Get Local Storage: ${getLightStyle}`);

        //return getLightStyle;
    }

    //console.log(`Get Style: ${getDarkStyle} or ${getLightStyle}`);
    //localStorage.getItem(styleLinkRefAttribute.name);
    //console.log(`isChecked: ${isChecked}`);
    //console.log(`isDark: ${isDark}`);
    //console.log(`localStorage: ${localStorage.getItem(styleLinkRefAttribute.name)}`);
    //console.log(`After style Key: ${styleLinkRefAttribute.name}`);
    //console.log(`After style Value: ${styleLinkRefAttribute.value}`);
    ////window.localStorage.getItem(styleLinkRefAttribute.name);
    //console.log(`localStorage length: ${localStorage.length}`);
    //console.log(`localStorage key: ${localStorage.key(0)}`);
    //console.log(styleLink);
    //console.log(styleLinkRefAttributes);
    //console.log(`localStorage outside SetTheme if/else: ${localStorage.getItem(styleLinkRefAttribute.name)}`)
    //let setStyle = localStorage.getItem(styleLinkRefAttribute.name);
    //console.log(`setStyle: ${setStyle}`);
    //return setStyle;
}

//let getStyle = localStorage.getItem(styleLinkRefAttribute.name);
//console.log(`getStyle: ${getStyle}`);

//console.log(`Outside SetTheme style Key: ${styleLinkRefAttribute.name}`);
//console.log(`Outside SetTheme style Value: ${styleLinkRefAttribute.value}`);

//console.log(`Get Style: ${getDarkStyle} or ${getLightStyle}`);

//console.log(`localStorage outside SetTheme: ${localStorage.getItem(styleLinkRefAttribute.name)}`)


// This method basically works. Still need to integrate it.
//if (getStyle == lightStyleUrl) {
//    isChecked = true;
//    console.log(`getStyle localStorage in if: ${getStyle}`);
//    console.log(`getStyle == lightStyleUrl: ${getStyle == lightStyleUrl}`);
//}
//else {
//    isChecked = false;
//    console.log(`getStyle localStorage in else: ${getStyle}`);
//    console.log(`getStyle == lightStyleUrl: ${getStyle == lightStyleUrl}`);
//}


//isChecked = true;
function LightDarkToggle() {
    //isChecked = !isChecked;
    isDark = !isDark;
    SetTheme();
}



//function LightDarkToggle() {
//    // get element by id

//    let settingSwitch = document.getElementById("flexSwitchCheckDefault");
//    console.log(settingSwitch.id);

//    let label = settingSwitch.nextElementSibling;
//    console.log(label.innerHTML);

//    let isChecked = settingSwitch.hasAttribute("checked");
//    console.log(isChecked);

//    let styleLink = document.getElementById("dark-light-bootst-style");

//    let styleLinkRefAttribute = document.getElementById("dark-light-bootst-style").attributes[2];
//    console.log(styleLinkRefAttribute.value);
//    console.log(`Key ${styleLinkRefAttribute.name}`);

//    // Insert query selector
//    //settingSwitch.querySelector("change", );


//    //

//    if (isChecked) {
//        console.log("Light mode active.");
//        settingSwitch.removeAttribute("checked");
//        label.innerHTML = "Light";
//        //let lightStyle = styleLinkRefAttribute.value = "/lib/bootstrap/dist/css/light/bootstrap.min.css";
//        //let setLightStyle = localStorage.setItem(styleLinkRefAttribute.name, lightStyle);
//        //return setLightStyle;

//    }
//    else {
//        console.log("Dark mode active.");
//        settingSwitch.setAttribute("checked", "");
//        label.innerHTML = "Dark";
//        //let darkStyle = styleLinkRefAttribute.value = "/lib/bootstrap/dist/css/dark/bootstrap.min.css";
//        //let setDarkStyle = localStorage.setItem(styleLinkRefAttribute.name, darkStyle);
//        //return setDarkStyle;

//    }
//    console.log(`isChecked value after if/else: ${isChecked}`);
//    return isChecked;
//}



//$(".change").on("click", function () {
//    if ($("body").hasClass("dark")) {
//        $("body").removeClass("dark");
//        $(".change").text("OFF");
//    } else {
//        $("body").addClass("dark");
//        $(".change").text("ON");
//    }
//});


//from W3 Scools (Notice the classlist)
//function myFunction() {
//    var element = document.body;
//    element.classList.toggle("dark-mode");
//}