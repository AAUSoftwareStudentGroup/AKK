$.ajax({
    url: "js/eventnotifier.js",
    dataType: "script",
    async: false
});

function HeaderViewModel(client) {
    var self = this;
    this.client = client;
    this.isAuthenticated = false;
    this.isAdmin = false;


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
    this.init();
};

HeaderViewModel.prototype = new EventNotifier();