function TestNavigationService() {
    this.back = function () { window.history.back(); };

    this.getParameters = function () {
        return { "routeId": "5176e2ed-9527-470a-a0a2-f085d12b71ac" };
    }
    this.toEditRoute = function (routeId) {
        if (routeId == null) {
            return false;
        }
        return true;
    };
    this.toRouteInfo = function (routeId) {
        if (routeId == null) {
            return false;
        }
        return true;
    };
    this.toNewRoute = function () {
        return true;
    };
    this.toRoutes = function () {
        return true;
    };
    this.toLogin = function () {
        return true;
    };
    this.toRegister = function (target) {
        return true;
    };
    this.to = function (target) {
        if (target == null) {
            return true;
        }
        return false;
    }
}