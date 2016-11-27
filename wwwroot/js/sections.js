var headerViewModel;
var viewModel;

$(document).ready(function () {
    $.get("js/templates/header-template.handlebars",
        function (response) {
            var template = Handlebars.compile($("#sections-template").html());
            var templateheader = Handlebars.compile(response);
            var sectionTemplate = Handlebars.compile($("#sectionsArea-template").html());
            var routeTemplate = Handlebars.compile($("#routes-template").html());
            var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService());

            headerViewModel = new HeaderViewModel("Admin Panel", client, new CookieService());
            headerViewModel.addEventListener("headerUpdated", function () {
                $('#header').html(templateheader(headerViewModel));
            });

            viewModel = new SectionsViewModel(client, new DialogService());
            viewModel.addEventListener("DoneLoading", function changed() {
                $('#content').html(template(viewModel));
                $('#section-input-' + viewModel.selectedSection.name).prop("selected", true);
            });

            viewModel.addEventListener("SectionsUpdated", function () {
                $('#sectionArea').html(sectionTemplate(viewModel));
            });

            viewModel.addEventListener("RoutesUpdated", function () {
                $('#routeList').html(routeTemplate(viewModel));
            });

            headerViewModel.init();
            viewModel.init();
        });
});