using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using AKK.Classes.Models;
using AKK.Classes.ApiResponses;
using AKK.Classes.Models.Repository;


namespace AKK.Controllers {
    [Route("api/section")]
    public class SectionController : Controller {
        IRepository<Section> _sectionRepository;
        public SectionController(IRepository<Section> sectionRepository )
        {
            _sectionRepository = sectionRepository;
        }

        // GET: /api/section
        [HttpGet]
        public ApiResponse GetAllSections() {
            var sections = _sectionRepository.GetAll().OrderBy(s => s.Name);

            return new ApiSuccessResponse(
                Mappings.Mapper.Map<IEnumerable<Section>, IEnumerable<SectionTransferObject>>(sections)
            );
        }

        // POST: /api/section
        [HttpPost]
        public ApiResponse AddSection(string name) {
            var sectionExsits = _sectionRepository.GetAll().Where(s => s.Name == name);
            if(sectionExsits.Any()) {
                return new ApiErrorResponse("A section with name "+name+" already exist");
            }
            if (name == null)
            {
                return new ApiErrorResponse("Name must have a value");
            }
            Section section = new Section() {Name=name};
            _sectionRepository.Add(section);
            try
            {
                _sectionRepository.Save();
                return new ApiSuccessResponse(Mappings.Mapper.Map<Section, SectionTransferObject>(section));
            }
            catch
            {
                return new ApiErrorResponse("Failed to create new section with name " + name);
            }
        }

        // DELETE: /api/section
        [HttpDelete]
        public ApiResponse DeleteAllSections() {
            var sections = _sectionRepository.GetAll();
            if(!sections.Any())
                return new ApiErrorResponse("No sections exist");
            
            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(sections));

            foreach (Section section in sections)
            {
                _sectionRepository.Delete(section);
            }

            try
            {
                _sectionRepository.Save();
                return new ApiSuccessResponse(resultCopy);
            }
            catch
            {
                return new ApiErrorResponse("Failed to remove sections from database");
            }
        }

        // GET: /api/section/{name}
        [HttpGet("{name}")]
        public ApiResponse GetSection(string name) {
            var sections = _sectionRepository.GetAll();
            
            try {
                Guid id = new Guid(name);
                sections = sections.Where(s => s.Id == id);
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
            Section section;
            
            try {
                Guid id = new Guid(name);
                section = _sectionRepository.Find(id);
            } catch(System.FormatException) {
                section = _sectionRepository.GetAll().FirstOrDefault(s => s.Name == name);
            }

            if(section == null)
                return new ApiErrorResponse("No section exists with name/id "+name);
            else {
                // create copy that can be sent as result // we dont map so that we can output the deleted routes as well
                var resultCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(section));
        
                _sectionRepository.Delete(section);

                try
                {
                    _sectionRepository.Save();
                    return new ApiSuccessResponse(resultCopy);
                }
                catch
                {
                    return new ApiErrorResponse("Failed to delete section with name/id " + name);
                }
            }

        }

        // GET: /api/section/{name}/routes
        [HttpGet("{name}/routes")]
        public ApiResponse GetSectionRoutes(string name)
        {
            Section section;

            try {
                Guid id = new Guid(name);
                section = _sectionRepository.Find(id);
            } catch(System.FormatException) {
                section = _sectionRepository.GetAll().FirstOrDefault(s => s.Name== name);
            }

            if(section == null)
                return new ApiErrorResponse("No section with name/id "+name);
            return new ApiSuccessResponse(
                Mappings.Mapper.Map<IEnumerable<Route>, IEnumerable<RouteDataTransferObject>>(
                    section.Routes
                )
            );
        }

        // DELETE: /api/section/{name}/routes
        [HttpDelete("{name}/routes")]
        public ApiResponse DeleteSectionRoutes(string name)
        {
            Section section;

            try
            {
                Guid id = new Guid(name);
                section = _sectionRepository.Find(id);
            }
            catch (System.FormatException)
            {
                section = _sectionRepository.GetAll().FirstOrDefault(s => s.Name == name);
            }

            if (section == null)
                return new ApiErrorResponse("No section with name/id "+name);
            
            // create copy that can be sent as result
            var resultCopy = JsonConvert.DeserializeObject(
                JsonConvert.SerializeObject(
                    Mappings.Mapper.Map<IEnumerable<Route>, IEnumerable<RouteDataTransferObject>>(section.Routes)
                )
            );
            section.Routes.RemoveAll(r => true);

            try
            {
                _sectionRepository.Save();
                return new ApiSuccessResponse(resultCopy);

            }
            catch
            {
                return new ApiErrorResponse("Failed to delete routes of section with name/id " + name);
            }
        }
    }
}