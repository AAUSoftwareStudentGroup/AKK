$(document).ready(function () {
    var client = new Client(API_ROUTE_URL, API_SECTION_URL);
    var template = Handlebars.compile($("#routes-template").html());

    var context = {
        grades: [
            { value: 1, name: "red" },
            { value: 2, name: "blue" }
        ],
        sectionNames: [],
        sortOptions: [],
        routes: [
            {
              "id": "f4f25dc9-a031-473a-8723-282403017fa8",
              "name": "1",
              "author": "Mathias Hornumm",
              "date": "2016-10-28T14:42:17.8477058",
              "colorOfHolds": 255,
              "grade": 0,
              "sectionID": "A"
            },
            {
              "id": "b8b87a72-343e-4512-93da-1e59575d763d",
              "name": "2",
              "author": "Mathias Hornumm",
              "date": "2016-10-28T14:42:17.8477058",
              "colorOfHolds": 4278190335,
              "grade": 1,
              "sectionID": "A"
            },
            {
              "id": "0acca3c4-d426-42e2-a343-8529e7db387f",
              "name": "3",
              "author": "Mathias Jakobsen",
              "date": "2016-10-28T14:42:17.8477058",
              "colorOfHolds": 16711935,
              "grade": 4,
              "sectionID": "A"
            }
        ]
    }

    var html = template(context);
    $('body').append(html);
});