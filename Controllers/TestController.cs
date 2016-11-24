using Microsoft.AspNetCore.Mvc;
using AKK.Models;

namespace AKK.Controllers {
    [Route("test")]
    public class TestController : Controller {
        
        MainDbContext _dbContext;

        public TestController (MainDbContext dbContext)
        {
        }
        
        [HttpGet]
        public IActionResult Index() {
            return new JsonResult("");
        }
    }
} 