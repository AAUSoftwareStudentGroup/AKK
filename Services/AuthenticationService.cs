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
            //Don't try to login if no username or password has been inputted
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Username.ToLower() == username.ToLower());

            //Login and assign a token to the member, if the found member's password is the same as the password inputted
            if (member != default(Member) && TestPassword(password, member.Password))
            {
                //Allows multiple sessions for a member
                if(member.Token != null) {
                    return member.Token;
                }
                member.Token = Guid.NewGuid().ToString();
                _memberRepository.Save();
                return member.Token;
            }
            else
                return null;
        }

        public void Logout(string token)
        {
            //Nothing to do if no token has been inputted
            if (string.IsNullOrEmpty(token))
            {
                return;
            }
            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Token == token);

            //Remove the token from the member who logs out
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
                //If no found member is found, then this case will return true, else it'll return false
                case Role.Unauthenticated:
                    return member == default(Member);
                //If a member is not found, then this case will return false, and the member is therefore not Authenticated
                case Role.Authenticated:
                    return member != default(Member);
                //This case will only return true, if a member is found, and the property IsAdmin, is true
                case Role.Admin:
                    return member != default(Member) && member.IsAdmin;
                default:
                    return false;
            }
        }

        public void ChangeRole(Guid id, Role role)
        {
            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Id == id);

            //Can't change the role of a member that doesn't exist in the database, so skip the rest of the method
            if (member == default(Member))
            {
                return;
            }

            //Change the member to the selected role, by changing the IsAdmin property of the found member
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

        //Makes use of the HasRole method to return all roles a member with the inputted token, has
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

        //Uses the Hash method which takes two byte arrays as its input parameters as a helper method, hash a given value
        private byte[] Hash(string value, string salt)
        {
            //Calls the helper method with the bytes of a UTF8 representation of the value, and the salt
            return Hash(Encoding.UTF8.GetBytes(value), Encoding.UTF8.GetBytes(salt));
        }

        //Concatenate the value with the salt to produce a salted and hashed value
        private byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();
            return SHA256.Create().ComputeHash(saltedValue);
        }

        //Generates a new Guid, then converts it to base64, and uses the base64 representation of the Guid, as the salt for the password
        private string GenerateSalt() {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        //Hashes and salts the password for added security
        public string HashPassword(string password)
        {
            string salt = GenerateSalt();
            string hash = Convert.ToBase64String(Hash(password, salt));
            return salt+":"+hash;
        }
        
        //Hashes and salts the testPass, then compares the two to determine if the two passwords are the same. Return true if they are, else false
        public bool TestPassword(string testPass, string hashedPass) {
            string salt = (hashedPass.Substring(0, 24));
            string hash = (hashedPass.Substring(25));
            string confirmPass = Convert.ToBase64String( Hash(testPass, salt) );
            return string.Compare(confirmPass, hash) == 0;
        } 
    }
}
