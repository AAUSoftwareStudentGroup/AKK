function LogInViewModel(client, navigationService, cookieService) {
    var self = this;
    this.client = client;
    this.navigationService = navigationService;
    this.cookieService = cookieService;
    
    this.init = function () {
        var getTarget = navigationService.getParameters()["target"];
        self.target = (getTarget == null ? self.target : getTarget);
    };
    
    this.target = "/";
    this.username = "";
    this.password = "";

    this.changeUsername = function (username) {
        self.username = username;
    };

    this.changePassword = function (password) {
        self.password = password;
    };


    this.register = function () {
        self.navigationService.toRegister(self.target, self.username);
    };
    
    this.logIn = function () {
        self.client.members.logIn
        self.client.members.logIn(self.username, self.password, function(response) {
            if (response.success) {
                if(response.data) {
                    self.cookieService.setToken(response.data);
                }
                navigationService.to(self.target);
            } else {
                $("#error-message").html(response.message).show();
            }
        });
    };
}
LogInViewModel.prototype = new EventNotifier();