using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AKK.Controllers.ApiResponses;
using AKK.Models;
using AKK.Models.Repositories;
using AKK.Services;

namespace AKK.Controllers {
    [Route("api/route")]
    public class RouteController : Controller
    {

        private readonly IRepository<Route> _routeRepository;
        private readonly IRepository<Section> _sectionRepository;
        private readonly IRepository<Grade> _gradeRepository;
        private readonly IRepository<Image> _imageRepository;
        private readonly IRepository<Hold> _holdRepository;
        private readonly IRepository<Member> _memberRepository;
        private readonly IAuthenticationService _authenticationService;
        public RouteController(IRepository<Route> routeRepository, IRepository<Section> sectionRepository, IRepository<Grade> gradeRepository, IRepository<Image> imageRepository, IRepository<Hold> holdRepository, IRepository<Member> memberRepository, IAuthenticationService authenticationService) 
        {
            _routeRepository = routeRepository;
            _sectionRepository = sectionRepository;
            _gradeRepository = gradeRepository;
            _imageRepository = imageRepository;
            _holdRepository = holdRepository;
            _memberRepository = memberRepository;
            _authenticationService = authenticationService;
        }

        // GET: /api/route
        [HttpGet]
        public ApiResponse<IEnumerable<Route>> GetRoutes(Guid? gradeId, Guid? sectionId, string searchStr, int maxResults, SortOrder sortBy)
        {
            var routes = _routeRepository.GetAll();

            if (gradeId != null)
            {
                routes = routes.Where(r => r.GradeId == gradeId);
            }
            if (sectionId != null)
            {
                routes = routes.Where(p => p.SectionId == sectionId);
            }

            switch(sortBy) 
            {
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

            if (!string.IsNullOrEmpty(searchStr))
            {
                //Initialize a RouteSearcher
                var searcher = new RouteSearcher(routes, maxResults);

                //Search for route
                routes = searcher.Search(searchStr);

                //If no routes were found.
                if (!routes.Any()) {
                    return new ApiErrorResponse<IEnumerable<Route>>("No routes matched your search");
                }
            }
            return new ApiSuccessResponse<IEnumerable<Route>>(routes);
        }

        // POST: /api/route
        [HttpPost]
        public ApiResponse<Route> AddRoute(string token, Route route) 
        {
            route.Member = _memberRepository.GetAll().FirstOrDefault(x => x.Token == token);

            if (!_authenticationService.HasRole(token, Role.Authenticated))
            {
                return new ApiErrorResponse<Route>("You need to be logged in to create a new route");
            }

            if (route.ColorOfHolds == null)
            {
                return new ApiErrorResponse<Route>("A hold color must be specified");
            }

            if (route.Name == null)
            {
                return new ApiErrorResponse<Route>("A route number must be specified");
            }

            if(route.SectionId != default(Guid))     
            {
                if (_sectionRepository.Find(route.SectionId) == null)
                {
                    return new ApiErrorResponse<Route>($"No section with id {route.SectionId}");
                }
            }
            else
            {
                return new ApiErrorResponse<Route>("A section must be specified");
            }
                
            if(route.GradeId != default(Guid))     
            {
                if (_gradeRepository.Find(route.GradeId) == null)
                {
                    return new ApiErrorResponse<Route>($"No grade with id {route.GradeId}");
                }
            }
            else
            {
                return new ApiErrorResponse<Route>("A grade must be specified");
            }

            if (_routeRepository.GetAll().Any(r => r.GradeId == route.GradeId && r.Name == route.Name))
            {
                return new ApiErrorResponse<Route>("A route with this grade and number already exists");
            }

            route.CreatedDate = DateTime.Now; 
            _routeRepository.Add(route);

            try
            {
                _routeRepository.Save();
                return new ApiSuccessResponse<Route>(route);
            }
            catch
            {
                return new ApiErrorResponse<Route>("Failed to update database");
            }
        }
        
        // DELETE: /api/route
        [HttpDelete]
        public ApiResponse<IEnumerable<Route>> DeleteAllRoutes(string token)
        {
            if (!_authenticationService.HasRole(token, Role.Admin))
            {
                return new ApiErrorResponse<IEnumerable<Route>>("You need to be logged in as an administrator to delete all routes");
            }
            var routes = _routeRepository.GetAll();
            if (!routes.Any())
            {
                return new ApiErrorResponse<IEnumerable<Route>>("No routes exist");
            }
                
            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(
                JsonConvert.SerializeObject(routes)) as IEnumerable<Route>;

            foreach (var route in routes)
            {
                _routeRepository.Delete(route);
            }

            try
            {
                _routeRepository.Save();
                return new ApiSuccessResponse<IEnumerable<Route>>(resultCopy);
            }
            catch
            {
                return new ApiErrorResponse<IEnumerable<Route>>("Failed to remove routes from database");
            }
        }

        // GET: /api/route/{id}
        [HttpGet("{id}")]
        public ApiResponse<Route> GetRoute(Guid id) 
        {
            var route = _routeRepository.Find(id);
            if (route == null)
            {
                return new ApiErrorResponse<Route>($"No route exists with id {id}");
            }

            return new ApiSuccessResponse<Route>(route);
        }

        // GET: /api/route/{id}/image
        [HttpGet("{id}/image")]
        public ApiResponse<Image> GetImage(Guid id)
        {
            var route = _routeRepository.Find(id);
            if (route == null) {
                return new ApiErrorResponse<Image>($"No route exists with id {id}");
            }
            var image = _imageRepository.GetAll().AsQueryable().FirstOrDefault(x => x.RouteId == id);
            if (image == null) {
                return new ApiErrorResponse<Image>($"No image exists for route with id {id}");
            }

            image.Holds = _holdRepository.GetAll().Where(h => h.ImageId == image.Id).ToList();

            return new ApiSuccessResponse<Image>(image);
        }

        // PATCH: /api/route/{routeId}
        [HttpPatch("{routeId}")]
        public ApiResponse<Route> UpdateRoute(string token, Guid routeId, Route route)
        {
            if(!_authenticationService.HasRole(token, Role.Authenticated))
            {
                return new ApiErrorResponse<Route>("You need to be logged in to edit a route");
            }
            Route routeToUpdate = _routeRepository.Find(routeId);
            routeToUpdate.ColorOfHolds = route.ColorOfHolds ?? routeToUpdate.ColorOfHolds;
            routeToUpdate.ColorOfTape = route.ColorOfTape ?? routeToUpdate.ColorOfTape;
            routeToUpdate.Name = route.Name ?? routeToUpdate.Name;
            if(route.Image != null)
            {
                if(_imageRepository.GetAll().Any(i => i.RouteId == routeId)) {
                    Image img = _imageRepository.GetAll().First(i => i.RouteId == routeId);
                    _imageRepository.Delete(img);
                }

                routeToUpdate.Image = route.Image;
            }
            if(route.GradeId != default(Guid))
            {
                routeToUpdate.Grade = _gradeRepository.Find(route.GradeId);
            }
            if(route.SectionId != default(Guid))
            {
                routeToUpdate.SectionId = route.SectionId;
            }
            try
            {
                _routeRepository.Save();
                return new ApiSuccessResponse<Route>(_routeRepository.Find(routeId));
            }
            catch
            {
                return new ApiErrorResponse<Route>("Could not update route");    
            }
        }

        // DELETE: /api/route/{routeId}
        [HttpDelete("{routeId}")]
        public ApiResponse<Route> DeleteRoute(string token, Guid routeId)
        {
            if (!_authenticationService.HasRole(token, Role.Authenticated)) 
            {
                return new ApiErrorResponse<Route>("You need to be logged in to delete a route");
            }

            var route = _routeRepository.Find(routeId);
            if(route == null) 
            {
                return new ApiErrorResponse<Route>($"No route exists with id {routeId}");
            }
            
            // Create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(route)) as Route;
            _routeRepository.Delete(route);

            try
            {
                _routeRepository.Save();
                return new ApiSuccessResponse<Route>(resultCopy);

            }
            catch
            {
                return new ApiErrorResponse<Route>($"Failed to remove routes with id {routeId}");
            }
        }
    }
}