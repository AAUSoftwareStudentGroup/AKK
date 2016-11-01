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
                    viewModel.route.colorOfHolds = (Math.floor(viewModel.route.colorOfHolds / 256)).toString(16);
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
        route: null,
        deleteRoute: function()
        {
            if(viewModel.route != null)
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