using System.Text.Json; // For System.Text.Json
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using static ViewModels.ExportImportActivity_API;
public class TestController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

    public async Task<IActionResult> Index()
    {

        try
        {
            int User_Id = 291;
            string Check_Date = "03-05-2025";

            string apiUrl = $"http://localhost:8022/api/ExportImportActivity_API?User_Id={User_Id}&Check_Date={Uri.EscapeDataString(Check_Date)}";

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