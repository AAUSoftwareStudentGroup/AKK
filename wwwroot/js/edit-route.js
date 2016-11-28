var viewModel;
var headerViewModel;
var rc;
$(document).ready(function () {
    $.get("js/templates/header-template.handlebars",
        function(response) {
            var template = Handlebars.compile($("#edit-route-template").html());
            var templateheader = Handlebars.compile(response);
            var colortemplate = Handlebars.compile($("#holdcolortemplate").html());
            var imagetemplate = Handlebars.compile($("#imagetemplate").html());
            var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService());

            headerViewModel = new HeaderViewModel("Edit Route", client, new CookieService());
            headerViewModel.addEventListener("headerUpdated", function () {
                $('#header').html(templateheader(headerViewModel));
            });

            viewModel = new EditRouteViewModel(client, new NavigationService());
            viewModel.addEventListener("OnGradeOrSectionChanged", function () {
                $('#content').html(template(viewModel));
                $('#section-input-' + viewModel.selectedSection.name).prop("checked", true);
                $('#grade-input-' + viewModel.selectedGrade.id).prop("checked", true);
            });

            viewModel.addEventListener("OnImageChanged", function() {
                $('#imageContent').html(imagetemplate(viewModel));
                if (viewModel.hasImage) {
                    rc = new RouteCanvas($("#route-edit-image")[0], viewModel.image, viewModel, true);
                    rc.DrawCanvas();
                }
            });

            viewModel.addEventListener("OnColorChanged", function() {
                $('#holdColorContent').html(colortemplate(viewModel));
                if (viewModel.hasTape === false)
                    $('#holdColor-input-' + viewModel.selectedColor.value).prop("checked", true);
                else if (viewModel.selectedTapeColor)
                    $('#holdColor-input-' + viewModel.selectedTapeColor.value).prop("checked", true);
            });

            viewModel.init();          
            headerViewModel.init();
        });

});

function UpdateCanvas(input) {
    readURL(input, function(i) {
        resizeImage(i, function(ni) {
            console.log(ni.width + "," + ni.height);
            viewModel.setImage(ni);
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