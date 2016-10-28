$(document).ready(function () {
    var client = new Client(API_ROUTE_URL, API_SECTION_URL);
    var template = Handlebars.compile($("#routes-template").html());

    var context = {
        grades: [
            { value: 1, name: "red" },
            { value: 2, name: "blue" }
        ],
        sectionNames: [],
        sortOptions: []
    }

    var html = template(context);
    $('body').append(html);
});