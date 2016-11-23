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
                    self.route.image = {};
                    self.route.image.width = 470;
                    self.route.image.height = 627;
                    
                    self.route.date = self.route.createdDate.split("T")[0].split("-").reverse().join("/");
                    self.trigger("ContentUpdated");
                }
            });
    };
    this.client = client;
    this.grade = null;
        
    this.routeImage = new Image();
    this.routeImage.src ="vaeg.jpg";
    
    this.HoldPositions = [
        {
            x: 0.42,
            y: 0.45,
            r: 0.05
        },
        {
            x: 0.65,
            y: 0.455,
            r: 0.05
        },
        {
            x: 0.49,
            y: 0.65,
            r: 0.05
        }
    ];

    this.editRoute = function () {
        if (self.route != null) {
            navigationService.location(self.route.routeId);
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