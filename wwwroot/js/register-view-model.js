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

function RegisterViewModel(client, navigationService) {
    var self = this
    this.navigationService = navigationService;
    
    this.init = function () {
        var getTarget = FindGetParam("target");
        var getUsername = FindGetParam("username");
        self.target = (getTarget == null ? self.target : getTarget);
        self.username = (getUsername == null ? self.username : getUsername);
        self.trigger("ContentUpdated");
    };
    
    this.target = "/";
    this.fullName = "";
    this.username = "";
    this.password = "";

    this.changeFullName = function (fullName) {
        self.fullName = fullName;
    };

    this.changeUsername = function (username) {
        self.username = username;
    };

    this.changePassword = function (password) {
        self.password = password;
    };

    this.register = function () {
        console.log("GoGo Dr. Register!!");
        if(false) { // on success
            window.location = target;
        }
    };
}
RegisterViewModel.prototype = new EventNotifier();