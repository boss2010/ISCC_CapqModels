using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using static ViewModels.ExportImportActivity_API;

namespace Capqwebsite.Controllers
{
    public class HomeController(IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        //Api Get with param
        public async Task<IActionResult> Index()
        {
#if false // Old Home API is no longer needed. ImportRequests reads the data directly from DBConnection.
            try
            {
                if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
                    !long.TryParse(HttpContext.Session.GetString("UserId"), out long userId) ||
                    !long.TryParse(HttpContext.Session.GetString("EmployeeId"), out long employeeId))
                {
                    return RedirectToAction("Index", "Login");
                }

                long User_Id = userId;
                ViewBag.User_Id = User_Id;
                ViewBag.EmployeeId = employeeId;
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                //string Check_Date = "05-31-2025";
                string Check_Date = DateTime.Now.ToString("MM-dd-yyyy");
                //string Check_Date = "2025-04-27";

                string apiUrl = $"http://10.7.7.250:40/api/ExportImportActivity_API?User_Id={employeeId}&Check_Date={Uri.EscapeDataString(Check_Date)}";

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into list of model objects
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var activityList = JsonSerializer
                    .Deserialize<List<Ex_Im_CheckRequest_GetAllByUser_DateVM>>(responseBody, options)
                    ?.Where(item => item.IsExport == 2)
                    .ToList() ?? new List<Ex_Im_CheckRequest_GetAllByUser_DateVM>();
              

                return View("Index", activityList);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"API Error: {ex.Message}");
            }
#else
            await Task.CompletedTask;
            return RedirectToAction("Index", "ImportRequests");
#endif
        }
    }


}

public class CommitteeItem
{
    public int Row_Num { get; set; }
    public int? IsExport { get; set; }
    public string CheckRequest_Number { get; set; }
    public int checkRequest_Id { get; set; }
    public int Committee_ID { get; set; }
    public string Committee_Type_Name { get; set; }
    public int Committee_Type_Id { get; set; }
    public string RequestCommittee_Status { get; set; }
    public int RequestCommittee_Status_Id { get; set; }
    public string BarCode { get; set; }
    public string Emp_Committe { get; set; }
    public string Request_Treatment { get; set; }
    public string Request_Treatment_Data { get; set; }
    public string FarmAnlysisCount_XMl { get; set; }
}

public class Employee
{
    public string Employee_Id { get; set; }
    public string FullName { get; set; }
    public string LoginName { get; set; }
    public string Password { get; set; }
    public string ISAdmin { get; set; }
    public string EmpToken { get; set; }
}
