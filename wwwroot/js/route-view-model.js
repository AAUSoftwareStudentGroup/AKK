function RouteViewModel(client, navigationService) {
    EventNotifier.apply(this);
    var self = this;
    this.navigationService = navigationService;
    this.client = client;
    
    this.sections = [];
    this.selectedSection = null;
    this.grades = [];
    this.selectedGrade = null;
    this.number = null;
    this.author = null;
    this.colors = [
        { value: 0, name: "Cyan", color: "00c8c8", r: 0, g: 200, b: 200, a: 1 },
        { value: 1, name: "Azure", color: "017EFF", r: 1, g: 127, b: 255, a: 1 },
        { value: 2, name: "Blue", color: "3C3Cff", r: 60, g: 60, b: 255, a: 1 },
        { value: 3, name: "Violet", color: "7F00FF", r: 127, g: 0, b: 255, a: 1 },
        { value: 4, name: "Magenta", color: "C832C8", r: 200, g: 50, b: 200, a: 1 },
        { value: 5, name: "Rose", color: "D21978", r: 210, g: 25, b: 120, a: 1 },
        { value: 6, name: "Red", color: "C81E1E", r: 200, g: 30, b: 30, a: 1},
        { value: 7, name: "Orange", color: "FF7F00", r: 255, g: 127, b: 0, a: 1},
        { value: 8, name: "Yellow", color: "DCC81E", r: 220, g: 200, b: 30, a: 1},
        { value: 9, name: "Light Green", color: "6ED214", r: 110, g: 210, b: 20, a: 1},
        { value: 10, name: "Green", color: "149614", r: 20, g: 150, b: 20, a: 1},
        { value: 11, name: "Black", color: "000000", r: 0, g: 0, b: 0, a: 1},
        { value: 12, name: "Brown", color: "7E360F", r: 127, g: 54, b: 15, a: 1},
        { value: 13, name: "Grey", color: "5c5959", r: 92, g: 89, b: 89, a: 1},
        { value: 14, name: "White", color: "CDCDCD", r: 205, g: 205, b: 205, a: 1},
    ];
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
            self.selectedTapeColor = null;
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
        self.selectedHold = self.colors.filter(function (g) { return g.r == holdColor.r && g.g == holdColor.g && g.b == holdColor.b; })[0];
    };
    
    this.changeTape = function (tapeColor) {
        self.selectedTape = self.colors.filter(function (g) { return g.r == tapeColor.r && g.g == tapeColor.g && g.b == tapeColor.b; })[0];
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
        readURL(input, function(image) {
            resizeImage(image, function(resizedImage) {
                self.changeImage(resizedImage);
                self.HoldPositions = [];
            });
        });
    }
}

