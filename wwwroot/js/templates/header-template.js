$(document).ready(function () {
    var navbar = document.getElementById("navbar");
    var navigationContent = document.getElementById("navigationid");
    var navigationCloser = document.getElementById("navigationcloserid");

    navbar.onclick = function (e) {
        if(navigationContent.className != "navigation-visible")
        {
            navigationContent.className = "navigation-visible";
            navigationCloser.style.display = "block";
        }
        else
        {
            navigationContent.className = "navigation-hidden";
            navigationCloser.style.display = "none";
        }
    }

    navigationCloser.onclick = function (e) {
        navigationContent.className = "navigation-hidden";
        navigationCloser.style.display = "none";
    }

});