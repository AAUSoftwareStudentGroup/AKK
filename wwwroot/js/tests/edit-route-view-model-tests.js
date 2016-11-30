var viewModel;

var viewModel;
QUnit.test("Editroute ViewModel init test", function (assert) {
    viewModel = new EditRouteViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService());

    var sectionsUpdatedTriggered = false;
    var gradesUpdatedTriggered = false;
    var numberUpdatedTriggered = false;
    var authorUpdatedTriggered = false;
    var holdsUpdatedTriggered = false;
    var noteUpdatedTriggered = false;


    viewModel.addEventListener("sectionsUpdated", function () {
        sectionsUpdatedTriggered = true;
    });
    viewModel.addEventListener("gradesUpdated", function () {
        gradesUpdatedTriggered = true;
    });
    viewModel.addEventListener("numberUpdated", function () {
        numberUpdatedTriggered = true;
    });
    viewModel.addEventListener("authorUpdated", function () {
        authorUpdatedTriggered = true;
    });
    viewModel.addEventListener("holdsUpdated", function () {
        holdsUpdatedTriggered = true;
    });
    viewModel.addEventListener("noteUpdated", function () {
        noteUpdatedTriggered = true;
    });

    viewModel.init();

    assert.equal(sectionsUpdatedTriggered, true, "editRoute ViewModel sectionUpdated Triggered");
    assert.equal(gradesUpdatedTriggered, true, "editRoute ViewModel gradeUpdated Triggered");
    assert.equal(numberUpdatedTriggered, true, "editRoute ViewModel numberUpdated Triggered");
    assert.equal(authorUpdatedTriggered, true, "editRoute ViewModel authorUpdated Triggered");
    assert.equal(holdsUpdatedTriggered, true, "editRoute ViewModel holdsUpdated Triggered");
    assert.equal(noteUpdatedTriggered, true, "editRoute ViewModel noteUpdated Triggered");

});
