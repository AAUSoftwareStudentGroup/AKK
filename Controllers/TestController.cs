using System;
using Microsoft.AspNetCore.Mvc;
using AKK.Classes.Models;
using System.Linq;
using AKK.Classes.Models.Repository;

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