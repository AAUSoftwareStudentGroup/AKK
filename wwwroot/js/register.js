var viewModel;

var rc;
var fullwidth = false;
$(document).ready(function () {
    var template = Handlebars.compile($("#register-template").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
    
    viewModel = new RegisterViewModel(client, new NavigationService());
    viewModel.addEventListener("ContentUpdated", function() { 
        $('#content').html(template(viewModel)); 
    });
    viewModel.init();
});
Handlebars.registerHelper('if_eq', function (a, b, opts) {
        if (a == b) // Or === depending on your needs
            return opts.fn(this);
        else
            return opts.inverse(this);
    });
Handlebars.registerHelper('ifCond', function (v1, v2, options) {
    if (v1.g <= v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});
