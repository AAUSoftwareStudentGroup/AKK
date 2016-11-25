$.ajax({
    url: "js/eventnotifier.js",
    dataType: "script",
    async: false
});

function HeaderViewModel(client, cookieService) {
    var self = this;
    this.client = client;
    this.cookieService = cookieService;
    this.isAuthenticated = false;
    this.isAdmin = false;


    this.init = function () {
        self.getRoles();
    }

    this.getRoles = function () {
        self.client.members.getRoles(self.cookieService.getToken(), function (response) {
            console.log(response);
            console.log(response.data.indexOf(1));
            if (response.success) {
                if (response.data && response.data.length > 0) {
                    if (response.data.indexOf(1) != -1) {
                        self.isAuthentication = true;
                    }
                    if (response.data.indexOf(2) != -1) {
                        self.isAdmin = true;
                    }
                }
            } else {
                $("#error-message").html(response.message).show();
            }
        });
    };
    this.init();
};