using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using AKK.Controllers.ApiResponses;
using AKK.Models;
using AKK.Models.Repositories;
using AKK.Services;

namespace AKK.Controllers 
{
    [Route("api/grade")]
    public class GradeController : Controller 
    {
        readonly IRepository<Grade> _gradeRepository;
        readonly IAuthenticationService _authenticationService;

        public GradeController(IRepository<Grade> gradeRepository, IAuthenticationService authenticationService)
        {
            _gradeRepository = gradeRepository;
            _authenticationService = authenticationService;
        }

        // GET: /api/grade
        [HttpGet]
        public ApiResponse<IEnumerable<Grade>> GetAllGrades() {
            var grades = _gradeRepository.GetAll().AsQueryable().OrderBy(g => g.Difficulty);

            return new ApiSuccessResponse<IEnumerable<Grade>>(grades);
        }

        // POST: /api/grade
        [HttpPost]
        public ApiResponse<Grade> AddGrade(string token, Grade grade) 
        {
            if (!_authenticationService.HasRole(token, Role.Admin))
            {
                return new ApiErrorResponse<Grade>("You need to be logged in as an administrator to add a new grade");
            }
            if(_gradeRepository.GetAll().Count(g => g.Difficulty == grade.Difficulty) != 0)
                return new ApiErrorResponse<Grade>("A grade already exists with the given difficulty");

            _gradeRepository.Add(grade);
            try
            {
                _gradeRepository.Save();
                return new ApiSuccessResponse<Grade>(grade);
            }
            catch
            {
                return new ApiErrorResponse<Grade>("Failed to add grade");
            }
        }

        // GET: /api/grade/{id}
        [HttpGet("{id}")]
        public ApiResponse<Grade> GetGrade(Guid id)
        {
            var grade = _gradeRepository.Find(id);
            if (grade == null)
            {
                return new ApiErrorResponse<Grade>("No grades with given id exist");
            }
                
            return new ApiSuccessResponse<Grade>(grade);
        }

        // PATCH: /api/grade/{gradeId}
        [HttpPatch("{gradeId}")]
        public ApiResponse<Grade> UpdateGrade(string token, Guid gradeId, Grade grade) 
        {
            if (!_authenticationService.HasRole(token, Role.Admin))
            {
                return new ApiErrorResponse<Grade>("You need to be logged in as an administrator to change this grade");
            }
            Grade oldGrade = _gradeRepository.Find(gradeId);
            if (oldGrade == null)
            {
                return new ApiErrorResponse<Grade>($"No grade exists with id {gradeId}");
            }
            if (grade == null)
            {
                return new ApiSuccessResponse<Grade>(oldGrade);
            }

            oldGrade.Name = grade.Name ?? oldGrade.Name;
            oldGrade.Difficulty = grade.Difficulty ?? oldGrade.Difficulty;
            oldGrade.Color = grade.Color ?? oldGrade.Color;


            if (_gradeRepository.GetAll().Any(g => g.Id != oldGrade.Id && g.Difficulty == oldGrade.Difficulty ))
            {
                return new ApiErrorResponse<Grade>("A grade with this difficulty already exists");
            }

            if (_gradeRepository.GetAll().Any(g => g.Id != oldGrade.Id && g.Name == oldGrade.Name ))
            {
                return new ApiErrorResponse<Grade>("A grade with this name already exists");
            }
                
            try
            {
                _gradeRepository.Save();
                return new ApiSuccessResponse<Grade>(oldGrade);
            }
            catch
            {
                return new ApiErrorResponse<Grade>("Failed to update grade to database");
            }
        }

        // DELETE: /api/grade/{id}
        [HttpDelete("{id}")]
        public ApiResponse<Grade> DeleteGrade(string token, Guid id) 
        {
            if (!_authenticationService.HasRole(token, Role.Admin))
            {
                return new ApiErrorResponse<Grade>("You need to be logged in as an administrator to delete this grade");
            }

            var grade = _gradeRepository.Find(id);
            if (grade == default(Grade))
            {
                return new ApiErrorResponse<Grade>($"No grade exists with id {id}");
            }


            if (grade.Routes == null)
            {
                return new ApiErrorResponse<Grade>("Not sure if this grade has any routes. If it has, deleting it would mean trouble!");
            }

            if (grade.Routes.Count != 0)
            {
                return new ApiErrorResponse<Grade>("Routes already exists with this grade. Remove those before you delete this grade");
            }

            // create copy that can be sent as result
            Grade resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(grade)) as Grade;
            //resultCopy = new Grade();
            //System.Console.WriteLine(resultCopy.Color.R);
            _gradeRepository.Delete(grade.Id);

            try
            {
                _gradeRepository.Save();
                return new ApiSuccessResponse<Grade>(resultCopy);

            }
            catch
            {
                return new ApiErrorResponse<Grade>("Failed to remove grade from database");
            }
        }

        // GET: /api/grade/{id}/routes
        [HttpGet("{id}/routes")]
        public ApiResponse<IEnumerable<Route>> GetGradeRoutes(Guid id)
        {
            var grade = _gradeRepository.Find(id);

            if (grade == null)
            {
                return new ApiErrorResponse<IEnumerable<Route>>("No grades with given id exists");
            }

            return new ApiSuccessResponse<IEnumerable<Route>>(grade.Routes);
        }
    }
}