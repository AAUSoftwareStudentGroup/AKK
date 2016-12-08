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
                return new ApiErrorResponse<Grade>("A grade with this difficulty already exists");

            //Add the grade to the grade repository, if the caller of the method is an administrator and the grade doesn't already exist
            _gradeRepository.Add(grade);
            try
            {
                _gradeRepository.Save();
                return new ApiSuccessResponse<Grade>(grade);
            }
            catch
            {
                return new ApiErrorResponse<Grade>("Failed to save grade to database");
            }
        }

        // GET: /api/grade/{id}
        [HttpGet("{id}")]
        public ApiResponse<Grade> GetGrade(Guid id)
        {
            var grade = _gradeRepository.Find(id);
            if (grade == null)
            {
                return new ApiErrorResponse<Grade>($"No grade exists with id {id}");
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

            //Update the existing grade with the changed values. If some of the values of the new grade is null, keep the existing ones
            oldGrade.Name = grade.Name ?? oldGrade.Name;
            oldGrade.Difficulty = grade.Difficulty ?? oldGrade.Difficulty;
            oldGrade.Color = grade.Color ?? oldGrade.Color;

            //If a grade exists with the same difficulty as the one we are about to save, don't add it and return an error 
            if (_gradeRepository.GetAll().Any(g => g.Id != oldGrade.Id && g.Difficulty == oldGrade.Difficulty ))
            {
                return new ApiErrorResponse<Grade>("A grade with this difficulty already exists");
            }

            //If a grade exists with the same name as the one we are about to save, don't add it and return an error 
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
                return new ApiErrorResponse<Grade>("Failed to save updated grade to databae");
            }
        }

        // PATCH: /api/grade/{grade1Id}/swap/{grade2Id}
        [HttpPatch("{grade1Id}/swap/{grade2Id}")]
        public ApiResponse<IEnumerable<Grade>> SwapGrades(string token, Guid grade1Id, Guid grade2Id) 
        {
            if (!_authenticationService.HasRole(token, Role.Admin))
            {
                return new ApiErrorResponse<IEnumerable<Grade>>("You need to be logged in as an administrator to change this grade");
            }
            Grade grade1 = _gradeRepository.Find(grade1Id);
            Grade grade2 = _gradeRepository.Find(grade2Id);
            if (grade1 == null)
            {
                return new ApiErrorResponse<IEnumerable<Grade>>($"No grade exists with id {grade1Id}");
            }
            if (grade2 == null)
            {
                return new ApiErrorResponse<IEnumerable<Grade>>($"No grade exists with id {grade2Id}");
            }

            //Swap the difficulty of the found grades, to swap them in the database
            var tmpDiff = grade1.Difficulty;
            grade1.Difficulty = grade2.Difficulty;
            grade2.Difficulty = tmpDiff;

            try
            {
                var result = new List<Grade>();
                result.Add(grade1);
                result.Add(grade2);
                _gradeRepository.Save();
                return new ApiSuccessResponse<IEnumerable<Grade>>(result);
            }
            catch
            {
                return new ApiErrorResponse<IEnumerable<Grade>>("Failed to save swapped grades to database");
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

            if (grade.Routes.Count != 0)
            {
                return new ApiErrorResponse<Grade>("This grade has routes associated with it. Delete them before deleting the grade");
            }

            // create copy that can be sent as result
            Grade resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(grade)) as Grade;

            //Delete the grade from the repository if the caller of the method is an administrator and no routes exists with this grade
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
    }
}