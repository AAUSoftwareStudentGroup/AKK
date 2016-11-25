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
        private IAuthenticationService _auth;

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
            var memberRepo = new TestRepository<Member>();
            memberRepo.Add(new Member {Id = new Guid(), DisplayName = "TannerHelland", Username = "Tanner", Password = "Helland", IsAdmin = false, Token = "TannerHelland"});
            memberRepo.Add(new Member {Id = new Guid(), DisplayName = "Morten Rask", Username = "Morten", Password = "Rask", IsAdmin = true, Token = "AdminTestToken"});
            _auth = new AuthenticationService(memberRepo);
            _controller = new RouteController(_routeRepo, _sectionRepo, _gradeRepo, new TestRepository<Image>(), new TestRepository<Hold>(), _auth);
        }

        [TearDown] // Runs after each test
        public void TearDownTest() 
        {
            _controller?.Dispose();
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
            Route testRoute = new Route();
            testRoute.Grade = _gradeRepo.GetAll().First();
            testRoute.Section = _sectionRepo.GetAll().First();
            testRoute.Name = "50";
            testRoute.ColorOfHolds = _routeRepo.GetAll().First().ColorOfHolds;
            
            var token = _auth.Login("Tanner", "Helland");
            var response = _controller.AddRoute(token, testRoute, testRoute.Section.Name);
            var message = response as ApiErrorResponse<Route>;
            if (response.Success != true)
            {
                Assert.Fail(message.ErrorMessage);
            }
         //   Assert.AreEqual(true, response.Success, response.Success.);
        }
    }
}
