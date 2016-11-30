function RouteInfoViewModel(client, navigationService, dialogService) {
    var self = this
    this.navigationService = navigationService;
    this.dialogService = dialogService;
    this.init = function () {
        self.client.routes.getRoute(navigationService.getParameters()["routeId"], function (routeResponse) {
            if (routeResponse.success) {
                    self.route = routeResponse.data;
                    self.route.date = self.route.createdDate.split("T")[0].split("-").reverse().join("/");
                    self.client.routes.getImage(self.route.id, function(imageResponse) {
                        if (imageResponse.success) {
                            self.hasImage = true;
                            self.route.image = new Image();
                            self.route.image.src = imageResponse.data.fileUrl;
                            self.HoldPositions = imageResponse.data.holds;
                            self.route.image.onload = function() {
                                self.trigger("cardUpdated");
                                self.trigger("commentsUpdated");
                            }
                        } else {
                            self.trigger("cardUpdated");
                            self.trigger("commentsUpdated");
                        }
                    });
                }
            }
        );
        self.client.members.getMemberInfo(function(response) {
            if(response.success) {
                self.isAuthed = true;
                self.member = response.data;
            }
        });
    };
    this.member = null;
    this.image = null;
    this.hasImage = false;
    this.HoldPositions = [];
    this.client = client;
    this.grade = null;
    this.route = null;
    this.isAuthed = false;
    
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
    this.addComment = function(form) {
        var fd = new FormData(form);
        this.client.routes.addComment(fd, self.route.id, function(response) {
            self.init();
        });
    }

    this.imageAdded = function() {
        this.trigger("imageUpdated");
    }

    this.removeComment = function (id, routeId) {
        if (!self.dialogService.confirm("Are you sure that you want to remove the comment?")) return;
        this.client.routes.removeComment(id, routeId, function(response) {
            if (response.success) {
                self.client.routes.getRoute(navigationService.getParameters()["routeId"], function (routeResponse) {
                    if (routeResponse.success) {
                        self.route.comments = routeResponse.data.comments;
                        self.trigger("commentsUpdated");
                    }
                });
            } else {
                self.trigger("Error", response.message);
            }
        });
    }
}

RouteInfoViewModel.prototype = new EventNotifier();