function AdminPanelViewModel(client, dialogService) {
    var self = this;
    this.client = client;
    this.dialogService = dialogService;

    this.selectedSection = null;
    this.selectedGrade = null;
    this.gradeName = "";
    this.sections = [];
    this.routes = [];
    this.editingGrades = false;

    this.init = function() {
        self.downloadSections();
        self.downloadGrades();
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
        if(self.selectedSection != null || self.selectedGrade != null) {
            self.client.routes.getRoutes((self.selectedGrade ? self.selectedGrade.id : ""), (self.selectedSection ? self.selectedSection.id : ""), "", function(response) {
                if(response.success) {
                    self.routes = response.data;
                    for(var i = 0; i < self.routes.length; i++) {
                        self.routes[i].date = self.routes[i].createdDate.split("T")[0].split("-").reverse().join("/");
                    }
                    self.trigger("routesChanged");
                }
            });
        }
        else {
            console.log("wat");
            self.routes = [];
            self.trigger("routesChanged");
        }
    }

    this.changeSection = function (sectionId) {
        self.selectedSection = self.sections.filter(function (s) { return s.id == sectionId; })[0];
        self.selectedGrade = null;
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
                else
                    self.dialogService.showMessage(response.message);
            });
        }
    }

    this.deleteSection = function() {
        if(self.selectedSection != null && self.dialogService.confirm("Do you really want permanently delete this section?")) {
            self.client.sections.deleteSection(self.selectedSection.id, function(response) {
                if(response.success) {
                    self.downloadSections();
                    self.downloadRoutes();
                }
                else
                    self.dialogService.showMessage(response.message);
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
                else
                    self.dialogService.showMessage(response.message);
            });
        }
    }
///////////////////////////////////
    this.downloadGrades = function()
    {
        self.client.grades.getAllGrades(function(response) {
            if(response.success)
            {
                self.grades = response.data;
                self.trigger("gradesChanged");
            }
            else
                self.dialogService.showMessage(response.message);
        })
    }

    this.addNewGrade = function()
    {
        var name = "New Grade";
        var newGrade = (self.selectedGrade ? self.selectedGrade : self.grades[0]);
        newGrade = JSON.parse(JSON.stringify(newGrade));
        newGrade.name = name;
        newGrade.difficulty = self.grades.length;
        newGrade.color = {r: 0x66, b: 0x66, g: 0x66};
        delete newGrade.id;
        self.client.grades.addGrade(newGrade, function(response) {
            if(response.success) {
                console.log(response.data);
                self.grades.push(response.data);
                self.selectedGrade = self.grades[self.grades.length-1];           
                self.trigger("gradesChanged");
            }
            else
                self.dialogService.showMessage(response.message);
        });
    }

    this.updateGrade = function()
    {
        self.client.grades.updateGrade(self.selectedGrade, function(response) {
            if(response.success) {
                self.selectedGrade = null;
                self.trigger("gradesChanged");
                self.downloadRoutes();
            }
            else {
                self.dialogService.showMessage(response.message);
            }
        });
    }

    this.deleteGrade = function()
    {
        if(self.selectedGrade != null && self.dialogService.confirm("Do you really want to permanently delete this difficulty?"))
        {
            self.client.grades.deleteGrade(self.selectedGrade.id, function(response) {
                if(response.success)
                    self.trigger("gradesChanged");
                else {
                    self.dialogService.showMessage(response.message);
                }
            });
        }
    }
////////////////
    this.setColor = function(r, g, b) {
        if(self.selectedGrade) {
            self.selectedGrade.color = {r: r, g: g, b: b};
            self.trigger("gradeColorChanged");
        }
    }

    this.setHue = function(deg) {
        if(self.selectedGrade) {
            // hue to rgb hsl (deg, 70%, 60%)
            self.selectedGrade.color = hslToRgb(deg/360, 0.7, 0.6);
            self.trigger("gradeColorChanged");
        }
    }

    this.selectGrade = function(gradeValue)
    {
        self.selectedSection = null;
        if(gradeValue === undefined || gradeValue === null) {
            self.selectedGrade = null;
            self.gradeName = "";
        }
        else {
            self.selectedGrade = self.grades.filter(function(g) { return g.difficulty == gradeValue; })[0];
        }
        self.trigger("gradesChanged");
        self.downloadRoutes();
    }

    this.changeGradeName = function(name) {
        if(self.selectedGrade) {
            self.selectedGrade.name = name;
        }
    }

    this.gradesSwap = function(diff, change) {
        if(diff+change < 0 || diff+change >= self.grades.length)
            return;
        
        var gradeA = self.grades[diff];
        var gradeB = self.grades[diff+change];
        var gradeBCopy = JSON.parse(JSON.stringify(gradeB));

        gradeB.difficulty = gradeA.difficulty;
        gradeA.difficulty = 999;

        self.client.grades.updateGrade(gradeA, function(response) {
            if(response.success) {
                self.client.grades.updateGrade(gradeB, function(response) {
                    if(response.success) {
                        gradeA.difficulty = gradeBCopy.difficulty
                        self.client.grades.updateGrade(gradeA, function(response) {
                            if(response.success) {
                                self.downloadGrades();
                            }
                            else
                                self.dialogService.showMessage(response.message);
                        });
                    }
                    else
                        self.dialogService.showMessage(response.message);
                });
            }
            else
                self.dialogService.showMessage(response.message);
        });
    }
}
AdminPanelViewModel.prototype = new EventNotifier();



    ;;;;;;;      ;;       ;;;;;;;  ;;;;;;;   ;;;;;;;  ;;;;;;
   ;;;   ;;;     ;;       ;;       ;;   ;;;  ;;       ;;   ;;
  ;;;     ;;;    ;;       ;;;;;    ;;;;;;;   ;;;;;    ;;    ;;
 ;;;;;;;;;;;;;   ;;       ;;       ;;  ;;;   ;;       ;;   ;;
;;;         ;;;  ;;;;;;;  ;;       ;;   ;;;  ;;;;;;;  ;;;;;;