$.ajax({
  url: "js/eventnotifier.js",
  dataType: "script",
  async: false
});

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

function LogInViewModel(client, navigationService, cookieService) {
    var self = this;
    this.client = client;
    this.navigationService = navigationService;
    this.cookieService = cookieService;
    
    this.init = function () {
        var getTarget = FindGetParam("target");
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
            console.log(response);
            if (response.success) {
                if(response.data) {
                    self.cookieService.setToken(response.data);
                }
                self.navigationService.to(self.target);
            } else {
                $("#error-message").html(response.message).show();
            }
        });
    };
}
LogInViewModel.prototype = new EventNotifier();