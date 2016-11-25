QUnit.test("SectionClient.getAllSections GradeClient.getAllGrades RouteClient.addRoute RouteClient.getRoutes RouteClient.getRoute", function( assert ) {
  var done = assert.async();
  var routeClient = new RouteClient(API_ROUTE_URL);
  var sectionClient = new SectionClient(API_SECTION_URL);
  var gradeClient = new GradeClient(API_GRADE_URL);
  sectionClient.getAllSections(function(sectionsResponse) {
    assert.equal(sectionsResponse.success, true);
    var sectionId = sectionsResponse.data[0].id;
    gradeClient.getAllGrades(function(gradesResponse) {
      assert.equal(gradesResponse.success, true);
      var gradeId = gradesResponse.data[0].id;
      var name = 145698526685236652;
      var holdColor = { r: 200, g: 200, b: 200, a: 1 };
      var tape = { r: 200, g: 200, b: 200, a: 1 };
      routeClient.addRoute(sectionId, name, holdColor, gradeId, tape, function (routeResponse) {
        assert.equal(routeResponse.success, true);
        assert.ok(routeResponse.id != null);
        assert.equal(gradeId, routeResponse.data.gradeId);
        assert.equal(holdColor, routeResponse.data.colorOfHolds);
        assert.equal(tape, routeResponse.data.colorOfTape);
        assert.equal(name, routeResponse.data.name);
        assert.test("RouteClient.getRoutes", function( subAssert ) {
          var routeClient = new RouteClient(API_ROUTE_URL);
          routeClient.getRoutes(null, null, null, function (routesResponse) {
            subAssert.equal(routesResponse.success, true);
            subAssert.ok(routesResponse.data.length > 0);
            subAssert.equal(routesResponse.data.filter(function(r) { return r.id == routeResponse.id; }).length, 1);
            done();
          });
        });
        assert.test("RouteClient.getRoute", function( subAssert ) {
          var routeClient = new RouteClient(API_ROUTE_URL);
          routeClient.getRoute(routeResponse.id, function (getRouteResponse) {
            subAssert.equal(getRouteResponse.success, true);
            subAssert.equal(getRouteResponse.success, true);
            subAssert.ok(getRouteResponse.id != null);
            subAssert.equal(gradeId, getRouteResponse.data.gradeId);
            subAssert.equal(holdColor, getRouteResponse.data.colorOfHolds);
            subAssert.equal(tape, getRouteResponse.data.colorOfTape);
            subAssert.equal(name, getRouteResponse.data.name);
            done();
          });
        });
      });
    });
  });
});