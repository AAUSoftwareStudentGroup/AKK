using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AKK.Controllers.ApiResponses;
using AKK.Models;
using AKK.Models.Repositories;
using AKK.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace AKK.Controllers
{
    [Route("api/route")]
    public class RouteController:Controller
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
            var numRoutes = routes.Count();
            maxResults = maxResults <= 0 ? numRoutes : Math.Min(maxResults, numRoutes);

            if (gradeId != null)
            {
                routes = routes.Where(r => r.GradeId == gradeId);
            }
            if (sectionId != null)
            {
                routes = routes.Where(p => p.SectionId == sectionId);
            }

            switch (sortBy)
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
                ISearchService<Route> searcher = new RouteSearchService(routes);

                //Search for route
                routes = searcher.Search(searchStr);

                //If no routes were found.
                if (!routes.Any())
                {
                    return new ApiErrorResponse<IEnumerable<Route>>("No routes matched your search");
                }
            }

            return new ApiSuccessResponse<IEnumerable<Route>>(routes.Take(maxResults));
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

            int temp;
            if (!int.TryParse(route.Name, out temp) || temp<1)
            {
                return new ApiErrorResponse<Route>("Route number must be a positive integer");
            }

            if (route.Author == null)
            {
                return new ApiErrorResponse<Route>("An author must be specified");
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

            var routes = _routeRepository.GetAll().ToList();

            if (!routes.Any())
            {
                return new ApiErrorResponse<IEnumerable<Route>>("No routes exist");
            }

            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(
                JsonConvert.SerializeObject(routes)) as IEnumerable<Route>;

            for(int index = 0; index < routes.Count; index++)
            {
                _routeRepository.Delete(routes[index].Id);
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
            if (route == null)
            {
                return new ApiErrorResponse<Image>($"No route exists with id {id}");
            }
            var image = _imageRepository.GetAll().AsQueryable().FirstOrDefault(x => x.RouteId == id);
            if (image == null)
            {
                return new ApiErrorResponse<Image>($"No image exists for route with id {id}");
            }

            image.Holds = _holdRepository.GetAll().Where(h => h.ImageId == image.Id).ToList();

            return new ApiSuccessResponse<Image>(image);
        }

        //POST /api/route/beta
        [HttpPost("comment")]
        public async Task<ApiResponse<string>> AddComment(string token, IFormFile file, Guid id, string text) {
            if (text == null && file == null) {
                return new ApiErrorResponse<string>("You cannot add an empty comment");
            }
            if (!_authenticationService.HasRole(token, Role.Authenticated))
            {
                return new ApiErrorResponse<string>("You need to be logged in to add a comment");
            }
            var route = _routeRepository.Find(id);
            if (route == null)
            {
                return new ApiErrorResponse<string>($"No route exists with id {id}");
            }

            var member = _memberRepository.GetAll()
                                          .FirstOrDefault(m => m.Token == token);
            var comment = new Comment {Member = member, Message = text ?? ""};
            if (file != null) {
                try {
                    var fileExtension = ContentDispositionHeaderValue
                        .Parse(file.ContentDisposition)
                        .FileName
                        .Trim('"')
                        .Split('.')
                        .Last();
                    var fileName = Guid.NewGuid().ToString() + $".{fileExtension}";
                    var path = "files/" + fileName;
                    var savePath = "wwwroot/" + path;
                        using (var fileStream = System.IO.File.Create(savePath)) {
                                await file.CopyToAsync(fileStream);
                        }
                    var video = new Video {FileUrl = path};
                    comment.Video = video;
                } catch (System.OperationCanceledException) {
                    //ASP.NET bug causes this exception to be thrown, even though it is not an error
                }
            }
            
            route.Comments.Add(comment);
            System.Console.WriteLine(route.Comments.Count);
            _routeRepository.Save();
            
            return new ApiSuccessResponse<string>("success");
        }

        // PATCH: /api/route/{routeId}
        [HttpPatch("{routeId}")]
        public ApiResponse<Route> UpdateRoute(string token, Guid routeId, Route route)
        {
            if (!_authenticationService.HasRole(token, Role.Authenticated))
            {
                return new ApiErrorResponse<Route>("You need to be logged in to edit a route");
            }

            Route routeToUpdate = _routeRepository.Find(routeId);

            if(routeToUpdate == null)
            {
                return new ApiErrorResponse<Route>($"Could not find route with id {routeId}");
            }
                
            if(route == null)
            {
                return new ApiSuccessResponse<Route>(routeToUpdate);
            }
            
            routeToUpdate.ColorOfHolds = route.ColorOfHolds ?? routeToUpdate.ColorOfHolds;
            routeToUpdate.ColorOfTape = route.ColorOfTape;
            routeToUpdate.Name = route.Name ?? routeToUpdate.Name;
            routeToUpdate.Author = route.Author ?? routeToUpdate.Author;
            routeToUpdate.Note = route.Note ?? routeToUpdate.Note;
            
            if(route.Image != null)
            {
                if (_imageRepository.GetAll().Any(i => i.RouteId == routeId))
                {
                    Image img = _imageRepository.GetAll().First(i => i.RouteId == routeId);
                    IEnumerable<Hold> holds = _holdRepository.GetAll().Where(h => h.ImageId == img.Id);
                    if(holds != null && holds.Any())
                    {
                        holds.ToList().ForEach(h => _holdRepository.Delete(h.Id));
                    }
                    _holdRepository.Save();
                    _imageRepository.Delete(img.Id);
                }

                routeToUpdate.Image = route.Image;
            }

            if(route.GradeId != default(Guid))
            {
                routeToUpdate.Grade = _gradeRepository.Find(route.GradeId);
                routeToUpdate.GradeId = routeToUpdate.Grade.Id;
            }

            if(route.SectionId != default(Guid))
            {
                routeToUpdate.SectionId = route.SectionId;
            }

            if (_routeRepository.GetAll().Any(r => r.GradeId == routeToUpdate.GradeId && r.Name == routeToUpdate.Name && r.Id != routeToUpdate.Id))
            {
                return new ApiErrorResponse<Route>("A route with this grade and number already exists");
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
            if (route == null)
            {
                return new ApiErrorResponse<Route>($"No route exists with id {routeId}");
            }

            // Create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(route)) as Route;
            _routeRepository.Delete(route.Id);

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

        // POST: /api/route/{routeId}/rating
        [HttpPost("routeId/rating")]
        public ApiResponse<Route> AddRating(string token, Guid routeId, int ratingValue)
        {
            if (!_authenticationService.HasRole(token, Role.Authenticated))
            {
                return new ApiErrorResponse<Route>("You need to be logged in to add a rating to this route");
            }

            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Token == token);
            var route = _routeRepository.Find(routeId);

            //Checks if user already rated specific route
            if (route.Ratings.FirstOrDefault(r => r.Member == member) != null)
            {
                return new ApiErrorResponse<Route>($"{member.DisplayName} already rated this route");
            }

            //Checks if route is the default value, which means Find found nothing
            if (route.Equals(null))
            {
                return new ApiErrorResponse<Route>($"No route exists with id {routeId}");
            }

            if (ratingValue > 5 || ratingValue < 1)
            {
                return new ApiErrorResponse<Route>($"The given rating is not within the allowed spectrum, which is 5>=rating>=1");
            }

            var rating = new Rating
            {
                RatingValue = ratingValue,
                Member = member,
                RouteId = routeId
            };

            route.Ratings.Add(rating);

            _routeRepository.Save();

            return new ApiSuccessResponse<Route>(route);
        }

        // DELETE: /api/route/{routeId}/rating
        [HttpDelete("routeId/rating")]
        public ApiResponse<Route> DeleteRating(string token, Rating rating)
        {
            //Checks if user is logged in
            if (!_authenticationService.HasRole(token, Role.Authenticated))
            {
                return new ApiErrorResponse<Route>("You need to be logged in to delete this rating");
            }

            //Finds the member according to the token
            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Token == token);
            var route = _routeRepository.Find(rating.RouteId);

            if (route == null)
            {
                return new ApiErrorResponse<Route>($"No route exists with id {rating.RouteId}");
            }

            //If the person trying to delete is not the same as the one who created the rating, or if the member is not an admin
            if (rating.Member != member && !_authenticationService.HasRole(token, Role.Admin))
            {
                return new ApiErrorResponse<Route>($"User {member.DisplayName} is unauthenticated to delete this rating");
            }

            route.Ratings.Remove(rating);
            _routeRepository.Save();

            return new ApiSuccessResponse<Route>(route);
        }

        // PATCH: /api/route/{routeId}/rating
        [HttpPatch("routeId/rating")]
        public ApiResponse<Route> UpdateRating(string token, Rating rating, int ratingValue)
        {
            if (!_authenticationService.HasRole(token, Role.Authenticated))
            {
                return new ApiErrorResponse<Route>("You need to be logged in to change this rating");
            }

            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Token == token);
            var route = _routeRepository.Find(rating.RouteId);

            if (member != rating.Member)
            {
                return new ApiErrorResponse<Route>("Unauthenticated to change rating");
            }

            if (ratingValue > 5 || ratingValue < 1)
            {
                return new ApiErrorResponse<Route>($"The given rating is not within the allowed spectrum, which is 5>=rating>=1");
            }

            rating.RatingValue = ratingValue;
            _routeRepository.Save();

            return new ApiSuccessResponse<Route>(route);
        }
    }
}