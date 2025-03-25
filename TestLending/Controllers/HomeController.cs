using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestLending.Models;
using Microsoft.Extensions.Logging;
using static TestLending.Models.Models;
using System.Data;
using System;
using Microsoft.AspNetCore.Http;

namespace TestLending.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly DatabaseHelper _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context; 
            _db = new DatabaseHelper(configuration);
        }

        public IActionResult Index(short i = 1)
        {
            if(Request.Cookies.TryGetValue("Leng", out string? cookie))
            {
                ViewBag.Cookie = cookie;
            }
            else
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(365),
                    HttpOnly = false
                };
                Response.Cookies.Append("Leng", "ua", options);
            }
            var data = _db.ExecuteQuery<PageData>(@"select s.SectionName, sc.ua, sc.en, sc.Img, sc.`Order`, sct.Name as SectionContentType from TLending.Pages p
                                left join TLending.Sections s on s.PageID = p.ID
                                left join TLending.SectionsContent sc on sc.SectionID = s.ID
                                left join TLending.SectionsContentType sct on sct.ID = sc.Type
                                where p.ID = @p0", i).ToList();
            return View(data);
        }
        [HttpGet("/set-leng/{lang}")]
        public IActionResult SetLangCookie(string lang)
        {
            try
            {
                if(Request.Cookies.TryGetValue("Leng", out string? cookie))
                {
                    Response.Cookies.Delete("Leng");
                }
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(365),
                    HttpOnly = false,
                };
                Response.Cookies.Append("Leng", lang, options);
            }
            catch (Exception ex)
            {

            }
            return Ok();
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
