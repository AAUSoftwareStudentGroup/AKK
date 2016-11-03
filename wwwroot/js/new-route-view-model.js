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
            { value: 0, name: "Green"},
            { value: 1, name: "Blue"},
            { value: 2, name: "Red"},
            { value: 3, name: "Black"},
            { value: 4, name: "White"}
        ],
        holdColors: [
            { value: 0, name: "Cyan", color: 65535},
            { value: 1, name: "Azure", color: 65535},
            { value: 2, name: "Blue", color: 65535},
            { value: 3, name: "Violet", color: 65535},
            { value: 4, name: "Magenta", color: 65535},
            { value: 5, name: "Rose", color: 65535},
            { value: 6, name: "Red", color: 65535},
            { value: 7, name: "Orange", color: 65535},
            { value: 8, name: "Yellow", color: 65535},
            { value: 9, name: "Chartreuse Green", color: 65535},
            { value: 10, name: "Green", color: 65535},
            { value: 11, name: "Black", color: 65535},
            { value: 12, name: "Brown", color: 65535},
            { value: 13, name: "Grey", color: 65535},
            { value: 14, name: "White", color: 65535}
        ],
        changeSection: function(sectionId)
        {
            viewModel.selectedSection = viewModel.sections.filter(function(s) { return s.sectionId == sectionId; })[0];
        },
        changeGrade: function(gradeValue)
        {
            viewModel.selectedGrade = viewModel.grades.filter(function(g) { return g.value == gradeValue; })[0];
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
                var gradeValue = viewModel.selectedGrade.value;
                var holdColor = viewModel.selectedColor.color;
                var routeNumber = viewModel.routeNumber;
                var author = viewModel.author;
                viewModel.client.routes.addRoute(sectionId, routeNumber, author, holdColor, gradeValue, function(response) {
                    if(response.success)
                    {
                        window.history.back();
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
                var holdColor = viewModel.selectedColor.value;
                var routeNumber = viewModel.routeNumber;
                var author = viewModel.author;
                viewModel.client.routes.addRoute(sectionId, author, routeNumber, gradeValue, holdColor, function(response) {
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