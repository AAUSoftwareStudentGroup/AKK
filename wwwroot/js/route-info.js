var viewModel;

function NavigationService() {
    this.back = function() { window.location.replace("routes.html"); };
    this.location = function(routeId) {
        window.location = "edit-route.html?routeId=" + routeId;
    };
}

$(document).ready(function () {
    var template = Handlebars.compile($("#route-info-template").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
    
    viewModel = new RouteInfoViewModel(client, new NavigationService());
    viewModel.AddEventListener("ContentUpdated", function() { 
        $('#content').html(template(viewModel)); 
        var rc = new RouteCanvas($("#routeimage")[0], viewModel.routeImage, viewModel, true);
        rc.DrawCanvas();
    });
    viewModel.init();
});
Handlebars.registerHelper('ifCond', function (v1, v2, options) {
    if (v1.g <= v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});