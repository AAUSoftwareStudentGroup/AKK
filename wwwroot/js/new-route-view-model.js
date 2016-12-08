function NewRouteViewModel(client, navigationService, dialogService) {
    RouteViewModel.apply (this, arguments);
    var self = this;

    //Initialise the new route page by downloading everything from the server and store them in their respective variables
    //This view model makes use of route-view-model.js 
    this.init = function() {
        this.downloadSections();
        this.downloadGrades();
        this.downloadHolds();
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

    //Adds a route with all its properties
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
        var holdColor = null;
        if(self.selectedHold != null)
        {
            holdColor = self.selectedHold.colorOfHolds;
        }
        var tapeColor = null;
        if (self.selectedTape != null) {
            tapeColor = self.selectedTape.colorOfHolds;
        }
        var routeNumber = self.number;
        var author = self.author;

        self.client.routes.addRoute(sectionId, routeNumber, author, holdColor, gradeId, tapeColor, this.note, imgObject, function(response) {
            if (response.success) {
                self.navigationService.toRouteInfo(response.data.id);
            } else {
                self.dialogService.showError(response.message);
            }
        });
    };
}