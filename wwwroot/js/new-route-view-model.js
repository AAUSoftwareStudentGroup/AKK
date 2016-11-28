function NewRouteViewModel(client, navigationService) {
    RouteViewModel.apply (this, arguments);
    var self = this;
    this.init = function() {
        this.downloadSections();
        this.downloadGrades();
        this.trigger("numberUpdated");
        this.client.members.getMemberInfo(function(response) {
            if (response.success) {
                self.changeAuthor(response.data.displayName);
                self.trigger("authorUpdated");
            }
        });
        this.trigger("holdsUpdated");
        this.trigger("imageUpdated");
        this.trigger("noteUpdated");
    }

    this.addRoute = function() {
        var imgObject = null;
        if (this.image != null) {
            imgObject = {
                fileUrl : this.image.src,
                width : this.image.width,
                height : this.image.height,
                holds : this.HoldPositions || []
            }
        }
        var sectionId = (self.selectedSection == null ? null : self.selectedSection.id);
        var gradeId = (self.selectedGrade == null ? null : self.selectedGrade.id);
        var holdColor = self.selectedHold;
        var tapeColor = self.selectedTape;
        var routeNumber = self.number;
        var author = self.author;
        self.client.routes.addRoute(sectionId, routeNumber, author, holdColor, gradeId, tapeColor,imgObject, function(response) {
            if (response.success) {
                self.navigationService.back();
            } else {
                $("#error-message").html(response.message).show();
            }
        });
    };
}