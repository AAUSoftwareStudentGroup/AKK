$.ajax({
  url: "js/eventnotifier.js",
  dataType: "script",
  async: false
});

function SectionsViewModel(client)
{
    var self = this;
    this.init = function()
    {
        self.getGrades();
        self.client.sections.getAllSections(function(response)
        {
            if(response.success)
            {
                self.sections = self.sections.concat(response.data);
                self.selectedGrade = null;
                self.selectedSection = self.sections[0];
                self.selectedSortBy = self.sortOptions[0];
                self.trigger("DoneLoading");
                self.trigger("RoutesUpdated");
                self.trigger("SectionsUpdated");

            }
        });
    };
    this.client = client;
    this.selectedSection = null;
    this.selectedGrade = null;
    this.routes = [];
    this.grades = [];
    this.sections = [];
    this.sortOptions = [
        { value: 0, name: "Newest" },
        { value: 1, name: "Oldest" },
        { value: 2, name: "Grading" },
        { value: 3, name: "Author" },
    ];
    this.refreshRoutes = function()
    {
        var gradeValue = self.selectedGrade == null ? "all" : self.selectedGrade.value;
        var sectionId = self.selectedSection.sectionId == -1 ? null : self.selectedSection.sectionId;
        var sortByValue = self.selectedSortBy.value == -1 ? null : self.selectedSortBy.value;
        self.client.routes.getRoutes(gradeValue, sectionId, sortByValue, function(response) {
            if(response.success)
            {
                self.routes = response.data;
                for(var i = 0; i < self.routes.length; i++)
                {
                    self.routes[i].sectionName = self.sections.filter(function(s) { 
                        return s.sectionId == self.routes[i].sectionId; 
                    })[0].name;
                    self.routes[i].date = self.routes[i].createdDate.split("T")[0].split("-").reverse().join("/");
                    self.routes[i].selectedColor = self.routes[i].colorOfHolds;
                }
                self.trigger("RoutesUpdated");
            }
        });
    };
    this.refreshSections = function()
    {
        self.client.sections.getAllSections(function(response) {
            if(response.success) {
                self.sections = response.data;

                self.trigger("DoneLoading");
            }
        });
    };
    this.getSectionDetails = function(sectionId)
    {
        self.selectedSection = self.sections.filter(function(section){ return section.sectionId == sectionId; })[0];
        //  viewModel.selectedSection = viewModel.client.sections.getSection(viewModel.selectedSection.sectionId);
        /* var response;
        viewModel.client.routes.getRoutes(viewModel.grades[0], viewModel.selectedSection.sectionId, viewModel.sortOptions[0], function(response) {
            if(response.success)
                viewModel.refreshRoutes();
        });*/
        self.refreshRoutes();

    };
    this.getGrades = function()
    {
        self.client.grades.getAllGrades(function(response) {
            if(response.success)
            {
                self.grades = self.grades.concat(response.data);
                self.trigger("DoneLoading");
            }
        })
    };
    this.changeGrade = function(gradeValue)
    {
        if (self.selectedGrade != null && self.selectedGrade.difficulty == gradeValue) {
            self.selectedGrade = null;
            self.trigger("DoneLoading");
        }
        else
            self.selectedGrade = self.grades.filter(function(g) { return g.difficulty == gradeValue; })[0];
    };
    this.addNewSection = function()
    {
        var name = prompt("Enter name of new Section","");
        var response;
        self.client.sections.addSection(name, function(response) {
            if(response.success)
                self.refreshSections();
        });
    };
    this.clearSection = function()
    {
        if(self.selectedSection != null && confirm("Do you really want to remove all routes from this section?"))
        {
            self.client.sections.deleteSectionRoutes(self.selectedSection.sectionId, function(response) {
                if(response.success)
                    self.refreshRoutes();
            });
        }
    };
    this.deleteSection = function()
    {
        if(self.selectedSection != null && confirm("Do you really want permanently delete this section?"))
        {
            self.client.sections.deleteSection(self.selectedSection.sectionId, function(response) {
                if(response.success)
                    self.refreshSections();
            });
        }
    };
    this.renameSection = function()
    {
        var newName = prompt("Enter the new name","");
        if(self.selectedSection != null && confirm("Do you really want to rename this section?"))
        {
            self.client.sections.renameSection(self.selectedSection.sectionId, newName, function(response) {
                if(response.success)
                    self.refreshSections();
            });
        }
    };
    this.addNewGrade = function()
    {
        var name = prompt("Enter name of new Difficulty", "");
        var newGrade = self.grades[0];
        newGrade.name = name;
/*            newGrade.color.r = 128;
        newGrade.color.g = 0;
        newGrade.color.b = 128;*/
        newGrade.difficulty = self.grades.length + 1;
        self.client.grades.addGrade(newGrade, function(response) {
            if(response.success)                
            self.trigger("DoneLoading");

        });
    };
    this.deleteGrade = function()
    {
        if(self.selectedGrade != null && confirm("Do you really want to permanently delete this difficulty?"))
        {
            self.client.grades.deleteGrade(self.selectedGrade.difficulty, function(response) {
                if(response.success)
                self.trigger("DoneLoading");                
            });
        }
    };
}
SectionsViewModel.prototype = new EventNotifier();