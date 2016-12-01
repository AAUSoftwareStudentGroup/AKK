var viewModel;

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

/*
//downloadRoutes cannot be tested, since it cannot read property null of selectedSection.id
*/

QUnit.test("admin panel viewModel changeSection", function (assert) 
{
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL), new TestDialogService());

	viewModel.downloadSections();
	var selectedSection = viewModel.selectedSection;

	//Test that no section is selected prior to calling changeSection
	assert.equal(selectedSection, null, "admin panel viewModel changeSection");

	//Change section
	viewModel.changeSection(TEST_SECTIONS[0].id);

	//Test that section[0] is selected
	assert.equal(viewModel.selectedSection, viewModel.sections[0], "admin panel viewModel changeSection");
});

//This does not make use of th addNewSection method, therefore invalid test
QUnit.test("admin panel viewModel addNewSection", function (assert) 
{
	var dialogService = new TestDialogService();
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL), dialogService);

	viewModel.downloadSections();
	
	var lengthBeforeAdd = viewModel.sections.length;

	//Add a section to viewModel.sections, and thereafter assert that viewModel.sections.length is one more than lengthBeforeAdd
	viewModel.sections.push(
		{
      		"name": "X",
      		"routes": [],
      		"id": "AAABBBCCCDDDEEEFFF"
    	});


	//Assert that a section has been added
	assert.equal(viewModel.sections.length, lengthBeforeAdd+1, "");

	//Assert that the most recently pushed section is the one that was just added
	assert.equal(viewModel.sections[viewModel.sections.length-1].name, "X", "");
	assert.equal(viewModel.sections[viewModel.sections.length-1].id, "AAABBBCCCDDDEEEFFF", "");
});

QUnit.test("admin panel viewModel clearSection", function (assert) 
{
	var dialogService = new TestDialogService();
	viewModel = new AdminPanelViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL), dialogService);

	viewModel.downloadSections();
	
	//Assert that previous to the pushing of a route to section[1], the are no routes in this section
	assert.equal(viewModel.sections[1].routes.length, 0, "");

	viewModel.sections[1].routes.push
	(
		{
      		"memberId": "d346c25f-fb01-4b12-88ef-4fbe7c96f33d",
	      "sectionId": "69a4472a-4997-47e0-a543-f40fdc015591",
	      "author": "Morten Rask",
	      "sectionName": "A",
	      "name": "43",
	      "createdDate": "2016-11-11T00:00:00",
	      "grade": {
	        "name": "Red",
	        "difficulty": 2,
	        "hexColor": 3830665471,
	        "color": {
	          "r": 228,
	          "g": 83,
	          "b": 80,
	          "a": 1
	        },
	        "id": "92cc529e-52d3-4b1a-a365-da63d96e740e"
	      },
	      "gradeId": "92cc529e-52d3-4b1a-a365-da63d96e740e",
	      "pendingDeletion": false,
	      "image": null,
	      "colorOfHolds": {
	        "r": 255,
	        "g": 0,
	        "b": 255,
	        "a": 1
	      },
	      "colorOfTape": null,
	      "id": "7f98e1cd-f785-4827-843e-a2d49af3d6cb"
	    }
	);

	//After the push of one route, this should be reflected in viewModel.sections[1].routes.length
	assert.equal(viewModel.sections[1].routes.length, 1, "");

	viewModel.clearSection();



    assert.equal(viewModel.sections[1].routes.length, 0, "");
	

});