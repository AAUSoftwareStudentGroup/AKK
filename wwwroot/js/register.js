var viewModel;

var rc;
var fullwidth = false;
$(document).ready(function () {
    var template = Handlebars.compile($("#register-template").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL);
    
    viewModel = new RegisterViewModel(client, new NavigationService());
    $('#content').html(template(viewModel)); 

    viewModel.init();
});
