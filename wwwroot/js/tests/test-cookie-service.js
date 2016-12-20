function TestCookieService() {
    var self = this;

    this.token = null;

    this.getToken = function() {
        return token;
    };

    this.setToken = function(token) {
        token = token;
    };

    this.expireToken = function() {
        token = null;
    }
}