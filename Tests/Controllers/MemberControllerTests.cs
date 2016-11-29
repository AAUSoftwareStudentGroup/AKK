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
    public class MemberControllerTests
    {
        private TestDataFactory _dataFactory;
        private MemberController _controller;
        private IRepository<Member> _memberRepo;
        private Member _testMember;

        [OneTimeSetUp] // Runs once before first test
        public void SetUpSuite() { }

        [OneTimeTearDown] // Runs once after last test
        public void TearDownSuite() { }

        [SetUp] // Runs before each test
        public void SetupTest () 
        { 
            _dataFactory = new TestDataFactory();
            _memberRepo = new TestRepository<Member>(_dataFactory.Members);

            _testMember = new Member();
            _testMember.Username = "Test";
            _testMember.Password = "Member";
            _testMember.DisplayName = _testMember.Username + _testMember.Password;
            _testMember.IsAdmin = false;
            _testMember.Token = null;

            _controller = new MemberController(_memberRepo);
        }

        [TearDown] // Runs after each test
        public void TearDownTest() 
        {
            _controller = null;
            _dataFactory = null;
            _memberRepo = null;
            _testMember = null;
        }

        [Test]
        public void _Login_UserThatExistsLogsIn_UserLogsIn()
        {
            var member = _dataFactory.Members.First();

            var response = _controller.Login(member.Username, member.Password);

            Assert.IsTrue(response.Success);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(response.Data, member.Token);
        }

        [Test]
        public void _Login_UserThatDoesntExistLogsIn_Error()
        {
            var response = _controller.Login(_testMember.Username, _testMember.Password);

            Assert.IsFalse(response.Success);
            Assert.IsNull(_testMember.Token);
        }

        [Test]
        public void _Login_UserThatIsAlreadyLoggedInLogsIn_ExpectsNoChange()
        {
            var member = _dataFactory.Members.First();
            string token;
            token = _controller.Login(member.Username, member.Password).Data;

            Assert.AreEqual(token, _controller.Login(member.Username, member.Password).Data);
        }

        [Test]
        public void _GetMemberInfo_GetMemberThatExist_MemberIsReturned()
        {
            var member = _controller.GetMemberInfo(_controller.Login(_memberRepo.GetAll().First().Username, _memberRepo.GetAll().First().Password).Data).Data;

            Assert.AreEqual(_memberRepo.GetAll().First(), member);
        }
        
        [Test]
        public void _GetMemberInfo_GetMemberThatDoesntExist_ExpectError()
        {
            var response = _controller.GetMemberInfo(_controller.Login(_testMember.Username, _testMember.Password).Data);

            Assert.IsFalse(response.Success);
        }

        [Test]
        public void _Logout_MemberThatIsLoggedInLogsOut_MemberLogsOut()
        {
            var member = _dataFactory.Members.First();
            var response = _controller.Logout(_controller.Login(member.Username, member.Password).Data);

            Assert.IsTrue(response.Success);
            Assert.IsNull(member.Token);
        }

        [Test]
        public void _Logout_LoggingOutWithInvalidToken_ExpectNoChange()
        {
            var foundMember = _memberRepo.GetAll().FirstOrDefault(m => m.Token == "123");
            Assert.IsNull(foundMember);
            var response = _controller.Logout("123");
            Assert.IsNull(foundMember);
        }

        [Test]
        public void _AddMember_AddingNewMember_MemberGetsAdded()
        {
            var response = _controller.AddMember(_testMember.Username,_testMember.Password,_testMember.DisplayName);

            Assert.IsTrue(response.Success);
            var member = _memberRepo.GetAll().FirstOrDefault(m => m.DisplayName == _testMember.DisplayName);

            Assert.AreEqual(_testMember.DisplayName, member.DisplayName);
            Assert.IsNotNull(member.Token);
        }

        [Test]
        public void _AddMember_AddingNewMemberWithExistingUsername_ExpectError()
        {
            var response = _controller.AddMember(_memberRepo.GetAll().First().Username, "1234", "TannerHelland");

            Assert.IsFalse(response.Success);
        }

        [Test]
        public void _AddMember_AddingNewMemberWithNullValue_ExpectError()
        {
            var response = _controller.AddMember("Username", "Password", null);

            Assert.IsFalse(response.Success);
        }

        [Test]
        public void _GetRole_GetAllRolesOfAdmin_ExpectTwoRoles()
        {
            var member = _memberRepo.GetAll().First(m => m.IsAdmin == true);
            string token = _controller.Login(member.Username, member.Password).Data;

            var roles = _controller.GetRole(token).Data;
            int i = 0;
            foreach (Role role in roles)
            {
                i++;
            }

            Assert.AreEqual(2, i);
        }

        [Test]
        public void _GetRole_GetAllRolesOfMember_ExpectOnlyOneRole()
        {
            var member = _memberRepo.GetAll().First();
            string token = _controller.Login(member.Username, member.Password).Data;

            var roles = _controller.GetRole(token).Data;
            int i = 0;
            foreach (Role role in roles)
            {
                i++;
            }

            Assert.AreEqual(1, i);
        }

        public void _GetRole_GetAllRolesOfGuest_ExpectUnauthorised()
        {
            var response = _controller.GetRole("123");
            Assert.IsFalse(response.Success);

            int i = 0;
            foreach (Role role in response.Data)
            {
                i++;
            }

            Assert.AreEqual(0, i);
        }
    }
}