function NewRouteViewModel(client, navigationService) {

    this.init = function() {
        this.getSections();
        this.getGrades();
        self.trigger("HoldColorUpdated");
        self.client.members.getMemberInfo(function(response) {
            if (response.success) {
                self.changeAuthor(response.data.displayName);
                self.trigger("DataLoaded");
            }
        });
    }

    var self = this;
    this.navigationService = navigationService;
    this.client = client;
    this.sections = [];
    this.selectedSection = null;
    this.selectedGrade = null;
    this.selectedColor = null;
    this.selectedTapeColor = null;
    this.routeNumber = null;
    this.author = null;
    this.hasTape = false;
    this.grades = [];
    this.holdColors = [
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
    
    this.changeSection = function (sectionId) {
        self.selectedSection = self.sections.filter(function (s) { return s.id == sectionId; })[0];
    };
    this.changeGrade = function (gradeId) {
        self.selectedGrade = self.grades.filter(function (g) { return g.id == gradeId; })[0];
    };
    this.changeHoldColor = function (holdColor) {
        self.selectedColor = self.holdColors.filter(function (g) { return g.value == holdColor; })[0];
    };
    this.changeTapeColor = function (tapeColor) {
        self.selectedTapeColor = self.holdColors.filter(function (g) { return g.value == tapeColor; })[0];
    };
    this.changeRouteNumber = function (routeNumber) {
        self.routeNumber = routeNumber;
    };
    this.changeAuthor = function (author) {
        self.author = author;
    };
    this.getSections = function () {
        self.client.sections.getAllSections(function (response) {
            if (response.success) {
                self.trigger("DataLoaded");
                self.sections = response.data;
            } else {
                $("#error-message").html(response.message).show();
            }
        });
    };
    this.getGrades = function () {
        self.client.grades.getAllGrades(function (response) {
            if (response.success) {
                self.grades = response.data;
                self.trigger("DataLoaded");
            }
        })
    };
    this.gradesGotTape = function () {
        if (self.hasTape === true) {
            self.hasTape = false;
            self.selectedTapeColor = null;
        } else
            self.hasTape = true;
        self.trigger("HoldColorUpdated");
    };
    this.addRoute = function() {
        if (!isNaN(self.routeNumber)) {
            var sectionId = (self.selectedSection == null ? null : self.selectedSection.id);
            var gradeId = (self.selectedGrade == null ? null : self.selectedGrade.id);
            var holdColor = self.selectedColor;
            var tapeColor = self.selectedTapeColor;
            var routeNumber = self.routeNumber;
            self.client.routes.addRoute(sectionId, routeNumber, holdColor, gradeId, tapeColor, function(response) {
                    if (response.success) {
                        self.navigationService.back();
                    } else {
                        $("#error-message").html(response.message).show();
                    }
                });
        }
    };
}
NewRouteViewModel.prototype = new EventNotifier();