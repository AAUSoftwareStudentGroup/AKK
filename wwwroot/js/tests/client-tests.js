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
          var name = "33";
          var author = "Morten";
          var holdColor = { r: 200, g: 200, b: 200, a: 99 };
          var tape = { r: 200, g: 200, b: 200, a: 55 };
          var image = { fileUrl: TEST_IMAGE, width: TEST_IMAGE_WIDTH, height: TEST_IMAGE_HEIGHT };
          var note = "Test note";
          var onRouteAdded = function(route) {
            //Update route
            var routeId = route.id;
            name = "2";
            author = "Morten ";
            holdColor = { r: 201, g: 150, b: 99, a: 20 };
            tape = { r: 77, g: 66, b: 55, a: 30 };
            image = { fileUrl: TEST_IMAGE, width: TEST_IMAGE_WIDTH / 2, height: TEST_IMAGE_HEIGHT / 2 };
            note = "Test note ";
            routeClient.updateRoute(route.id, sectionId, author, name, holdColor, gradeId, tape, note, image, function (routeResponse) {
              assert.equal(routeResponse.success, true, "updateRoute success = " + true + " message = " + routeResponse.message);
              assert.equal(routeResponse.data.id, routeId, "updateRoute id = " + routeId);
              assert.equal(routeResponse.data.gradeId, gradeId, "updateRoute gradeId = " + gradeId);
              assert.equal(routeResponse.data.colorOfHolds.r, holdColor.r, "updateRoute colorOfHolds.r = " + holdColor.r);
              assert.equal(routeResponse.data.colorOfTape.a, tape.a, "updateRoute colorOfTape.a = " + tape.a);
              assert.equal(routeResponse.data.name, name, "updateRoute name = " + name);
              assert.equal(routeResponse.data.note, note, "updateRoute note = " + note);
              //assert.equal(routeResponse.data.image.width, image.width, "updateRoute image width = " + image.width);
              
              routeClient.getRoutes(null, null, null, function (routesResponse) {
                assert.equal(routesResponse.success, true, "getRoutes success = " + true);
                assert.ok(routesResponse.data.length > 0, "getRoutes length > 0");
                //Added route is in the list of all routes
                assert.equal(routesResponse.data.filter(function(r) { return r.id == routeId; }).length, 1, "getRoutes contains added route");
                //Get added route
                routeClient.getRoute(routeId, function (getRouteResponse) {
                  console.log(getRouteResponse);
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
            assert.equal(allRoutesResponse.success, true, "" +
                "getRoutes success =" + true);
                console.log(note);
            var testRoutes = allRoutesResponse.data.filter(function(route) { return route.name == "T" });
            if(testRoutes.length == 0)
            {
              routeClient.addRoute(sectionId, name, author, holdColor, gradeId, tape, note, image, function (routeAddedResponse) {
                console.log(routeAddedResponse);
                onRouteAdded(routeAddedResponse.data);
              });
            }
            else {
                console.log("hej");
              onRouteAdded(testRoutes[0]);
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