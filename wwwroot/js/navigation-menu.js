$(document).ready(function () {
    var navMenuToggle = $(".nav-menu-toggle");
    var navMenu = $(".nav-menu");

    $(".nav-menu-toggle").on('click', function () {
        navMenuToggle.toggleClass('open');
        navMenu.toggleClass('open');
    });
    
    $(".nav-menu-closer").on('click', function () {
        navMenuToggle.removeClass('open');
        navMenu.removeClass('open');
    });
});