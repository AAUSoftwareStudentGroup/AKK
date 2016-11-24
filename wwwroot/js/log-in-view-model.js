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

function LogInViewModel(client, navigationService) {
    var self = this
    this.navigationService = navigationService;
    
    this.init = function () {
        var getTarget = FindGetParam("target");
        self.target = (getTarget == null ? self.target : getTarget);
        self.trigger("ContentUpdated");
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
        navigationService.toRegister(self.target, self.username);
    };
    
    this.logIn = function () {
        console.log("GoGo Dr. LOGIN!!");
        if(false) { // on success
            window.location = target;
        }
    };
}
LogInViewModel.prototype = new EventNotifier();