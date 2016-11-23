$.ajax({
  url: "js/eventnotifier.js",
  dataType: "script",
  async: false
});

function RouteInfoViewModel(client, navigationService) {
    var self = this
    this.navigationService = navigationService;
    this.init = function () {
        self.client.routes.getRoute(window.location.search.split("routeId=")[1],
            function (routeResponse) {
                if (routeResponse.success) {
                    self.route = routeResponse.data;
                    self.route.date = self.route.createdDate.split("T")[0].split("-").reverse().join("/");

                    self.client.routes.getImage(self.route.id, function(imageResponse) {
                        if (imageResponse.success) {
                            self.hasImage = true;
                            self.image = imageResponse.image;
                            self.HoldPositions = imageResponse.holds;
                        }
                        self.trigger("ContentUpdated");
                    });
                    
                }
            });
    };
    this.image = null;
    this.hasImage = false;
    this.HoldPositions = [];
    this.client = client;
    this.grade = null;

    this.route = null;
    this.editRoute = function () {
        if (self.route != null) {
            navigationService.toEditRoute(self.route.id);
        }
    };
    this.deleteRoute = function () {
        if (self.route != null && confirm("Do you really want to delete this route?")) {
            self.client.routes.deleteRoute(self.route.routeId,
                function (response) {
                    if (response.success) {
                        navigationService.back();
                    }
                });
        }
    };
}
RouteInfoViewModel.prototype = new EventNotifier();