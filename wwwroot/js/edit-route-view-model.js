function EditRouteViewModel(client, changed, changed2)
{
    var viewModel = {
        init: function()
        {
            viewModel.routeId = window.location.search.split("routeId=")[1];
            viewModel.getGrades();
            viewModel.client.sections.getAllSections(function(response)
            {
                if(response.success)
                {
                    viewModel.sections = response.data;
                    viewModel.client.routes.getRoute(viewModel.routeId, function(routeResponse)
                    {
                        if(routeResponse.success)
                        {
                            viewModel.changeSection(routeResponse.data.sectionId);
                            viewModel.changeGrade(routeResponse.data.grade.difficulty);
                            viewModel.getHoldColor(routeResponse.data.colorOfHolds);
                            viewModel.changeRouteNumber(routeResponse.data.name);
                            var temp = routeResponse.data.colorOfTape;
                            if(temp != null) {
                                for(i = 0; i < viewModel.holdColors.length; i++) {
                                    if(viewModel.holdColors[i].r == temp.r && viewModel.holdColors[i].g == temp.g && viewModel.holdColors[i].b == temp.b) {
                                        viewModel.selectedTapeColor = viewModel.holdColors[i];
                                        break;
                                    }
                                }
                            }
                            if(viewModel.selectedTapeColor != null)
                                viewModel.hasTape = true;
                            viewModel.changeAuthor(routeResponse.data.author);
                            viewModel.changed();
                            viewModel.changed2();
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
        },
        routeId: null,
        client: client,
        changed: changed,
        changed2: changed2,
        sections: [],
        selectedSection: null,
        selectedGrade: null,
        selectedColor: null,
        routeNumber: null,
        selectedTapeColor: null,
        hasTape: false,
        author: null,
        grades: [ ],
        holdColors: [
            { value: 0, name: "Cyan", color: "00c8c8", r: 0, g: 200, b: 200, a: 1},
            { value: 1, name: "Azure", color: "017EFF", r: 1, g: 127, b: 255, a: 1},
            { value: 2, name: "Blue", color: "0000FF", r: 0, g: 0, b: 255, a: 1},
            { value: 3, name: "Violet", color: "7F00FF", r: 127, g: 0, b: 255, a: 1},
            { value: 4, name: "Magenta", color: "FF00FF", r: 255, g: 0, b: 255, a: 1},
            { value: 5, name: "Rose", color: "FF017F", r: 255, g: 1, b: 127, a: 1},
            { value: 6, name: "Red", color: "FF0000", r: 255, g: 0, b: 0, a: 1},
            { value: 7, name: "Orange", color: "FF7F00", r: 255, g: 127, b: 0, a: 1},
            { value: 8, name: "Yellow", color: "e4dc00", r: 228, g: 220, b: 0, a: 1},
            { value: 9, name: "Chartreuse Green", color: "79FF00", r: 121, g: 255, b: 0, a: 1},
            { value: 10, name: "Green", color: "00e900", r: 0, g: 233, b: 0, a: 1},
            { value: 11, name: "Black", color: "000000", r: 0, g: 0, b: 0, a: 1},
            { value: 12, name: "Brown", color: "7E360F", r: 127, g: 54, b: 15, a: 1},
            { value: 13, name: "Grey", color: "5c5959", r: 92, g: 89, b: 89, a: 1},
            { value: 14, name: "White", color: "D0D0D0", r: 208, g: 208, b: 208, a: 1},
        ],
        changeSection: function(sectionId)
        {
            viewModel.selectedSection = viewModel.sections.filter(function(s) { return s.sectionId == sectionId; })[0];
        },
        changeGrade: function(gradeValue)
        {
            viewModel.selectedGrade = viewModel.grades.filter(function(g) { return g.difficulty == gradeValue; })[0];
        },
        getGrades: function()
        {
            viewModel.client.grades.getAllGrades(function(response) {
                if(response.success)
                {
                    viewModel.grades = response.data;
                  //  viewModel.changed();
                }
            })
        },
        getHoldColor: function(holdColor)
        {
            var temp = viewModel.holdColors;
            viewModel.selectedColor = temp[0];
            for(var i = 0; i < temp.length; i++) {
                if(temp[i].r == holdColor.r && temp[i].g == holdColor.g && temp[i].b == holdColor.b && temp[i].a == holdColor.a) {
                    viewModel.selectedColor = temp[i];
                    return;
                }
            }
        },
        changeHoldColor: function(holdColor)
        {/*
            var temp = viewModel.holdColors;
            for(var i = 0; i < temp.length; i++) {
                if(temp[i].r == holdColor.r && temp[i].g == holdColor.g && temp[i].b == holdColor.b && temp[i].a == holdColor.a) {
                    viewModel.selectedColor = holdColor;
                    return;
                }
            }*/
           viewModel.selectedColor = viewModel.holdColors.filter(function(g) {return g.value == holdColor; })[0];
        },
        changeTapeColor: function(tapeColor)
        {
            viewModel.selectedTapeColor = viewModel.holdColors.filter(function(g) {return g.value == tapeColor; })[0];
        },
        changeRouteNumber: function(routeNumber)
        {
            viewModel.routeNumber = routeNumber;
        },
        changeAuthor: function(author)
        {
            viewModel.author = author;
        },
        getSections: function()
        {
            viewModel.client.sections.getAllSections(function(response)
            {
                if(response.success)
                {
                    viewModel.sections = response.data;
                    viewModel.changed();
                }
                else
                {
                    $("#error-message").html(response.message).show();
                }
            });
        },
        gradesGotTape: function()
        {
            if(viewModel.hasTape === true) {
                viewModel.hasTape = false;
                viewModel.selectedTapeColor = null;
            }
            else
                viewModel.hasTape = true;
            viewModel.changed2();
        },
        updateRoute: function()
        {
            if(viewModel.selectedSection != null && viewModel.selectedGrade != null && viewModel.selectedColor != null && !isNaN(viewModel.routeNumber))
            {
                var routeId = viewModel.routeId;
                var sectionId = viewModel.selectedSection.sectionId;
                var gradeValue = viewModel.selectedGrade;
                var tapeColor = viewModel.selectedTapeColor;
                var holdColor = viewModel.selectedColor;
                var routeNumber = viewModel.routeNumber;
                var author = viewModel.author;
                viewModel.client.routes.updateRoute(routeId, sectionId, routeNumber, author, holdColor, gradeValue, tapeColor, function(response) {
                    if(response.success)
                    {
                        window.history.back();
                    }
                    else
                    {
                        $("#error-message").html(response.message).show();
                    }
                });
            }
        }
    };
    viewModel.init();
    return viewModel;
}