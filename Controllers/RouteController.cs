using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AKK.Classes.Models;
using AKK.Classes.ApiResponses;

namespace AKK.Controllers {
    [Route("api/route")]
    public class RouteController : Controller {
        MainDbContext db;
        public RouteController(MainDbContext mainDbContext) {
            db = mainDbContext;
        }

        // GET: /api/route
        [HttpGet]
        public ApiResponse GetRoutes(int? grade, Guid? sectionId, SortOrder sortBy) {
            var routes = db.Routes.Include(r => r.Section).Include(r => r.Grade).AsQueryable();
            
            if(grade != null)
                routes = routes.Where(r => r.Grade.Difficulty == grade);
            if(sectionId != null)
                routes = routes.Where(p => p.SectionId == sectionId);
            switch(sortBy) {
                case SortOrder.Newest:
                    routes = routes.OrderByDescending(p => p.CreatedDate);
                    break;
                case SortOrder.Oldest:
                    routes = routes.OrderBy(p => p.CreatedDate);
                    break;
                case SortOrder.Author:
                    routes = routes.OrderBy(p => p.Author);
                    break;
                case SortOrder.Grading:
                    routes = routes.OrderBy(p => p.Grade.Difficulty);
                    break;
            }
            
            return new ApiSuccessResponse(Mappings.Mapper.Map<IEnumerable<Route>, IEnumerable<RouteDataTransferObject>>(routes));
        }

        // POST: /api/route
        [HttpPost]
        public ApiResponse AddRoute(Route route, string sectionName) {
            if(route.Author == null) return new ApiErrorResponse("An author must be specified");
            if(route.ColorOfHolds == null) return new ApiErrorResponse("A hold color must be specified");
            if(route.Grade == null) return new ApiErrorResponse("A grade must be specified");
            if(route.Name == null) return new ApiErrorResponse("A route number must be specified");

            var sections = db.Sections.AsQueryable();
            if(route.SectionId != null && route.SectionId != default(Guid)) {
                sections = sections.Where(s => s.SectionId == route.SectionId);
                if(sections.Count() == 0)
                    return new ApiErrorResponse("No section with id "+route.SectionId);
            }
            else if(sectionName != null) {
                sections = sections.Where(s => s.Name == sectionName);
                if(sections.Count() == 0)
                    return new ApiErrorResponse("No section with name "+sectionName);
            }
            else {
                return new ApiErrorResponse("A section must be specified");
            }
            
            var grades = db.Grades.Where(g => g.Difficulty == route.Grade.Difficulty);
            if(grades.Count() != 1)
                return new ApiErrorResponse("No grade with given difficulty");
            route.Grade = grades.First();
            

            if(db.Routes.AsQueryable().Where(r => r.Grade.Difficulty == route.Grade.Difficulty && r.Name == route.Name).Count() > 0)
                return new ApiErrorResponse("A route with this grade and number already exists");

            Section section = sections.First();
            route.CreatedDate = DateTime.Now; 
            route.Section = section; 
            route.SectionId=section.SectionId;
            
            section.Routes.Add(route);
            db.Routes.Add(route);

            if(db.SaveChanges() == 0)
                return new ApiErrorResponse("Failed to update database");
            return new ApiSuccessResponse(Mappings.Mapper.Map<Route, RouteDataTransferObject>(route));
        }
        
        // DELETE: /api/route
        [HttpDelete]
        public ApiResponse DeleteAllRoutes() {
            var routes = db.Routes.Include(r => r.Section).Include(r => r.Grade).AsQueryable();
            if(routes.Count() == 0)
                return new ApiErrorResponse("No routes exist");
            
            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(
                JsonConvert.SerializeObject(
                    Mappings.Mapper.Map<IEnumerable<Route>, IEnumerable<RouteDataTransferObject>>(
                        routes
                    )
                )
            );

            db.Routes.RemoveRange(routes);
            if(db.SaveChanges() == 0)
                return new ApiErrorResponse("Failed to remove routes from database");
            
            return new ApiSuccessResponse(resultCopy);
        }

        // GET: /api/route/{id}
        [HttpGet("{id}")]
        public ApiResponse GetRoute(Guid id) {
            var routes = db.Routes.Include(r => r.Section).Include(r => r.Grade).AsQueryable().Where(r => r.RouteId == id);
            if(routes.Count() == 0)
                return new ApiErrorResponse("No route exists with id "+id);
            
            return new ApiSuccessResponse(Mappings.Mapper.Map<Route, RouteDataTransferObject>(routes.First()));
        }

        // PATCH: /api/route/{routeId}
        [HttpPatch("{routeId}")]
        public ApiResponse UpdateRoute(Guid routeId, string sectionName, Route route) {
            Route oldRoute = null;
            bool changed = false;
            var routes = db.Routes
                .Include(r => r.Section)
                .Include(r => r.Grade).AsQueryable().Where(r => r.RouteId == routeId);

            if(routes.Count() != 1)
                return new ApiErrorResponse("Route does not exist");
            oldRoute = routes.First();
            
            if(route.Name != null && route.Name != oldRoute.Name) { oldRoute.Name = route.Name; changed = true;}
            if(route.Author != null) oldRoute.Author = route.Author;
            if(route.ColorOfHolds != null) oldRoute.ColorOfHolds = route.ColorOfHolds;
            if(route.ColorOfTape != null) oldRoute.ColorOfTape = route.ColorOfTape;
            if(route.Grade != null) {
                var grades = db.Grades.Where(g => g.Difficulty == route.Grade.Difficulty);
                if(grades.Count() != 1)
                    return new ApiErrorResponse("No grade with given difficulty");
                
                if(route.Grade.Difficulty != oldRoute.Grade.Difficulty)
                    changed = true;
                oldRoute.Grade = grades.First();
            }

            if(changed) {
                var routeswithgradeAndName = db.Routes.Where(r => r.Grade.Difficulty == oldRoute.Grade.Difficulty).Where(r => r.Name == oldRoute.Name);
                if(routeswithgradeAndName.Count() > 0)
                    return new ApiErrorResponse("A route with that grade and name already exist");
            }

            if(route.SectionId != default(Guid)) {
                var section = db.Sections.Where(s => s.SectionId == route.SectionId);
                if(section.Count() != 1)
                    return new ApiErrorResponse("No section with id " + route.RouteId);

                oldRoute.Section = section.First();
            }
            else if(sectionName != null) {
                var section = db.Sections.Where(s => s.Name == sectionName);
                if(section.Count() != 1)
                    return new ApiErrorResponse("No section with name " + sectionName);

                oldRoute.Section = section.First();
            }

            db.SaveChanges();

            return new ApiSuccessResponse(Mappings.Mapper.Map<Route, RouteDataTransferObject>(oldRoute));
        }

        // DELETE: /api/route/{routeId}
        [HttpDelete("{routeId}")]
        public ApiResponse DeleteRoute(Guid routeId) {
            var routes = db.Routes.Include(r => r.Section).Include(r => r.Grade).AsQueryable().Where(r => r.RouteId == routeId);
            if(routes.Count() == 0) {
                return new ApiErrorResponse("No route exists with id "+routeId);
            }
            
            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(
                JsonConvert.SerializeObject(
                    Mappings.Mapper.Map<Route, RouteDataTransferObject>(
                        routes.First()
                    )
                )
            );
            
            db.Routes.RemoveRange(routes);
            if(db.SaveChanges() == 0) {
                return new ApiErrorResponse("Failed to remove routes with id "+routeId);
            }

            return new ApiSuccessResponse(resultCopy);
        }
    }
}