var viewModel;
var events = ["RoutesChanged", ];
QUnit.test("Route View Model Tests", function( assert ) {
    viewModel = new RoutesViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()));

    viewModel.init();

    var RoutesChangedTriggered = false;

    viewModel.addEventListener("RoutesChanged", function () {
        RoutesChangedTriggered = true;
    });

    viewModel.changeSection(TEST_SECTIONS[2].id);

    assert.equal(RoutesChangedTriggered, true);

    for (var i = 0; i < viewModel.routes.length; i++) {
        assert.equal(viewModel.routes[i].sectionId, TEST_SECTIONS[2].id);
    }


});