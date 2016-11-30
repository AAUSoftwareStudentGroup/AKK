var viewModel;
QUnit.test("Route ViewModel SearchMethod triggered", function (assert) {
    viewModel = new RoutesViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()));

    viewModel.init();

    var searchMethodChangedTriggered = false;


    viewModel.addEventListener("SearchMethodChanged", function () {
        searchMethodChangedTriggered = true;
    });
    viewModel.searchClicked();

    assert.equal(searchMethodChangedTriggered, true, "SearchClick SearchMethodChanged triggered");

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


QUnit.test("Change Grade in RouteViewmodel", function (assert) {
    viewModel = new RoutesViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()));
    viewModel.init();

    var selectedGradeTriggered = false;
    viewModel.addEventListener("selectedGradeChanged", function () {
        selectedGradeTriggered = true;
    });

    viewModel.changeGrade(TEST_GRADES[3].id);

    var correctGrade = true;

    for (var i = 0; i < viewModel.routes.length; i++) {
        correctGrade = correctGrade && viewModel.routes[i].gradeId == TEST_GRADES[3].id;
    }

    if (viewModel.routes.length > 0) {
        assert.equal(correctGrade, true, "Changegrade all routes in selected Grade");
    }

});


QUnit.test("RouteViewmodel ToggleSearch Triggered", function (assert) {
    viewModel = new RoutesViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()));
    viewModel.init();

    var filteringChangedTriggered = false;

    viewModel.addEventListener("filteringChanged", function () {
        filteringChangedTriggered = true;
    });

    viewModel.toggleSearch();

    assert.equal(filteringChangedTriggered, true, "ToggleSearch filteringChanged triggered");

});

QUnit.test("RouteViewModel SearchOptions changed", function (assert) {
    viewModel = new RoutesViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()));
    viewModel.init();

    var sortByValue = 1;
    viewModel.changeSortBy(sortByValue);

    assert.equal(viewModel.selectedSortBy.value, sortByValue);

});


