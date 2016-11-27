var viewModel;

var rc;
var fullwidth = false;
$(document).ready(function () {
    var template = Handlebars.compile($("#login-template").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService());
    
    viewModel = new LogInViewModel(client, new NavigationService(), new CookieService());
    $('#content').html(template(viewModel)); 

    viewModel.init();
});