function LogInViewModel(client, navigationService, cookieService) {
    var self = this;
    this.client = client;
    this.navigationService = navigationService;
    this.cookieService = cookieService;
    
    this.target = "/";
    this.username = "";
    this.password = "";
    
    this.init = function () {
        var getTarget = navigationService.getParameters()["target"];
        self.target = (getTarget == undefined ? self.target : getTarget);
        self.trigger('loginChanged')
    };
    
    this.logIn = function () {
        self.client.members.logIn(self.username, self.password, function(response) {
            if (response.success) {
                navigationService.to(self.target);
            } else {
                $("#error-message").html(response.message).show();
            }
        });
    };

    this.register = function () {
        self.navigationService.toRegister(self.target);
    };

    this.changeUsername = function (username) {
        self.username = username;
    };

    this.changePassword = function (password) {
        self.password = password;
    };
}
LogInViewModel.prototype = new EventNotifier();