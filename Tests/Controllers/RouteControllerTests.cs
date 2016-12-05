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
using Microsoft.AspNetCore.Http;

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
        private IRepository<Member> _memberRepo;
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

            _memberRepo = new TestRepository<Member>();
            _auth = new AuthenticationService(_memberRepo);
            _memberRepo.Add(new Member {Id = new Guid(), DisplayName = "TannerHelland", Username = "Tanner", Password = _auth.HashPassword("Helland"), IsAdmin = false, Token = "TannerHelland"});
            _memberRepo.Add(new Member {Id = new Guid(), DisplayName = "Morten Rask", Username = "Morten", Password = _auth.HashPassword("Rask"), IsAdmin = true, Token = "AdminTestToken"});
            _controller = new RouteController(_routeRepo, _sectionRepo, _gradeRepo, _imageRepo, _holdRepo, _memberRepo, _auth);
            
            testRoute = new Route();
            testRoute.GradeId = _gradeRepo.GetAll().First().Id;
            testRoute.SectionId = _sectionRepo.GetAll().First().Id;
            testRoute.Name = "50";
            testRoute.Member = _memberRepo.GetAll().First();
            testRoute.Author = testRoute.Member.DisplayName;
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
            var grade = _gradeRepo.GetAll().First(g => g.Name == "Green");
            var response = _controller.GetRoutes(grade.Id, null, null, 0, SortOrder.Newest);
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
            var response = _controller.AddRoute(token, testRoute);

            Assert.AreEqual(true, response.Success, (response as ApiErrorResponse<Route>)?.ErrorMessage + "\n" + testRoute.SectionId);

            Assert.AreEqual(true, _dataFactory.Routes.AsEnumerable<Route>().Contains(testRoute));
        }

        [Test]
        public void _AddRoute_NewRouteWithBadGradeId_RouteDoesntGetAdded()
        {
            testRoute.GradeId = new Guid();
            var response = _controller.AddRoute(token, testRoute);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _AddRoute_NewRouteWithBadSectionId_RouteDoesntGetAdded()
        {
            testRoute.SectionId = new Guid();
            var response = _controller.AddRoute(token, testRoute);
            
            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _AddRoute_NewRouteWithAnExistingID_RouteGetsAdded()
        {
            testRoute.SectionId = _sectionRepo.GetAll().First().Id;

            var response = _controller.AddRoute(token, testRoute);

            Assert.AreEqual(true, response.Success);
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
            var response = _controller.GetImage(_routeRepo.GetAll().First().Id);

            Assert.AreEqual(true, response.Success);
        }

        [Test]
        public void _GetImage_GetNonExistingImageByID_NoImageReturned()
        {
            Image test = new Image();

            var response = _controller.GetImage(test.Id);

            Assert.AreEqual(false, response.Success);
        }
/*
        [Test]
        public void _AddBeta_AddBetaToRouteAsMember_BetaGetsAdded()
        {
            IFormFile beta;
            beta.OpenReadStream();
            var response = _controller.AddBeta(token, beta, _routeRepo.GetAll().FirstOrDefault().Id, "");
            Assert.IsTrue(response.Result.Success);
            Assert.IsNotEmpty(_routeRepo.GetAll().First().Videos);
        }
*/
        [Test]
        public void _UpdateRoute_UpdateNameOnRoute_NameGetsUpdated()
        {
            Route routeToUpdate = _routeRepo.GetAll().First();
            routeToUpdate.Name = "40";

            var response = _controller.UpdateRoute(token, routeToUpdate.Id, routeToUpdate);

            Assert.AreEqual(true, response.Success);

            Assert.AreEqual("40", response.Data.Name);
            Assert.AreEqual("40", _routeRepo.Find(routeToUpdate.Id).Name);
        }

        [Test]
        public void _UpdateRoute_UpdateGradeIdOnRoute_GradeGetsUpdated()
        {
            Route routeToUpdate = _routeRepo.GetAll().First();
            Guid gradeId = _gradeRepo.GetAll().First(g => g.Difficulty == 3).Id;
            routeToUpdate.GradeId = gradeId;

            var response = _controller.UpdateRoute(token, routeToUpdate.Id, routeToUpdate);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(response.Data.GradeId, gradeId);
            Assert.AreEqual(_routeRepo.Find(routeToUpdate.Id).GradeId, gradeId);
        }

        [Test]
        public void _UpdateRoute_UpdateNameAndId_NameGetsUpdatedAndIdDoesNotUpdate() 
        {
            Route routeToUpdate = _routeRepo.Find(_routeRepo.GetAll().First().Id).Clone();
            Assert.AreNotEqual(routeToUpdate, null);
            Guid oldId = routeToUpdate.Id;
            routeToUpdate.Name = "40";
            routeToUpdate.Id = Guid.NewGuid();

            Assert.AreNotEqual(_routeRepo.Find(oldId), null);

            var response = _controller.UpdateRoute(token, oldId, routeToUpdate);

            Assert.AreEqual(true, response.Success);

            Assert.AreEqual("40", response.Data.Name);
            Assert.AreEqual(oldId, response.Data.Id);
            Assert.AreEqual("40", _routeRepo.Find(oldId).Name);
        }

        [Test]
        public void _UpdateRoute_UpdateSectionId_SectionIdGetsUpdated() 
        {
            Route routeToUpdate = _routeRepo.GetAll().First();
            routeToUpdate.SectionId = _routeRepo.GetAll().First(s => s.Section.Name == "C").SectionId;

            var response = _controller.UpdateRoute(token, routeToUpdate.Id, routeToUpdate);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(routeToUpdate.SectionId, response.Data.SectionId);
            Assert.AreEqual(routeToUpdate.SectionId, _routeRepo.Find(routeToUpdate.Id).SectionId);
        }

        [Test]
        public void _UpdateRoute_UpdateSectionIdAndGradeId_SectionIdAndGradeIdGetsUpdated() 
        {
            Route routeToUpdate = _routeRepo.GetAll().First();
            Guid gradeId = _gradeRepo.GetAll().First(g => g.Difficulty == 3).Id;
            Guid sectionId = _routeRepo.GetAll().First(s => s.Section.Name == "C").SectionId;
            routeToUpdate.GradeId = gradeId;
            routeToUpdate.SectionId = sectionId;

            var response = _controller.UpdateRoute(token, routeToUpdate.Id, routeToUpdate);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(response.Data.GradeId, gradeId);
            Assert.AreEqual(_routeRepo.Find(routeToUpdate.Id).GradeId, gradeId);
            Assert.AreEqual(routeToUpdate.SectionId, response.Data.SectionId);
            Assert.AreEqual(routeToUpdate.SectionId, _routeRepo.Find(routeToUpdate.Id).SectionId);
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
        public void _UpdateRoute_UpdateTapeOnRouteRemoveTape_TapeGetsRemoved()
        {
            Route Origroute = _routeRepo.GetAll().First(t => t.Author == "Manfred");
            Route test = new Route();

            test.Author = Origroute.Author;
            test.ColorOfHolds = Origroute.ColorOfHolds;
            test.ColorOfTape = null;
            test.CreatedDate = Origroute.CreatedDate;
            test.Grade = Origroute.Grade;
            test.GradeId = Origroute.GradeId;
            test.HexColorOfHolds = Origroute.HexColorOfHolds;
            test.HexColorOfTape = null;

            var response = _controller.UpdateRoute(token, Origroute.Id, testRoute);

            Assert.IsNull(Origroute.ColorOfTape);
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
            var response = _controller.DeleteRoute(token, new Guid());

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

        [Test]
        public void _SetRating_SetRatingAsMemberToRoute_RatingGetsAdded()
        {
            Route route = _routeRepo.GetAll().First();
            var response = _controller.SetRating(token, route.Id, 5);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, route.Ratings.Count);
            Assert.AreEqual(5, route.Ratings.First().RatingValue);
        }

        [Test]
        public void _SetRating_SetRatingAsMemberToRouteWhichHasSeveralRatings_AverageIsCorrect()
        {
            Route route = _routeRepo.GetAll().First();
            _controller.SetRating(token, route.Id, 5);
            _controller.SetRating(_auth.Login("Tanner", "Helland"), route.Id, 3);
            
            Member testMember = new Member();
            testMember.Password = "123";
            testMember.Username = "test";
            testMember.IsAdmin = false;
            testMember.DisplayName = "test123";
            _memberRepo.Add(testMember);
            var response = _controller.SetRating(_auth.Login("test", "123"), route.Id, 1);
            Assert.IsTrue(response.Success);

            Assert.AreEqual(3, route.Ratings.Count);
            Assert.AreEqual(3, route.AverageRating);
        }

        [Test]
        public void _SetRating_SetRatingAsGuest_NoRatingGetsAdded()
        {
            Route route = _routeRepo.GetAll().First();
            var response = _controller.SetRating("123", route.Id, 5);
            Assert.IsFalse(response.Success);
            Assert.IsEmpty(route.Ratings);
        }

        [Test]
        public void _SetRating_SetRatingAsMemberWhoAddedIt_RatingGetsUpdated()
        {
            Route route = _routeRepo.GetAll().First();
            var tokenForMember = _auth.Login("Tanner", "Helland");
            _controller.SetRating(tokenForMember, route.Id, 5);
            var response = _controller.SetRating(tokenForMember, route.Id, 2);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(2, route.Ratings.First().RatingValue);
        }

        [Test]
        public void _SetRating_SetRatingAsGuest_RatingDoesntGetChanged()
        {
            Route route = _routeRepo.GetAll().First();
            _controller.SetRating(token, route.Id, 5);
            var response = _controller.SetRating("123", route.Id, 2);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(5, route.Ratings.First().RatingValue);
        }
    }
}
