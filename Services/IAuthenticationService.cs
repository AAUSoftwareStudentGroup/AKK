using System.Collections.Generic;
using System;

namespace AKK.Services
{
    public interface IAuthenticationService
    {
        string Login(string username, string password);

        void Logout(string token);

        bool HasRole(string token, Role role);

        void ChangeRole(Guid id, Role role);

        IEnumerable<Role> GetRoles(string token);

        string HashPassword(string password);

        bool TestPassword(string password, string hashedPass);
    }
}
