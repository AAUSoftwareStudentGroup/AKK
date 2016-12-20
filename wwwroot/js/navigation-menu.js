$(document).ready(function () {
    $(document.body).on('click',".nav-menu-toggle" , function () {
        var navMenuToggle = $(".nav-menu-toggle");
        var navMenu = $(".nav-menu");
        navMenuToggle.toggleClass('open');
        navMenu.toggleClass('open');
    });
    
    $(document.body).on('click', ".nav-menu-closer", function () {
        var navMenuToggle = $(".nav-menu-toggle");
        var navMenu = $(".nav-menu");
        navMenuToggle.removeClass('open');
        navMenu.removeClass('open');
    });
});