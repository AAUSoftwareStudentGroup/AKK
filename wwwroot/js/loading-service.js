function LoadingService() {
    this.load = function() {
        $(".header-loading-bar").removeClass("hidden");
    }
    this.stopLoad = function() {
        $(".header-loading-bar").addClass("hidden");
    }
}