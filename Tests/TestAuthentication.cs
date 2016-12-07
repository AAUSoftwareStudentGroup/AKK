using NUnit.Framework;
using AKK.Services;
using AKK.Models.Repositories;
using AKK.Models;
using System;

namespace AKK.Tests
{
    [TestFixture]
    public class TestAuthentication
    {
        private IRepository<Member> _repo;
        private IAuthenticationService _auth;

        [OneTimeSetUp] // Runs once before first test
        public void SetUpSuite() { }

        [OneTimeTearDown] // Runs once after last test
        public void TearDownSuite() { }

        [SetUp] // Runs before each test
        public void SetupTest () 
        { 
            _repo = new TestRepository<Member>();
            _auth = new AuthenticationService(_repo);
            _repo.Add(new Member {Id = new Guid(), DisplayName = "TannerHelland", Username = "Tanner", Password = _auth.HashPassword("Helland"), IsAdmin = false, Token = "TannerHelland"});
            _repo.Add(new Member {Id = new Guid(), DisplayName = "Morten Rask", Username = "Morten", Password = _auth.HashPassword("Rask"), IsAdmin = true, Token = "AdminTestToken"});
        }

        [TearDown] // Runs after each test
        public void TearDownTest() 
        {
            _repo = null;
        }
        [Test]
        public void _Login_MemberWithUsernameTannerPasswordHelland_IsAuthenticated()
        {
            var token = _auth.Login("Tanner", "Helland");

            Assert.AreEqual(true, _auth.HasRole(token, Role.Authenticated));
        }

        [Test]
        public void _Logout_LoggingOutRemovesToken_TokenIsRemoved()
        {
            var token = _auth.Login("Tanner", "Helland");
            _auth.Logout(token);

            Assert.AreEqual(false, _auth.HasRole(token, Role.Authenticated));
        }

        [Test]
        public void _LoggingInAsSetter_TokenIsOnlySetterAndNotAdmin()
        {
            var token = _auth.Login("Tanner", "Helland");

            Assert.AreEqual(true, _auth.HasRole(token, Role.Authenticated));
            Assert.AreEqual(false, _auth.HasRole(token, Role.Admin));
        }

        [Test]
        public void _GetRoles_MortenRask_IsAuthenticatedAndIsAdmin()
        {
            var token = _auth.Login("Morten", "Rask");

            var roles = _auth.GetRoles(token);
            int i = 0;
            foreach (var role in roles)
            {
                if (role == Role.Unauthenticated)
                {
                    Assert.Fail($"  Expected: Authenticated or Admin\n  But Was: {role.ToString()}");
                }
                else if (role == Role.Admin || role == Role.Authenticated)
                {
                    i++;
                }
            }
            if (i == 2)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void _GetRoles_TannerHelland_IsAuthenticatedButNotAdmin()
        {
            var token = _auth.Login("Tanner", "Helland");

            var roles = _auth.GetRoles(token);
            int i = 0;
            foreach (var role in roles)
            {
                if (role == Role.Admin || role == Role.Unauthenticated)
                {
                    Assert.Fail($"  Expected: Authenticated\n But Was: {role.ToString()}");
                }
                else if (role == Role.Authenticated)
                {
                    i++;
                }
            }
            
            if (i == 1)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void _GetRoles_UnauthenticatedUser_IsUnauthenticated()
        {
            var roles = _auth.GetRoles("13456789");

            foreach (var role in roles)
            {
                if (role != Role.Unauthenticated)
                {
                    Assert.Fail($"  Expected: Unauthenticated\n  But Was: {role.ToString()}");
                }
            }
            Assert.Pass();
        }
    }
}