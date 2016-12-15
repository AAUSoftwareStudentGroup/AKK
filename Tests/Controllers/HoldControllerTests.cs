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
    public class HoldControllerTests
    {
        private TestDataFactory _dataFactory;
        private HoldColorController _controller;
        private IRepository<HoldColor> _repo;
        private IAuthenticationService _auth;
        string token;
        [OneTimeSetUp] // Runs once before first test
        public void SetUpSuite() { }

        [OneTimeTearDown] // Runs once after last test
        public void TearDownSuite() { }

        [SetUp] // Runs before each test
        public void SetupTest () 
        { 
            _dataFactory = new TestDataFactory();
            _repo = new TestRepository<HoldColor>(_dataFactory.HoldColors);
            var memberRepo = new TestRepository<Member>();
            _auth = new AuthenticationService(memberRepo);
            memberRepo.Add(new Member {Id = new Guid(), DisplayName = "TannerHelland", Username = "Tanner", Password = _auth.HashPassword("Helland"), IsAdmin = false, Token = "TannerHelland"});
            memberRepo.Add(new Member {Id = new Guid(), DisplayName = "Morten Rask", Username = "Morten", Password = _auth.HashPassword("Rask"), IsAdmin = true, Token = "AdminTestToken"});
            _controller = new HoldColorController(_repo, _auth);
            token = _auth.Login("Morten", "Rask");
        }

        [TearDown] // Runs after each test
        public void TearDownTest() 
        {
            _controller = null;
            _dataFactory = null;
            _repo = null;
            _auth = null;
            token = null;
        }

        [Test]
        public void GetAllHoldColors_GettingHoldColors_ExpectAllHolds()
        {
            var response = _controller.GetAllHoldColors();

            Assert.IsTrue(response.Success);
            Assert.AreEqual(15, _dataFactory.HoldColors.Count);
        }

        [Test]
        public void AddHoldColor_AddNewHoldColorAsAdmin_ExpectNewHold()
        {
            HoldColor testColor = new HoldColor();
            testColor.Name = "TestColor";
            testColor.ColorOfHolds = new Color(255, 255, 255);
            var response = _controller.AddHoldColor(token, testColor);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(16, _dataFactory.HoldColors.Count);
            Assert.IsTrue(_dataFactory.HoldColors.Any(h => h.Name == "TestColor"));
        }

        [Test]
        public void AddHoldColor_AddNewHoldColorAsMember_NoHoldGetsAdded()
        {
            HoldColor testColor = new HoldColor();
            testColor.Name = "TestColor";
            testColor.ColorOfHolds = new Color(255, 255, 255);
            var response = _controller.AddHoldColor("TannerHelland", testColor);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(15, _dataFactory.HoldColors.Count);
        }

        [Test]
        public void AddHoldColor_AddNewHoldColorAsGuest_NoHoldGetsAdded()
        {
            HoldColor testColor = new HoldColor();
            testColor.Name = "TestColor";
            testColor.ColorOfHolds = new Color(255, 255, 255);
            var response = _controller.AddHoldColor("123", testColor);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(15, _dataFactory.HoldColors.Count);
        }
        
        [Test]
        public void AddHoldColor_AddNewHoldColorAsAdminWithNoColor_NoHoldGetsAdded()
        {
            HoldColor testColor = new HoldColor();
            testColor.Name = "TestColor";
            testColor.ColorOfHolds = null;
            var response = _controller.AddHoldColor(token, testColor);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(15, _dataFactory.HoldColors.Count);
        }
        
        [Test]
        public void AddHoldColor_AddNewHoldColorAsAdminWithNullName_HoldGetsAdded()
        {
            HoldColor testColor = new HoldColor();
            testColor.Name = null;
            testColor.ColorOfHolds = new Color(255, 255, 255);
            var response = _controller.AddHoldColor(token, testColor);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(16, _dataFactory.HoldColors.Count);
        }

        [Test]
        public void DeleteHoldColor_DeleteHoldThatExistsAsAdmin_HoldGetsRemoved()
        {
            var response = _controller.DeleteHoldColor(token, _dataFactory.HoldColors.First().Id);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(14, _dataFactory.HoldColors.Count);
        }

        [Test]
        public void DeleteHoldColor_DeleteHoldThatExistsAsMember_HoldDoesntGetRemoved()
        {
            var response = _controller.DeleteHoldColor("TannerHelland", _dataFactory.HoldColors.First().Id);
            Assert.IsFalse(response.Success);
            Assert.AreEqual(15, _dataFactory.HoldColors.Count);
        }

        [Test]
        public void DeleteHoldColor_DeleteHoldThatExistsAsGuest_HoldDoesntGetRemoved()
        {
            var response = _controller.DeleteHoldColor("123", _dataFactory.HoldColors.First().Id);
            Assert.IsFalse(response.Success);
            Assert.AreEqual(15, _dataFactory.HoldColors.Count);
        }

        [Test]
        public void DeleteHoldColor_DeleteHoldThatDoesntExistAsAdmin_ExpectError()
        {
            var response = _controller.DeleteHoldColor(token, Guid.NewGuid());
            Assert.IsFalse(response.Success);
            Assert.AreEqual(15, _dataFactory.HoldColors.Count);
        }
    }
}