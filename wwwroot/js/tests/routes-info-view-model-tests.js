var viewModel;

QUnit.test("RouteInfoViewModel init", function (assert) {
    viewModel = new RouteInfoViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService(), new TestDialogService());

    var cardsUpdatedTriggered = false;
    var betasUpdatedTriggered = false;

    viewModel.addEventListener("cardChanged", function () {
        cardsUpdatedTriggered = true;
    });

    viewModel.addEventListener("commentsChanged", function () {
        betasUpdatedTriggered = true;
    });
    viewModel.init();

    assert.equal(cardsUpdatedTriggered, true, "RouteInfoViewModel cardUpdated Triggered");
    assert.equal(betasUpdatedTriggered, true, "RouteInfoViewModel betaUpdated Triggered");

});


QUnit.test("RouteInfoViewModel set rating", function (assert) {
    viewModel = new RouteInfoViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService(), new TestDialogService());

    viewModel.init();
    var myRating = 4;
    viewModel.changeRating(myRating);
    assert.equal(viewModel.filledStars, myRating, "RouteInfoViewModel set rating");
});


QUnit.test("RouteInfoViewModel delete route", function (assert) {
    viewModel = new RouteInfoViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService(), new TestDialogService());
    viewModel.init();
    if (viewModel.route != null) {
        viewModel.deleteRoute();
        assert.equal(viewModel.route, null, "RouteInfoViewModel route deleted");
    }
});

QUnit.test("RouteInfoViewModel download image", function (assert) {
    viewModel = new RouteInfoViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService(), new TestDialogService());
    viewModel.init();
    if (viewModel.route != null) {
        viewModel.downloadImage();
        assert.equal(viewModel.hasImage, true, "RouteInfoViewModel Image downloaded");
    }
});