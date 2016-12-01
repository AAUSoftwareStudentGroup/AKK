var viewModel;
var headerViewModel;
$(document).ready(function () {
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL, new CookieService());
    headerViewModel = new HeaderViewModel("Find Route", client);
    viewModel = new RoutesViewModel(client);

    var content = [
        {
            scriptSource: "js/templates/header-template.handlebars", 
            elementId: "header", 
            event: "headerUpdated",
            viewmodel: headerViewModel
        },
        {
            scriptSource: "js/templates/route-filtering-template.handlebars", 
            elementId: "filter-content", 
            event: "filteringChanged",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/route-list-template.handlebars", 
            elementId: "routes-content", 
            event: "routesChanged",
            viewmodel: viewModel
        }
    ];

    setUpContentUpdater(content, function() {
        viewModel.init();
        headerViewModel.init();
    });

    $(document).on("click", "#search-toggler", function() {
        $('#search-field').focus();
    });

    $(document).on("input", "#search-field", function() {
        viewModel.search($("#search-field").val());
    });
});