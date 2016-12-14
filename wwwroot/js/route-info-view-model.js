function RouteInfoViewModel(client, navigationService, dialogService) {
    var self = this
    this.navigationService = navigationService;
    this.dialogService = dialogService;

    this.member = null;
    this.image = null;
    this.hasImage = false;
    this.HoldPositions = [];
    this.client = client;
    this.grade = null;
    this.route = null;
    this.isAuthed = false;

    this.hasRated = false;
    this.filledStars;
    this.emptyStars;

    this.init = function () {
        self.client.routes.getRoute(navigationService.getParameters()["routeId"], function (routeResponse) {
            if (routeResponse.success) {

                self.route = routeResponse.data;
                //Makes the date look nice on a route
                self.route.date = self.route.createdDate.split("T")[0].split("-").reverse().join("/");
                self.downloadImage();
                self.trigger("commentsChanged");

                //Updates the rating on the route in case the member accessing it, has rated
                self.client.members.getMemberRatings(function(ratingResponse) {
                    var ratingValue = null;
                    if (ratingResponse.success) {
                        var memberRating = ratingResponse.data.filter(function(r) { return r.routeId == self.route.id; })[0];
                        if (memberRating) {
                            ratingValue = memberRating.ratingValue;
                        }
                    }

                    self.updateRating(self.route.averageRating, ratingValue);
                });
            }
        });

        //If authenticated, show the box that allows members to submit comments/Beta
        self.client.members.getMemberInfo(function(response) {
            if (response.success) {
                self.isAuthed = true;
                self.member = response.data;
            }
            self.trigger("commentsChanged");
        });

    };

    this.updateRating = function(averageRating, memberRating) {
        var rating;

        if (memberRating) {
            this.hasRated = true;
            rating = memberRating;
            self.client.routes.setRating(self.route.id, rating, function (response) {
                if (!response.success) {
                    self.dialogService.showError(response.message);
                }
            });
        } else {
            this.hasRated = false
            rating = averageRating;
        }

        //Use integer division to color the stars approximately to a route's average rating
        rating = Math.round(rating);
        self.filledStars = rating;
        self.emptyStars = 5 - rating;
        self.trigger("cardChanged");
    };

    this.downloadImage = function() {
        self.client.routes.getImage(self.route.id, function(imageResponse) {
            if (imageResponse.success) {
                self.hasImage = true;
                self.route.image = new Image();
                self.route.image.src = imageResponse.data.fileUrl;
                self.HoldPositions = imageResponse.data.holds;
                self.route.image.onload = function() {
                    self.trigger("cardChanged");
                }
            } else {
                self.trigger("cardChanged");
            }
        });
    };

    this.changeRating = function(rating) {
        if (self.isAuthed) {
            self.updateRating(self.route.averageRating, rating);
            self.dialogService.showInfo("Rating saved");
        }
        else
            self.dialogService.showInfo("You need to log in to rate routes");
    };

    this.editRoute = function () {
        if (self.route != null) {
            navigationService.toEditRoute(self.route.id);
        }
    };

    this.deleteRoute = function () {
        if (self.route != null && self.dialogService.confirm("Do you really want to delete this route?")) {
            self.client.routes.deleteRoute(self.route.id, function (response) {
                if (response.success) {
                    self.route = null;
                    navigationService.toRoutes();
                }
            });
        }
    };

    this.addingComment = false;

    this.addComment = function(form) {
        if (!self.isAuthed) {
            navigationService.toLogin("route-info?routeId=" + self.route.id);
            return;
        }
        var fd = new FormData(form);
        this.addingComment = true;
        this.client.routes.addComment(fd, self.route.id, function(response) {
            if (response.success) {
                self.addingComment = false;
                self.getComments();
            } else {
                self.dialogService.showError(response.message);
                self.addingComment = false;
                self.trigger("commentsChanged");
            }
        }, function(response) {
            self.dialogService.showError("Failed adding comment. The video added is probably too large.");
            self.trigger("commentsChanged");
        });
    };

    this.getComments = function(callback) {
        self.client.routes.getRoute(navigationService.getParameters()["routeId"], function (routeResponse) {
            if (routeResponse.success) {
                self.route.comments = routeResponse.data.comments;
                self.trigger("commentsChanged")
            }
        });
    }

    //Deletes a comment if the author of it, or an admin, confirms the deletion
    this.removeComment = function (id, routeId) {
        if (!self.dialogService.confirm("Are you sure that you want to remove the comment?")) return;
        this.client.routes.removeComment(id, routeId, function(response) {
            if (response.success) {
                self.client.routes.getRoute(navigationService.getParameters()["routeId"], function (routeResponse) {
                    if (routeResponse.success) {
                        self.route.comments = routeResponse.data.comments;
                        self.trigger("commentsChanged");
                    }
                });
            } else {
                self.dialogService.showError(response.message);
            }
        });
    };

}

RouteInfoViewModel.prototype = new EventNotifier();