
function SectionsViewModel(client, changed)
{
	    var viewModel = {
        init: function()
        {
            viewModel.getGrades();
            viewModel.client.sections.getAllSections(function(response)
            {
                if(response.success)
                {
                    viewModel.sections = viewModel.sections.concat(response.data);
                    viewModel.selectedGrade = viewModel.grades[0];
                    viewModel.selectedSection = viewModel.sections[0];
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
            { value: -1, name: "All"}],
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
                        viewModel.routes[i].selectedColor = viewModel.routes[i].colorOfHolds;
                    }
                    viewModel.changed();
                }
            });
        },
        refreshSections: function()
        {
            viewModel.client.sections.getAllSections(function(response) {
                if(response.success) {
                    viewModel.sections = response.data;

                    viewModel.changed();
                }
            });
        },
        getSectionDetails: function(sectionId)
        {
            viewModel.selectedSection = viewModel.sections.filter(function(section){ return section.sectionId == sectionId; })[0];
          //  viewModel.selectedSection = viewModel.client.sections.getSection(viewModel.selectedSection.sectionId);
         /* var response;
            viewModel.client.routes.getRoutes(viewModel.grades[0], viewModel.selectedSection.sectionId, viewModel.sortOptions[0], function(response) {
                if(response.success)
                    viewModel.refreshRoutes();
            });*/
            viewModel.refreshRoutes();

        },
        getGrades: function()
        {
            viewModel.client.grades.getAllGrades(function(response) {
                if(response.success)
                {
                    viewModel.grades = viewModel.grades.concat(response.data);
                    for (var i = 1; i < viewModel.grades.length; i++) {
                       // viewModel.grades[i].name = "Green";
                    };
                  //  viewModel.changed();
                }
            })
        },
        addNewSection: function()
        {
            var name = prompt("Enter name of new Section","");
            var response;
            viewModel.client.sections.addSection(name, function(response) {
                if(response.success)
                    viewModel.refreshSections();
            });
        },
        clearSection: function()
        {
            if(viewModel.selectedSection != null && confirm("Do you really want to remove all routes from this section?"))
            {
                viewModel.client.sections.deleteSectionRoutes(viewModel.selectedSection.sectionId, function(response) {
                    if(response.success)
                        viewModel.refreshRoutes();
                });
            }
        },
        deleteSection: function()
        {
            if(viewModel.selectedSection != null && confirm("Do you really want permanently delete this section?"))
            {
                viewModel.client.sections.deleteSection(viewModel.selectedSection.sectionId, function(response) {
                    if(response.success)
                        viewModel.refreshSections();
                });
            }
        },
        renameSection: function()
        {
            var newName = prompt("Enter the new name","");
            if(viewModel.selectedSection != null && confirm("Do you really want to rename this section?"))
            {
                viewModel.client.sections.renameSection(viewModel.selectedSection.sectionId, newName, function(response) {
                    if(response.success)
                        viewModel.refreshSections();
                });
            }
        }
    };
    viewModel.init();
    return viewModel;
}