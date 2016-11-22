var viewModel;
$(document).ready(function () {
    var template = Handlebars.compile($("#routes-template").html());
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
    viewModel = new RoutesViewModel(client);
    viewModel.AddEventListener("RoutesChanged", function() {
        $('#content').html(template(viewModel));
        $('#grade-' + viewModel.selectedGrade.difficulty).prop("selected", true);
        //       $('#hold-' + viewModel.selectedColor.value).prop("selected", true);
        $('#section-' + viewModel.selectedSection.name).prop("selected", true);
        $('#sortby-' + viewModel.selectedSortBy.value).prop("selected", true);
    });
    viewModel.init();
});
Handlebars.registerHelper('ifCond', function (v1, v2, options) {
    if (v1.g <= v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});