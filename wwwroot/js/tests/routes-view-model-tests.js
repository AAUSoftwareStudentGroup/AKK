var viewModel;
var events = ["RoutesChanged", ];
QUnit.test("Route View Model Tests", function( assert ) {
    viewModel = new RoutesViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()));

    viewModel.init();

    var routesChangedTriggered = false;
    var selectedSectionTriggered = false;
    var selectedGradeTriggered = false;

    viewModel.addEventListener("routesChanged", function () {
        routesChangedTriggered = true;
    });

    viewModel.addEventListener("selectedSectionChanged", function () {
        selectedSectionTriggered = true;
    });

    viewModel.changeSection(TEST_SECTIONS[2].id);

    assert.equal(routesChangedTriggered, true, "changeSection routesChanged triggered");

    assert.equal(selectedSectionTriggered, true, "changeSection selectedSectionChanged triggered");

    var correctSection = true;

    for (var i = 0; i < viewModel.routes.length; i++) {
        correctSection = correctSection && viewModel.routes[i].sectionId == TEST_SECTIONS[2].id
    }

    if(viewModel.routes.length > 0)
    {
        assert.equal(correctSection, true, "changeSection all routes in selected section");
    }

    viewModel.addEventListener("selectedGradeChanged", function () {
        selectedGradeTriggered = true;
    });

    viewModel.changeGrade(TEST_GRADES[3].id);

    assert.equal(selectedSectionTriggered, true, "changeSection selectedGradeChanged triggered");

    var correctGrade = true;

    for (var i = 0; i < viewModel.routes.length; i++) {
        correctGrade = correctGrade && viewModel.routes[i].gradeId == TEST_GRADES[3].id
    }

    if(viewModel.routes.length > 0)
    {
        assert.equal(correctGrade, true, "changeGrade all routes in selected Grade");
    }
});