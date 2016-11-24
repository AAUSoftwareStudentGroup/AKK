var viewModel;
$(document).ready(function () {
    $.get("js/templates/header-template.handlebars",
        function (response) {
            var template = Handlebars.compile($("#sections-template").html());
            var templateheader = Handlebars.compile(response);
            var sectionTemplate = Handlebars.compile($("#sectionsArea-template").html());
            var routeTemplate = Handlebars.compile($("#routes-template").html());
            var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
            viewModel = new SectionsViewModel(client);
            viewModel.addEventListener("DoneLoading", function changed() {
                $("#header").html(templateheader({ viewModel: viewModel, title: "Sections-panel", location: "/"}));
                $('#content').html(template(viewModel));
                $('#section-input-' + viewModel.selectedSection.name).prop("selected", true);
            });
            viewModel.addEventListener("SectionsUpdated", function () {
                $('#sectionArea').html(sectionTemplate(viewModel));
            });
            viewModel.addEventListener("RoutesUpdated", function () {
                $('#routeList').html(routeTemplate(viewModel));
            });
            viewModel.init();
        });
});