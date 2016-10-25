using Microsoft.AspNetCore.Mvc;

namespace AKK.Controllers {
    [Route("")]
    public class HomeController : Controller {
        [HttpGet]
        public IActionResult Index() {
            return View();
        }
    }
}