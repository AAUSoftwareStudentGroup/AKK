using Microsoft.AspNetCore.Mvc;

namespace AKK.Controllers {
    [Route("")]
    public class ViewController : Controller {
        public ViewController()
        {
            
        }

        // GET: /
        [HttpGet]
        public IActionResult Routes() { return View(); }

        // GET: /route-info
        [HttpGet("route-info")]
        public IActionResult RouteInfo() { return View(); }

        // GET: /new-route
        [HttpGet("new-route")]
        public IActionResult NewRoute() { return View(); }

        // GET: /edit-route
        [HttpGet("edit-route")]
        public IActionResult EditRoute() { return View(); }

        // GET: /sections
        [HttpGet("sections")]
        public IActionResult Sections() { return View(); }
    }
}