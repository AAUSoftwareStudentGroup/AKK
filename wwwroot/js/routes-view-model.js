function RoutesViewModel(client, loadingService) {
    var self = this;
    var client = client;
    var loadingService = loadingService;
    var currentAjaxRequest = null;
    var parseRoutes = function(response) {
        if (response.success) {
            self.routes = response.data;
            for (var i = 0; i < self.routes.length; i++) {
                var temp = Math.round(self.routes[i].averageRating || "0.0");
                self.routes[i].filledStars = temp
                self.routes[i].emptyStars = 5 - temp;
                self.routes[i].date = self.routes[i].createdDate.split("T")[0].split("-").reverse().join("/");
            }
            self.trigger("routesChanged");
        } else {
            self.routes = [];
        }
    };

    this.init = function () {
        client.grades.getAllGrades(function (gradesResponse) {
            client.sections.getAllSections(function (sectionsResponse) {
                if (gradesResponse.success && sectionsResponse.success) {
                    self.sections = [ { id: "", name: "All" } ];
                    self.grades = [ { id: "", name: "All" } ];
                    self.sections = self.sections.concat(sectionsResponse.data);
                    self.trigger("sectionsChanged");
                    self.grades = self.grades.concat(gradesResponse.data);
                    self.trigger("gradesChanged");
                    self.changeGrade(self.grades[0].id);
                    self.changeSection(self.sections[0].id);
                    self.changeSortBy(self.sortOptions[0].value);
                    self.refreshRoutes();
                }
            });
        });
    }

    this.routes = [ ];
    this.grades = [ ];
    this.sections = [ ];
    this.sortOptions = [
        { value: 0, name: "Newest" },
        { value: 1, name: "Oldest" },
        { value: 2, name: "Grading" },
        { value: 3, name: "Rating" },
        { value: 4, name: "Author" },
    ];
    this.selectedGrade = { id: "", name: "All" };
    this.selectedSection = { id: "", name: "All" };
    this.selectedSortBy = this.sortOptions[0];
    this.isSearching = false;

    this.refreshRoutes = function () {
        client.routes.getRoutes(self.selectedGrade.id, self.selectedSection.id, self.selectedSortBy.value, function (response) {
            parseRoutes(response);
        });
    };

    this.search = function (searchstring) {
        loadingService.load();
        if (this.currentAjaxRequest != null) {
            this.currentAjaxRequest.abort();
        }
        currentAjaxRequest = client.routes.searchRoutes(searchstring, function (response) {
            if (response.success) {
                self.currentAjaxRequest = null;
            }
            parseRoutes(response);
            loadingService.stopLoad();
        });
    };

    this.toggleIsSearching = function() {
        this.isSearching = !this.isSearching;
        self.trigger("isSearchingChanged");
        if(!this.isSearching)
        {
            self.changeGrade(self.grades[0].id);
            self.changeSection(self.sections[0].id);
            self.changeSortBy(self.sortOptions[0].value);
            self.refreshRoutes();
        }
    };

    this.changeGrade = function (gradeId) {
        self.selectedGrade = self.grades.filter(function (grade) { return grade.id == gradeId; })[0];
        self.trigger("selectedGradeChanged");
        self.refreshRoutes();
    }

    this.changeSection = function (sectionId) {
        self.selectedSection = self.sections.filter(function (section) { return section.id == sectionId; })[0];
        self.trigger("selectedSectionChanged");
        self.refreshRoutes();
    }

    this.changeSortBy = function (sortByValue) {
        self.selectedSortBy = self.sortOptions.filter(function (sortBy) { return sortBy.value == sortByValue; })[0];
        self.trigger("selectedSortByChanged");
        self.refreshRoutes();
    }
};

RoutesViewModel.prototype = new EventNotifier();