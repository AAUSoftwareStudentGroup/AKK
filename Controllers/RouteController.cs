using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AKK.Classes.Models;
using AKK.Classes.ApiResponses;
using AKK.Classes.Models.Repository;

namespace AKK.Controllers {
    [Route("api/route")]
    public class RouteController : Controller
    {

        private IRepository<Route> _routeRepository;
        private IRepository<Section> _sectionrepository;
        private IRepository<Grade> _gradeRepository;
        public RouteController(IRepository<Route> routeRepository, IRepository<Section> sectionrepository, IRepository<Grade> gradeRepository ) {
            _routeRepository = routeRepository;
            _sectionrepository = sectionrepository;
            _gradeRepository = gradeRepository;
        }

        // GET: /api/route
        [HttpGet]
        public ApiResponse GetRoutes(int? grade, Guid? sectionId, SortOrder sortBy)
        {
            var routes = _routeRepository.GetAll();            
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

            var sections = _sectionrepository.GetAll();
            if(route.SectionId != null && route.SectionId != default(Guid)) {
                sections = sections.Where(s => s.Id == route.SectionId);
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

            var grades = _gradeRepository.GetAll().Where(g => g.Difficulty == route.Grade.Difficulty);
            if(grades.Count() != 1)
                return new ApiErrorResponse("No grade with given difficulty");
            route.Grade = grades.First();
            

            if(_routeRepository.GetAll().Any(r => r.Grade.Difficulty == route.Grade.Difficulty && r.Name == route.Name))
                return new ApiErrorResponse("A route with this grade and number already exists");

            Section section = sections.First();
            route.CreatedDate = DateTime.Now; 
            route.Section = section; 
            route.SectionId=section.Id;
            
            section.Routes.Add(route);
            _routeRepository.Add(route);

            try
            {
                _gradeRepository.Save();
                return new ApiSuccessResponse(Mappings.Mapper.Map<Route, RouteDataTransferObject>(route));

            }
            catch
            {
                return new ApiErrorResponse("Failed to update database");
            }

        }
        
        // DELETE: /api/route
        [HttpDelete]
        public ApiResponse DeleteAllRoutes()
        {
            var routes = _routeRepository.GetAll();
            if(!routes.Any())
                return new ApiErrorResponse("No routes exist");
            
            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(
                JsonConvert.SerializeObject(
                    Mappings.Mapper.Map<IEnumerable<Route>, IEnumerable<RouteDataTransferObject>>(
                        routes
                    )
                )
            );

            foreach (var route in routes)
            {
                _routeRepository.Delete(route);
            }

            try
            {
                _routeRepository.Save();
                return new ApiSuccessResponse(resultCopy);

            }
            catch
            {
                return new ApiErrorResponse("Failed to remove routes from database");
            }
        }

        // GET: /api/route/{id}
        [HttpGet("{id}")]
        public ApiResponse GetRoute(Guid id)
        {
            var route = _routeRepository.Find(id);
            if(route == null)
                return new ApiErrorResponse("No route exists with id "+id);
            
            return new ApiSuccessResponse(Mappings.Mapper.Map<Route, RouteDataTransferObject>(route));
        }

        // PATCH: /api/route/{routeId}
        [HttpPatch("{routeId}")]
        public ApiResponse UpdateRoute(Guid routeId, string sectionName, Route route) {
            Route oldRoute = null;
            bool changed = false;
            var routes = _routeRepository.GetAll().Where(r => r.Id == routeId);

            if(routes.Count() != 1)
                return new ApiErrorResponse("Route does not exist");
            oldRoute = routes.First();
            
            if(route.Name != null && route.Name != oldRoute.Name) { oldRoute.Name = route.Name; changed = true;}
            if(route.Author != null) oldRoute.Author = route.Author;
            if(route.ColorOfHolds != null) oldRoute.ColorOfHolds = route.ColorOfHolds;
            oldRoute.ColorOfTape = route.ColorOfTape;
            if(route.Grade != null)
            {
                var grades = _gradeRepository.GetAll().Where(g => g.Difficulty == route.Grade.Difficulty);
                if(grades.Count() != 1)
                    return new ApiErrorResponse("No grade with given difficulty");
                
                if(route.Grade.Difficulty != oldRoute.Grade.Difficulty)
                    changed = true;
                oldRoute.Grade = grades.First();
            }

            if(changed)
            {
                var routesWithGradeAndName =
                    _routeRepository.GetAll()
                        .Where(r => r.Grade.Difficulty == oldRoute.Grade.Difficulty
                        && r.Name == oldRoute.Name);

                if(routesWithGradeAndName.Any()) {
                    return new ApiErrorResponse("A route with that grade and name already exist");
                }
            }

            if(route.SectionId != default(Guid))
            {
                var section = _sectionrepository.GetAll().Where(s => s.Id == route.SectionId);
                if(section.Count() != 1)
                    return new ApiErrorResponse("No section with id " + route.Id);

                oldRoute.Section = section.First();
            }
            else if(sectionName != null)
            {
                var section = _sectionrepository.GetAll().Where(s => s.Name == sectionName);
                if(section.Count() != 1)
                    return new ApiErrorResponse("No section with name " + sectionName);

                oldRoute.Section = section.First();
            }

            try
            {
                _sectionrepository.Save();
                return new ApiSuccessResponse(Mappings.Mapper.Map<Route, RouteDataTransferObject>(oldRoute));
            }
            catch
            {
                return new ApiErrorResponse("Failed to update database");
            }
        }

        // DELETE: /api/route/{routeId}
        [HttpDelete("{routeId}")]
        public ApiResponse DeleteRoute(Guid routeId)
        {
            var route = _routeRepository.Find(routeId);
            if(route == null) {
                return new ApiErrorResponse("No route exists with id "+routeId);
            }
            
            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(
                JsonConvert.SerializeObject(
                    Mappings.Mapper.Map<Route, RouteDataTransferObject>(
                        route
                    )
                )
            );

            _routeRepository.Delete(route);

            try
            {
                _routeRepository.Save();
                return new ApiSuccessResponse(resultCopy);

            }
            catch
            {
                return new ApiErrorResponse("Failed to remove routes with id " + routeId);
            }
        }
    }
}