var viewModel;

QUnit.test("RouteViewModel downloadSections", function (assert) {
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
        assert.equal(times, viewModel.sections.length, "RouteViewModel downloadsections");
    }
    assert.equal(sectionUpdatedTriggered, true, "RouteViewModel sectionsUpdated triggered");
});

QUnit.test("RouteViewModel downloadGrades", function (assert) {
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
        assert.equal(times, viewModel.grades.length, "RouteViewModel downloadgrades");
    }
    assert.equal(downloadGradesTriggered, true, "RouteViewModel gradesUpdated triggered");
});

QUnit.test("RouteViewModel add holds", function (assert) {
    viewModel = new RouteViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService());

    var holdsUpdatedtriggered = false;

    viewModel.addEventListener("holdsUpdated",
        function () {
            holdsUpdatedtriggered = true;
        });

    var hold = { x: 1, y: 4, radius: 10 };

    viewModel.addHold(hold);

    assert.equal(holdsUpdatedtriggered, true, "RouteViewModel HoldsUpdated triggered");
    assert.equal(viewModel.HoldPositions[0], hold, "RouteViewModel addHolds");

});


QUnit.test("RouteViewModel toggle tape", function (assert) {
    viewModel = new RouteViewModel(new TestClient(API_ROUTE_URL, API_SECTION_URL, API_GRADE_URL, API_MEMBER_URL, new TestCookieService()), new TestNavigationService());

    var holdsUpdatedtriggered = false;

    viewModel.addEventListener("holdsUpdated",
        function () {
            holdsUpdatedtriggered = true;
        });

    viewModel.hasTape = false;
    viewModel.toggleTape();

    assert.equal(viewModel.hasTape, true, "RouteViewModel toggle tape");

    assert.equal(holdsUpdatedtriggered, true, "RouteViewModel HoldsUpdated triggered");

});

QUnit.test("RouteViewModel change attributes", function (assert) {
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
    assert.equal(viewModel.selectedGrade, grade, "RouteViewModel chamge grade");
    assert.equal(viewModel.selectedHold.colorOfHolds.r == color.colorOfHolds.r && viewModel.selectedHold.colorOfHolds.g == color.colorOfHolds.g && viewModel.selectedHold.colorOfHolds.b == color.colorOfHolds.b, true, "RouteViewModel change hold");
    assert.equal(viewModel.number, number, "RouteViewModel change number");
    assert.equal(viewModel.author, author, "RouteViewModel change author");
    assert.equal(viewModel.selectedTape.colorOfHolds.r == color.colorOfHolds.r && viewModel.selectedTape.colorOfHolds.b == color.colorOfHolds.b  && viewModel.selectedTape.colorOfHolds.g == color.colorOfHolds.g, true, "RouteViewModel  change tape");
    assert.equal(viewModel.note, note, "RouteViewModel change note");
});

