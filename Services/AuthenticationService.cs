using System;
using System.Collections.Generic;
using System.Linq;
using AKK.Models;
using AKK.Models.Repositories;

namespace AKK.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<Member> _memberRepository;

        public AuthenticationService(IRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public string Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Username == username);

            if (member != default(Member) && member.Password == password)
            {
                if(member.Token != null) {
                    return member.Token;
                }
                member.Token = Guid.NewGuid().ToString();
                _memberRepository.Save();
                return member.Token;
            }
            return null;
        }

        public void Logout(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return;
            }
            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Token == token);

            if (member != default(Member))
            {
                member.Token = null;
                _memberRepository.Save();
            }
        }

        public bool HasRole(string token, Role role)
        {
            if (string.IsNullOrEmpty(token)) 
            {
                return false;
            }

            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Token == token);
            switch (role)
            {
                case Role.Unauthenticated:
                    return member == default(Member);
                case Role.Authenticated:
                    return member != default(Member);
                case Role.Admin:
                    return member != default(Member) && member.IsAdmin;
                default:
                    return false;
            }
        }

        public IEnumerable<Role> GetRoles(string token)
        {
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                if (HasRole(token, role))
                {
                    yield return role;
                }
            }
        }
    }
}
