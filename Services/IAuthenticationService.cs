using System.Collections.Generic;

namespace AKK.Services
{
    public interface IAuthenticationService
    {
        string Login(string username, string password);

        void Logout(string token);

        bool HasRole(string token, Role role);
        IEnumerable<Role> GetRoles(string token);
    }
}
