function RouteViewModel(client, navigationService) {
    EventNotifier.apply(this);
    var self = this;
    this.navigationService = navigationService;
    this.client = client;
    
    this.addingImage = false;
    this.sections = [];
    this.selectedSection = null;
    this.grades = [];
    this.selectedGrade = null;
    this.number = null;
    this.author = null;
    this.colors = [];
    this.selectedHold = null;
    this.selectedTape = null;
    this.hasTape = false;
    this.HoldPositions = [];
    this.hasImage = false;
    this.image = null;
    this.imageRotation = 0;
    this.note = null;
    this.downloadSections = function(callback) {
        self.client.sections.getAllSections(function (response) {
            if (response.success) {
                self.sections = response.data;
                self.trigger("sectionsUpdated");
            } else {
                self.trigger("Error", response.message);
            }
            if (callback) callback();
        });
    }

    this.downloadGrades = function(callback) {
        self.client.grades.getAllGrades(function (response) {
            if (response.success) {
                self.grades = response.data;
                self.trigger("gradesUpdated");
            } else {
                self.trigger("Error", response.message);
            }
            if (callback) callback();
        });
    }

    this.downloadHolds = function(callback) {
        self.client.holds.getAllHolds(function (response) {
            if (response.success) {
                self.colors = response.data;
                self.trigger("holdsUpdated");
            } else {
                self.trigger("Error", response.message);
            }
            if (callback) callback();
        });
    }

    this.downloadImage = function(callback) {
        self.client.routes.getImage(self.routeId, function(imageResponse) {
            if (imageResponse.success) {
                self.hasImage = true;
                self.image = new Image();
                self.image.src = imageResponse.data.fileUrl;
                self.HoldPositions = imageResponse.data.holds;
                self.image.onload = function() {
                    self.trigger("imageUpdated")
                }
            } else {
                self.trigger("imageUpdated");
            }
        });
    }
    this.addHold = function(hold) {
        this.HoldPositions.push(hold);
        this.trigger("HoldsUpdated");
    }
    
    this.toggleTape = function() {
        self.hasTape = !self.hasTape;
        if (!self.hasTape) {
            self.selectedTape = null;
        }
        self.trigger("holdsUpdated");
    }

    this.changeSection = function (sectionId) {
        self.selectedSection = self.sections.filter(function (s) { return s.id == sectionId; })[0];
    };

    this.changeGrade = function (gradeId) {
        self.selectedGrade = self.grades.filter(function (g) { return g.id == gradeId; })[0];
    };

    this.changeHold = function (holdColor) {
      //  console.log(holdColor);                   
     //  self.toggleTape();

        self.selectedHold = self.colors.filter(function (g) { return g.colorOfHolds.r == holdColor.r && g.colorOfHolds.g == holdColor.g && g.colorOfHolds.b == holdColor.b; })[0];
    };
    
    this.changeTape = function (tapeColor) {
    //    console.log(tapeColor);
        self.selectedTape = self.colors.filter(function (g) { return g.colorOfHolds.r == tapeColor.r && g.colorOfHolds.g == tapeColor.g && g.colorOfHolds.b == tapeColor.b; })[0];
    };

    this.changeNumber = function (routeNumber) {
        self.number = routeNumber;
    };

    this.changeAuthor = function (author) {
        self.author = author;
    };

    this.changeImage = function(image) {
        self.hasImage = true;
        self.image = new Image();
        self.image.src = image.src;
        self.HoldPositions = [];
        self.image.onload = function() {
            self.trigger("imageUpdated")
        }
    }

    this.changeNote = function(note) {
        self.note = note;
    }
    
    this.rotateImageClockwise = function() {
        rotateImage(self.image, 1, function (rotatedImage) {
            self.changeImage(rotatedImage);
            self.HoldPositions = [];
        })
    }


    this.UpdateCanvas = function(input) {
        this.addingImage = true;
        this.trigger("imageUpdated");
        readURL(input, function(image) {
            resizeImage(image, function(resizedImage) {
                self.addingImage = false;
                self.changeImage(resizedImage);
                self.HoldPositions = [];
            });
        });
    }
}

