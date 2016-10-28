using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AKK.Classes.Models;
using AKK.Classes.ApiResponses;
using Newtonsoft.Json;


namespace AKK.Controllers {
    [Route("api/section")]
    public class SectionController : Controller {
        MainDbContext _mainDbContext;
        public SectionController(MainDbContext mainDbContext) {
            _mainDbContext = mainDbContext;
        }

        [HttpGet]
        public ApiSuccessResponse GetAllSections() {
            var sections = _mainDbContext.Sections.Include(s => s.Routes).AsQueryable();

            return new ApiSuccessResponse(sections);
        }

        [HttpGet("{name}")]
        public ApiResponse GetSection(string name) {
            var sections = _mainDbContext.Sections.Include(s => s.Routes).AsQueryable();

            sections = sections.Where(s => s.Name == name);
            if(sections.Count() != 1)
                return new ApiErrorResponse("No section with name " + name);

            return new ApiSuccessResponse(sections.Where(s => s.Name == name));
        }

        [HttpPost]
        public ApiResponse AddSection(string name) {
            var sectionExsits = _mainDbContext.Sections.Where(s => s.Name == name);
            if(sectionExsits.Count() > 0) {
                return new ApiErrorResponse("A section with "+name+" already exsists");
            }
            Section section = new Section() {Name=name};
            _mainDbContext.Sections.Add(section);
            if(_mainDbContext.SaveChanges() > 0)
                return new ApiSuccessResponse(section);
            else
                return new ApiErrorResponse("Failed to create new section with name "+name);
        }

        [HttpDelete("{name}")]
        public ApiResponse DeleteSection(string name) {
            var section = _mainDbContext.Sections.Where(s => s.Name == name);

            if(section.Count() == 0)
                return new ApiErrorResponse("No section exsists with name "+name);
            else {
                // create copy that can be sent as result
                var resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(section));
        
                _mainDbContext.Sections.Remove(section.First());

                if(_mainDbContext.SaveChanges() > 0)
                    return new ApiSuccessResponse(resultCopy);
            }

            return new ApiErrorResponse("Failed to delete section with name "+name);
        }
    }
}