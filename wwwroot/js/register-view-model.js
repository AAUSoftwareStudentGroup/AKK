function RegisterViewModel(client, navigationService, cookieService) {
    var self = this
    this.navigationService = navigationService;
    this.cookieService = cookieService;
    this.target = "/";
    this.fullName = "";
    this.username = "";
    this.password = "";
    this.passwordConfirm = "";
    
    this.init = function () {
        var parameters = navigationService.getParameters();
        self.target = (parameters["target"] == undefined ? self.target : parameters["target"]);
        self.trigger("registerChanged");
    };

    this.changeFullName = function (fullName) {
        self.fullName = fullName;
    };

    this.changeUsername = function (username) {
        self.username = username;
    };

    this.changePassword = function (password) {
        self.password = password;
    };

    this.changePasswordconfirm = function (password) {
        self.passwordConfirm = password;
    }

    this.register = function () {
        if(self.password != self.passwordConfirm) {
            $("#error-message").html("The passwords you entered are not the same!").show();
            return;
        }
        client.members.register(self.fullName, self.username, self.password, function(response) {
            if (response.success) {
                if(response.data) {
                    cookieService.setToken(response.data);
                }
                self.navigationService.to(self.target);
            } else {
                $("#error-message").html(response.message).show();
            }
        });        
    };
}

RegisterViewModel.prototype = new EventNotifier();