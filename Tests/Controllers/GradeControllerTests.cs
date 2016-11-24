using AKK.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using AKK.Controllers.ApiResponses;
using AKK.Models;
using AKK.Models.Repositories;
using AKK.Services;

namespace AKK.Tests.Controllers
{
    [TestFixture]
    public class GradeControllerTests
    {
        private TestDataFactory _dataFactory;
        private GradeController _controller;
        private IRepository<Grade> _repo;
        private IAuthenticationService _auth;

        [OneTimeSetUp] // Runs once before first test
        public void SetUpSuite() { }

        [OneTimeTearDown] // Runs once after last test
        public void TearDownSuite() { }

        [SetUp] // Runs before each test
        public void SetupTest () 
        { 
            _dataFactory = new TestDataFactory();
            _repo = new TestRepository<Grade>(_dataFactory.Grades);
            _auth = new TestAuthenticationService();
            _controller = new GradeController(_repo, _auth);
        }

        [TearDown] // Runs after each test
        public void TearDownTest() 
        {
            _controller?.Dispose();
        }

        [Test] // A test
        public void GetAllGrades_Result_ReturnsAll() 
        {
            var result = _controller.GetAllGrades();
            var data = result.Data;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(data.Count() == _repo.GetAll().Count());

            foreach(Grade g in _repo.GetAll()) 
            {
                Assert.Contains(g, data.ToArray());
            }
        }

        [Test] // A test
        public void AddGrade_Repo_ContainsNewRoute() 
        {
            Grade data;

            Grade grade = new Grade() {Name = "Purple", Difficulty = 10, Color = new Color(255, 0, 255)};

            _controller.AddGrade("123", grade);
            data =  _repo.GetAll().FirstOrDefault(g => g.Difficulty == grade.Difficulty);

            Assert.IsTrue(grade.Name == data.Name);
            Assert.IsTrue(grade.Difficulty == data.Difficulty);
            Assert.IsTrue(grade.Color.ToUint() == data.Color.ToUint());
        }

        [Test]
        public void AddGrade_NotPossibleIfNotAuthenticated() 
        {
            ApiResponse<Grade> message;

            Grade grade = new Grade
            {
                Name = "Purple", Difficulty = 10, Color = new Color(255, 0, 255)
            };
            message = _controller.AddGrade("noToken", grade);
            Assert.AreEqual(false, message.Success);
            
        }

        [Test] // A test
        public void AddGrade_Result_ReturnsNewRoute() 
        {
            ApiResponse<Grade> result;
            Grade data;

            Grade grade = new Grade
            {
                Id = new Guid(), Name = "Purple", Difficulty = 10, Color = new Color(255, 0, 255)
            };

            result = _controller.AddGrade("123", grade);
            data = result.Data;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(grade.Name == data.Name);
            Assert.IsTrue(grade.Difficulty == data.Difficulty);
            Assert.IsTrue(grade.Color.ToUint() == data.Color.ToUint());
        }

        [Test] // A test
        public void GetGrade_Result_ReturnsCorrectGrade() 
        {
            ApiResponse<Grade> result;
            Grade data;

            IEnumerable<Grade> grades = _repo.GetAll();

            foreach( Grade grade in grades )
            {
                result = _controller.GetGrade(grade.Difficulty.ToString());
                data = result.Data;

                Assert.IsTrue(result.Success);
                Assert.IsTrue(grade.Name == data.Name);
                Assert.IsTrue(grade.Difficulty == data.Difficulty);
                Assert.IsTrue(grade.Color.ToUint() == data.Color.ToUint());
            }
        }

        [Test] // A test
        public void UpdateGrade_Repo_ContainsUpdatedGrade() 
        {
            Random rnd = new Random();

            Grade grade = _repo.GetAll().First();
            int newdifficulty = rnd.Next(15, 99);
            Color newColor = new Color((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));

            _controller.UpdateGrade("123", grade.Difficulty.ToString(), newdifficulty, newColor);

            Assert.IsTrue(grade.Difficulty == newdifficulty);
            Assert.IsTrue(grade.Color.ToUint() == newColor.ToUint());
        }

        [Test] // A test
        public void UpdateGrade_Result_ReturnsUpdatedGrade() 
        {
            ApiResponse<Grade> result;
            Grade data;
            Random rnd = new Random();

            Grade grade = _repo.GetAll().First();
            int newdifficulty = rnd.Next(15, 99);
            Color newColor = new Color((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));

            result = _controller.UpdateGrade("123", grade.Difficulty.ToString(), newdifficulty, newColor);
            data = result.Data;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(grade.Name == data.Name);
            Assert.IsTrue(grade.Difficulty == data.Difficulty);
            Assert.IsTrue(grade.Color.ToUint() == data.Color.ToUint());
        }
        /*
        [Test] // A test
        public void DeleteGrade_Result_ReturnsDeletedGrade() 
        {
            ApiResponse<Grade> result;
            Grade data;

            Grade grade = _repo.GetAll().First();
            string name = grade.Name;
            int difficulty = grade.Difficulty;
            uint? color = grade.Color.ToUint();

            // delete all routes
            new RouteController(new TestRepository<Route>(_dataFactory.Routes), new TestRepository<Section>(_dataFactory.Sections), _repo).DeleteAllRoutes();

            result = _controller.DeleteGrade(grade.Difficulty.ToString());
            data = result.Data;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(name == data.Name);
            Assert.IsTrue(difficulty == data.Difficulty);
            Assert.IsTrue(color == data.Color.ToUint());
        }

        [Test] // A test
        public void DeleteGrade_Repo_DoesNotContainRoute() 
        {
            ApiResponse<Grade> result;
            Grade data;

            Grade grade = _repo.GetAll().First();
            int difficulty = grade.Difficulty;

            // delete all routes
            new RouteController(new TestRepository<Route>(_dataFactory.Routes), new TestRepository<Section>(_dataFactory.Sections), _repo).DeleteAllRoutes();

            result = _controller.DeleteGrade(grade.Difficulty.ToString());
            data = result.Data;

            Assert.False(_repo.GetAll().Any(g => g.Difficulty == difficulty));            
        }
        */
    }
}
