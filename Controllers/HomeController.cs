using Microsoft.AspNetCore.Mvc;
using AKK.Classes.Models;
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
            Route r = db.Routes.First();
            var rto = Mappings.Mapper.Map<Route, RouteDataTransferObject>(
                        r
                    );
            return Newtonsoft.Json.JsonConvert.SerializeObject(r, Newtonsoft.Json.Formatting.Indented);
        }
    }
} 