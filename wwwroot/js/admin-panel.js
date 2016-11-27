﻿var headerViewModel;
var viewModel;

$(document).ready(function () {
    $.get("js/templates/header-template.handlebars",
        function (response) {
            var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService());
            var headerTemplate = Handlebars.compile(response);

            headerViewModel = new HeaderViewModel("Admin Panel", client, new CookieService());
            headerViewModel.addEventListener("headerUpdated", function () {
                $('#header').html(templateheader(headerViewModel));
            });

            //var gradesTemplate = Handlebars.compile($("#admin-grades-template").html());
            var sectionsTemplate = Handlebars.compile($("#admin-sections-template").html());
            var routesTemplate = Handlebars.compile($("#admin-routes-template").html());


            viewModel = new AdminPanelViewModel(client, new DialogService());
            viewModel.addEventListener("DoneLoading", function changed() {
                $("#header").html(headerTemplate({ viewModel: viewModel, title: "Admin Panel"}));
                //$('.grade-picker').html(gradesTemplate(viewModel));
                $('.section-picker').html(sectionsTemplate(viewModel));
            });

            viewModel.addEventListener("SectionsUpdated", function () {
                $('.section-picker').html(sectionsTemplate(viewModel));
                $('#section-input-' + viewModel.selectedSection.name).prop("selected", true);
            });

            viewModel.addEventListener("RoutesUpdated", function () {
                $('.section-details').html(routesTemplate(viewModel));
            });


            headerViewModel.init();
            viewModel.init();
        });
});