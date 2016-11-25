function FindGetParam(param) {
    var result = null,
        tmp = [];
    var items = location.search.substr(1).split("&");
    for (var index = 0; index < items.length; index++) {
        tmp = items[index].split("=");
        if (tmp[0] === param) result = decodeURIComponent(tmp[1]);
    }
    return result;
}

function RegisterViewModel(client, navigationService, cookieService) {
    var self = this
    this.navigationService = navigationService;
    this.cookieService = cookieService;
    
    this.init = function () {
        var getTarget = FindGetParam("target");
        var getUsername = FindGetParam("username");
        self.target = (getTarget == null ? self.target : getTarget);
        self.username = (getUsername == null ? self.username : getUsername);
    };
    
    this.target = "/";
    this.fullName = "";
    this.username = "";
    this.password = "";
    this.passwordConfirm = "";

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
            console.log("invalid password match")
            $("#error-message").html("The passwords you entered are not the same!").show();
            return;
        }
        client.members.register(self.fullName, self.username, self.password, function(response) {
            console.log(response);
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