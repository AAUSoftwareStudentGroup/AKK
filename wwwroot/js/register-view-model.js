function RegisterViewModel(client, navigationService, cookieService, dialogService) {
    var self = this
    this.navigationService = navigationService;
    this.cookieService = cookieService;
    this.dialogService = dialogService;

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
            self.dialogService.showError("The passwords you entered are not the same");
            return;
        }
        client.members.register(self.fullName, self.username, self.password, function(response) {
            if (response.success) {
                if(response.data) {
                    cookieService.setToken(response.data);
                }
                self.navigationService.to(self.target);
            } else {
                self.dialogService.showError(response.message);
            }
        });        
    };
}

RegisterViewModel.prototype = new EventNotifier();