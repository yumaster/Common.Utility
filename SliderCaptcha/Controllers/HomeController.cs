using Microsoft.AspNetCore.Mvc;

namespace SliderCaptcha.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("/Home/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}