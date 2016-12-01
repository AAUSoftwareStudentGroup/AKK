var viewModel;
var headerViewModel;
$(document).ready(function () {
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL, new CookieService());
    headerViewModel = new HeaderViewModel("Admin Panel", client, "/");
    viewModel = new AdminPanelViewModel(client, new DialogService());

    var content = [
        {
            scriptSource: "js/templates/header-template.handlebars", 
            elementId: "header", 
            event: "headerUpdated",
            viewmodel: headerViewModel
        },
        {
            scriptSource: "js/templates/section-admin-template.handlebars", 
            elementId: "section-admin", 
            event: "sectionsChanged",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/grade-admin-template.handlebars", 
            elementId: "content-grade",
            event: "gradesChanged",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/member-admin-template.handlebars", 
            elementId: "content-member",
            event: "membersChanged",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/route-list-template.handlebars", 
            elementId: "routes-content", 
            event: "routesChanged",
            viewmodel: viewModel
        }
    ];

    setUpContentUpdater(content, function() {
        viewModel.init();
        headerViewModel.init();
    });


    viewModel.addEventListener("gradeColorChanged", function(msg) {
        $('.color-select-preview').css('background-color', 'rgb('+viewModel.selectedGrade.color.r+','+viewModel.selectedGrade.color.g+','+viewModel.selectedGrade.color.b+')');
    });
    viewModel.addEventListener("gradesChanged", function(msg) {
        window.setTimeout(function() {
            if(viewModel.selectedGrade)
                $('.orderable-list').scrollTop(61*viewModel.selectedGrade.difficulty-61*2);
        },10);
    });

    $(document).on('click','.expansion-panel-header', function (event) {
        var element = $(this).closest('.expansion-panel');
        viewModel.changeSection();
        viewModel.selectGrade();
        
        if(element.hasClass('expanded'))
            element.removeClass('expanded');
        else {
            $('.expansion-panel.expanded').removeClass('expanded');
            $('[type="radio"]').prop('checked', false);

            element.addClass('expanded');
        }
    });
});