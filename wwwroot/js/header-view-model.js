function HeaderViewModel(title, client, returnPath) {
    var self = this;
    this.title = title;
    this.client = client;
    this.isAuthenticated = false;
    this.isAdmin = false;
    this.returnPath = returnPath || null;
    this.cookieService = new CookieService();

    this.init = function () {
        self.getRoles();
    }

    //Gets the roles of the current user, so we know what to render in the navigation menu
    this.getRoles = function () {
        client.members.getMemberInfo(function(response) {
            if (response.success) {
                self.isAdmin = response.data.isAdmin;
                self.isAuthenticated = true;
            }
            self.trigger("headerUpdated");
        });
    };
};

HeaderViewModel.prototype = new EventNotifier();