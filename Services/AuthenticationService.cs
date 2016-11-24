using System;
using System.Linq;
using AKK.Classes.Models;
using AKK.Classes.Models.Repository;

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
                member.Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
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

        public bool HasRole(string token, Role missing_name)
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticated(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Token == token);

            return member != default(Member);
        }
    }
}
