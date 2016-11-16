
function RoutesViewModel(client, changed)
{
    var viewModel = {
        init: function()
        {   viewModel.grades = [{ difficulty: -1, name: "All" }];
            viewModel.getGrades();
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
            { difficulty: -1, name: "All" }
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
        selectedSection: null,
        selectedColor: null,
        selectedTape: null,
        selectedSortBy: null,
        routes: [],
        refreshRoutes: function()
        {
            var gradeValue = viewModel.selectedGrade.difficulty == -1 ? null : viewModel.selectedGrade.difficulty;
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
                  //      viewModel.routes[i].colorOfHolds = (Math.floor(viewModel.routes[i].colorOfHolds / 256)).toString(16);
                        viewModel.routes[i].selectedColor = viewModel.routes[i].colorOfHolds;
                    }
                    viewModel.changed();
                }
            });
        },
        changeGrade: function(gradeValue)
        {
            viewModel.selectedGrade = viewModel.grades.filter(function(grade){ return grade.difficulty == gradeValue; })[0];
            viewModel.refreshRoutes();
        },
        getGrades: function()
        {
            viewModel.client.grades.getAllGrades(function(response) {
                if(response.success)
                {
                    viewModel.grades = viewModel.grades.concat(response.data);
                }
            })
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


