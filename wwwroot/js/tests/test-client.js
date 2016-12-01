function clone(obj)
{
    return obj;
}

function TestRouteClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;
    this.getRoutes = function(grade, sectionId, sortBy, success)
    {
        success({success: true, data: TEST_ROUTES.filter(function(r) {return (grade == "" ? true : r.gradeId == grade) && (sectionId == "" ? true : r.sectionId == sectionId);})});
    };

    this.searchRoutes = function(searchstring, success) {
        success({success: true, data: clone(TEST_ROUTES)});
    }

    this.getRoute = function(id, success)
    {
        success({success: true, data: clone(TEST_ROUTES.filter(function(r){ return r.id == id })[0])});
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
    var sections = TEST_SECTIONS;
    var idCounter = 0;
    this.cookieService = cookieService;
    this.getAllSections = function(success)
    {
        success({success: true, data: clone(TEST_SECTIONS)});
    };

    this.getSection = function(name, success)
    {
        success({success: true, data: clone(TEST_SECTIONS.filter(function(s) { return s.name == name || s.id == name; })[0])});
    };

    this.addSection = function(name, success) 
    { 
        var newSection = {id: idCounter++, name: name};
        sections.push(newSection);
        success({success: true, data: clone(newSection)});
    };

    this.deleteSection = function(name, success) 
    { 
        var section;
        this.getSection(name, function(s) { section = s.data; });
        this.deleteSectionRoutes(name, function(){});
        sections.splice(sections.indexOf(section), 1);
        success({success: true, data: clone(section)});
    };

    this.deleteSectionRoutes = function(name, success) 
    { 
        var sectionId;
        this.getSection(name, function(r) { sectionId = r.data.id; });
        var deletedRoutes = TEST_ROUTES.filter(function(route) { return route.sectionId == sectionId; });
        TEST_ROUTES = TEST_ROUTES.filter(function(route) { return route.sectionId != sectionId; });
        success({success: true, data: clone(deletedRoutes)});
    };

    this.renameSection = function(sectionId, newName, success) 
    { 
        var section;
        this.getSection(sectionId, function(s) { section = s.data; });
        console.log(section.name);
        section.name = newName;
        success({success: true, data: clone(section)});
    };
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
    this.members = TEST_MEMBERS;
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

    this.getAllMembers = function(success){ 
        success({success: true, data: TEST_MEMBERS});
    };
}

function TestHoldClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;
    this.getAllHolds = function(success) 
    { 
        success({success: true, data: TEST_HOLDS});
    };
}

function TestClient(routeUrl, sectionUrl, gradeUrl, memberUrl, holdUrl, cookieService)
{
    this.routes = new TestRouteClient(routeUrl, cookieService);
    this.sections = new TestSectionClient(sectionUrl, cookieService);
    this.grades = new TestGradeClient(gradeUrl, cookieService);
    this.members = new TestMemberClient(memberUrl, cookieService);
    this.holds = new TestHoldClient(holdUrl, cookieService);
}