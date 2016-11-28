var viewModel;
var headerViewModel;
var rc;
var client;

$(document).ready(function () {
    client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService());
    headerViewModel = new HeaderViewModel("Edit Route", client, new CookieService());
    viewModel = new EditRouteViewModel(client, new NavigationService());

    var content = [
        {
            scriptSource: "js/templates/header-template.handlebars", 
            elementId: "header", 
            event: "headerUpdated",
            viewmodel: headerViewModel
        },
        {
            scriptSource: "js/templates/section-picker-template.handlebars", 
            elementId: "section-picker-content", 
            event: "sectionsUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/grade-picker-template.handlebars",
            elementId: "grade-picker-content",
            event: "gradesUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/number-picker-template.handlebars",
            elementId: "number-picker-content",
            event: "numberUpdated",
            viewmodel: viewModel
            
        },
        {
            scriptSource: "js/templates/author-picker-template.handlebars",
            elementId: "author-picker-content",
            event: "authorUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/image-picker-template.handlebars",
            elementId: "image-picker-content",
            event: "imageUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/hold-picker-template.handlebars",
            elementId: "hold-picker-content",
            event: "holdsUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/image-picker-template.handlebars",
            elementId: "image-picker-content",
            event: "imageUpdated",
            viewmodel: viewModel
        },
        {
            scriptSource: "js/templates/note-picker-template.handlebars",
            elementId: "note-picker-content",
            event: "noteUpdated",
            viewmodel: viewModel
        }
    ];

    setUpContentUpdater(content, function() {
        viewModel.init();
        headerViewModel.init();
    });
});

/*$(document).ready(function () {
    $.get("js/templates/header-template.handlebars",
        function(response) {
            var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService());
            var headerViewModel = new HeaderViewModel("Edit Route", client, new CookieService());
            var headerTemplate = Handlebars.compile(response);

            headerViewModel.addEventListener("headerUpdated", function () {
                $('#header').html(headerTemplate(headerViewModel));
            });

            var contentTemplate = Handlebars.compile($("#edit-route-template").html());
            var holdsTemplate = Handlebars.compile($("#edit-route-holds-template").html());

            viewModel = new EditRouteViewModel(client, new NavigationService());
            viewModel.addEventListener("OnGradeOrSectionChanged", function () {
                $('#content').html(contentTemplate(viewModel));
                $('#section-input-' + viewModel.selectedSection.name).prop("checked", true);
                $('#grade-input-' + viewModel.selectedGrade.id).prop("checked", true);
            });

            viewModel.addEventListener("OnColorChanged", function() {
                $('.hold-picker').html(holdsTemplate(viewModel));
                if (viewModel.hasTape === false)
                    $('#holdColor-input-' + viewModel.selectedColor.value).prop("checked", true);
                else if (viewModel.selectedTapeColor)
                    $('#holdColor-input-' + viewModel.selectedTapeColor.value).prop("checked", true);
            });

            viewModel.addEventListener("OnImageChanged", function() {
                $('#content').html(contentTemplate(viewModel));
                if (viewModel.hasImage) {
                    rc = new RouteCanvas($("#route-edit-image")[0], viewModel.image, viewModel, true);
                    rc.DrawCanvas();
                }
            });

            viewModel.init();          
            headerViewModel.init();
        });

});*/

function UpdateCanvas(input) {
    readURL(input, function(i) {
        resizeImage(i, function(ni) {
            viewModel.changeImage(ni);
            viewModel.HoldPositions = [];
        });
    });
}

function resizeImage(image, callback) {
    var maxWidth = 500;
    if (image.width < maxWidth) callback(image);
    var ratio = image.width / image.height;

    var canvas =  document.createElement('canvas');
    canvas.width = maxWidth;
    canvas.height = maxWidth / ratio;
    var context = canvas.getContext("2d");
    context.drawImage(image, 0, 0, canvas.width, canvas.height);
    var newImageData = canvas.toDataURL();

    var newImage = new Image();
    newImage.src = newImageData;
    newImage.onload = function() {
        callback(newImage);
    }
}

function readURL(input, callback) {
  if (input.files && input.files[0]) {
    var reader = new FileReader();
    var i = document.createElement('img');
    reader.onload = function (e) {
        i.setAttribute("src", e.target.result);
        i.onload = function(e) {
            callback(i);
        }
    }
    reader.readAsDataURL(input.files[0]);
  }
}