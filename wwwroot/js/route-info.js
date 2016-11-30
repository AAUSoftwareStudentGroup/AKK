var viewModel;
var headerViewModel;
var rc;
$(document).ready(function () {
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService());
    headerViewModel = new HeaderViewModel("Route Info", client, "/");
    viewModel = new RouteInfoViewModel(client, new NavigationService(), new DialogService());

    var content = [
        {
            scriptSource: "js/templates/header-template.handlebars", 
            elementId: "header", 
            event: "headerUpdated",
            viewmodel: headerViewModel
        },
        {
            scriptSource: "js/templates/route-info-card-template.handlebars", 
            elementId: "cardtemplate", 
            event: "cardUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/comment-picker-template.handlebars", 
            elementId: "commenttemplate", 
            event: "commentsUpdated",
            viewmodel: viewModel
        }
    ];

    $(document).on("click", "#routeimagecontainer", function(e) {
        e.stopPropagation();
        $("#routeimagecontainer").toggleClass("small");
        $("#image-overlay").toggleClass("hidden");

        if(rc != null) {
            rc.resize();
            rc.DrawCanvas();
        }
    });

    setUpContentUpdater(content, function() {
        viewModel.addEventListener("cardUpdated", function() {
            if (viewModel.hasImage) {
                rc = new RouteCanvas($("#routeimage")[0], viewModel.route.image, viewModel, false);
                rc.DrawCanvas();
            }
        });
        viewModel.addEventListener("imageUpdated", function() {
            $("#comment-form").addClass("video-added");
        });
        viewModel.addEventListener("commentsUpdated", function() {
            autosize($('textarea'));
        });
        viewModel.init();
        headerViewModel.init();
    });
});