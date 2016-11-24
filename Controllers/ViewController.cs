using Microsoft.AspNetCore.Mvc;


namespace AKK.Controllers {
    [Route("")]
    public class ViewController : Controller {
        // GET: /
        [HttpGet]
        public IActionResult Routes() { return View("Views/Routes.cshtml"); }

        // GET: /route-info
        [HttpGet("route-info")]
        public IActionResult RouteInfo() { return View("Views/RouteInfo.cshtml"); }

        // GET: /new-route
        [HttpGet("new-route")]
        public IActionResult NewRoute() { return View("Views/NewRoute.cshtml"); }

        // GET: /edit-route
        [HttpGet("edit-route")]
        public IActionResult EditRoute() { return View("Views/EditRoute.cshtml"); }

        // GET: /sections
        [HttpGet("sections")]
        public IActionResult Sections() { return View("Views/Sections.cshtml"); }
    }
}