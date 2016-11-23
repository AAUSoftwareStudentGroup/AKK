var viewModel;
$(document).ready(function () {
    var template = Handlebars.compile($("#sections-template").html());
    var sectionTemplate = Handlebars.compile($("#sectionsArea-template").html());
    var routeTemplate = Handlebars.compile($("#routes-template").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
    var changed = function changed() {
        $('#content').html(template(viewModel));
        $('#section-input-' + viewModel.selectedSection.name).prop("selected", true);
    };
    var changed2 = function changed2() {
        $('#sectionArea').html(sectionTemplate(viewModel));
    };
    var changed3 = function changed3() {
        $('#routeList').html(routeTemplate(viewModel));
    };
    viewModel = new SectionsViewModel(client, changed, changed2, changed3);
});
Handlebars.registerHelper('ifCond', function (v1, v2, options) {
    if (v1.g <= v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});