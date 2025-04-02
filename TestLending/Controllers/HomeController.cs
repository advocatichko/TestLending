using Microsoft.AspNetCore.Mvc;
using TestLending.Data;
using TestLending.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestLending.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext dbContext, ILogger<HomeController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendApplication(CrmLead lead)
        {
            var requestId = Guid.NewGuid();
            _logger.LogInformation("Request ID: {RequestId} - Получен запрос на SendApplication", requestId);
            _logger.LogInformation("Request ID: {RequestId} - Метод: {Method}, URL: {Url}", requestId, HttpContext.Request.Method, HttpContext.Request.Path);

            foreach (var header in HttpContext.Request.Headers)
            {
                _logger.LogInformation("Request ID: {RequestId} - Заголовок: {Key} = {Value}", requestId, header.Key, header.Value);
            }

            _logger.LogInformation("Request ID: {RequestId} - Отримано заявку: {@Lead}", requestId, lead);

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    foreach (var subError in error.Value.Errors)
                    {
                        _logger.LogWarning("Request ID: {RequestId} - Помилка в полі {Field}: {ErrorMessage}", requestId, error.Key, subError.ErrorMessage);
                    }
                }

                return StatusCode(400, "Дані заявки некоректні.");
            }

            // Проверка на существующую заявку по Email или номеру телефона
            var existingLead = await _dbContext.CrmLeads
                .FirstOrDefaultAsync(l => 
                    (l.Email == lead.Email || l.Phone == lead.Phone) && l.IsProcessed == false);

            if (existingLead != null)
            {
                _logger.LogWarning("Request ID: {RequestId} - Заявка для Email {Email} або телефону {Phone} вже існує і очікує обробки.", requestId, lead.Email, lead.Phone);
                return Ok("Заявка з такими даними вже існує і очікує обробки.");
            }

            lead.Message ??= string.Empty;

            try
            {
                lead.CreatedAt = DateTime.UtcNow;
                lead.IsProcessed = false;
                lead.Source = "Website штраф-тцк.укр";

                _logger.LogInformation("Request ID: {RequestId} - Додавання заявки до БД...", requestId);
                _dbContext.CrmLeads.Add(lead);

                _logger.LogInformation("Request ID: {RequestId} - Збереження змін у БД...", requestId);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Request ID: {RequestId} - Заявка для {Name} успішно збережена в БД", requestId, lead.Name);
                return Ok("Заявка успішно відправлена.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request ID: {RequestId} - Помилка збереження заявки для {Name}", requestId, lead.Name);
                return StatusCode(500, "Сталася помилка під час збереження заявки.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClientData()
        {
            // Пример получения данных клиента из базы данных
            var clientId = HttpContext.User.Identity?.Name; // Используем идентификатор пользователя, если он авторизован
            if (string.IsNullOrEmpty(clientId))
            {
                return Json(new { name = "", phone = "", email = "" }); // Возвращаем пустые данные, если пользователь не авторизован
            }

            var client = await _dbContext.CrmLeads
                .FirstOrDefaultAsync(c => c.Email == clientId); // Пример фильтрации по Email

            if (client == null)
            {
                return Json(new { name = "", phone = "", email = "" }); // Если клиент не найден
            }

            return Json(new
            {
                name = client.Name,
                phone = client.Phone,
                email = client.Email
            });
        }
    }
}
