QUnit.test("Client tests", function( assert ) {
  var client = new Client(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL, new CookieService());
  var done = assert.async();
  register(assert, done, client);
});

function register(assert, done, client)
{
  client.members.register("Test User", "testuser", "password", function(registerResponse) {
    if(registerResponse.success)
    {
      getAllSections(assert, done, client);
    }
    else
    {
      client.members.logIn("testuser", "password", function (loginResponse) {
        assert.equal(loginResponse.success, true, "login success");
        getAllSections(assert, done, client);
      });
    }
  });
}

function getAllSections(assert, done, client)
{
  client.sections.getAllSections(function(sectionsResponse) { 
    assert.equal(sectionsResponse.success, true, "getAllSections success = " + true);
    var sectionId = sectionsResponse.data[0].id;
    getAllGrades(sectionId, assert, done, client);
  });
}

function getAllGrades(sectionId, assert, done, client)
{
  client.grades.getAllGrades(function(gradesResponse) {
    assert.equal(gradesResponse.success, true, "getAllGrades success = " + true);
    var gradeId = gradesResponse.data[0].id;
    addRoute(sectionId, gradeId, assert, done, client);
  });
}

function addRoute(sectionId, gradeId, assert, done, client)
{
  var name = 1337;
  var author = "Morten";
  var holdColor = { r: 220, g: 200, b: 30, a: 255 };
  var tape = holdColor;
  var image = { fileUrl: TEST_IMAGE, width: TEST_IMAGE_WIDTH, height: TEST_IMAGE_HEIGHT };
  var note = "Test note";
  client.routes.getRoutes(null, null, null, function (allRoutesResponse) {
    assert.equal(allRoutesResponse.success, true, "" +
        "getRoutes success =" + true);
    var testRoutes = allRoutesResponse.data.filter(function(route) { return route.name == name });
    if(testRoutes.length == 0)
    {
      client.routes.addRoute(sectionId, name, author, holdColor, gradeId, tape, note, image, function (routeAddedResponse) {
        updateRoute(routeAddedResponse.data.id, sectionId, gradeId, assert, done, client);
      });
    }
    else {
      updateRoute(testRoutes[0].id, sectionId, gradeId, assert, done, client);
    }
  });
}

function updateRoute(routeId, sectionId, gradeId, assert, done, client)
{
  name = "2";
  author = "Morten ";
  holdColor = { r: 220, g: 200, b: 30, a: 255 };
  tape = holdColor;
  image = { fileUrl: TEST_IMAGE, width: TEST_IMAGE_WIDTH / 2, height: TEST_IMAGE_HEIGHT / 2 };
  note = "Test note ";
  client.routes.updateRoute(routeId, sectionId, author, name, holdColor, gradeId, tape, note, image, function (routeResponse) {
    assert.equal(routeResponse.success, true, "updateRoute success = " + true + " message = " + routeResponse.message);
    assert.equal(routeResponse.data.id, routeId, "updateRoute id = " + routeId);
    assert.equal(routeResponse.data.gradeId, gradeId, "updateRoute gradeId = " + gradeId);
    assert.equal(routeResponse.data.colorOfHolds.r, holdColor.r, "updateRoute colorOfHolds.r = " + holdColor.r);
    assert.equal(routeResponse.data.colorOfTape.a, tape.a, "updateRoute colorOfTape.a = " + tape.a);
    assert.equal(routeResponse.data.name, name, "updateRoute name = " + name);
    assert.equal(routeResponse.data.note, note, "updateRoute note = " + note);
    getRoutes(routeId, sectionId, gradeId, assert, done, client);
  });
}

function getRoutes(routeId, sectionId, gradeId, assert, done, client)
{
  client.routes.getRoutes(null, null, null, function (routesResponse) {
    assert.equal(routesResponse.success, true, "getRoutes success = " + true);
    assert.ok(routesResponse.data.length > 0, "getRoutes length > 0");
    //Added route is in the list of all routes
    assert.equal(routesResponse.data.filter(function(r) { return r.id == routeId; }).length, 1, "getRoutes contains added route");
    //Get added route
    getRoute(routeId, sectionId, gradeId, assert, done, client);
  });
}

function getRoute(routeId, sectionId, gradeId, assert, done, client)
{
  client.routes.getRoute(routeId, function (getRouteResponse) {
    assert.equal(getRouteResponse.success, true, "getRoute success = " + true);
    assert.equal(getRouteResponse.data.id, routeId, "getRoute id = " + routeId);
    assert.equal(getRouteResponse.data.gradeId, gradeId, "getRoute gradeId = " + gradeId);
    assert.equal(getRouteResponse.data.colorOfHolds.a, holdColor.a, "getRoute colorOfHolds.a = " + holdColor.a);
    assert.equal(getRouteResponse.data.colorOfTape.b, tape.b, "getRoute colorOfTape.b = " + tape.b);
    assert.equal(getRouteResponse.data.name, name, "getRoute name = " + name);
    getImage(routeId, assert, done, client);
  });
}

function getImage(routeId, assert, done, client)
{
  client.routes.getImage(routeId, function (imageResponse) {
    assert.equal(imageResponse.success, true, "getImage success = " + true);
    assert.equal(imageResponse.data.fileUrl, TEST_IMAGE, "getImage fileUrl");
    deleteRoute(routeId, assert, done, client);
  });
}

function deleteRoute(routeId, assert, done, client)
{
  client.routes.deleteRoute(routeId, function (deleteResponse) {
    assert.equal(deleteResponse.success, true, "deleteRoute success = " + true);
    //End of test
    done();
  });
}