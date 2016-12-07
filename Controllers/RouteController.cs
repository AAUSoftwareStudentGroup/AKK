using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IRepository<HoldColor> _holdColorRepository;
        private readonly IRepository<Member> _memberRepository;
        private readonly IAuthenticationService _authenticationService;

        public RouteController(IRepository<Route> routeRepository, IRepository<Section> sectionRepository, IRepository<Grade> gradeRepository, IRepository<Image> imageRepository, IRepository<Hold> holdRepository, IRepository<HoldColor> holdColorRepository, IRepository<Member> memberRepository, IAuthenticationService authenticationService)
        { 
            _routeRepository = routeRepository;
            _sectionRepository = sectionRepository;
            _gradeRepository = gradeRepository;
            _imageRepository = imageRepository;
            _holdRepository = holdRepository;
            _holdColorRepository = holdColorRepository;
            _memberRepository = memberRepository;
            _authenticationService = authenticationService;
        }

        // GET: /api/route
        [HttpGet]
        public ApiResponse<IEnumerable<Route>> GetRoutes(Guid? gradeId, Guid? sectionId, string searchStr, int maxResults, SortOrder sortBy)
        {
            //Get all routes from the repository and assign a new value to maxResults which determines how many routes to return to the caller
            //Return the inputted value given to maxResults if that value is smaller than the amount of routes in the repository
            //Else, return all routes in the repository
            var routes = _routeRepository.GetAll();
            var numRoutes = routes.Count();
            maxResults = maxResults <= 0 ? numRoutes : Math.Min(maxResults, numRoutes);

            //Filters the found routes based on the grade and/or section, if the values aren't null
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
                case SortOrder.Rating:
                    routes = routes.OrderByDescending(p => p.AverageRating);
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
            //Return the amount of routes asked for if that amount exists, with the given sort-order and with the applied filters
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
                return new ApiErrorResponse<Route>("A valid hold color must be specified");
            } 
 
            if(!_holdColorRepository.GetAll().Any(r => r.ColorOfHolds.R == route.ColorOfHolds.R &&
                                                       r.ColorOfHolds.G == route.ColorOfHolds.G &&
                                                       r.ColorOfHolds.B == route.ColorOfHolds.B
                                                       ))
            {
                return new ApiErrorResponse<Route>("A valid hold color must be specified");
            }

            if (route.ColorOfTape != null) {
                if(!_holdColorRepository.GetAll().Any(r => r.ColorOfHolds.R == route.ColorOfTape.R &&
                                                      r.ColorOfHolds.G == route.ColorOfTape.G &&
                                                      r.ColorOfHolds.B == route.ColorOfTape.B &&
                                                      r.ColorOfHolds.A == route.ColorOfTape.A
                                                      ))
                    return new ApiErrorResponse<Route>("The specified tape color doesn't exist. Choose a valid one, or none at all");
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

            //Add the route to the repository if the member is authorised to do so, and only if all the required information is given
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
            //Create a new comment and assign a member to it, with the inputted text if the text-field isn't empty
            var comment = new Comment {Member = member, Message = text ?? ""};

            //Create a new Video object if the file extension is supported, then saves it to wwwroot/files/{fileName}
            if (file != null) {
                try {
                    var fileExtension = ContentDispositionHeaderValue
                        .Parse(file.ContentDisposition)
                        .FileName
                        .Trim('"')
                        .Split('.')
                        .Last()
                        .ToLower();
                    if (fileExtension != "mp4" && fileExtension != "webm" && fileExtension != "ogg") {
                        return new ApiErrorResponse<string>("File is not a valid video file");
                    }
                    var fileName = Guid.NewGuid().ToString() + $".{fileExtension}";
                    var path = "files/" + fileName;
                    var savePath = "wwwroot/" + path;
                        using (var fileStream = System.IO.File.Create(savePath)) {
                                await file.CopyToAsync(fileStream);
                        }
                    var video = new Video {FileUrl = path, FilePath = savePath};
                    //Adds the Video to the created Comment object
                    comment.Video = video;
                } catch (System.OperationCanceledException) {
                    //ASP.NET bug causes this exception to be thrown, even though it is not an error
                }
            }
            
            //Adds the comment to the route
            route.Comments.Add(comment);
            _routeRepository.Save();
            
            return new ApiSuccessResponse<string>("success");
        }

        [HttpPost("comment/remove")]
        public ApiResponse<string> RemoveComment(string token, Guid commentId, Guid routeId) {
            if (!_authenticationService.HasRole(token, Role.Authenticated))
            {
                return new ApiErrorResponse<string>("You need to be logged in to remove a comment");
            }

            bool isAdmin = _authenticationService.HasRole(token, Role.Admin);
            
            var user = _memberRepository.GetAll().FirstOrDefault(m => m.Token == token);

            if (user == null)
                return new ApiErrorResponse<string>("You need to be logged in to remove a comment");
            
            var route = _routeRepository.Find(routeId);
            if (route == null) 
                return new ApiErrorResponse<string>("Invalid route");

            var comment = route.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)   
                return new ApiErrorResponse<string>("Invalid comment");

            //Only the creater of a comment can delete it, unless they're an administrator
            if (user.Id != comment.MemberId && !isAdmin)
                return new ApiErrorResponse<string>("You cannot delete a comment you haven't made yourself");

            //Deletes the video from the database if such a video exists
            if (comment.Video != null) {
                System.IO.File.Delete(comment.Video.FilePath);
            }

            route.Comments.Remove(comment);
            _routeRepository.Save();                  
            
            return new ApiSuccessResponse<string>("Comment deleted");
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
            
            if (routeToUpdate.Name != route.Name && routeToUpdate.GradeId != route.GradeId)
                if (_routeRepository.GetAll().Any(r => r.GradeId == route.GradeId && r.Name == route.Name))
                {
                    return new ApiErrorResponse<Route>("A route with this grade and number already exists");
                }

            //Update the existing route with the changed values. 
            //If some of the values of the new route is null, keep the existing ones, except for ColorOfTape, which is allowed to be null
            routeToUpdate.ColorOfHolds = route.ColorOfHolds ?? routeToUpdate.ColorOfHolds;
            routeToUpdate.ColorOfTape = route.ColorOfTape;
            routeToUpdate.Name = route.Name ?? routeToUpdate.Name;
            routeToUpdate.Author = route.Author ?? routeToUpdate.Author;
            routeToUpdate.Note = route.Note;

            //If the new route's image is not null, then replace the existing image with the new one
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
                // Console.WriteLine(JsonConvert.SerializeObject(route.Image));
                
                route.Image.RouteId = routeToUpdate.Id;
                routeToUpdate.Image = route.Image;
                _imageRepository.Save();
            }

            if(route.GradeId != default(Guid))
            {
                routeToUpdate.Grade = _gradeRepository.Find(route.GradeId);
                if (routeToUpdate.Grade != null)
                    routeToUpdate.GradeId = routeToUpdate.Grade.Id;
                else
                    return new ApiErrorResponse<Route>("The specified grade does not exist");
            }

            if(route.SectionId != default(Guid))
            {
                routeToUpdate.SectionId = route.SectionId;
            }
 
            if(!_holdColorRepository.GetAll().Any(r => r.ColorOfHolds.R == routeToUpdate.ColorOfHolds.R &&
                                                       r.ColorOfHolds.G == routeToUpdate.ColorOfHolds.G &&
                                                       r.ColorOfHolds.B == routeToUpdate.ColorOfHolds.B
                                                       ))
            {
                return new ApiErrorResponse<Route>("A valid hold color must be specified");
            }

            if (routeToUpdate.ColorOfTape != null) {
                if(!_holdColorRepository.GetAll().Any(r => r.ColorOfHolds.R == routeToUpdate.ColorOfTape.R &&
                                                      r.ColorOfHolds.G == routeToUpdate.ColorOfTape.G &&
                                                      r.ColorOfHolds.B == routeToUpdate.ColorOfTape.B &&
                                                      r.ColorOfHolds.A == routeToUpdate.ColorOfTape.A
                                                      ))
                    return new ApiErrorResponse<Route>("The specified tape color doesn't exist. Choose a valid one, or none at all");
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

            //Delete route if member is authorised to do so, and if the specified route exists
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

        // PUT: /api/route/{routeId}/rating
        [HttpPut("{routeId}/rating")]
        public ApiResponse<Route> SetRating(string token, Guid routeId, int ratingValue)
        {
            if (!_authenticationService.HasRole(token, Role.Authenticated))
            {
                return new ApiErrorResponse<Route>("You need to be logged in to change ratings");
            }
            
            //Make sure ratingvalue is between 1 and 5
            ratingValue = Math.Max(1, Math.Min(5, ratingValue));

            var route = _routeRepository.Find(routeId);
            var member = _memberRepository.GetAll().FirstOrDefault(m => m.Token == token);
            var previousRating = route.Ratings.FirstOrDefault(r => r.Member.Token == token);

            if (route == null)
                return new ApiErrorResponse<Route>("No route with this id exists");

            if (member == default(Member))
                return new ApiErrorResponse<Route>("No member with this token exists. Are you logged in?");

            //Add a new rating if a rating by this member doesn't exist for the given route
            if (previousRating == default(Rating))
            {
                route.Ratings.Add(new Rating
                {
                    Id = new Guid(),
                    Member = member,
                    Route = route,
                    RouteId = route.Id,
                    RatingValue = ratingValue
                });
            }
            //Else, change the value of the rating
            else
            {
                route.Ratings.FirstOrDefault(r => r.Member.Token == token).RatingValue = ratingValue;
            }

            _routeRepository.Save();

            return new ApiSuccessResponse<Route>(route);
        }
    }
}