using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using AKK.Classes.Models;
using AKK.Classes.ApiResponses;



namespace AKK.Controllers {
    [Route("api/section")]
    public class SectionController : Controller {
        MainDbContext _mainDbContext;
        public SectionController(MainDbContext mainDbContext) {
            _mainDbContext = mainDbContext;
        }

        [HttpGet]
        public ApiSuccessResponse GetAll() {
            var sections = _mainDbContext.Sections.AsQueryable();
            // var sections = _mainDbContext.Sections.Include(s => s.Routes).AsQueryable();

            return new ApiSuccessResponse(sections);
        }

        [HttpGet("{name}")]
        public ApiResponse GetAll(string name) {
            var sections = _mainDbContext.Sections.AsQueryable();

            sections = sections.Where(s => s.Name == name);
            if(sections.Count() != 1)
                return new ApiErrorResponse("No section with name " + name);

            return new ApiSuccessResponse(sections.Where(s => s.Name == name));
        }
    }
}