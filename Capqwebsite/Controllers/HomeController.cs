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
        public async Task<IActionResult> Index(long User_Id= 222)
        {

            try
            {
                ViewBag.User_Id = User_Id;
                //string Check_Date = "05-31-2025";
                string Check_Date = DateTime.Now.ToString("MM-dd-yyyy");
                //string Check_Date = "2025-04-27";

                string apiUrl = $"http://10.7.7.250:40/api/ExportImportActivity_API?User_Id={User_Id}&Check_Date={Uri.EscapeDataString(Check_Date)}";

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into list of model objects
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var activityList = JsonSerializer.Deserialize<List<Ex_Im_CheckRequest_GetAllByUser_DateVM>>(responseBody, options);
              

                return View("Index", activityList);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"API Error: {ex.Message}");
            }
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
