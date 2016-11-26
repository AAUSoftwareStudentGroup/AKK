$(document).ready(function () {
    var navbar = document.getElementById("navbar");
    var navigationContent = document.getElementById("navigationid");

    navbar.onclick = function (e) {
        if (navigationContent.style.display == "none") {
            navigationContent.style.display = "block";
        } else {
            navigationContent.style.display = "none";
        }
    }
});