var viewModel;
$(document).ready(function () {
    var template = Handlebars.compile($("#sections-template").html());
    var sectionTemplate = Handlebars.compile($("#sectionsArea-template").html());
    var routeTemplate = Handlebars.compile($("#routes-template").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL);
    viewModel = new SectionsViewModel(client);
    viewModel.addEventListener("DoneLoading", function changed() {
        $('#content').html(template(viewModel));
        $('#section-input-' + viewModel.selectedSection.name).prop("selected", true);
    });
    viewModel.addEventListener("SectionsUpdated", function() {
        $('#sectionArea').html(sectionTemplate(viewModel));
    });
    viewModel.addEventListener("RoutesUpdated", function() {
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