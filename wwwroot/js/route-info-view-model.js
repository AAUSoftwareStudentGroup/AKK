function RouteInfoViewModel(client, changed, navigationService) {
    var self = this
    this.navigationService = navigationService;
    this.init = function () {
        self.client.routes.getRoute(window.location.search.split("routeId=")[1],
            function (routeResponse) {
                if (routeResponse.success) {
                    self.route = routeResponse.data;
                    self.route.date = self.route.createdDate.split("T")[0].split("-").reverse().join("/");
                    self.route.colorOfHolds = self.route.colorOfHolds;
                    self.route.grade = self.route.grade;
                    self.client.sections.getSection(self.route.sectionId,
                        function (sectionResponse) {
                            if (sectionResponse.success) {
                                self.route.sectionName = sectionResponse.data.name;
                                self.changed();
                            }
                        });
                }
            });
    };
    this.client = client;
    this.changed = changed;
    this.grade = null;
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
    this.init();
}
