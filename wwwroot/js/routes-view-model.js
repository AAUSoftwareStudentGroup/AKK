
function RoutesViewModel(client, changed)
{
    var viewModel = {
        init: function()
        {
            viewModel.client.sections.getAllSections(function(response)
            {
                if(response.success)
                {
                    viewModel.sections = [{ sectionId: -1, name: "All" }];
                    viewModel.sections = viewModel.sections.concat(response.data);
                    viewModel.selectedGrade = viewModel.grades[0];
                    viewModel.selectedSection = viewModel.sections[0];
                    viewModel.selectedSortBy = viewModel.sortOptions[0];
                    viewModel.refreshRoutes();
                    viewModel.changed();
                }
            });
        },
        client: client,
        changed: changed,
        grades: [
            { value: -1, name: "All"},
            { value: 0, name: "Green"},
            { value: 1, name: "Blue"},
            { value: 2, name: "Red"},
            { value: 3, name: "Black"},
            { value: 4, name: "White"}
        ],
        holdColors: [
            { value: 0, name: "Green"},
            { value: 1, name: "Blue"},
            { value: 2, name: "Black"}
        ],
        sections: [
            { sectionId: -1, name: "All" }
        ],
        sortOptions: [
            { value: 0, name: "Newest" },
            { value: 1, name: "Oldest" },
            { value: 2, name: "Grading" },
            { value: 3, name: "Author" },
        ],
        selectedGrade: null,
        selectedColor: null,
        selectedSection: null,
        selectedSortBy: null,
        routes: [],
        refreshRoutes: function()
        {
            var gradeValue = viewModel.selectedGrade.value == -1 ? null : viewModel.selectedGrade.value;
            var colorValue = viewModel.selectedColor.value == -1 ? null : viewModel.selectedColor.value;
            var sectionId = viewModel.selectedSection.sectionId == -1 ? null : viewModel.selectedSection.sectionId;
            var sortByValue = viewModel.selectedSortBy.value == -1 ? null : viewModel.selectedSortBy.value;
            viewModel.client.routes.getRoutes(gradeValue, sectionId, sortByValue, function(response) {
                if(response.success)
                {
                    viewModel.routes = response.data;
                    for(var i = 0; i < viewModel.routes.length; i++)
                    {
                        viewModel.routes[i].sectionName = viewModel.sections.filter(function(s) { 
                            return s.sectionId == viewModel.routes[i].sectionId; 
                        })[0].name;
                        viewModel.routes[i].date = viewModel.routes[i].createdDate.split("T")[0].split("-").reverse().join("/");
                        viewModel.routes[i].colorOfHolds = (Math.floor(viewModel.routes[i].colorOfHolds / 256)).toString(16);
                    }
                    viewModel.changed();
                }
            });
        },
        changeGrade: function(gradeValue)
        {
            viewModel.selectedGrade = viewModel.grades.filter(function(grade){ return grade.value == gradeValue; })[0];
            viewModel.refreshRoutes();
        },
        changeHoldColor: function(colorValue)
        {
            viewModel.selectedColor = viewModel.holdColors.filter(function(holdColor)){ return holdColor.value == colorValue; })[0];
            viewModel.refreshRoutes();
        },
        changeSection: function(sectionId)
        {
            viewModel.selectedSection = viewModel.sections.filter(function(section){ return section.sectionId == sectionId; })[0];
            viewModel.refreshRoutes();
        },
        changeSortBy: function(sortByValue)
        {
            viewModel.selectedSortBy = viewModel.sortOptions.filter(function(sortBy){ return sortBy.value == sortByValue; })[0];
            viewModel.refreshRoutes();
        }
    };
    viewModel.init();
    return viewModel;
}


