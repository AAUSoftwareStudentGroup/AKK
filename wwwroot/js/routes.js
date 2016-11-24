var viewModel;
var navbarbutton;
var navigation;
$(document).ready(function () {
    $.get("js/templates/header-template.handlebars",
        function(response) {
            var template = Handlebars.compile($("#routes-template").html());
            var templateheader = Handlebars.compile(response);
            var templatefiltersection = Handlebars.compile($("#filter-section-template").html());
            var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
            viewModel = new RoutesViewModel(client);
            viewModel.addEventListener("RoutesChanged", function () {
                $("#header").html(templateheader({ viewModel: viewModel, title: "Find Route"}));
                $('#content').html(template(viewModel));
                $('#grade-' + viewModel.selectedGrade.difficulty).prop("selected", true);
                $('#section-' + viewModel.selectedSection.name).prop("selected", true);
                $('#sortby-' + viewModel.selectedSortBy.value).prop("selected", true);
            });
            viewModel.addEventListener("SearchMethodChanged", function() {
                $('#filtersectiontemplate').html(templatefiltersection(viewModel));
                if (viewModel.isSearching) {
                    $("#search-field").focus();
                }
            });
            viewModel.init();
            
        });
        $(document).on("input", "#search-field", function() {
            var searchstring = $("#search-field").val();
            viewModel.search(searchstring);
        });
  
});