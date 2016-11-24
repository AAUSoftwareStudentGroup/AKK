using System;
using AKK.Classes.Models.Repository;
using AKK.Classes.Services;

namespace AKK.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        public string Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout(string token)
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticated(string token)
        {
            throw new NotImplementedException();
        }
    }
}
