using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System;
using System.Linq;

using AKK.Models;

namespace AKK.Controllers {
    [Route("api/route")]
    public class RouteController : Controller {
        MainDbContext _mainDbContext;
        public RouteController(MainDbContext mainDbContext) {
            _mainDbContext = mainDbContext;
        }

        [HttpGet]
        public JsonResult GetAll(string grade, Section section, string sort) {
            return new JsonResult(_mainDbContext.Routes);
        }
        
    }
}