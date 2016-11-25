QUnit.test("RouteClient.getRoutes", function( assert ) {
  var done = assert.async();
  var routeClient = new RouteClient(API_ROUTE_URL);
  routeClient.getRoutes(null, null, null, function (routesResponse) {
    assert.equal(routesResponse.success, true);
    done();
  });
});

QUnit.test("RouteClient.getRoute", function( assert ) {
  var done = assert.async();
  var routeClient = new RouteClient(API_ROUTE_URL);
  routeClient.getRoutes(null, null, null, function (routesResponse) {
    assert.equal(routesResponse.success, true);
    routeClient.getRoute(routesResponse.data[0].id, function (routeResponse) {
      assert.equal(routeResponse.success, true);
      done();
    });
  });
});

QUnit.test("RouteClient.getImage", function( assert ) {
  var done = assert.async();
  var routeClient = new RouteClient(API_ROUTE_URL);
  routeClient.getRoutes(null, null, null, function (routesResponse) {
    assert.equal(routesResponse.success, true);
    routeClient.getImage(routesResponse.data[0].id, function (routeResponse) {
      assert.equal(routeResponse.success, true);
      done();
    });
  });
});

QUnit.test("SectionClient.getAllSections GradeClient.getAllGrades RouteClient.addRoute", function( assert ) {
  var done = assert.async();
  var routeClient = new RouteClient(API_ROUTE_URL);
  var sectionClient = new RouteClient(API_SECTION_URL);
  var gradeClient = new RouteClient(API_GRADE_URL);
  sectionClient.getAllSections(function(sectionsResponse) {
    assert.equal(sectionsResponse.success, true);
    var sectionId = sectionsResponse.data[0].id;
    gradeClient.getAllGrades(function(gradesResponse) {
      assert.equal(gradesResponse.success, true);
      var gradeId = gradesResponse.data[0].id;
      var name = 145698526685236652;
      var author = "Test";
      var holdColor = { r: 200, g: 200, b: 200, a: 1 };
      var tape = { r: 200, g: 200, b: 200, a: 1 };
      routeClient.addRoute(sectionId, name, author, holdColor, gradeId, tape, success, function (routeResponse) {
        assert.equal(routeResponse.success, true);
        assert.equal(gradeId, routeResponse.data.gradeId);
        assert.equal(author, routeResponse.data.author);
        assert.equal(holdColor, routeResponse.data.colorOfHolds);
        assert.equal(tape, routeResponse.data.colorOfTape);
        assert.equal(name, routeResponse.data.name);
      });
    });
  });
  
});

