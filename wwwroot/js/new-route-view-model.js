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
        routeNumber: null,
        author: null,
        grades: [
            { value: 0, name: "Green"},
            { value: 1, name: "Blue"},
            { value: 2, name: "Red"},
            { value: 3, name: "Black"},
            { value: 4, name: "White"}
        ],
        changeSection: function(sectionId)
        {
            viewModel.selectedSection = viewModel.sections.filter(function(s) { return s.sectionId == sectionId; })[0];
        },
        changeGrade: function(gradeValue)
        {
            viewModel.selectedGrade = viewModel.grades.filter(function(g) { return g.value == gradeValue; })[0];
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
            if(viewModel.selectedSection != null && viewModel.selectedGrade != null && !isNaN(viewModel.routeNumber))
            {
                var sectionId = viewModel.selectedSection.sectionId;
                var gradeValue = viewModel.selectedGrade.value;
                var routeNumber = viewModel.routeNumber;
                var author = viewModel.author;
                viewModel.client.routes.addRoute(sectionId, author, routeNumber, gradeValue, function(response) {
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