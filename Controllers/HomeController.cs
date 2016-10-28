using Microsoft.AspNetCore.Mvc;
using AKK.Models;
using System.Linq;

namespace AKK.Controllers {
    [Route("")]
    public class HomeController : Controller {
        
        MainDbContext db;

        public HomeController (MainDbContext context)
        {
          db = context;
        }
        
        [HttpGet]
        public string Index() {
            return db.Sections.First(x => x.Name == "A").Routes.Count.ToString();
        }
    }
} 