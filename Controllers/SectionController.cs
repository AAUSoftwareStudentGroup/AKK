using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using AKK.Classes.Models;
using AKK.Classes.ApiResponses;


namespace AKK.Controllers {
    [Route("api/section")]
    public class SectionController : Controller {
        MainDbContext _mainDbContext;
        public SectionController(MainDbContext mainDbContext) {
            _mainDbContext = mainDbContext;
        }

        // GET: /api/section
        [HttpGet]
        public ApiResponse GetAllSections() {
            var sections = _mainDbContext.Sections.AsQueryable().OrderBy(s => s.Name);

            return new ApiSuccessResponse(
                Mappings.Mapper.Map<IEnumerable<Section>, IEnumerable<SectionTransferObject>>(sections)
            );
        }

        // POST: /api/section
        [HttpPost]
        public ApiResponse AddSection(string name) {
            var sectionExsits = _mainDbContext.Sections.Where(s => s.Name == name);
            if(sectionExsits.Count() > 0) {
                return new ApiErrorResponse("A section with name "+name+" already exist");
            }
            Section section = new Section() {Name=name};
            _mainDbContext.Sections.Add(section);
            if(_mainDbContext.SaveChanges() > 0)
                return new ApiSuccessResponse(Mappings.Mapper.Map<Section, SectionTransferObject>(section));
            else
                return new ApiErrorResponse("Failed to create new section with name "+name);
        }

        // DELETE: /api/section
        [HttpDelete]
        public ApiResponse DeleteAllSections() {
            var sections = _mainDbContext.Sections
                .Include(s => s.Routes).ThenInclude(r => r.Grade).ThenInclude(g => g.Color)
                .Include(s => s.Routes).ThenInclude(r => r.ColorOfHolds)
                .Include(s => s.Routes).ThenInclude(r => r.ColorOfTape).AsQueryable();
            if(sections.Count() == 0)
                return new ApiErrorResponse("No sections exist");
            
            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(sections));

            _mainDbContext.Sections.RemoveRange(sections);
            if(_mainDbContext.SaveChanges() == 0)
                return new ApiErrorResponse("Failed to remove sections from database");
            
            return new ApiSuccessResponse(resultCopy);
        }

        // GET: /api/section/{name}
        [HttpGet("{name}")]
        public ApiResponse GetSection(string name) {
            var sections = _mainDbContext.Sections.AsQueryable();
            
            try {
                Guid id = new Guid(name);
                sections = sections.Where(s => s.SectionId == id);
            } catch(System.FormatException) {
                sections = sections.Where(s => s.Name == name);
            }

            if(sections.Count() != 1)
                return new ApiErrorResponse("No section with name/id " + name);

            return new ApiSuccessResponse(Mappings.Mapper.Map<Section, SectionTransferObject>(sections.First()));
        }

        // DELETE: /api/section/{name}
        [HttpDelete("{name}")]
        public ApiResponse DeleteSection(string name) {
            var sections = _mainDbContext.Sections
                .Include(s => s.Routes).ThenInclude(r => r.Grade).ThenInclude(g => g.Color)
                .Include(s => s.Routes).ThenInclude(r => r.ColorOfHolds)
                .Include(s => s.Routes).ThenInclude(r => r.ColorOfTape).AsQueryable();
            
            try {
                Guid id = new Guid(name);
                sections = sections.Where(s => s.SectionId == id);
            } catch(System.FormatException) {
                sections = sections.Where(s => s.Name == name);
            }

            if(sections.Count() == 0)
                return new ApiErrorResponse("No section exists with name/id "+name);
            else {
                // create copy that can be sent as result // we dont map so that we can output the deleted routes as well
                var resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(sections));
        
                _mainDbContext.Sections.Remove(sections.First());

                if(_mainDbContext.SaveChanges() > 0)
                    return new ApiSuccessResponse(resultCopy);
            }

            return new ApiErrorResponse("Failed to delete section with name/id "+name);
        }

        // GET: /api/section/{name}/routes
        [HttpGet("{name}/routes")]
        public ApiResponse GetSectionRoutes(string name) {
            var sections = _mainDbContext.Sections
                .Include(s => s.Routes).ThenInclude(r => r.Grade).ThenInclude(g => g.Color)
                .Include(s => s.Routes).ThenInclude(r => r.ColorOfHolds)
                .Include(s => s.Routes).ThenInclude(r => r.ColorOfTape).AsQueryable();

            try {
                Guid id = new Guid(name);
                sections = sections.Where(s => s.SectionId == id);
            } catch(System.FormatException) {
                sections = sections.Where(s => s.Name == name);
            }

            if(sections.Count() != 1)
                return new ApiErrorResponse("No section with name/id "+name);
            return new ApiSuccessResponse(
                Mappings.Mapper.Map<IEnumerable<Route>, IEnumerable<RouteDataTransferObject>>(
                    sections.First().Routes
                )
            );
        }

        // DELETE: /api/section/{name}/routes
        [HttpDelete("{name}/routes")]
        public ApiResponse DeleteSectionRoutes(string name) {
            var sections = _mainDbContext.Sections
                .Include(s => s.Routes).ThenInclude(r => r.Section)
                .Include(s => s.Routes).ThenInclude(r => r.Grade).ThenInclude(g => g.Color)
                .Include(s => s.Routes).ThenInclude(r => r.ColorOfHolds)
                .Include(s => s.Routes).ThenInclude(r => r.ColorOfTape).AsQueryable();

            try {
                Guid id = new Guid(name);
                sections = sections.Where(s => s.SectionId == id);
            } catch(System.FormatException) {
                sections = sections.Where(s => s.Name == name);
            }

            if(sections.Count() != 1)
                return new ApiErrorResponse("No section with name/id "+name);

            var section = sections.First();
            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(
                JsonConvert.SerializeObject(
                    Mappings.Mapper.Map<IEnumerable<Route>, IEnumerable<RouteDataTransferObject>>(section.Routes)
                )
            );
            section.Routes.RemoveAll(r => true);
            if(_mainDbContext.SaveChanges() == 0) {
                return new ApiErrorResponse("Failed to delete routes of section with name/id "+name);
            }

            return new ApiSuccessResponse(resultCopy);
        }
    }
}