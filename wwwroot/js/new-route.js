var viewModel;
$(document).ready(function () {
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
    var template = Handlebars.compile($("#new-route-template").html());
    var colortemplate = Handlebars.compile($("#holdcolortemplate").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
    viewModel = new NewRouteViewModel(client);
    
    viewModel.addEventListener("DataLoaded", function() {
        $('#content').html(template(viewModel));
    });
    viewModel.addEventListener("HoldColorUpdated", function() {
        $('#holdColorContent').html(colortemplate(viewModel));
        if (viewModel.hasTape === false)
            $('#holdColor-input-' + viewModel.selectedColor.value).prop("checked", true);
        else
            $('#holdColor-input-' + viewModel.selectedTapeColor.value).prop("checked", true);
    });
    viewModel.init();
});