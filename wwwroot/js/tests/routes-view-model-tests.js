QUnit.test("Route View Model Tests", function( assert ) {
    var viewModel = new RoutesViewModel(new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new CookieService()));
    viewModel.init();
});