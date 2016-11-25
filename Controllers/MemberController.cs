using System;
using System.Linq;
using AKK.Controllers.ApiResponses;
using AKK.Models;
using AKK.Models.Repositories;
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

        // GET: /api/member/login
        [HttpGet("login")]
        public ApiResponse<string> Login(string username, string password)
        {
            string token = _authenticator.Login(username, password);

            if (string.IsNullOrEmpty(token))
            {
                return new ApiErrorResponse<string>("Login failed");
            }
            
            return new ApiSuccessResponse<string>(token);
        }

        // GET: /api/member/
        [HttpGet]
        public ApiResponse<Member> GetMemberInfo(string token) {
            var member = _memberRepository.GetAll().FirstOrDefault(x => x.Token == token);

            if (member == null || token == null) {
                return new ApiErrorResponse<Member>("Invalid token");
            }

            return new ApiSuccessResponse<Member>(member);
        }

        // GET: /api/member/logout
        [HttpGet("logout")]
        public ApiResponse<string> Logout(string token)
        {
            _authenticator.Logout(token);

            return new ApiSuccessResponse<string>("Logout successful");
        }

        // POST: /api/member
        [HttpPost]
        public ApiResponse<string> AddMember(string username, string password, string displayName)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(displayName)) {
                return new ApiErrorResponse<string>("Failed to create user. Missing username, password or display name");
            }

            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Username == username);
            if (member != default(Member))
            {
                return new ApiErrorResponse<string>("Username is already in use");
            }

            _memberRepository.Add(new Member 
            {
                DisplayName = displayName, Username = username, Password = password
            });
            _memberRepository.Save();

            return Login(username, password);
        }
    }
}
