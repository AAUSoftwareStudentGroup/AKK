function RoutesViewModel(client) {
    var self = this;
    this.client = client;
    this.selectedGrade = null;
    this.selectedSection = null;
    this.selectedColor = null;
    this.selectedTape = null;
    this.selectedSortBy = null;
    this.routes = [];
    this.grades = [
        { difficulty: -1, name: "All" }
    ];
    this.sections = [
        { sectionId: -1, name: "All" }
    ];
    this.sortOptions = [
        { value: 0, name: "Newest" },
        { value: 1, name: "Oldest" },
        { value: 2, name: "Grading" },
        { value: 3, name: "Author" },
    ];
    this.searchClicked = function() {
        this.isSearching = !this.isSearching;
        if (!this.isSearching) {
            this.init();
        }
        self.trigger("SearchMethodChanged");
    }

    this.search = function(searchstring) {
        this.client.routes.searchRoutes(searchstring, function(response) {
            if (response.success) {
                self.routes = response.data;
                for (var i = 0; i < self.routes.length; i++) {
                    self.routes[i].sectionName = self.sections.filter(function (s) {
                        return s.id == self.routes[i].sectionId;
                    })[0].name;
                    self.routes[i].date = self.routes[i].createdDate.split("T")[0].split("-")
                        .reverse()
                        .join("/");
                    self.routes[i].selectedColor = self.routes[i].colorOfHolds;
                }
            } else {
                self.routes = [];
            }
            self.trigger("RoutesChanged");
        });
    }

    this.isSearching = false;
    this.init = function () {
        self.grades = [{ difficulty: -1, name: "All" }];
        self.getGrades();
        self.client.sections.getAllSections(function (response) {
            if (response.success) {
                self.sections = [{ sectionId: -1, name: "All" }];
                self.sections = self.sections.concat(response.data);
                self.selectedGrade = self.grades[0];
                self.selectedSection = self.sections[0];
                self.selectedSortBy = self.sortOptions[0];
                self.refreshRoutes();
                self.trigger("RoutesChanged");
                self.trigger("SearchMethodChanged");
            }
        });
    };
    this.changeGrade = function (gradeValue) {
        self.selectedGrade = self.grades.filter(function (grade) { return grade.difficulty == gradeValue; })[0];
        self.refreshRoutes();
    };
    this.getGrades = function () {
        self.client.grades.getAllGrades(function (response) {
            if (response.success) {
                self.grades = self.grades.concat(response.data);
            }
        });
    };
    this.changeSection = function (sectionId) {
        self.selectedSection = self.sections.filter(function (section) { return section.id == sectionId; })[0];
        self.refreshRoutes();
    };
    this.changeSortBy = function (sortByValue) {
        self.selectedSortBy = self.sortOptions.filter(function (sortBy) { return sortBy.value == sortByValue; })[0];
        self.refreshRoutes();
    };
    this.refreshRoutes = function () {
        var gradeValue = self.selectedGrade.difficulty == -1 ? null : self.selectedGrade.difficulty;
        var sectionId = self.selectedSection.id == -1 ? null : self.selectedSection.id;
        var sortByValue = self.selectedSortBy.value == -1 ? null : self.selectedSortBy.value;
        self.client.routes.getRoutes(gradeValue,
            sectionId,
            sortByValue,
            function (response) {
            if (response.success) {
                self.routes = response.data;
                for (var i = 0; i < self.routes.length; i++) {
                    self.routes[i].sectionName = self.sections.filter(function (s) {
                        return s.id == self.routes[i].sectionId;
                    })[0].name;
                    self.routes[i].date = self.routes[i].createdDate.split("T")[0].split("-")
                        .reverse()
                        .join("/");
                    self.routes[i].selectedColor = self.routes[i].colorOfHolds;
                }
                self.trigger("RoutesChanged");
            }
        });
    };
};



RoutesViewModel.prototype = new EventNotifier();