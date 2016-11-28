function EditRouteViewModel(client, navigationService) {
    RouteViewModel.apply (this, arguments);
    var self = this;
    this.init = function() {
        var routeId = this.navigationService.getParameters()["routeId"];
        this.client.routes.getRoute(routeId, function(response) {
            if (response.success) {
                self.downloadSections(function() {
                    self.changeSection(response.data.sectionId);
                    self.trigger("sectionsUpdated");
                });

                self.downloadGrades(function() {
                    self.changeGrade(response.data.gradeId);
                    self.trigger("gradesUpdated");
                });

                self.changeNumber(response.data.name);
                self.trigger("numberUpdated");

                self.changeAuthor(response.data.author);
                self.trigger("authorUpdated");

                self.changeHold(response.data.colorOfHolds);
                if (response.data.colorOfTape) {
                    self.changeTape(response.data.colorOfTape);
                    self.toggleTape();
                }
                self.trigger("holdsUpdated");

                self.downloadImage();

                self.changeNote(response.data.note);
                self.trigger("noteUpdated");
            } else {
                self.trigger("Error", response.message);
            }
        });
    }
    this.UpdateRoute = function() {
        
    }
    
}

/*function EditRouteViewModel(client, navigationService)
{
    var self = this;
    this.client = client;
    this.navigationService = navigationService;
    this.init = function() {
        self.routeId = navigationService.getParameters()["routeId"];
        self.client.grades.getAllGrades(function(gradesResponse) {
            if(gradesResponse.success)
            {
                self.grades = gradesResponse.data;
                self.client.sections.getAllSections(function(sectionsResponse)
                {
                    if(sectionsResponse.success)
                    {
                        self.sections = sectionsResponse.data;
                        self.client.routes.getRoute(self.routeId, function(routeResponse)
                        {
                            if(routeResponse.success)
                            {
                                self.changeSection(routeResponse.data.sectionId);
                                self.changeGrade(routeResponse.data.gradeId);
                                self.getHoldColor(routeResponse.data.colorOfHolds);
                                self.changeRouteNumber(routeResponse.data.name);
                                var temp = routeResponse.data.colorOfTape;
                                if(temp != null) {
                                    for(i = 0; i < self.holdColors.length; i++) {
                                        if(self.holdColors[i].r == temp.r && self.holdColors[i].g == temp.g && self.holdColors[i].b == temp.b) {
                                            self.selectedTapeColor = self.holdColors[i];
                                            break;
                                        }
                                    }
                                }
                                if(self.selectedTapeColor != null)
                                    self.hasTape = true;

                                
                                self.client.routes.getImage(self.routeId, function(imageResponse) {
                                    if (imageResponse.success) {
                                        self.hasImage = true;
                                        self.image = new Image();
                                        self.image.src = imageResponse.data.fileUrl;
                                        self.HoldPositions = imageResponse.data.holds;
                                        self.image.onload = function() {
                                            self.changeAuthor(routeResponse.data.author);
                                            self.trigger("OnGradeOrSectionChanged");
                                            self.trigger("OnColorChanged");
                                            self.trigger("OnImageChanged");
                                        }
                                    } else {
                                        self.changeAuthor(routeResponse.data.author);
                                        self.trigger("OnGradeOrSectionChanged");
                                        self.trigger("OnColorChanged");
                                        self.trigger("OnImageChanged");
                                    }
                                });
                            }
                            else
                            {
                                $("#error-message").html(response.message).show();
                            }
                        });
                    }
                    else
                    {
                        $("#error-message").html(response.message).show();
                    }
                });
            }
        })
        
    }
    this.hasImage = false;
    this.routeId = null;
    this.sections = [];
    this.selectedSection = null;
    this.selectedGrade = null;
    this.selectedColor = null;
    this.routeNumber = null;
    this.selectedTapeColor = null;
    this.hasTape = false;
    this.author = null;
    this.grades = [ ];
    this.HoldPositions = [ ];
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

    this.setImage = function(img) {
        this.image = img;
        this.image.width = img.width;
        this.image.height = img.height;
        this.hasImage = true;
        this.HoldPositions = [];
        this.trigger("OnImageChanged");
    };

    this.changeSection = function(sectionId)
    {
        self.selectedSection = self.sections.filter(function(s) { return s.id == sectionId; })[0];
    };
    this.changeGrade = function(gradeId)
    {
        self.selectedGrade = self.grades.filter(function(g) { return g.id == gradeId; })[0];
    };
    this.getHoldColor = function(holdColor) 
    {
        var temp = self.holdColors;
        self.selectedColor = temp[0];
        for(var i = 0; i < temp.length; i++) {
            if(temp[i].r == holdColor.r && temp[i].g == holdColor.g && temp[i].b == holdColor.b && temp[i].a == holdColor.a) {
                self.selectedColor = temp[i];
                return;
            }
        }
    };
    this.changeHoldColor = function(holdColor)
    {
        self.selectedColor = self.holdColors.filter(function(g) {return g.value == holdColor; })[0];
    };
    this.changeTapeColor = function(tapeColor)
    {
        self.selectedTapeColor = self.holdColors.filter(function(g) {return g.value == tapeColor; })[0];
    };
    this.changeRouteNumber = function(routeNumber)
    {
        self.routeNumber = routeNumber;
    };
    this.changeAuthor = function(author)
    {
        self.author = author;
    };
    this.getSections = function()
    {
        self.client.sections.getAllSections(function(response)
        {
            if(response.success)
            {
                self.sections = response.data;
                self.trigger("OnGradeOrSectionChanged");
            }
            else
            {
                $("#error-message").html(response.message).show();
            }
        });
    };
    this.addHold = function(hold) 
    {
        self.HoldPositions.push(hold);
        self.trigger("HoldsUpdated");
    },
    this.gradesGotTape =  function()
    {
        if(self.hasTape === true) {
            self.hasTape = false;
            self.selectedTapeColor = null;
        }
        else
            self.hasTape = true;
        self.trigger("OnColorChanged");
    },
    this.updateRoute = function()
    {
        if(self.selectedSection != null && self.author && self.selectedGrade != null && self.selectedColor != null && !isNaN(this.routeNumber))
        {
            var routeId = self.routeId;
            var sectionId = self.selectedSection.id;
            var gradeId = self.selectedGrade.id;
            var tapeColor = self.selectedTapeColor;
            var holdColor = self.selectedColor;
            var routeNumber = self.routeNumber;
            var author = self.author;
            var image = this.image;
            var imgObject = null;
            if (image != null) {
                imgObject = {
                    fileUrl : image.src,
                    width : image.width,
                    height : image.height,
                    holds : this.HoldPositions || []
                }
            }
            self.client.routes.updateRoute(routeId, sectionId, author, routeNumber, holdColor, gradeId, tapeColor, imgObject, function(response) {
                if(response.success)
                {
                    navigationService.toRouteInfo(routeId);
                }
                else
                {
                    $("#error-message").html(response.message).show();
                }
            });
        }
    }
}
EditRouteViewModel.prototype = new EventNotifier();
*/