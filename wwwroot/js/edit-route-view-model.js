function EditRouteViewModel(client, navigationService) {
    RouteViewModel.apply (this, arguments);
    this.routeId = "";
    var self = this;
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
                self.trigger("Error", response.message);
            }
        });
    }
    this.UpdateRoute = function() {
        var imgObject = null;
        if (this.image != null) {
            imgObject = {
                fileUrl : this.image.src,
                width : this.image.width,
                height : this.image.height,
                holds : this.HoldPositions || []
            }
        }
        if (this.selectedTape != null) {
            this.selectedTape = this.selectedTape.colorOfHolds;
        }
        self.client.routes.updateRoute( this.routeId, 
                                        this.selectedSection.id, 
                                        this.author, 
                                        this.routeNumber, 
                                        this.selectedHold.colorOfHolds, 
                                        this.selectedGrade.id, 
                                        this.selectedTape,
                                        this.note,
                                        imgObject, function(response) {
            if(response.success)
            {
                navigationService.toRouteInfo(self.routeId);
            }
            else
            {
                self.trigger("Error", response.message);
            }
        });
    }
}