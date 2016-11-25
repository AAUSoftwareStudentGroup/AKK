using Microsoft.AspNetCore.Mvc.Filters;
using AKK.Controllers;
using System.Text.Encodings.Web;

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
            var context = filterContext.HttpContext;
            if(context.Request.Cookies.TryGetValue("token", out token))
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

            context.Response.Redirect("login?target="+UrlEncoder.Default.Encode(context.Request.Path+context.Request.QueryString));
        }
    }
}