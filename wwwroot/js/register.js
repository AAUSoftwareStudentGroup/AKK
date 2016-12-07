var viewModel;
var headerViewModel;
$(document).ready(function () {
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL, new CookieService());
    headerViewModel = new HeaderViewModel("Register", client, "/login");
    viewModel = new RegisterViewModel(client, new NavigationService(), new CookieService());

    var configurations = [
        {
            scriptSource: "js/templates/header-template.handlebars", 
            elementId: "header", 
            event: "headerUpdated",
            viewmodel: headerViewModel
        },
        {
            scriptSource: "js/templates/register-template.handlebars", 
            elementId: "register-content", 
            event: "registerChanged",
            viewmodel: viewModel
        }
    ];

    setUpContentUpdater(configurations, function() {
        viewModel.init();
        headerViewModel.init();
    });
});