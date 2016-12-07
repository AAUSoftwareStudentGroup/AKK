function CookieService() {
    var self = this;

    //Gets the token by its key
    this.get = function(key) {
        var value = "; " + document.cookie;
        var parts = value.split("; " + key + "=");
        if (parts.length == 2) return parts.pop().split(";").shift();
    }

    //Saves a cookie with the token value from the server, and call that token "token"
    this.set = function(key, val) {
        document.cookie = key + "=" + val + "; expires=Fri, 31 Dec 2035 23:59:59 GMT";
    }

    this.expire = function(key) {
        document.cookie = key + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC";
    }

    this.getToken = function() {
        return self.get("token");
    };

    this.setToken = function(token) {
        self.set("token", token);
    };

    this.expireToken = function() {
        self.expire("token");
    }
}