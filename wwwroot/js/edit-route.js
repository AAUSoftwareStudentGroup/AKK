var viewModel;
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
        var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
        viewModel = new EditRouteViewModel(client);
        
        viewModel.addEventListener("OnGradeOrSectionChanged", function changed() {
            $('#content').html(template(viewModel));

            $('#section-input-' + viewModel.selectedSection.name).prop("checked", true);
            $('#grade-input-' + viewModel.selectedGrade.difficulty).prop("checked", true);
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

function readURL(input) {
  if (input.files && input.files[0]) {
    var reader = new FileReader();
    var i = new Image();
    reader.onload = function (e) {
      i.src = e.target.result;
    }
    i.onload = function(e) {
        alert("Image loaded");
        return i;
    }
    reader.readAsDataURL(input.files[0]);
  }
}