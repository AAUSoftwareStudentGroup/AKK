var viewModel;
var headerViewModel;
$(document).ready(function () {
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL, new CookieService());
    headerViewModel = new HeaderViewModel("Log In", client, "/");
    viewModel = new LogInViewModel(client, new NavigationService(), new CookieService());

    var configurations = [
        {
            scriptSource: "js/templates/header-template.handlebars", 
            elementId: "header", 
            event: "headerUpdated",
            viewmodel: headerViewModel
        },
        {
            scriptSource: "js/templates/login-template.handlebars", 
            elementId: "login-content", 
            event: "loginChanged",
            viewmodel: viewModel
        }
    ];

    setUpContentUpdater(configurations, function() {
        viewModel.addEventListener("error", function(response) {
            $("#error-message").html(response).show();
        });
        viewModel.addEventListener("info", function(response) {
            $("#info-message").html(response).show();
        });
        viewModel.init();
        headerViewModel.init();
    });
});