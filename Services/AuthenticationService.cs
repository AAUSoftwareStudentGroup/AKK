using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

            if (member != default(Member) && TestPassword(password, member.Password))
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

        public void ChangeRole(Guid id, Role role)
        {
            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Id == id);

            if (member == default(Member))
            {
                return;
            }

            switch (role)
            {
                case Role.Admin:
                    member.IsAdmin = true;
                    break;
                case Role.Authenticated:
                    member.IsAdmin = false;
                    break;
            }
            _memberRepository.Save();
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

        private byte[] Hash(string value, string salt)
        {
            return Hash(Encoding.UTF8.GetBytes(value), Encoding.UTF8.GetBytes(salt));
        }

        private byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();
            return SHA256.Create().ComputeHash(saltedValue);
        }

        private string GenerateSalt() {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public string HashPassword(string password)
        {
            string salt = GenerateSalt();
            string hash = Convert.ToBase64String(Hash(password, salt));
            return salt+":"+hash;
        }
        
        public bool TestPassword(string testPass, string hashedPass) {
            string salt = (hashedPass.Substring(0, 24));
            string hash = (hashedPass.Substring(25));
            string confirmPass = Convert.ToBase64String( Hash(testPass, salt) );
            return string.Compare(confirmPass, hash) == 0;
        } 
    }
}
