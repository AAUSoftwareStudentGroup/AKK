$.ajax({
  url: "js/eventnotifier.js",
  dataType: "script",
  async: false
});

function NewRouteViewModel(client) {

    this.init = function() {
        this.getSections();
        this.getGrades();
        self.trigger("HoldColorUpdated");
    }

    var self = this;
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
        { value: 2, name: "Blue", color: "0000FF", r: 0, g: 0, b: 255, a: 1 },
        { value: 3, name: "Violet", color: "7F00FF", r: 127, g: 0, b: 255, a: 1 },
        { value: 4, name: "Magenta", color: "FF00FF", r: 255, g: 0, b: 255, a: 1 },
        { value: 5, name: "Rose", color: "FF017F", r: 255, g: 1, b: 127, a: 1 },
        { value: 6, name: "Red", color: "FF0000", r: 255, g: 0, b: 0, a: 1 },
        { value: 7, name: "Orange", color: "FF7F00", r: 255, g: 127, b: 0, a: 1 },
        { value: 8, name: "Yellow", color: "e4dc00", r: 228, g: 220, b: 0, a: 1 },
        { value: 9, name: "Chartreuse Green", color: "79FF00", r: 121, g: 255, b: 0, a: 1 },
        { value: 10, name: "Green", color: "00e900", r: 0, g: 233, b: 0, a: 1 },
        { value: 11, name: "Black", color: "000000", r: 0, g: 0, b: 0, a: 1 },
        { value: 12, name: "Brown", color: "7E360F", r: 127, g: 54, b: 15, a: 1 },
        { value: 13, name: "Grey", color: "5c5959", r: 92, g: 89, b: 89, a: 1 },
        { value: 14, name: "White", color: "D0D0D0", r: 208, g: 208, b: 208, a: 1 },
    ];
    this.changeSection = function (sectionId) {
        self.selectedSection = self.sections.filter(function (s) { return s.sectionId == sectionId; })[0];
    };
    this.changeGrade = function (gradeValue) {
        self.selectedGrade = self.grades.filter(function (g) { return g.difficulty == gradeValue; })[0];
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
        if (self.selectedSection != null &&
            self.selectedGrade != null &&
            self.selectedColor != null &&
            !isNaN(self.routeNumber)) {
            var sectionId = self.selectedSection.sectionId;
            var gradeValue = self.selectedGrade;
            var holdColor = self.selectedColor;
            var tapeColor = self.selectedTapeColor;
            var routeNumber = self.routeNumber;
            var author = self.author;
            self.client.routes.addRoute(sectionId,
                routeNumber,
                author,
                holdColor,
                gradeValue,
                tapeColor,
                function(response) {
                    if (response.success) {
                        window.history.back();
                    } else {
                        $("#error-message").html(response.message).show();
                    }
                });
        }
    };
}
NewRouteViewModel.prototype = new EventNotifier();