using Microsoft.Extensions.Logging;

namespace sunstealer.mvc.Controllers
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly Microsoft.Extensions.Logging.ILogger<HomeController> _logger;

        public HomeController(Microsoft.Extensions.Logging.ILogger<HomeController> logger)
        {
            _logger = logger;
            _logger.LogInformation("HomeController.HomeController()");
        }

        public Microsoft.AspNetCore.Mvc.IActionResult Index()
        {
            return View();
        }

        public Microsoft.AspNetCore.Mvc.IActionResult Privacy()
        {
            return View();
        }

        [Microsoft.AspNetCore.Mvc.ResponseCache(Duration = 0, Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.None, NoStore = true)]
        public Microsoft.AspNetCore.Mvc.IActionResult Error()
        {
            return View(new sunstealer.mvc.Models.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
