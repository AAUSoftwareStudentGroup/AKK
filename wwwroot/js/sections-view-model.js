
function SectionsViewModel(client, changed)
{
	    var viewModel = {
        init: function()
        {
            viewModel.client.sections.getAllSections(function(response)
            {
                if(response.success)
                {
                    viewModel.sections = viewModel.sections.concat(response.data);
                    viewModel.selectedGrade = viewModel.grades[0];
                   // viewModel.selectedSection = viewModel.sections[0];
                    viewModel.selectedSortBy = viewModel.sortOptions[0];
                   // viewModel.refreshRoutes();
                    viewModel.changed();
                }
            });
        },
        client: client,
        changed: changed,
        selectedSection: null,
        selectedGrade: null,
        routes: [],
        grades: [
            { value: -1, name: "All"},
            { value: 0, name: "Green"},
            { value: 1, name: "Blue"},
            { value: 2, name: "Red"},
            { value: 3, name: "Black"},
            { value: 4, name: "White"}
        ],
        sections: [],
        sortOptions: [
            { value: 0, name: "Newest" },
            { value: 1, name: "Oldest" },
            { value: 2, name: "Grading" },
            { value: 3, name: "Author" },
        ],
        refreshRoutes: function()
        {
            var gradeValue = viewModel.selectedGrade.value == -1 ? null : viewModel.selectedGrade.value;
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
        getSectionDetails: function(sectionId)
        {
            viewModel.selectedSection = viewModel.sections.filter(function(section){ return section.sectionId == sectionId; })[0];
          //  viewModel.selectedSection = viewModel.client.sections.getSection(viewModel.selectedSection.sectionId);
          var response;
            viewModel.client.routes.getRoutes(viewModel.grades[0], viewModel.selectedSection.sectionId, viewModel.sortOptions[0], function(response) {
                if(response.success)
                    viewModel.routes = response.data;
            });
            viewModel.refreshRoutes();
        }
    };
    viewModel.init();
    return viewModel;
}