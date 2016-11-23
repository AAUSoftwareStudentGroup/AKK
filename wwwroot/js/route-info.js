var viewModel;
$(document).ready(function () {
    var template = Handlebars.compile($("#route-info-template").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
    var changed = function changed() { $('#content').html(template(viewModel)); };

    viewModel = new RouteInfoViewModel(client, changed, new NavigationService());
});
Handlebars.registerHelper('ifCond', function (v1, v2, options) {
    if (v1.g <= v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});