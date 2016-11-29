var viewModel;
var events = ["RoutesChanged", ];
QUnit.test("Route View Model Tests", function (assert) {
    viewModel = new RoutesViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()));

    viewModel.init();

    var selectedGradeTriggered = false;
    var filteringChangedTriggered = false;
    var searchMethodChangedTriggered = false;


    viewModel.addEventListener("filteringChanged", function () {
        filteringChangedTriggered = true;
    });

    viewModel.addEventListener("SearchMethodChanged", function () {
        searchMethodChangedTriggered = true;
    });

    viewModel.changeSection(TEST_SECTIONS[2].id);

    viewModel.toggleSearch();

    assert.equal(filteringChangedTriggered, true, "ToggleSearch filteringChanged triggered");

    viewModel.searchClicked();

    assert.equal(searchMethodChangedTriggered, true, "SearchClick SearchMethodChanged triggered");

    viewModel.addEventListener("selectedGradeChanged", function () {
        selectedGradeTriggered = true;
    });

    viewModel.changeGrade(TEST_GRADES[3].id);

    var correctGrade = true;

    for (var i = 0; i < viewModel.routes.length; i++) {
        correctGrade = correctGrade && viewModel.routes[i].gradeId == TEST_GRADES[3].id;
    }

    if (viewModel.routes.length > 0) {
        assert.equal(correctGrade, true, "changeGrade all routes in selected Grade");
    }
});

QUnit.test("Change Section in Route ViewModel", function (assert) {
    viewModel = new RoutesViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()));
    var routesChangedTriggered = false;

    viewModel.init();

    viewModel.addEventListener("routesChanged", function () {
        routesChangedTriggered = true;
    });

    viewModel.changeSection(TEST_SECTIONS[2].id);

    assert.equal(routesChangedTriggered, true, "changeSection routesChanged triggered");

    viewModel.changeSection(TEST_SECTIONS[2].id);
    var correctSection = true;

    for (var i = 0; i < viewModel.routes.length; i++) {
        correctSection = correctSection && viewModel.routes[i].sectionId == TEST_SECTIONS[2].id;
    }

    if (viewModel.routes.length > 0) {
        assert.equal(correctSection, true, "changeSection all routes in selected section");
    }
});



