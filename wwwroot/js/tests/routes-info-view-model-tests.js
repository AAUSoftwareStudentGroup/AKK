var viewModel;

var viewModel;
QUnit.test("Routes Info viewModel init", function (assert) {
    viewModel = new RouteInfoViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService(), TestDialogService());

    var cardsUpdatedTriggered = false;
    var betasUpdatedTriggered = false;
   
    viewModel.addEventListener("cardChanged", function() {
        cardsUpdatedTriggered = true;
    });

    viewModel.addEventListener("commentsChanged", function () {
        betasUpdatedTriggered = true;
    });
    viewModel.init();

    assert.equal(cardsUpdatedTriggered, true, "Routes Info ViewModel cardUpdated Triggered")
    assert.equal(betasUpdatedTriggered, true, "Routes Info ViewModel betaUpdated Triggered")

});
