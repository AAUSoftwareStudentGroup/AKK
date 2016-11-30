function RoutesViewModel(client) {
    var self = this;
    this.client = client;

    this.routes = [];
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

    this.selectedGrade = this.grades[0];
    this.selectedSection = this.sections[0];
    this.selectedSortBy = this.sortOptions[0];

    this.isSearching = false;

    this.init = function () {
        self.client.grades.getAllGrades(function (gradesResponse) {
            self.client.sections.getAllSections(function (sectionsResponse) {
                if (gradesResponse.success && sectionsResponse.success) {
                    self.sections = self.sections.concat(sectionsResponse.data);
                    self.grades = self.grades.concat(gradesResponse.data);
                    self.selectedGrade = self.grades[0];
                    self.selectedSection = self.sections[0];
                    self.selectedSortBy = self.sortOptions[0];
                    self.downloadRoutes();
                    self.trigger("routesChanged");
                    self.trigger("filteringChanged");
                }
            });
        });
    }

    this.downloadRoutes = function () {
        self.client.routes.getRoutes(self.selectedGrade.id, self.selectedSection.id, self.selectedSortBy.value, function (response) {
            self.parseRoutes(response);
        });
    };

    this.searchClicked = function () {
        this.isSearching = !this.isSearching;
        if (!this.isSearching) {
            this.init();
        }
        self.trigger("SearchMethodChanged");
    };

    this.currentAjaxRequest = null;
    this.search = function (searchstring) {
        if (this.currentAjaxRequest != null) {
            this.currentAjaxRequest.abort();
        }
        this.currentAjaxRequest = this.client.routes.searchRoutes(searchstring, function (response) {
            self.parseRoutes(response);
            self.trigger("routesChanged");
        });
    };

    this.parseRoutes = function(response) {
        if (response.success) {
            self.routes = response.data;
            for (var i = 0; i < self.routes.length; i++) {
                self.routes[i].rating = Math.random() * 5;
                var temp = Math.round(self.routes[i].rating);
                self.routes[i].filledStars = temp
                self.routes[i].emptyStars = 5 - temp;
                self.routes[i].date = self.routes[i].createdDate.split("T")[0].split("-").reverse().join("/");
            }
            self.trigger("routesChanged");
        } else {
            self.routes = [];
        }
    }

    this.toggleSearch = function() {
        this.isSearching = !this.isSearching;
        if (!this.isSearching) {
            this.init();
        }
        self.trigger("filteringChanged");
    }   

    this.changeGrade = function (gradeId) {
        self.selectedGrade = self.grades.filter(function (grade) { return grade.id == gradeId; })[0];
        self.downloadRoutes();
    }

    this.changeSection = function (sectionId) {
        self.selectedSection = self.sections.filter(function (section) { return section.id == sectionId; })[0];
        self.downloadRoutes();
    }

    this.changeSortBy = function (sortByValue) {
        self.selectedSortBy = self.sortOptions.filter(function (sortBy) { return sortBy.value == sortByValue; })[0];
        self.downloadRoutes();
    }
};

RoutesViewModel.prototype = new EventNotifier();