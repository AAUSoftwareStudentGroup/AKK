var viewModel;
var headerViewModel;
$(document).ready(function () {
    $.get("js/templates/header-template.handlebars",
        function(response) {
            var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService());
            var headerViewModel = new HeaderViewModel("New Route", client, new CookieService());
            var headerTemplate = Handlebars.compile(response);

            headerViewModel.addEventListener("headerUpdated", function () {
                $('#header').html(headerTemplate(headerViewModel));
            });
            
            var contentTemplate = Handlebars.compile($("#new-route-content-template").html());
            var holdsTemplate = Handlebars.compile($("#new-route-holds-template").html());
            viewModel = new NewRouteViewModel(client, new NavigationService());

            viewModel.addEventListener("DataLoaded", function() {
                $('#content').html(contentTemplate(viewModel));
                $('.hold-picker').html(holdsTemplate(viewModel));
            });

            viewModel.addEventListener("HoldColorUpdated", function() {
                $('.hold-picker').html(holdsTemplate(viewModel));
                if (viewModel.hasTape === false && viewModel.selectedColor)
                    $('#holdColor-input-' + viewModel.selectedColor.value).prop("checked", true);
                else if (viewModel.selectedTapeColor)
                    $('#holdColor-input-' + viewModel.selectedTapeColor.value).prop("checked", true);
            });

            headerViewModel.init();
            viewModel.init();          
        });
});