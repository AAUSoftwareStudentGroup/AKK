function NavigationService() {
    this.back = function() { window.history.back(); };

    this.getParameters = function() {
        try {
            var parameterStrings = window.location.search.split("?")[1].split("&");
            var parameters = {};
            
            for(var i = 0; i < parameterStrings.length; i++)
            {
                var keyValue = parameterStrings[i].split("=");
                parameters[decodeURIComponent(keyValue[0])] = decodeURIComponent(keyValue[1]);
            }
            
            return parameters;            
        }
        catch (error)
        {
            return [];
        }
    }
    this.toEditRoute = function(routeId) {
        window.location = "edit-route?routeId=" + encodeURIComponent(routeId);
    };
    this.toRouteInfo = function(routeId) {
        window.location = "route-info?routeId=" + encodeURIComponent(routeId);
    };
    this.toNewRoute = function() {
        window.location = "new-route";
    };
    this.toRoutes = function() {
        window.location.href = "/";
    };
    this.toLogin = function(target) {
        if (target) {
            target = "?target=" + encodeURIComponent(target);
        } else {
            target = "";
        }
        var url = "login" + target;
        window.location = url;
    };
    this.toRegister = function(target) {
        window.location = "register?target=" + encodeURIComponent(target);
    };
    this.to = function(target) {
        window.location = target;
    }
}