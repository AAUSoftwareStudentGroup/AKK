var viewModel;
var headerViewModel;
var rc;
$(document).ready(function () {
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL, new CookieService());
    headerViewModel = new HeaderViewModel("Route Info", client, "/");
    viewModel = new RouteInfoViewModel(client, new NavigationService(), new DialogService());

    var configurations = [
        {
            scriptSource: "js/templates/header-template.handlebars", 
            elementId: "header", 
            event: "headerUpdated",
            viewmodel: headerViewModel
        },
        {
            scriptSource: "js/templates/route-info-card-template.handlebars", 
            elementId: "route-content", 
            event: "cardChanged",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/comment-picker-template.handlebars", 
            elementId: "comment-content", 
            event: "commentsChanged",
            viewmodel: viewModel
        }
    ];

    setUpContentUpdater(configurations, function() {
        viewModel.addEventListener("cardChanged", function() {
            if (viewModel.hasImage) {
                rc = new RouteCanvas($("#routeimage")[0], viewModel.route.image, viewModel, false);
                rc.DrawCanvas();
            }
        });
        viewModel.addEventListener("commentsChanged", function() {
            autosize($('textarea'));
        });
        viewModel.addEventListener("Error", function(response) {
            $("#error-message").html(response).show();
        });
        viewModel.addEventListener("Info", function(response) {
            $("#info-message").html(response).show();
        });
        viewModel.init();
        headerViewModel.init();
    });

    $(document).on('click', '#routeimagecontainer', function(e) {
        e.stopPropagation();
        $("#routeimagecontainer").toggleClass("small");
        $(".image-overlay").toggleClass("hidden");

        if(rc != null) {
            rc.resize();
            rc.DrawCanvas();
        }
    });

    $(document).on("click", ".route-rating svg", function(e) {
        e.stopPropagation();
        viewModel.changeRating($(this).index() + 1);
    });
});

function addComment(form) {
    if (viewModel.addingComment) return;
    $(".editable").removeClass("editable");
    viewModel.addComment(form);
}