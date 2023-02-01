// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function LightDarkToggle() {
    // get element by id

    let settingSwitch = document.getElementById("flexSwitchCheckDefault");
    console.log(settingSwitch.id);

    let label = settingSwitch.nextElementSibling;
    console.log(label.innerHTML);

    let isChecked = settingSwitch.hasAttribute("checked");
    console.log(isChecked);

    let styleLink = document.getElementById("dark-light-bootst-style").attributes[2];
    console.log(styleLink.value);
    console.log(`Key ${styleLink.name}`);



    if (isChecked) {
        console.log("Light mode active.");
        settingSwitch.removeAttribute("checked");
        label.innerHTML = "Light";
        styleLink.value = "/lib/bootstrap/dist/css/light/bootstrap.min.css";
        localStorage.setItem(styleLink.name, styleLink.value);
    }
    else {
        console.log("Dark mode active.");
        settingSwitch.setAttribute("checked", "");
        label.innerHTML = "Dark";
        styleLink.value = "/lib/bootstrap/dist/css/dark/bootstrap.min.css";
        localStorage.setItem(styleLink.name, styleLink.value);
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