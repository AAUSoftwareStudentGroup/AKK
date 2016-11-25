using AKK.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Runtime.InteropServices.ComTypes;
using AKK.Controllers.ApiResponses;
using AKK.Models;
using AKK.Models.Repositories;
using AKK.Services;

namespace AKK.Tests.Controllers
{
    [TestFixture]
    public class RouteControllerTests
    {
        private TestDataFactory _dataFactory;
        private RouteController _controller;
        private IRepository<Section> _sectionRepo;
        private IRepository<Grade> _gradeRepo;
        private IRepository<Route> _routeRepo;
        private IRepository<Image> _imageRepo;
        private IRepository<Hold> _holdRepo;
        private IAuthenticationService _auth;

        private string token;
        private Route testRoute;

        [OneTimeSetUp] // Runs once before first test
        public void SetUpSuite() { }

        [OneTimeTearDown] // Runs once after last test
        public void TearDownSuite() { }

        [SetUp] // Runs before each test
        public void SetupTest () 
        { 
            _dataFactory = new TestDataFactory();
            _sectionRepo = new TestRepository<Section>(_dataFactory.Sections);
            _gradeRepo = new TestRepository<Grade>(_dataFactory.Grades);
            _routeRepo = new TestRepository<Route>(_dataFactory.Routes);
            _imageRepo = new TestRepository<Image>(_dataFactory._images);
            _holdRepo = new TestRepository<Hold>(_dataFactory._holds);

            var memberRepo = new TestRepository<Member>();
            memberRepo.Add(new Member {Id = new Guid(), DisplayName = "TannerHelland", Username = "Tanner", Password = "Helland", IsAdmin = false, Token = "TannerHelland"});
            memberRepo.Add(new Member {Id = new Guid(), DisplayName = "Morten Rask", Username = "Morten", Password = "Rask", IsAdmin = true, Token = "AdminTestToken"});
            _auth = new AuthenticationService(memberRepo);
            _controller = new RouteController(_routeRepo, _sectionRepo, _gradeRepo, _imageRepo, _holdRepo, _auth);
            
            testRoute = new Route();
            testRoute.Grade = _gradeRepo.GetAll().First();
            testRoute.Section = _sectionRepo.GetAll().First();
            testRoute.Name = "50";
            testRoute.Author = "TannerHelland";
            testRoute.ColorOfHolds = _routeRepo.GetAll().First().ColorOfHolds;

            token = _auth.Login("Tanner", "Helland");
        }

        [TearDown] // Runs after each test
        public void TearDownTest() 
        {
            _controller?.Dispose();
            _dataFactory = null;
            _sectionRepo = null;
            _gradeRepo = null;
            _routeRepo = null;
            _imageRepo = null;
            _holdRepo = null;
            testRoute = null;
            token = null;
        }

        [Test]
        public void _GetRoutes_GettingAllRoutesInTheSystem_ExpectTheyreAllThere()
        {
            var response = _controller.GetRoutes(null,null,null,0,SortOrder.Newest);
            var routes = response.Data;
            Assert.AreEqual(true, response.Success);
            
            Assert.AreEqual(_routeRepo.GetAll().Count(), routes.Count());
        }

        [Test]
        public void _GetRoutes_GettingRoutesOfCertainGrade_ExpectOnlyRoutesWithThatGrade()
        {
            var response = _controller.GetRoutes(0, null, null, 0, SortOrder.Newest);
            var routes = response.Data;

            Assert.AreEqual(true, response.Success);

            foreach (var route in routes)
            {
                if (route.Grade.Name != "Green")
                {
                    Assert.Fail($"  Expected: Route with Grade Green\n  But Was: {route.Grade.Name}");
                }
            }
            
            Assert.Pass();
        }

        [Test]
        public void _GetRoutes_Getting5Routes_ExpectOnly5Routes()
        {
            var response = _controller.GetRoutes(null,null,null,5,SortOrder.Newest);
            var routes = response.Data;

            Assert.AreEqual(true, response.Success);

            Assert.AreEqual(5, routes.Count());
        }

        [Test]
        public void _GetRoutes_GettingRoutesFromSectionWithID_ExpectOnlyRoutesFromThatSection()
        {
            var section = _sectionRepo.GetAll().First();
            var response = _controller.GetRoutes(null, section.Id, null, 0, SortOrder.Newest);
            var routes = response.Data;

            Assert.AreEqual(true, response.Success);

            Assert.AreEqual(section.Routes.Count, routes.Count());
        }

        [Test]
        public void _AddRoute_NewRouteGetsAdded_RouteGetsAdded()
        {
            var response = _controller.AddRoute(token, testRoute, testRoute.Section.Name);
            var message = response as ApiErrorResponse<Route>;

            Assert.AreEqual(true, response.Success);

            Assert.AreEqual(true, _dataFactory.Routes.AsEnumerable<Route>().Contains(testRoute));
        }

        [Test]
        public void _AddRoute_NewRouteWithBadGradeDifficultyAdded_RouteDoesntGetAdded()
        {
            Grade testGrade = new Grade();
            testGrade.Difficulty = 10;
            testGrade.Color = testRoute.Grade.Color;
            
            testRoute.Grade = testGrade;
            var response = _controller.AddRoute(token, testRoute, testRoute.Section.Name);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _AddRoute_NewRouteWithBadGradeNameAdded_RouteGetsAddedButNameIsTheWhatsInTheDatabase()
        {
            Grade testGrade = new Grade();
            testGrade.Name = "Purple";
            testGrade.Color = testRoute.Grade.Color;
            
            testRoute.Grade = testGrade;
            var response = _controller.AddRoute(token, testRoute, testRoute.Section.Name);

            Assert.AreEqual(true, response.Success);
            Assert.AreNotEqual(testGrade, response.Data.Grade);
        }

        [Test]
        public void _AddRoute_NewRouteWithBadSectionNameAdded_RouteDoesntGetAdded()
        {
            Section testSection = new Section();
            testSection.Name = "E";

            testRoute.Section = testSection;
            var response = _controller.AddRoute(token, testRoute, testRoute.Section.Name);
            
            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _AddRoute_NewRouteWithBadSectionNameAddedButWithAnExistingID_RouteGetsAddedButWithValidSectionName()
        {
            testRoute.Section = new Section();
            testRoute.Section.Id = _sectionRepo.GetAll().First().Id;
            testRoute.SectionId = _sectionRepo.GetAll().First().SectionId;
            testRoute.Section.Name = "E";

            var response = _controller.AddRoute(token, testRoute, testRoute.Section.Name);

            Assert.AreEqual(true, response.Success);
            Assert.AreNotEqual("E", response.Data.Section.Name);
        }

        [Test]
        public void _DeleteAllRoutes_AdminDeletesAllRoutes_AllRoutesGetDeleted()
        {
            var token = _auth.Login("Morten", "Rask");
            var response = _controller.DeleteAllRoutes(token);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(0, _routeRepo.GetAll().Count());
        }

        [Test]
        public void _DeleteAllRoutes_MemberDeletesAllRoutes_NoRoutesGetDeleted()
        {
            var response = _controller.DeleteAllRoutes(token);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _DeleteAllRoutes_GuestDeletesAllRoutes_NoRoutesGetDeleted()
        {
            var response = _controller.DeleteAllRoutes("123");

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _GetRoute_GetExistingRouteByItsID_RouteReturned()
        {
            testRoute = _routeRepo.GetAll().First();
            var response = _controller.GetRoute(testRoute.Id);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(testRoute, response.Data);
        }

        [Test]
        public void _GetRoute_GetNonExistingRoute_NoRouteFound()
        {
            var response = _controller.GetRoute(testRoute.Id);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _GetImage_GetExistingImageByItsID_ImageReturned()
        {
            Image test = _imageRepo.GetAll().First();

            var response = _controller.GetImage(test.Id);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(test, response.Data);
        }

        [Test]
        public void _GetImage_GetNonExistingImageByID_NoImageReturned()
        {
            Image test = new Image();

            var response = _controller.GetImage(test.Id);

            Assert.AreEqual(false, response.Success);
        }
    }
}
