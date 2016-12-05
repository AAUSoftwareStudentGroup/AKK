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
    public class SectionControllerTests
    {
        private TestDataFactory _dataFactory;
        private SectionController _controller;
        private IRepository<Section> _sectionRepo;
        private IRepository<Grade> _gradeRepo;
        private IRepository<Route> _routeRepo;
        private IAuthenticationService _auth;

        private string token;

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
            _auth = new AuthenticationService(memberRepo);
            memberRepo.Add(new Member {Id = new Guid(), DisplayName = "TannerHelland", Username = "Tanner", Password = _auth.HashPassword("Helland"), IsAdmin = false, Token = "TannerHelland"});
            memberRepo.Add(new Member {Id = new Guid(), DisplayName = "Morten Rask", Username = "Morten", Password = _auth.HashPassword("Rask"), IsAdmin = true, Token = "AdminTestToken"});
            _controller = new SectionController(_sectionRepo, _auth, _routeRepo);

            token = _auth.Login("Morten", "Rask");
        }

        [TearDown] // Runs after each test
        public void TearDownTest() 
        {
            _controller = null;
            _dataFactory = null;
            _sectionRepo = null;
            _gradeRepo = null;
            _routeRepo = null;
            _auth = null;
            token = null;
        }

        [Test]
        public void _GetAllSections_GettingAllSectionsInTheSystem_ExpectTheyreAllThere()
        {
            var response = _controller.GetAllSections();
            var sections = response.Data;
            Assert.AreEqual(true, response.Success);
            
            Assert.AreEqual(_sectionRepo.GetAll().Count(), sections.Count());
        }

        [Test]
        public void _GetSections_GettingAllSectionsInTheSystem_ExpectAllRoutesAreThere()
        {
            var response = _controller.GetAllSections();
            var sections = response.Data;

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(_sectionRepo.GetAll().First().Routes[0].Author, sections.First().Routes[0].Author);
        }

        [Test]
        public void _AddSection_NewSectionGetsAdded_SectionGetsAdded()
        {
            var response = _controller.AddSection(token, "E");

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(5, _sectionRepo.GetAll().Count());
        }

        [Test]
        public void _AddSection_AddSectionWithNameOfSectionThatAlreadyExists_NoSectionAdded()
        {
            var response = _controller.AddSection(token, "A");

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _AddSection_AddSectionAsGuest_NoSectionAdded()
        {
            var response = _controller.AddSection("123", "E");

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _AddSection_AddSectionAsMember_NoSectionAdded()
        {
            var token = _auth.Login("Tanner", "Helland");
            var response = _controller.AddSection(token, "E");

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _DeleteAllSections_AdminDeletesAllSections_AllSectionsGetDeleted()
        {
            var response = _controller.DeleteAllSections(token);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(0, _sectionRepo.GetAll().Count());
        }

       [Test]
        public void _DeleteAllSections_MemberDeletesAllSections_AllSectionsGetDeleted()
        {
            var token = _auth.Login("Tanner", "Helland");
            var response = _controller.DeleteAllSections(token);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _DeleteAllSections_GuestDeletesAllSections_AllSectionsGetDeleted()
        {
            var response = _controller.DeleteAllSections("123");

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _GetSection_GettingRoutesFromSectionA_ExpectAllRoutes()
        {
            var response = _controller.GetSection("A");
            var section = response.Data;

            Assert.AreEqual(true, response.Success);

            Assert.AreEqual(4, section.Routes.Count);
        }

        [Test]
        public void _GetSection_GettingRoutesFromSectionThatDoesntExist_ExpectError()
        {
            var response = _controller.GetSection("E");
            var section = response.Data;

            Assert.AreEqual(false, response.Success);
        }
                [Test]
        public void _DeleteSection_DeleteExistingSection_SectionGetsDeleted()
        {
            int numberOfSections = _sectionRepo.GetAll().Count();
            var response = _controller.DeleteSection(token, "A");

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(numberOfSections-1, _sectionRepo.GetAll().Count());
            Assert.AreNotEqual("A", _sectionRepo.GetAll().First().Name);
        }

        [Test]
        public void _DeleteSection_DeleteSectionThatDoesntExist_Error()
        {
            var response = _controller.DeleteSection(token, "E");

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _DeleteSection_DeleteSectionAsGuest_Error()
        {
            var response = _controller.DeleteSection("123", "A");

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _DeleteSection_DeleteSectionAsMember_Error()
        {
            token = _auth.Login("Tanner", "Helland");
            var response = _controller.DeleteSection(token, "A");

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _GetSectionRoutes_GetSectionRoutesOfSectionThatExists_GetRoutes()
        {
            var response = _controller.GetSectionRoutes("A");
            Assert.AreEqual(true, response.Success);

            int numberOfRoutes = response.Data.Count();
            Assert.AreEqual(numberOfRoutes, _sectionRepo.GetAll().First().Routes.Count);

            for (int i = 0; i < numberOfRoutes; i++)
            {
                Route route = response.Data.ElementAt(i);
                if (route.Id != _sectionRepo.GetAll().First().Routes.ElementAt(i).Id)
                {
                    Assert.Fail($"  Expected: Routes of Same ID\n  But Was: {route.Id} != {_sectionRepo.GetAll().First().Routes.ElementAt(i).Id}");
                }
            }
        }

        [Test]
        public void _GetSectionRoutes_GetSectionRoutesOfSectionThatDoesntExists_ExpectNoRoutes()
        {
            var response = _controller.GetSectionRoutes("E");
            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _DeleteSectionRoutes_DeleteSectionRoutesOfSectionThatExists_EmptySection()
        {
            var response = _controller.DeleteSectionRoutes(token, "A");
            Assert.IsTrue(response.Success);
            Assert.IsEmpty(_sectionRepo.GetAll().FirstOrDefault().Routes);
        }

        [Test]
        public void _DeleteSectionRoutes_DeleteSectionRoutesOfSectionThatDoesntExists_Error()
        {
            var response = _controller.DeleteSectionRoutes(token, "E");
            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _DeleteSectionRoutes_DeleteSectionRoutesAsMember_Error()
        {
            token = _auth.Login("Tanner", "Helland");
            var response = _controller.DeleteSectionRoutes(token, "A");
            Assert.AreEqual(false, response.Success);
        }

        [Test]
        public void _DeleteSectionRoutes_DeleteSectionRoutesAsGuest_Error()
        {
            var response = _controller.DeleteSectionRoutes("123", "A");
            Assert.AreEqual(false, response.Success);
        }
    }
}
