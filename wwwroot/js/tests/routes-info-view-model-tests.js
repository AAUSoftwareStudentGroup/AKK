var viewModel;

var viewModel;
QUnit.test("Routes Info viewModel init", function (assert) {
    viewModel = new RouteInfoViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService(), TestDialogService());

    var carsUpdatedTriggered = false;
    var betasUpdatedTriggered = false;
   
    viewModel.addEventListener("cardUpdated", function() {
        carsUpdatedTriggered = true;
    });

    viewModel.addEventListener("betasUpdated", function () {
        betasUpdatedTriggered = true;
    });
    viewModel.init();

    assert.equal(carsUpdatedTriggered, true, "Routes Info ViewModel cardUpdated Triggered")
    assert.equal(betasUpdatedTriggered, true, "Routes Info ViewModel betaUpdated Triggered")

});
