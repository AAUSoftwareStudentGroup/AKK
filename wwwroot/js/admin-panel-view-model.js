function AdminPanelViewModel(client, dialogService) {
    var self = this;
    this.client = client;
    this.dialogService = dialogService;

    this.selectedSection = null;
    this.selectedGrade = null;
    this.selectedHold = null;
    this.holdName = "";
    this.gradeName = "";
    this.sections = [];
    this.routes = [];
    this.members = [];
    this.holds = [];
    this.editingGrades = false;

    this.init = function() {
        self.downloadSections();
        self.downloadGrades();
        self.downloadMembers();
        self.downloadHolds();
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
        if(name != null) {
            self.client.sections.addSection(name, function(response) {
                if(response.success) {
                    self.downloadSections();
                } 
                else
                    self.dialogService.showMessage(response.message);
            });
        }
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
        if (newName != null) {
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
                self.grades[self.selectedGrade.difficulty] = self.selectedGrade;
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
        if(self.grades.length <= 1) {
            alert("There must be at least on grade.");
            return;
        }
        if(self.selectedGrade != null && self.dialogService.confirm("Do you really want to permanently delete this difficulty?"))
        {
            self.client.grades.deleteGrade(self.selectedGrade.id, function(response) {
                if(response.success) {
                    self.selectedGrade = null;
                    self.downloadGrades();
                }
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
        } else {
            self.selectedHold.colorOfHolds = {r: r, g: g, b: b};
            self.trigger("holdColorChanged");
        }
    }

    this.setHue = function(deg) {
        if(self.selectedGrade) {
            // hue to rgb hsl (deg, 70%, 60%)
            self.selectedGrade.color = hslToRgb(deg/360, 0.7, 0.6);
            self.trigger("gradeColorChanged");
        } else {
            self.selectedHold.colorOfHolds = hslToRgb(deg/360, 0.7, 0.6);
            self.trigger("holdColorChanged");
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
            self.selectedGrade = JSON.parse(JSON.stringify(self.selectedGrade));
        }
        self.trigger("gradesChanged");
        self.downloadRoutes();
    }

    this.changeGradeName = function(name) {
        if(self.selectedGrade) {
            self.selectedGrade.name = name;
        } else {
            self.selectedHold.name = name;
        }
    }

    this.gradesSwap = function(diff, change) {
        var index = viewModel.grades.findIndex(function(elm) {return elm.difficulty == diff});
        if(index < 0 || index+change < 0 || index+change >= self.grades.length)
            return;
        
        var gradeAid = self.grades[index].id;
        var gradeBid = self.grades[index+change].id;

        self.client.grades.swapGrades(gradeAid, gradeBid, function(response) {
            if(response.success) {
                self.downloadGrades();
            }
            else
                self.dialogService.showMessage(response.message);
        });
    }

    this.downloadMembers = function() {
        self.client.members.getAllMembers(function(response) {
            if(response.success) {
                self.members = response.data;
                self.members.sort(function(a,b) {return a.displayName > b.displayName});
                self.trigger("membersChanged");
            }
        });
    }

    this.selectMember = function(id) {
        // how do we lookup member on a route?
    }

    this.toggleAdmin = function(id, isAdmin) {
        self.client.members.changeRole(id, (isAdmin ? "Authenticated" : "Admin"), function(response) {
            if(response.success) {
                var member = self.members.filter(function (s) { return s.id == id; })[0];
                member.isAdmin = !member.isAdmin;
                self.trigger("membersChanged");
            }
            else
                self.dialogService.showMessage(response.message);
        });
    }
///////////////////////////////////
    this.downloadHolds = function()
    {
        self.client.holds.getAllHolds(function(response) {
            if(response.success)
            {
                self.holds = response.data;
                for (var i = self.holds.length - 1; i >= 0; i--) {
                    self.holds[i].value = i;
                };
                self.trigger("holdsChanged");
            }
            else
                self.dialogService.showMessage(response.message);
        })
    }

    this.createHold = function()
    {
        var name = "New Hold/Tape";
        var newHold = self.holds[0];
        newHold = JSON.parse(JSON.stringify(newHold));
        newHold.name = name;
        newHold.colorOfHolds = {r: 0x66, b: 0x66, g: 0x66};
        newHold.value = self.holds.length;
        delete newHold.id;
        self.client.holds.addHold(newHold, function(response) {
            if(response.success) {
                self.holds.push(response.data);
                self.holds[self.holds.length-1].value = self.holds.length-1;
                self.selectedHold = self.holds[self.holds.length-1];           
            }
            else {
                self.dialogService.showMessage(response.message);
                self.downloadHolds();
                self.selectedHold = null;
            }
            self.trigger("holdsChanged");
        });
    }

    this.updateHold = function()
    {
        self.selectedHold.colorOfHolds.a = 255;
        self.client.holds.deleteHold(self.selectedHold.id, function(response) {
            if(response.success) {

                delete self.selectedHold.id;
                self.client.holds.addHold(self.selectedHold, function(response) {
                    if(response.success) {
                        self.holds.push(response.data);
                        self.selectedHold = self.holds[self.holds.length-1];           
                    }
                    else
                        self.dialogService.showMessage(response.message);
                    self.downloadHolds();
                    self.selectedHold = null;
                    self.trigger("holdsChanged");
                });
                
            }
            else {
                self.dialogService.showMessage(response.message);
            }
        })
    }

    this.deleteHold = function()
    {
        if(self.holds.length <= 1) {
            alert("You can't have a climbing club without holds...");
            return;
        }
        if(self.selectedHold != null && self.dialogService.confirm("Do you really want to permanently delete this hold?"))
        {
            self.client.holds.deleteHold(self.selectedHold.id, function(response) {
                if(response.success) {
                    self.selectedHold = null;
                    self.downloadHolds();
                }
                else {
                    self.dialogService.showMessage(response.message);
                }
            });
        }
    }   

    this.selectHold = function(holdValue)
    {
        self.selectedSection = null;
        if(holdValue === undefined || holdValue === null) {
            self.selectedHold = null;
            self.holdName = "";
        }
        else {
            self.selectedHold = self.holds.filter(function(g) { return g.value == holdValue; })[0];
            self.selectedHold = JSON.parse(JSON.stringify(self.selectedHold));
        }
        self.trigger("holdsChanged");
        self.downloadRoutes();
    } 

}
AdminPanelViewModel.prototype = new EventNotifier();


    
      ;;;;       ;;       ;;;;;;;  ;;;;;;;   ;;;;;;;  ;;;;;;
    ;;;  ;;;     ;;       ;;       ;;   ;;;  ;;       ;;   ;;
  ;;;     ;;;    ;;       ;;;;;    ;;;;;;;   ;;;;;    ;;    ;;
 ;;;;;;;;;;;;;   ;;       ;;       ;;  ;;;   ;;       ;;   ;;
;;;         ;;;  ;;;;;;;  ;;       ;;   ;;;  ;;;;;;;  ;;;;;;