$.ajax({
    url: "js/eventnotifier.js",
    dataType: "script",
    async: false
});

function HeaderViewModel(title, client, cookieService) {
    var self = this;
    this.title = title;
    this.client = client;
    this.isAuthenticated = false;
    this.isAdmin = false;
    this.CookieService = cookieService;

    this.init = function () {
        self.getRoles();
    }

    this.getRoles = function () {
        client.members.getMemberInfo(function(response) {
            console.log(response);
            if (response.success) {
                self.isAdmin = response.data.isAdmin;
                self.isAuthenticated = true;
            }
            self.trigger("headerUpdated");
        });
    };
};

HeaderViewModel.prototype = new EventNotifier();