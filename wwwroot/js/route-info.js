var viewModel;

function NavigationService() {
    this.back = function() { window.location.replace("routes.html"); };
    this.location = function(routeId) {
        window.location = "edit-route.html?routeId=" + routeId;
    };
}

var rc;
var fullwidth = false;
$(document).ready(function () {
    var template = Handlebars.compile($("#route-info-template").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
    
    viewModel = new RouteInfoViewModel(client, new NavigationService());
    viewModel.addEventListener("ContentUpdated", function() { 
        $('#content').html(template(viewModel)); 
        rc = new RouteCanvas($("#routeimage")[0], viewModel.routeImage, viewModel, false);
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

$(document).on("click", "#routeimage", function() {
    var el = $("#routeimage");
    el.css("width", fullwidth ? "50%" : "100%");
    fullwidth = !fullwidth;
    rc.resize();
    rc.DrawCanvas();
});

