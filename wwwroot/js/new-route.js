﻿var viewModel;
var headerViewModel;
$(document).ready(function () {
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL, new CookieService());
    headerViewModel = new HeaderViewModel("New Route", client, "/");
    viewModel = new NewRouteViewModel(client, new NavigationService(), new DialogService());

    var configurations = [
        {
            scriptSource: "js/templates/header-template.handlebars", 
            elementId: "header", 
            event: "headerUpdated",
            viewmodel: headerViewModel
        },
        {
            scriptSource: "js/templates/section-picker-template.handlebars", 
            elementId: "section-picker-content", 
            event: "sectionsUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/grade-picker-template.handlebars",
            elementId: "grade-picker-content",
            event: "gradesUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/number-picker-template.handlebars",
            elementId: "number-picker-content",
            event: "numberUpdated",
            viewmodel: viewModel
            
        },
        {
            scriptSource: "js/templates/author-picker-template.handlebars",
            elementId: "author-picker-content",
            event: "authorUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/hold-picker-template.handlebars",
            elementId: "hold-picker-content",
            event: "holdsUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/image-picker-template.handlebars",
            elementId: "image-picker-content",
            event: "imageUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/note-picker-template.handlebars",
            elementId: "note-picker-content",
            event: "noteUpdated",
            viewmodel: viewModel
        }
    ];

    setUpContentUpdater(configurations, function() {
        viewModel.addEventListener("imageUpdated", function() {
            if (viewModel.hasImage) {
                rc = new RouteCanvas($("#route-edit-image")[0], viewModel.image, viewModel, true);
                rc.DrawCanvas();
            }
        });
        viewModel.init();
        headerViewModel.init();
    });
});