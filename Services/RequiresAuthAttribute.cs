using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace AKK.Services
{
    public class RequiresAuthAttribute : ActionFilterAttribute 
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string token = null;
            if(filterContext.HttpContext.Request.Cookies.TryGetValue("token", out token))
            {
                // check token
                Console.WriteLine(token);
                
                // if valid
                return;
            }

            // Console.WriteLine(filterContext.HttpContext.Request.Path);  // /sections
            // Console.WriteLine(filterContext.HttpContext.Request.QueryString); // ?ingenTest=udenHest&a2=b3

            filterContext.HttpContext.Response.Redirect("login?target="+filterContext.HttpContext.Request.Path);
        }
    }
}