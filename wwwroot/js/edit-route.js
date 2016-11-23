var viewModel;
var rc;
$(document)
    .ready(function () {
        Handlebars.registerHelper('if_eq',
            function (a, b, opts) {
                if (a == b)
                    return opts.fn(this);
                else
                    return opts.inverse(this);
            });
        Handlebars.registerHelper('ifCond',
            function (v1, v2, options) {
                if (v1.g <= v2) {
                    return options.fn(this);
                }
                return options.inverse(this);
            });
        var template = Handlebars.compile($("#edit-route-template").html());
        var colortemplate = Handlebars.compile($("#holdcolortemplate").html());
        var imagetemplate = Handlebars.compile($("#imagetemplate").html());
        var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
        viewModel = new EditRouteViewModel(client);
        
        viewModel.addEventListener("OnGradeOrSectionChanged", function() {
            $('#content').html(template(viewModel));
            $('#section-input-' + viewModel.selectedSection.name).prop("checked", true);
            $('#grade-input-' + viewModel.selectedGrade.difficulty).prop("checked", true);
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

            else
                $('#holdColor-input-' + viewModel.selectedTapeColor.value).prop("checked", true);
        });

        viewModel.init();
    });

function UpdateCanvas(input) {
    readURL(input, function(i) {
        viewModel.setImage(i);
        viewModel.HoldPositions = [];
    });
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