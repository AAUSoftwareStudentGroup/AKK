var viewModel;

var rc;
var fullwidth = false;
$(document).ready(function () {
    var template = Handlebars.compile($("#route-info-template").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL);

    viewModel = new RouteInfoViewModel(client, new NavigationService());
    viewModel.addEventListener("ContentUpdated", function() { 
        $('#content').html(template(viewModel)); 
        if (viewModel.hasImage) {
            rc = new RouteCanvas($("#routeimage")[0], viewModel.route.image, viewModel, false);
            rc.DrawCanvas();
        }
    });
    viewModel.init();
});
Handlebars.registerHelper('if_eq', function (a, b, opts) {
        if (a == b) // Or === depending on your needs
            return opts.fn(this);
        else
            return opts.inverse(this);
    });
Handlebars.registerHelper('ifCond', function (v1, v2, options) {
    if (v1.g <= v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});

$(document).on("click", "#routeimage", function() {
    var el = $("#routeimage");
    el.toggleClass("routeimagefullscreen");
    el.toggleClass("routeimagesmall");
    fullwidth = !fullwidth;
    rc.resize();
    rc.DrawCanvas();
});

