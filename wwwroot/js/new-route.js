var viewModel;
var headerViewModel;
$(document).ready(function () {
    $.get("js/templates/header-template.handlebars",
        function(response) {
            var template = Handlebars.compile($("#new-route-template").html());
            var colortemplate = Handlebars.compile($("#holdcolortemplate").html());
            var templateheader = Handlebars.compile(response);
            var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService());

            headerViewModel = new HeaderViewModel("New Route", client, new CookieService());
            headerViewModel.addEventListener("headerUpdated", function () {
                $('#header').html(templateheader(headerViewModel));
            });

            viewModel = new NewRouteViewModel(client, new NavigationService());
            viewModel.addEventListener("DataLoaded", function() {
                $('#content').html(template(viewModel));
            });

            viewModel.addEventListener("HoldColorUpdated", function() {
                $('#holdColorContent').html(colortemplate(viewModel));
                if (viewModel.hasTape === false && viewModel.selectedColor)
                    $('#holdColor-input-' + viewModel.selectedColor.value).prop("checked", true);
                else if(viewModel.selectedTapeColor)
                    $('#holdColor-input-' + viewModel.selectedTapeColor.value).prop("checked", true);
            });

            viewModel.init();          
            headerViewModel.init();
        });
});