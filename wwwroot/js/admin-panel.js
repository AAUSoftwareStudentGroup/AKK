var viewModel;
var headerViewModel;
$(document).ready(function () {
    var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL, new CookieService());
    headerViewModel = new HeaderViewModel("Admin Panel", client, "/");
    viewModel = new AdminPanelViewModel(client, new DialogService());

    //Sets up the admin panel with each section, and the corresponding events, which will change each section when called
    var configurations = [
        {
            scriptSource: "js/templates/header-template.handlebars", 
            elementId: "header", 
            event: "headerUpdated",
            viewmodel: headerViewModel
        },
        {
            scriptSource: "js/templates/section-admin-template.handlebars", 
            elementId: "content-section", 
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
            scriptSource: "js/templates/hold-admin-template.handlebars", 
            elementId: "content-hold",
            event: "holdsChanged",
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

    //Initialise the viewmodels
    setUpContentUpdater(configurations, function() {
        viewModel.init();
        headerViewModel.init();
    });

    //The events that'll be triggered when the corresponding event gets called
    //Add an additional css class to the section panel when triggered, then animate the transition when it expands
    viewModel.addEventListener("sectionsChanged", function(msg) {
        window.setTimeout(function() {
            if($("#section-admin .expansion-panel").hasClass("expanded")) {
                $("#section-admin .expansion-panel").animate({
                    height: $('#section-admin .expansion-panel .expansion-panel-content').height(),
                }, 500);
            }
        },10);
    });

    //Changes the color of the grade when triggered
    viewModel.addEventListener("gradeColorChanged", function(msg) {
        $('.color-select-preview').css('background-color', 'rgb('+viewModel.selectedGrade.color.r+','+viewModel.selectedGrade.color.g+','+viewModel.selectedGrade.color.b+')');
    });

    //Changes the color of the hold when triggered
    viewModel.addEventListener("holdColorChanged", function(msg) {
        $('#grip-midtone').css('fill', 'rgb('+viewModel.selectedHold.colorOfHolds.r+','+viewModel.selectedHold.colorOfHolds.g+','+viewModel.selectedHold.colorOfHolds.b+')');
    });

    viewModel.addEventListener("gradesChanged", function(msg) {
        window.setTimeout(function() {
            if($("#grade-admin .expansion-panel").hasClass("expanded")) {
                $("#grade-admin .expansion-panel").animate({
                    height: $('#grade-admin .expansion-panel .expansion-panel-content').height(),
                }, 500);
            }
        },10);
    });
    viewModel.addEventListener("holdsChanged", function(msg) {
        window.setTimeout(function() {
            if($("#hold-admin .expansion-panel").hasClass("expanded")) {
                $("#hold-admin .expansion-panel").animate({
                    height: $('#hold-admin .expansion-panel .expansion-panel-content').height(),
                }, 500);
            }
        },10);
    });

    $(document).on('click','.expansion-panel-header', function (event) {
        var element = $(this).closest('.expansion-panel');
        viewModel.changeSection();
        viewModel.selectGrade();
        viewModel.selectHold();

        
        $(element).animate({
            height: element.find('.expansion-panel-content').height(),
        }, 500);
        $(".expansion-panel.expanded").animate({
            height: 0,
        }, 500);

        if(element.hasClass('expanded'))
            element.removeClass('expanded');
        else {
            $('.expansion-panel.expanded').removeClass('expanded');
            $('[type="radio"]').prop('checked', false);

            element.addClass('expanded');
        }
    });
});