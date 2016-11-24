function NavigationService() {
    this.back = function() { window.history.back(); };
    this.toEditRoute = function(routeId) {
        window.location = "edit-route?routeId=" + routeId;
    };
    this.toRouteInfo = function(routeId) {
        window.location = "route-info?routeId=" + routeId;
    };
    this.toNewRoute = function() {
        window.location = "new-route";
    };
    this.toRoutes = function() {
        window.location = "routes";
    };
    this.toLogin = function() {
        window.location = "login";
    };
    this.toRegister = function(target, username) {
        window.location = "register?target=" + target + "&username=" + username;
    };
}