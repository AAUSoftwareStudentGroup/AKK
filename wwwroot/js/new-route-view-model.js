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
            { value: 0, name: "Cyan", color: "00c8c8"},
            { value: 1, name: "Azure", color: "017EFF"},
            { value: 2, name: "Blue", color: "0000FF"},
            { value: 3, name: "Violet", color: "7F00FF"},
            { value: 4, name: "Magenta", color: "FF00FF"},
            { value: 5, name: "Rose", color: "FF017F"},
            { value: 6, name: "Red", color: "FF0000"},
            { value: 7, name: "Orange", color: "FF7F00"},
            { value: 8, name: "Yellow", color: "e4dc00"},
            { value: 9, name: "Chartreuse Green", color: "79FF00"},
            { value: 10, name: "Green", color: "00e900"},
            { value: 11, name: "Black", color: "000000"},
            { value: 12, name: "Brown", color: "7E360F"},
            { value: 13, name: "Grey", color: "5c5959"},
            { value: 14, name: "White", color: "D0D0D0"}
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
                var holdColor = parseInt(viewModel.selectedColor.color, 16) * 256;
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