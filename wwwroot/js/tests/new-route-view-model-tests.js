var viewModel;

var viewModel;
QUnit.test("NewRoute ViewModel init test", function (assert) {
    viewModel = new NewRouteViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService());

    var numberUpdatedTriggered = false;
    var holdsUpdatedTriggered = false;
    var imageUpdatedTriggered = false;
    var noteUpdatedTriggered = false;


    viewModel.addEventListener("numberUpdated", function () {
        numberUpdatedTriggered = true;
    });
    viewModel.addEventListener("holdsUpdated", function () {
        holdsUpdatedTriggered = true;
    });
    viewModel.addEventListener("imageUpdated", function () {
        imageUpdatedTriggered = true;
    });
    viewModel.addEventListener("noteUpdated", function () {
        noteUpdatedTriggered = true;
    });

    viewModel.init();

    assert.equal(numberUpdatedTriggered, true, "NewRoute ViewModel numberUpdated Triggered");
    assert.equal(holdsUpdatedTriggered, true, "NewRoute ViewModel holdsUpdated Triggered");
    assert.equal(imageUpdatedTriggered, true, "NewRoute ViewModel imageUpdated Triggered");
    assert.equal(noteUpdatedTriggered, true, "NewRoute ViewModel noteUpdated Triggered");


});
