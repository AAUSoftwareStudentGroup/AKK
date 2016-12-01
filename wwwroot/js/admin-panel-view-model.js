function AdminPanelViewModel(client, dialogService) {
    var self = this;
    this.client = client;
    this.dialogService = dialogService;

    this.selectedSection = null;
    this.sections = [];
    this.routes = [];

    this.init = function() {
        self.downloadSections();
        self.selectedSection = self.sections[0];
    }

    this.downloadSections = function() {
        self.client.sections.getAllSections(function(response) {
            if(response.success) 
            {
                self.sections = response.data;
                self.trigger("sectionsChanged");
            }
            else
            {
                self.trigger("Error", response.message);
            }
        });
    }

    this.downloadRoutes = function() {
        self.client.routes.getRoutes("", self.selectedSection.id, "", function(response) {
            if(response.success) {
                self.routes = response.data;
                for(var i = 0; i < self.routes.length; i++) {
                    self.routes[i].date = self.routes[i].createdDate.split("T")[0].split("-").reverse().join("/");
                }
                self.trigger("routesChanged");
                self.trigger("sectionsChanged"); //This shouldn't happen!
            }
        });
    }

    this.changeSection = function (sectionId) {
        self.selectedSection = self.sections.filter(function (s) { return s.id == sectionId; })[0];
        self.downloadRoutes();
    };

    this.addNewSection = function() {
        var name = self.dialogService.prompt("Enter name of new Section","");
        self.client.sections.addSection(name, function(response) {
            if(response.success) {
                self.downloadSections();
            }
        });
    }

    this.clearSection = function() {
        if(self.selectedSection != null && self.dialogService.confirm("Do you really want to remove all routes from this section?")) {
            self.client.sections.deleteSectionRoutes(self.selectedSection.name, function(response) {
                if(response.success) {
                    self.downloadRoutes();
                }
            });
        }
    }

    this.deleteSection = function() {
        if(self.selectedSection != null && self.dialogService.confirm("Do you really want permanently delete this section?")) {
            self.client.sections.deleteSection(self.selectedSection.id, function(response) {
                if(response.success) {
                    self.downloadSections();
                }
            });
        }
    }

    this.renameSection = function() {
        var newName = self.dialogService.prompt("Enter the new name","");
        if(self.selectedSection != null && self.dialogService.confirm("Do you really want to rename this section?")) {
            self.client.sections.renameSection(self.selectedSection.id, newName, function(response) {
                if(response.success) {
                    self.downloadSections();
                }
            });
        }
    }

/*
    this.getGrades = function()
    {
        self.client.grades.getAllGrades(function(response) {
            if(response.success)
            {
                self.grades = self.grades.concat(response.data);
                self.trigger("DoneLoading");
            }
        })
    }

    this.addNewGrade = function()
    {
        var name = self.dialogService.prompt("Enter name of new Difficulty", "");
        var newGrade = self.grades[0];
        newGrade.name = name;
        newGrade.difficulty = self.grades.length + 1;
        self.client.grades.addGrade(newGrade, function(response) {
            if(response.success)                
            self.trigger("DoneLoading");

        });
    }

    this.deleteGrade = function()
    {
        if(self.selectedGrade != null && self.dialogService.confirm("Do you really want to permanently delete this difficulty?"))
        {
            self.client.grades.deleteGrade(self.selectedGrade.id, function(response) {
                if(response.success)
                self.trigger("DoneLoading");                
            });
        }
    }

    this.changeGrade = function(gradeValue)
    {
        if (self.selectedGrade != null && self.selectedGrade.difficulty == gradeValue) {
            self.selectedGrade = null;
            self.trigger("DoneLoading");
        }
        else
            self.selectedGrade = self.grades.filter(function(g) { return g.difficulty == gradeValue; })[0];
    }
*/
}
AdminPanelViewModel.prototype = new EventNotifier();