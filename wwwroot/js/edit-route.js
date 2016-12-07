var navigationService;
var headerViewModel;
var viewModel;
var client;
var rc;

$(document).ready(function () {
    navigationService = new NavigationService();
    client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL, new CookieService());
    headerViewModel = new HeaderViewModel("Edit Route", client, "/route-info?routeId=" + navigationService.getParameters()['routeId']);
    viewModel = new EditRouteViewModel(client, navigationService, new DialogService());

    //Sets up the edit-route page with each section of the edit-route page, and the corresponding events, which will change each section when called
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

    //Adds the new events
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