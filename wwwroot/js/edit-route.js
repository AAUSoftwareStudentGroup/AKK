var viewModel;
var headerViewModel;
var rc;
var client;

$(document).ready(function () {
    client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService());
    headerViewModel = new HeaderViewModel("Edit Route", client, new CookieService());
    viewModel = new EditRouteViewModel(client, new NavigationService());

    var content = [
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
            scriptSource: "js/templates/image-picker-template.handlebars",
            elementId: "image-picker-content",
            event: "imageUpdated",
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


    setUpContentUpdater(content, function() {
        viewModel.addEventListener("imageUpdated", function() {
            if (viewModel.hasImage) {
                rc = new RouteCanvas($("#route-edit-image")[0], viewModel.image, viewModel, true);
                rc.DrawCanvas();
            }
        });
        viewModel.addEventListener("Error", function(msg) {
            $("#error-message").html(msg).show();
        });
        viewModel.init();
        headerViewModel.init();
    });
    
});


