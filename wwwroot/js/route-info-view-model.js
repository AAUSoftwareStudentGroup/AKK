function RouteInfoViewModel(client, navigationService, dialogService) {
    var self = this
    this.navigationService = navigationService;
    this.dialogService = dialogService;

    this.image = null;
    this.hasImage = false;
    this.HoldPositions = [];
    this.client = client;
    this.grade = null;
    this.route = null;
    this.isAuthed = false;
    this.filledStars;
    this.emptyStars;

    this.init = function () {
        self.client.routes.getRoute(navigationService.getParameters()["routeId"], function (routeResponse) {
            if (routeResponse.success) {
                    self.route = routeResponse.data;
                    self.route.date = self.route.createdDate.split("T")[0].split("-").reverse().join("/");
                    self.parseRating();
                    self.downloadImage();
                }
            }
        );
        self.client.members.getMemberInfo(function(response) {
            if(response.success) {
                self.isAuthed = true;
            }
        });
    };
    
    this.parseRating = function() {
        self.route.rating = 3.74;
        var temp = Math.round(self.route.rating);
        self.filledStars = temp;
        self.emptyStars = 5 - temp;
    }

    this.downloadImage = function() {
        self.client.routes.getImage(self.route.id, function(imageResponse) {
            if (imageResponse.success) {
                self.hasImage = true;
                self.route.image = new Image();
                self.route.image.src = imageResponse.data.fileUrl;
                self.HoldPositions = imageResponse.data.holds;
                self.route.image.onload = function() {
                    self.trigger("cardUpdated");
                    self.trigger("betasUpdated");
                }
            } else {
                self.trigger("cardUpdated");
                self.trigger("betasUpdated");
            }
        });
    }

    this.editRoute = function () {
        if (self.route != null) {
            navigationService.toEditRoute(self.route.id);
        }
    };

    this.deleteRoute = function () {
        if (self.route != null && self.dialogService.confirm("Do you really want to delete this route?")) {
            self.client.routes.deleteRoute(self.route.id, function (response) {
                if (response.success) {
                    navigationService.toRoutes();
                }
            });
        }
    };

    this.addBeta = function(form) {
        var fd = new FormData(form);
        this.client.routes.addBeta(fd, self.route.id, function(response) {
            self.init();
        });
    }
}

RouteInfoViewModel.prototype = new EventNotifier();