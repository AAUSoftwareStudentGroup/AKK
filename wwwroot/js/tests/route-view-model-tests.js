var viewModel;

QUnit.test("route viewModel downloadSections", function (assert) {
    viewModel = new RouteViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService());

    var sectionUpdatedTriggered = false;

    viewModel.addEventListener("sectionsUpdated", function () {
        sectionUpdatedTriggered = true;
    });

    viewModel.downloadSections();

    var times = 0;
    if (viewModel.sections.length > 0) {
        for (var i = 0; i < viewModel.sections.length; i++) {
            if (viewModel.sections[i] == TEST_SECTIONS[i]) {
                times++;
            }
        }
        assert.equal(times, viewModel.sections.length, "route ViewModel downloadsections");
    }
    assert.equal(sectionUpdatedTriggered, true, "route ViewModel sectionsUpdated triggered");
});

QUnit.test("route viewModel downloadGrades", function (assert) {
    viewModel = new RouteViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService());

    var downloadGradesTriggered = false;

    viewModel.addEventListener("gradesUpdated",
        function() {
            downloadGradesTriggered = true;
        });

    viewModel.downloadGrades();
    
    var times = 0;

    if (viewModel.grades.length > 0) {
        for (var i = 0; i < viewModel.grades.length; i++) {
            if (viewModel.grades[i] == TEST_GRADES[i]) {
                times++;
            }
        }
        assert.equal(times, viewModel.grades.length, "route ViewModel downloadgrades");
    }
    assert.equal(downloadGradesTriggered, true, "route ViewModel gradesUpdated triggered");
});

QUnit.test("route viewModel add holds", function (assert) {
    viewModel = new RouteViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService());

    var holdsUpdatedtriggered = false;

    viewModel.addEventListener("holdsUpdated",
        function () {
            holdsUpdatedtriggered = true;
        });

    var hold = { x: 1, y: 4, radius: 10 };

    viewModel.addHold(hold);

    assert.equal(holdsUpdatedtriggered, true, "route ViewModel HoldsUpdated triggered");
    assert.equal(viewModel.HoldPositions[0], hold, "route viewModel addHolds");

});


QUnit.test("route viewModel toggle tape", function (assert) {
    viewModel = new RouteViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService());

    var holdsUpdatedtriggered = false;

    viewModel.addEventListener("holdsUpdated",
        function () {
            holdsUpdatedtriggered = true;
        });

    viewModel.hasTape = false;
    viewModel.toggleTape();

    assert.equal(viewModel.hasTape, true, "route viewModel toggle tape");

    assert.equal(holdsUpdatedtriggered, true, "route ViewModel HoldsUpdated triggered");

});

QUnit.test("route viewModel change attributes", function (assert) {
    viewModel = new RouteViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService());

    //initialize stuff in viewModel
    viewModel.downloadSections();
    viewModel.downloadGrades();
    viewModel.downloadHolds();

    var section = TEST_SECTIONS[2];
    var grade = TEST_GRADES[1];
    var author = "Mathias";
    var number = 4;
    var note = "Test note";
    var color = TEST_HOLDS[0];

    viewModel.changeSection(section.id);
    viewModel.changeGrade(grade.id);
    viewModel.changeHold(color.colorOfHolds);
    viewModel.changeNumber(number);
    viewModel.changeAuthor(author);
    viewModel.changeNote(note);
    viewModel.changeTape(color.colorOfHolds);

    assert.equal(viewModel.selectedSection, section);
    assert.equal(viewModel.selectedGrade, grade, "route viewModel chamge grade");
    assert.equal(viewModel.selectedHold.colorOfHolds.r == color.colorOfHolds.r && viewModel.selectedHold.colorOfHolds.g == color.colorOfHolds.g && viewModel.selectedHold.colorOfHolds.b == color.colorOfHolds.b, true, "route viewModel change hold");
    assert.equal(viewModel.number, number, "route viewModel change number");
    assert.equal(viewModel.author, author, "route viewModel change author");
    assert.equal(viewModel.selectedTape.colorOfHolds.r == color.colorOfHolds.r && viewModel.selectedTape.colorOfHolds.b == color.colorOfHolds.b  && viewModel.selectedTape.colorOfHolds.g == color.colorOfHolds.g, true, "route viewModel  change tape");
    assert.equal(viewModel.note, note, "route viewModel change note");
});

