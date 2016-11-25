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

            token = _auth.Login("Morten", "Rask");
        }

        [TearDown] // Runs after each test
        public void TearDownTest() 
        {
            _dataFactory = null;
            _sectionRepo = null;
            _gradeRepo = null;
            _routeRepo = null;
            _imageRepo = null;
            _holdRepo = null;
            _auth = null;
            _controller = null;
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
            var response = _controller.DeleteAllRoutes(token);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(0, _routeRepo.GetAll().Count());
        }

        [Test]
        public void _DeleteAllRoutes_MemberDeletesAllRoutes_NoRoutesGetDeleted()
        {
            var token = _auth.Login("Tanner", "Helland");
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

        [Test]
        public void _UpdateRoute_UpdateNameOnRoute_NameGetsUpdated()
        {
            Route Origroute = _routeRepo.GetAll().First();
            testRoute.Author = Origroute.Author;
            testRoute.Name = "40";

            var response = _controller.UpdateRoute(token, Origroute.Id, testRoute);

            Assert.AreEqual(true, response.Success);

            Assert.AreEqual("40", Origroute.Name);
        }

        [Test]
        public void _UpdateRoute_UpdateGradeOnRouteWithChangedGrade_GradeGetsUpdated()
        {
            Route Origroute = _routeRepo.GetAll().First();
            testRoute.Author = Origroute.Author;
            testRoute.Grade = _gradeRepo.GetAll().First(g => g.Difficulty == 3);

            var response = _controller.UpdateRoute(token, Origroute.Id, testRoute);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(testRoute.Grade.Name, Origroute.Grade.Name);
        }

        [Test]
        public void _UpdateRoute_UpdateGradeOnRouteWithChangedID_GradeGetsUpdated() 
        {
            Route Origroute = _routeRepo.GetAll().First();
            testRoute.Author = Origroute.Author;
            testRoute.Grade = _gradeRepo.GetAll().First(g => g.Difficulty == 3);
            testRoute.GradeId = testRoute.Grade.Id;

            var response = _controller.UpdateRoute(token, Origroute.Id, testRoute);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(testRoute.Grade.Name, Origroute.Grade.Name);
        }

        [Test]
        public void _UpdateRoute_UpdateSectionOnRouteWithChangedSectionName_SectionGetsUpdated() 
        {
            Route Origroute = _routeRepo.GetAll().First();
            testRoute.Author = Origroute.Author;
            testRoute.Section = _routeRepo.GetAll().First(s => s.Section.Name == "C").Section;

            var response = _controller.UpdateRoute(token, Origroute.Id, testRoute);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(testRoute.Section.Name, Origroute.Section.Name);
        }

        [Test]
        public void _UpdateRoute_UpdateSectionOnRouteWithChangedSectionID_SectionGetsUpdated() 
        {
            Route Origroute = _routeRepo.GetAll().First();
            testRoute.Author = Origroute.Author;
            testRoute.SectionId = _routeRepo.GetAll().First(s => s.Section.Name == "C").SectionId;

            var response = _controller.UpdateRoute(token, Origroute.Id, testRoute);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(testRoute.Section.Name, Origroute.SectionName);
        }

        [Test]
        public void _UpdateRoute_UpdateSectionOnRouteWithChangedSectionIDSectionNameAndSection_SectionGetsUpdated() 
        {
            Route Origroute = _routeRepo.GetAll().First();
            testRoute.Author = Origroute.Author;
            Route temp = _routeRepo.GetAll().First(s => s.Section.Name == "C");
            testRoute.SectionId = temp.SectionId;
            testRoute.Section = temp.Section;

            var response = _controller.UpdateRoute(token, Origroute.Id, testRoute);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(testRoute.Section.Name, Origroute.SectionName);
        }

        [Test]
        public void _UpdateRoute_UpdateAuthorOnRoute_AuthorGetsUpdated()
        {
            Route Origroute = _routeRepo.GetAll().First();
            if(Origroute.Author != "TannerHelland")
            {
                testRoute.Author = "TannerHelland";
            }
            else
            {
                testRoute.Author = "AdminAdmin";
            }
            
            var response = _controller.UpdateRoute(token, Origroute.Id, testRoute);

            Assert.AreEqual(testRoute.Author, Origroute.Author);
        }

        [Test]
        public void _UpdateRoute_UpdateHoldOnRoute_HoldGetsUpdated()
        {
            Route Origroute = _routeRepo.GetAll().First();
            testRoute.ColorOfHolds = _routeRepo.GetAll().First(s => s.Section.Name == "C").ColorOfHolds;
            
            if(Origroute.ColorOfHolds == testRoute.ColorOfHolds)
            {
                if(testRoute.ColorOfHolds.R < 255)
                    testRoute.ColorOfHolds.R++;
                else
                    testRoute.ColorOfHolds.R--;
            }
            var response = _controller.UpdateRoute(token, Origroute.Id, testRoute);

            Assert.AreEqual(testRoute.ColorOfHolds.R, Origroute.ColorOfHolds.R);
        }

        [Test]
        public void _UpdateRoute_AddTapeOnRouteWithNoTape_TapeGetsAdded()
        {
            Route Origroute = _routeRepo.GetAll().First();
            testRoute.ColorOfTape = _routeRepo.GetAll().First(t => t.Author == "Manfred").ColorOfTape;

            var response = _controller.UpdateRoute(token, Origroute.Id, testRoute);

            Assert.AreEqual(testRoute.ColorOfTape.G, Origroute.ColorOfTape.G);
        }

        [Test]
        public void _UpdateRoute_UpdateTapeOnRouteWithTape_TapeGetsUpdated()
        {
            Route Origroute = _routeRepo.GetAll().First();
            Color temp = _routeRepo.GetAll().First(t => t.Author == "Manfred").ColorOfTape;

            Origroute.ColorOfTape = temp;
            testRoute.ColorOfTape = temp;
            testRoute.ColorOfTape.R += 1;

            var response = _controller.UpdateRoute(token, Origroute.Id, testRoute);

            Assert.AreEqual(testRoute.ColorOfTape.R, Origroute.ColorOfTape.R);
        }

        [Test]
        public void _UpdateRoute_UpdateUserWhileNotAuthenticated_ErrorResponseAndRouteNotUpdated()
        {
            Route Origroute = _routeRepo.GetAll().First();
            
            var response = _controller.UpdateRoute("123", Origroute.Id, testRoute);

            Assert.AreEqual(false, response.Success);
            Assert.AreNotEqual(testRoute, Origroute);
        }

        [Test]
        public void _DeleteRoute_DeleteExistingRoute_RouteGetsDeleted()
        {
            Route Origroute = _routeRepo.GetAll().First();
            int numberOfRoutes = _routeRepo.GetAll().Count();
            var response = _controller.DeleteRoute(token, Origroute.Id);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(numberOfRoutes-1, _routeRepo.GetAll().Count());
            Assert.AreNotEqual(Origroute.Author, _routeRepo.GetAll().First().Author);
        }

        [Test]
        public void _DeleteRoute_DeleteRouteThatDoesntExist_Error()
        {
            var response = _controller.DeleteRoute(token, testRoute.Id);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _DeleteRoute_DeleteRouteAsGuest_Error()
        {
            Route Origroute = _routeRepo.GetAll().First();
            var response = _controller.DeleteRoute("123", Origroute.Id);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _DeleteRoute_DeleteRouteWithIdOfSection_Error()
        {
            var response = _controller.DeleteRoute(token, _sectionRepo.GetAll().First().Id);

            Assert.AreEqual(false, response.Success);
        }
    }
}
