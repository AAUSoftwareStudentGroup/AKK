function RoutesViewModel(client) {
    var self = this;
    this.client = client;
    this.selectedGrade = null;
    this.selectedSection = null;
    this.selectedSortBy = null;
    this.routes = [];
    this.init = function () {
        self.client.grades.getAllGrades(function (gradesResponse) {
            self.client.sections.getAllSections(function (sectionsResponse) {
                if (gradesResponse.success && sectionsResponse.success) {
                    self.sections = [{ id: "", name: "All" }];
                    self.sections = self.sections.concat(sectionsResponse.data);
                    self.grades = [{ id: "", name: "All" }];
                    self.grades = self.grades.concat(gradesResponse.data);
                    self.selectedGrade = self.grades[0];
                    self.selectedSection = self.sections[0];
                    self.selectedSortBy = self.sortOptions[0];
                    self.refreshRoutes();
                    self.trigger("RoutesChanged");
                    self.trigger("SearchMethodChanged");
                }
            });
        });
    };
    this.grades = [
        { id: "", name: "All" }
    ];
    this.sections = [
        { id: "", name: "All" }
    ];
    this.sortOptions = [
        { value: 0, name: "Newest" },
        { value: 1, name: "Oldest" },
        { value: 2, name: "Grading" },
        { value: 3, name: "Author" },
    ];
    this.searchClicked = function () {
        this.isSearching = !this.isSearching;
        if (!this.isSearching) {
            this.init();
        }
        self.trigger("SearchMethodChanged");
    }
    this.search = function (searchstring) {
        this.client.routes.searchRoutes(searchstring, function (response) {
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
    this.changeGrade = function (gradeId) {
        self.selectedGrade = self.grades.filter(function (grade) { return grade.id == gradeId; })[0];
        self.refreshRoutes();
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
        self.client.routes.getRoutes(self.selectedGrade.id, self.selectedSection.id, self.selectedSortBy.value, function (response) {
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