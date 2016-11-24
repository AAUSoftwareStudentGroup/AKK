using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using AKK.Classes;
using AKK.Services;

namespace AKK.Tests
{
    [TestFixture]
    public class TestAuthentication
    {
        [Test]
        public void _MemberWithUsernameTannerPasswordHelland_IsAuthenticated()
        {
            var Authenticator = new TestAuthenticationService();
            var token = Authenticator.Login("Tanner", "Helland");

            Assert.AreEqual(true, Authenticator.HasRole(token, Role.Authenticated));
        }

        [Test]
        public void _LoggingOutRemovesToken_TokenIsRemoved()
        {
            var Authenticator = new TestAuthenticationService();
            var token = Authenticator.Login("Tanner", "Helland");
            Authenticator.Logout(token);

            Assert.AreEqual(false, Authenticator.HasRole(token, Role.Authenticated));
        }

        [Test]
        public void _LoggingInAsSetter_TokenIsOnlySetterAndNotAdmin()
        {
            var Authenticator = new TestAuthenticationService();
            var token = Authenticator.Login("Tanner", "Helland");

            Assert.AreEqual(true, Authenticator.HasRole(token, Role.Authenticated));
            Assert.AreEqual(false, Authenticator.HasRole(token, Role.Admin));
        }
    }
}