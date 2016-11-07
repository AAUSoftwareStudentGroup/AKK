function NewRouteViewModel(client, changed)
{
    var viewModel = {
        init: function()
        {
            viewModel.getSections();
        },
        client: client,
        changed: changed,
        sections: [],
        selectedSection: null,
        selectedGrade: null,
        selectedColor: null,
        routeNumber: null,
        author: null,
        grades: [ 
            { value: 0, difficulty: 0, color: [{r: 67, g: 160, b: 71, a: 1}]},
            { value: 1, difficulty: 1, color: [{r: 33, g: 150, b: 254, a: 1}]},
            { value: 2, difficulty: 2, color: [{r: 239, g: 83, b: 80, a: 1}]},
            { value: 3, difficulty: 3, color: [{r: 97, g: 97, b: 97, a: 1}]},
            { value: 4, difficulty: 4, color: [{r: 251, g: 251, b: 251, a: 1}]},
        ],
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
        changeHoldColor: function(holdColor)
        {
            viewModel.selectedColor = viewModel.holdColors.filter(function(g) {return g.value == holdColor; })[0];
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
            });
        },
        addRoute: function()
        {
            if(viewModel.selectedSection != null && viewModel.selectedGrade != null && viewModel.selectedColor != null && !isNaN(viewModel.routeNumber))
            {
                var sectionId = viewModel.selectedSection.sectionId;
                var gradeValue = viewModel.selectedGrade;
                var holdColor = viewModel.selectedColor;
                var routeNumber = viewModel.routeNumber;
                var author = viewModel.author;
                viewModel.client.routes.addRoute(sectionId, routeNumber, author, holdColor, gradeValue, function(response) {
                    if(response.success)
                    {
                        window.history.back();
                    }
                    else
                    {
                        $("#error-message").html(response.message);
                    }
                });
            }
        },
        updateRoute: function()
        {
            if(viewModel.selectedSection != null && viewModel.selectedGrade != null && viewModel.selectedColor != null && !isNaN(viewModel.routeNumber))
            {
                var sectionId = viewModel.selectedSection.sectionId;
                var gradeValue = viewModel.selectedGrade.value;
                var holdColor = parseInt(viewModel.selectedColor.color, 16) * 256;
                var routeNumber = viewModel.routeNumber;
                var author = viewModel.author;
                viewModel.client.routes.addRoute(sectionId, routeNumber, author, holdColor, gradeValue, function(response) {
                    if(response.success)
                    {
                        window.history.back();
                    }
                });
            }
        }
    };
    viewModel.init();
    return viewModel;
}