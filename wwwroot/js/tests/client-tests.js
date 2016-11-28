QUnit.test("Client tests", function( assert ) {
  var done = assert.async();
  var memberClient = new MemberClient(API_MEMBER_URL, new CookieService());
  var routeClient = new RouteClient(API_ROUTE_URL, new CookieService());
  var sectionClient = new SectionClient(API_SECTION_URL, new CookieService());
  var gradeClient = new GradeClient(API_GRADE_URL, new CookieService());
  //Register to ensure that we can add a route in the test
  memberClient.register("Test User", "testuser", "password", function(registerResponse) { 
    var onAuthenticated = function () {
      //Get sections
      sectionClient.getAllSections(function(sectionsResponse) { 
        assert.equal(sectionsResponse.success, true, "getAllSections success = " + true);
        var sectionId = sectionsResponse.data[0].id;
        //Get grades
        gradeClient.getAllGrades(function(gradesResponse) {
          assert.equal(gradesResponse.success, true, "getAllGrades success = " + true);
          var gradeId = gradesResponse.data[0].id;
          var name = "T";
          var author = "Morten";
          var holdColor = { r: 200, g: 200, b: 200, a: 1 };
          var tape = { r: 200, g: 200, b: 200, a: 1 };
          var image = { fileUrl: TEST_IMAGE, width: TEST_IMAGE_WIDTH, height: TEST_IMAGE_HEIGHT };
          var onRouteAdded = function(routeId) {
            //Update route
            routeClient.updateRoute(routeId, sectionId, author, name, holdColor, gradeId, tape, image, function (routeResponse) {
              console.log(routeResponse);
              assert.equal(routeResponse.success, true, "updateRoute success = " + true);
              assert.equal(routeResponse.data.id, routeId, "updateRoute id = " + routeId);
              assert.equal(routeResponse.data.gradeId, gradeId, "updateRoute gradeId = " + gradeId);
              assert.equal(routeResponse.data.colorOfHolds.r, holdColor.r, "updateRoute colorOfHolds.r = " + holdColor.r);
              assert.equal(routeResponse.data.colorOfTape.r, tape.r, "updateRoute colorOfTape.r = " + tape.r);
              assert.equal(routeResponse.data.name, name, "updateRoute name = " + name);
              
              routeClient.getRoutes(null, null, null, function (routesResponse) {
                assert.equal(routesResponse.success, true, "getRoutes success = " + true);
                assert.ok(routesResponse.data.length > 0, "getRoutes length > 0");
                //Added route is in the list of all routes
                assert.equal(routesResponse.data.filter(function(r) { return r.id == routeId; }).length, 1, "getRoutes contains added route");
                //Get added route
                routeClient.getRoute(routeId, function (getRouteResponse) {
                  assert.equal(getRouteResponse.success, true, "getRoute success = " + true);
                  assert.equal(getRouteResponse.data.id, routeId, "getRoute id = " + routeId);
                  assert.equal(getRouteResponse.data.gradeId, gradeId, "getRoute gradeId = " + gradeId);
                  assert.equal(getRouteResponse.data.colorOfHolds.a, holdColor.a, "getRoute colorOfHolds.a = " + holdColor.a);
                  assert.equal(getRouteResponse.data.colorOfTape.b, tape.b, "getRoute colorOfTape.b = " + tape.b);
                  assert.equal(getRouteResponse.data.name, name, "getRoute name = " + name);
                  routeClient.getImage(routeId, function (imageResponse) {
                    assert.equal(imageResponse.success, true, "getImage success = " + true);
                    assert.equal(imageResponse.data.fileUrl, TEST_IMAGE, "getImage fileUrl");
                    routeClient.deleteRoute(routeId, function (deleteResponse) {
                      assert.equal(imageResponse.success, true, "deleteRoute success = " + true);
                      //End of test
                      done();
                    });
                  });
                });
              });
            });
          }
          
          routeClient.getRoutes(null, null, null, function (allRoutesResponse) {
            assert.equal(allRoutesResponse.success, true, "getRoutes success");
            var testRoutes = allRoutesResponse.data.filter(function(route) { return route.name == "T" });
            if(testRoutes.length == 0)
            {
              routeClient.addRoute(sectionId, name, author, holdColor, gradeId, tape, function (routeAddedResponse) {
                onRouteAdded(routeAddedResponse.data.id);
              });
            }
            else
            {
              onRouteAdded(testRoutes[0].id);
            }
          });
        });
      });
    };
    if(registerResponse.success)
    {
      onAuthenticated();
    }
    else
    {
      memberClient.logIn("testuser", "password", function (loginResponse) {
        assert.equal(loginResponse.success, true, "login success");
        onAuthenticated();
      });
    }
  });
});