using Microsoft.AspNetCore.Mvc.Filters;
using AKK.Controllers;

namespace AKK.Services
{
    public class RequiresAuthAttribute : ActionFilterAttribute 
    {
        Role[] _roles;
        public RequiresAuthAttribute(params Role[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string token = null;
            if(filterContext.HttpContext.Request.Cookies.TryGetValue("token", out token))
            {
                // check token
                if(filterContext.Controller is ViewController) {
                    ViewController v = (filterContext.Controller as ViewController);
                    foreach(Role r in _roles)
                    {
                        if(v.AuthenticationService.HasRole(token, r))
                            return;
                    }
                }
            }

            // Console.WriteLine(filterContext.HttpContext.Request.Path);  // /sections
            // Console.WriteLine(filterContext.HttpContext.Request.QueryString); // ?ingenTest=udenHest&a2=b3

            filterContext.HttpContext.Response.Redirect("login?target="+filterContext.HttpContext.Request.Path);
        }
    }
}