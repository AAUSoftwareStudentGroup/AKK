function EditRouteViewModel(client, navigationService, dialogService) {
    RouteViewModel.apply (this, arguments);
    this.routeId = "";
    var self = this;

    //Initialise the edit-route page and assigns the variables with the values the route, has
    //grade gets the grade the route has, route number is the route's number, etc.
    this.init = function() {
        this.routeId = this.navigationService.getParameters()["routeId"];
        this.client.routes.getRoute(this.routeId, function(response) {
            if (response.success) {
                self.downloadSections(function() {
                    self.changeSection(response.data.sectionId);
                    self.trigger("sectionsUpdated");
                });

                self.downloadGrades(function() {
                    self.changeGrade(response.data.gradeId);
                    self.trigger("gradesUpdated");
                });

                self.downloadHolds(function() {
                    self.changeHold(response.data.colorOfHolds);
                    if (response.data.colorOfTape) {
                        self.changeTape(response.data.colorOfTape);
                        self.toggleTape();
                    }
                    self.trigger("holdsUpdated");
                });
                
                self.changeNumber(response.data.name);
                self.trigger("numberUpdated");

                self.changeAuthor(response.data.author);
                self.trigger("authorUpdated");

                self.downloadImage();

                self.changeNote(response.data.note);
                self.trigger("noteUpdated");
            } else {
                self.dialogService.showError(response.message);
            }
        });
    }

    //Updates the existing route with the new values
    this.UpdateRoute = function() {
        var imgObject = null;
        if (self.image != null) {
            imgObject = {
                fileUrl : self.image.src,
                width : self.image.width,
                height : self.image.height,
                holds : self.HoldPositions || []
            }
        }
        if (self.selectedTape != null) {
            self.selectedTape = self.selectedTape.colorOfHolds;
        }
        self.client.routes.updateRoute( self.routeId, 
                                        self.selectedSection.id, 
                                        self.author, 
                                        self.number, 
                                        self.selectedHold.colorOfHolds, 
                                        self.selectedGrade.id, 
                                        self.selectedTape,
                                        self.note,
                                        imgObject, function(response) {
            if(response.success)
            {
                navigationService.toRouteInfo(self.routeId);
            }
            else
            {
                self.dialogService.showError(response.message);
            }
        });
    }
}