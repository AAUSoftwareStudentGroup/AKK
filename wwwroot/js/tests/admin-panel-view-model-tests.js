var viewModel;

//Routes
/*
//downloadRoutes cannot be tested, since it cannot read property null of selectedSection.id
*/

//Sections
QUnit.test("admin panel viewModel downloadSections", function (assert) 
{
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL), new TestDialogService());

	var sectionChangedTriggered = false;

	viewModel.addEventListener("sectionsChanged", function () 
	{
        sectionChangedTriggered = true;
    });

    viewModel.downloadSections();

    var times = 0;
    if (viewModel.sections.length > 0) {
        for (var i = 0; i < viewModel.sections.length; i++) 
        {
            if (viewModel.sections[i] == TEST_SECTIONS[i]) 
            {
                times++;
            }
        }
        assert.equal(times, viewModel.sections.length, "admin panel ViewModel downloadSections");
    }
    assert.equal(sectionChangedTriggered, true, "admin panel ViewModel sectionsChanged triggered");
});

QUnit.test("admin panel viewModel changeSection", function (assert) 
{
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL), new TestDialogService());

	viewModel.downloadSections();
	var selectedSection = viewModel.selectedSection;

	//Test that no section is selected prior to calling changeSection
	assert.equal(selectedSection, null, "admin panel viewModel changeSection");

	//Change section
	viewModel.changeSection(TEST_SECTIONS[0].id);

	//Test that section[0] is selected
	assert.equal(viewModel.selectedSection, viewModel.sections[0], "admin panel viewModel changeSection");
});

QUnit.test("admin panel viewModel addNewSection", function (assert) 
{
	init();

	var dialogService = new TestDialogService();
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL), dialogService);

	viewModel.downloadSections();
	
	//Assert that before adding a new section, the number of sections is 4
	assert.equal(viewModel.sections.length, 4, "number of sections =" + 4);

	//Add a section to TEST_SECTIONS
	dialogService.message = "test";
	viewModel.addNewSection();

	//Assert that a section has been added
	assert.equal(viewModel.sections[viewModel.sections.length-1].name, "test", "viewModel section name = " + "test");
	assert.equal(viewModel.sections.length, 5, "number of sections =" + 5)
});

QUnit.test("admin panel viewModel clearSection", function (assert) 
{
	var dialogService = new TestDialogService();
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL), dialogService);

	viewModel.downloadSections();
	
	//Change to a section to clear
	viewModel.changeSection(viewModel.sections[1].id);

	//Assert that the selected section is B
	assert.equal(viewModel.selectedSection.name, viewModel.sections[1].name, "changeSection selectedSection = " + viewModel.sections[1].name);
	//Assert that 4 routes belong to section B
    assert.equal(TEST_ROUTES.filter(function(r){return r.sectionId == viewModel.sections[1].id; }).length, 4, "Number of routes in selectedSection = " + 4);


	viewModel.clearSection();

	//Assert that after clearing the selectedSection = B, no routes belong to section B
    assert.equal(TEST_ROUTES.filter(function(r){return r.name == viewModel.sections[1].name; }).length, 0, "Number of routes in selectedSection = " + 0);
});

QUnit.test("admin panel viewModel deleteSection", function (assert) 
{
	init();
	var dialogService = new TestDialogService();
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL), dialogService);

	viewModel.init();

	//Change to section B, save id to a variable, since the section will be deleted later
	var sectionBId = TEST_SECTIONS[1].id;
	viewModel.changeSection(sectionBId);

	//Assert that the selected section is B
	assert.equal(viewModel.selectedSection.name, viewModel.sections[1].name, "changeSection selectedSection = " + viewModel.sections[1].name);
	
	//Assert that 4 routes belong to section B
    assert.equal(viewModel.routes.filter(function(r){return r.sectionId == viewModel.sections[1].id; }).length, 4, "Number of routes in selectedSection = " + 4);

	//Delete section B
	viewModel.deleteSection();
	
	//Assert that no section B exists
	for(var i = 0; i<viewModel.sections.length; i++){
		assert.notEqual(viewModel.sections[i].name, "B", "sections[i].name != B");
	}	

	//Assert that no routes with section B's id exist
	for (var i = 0; i<viewModel.routes.length; i++) {
		assert.notEqual(viewModel.routes[i].sectionId, sectionBId, "route[i].Id != section B ID (be2c65c9-e7c0-4d31-a600-bb3aa2dd4310)" + viewModel.routes[i].sectionId);
	} 
});

QUnit.test("admin panel viewModel renameSection", function (assert) 
{
	init();
	var dialogService = new TestDialogService();
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL), dialogService);

	viewModel.init();

	//Change to section C, save id to a variable
	var sectionCId = viewModel.sections[2].id;
	viewModel.changeSection(sectionCId);
	//Assert that the selected section is C
	assert.equal(viewModel.selectedSection.name, viewModel.sections[2].name, "selectedSection.name sections[2].name = " + viewModel.sections[2].name);

	//Save amount of routes in section C, to compare later
	var routesInC = viewModel.sections[2].routes.length;

	dialogService.message = "X";
	viewModel.renameSection();

	//Assert that the section is now named X instead of C
	assert.equal(viewModel.sections[2].name, "X", "sections[2].name = X = " + viewModel.sections[2].name);

	//Assert that it has the same amount of routes
	assert.equal(viewModel.sections[2].routes.length, routesInC, "sections[2].length = routesInC = " + viewModel.sections[2].routes.length);
});

//Grades
QUnit.test("admin panel viewModel downloadGrades", function (assert) 
{
	init();
	var dialogService = new TestDialogService();
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL), dialogService);

	var gradeChangedTriggered = false;

	viewModel.addEventListener("gradesChanged", function () 
	{
        gradeChangedTriggered = true;
    });

	//Not equal before downloading grades
	assert.notEqual(viewModel.grades, TEST_GRADES, "");

	//Download grades
	viewModel.downloadGrades();

	//Equal after downloading grades, also event triggered
	assert.equal(viewModel.grades, TEST_GRADES, "");
	assert.equal(gradeChangedTriggered, true, "admin panel ViewModel gradesChanged triggered");
});

QUnit.test("admin panel viewModel addNewGrade", function (assert) 
{
	var dialogService = new TestDialogService();
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL), dialogService);

	init();
	viewModel.init();

	var gradeChangedTriggered = false;

	viewModel.addEventListener("gradesChanged", function () 
	{
        gradeChangedTriggered = true;
    });

	var tempGradeCount = viewModel.grades.length;

	assert.equal(tempGradeCount, TEST_GRADES.length, "");

    viewModel.addNewGrade();

	assert.equal(gradeChangedTriggered, true, "admin panel ViewModel gradesChanged triggered");
	assert.equal(viewModel.grades[viewModel.grades.length-1].name, "New Grade", "");
	assert.equal(tempGradeCount+1, TEST_GRADES.length, "");
	assert.equal(viewModel.grades[viewModel.grades.length-1].color.r == TEST_GRADES[TEST_GRADES.length-1].color.r && viewModel.grades[viewModel.grades.length-1].color.g == TEST_GRADES[TEST_GRADES.length-1].color.g && viewModel.grades[viewModel.grades.length-1].color.b == TEST_GRADES[TEST_GRADES.length-1].color.b, true, "");
});

QUnit.test("admin panel viewModel deleteGrade", function (assert) 
{
	var dialogService = new TestDialogService();
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL), dialogService);

	init();
	viewModel.init();

	//Add grade to delete, since a grade with routes cannot be deleted
	viewModel.addNewGrade();

	//Select grade
	viewModel.selectGrade(viewModel.grades.length-1);

	//Id is by default undefined, to delete a grade it needs an id, which will be set here
	viewModel.selectedGrade.id = "99xx529e-52d3-4b1a-a365-da63d96e888x"

	var gradeId = viewModel.selectedGrade.id;
	var numOfGrades = viewModel.grades.length;

	//Assert that the correct grade was selected
	assert.equal(viewModel.selectedGrade.name, viewModel.grades[viewModel.grades.length-1].name, "");

	viewModel.deleteGrade();

	//Assert that the grade has now been deselected
	assert.equal(viewModel.selectedGrade, null, "");

	//Assert that no grade exists with the same id
	for(var i = 0; i<=numOfGrades-1; i++){
		assert.notEqual(viewModel.grades[i].id, gradeId);
	}
});

QUnit.test("admin panel viewModel changeGradeName", function (assert) 
{
	var dialogService = new TestDialogService();
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, API_HOLD_URL), dialogService);

	init();
	viewModel.init();

	//Select first grade (has name green)
	viewModel.selectGrade(0);

	//Assert that selected grade has name green and id '5b314675-435f-49bb-8fae-44c1567f9e69'
	assert.equal(viewModel.selectedGrade.id == "5b314675-435f-49bb-8fae-44c1567f9e69" && viewModel.selectedGrade.name == "Green", true, "");

	viewModel.changeGradeName("CAMO");

	assert.equal(viewModel.selectedGrade.id == "5b314675-435f-49bb-8fae-44c1567f9e69" && viewModel.selectedGrade.name == "CAMO", true, "");
});