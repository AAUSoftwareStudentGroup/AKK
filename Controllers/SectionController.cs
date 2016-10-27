using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using AKK.Models;

namespace AKK.Controllers {
    [Route("api/section")]
    public class SectionController : Controller {
        MainDbContext _mainDbContext;
        public SectionController(MainDbContext mainDbContext) {
            _mainDbContext = mainDbContext;
        }

        [HttpGet]
        public JsonResult GetAll() {
            var sections = _mainDbContext.Sections.AsQueryable(); 

            return new JsonResult(sections);
        }

        [HttpGet("{name}")]
        public JsonResult GetAll(string name) {
            var sections = _mainDbContext.Sections.AsQueryable(); 

            return new JsonResult(sections.Where(s => s.Name == name));
        }
    }
}