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
console.log(`Initial style Key: ${styleLinkRefAttribute.name}`);
console.log(`Initial style Value: ${styleLinkRefAttribute.value}`);
console.log(styleLinkRefAttributes);

let lightDarkCheckbox = document.querySelector("input[name=lightDarkCheck]");

lightDarkCheckbox.addEventListener('change', function () {
    if (this.checked) {
        console.log("Checkbox is checked..");
        //window.localStorage.getItem(styleLinkRefAttribute.name);
        styleLinkRefAttribute.value = "/lib/bootstrap/dist/css/dark/bootstrap.min.css";
        let darkStyleValue = styleLinkRefAttribute.value;
        window.localStorage.setItem(styleLinkRefAttribute.name, darkStyleValue);
        let getDarkStyle = window.localStorage.getItem(styleLinkRefAttribute.name);
        return getDarkStyle;
    } else {
        console.log("Checkbox is not checked..");
        //window.localStorage.getItem(styleLinkRefAttribute.name);
        styleLinkRefAttribute.value = "/lib/bootstrap/dist/css/light/bootstrap.min.css";
        let lightStyleValue = styleLinkRefAttribute.value;
        window.localStorage.setItem(styleLinkRefAttribute.name, lightStyleValue);
        let getLightStyle = window.localStorage.getItem(styleLinkRefAttribute.name);
        return getLightStyle;
    }
    console.log(`After style Key: ${styleLinkRefAttribute.name}`);
    console.log(`After style Value: ${styleLinkRefAttribute.value}`);
    //window.localStorage.getItem(styleLinkRefAttribute.name);

});



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