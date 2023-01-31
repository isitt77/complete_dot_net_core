// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function LightDarkToggle() {
    // get element by id
    let settingSwitch = document.getElementById("flexSwitchCheckDefault");
    console.log(settingSwitch.id);

    let isChecked = settingSwitch.hasAttribute("checked");
    console.log(isChecked);

    if (isChecked) {
        console.log("Light mode active.");
        settingSwitch.removeAttribute("checked");
    }
    else {
        console.log("Dark mode active.");
        settingSwitch.setAttribute("checked", "");
    }


}

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