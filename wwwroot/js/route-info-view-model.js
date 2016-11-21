function RouteInfoViewModel(client, changed)
{
    var viewModel = 
    {
        init: function()
        {
            client.routes.getRoute(window.location.search.split("routeId=")[1], function(routeResponse) {
                if(routeResponse.success)
                {
                    viewModel.route = routeResponse.data;
                    viewModel.route.date = viewModel.route.createdDate.split("T")[0].split("-").reverse().join("/");
                    viewModel.route.colorOfHolds = viewModel.route.colorOfHolds;
                    viewModel.route.grade = viewModel.route.grade;
                    viewModel.client.sections.getSection(viewModel.route.sectionId, function (sectionResponse) {
                        if(sectionResponse.success)
                        {
                            viewModel.route.sectionName = sectionResponse.data.name;
                            viewModel.changed();
                        }
                    });
                }
            });
        },
        client: client,
        changed: changed,
        grade: null,
        editRoute: function()
        {
            if(viewModel.route != null)
            {
                window.location = "edit-route.html?routeId=" + viewModel.route.routeId;
            }
        },
        deleteRoute: function()
        {
            if(viewModel.route != null && confirm("Do you really want to delete this route?"))
            {
                viewModel.client.routes.deleteRoute(viewModel.route.routeId, function(response) {
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