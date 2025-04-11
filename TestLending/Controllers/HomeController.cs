using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestLending.Models;
using Microsoft.Extensions.Logging;

namespace TestLending.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitOrder(string fullName, string phoneNumber, string email)
        {
            _logger.LogInformation("Заявка відправлена:");
            _logger.LogInformation("ПІБ: {FullName}", fullName);
            _logger.LogInformation("Номер телефону: {PhoneNumber}", phoneNumber);
            _logger.LogInformation("Email: {Email}", email);

            return Json(new { success = true });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
