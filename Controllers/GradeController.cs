using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using AKK.Classes.Models;
using AKK.Classes.ApiResponses;


namespace AKK.Controllers {
    [Route("api/grade")]
    public class GradeController : Controller {
        MainDbContext db;
        public GradeController(MainDbContext mainDbContext) {
            db = mainDbContext;
        }

        // GET: /api/grade
        [HttpGet]
        public ApiResponse GetAllGrades() {
            var grades = db.Grades.Include(g => g.Color).AsQueryable().OrderBy(g => g.Difficulty);

            return new ApiSuccessResponse(grades);
        }

        // POST: /api/grade
        [HttpPost]
        public ApiResponse AddGrade(Grade grade) {
            if(db.Grades.Where(g => g.Difficulty == grade.Difficulty).Count() != 0)
                return new ApiErrorResponse("A grade already exsist with the given difficulty");

            db.Grades.Add(grade);
            if(db.SaveChanges() == 0)
                return new ApiErrorResponse("Failed to add grade");
            return new ApiSuccessResponse(grade);
        }

        // GET: /api/grade/{id}
        [HttpGet("{id}")]
        public ApiResponse GetGrade(string id) {
            Grade grade = FindGrade(id, db.Grades.Include(g => g.Color).AsQueryable());
            if(grade == null)
                return new ApiErrorResponse("No grades with given id exsist");

            return new ApiSuccessResponse(grade);
        }

        // PATCH: /api/grade/{id}
        [HttpPatch("{id}")]
        public ApiResponse UpdateGrade(string id, int? difficulty, Color color) {
            Grade oldGrade = FindGrade(id, db.Grades.Include(g => g.Color).AsQueryable());
            if(oldGrade == null)
                return new ApiErrorResponse("No grade exsist with difficulty/id " + id);

            if(difficulty != null) {
                if(db.Grades.Where(g => g.Difficulty == difficulty).Count() > 0)
                    return new ApiErrorResponse("A grade with this difficulty already exsist");
    
                oldGrade.Difficulty = (int)difficulty; 
            }

            if(color != null)
                oldGrade.Color = color;

            if(db.SaveChanges() == 0)
                return new ApiErrorResponse("Failed to update grade to database");
            return new ApiSuccessResponse(oldGrade);
        }

        // DELETE: /api/grade/{id}
        [HttpDelete("{id}")]
        public ApiResponse DeleteGrade(string id) {
            Grade grade = FindGrade(id, db.Grades.Include(g => g.Color).Include(g => g.Routes).AsQueryable());
            if(grade == null)
                return new ApiErrorResponse("No grade exsist with difficulty/id " + id);
            
            if(grade.Routes.Count() != 0)
                return new ApiErrorResponse("Routes exsit with this grade. Remove those before you delete this grade");

            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(grade));

            db.Grades.Remove(grade);
            if(db.SaveChanges() == 0)
                return new ApiErrorResponse("Failed to remove grade from database");
            
            return new ApiSuccessResponse(resultCopy);
        }

        // GET: /api/grade/{id}/routes
        [HttpGet("{id}/routes")]
        public ApiResponse GetGradeRoutes(string id) {
            Grade grade = FindGrade(id, db.Grades
                .Include(g => g.Routes).ThenInclude(r => r.ColorOfHolds)
                .Include(g => g.Routes).ThenInclude(r => r.ColorOfTape)
                .Include(g => g.Routes).ThenInclude(r => r.Grade.Color).AsQueryable()
            );

            if(grade == null)
                return new ApiErrorResponse("No grades with given id exsist");


            return new ApiSuccessResponse(grade.Routes);
        }

        // returns grade on either guid or difficulty
        public Grade FindGrade(string identifier, IQueryable<Grade> queryContext) {
            // Guid guid = new Guid(identifier);
            int difficulty;
            Grade grade = null;
            if(int.TryParse(identifier, out difficulty)) {
                var grades = queryContext.Where(g => g.Difficulty == difficulty);
                if(grades.Count() == 1)
                    grade = grades.First();
            }
            /*
            else {
                var grades = queryContext.Where(g => g.GradeId == guid);
                if(grades.Count() == 1)
                    grade = grades.First();
            }
            */
            return grade;
        }
    }
}