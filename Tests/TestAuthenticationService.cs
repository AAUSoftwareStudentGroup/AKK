using System;
using System.Collections.Generic;
using AKK.Models;
using AKK.Services;

namespace AKK.Tests
{
    public class TestAuthenticationService : IAuthenticationService
    {
        public List<Member> _members = new List<Member>();

        public TestAuthenticationService() {
            _members.Add(new Member{Id = new Guid(), DisplayName = "TannerHelland", Username = "Tanner", Password = "Helland", IsAdmin = false, Token = "TannerHelland"});
            _members.Add(new Member{Id = new Guid(), DisplayName = "Morten Rask", Username = "Morten", Password = "Rask", IsAdmin = true, Token = "123"});
        }
        public string Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            foreach (var member in _members)
            {
                if (member.Username == username && member.Password == password) {
                    member.Token = member.Username + member.Password;
                    return member.Token;
                }
            }
            return null;
        }

        public void Logout(string token)
        {
            foreach (var member in _members)
            {
                if (member.Token == token) {
                    member.Token = null;
                }
            }
        }

        public bool HasRole(string token, Role role)
        {
            Member member = null;
            foreach (var memb in _members)
            {
                if (memb.Token == token) {
                    member = memb;
                }
            }

            if (member == null && role != Role.Unauthenticated) {
                return false;
            }

            switch (role)
            {
                case Role.Unauthenticated:
                    if (member.Token == token) {
                        return false;
                    } else {
                        return true;
                    }
                case Role.Authenticated:
                    return true;
                case Role.Admin:
                    if (member.IsAdmin) {
                        return true;
                    } else {
                        return false;
                    }
                default:
                    return false;
            }
        }

        public IEnumerable<Role> GetRoles(string token)
        {
            throw new NotImplementedException();
        }
    }
}
