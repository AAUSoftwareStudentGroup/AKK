function clone(obj)
{
    return obj;
}

//http://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript
function guid() {
  function s4() {
    return Math.floor((1 + Math.random()) * 0x10000)
      .toString(16)
      .substring(1);
  }
  return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
    s4() + '-' + s4() + s4() + s4();
}

function TestRouteClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;
    this.getRoutes = function(grade, sectionId, sortBy, success)
    {
        success({success: true, data: TEST_ROUTES.filter(function(r) {return (grade == "" || grade == null ? true : r.gradeId == grade) && (sectionId == ""  || sectionId == null ? true : r.sectionId == sectionId);})});
    };

    this.searchRoutes = function(searchstring, success) {
        success({success: true, data: clone(TEST_ROUTES)});
    }

    this.getRoute = function(id, success)
    {
        success({success: true, data: clone(TEST_ROUTES.filter(function(r){ return r.id == id })[0])});
    };

    this.getImage = function(id, success) {
        success({success: true, data: { fileUrl: TEST_IMAGE, width: TEST_IMAGE_WIDTH, height: TEST_IMAGE_HEIGHT } });
    }

    this.addRoute = function(sectionId, name, author, holdColor, gradeId, tape, note, image, success) 
    { 
        console.log(TEST_HOLDS.filter(function(h) { return h.colorOfHolds.r == tape.r && h.colorOfHolds.g == tape.g && h.colorOfHolds.b == tape.b })[0].colorOfHolds);
        var newRoute = {
            "memberId": "testid",
            "sectionId": sectionId,
            "author": author,
            "sectionName": TEST_SECTIONS.filter(function(s) { return s.id == sectionId; })[0].name,
            "name": name,
            "createdDate": new Date(),
            "grade": TEST_GRADES.filter(function(g) { return g.id == gradeId; })[0],
            "gradeId": gradeId,
            "pendingDeletion": false,
            "image": null,
            "colorOfHolds": TEST_HOLDS.filter(function(h) { return h.colorOfHolds.r == holdColor.r && h.colorOfHolds.g == holdColor.g && h.colorOfHolds.b == holdColor.b })[0].colorOfHolds,
            "colorOfTape": TEST_HOLDS.filter(function(h) { return h.colorOfHolds.r == tape.r && h.colorOfHolds.g == tape.g && h.colorOfHolds.b == tape.b })[0].colorOfHolds,
            "id": guid()
        };
        TEST_ROUTES.push(newRoute);
        success({success: true, data: newRoute });
    };

    this.updateRoute = function(routeId, sectionId, author, name, holdColor, gradeId, tape, note, image, success) 
    { 
        var route = TEST_ROUTES.filter(function(r) { return r.id == routeId; })[0];
        if(sectionId != null)
        {
            route.sectionId = sectionId;
            route.sectionName = TEST_SECTIONS.filter(function(s) { return s.id == sectionId; })[0].name;
        }
        if(author != null)    
            route.author = author;
        if(name != null)
            route.name = name;
        if(gradeId != null)
        {
            route.grade = TEST_GRADES.filter(function(g) { return g.id == gradeId; })[0];
            route.gradeId = gradeId;
        }
        route.image = null;
        if(holdColor != null)
            route.colorOfHolds = TEST_HOLDS.filter(function(h) { return h.colorOfHolds.r == holdColor.r && h.colorOfHolds.g == holdColor.g && h.colorOfHolds.b == holdColor.b })[0].colorOfHolds;
        if(tape != null)
            route.colorOfTape = TEST_HOLDS.filter(function(h) { return h.colorOfHolds.r == tape.r && h.colorOfHolds.g == tape.g && h.colorOfHolds.b == tape.b })[0].colorOfHolds;
        if(note != null)
            route.note = note;
        success({success: true, data: route });
    };

    this.deleteRoute = function(id, success) 
    { 
        var index = -1;
        var route = null;
        for(var i = 0; i < TEST_ROUTES.length; i++)
        {
            if(TEST_ROUTES[i].id == id)
            {
                index = i;
                route = TEST_ROUTES[i];
                break;
            }
        }
        if (index > -1) {
            TEST_ROUTES.splice(index, 1);
            success({success: true, data: route });
        }
        else
        {
            success({success: false, data: null, errorMessage: "No route exist" });
        }
    };
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
        section.name = newName;
        success({success: true, data: clone(section)});
    };
}

function TestGradeClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;
    var grades = TEST_GRADES;
    this.getAllGrades = function(success)
    {
        success({success: true, data: TEST_GRADES});
    };

    this.addGrade = function(grade, success) 
    { 
        var newGrade = {name: grade.name, difficulty: grade.difficulty, color: grade.color};
        grades.push(newGrade);
        success({success: true, data: clone(newGrade)});
    };

    this.getGrade = function(gradeId, success)
    {
        success({success: true, data: TEST_GRADES.filter(function(r) { return r.id == gradeId; })});
    };

    this.deleteGrade = function(gradeId, success) 
    {
        var grade;
        this.getGrade(gradeId, function(g) { grade = g.data });
        grades.splice(grades.indexOf(grade), 1);
        success({success: true, data: clone(grade)});
    };

    this.updateGrade = function(grade, success) 
    {
    };
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
    };

    this.getMemberRatings = function(success) {
        success({TEST_RATINGS});
    };

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