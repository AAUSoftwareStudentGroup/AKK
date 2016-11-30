function TestRouteClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;
    this.getRoutes = function(grade, sectionId, sortBy, success)
    {
        success({success: true, data: TEST_ROUTES.filter(function(r) {return (grade == "" ? true : r.gradeId == grade) && (sectionId == "" ? true : r.sectionId == sectionId);})});
    };

    this.searchRoutes = function(searchstring, success) {
        success({success: true, data: TEST_ROUTES});
    }

    this.getRoute = function(id, success)
    {
        success({success: true, data: TEST_ROUTES.filter(function(r){ return r.id == id })[0]});
    };

    this.getImage = function(id, success) {
        success({success: false, data: null, errorMessage: "No image" });
    }

    this.addRoute = function(sectionId, name, author, holdColor, gradeId, tape, success) { };

    this.updateRoute = function(routeId, sectionId, author, name, holdColor, gradeId, tape, image, success) { };

    this.deleteRoute = function(id, success) { };
}

function TestSectionClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;
    this.getAllSections = function(success)
    {
        success({success: true, data: TEST_SECTIONS});
    };

    this.getSection = function(name, success)
    {
        success({success: true, data: TEST_SECTIONS.filter(function(s) { return s.id == name; })});
    };

    this.addSection = function(name, success) { };

    this.deleteSection = function(name, success) { };

    this.deleteSectionRoutes = function(name, success) { };

    this.renameSection = function(sectionId, newName, success) { };
}

function TestGradeClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;
    this.getAllGrades = function(success)
    {
        success({success: true, data: TEST_GRADES});
    };

    this.addGrade = function(grade, success) { };

    this.getGrade = function(gradeId, success)
    {
        success({success: true, data: TEST_GRADES.filter(function(r) { return r.id == gradeId; })});
    };

    this.deleteGrade = function(gradeId, success) { };
}

function TestMemberClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;
    this.logIn = function(username, password, success)
    {
        self.cookieService.setToken("TESTTOKEN");
        success({success: true, data: "TESTTOKEN"});
    };

    this.logOut = function(success)
    {
        self.cookieService.expireToken();
        success({success: true, data: TEST_TOKEN});
    };

    this.register = function(displayname, username, password, success)
    {
        success({success: true, data: TEST_TOKEN});
    };

    this.getMemberInfo = function(success) {
        success({success: true, data: TEST_MEMBER});
    }
}

function TestClient(routeUrl, sectionUrl, gradeUrl, memberUrl, cookieService)
{
    this.routes = new TestRouteClient(routeUrl, cookieService);
    this.sections = new TestSectionClient(sectionUrl, cookieService);
    this.grades = new TestGradeClient(gradeUrl, cookieService);
    this.members = new TestMemberClient(memberUrl, cookieService);
}