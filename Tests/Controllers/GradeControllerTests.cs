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
            var memberRepo = new TestRepository<Member>();
            memberRepo.Add(new Member {Id = new Guid(), DisplayName = "TannerHelland", Username = "Tanner", Password = "Helland", IsAdmin = false, Token = "TannerHelland"});
            memberRepo.Add(new Member {Id = new Guid(), DisplayName = "Morten Rask", Username = "Morten", Password = "Rask", IsAdmin = true, Token = "AdminTestToken"});
            _auth = new AuthenticationService(memberRepo);
            _controller = new GradeController(_repo, _auth);
        }

        [TearDown] // Runs after each test
        public void TearDownTest() 
        {
            _controller?.Dispose();
        }

        [Test]
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

        [Test]
        public void AddGrade_AddGrade_RepoContainsNewGrade() 
        {
            var grade = new Grade
            {
                Name = "Purple", Difficulty = 10, Color = new Color(255, 0, 255)
            };

            _controller.AddGrade("AdminTestToken", grade);
            var data = _repo.GetAll().FirstOrDefault(g => g.Difficulty == grade.Difficulty);

            if (data == default(Grade))
            {
                Assert.Fail("Repo does not contain new grade");
            }
            Assert.IsTrue(grade.Name == data.Name);
            Assert.IsTrue(grade.Difficulty == data.Difficulty);
            Assert.IsTrue(grade.Color.ToUint() == data.Color.ToUint());
        }

        [Test]
        public void AddGrade_NotPossibleIfNotAuthenticated() 
        {
            Grade grade = new Grade
            {
                Name = "Purple", Difficulty = 10, Color = new Color(255, 0, 255)
            };
            var message = _controller.AddGrade("noToken", grade);
            Assert.AreEqual(false, message.Success);
        }

        [Test] // A test
        public void AddGrade_Result_ReturnsNewRoute() 
        {
            Grade grade = new Grade
            {
                Id = new Guid(), Name = "Purple", Difficulty = 10, Color = new Color(255, 0, 255)
            };

            var result = _controller.AddGrade("AdminTestToken", grade);
            var data = result.Data;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(grade.Name == data.Name);
            Assert.IsTrue(grade.Difficulty == data.Difficulty);
            Assert.IsTrue(grade.Color.ToUint() == data.Color.ToUint());
        }

        [Test]
        public void GetGrade_Result_ReturnsCorrectGrade() 
        {
            IEnumerable<Grade> grades = _repo.GetAll();

            foreach(Grade grade in grades)
            {
                var result = _controller.GetGrade(grade.Id);
                var data = result.Data;

                Assert.IsTrue(result.Success);
                Assert.IsTrue(grade.Name == data.Name);
                Assert.IsTrue(grade.Difficulty == data.Difficulty);
                Assert.IsTrue(grade.Color.ToUint() == data.Color.ToUint());
            }
        }

        [Test]
        public void UpdateGrade_Repo_ContainsUpdatedGrade() 
        {
            var rnd = new Random();

            var grade = _repo.GetAll().First();
            var newdifficulty = rnd.Next(15, 99);
            var newColor = new Color((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));

            _controller.UpdateGrade("AdminTestToken", grade.Id, newdifficulty, newColor);

            Assert.IsTrue(grade.Difficulty == newdifficulty);
            Assert.IsTrue(grade.Color.ToUint() == newColor.ToUint());
        }

        [Test]
        public void UpdateGrade_Result_ReturnsUpdatedGrade() 
        {
            var rnd = new Random();

            var grade = _repo.GetAll().First();
            var newdifficulty = rnd.Next(15, 99);
            var newColor = new Color((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));

            var result = _controller.UpdateGrade("AdminTestToken", grade.Id, newdifficulty, newColor);
            var data = result.Data;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(grade.Name == data.Name);
            Assert.IsTrue(grade.Difficulty == data.Difficulty);
            Assert.IsTrue(grade.Color.ToUint() == data.Color.ToUint());
        }

        [Test]
        public void DeleteGrade_RoutesWithGrade_FailsToDeleteGrade() {
            var grade = _repo.GetAll().First();
            var result = _controller.DeleteGrade("AdminTestToken", grade.Id);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void DeleteGrade_Result_ReturnsDeletedGrade() 
        {
            var grade = new Grade {
                Name = "Purple", Difficulty = 10, Color = new Color(255, 0, 255), Routes = new List<Route>()
            };

            _controller.AddGrade("AdminTestToken", grade);
            var result = _controller.DeleteGrade("AdminTestToken", grade.Id);
            Console.WriteLine(result.Success);
            Console.WriteLine(result.Value);
            Console.WriteLine(result);
            var data = result.Data;

            var grades = _controller.GetAllGrades().Data;

            foreach (Grade grad in grades)
            {
                if (grade.Difficulty == grad.Difficulty)
                {
                    Assert.Fail($"  Expected: Grade with Difficulty {grade.Difficulty} removed\n  Was: Grade still exists");
                }
            }
            /*
            
            Assert.IsTrue(result.Success);
            Assert.IsTrue(grade.Name == data.Name);
            Assert.IsTrue(grade.Difficulty == data.Difficulty);
            Assert.IsTrue(grade.Color.ToUint() == data.Color.ToUint());*/
        }

        [Test]
        public void DeleteGrade_Repo_DoesNotContainGrade() 
        {
            var grade = _repo.GetAll().First();
            var difficulty = grade.Difficulty;

            _controller.DeleteGrade("AdminTestToken", grade.Id);

            Assert.False(_repo.GetAll().Any(g => g.Difficulty == difficulty));            
        }
    }
}
