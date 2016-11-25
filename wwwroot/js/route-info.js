var viewModel;
var rc;
$(document).ready(function () {
    $.get("js/templates/header-template.handlebars",
        function(response) {
            var template = Handlebars.compile($("#route-info-template").html());
            var templateheader = Handlebars.compile(response);
            var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL);

            viewModel = new RouteInfoViewModel(client, new NavigationService());
            viewModel.addEventListener("ContentUpdated",
                function () {
                    $("#header").html(templateheader({ viewModel: viewModel, title: "Route Information", location: "/" }));
                    $('#content').html(template(viewModel));
                    if (viewModel.hasImage) {
                        rc = new RouteCanvas($("#routeimage")[0], viewModel.route.image, viewModel, false);
                        rc.DrawCanvas();
                    }
                });
            viewModel.init();
        });
});

$(document).on("click", "#routeimage", function() {
    var el = $("#routeimage");
    el.toggleClass("routeimagefullscreen");
    el.toggleClass("routeimagesmall");
    rc.resize();
    rc.DrawCanvas();
});

