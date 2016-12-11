var viewModel;

QUnit.test("Routes Info viewModel init", function (assert) {
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

    assert.equal(cardsUpdatedTriggered, true, "Routes Info ViewModel cardUpdated Triggered");
    assert.equal(betasUpdatedTriggered, true, "Routes Info ViewModel betaUpdated Triggered");

});


QUnit.test("Routes Info viewModel set rating", function (assert) {
    viewModel = new RouteInfoViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService(), new TestDialogService());

    viewModel.init();
    var myRating = 4;
    viewModel.changeRating(myRating);
    assert.equal(viewModel.filledStars, myRating, "Routes Info ViewModel set rating");
});


QUnit.test("Routes Info viewModel delete route", function (assert) {
    viewModel = new RouteInfoViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService(), new TestDialogService());
    viewModel.init();
    if (viewModel.route != null) {
        viewModel.deleteRoute();
        assert.equal(viewModel.route, null, "Routes Info ViewModel route deleted");
    }
});

QUnit.test("Routes Info viewModel download image", function (assert) {
    viewModel = new RouteInfoViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService(), new TestDialogService());
    viewModel.init();
    if (viewModel.route != null) {
        viewModel.downloadImage();
        assert.equal(viewModel.hasImage, true, "Routes Info ViewModel Image downloaded");
    }
});