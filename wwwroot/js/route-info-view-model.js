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
                    self.trigger("ContentUpdated");
                }
            });
    };
    this.client = client;
    this.grade = null;
        
    this.routeImage = new Image();
    this.routeImage.src ="https://www.nasa.gov/sites/default/files/styles/image_card_4x3_ratio/public/thumbnails/image/leisa_christmas_false_color.png?itok=Jxf0IlS4";
    
    this.HoldPositions = [
        {
            x: 0.5,
            y: 0.5,
            r: 0.25
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