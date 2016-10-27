using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public JsonResult GetAll(Grades? grade, string section, SortOrder sortBy) {
            var routes = _mainDbContext.Routes.AsQueryable(); 
            
            if(grade != null)
                routes = routes.Where(p => p.Grade == grade);
            if(section != null)
                routes = routes.Where(p => p.SectionID == section);
            if(sortBy == SortOrder.Newest)
                routes = routes.OrderByDescending(p => p.Date);

            return new JsonResult(routes);
        }
    }
}