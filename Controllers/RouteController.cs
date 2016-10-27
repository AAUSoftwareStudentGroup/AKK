using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AKK.Classes.Models;
using AKK.Classes.ApiResponses;

namespace AKK.Controllers {
    [Route("api/route")]
    public class RouteController : Controller {
        MainDbContext _mainDbContext;
        public RouteController(MainDbContext mainDbContext) {
            _mainDbContext = mainDbContext;
        }

        [HttpGet]
        public ApiResponse GetAll(Grades? grade, string section, SortOrder sortBy) {
            var routes = _mainDbContext.Routes.AsQueryable(); 
            
            if(grade != null)
                routes = routes.Where(p => p.Grade == grade);
            if(section != null)
                routes = routes.Where(p => p.SectionID == section);
            if(sortBy == SortOrder.Newest)
                routes = routes.OrderByDescending(p => p.Date);

            return new ApiSuccessResponse(routes);
        }
    }
}