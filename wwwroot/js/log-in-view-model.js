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
        if (getTarget) {
            self.trigger("Info", "You need to be logged in to use this feature");
        }
        self.target = (getTarget == undefined ? self.target : getTarget);
        self.trigger('loginChanged')
    };
    
    this.logIn = function () {
        self.client.members.logIn(self.username, self.password, function(response) {
            if (response.success) {
                navigationService.to(self.target);
            } else {
                self.trigger("Error", response.message);
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