
function RoutesViewModel(client, changed)
{
    var viewModel = {
        init: function()
        {
            viewModel.selectedGrade = viewModel.grades[0];
            viewModel.selectedSection = viewModel.sections[0];
            viewModel.selectedSortBy = viewModel.sortOptions[0];
            viewModel.refreshRoutes();
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
        sections: [
            { value: "-1", name: "All" },
            { value: "4803675e-d55e-44f8-b6bd-c9d88fe93eaf", name: "A" },
            { value: "5f16da85-2482-4346-ac31-466b6d407dec", name: "C" },
            { value: "4fbc639a-48a6-49ab-9554-298e3af6654a", name: "B" },
            { value: "ed5fe8fa-c27f-41f5-ad6e-9449b4d9ba50", name: "D" }
        ],
        sortOptions: [
            { value: 0, name: "Newest" }
        ],
        selectedGrade: null,
        selectedSection: null,
        selectedSortBy: null,
        routes: [],
        refreshRoutes: function()
        {
            viewModel.client.routes.getRoutes(
                viewModel.selectedGrade.value == -1 ? null : viewModel.selectedGrade.value, 
                viewModel.selectedSection.value == -1 ? null : viewModel.selectedSection.value, 
                viewModel.selectedSortBy.value, 
                function(response) {
                if(response.success)
                {
                    viewModel.routes = response.data;
                    for(var i = 0; i < viewModel.routes.length; i++)
                    {
                        viewModel.routes[i].sectionName = viewModel.sections.filter(function(section){
                            return section.value == viewModel.routes[i].sectionId;
                        })[0].name;
                        viewModel.routes[i].date = viewModel.routes[i].createdDate.split("T")[0].split("-").reverse().join("/");
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
        changeSection: function(sectionValue)
        {
            viewModel.selectedSection = viewModel.sections.filter(function(section){ return section.value == sectionValue; })[0];
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


