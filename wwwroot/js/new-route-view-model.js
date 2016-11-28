function NewRouteViewModel(client, navigationService) {
    RouteViewModel.apply (this, arguments);
    var self = this;
    this.init = function() {
        this.downloadSections();
        this.downloadGrades();
        this.client.members.getMemberInfo(function(response) {
            if (response.success) {
                self.changeAuthor(response.data.displayName);
                self.trigger("authorUpdated");
            }
        });
        this.trigger("holdsUpdated");
        this.trigger("numberUpdated");
    }

    this.addRoute = function() {
        var sectionId = (self.selectedSection == null ? null : self.selectedSection.id);
        var gradeId = (self.selectedGrade == null ? null : self.selectedGrade.id);
        var holdColor = self.selectedHold;
        var tapeColor = self.selectedTape;
        var routeNumber = self.number;
        var author = self.author;
        self.client.routes.addRoute(sectionId, routeNumber, author, holdColor, gradeId, tapeColor, function(response) {
            if (response.success) {
                self.navigationService.back();
            } else {
                $("#error-message").html(response.message).show();
            }
        });
    };
}