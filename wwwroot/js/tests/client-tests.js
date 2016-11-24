QUnit.test( "getRoutes", function( assert ) {
  var done = assert.async();
  var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL);
  client.routes.getRoutes(null, null, null, function (response) {
    assert.equal(response.success, true);
    done();
  });
});