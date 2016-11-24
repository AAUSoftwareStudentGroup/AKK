using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKK.Classes.ApiResponses;
using AKK.Classes.Models;
using AKK.Classes.Models.Repository;
using AKK.Services;
using Microsoft.AspNetCore.Mvc;

namespace AKK.Controllers
{
    [Route("api/member")]
    public class MemberController : Controller
    {
        private readonly IAuthenticationService _authenticator;
        private readonly IRepository<Member> _memberRepository;

        public MemberController(IRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
            _authenticator = new AuthenticationService(_memberRepository);
    }

        // POST: /api/member
        [HttpPost]
        public ApiResponse<string> Login(string username, string password)
        {
            string token = _authenticator.Login(username, password);

            if (string.IsNullOrEmpty(token))
            {
                return new ApiErrorResponse<string>("Login failed");
            }
            
            return new ApiSuccessResponse<string>(token);
        }

        // POST: /api/member
        [HttpPost]
        public ApiResponse<string> Logout(string token)
        {
            _authenticator.Logout(token);

            return new ApiSuccessResponse<string>("Logout successful");
        }
    }
}
