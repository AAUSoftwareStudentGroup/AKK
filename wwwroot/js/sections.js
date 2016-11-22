var viewModel;
$(document).ready(function () {
    var template = Handlebars.compile($("#sections-template").html());
    var sectionTemplate = Handlebars.compile($("#sectionsArea-template").html());
    var routeTemplate = Handlebars.compile($("#routes-template").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
    viewModel = new SectionsViewModel(client);
    viewModel.AddEventListener("DoneLoading", function changed() {
        $('#content').html(template(viewModel));
        $('#section-input-' + viewModel.selectedSection.name).prop("selected", true);
    });
    viewModel.AddEventListener("SectionsUpdated", function changed2() {
        $('#sectionArea').html(sectionTemplate(viewModel));
    });
    viewModel.AddEventListener("RoutesUpdated", function changed3() {
        $('#routeList').html(routeTemplate(viewModel));
    });
    viewModel.init();
});
Handlebars.registerHelper('ifCond', function (v1, v2, options) {
    if (v1.g <= v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});