using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Capqwebsite.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IConfiguration configuration, ILogger<LoginController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("/Login/Index")]

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserSession") == "Authenticated")
            {
                return RedirectToAction("Index", "ImportRequests");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GoDataEntryMenu(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.LoginError = "اكتب اسم المستخدم وكلمة السر";
                return View("Index");
            }

            try
            {
                var connectionString = _configuration.GetConnectionString("PrivilegeDBConnection");
                await using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT TOP (1) Id, EmpId, LoginName
                    FROM dbo.PR_User
                    WHERE LoginName = @LoginName
                      AND Password = @Password
                      AND Active = 1";
                command.Parameters.AddWithValue("@LoginName", userName.Trim());
                command.Parameters.AddWithValue("@Password", password);

                await using var reader = await command.ExecuteReaderAsync();
                if (!await reader.ReadAsync() || reader.IsDBNull(0) || reader.IsDBNull(1))
                {
                    ViewBag.LoginError = "اسم المستخدم أو كلمة السر غير صحيحة، أو الحساب غير نشط";
                    return View("Index");
                }

                long userId = Convert.ToInt64(reader.GetValue(0));
                long employeeId = Convert.ToInt64(reader.GetValue(1));
                string loginName = reader.GetString(2);
                HttpContext.Session.SetString("UserSession", "Authenticated");
                HttpContext.Session.SetString("UserName", loginName);
                HttpContext.Session.SetString("UserId", userId.ToString());
                HttpContext.Session.SetString("EmployeeId", employeeId.ToString());
                return RedirectToAction("Index", "ImportRequests");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Login database error for user {LoginName}; TraceId={TraceId}",
                    userName.Trim(), HttpContext.TraceIdentifier);
                ViewBag.LoginError = "تعذر الاتصال بقاعدة البيانات. حاول مرة أخرى أو تواصل مع مسؤول النظام";
                return View("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
