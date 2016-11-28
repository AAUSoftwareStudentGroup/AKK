﻿var viewModel;
var headerViewModel;
$(document).ready(function () {
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService());
    headerViewModel = new HeaderViewModel("New Route", client, new CookieService());
    viewModel = new NewRouteViewModel(client, new NavigationService());
    
    var content = [
        {
            scriptSource: "js/templates/header-template.handlebars", 
            elementId: "header", 
            event: "headerUpdated",
            callback: function() {
                headerViewModel.init();
            },
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
            callback: function() {
                viewModel.init();
            },
            viewmodel: viewModel
        }
    ];

    for (var i = 0; i < content.length; i++) {
        setUpContentUpdater(content[i]);
    }
});