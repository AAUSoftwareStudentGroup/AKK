using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Newtonsoft.Json;
using AKK.Classes.Models;
using AKK.Classes.ApiResponses;

namespace AKK.Controllers {
    [Route("api/route")]
    public class RouteController : Controller {
        MainDbContext _mainDbContext;
        public RouteController(MainDbContext mainDbContext) {
            _mainDbContext = mainDbContext;
        }

        // GET: /api/route
        [HttpGet]
        public ApiResponse GetRoutes(Grades? grade, Guid? sectionId, SortOrder sortBy) {
            var routes = _mainDbContext.Routes.AsQueryable(); 
            
            if(grade != null)
                routes = routes.Where(p => p.Grade == grade);
            if(sectionId != null)
                routes = routes.Where(p => p.SectionId == sectionId);
            switch(sortBy) {
                case SortOrder.Newest:
                    routes = routes.OrderByDescending(p => p.CreatedDate);
                    break;
            }

            return new ApiSuccessResponse(routes);
        }

        // POST: /api/route
        [HttpPost]
        public ApiResponse AddRoute(string sectionName, Guid sectionId, string name, string author, uint colorOfHolds, Grades grade) {
            int num;
            var sections = _mainDbContext.Sections.AsQueryable().Where(s => s.Name == sectionName || s.SectionId == sectionId);
            if(sections.Count() == 0)
                return new ApiErrorResponse("No section with name "+sectionId);
            
            if(!int.TryParse(name, out num) || num < 0) {
                return new ApiErrorResponse("Name must be a non-negative integer");
            }

            Section section = sections.First();
            Route route = new Route() {Name=name, Author=author, CreatedDate=DateTime.Now, ColorOfHolds=colorOfHolds, Grade=grade, Section=section, SectionId=sectionId};
            section.Routes.Add(route);
            _mainDbContext.Routes.Add(route);

            if(_mainDbContext.SaveChanges() == 0)
                return new ApiErrorResponse("Failed to update database");
            return new ApiSuccessResponse(route);
        }
        
        // DELETE: /api/route
        [HttpDelete]
        public ApiResponse DeleteAllRoutes() {
            var routes = _mainDbContext.Routes.AsQueryable();
            if(routes.Count() == 0)
                return new ApiErrorResponse("No routes exsist");
            
            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(routes));

            _mainDbContext.Routes.RemoveRange(routes);
            if(_mainDbContext.SaveChanges() == 0)
                return new ApiErrorResponse("Failed to remove routes from database");
            
            return new ApiSuccessResponse(resultCopy);
        }

        // GET: /api/route/{id}
        [HttpGet("{id}")]
        public ApiResponse GetRoute(Guid id) {
            var routes = _mainDbContext.Routes.AsQueryable().Where(r => r.RouteId == id);
            if(routes.Count() == 0)
                return new ApiErrorResponse("No route exsits with id "+id);
            
            return new ApiSuccessResponse(routes);
        }

        // PATCH: /api/route/{routeId}
        [HttpPatch("{routeId}")]
        public ApiResponse UpdateRoute(Guid routeId, string sectionName, Guid sectionId, string name, string author, uint? colorOfHolds, Grades? grade) {
            int num;
            var routes = _mainDbContext.Routes.AsQueryable().Where(r => r.RouteId == routeId);
            if(routes.Count() == 0)
                return new ApiErrorResponse("No route exsits with id "+routeId);
            var route = routes.First();
            
            var sections = _mainDbContext.Sections.AsQueryable().Where(s => s.Name == sectionName || s.SectionId == sectionId);
            if(sections.Count() == 1) {
                var section = sections.First();
                var oldSection = _mainDbContext.Sections.AsQueryable().Where(s => s.SectionId == route.SectionId).First();
                oldSection.Routes.Remove(route);
                route.SectionId = sectionId;
                route.Section = section;
                section.Routes.Add(route);
            }

            if(name != null) {
                if(!int.TryParse(name, out num) || num < 0)
                    return new ApiErrorResponse("Name must be a non-negative integer");
                else
                    route.Name = name;
            }
            if(author != null)
                route.Author = author;
            if(colorOfHolds != null)
                route.ColorOfHolds = (uint)colorOfHolds;
            if(grade != null) 
                route.Grade = (Grades)grade;
            
            if(_mainDbContext.SaveChanges() == 0)
                return new ApiErrorResponse("Failed to update database");

            return new ApiSuccessResponse(route);
        }

        // DELETE: /api/route/{routeId}
        [HttpDelete("{routeId}")]
        public ApiResponse DeleteRoute(Guid routeId) {
            var routes = _mainDbContext.Routes.AsQueryable().Where(r => r.RouteId == routeId);
            if(routes.Count() == 0) {
                return new ApiErrorResponse("No route exsits with id "+routeId);
            }
            
            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(routes));
            
            _mainDbContext.Routes.RemoveRange(routes);
            if(_mainDbContext.SaveChanges() == 0) {
                return new ApiErrorResponse("Failed to remove routes with id "+routeId);
            }

            return new ApiSuccessResponse(resultCopy);
        }
    }
}