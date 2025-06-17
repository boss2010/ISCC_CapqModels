using EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.Common;
using System.Net.Http;
using System.Text;
using ViewModels;

namespace Capqwebsite.Controllers
{
    public class shohna_saderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public shohna_saderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [AllowAnonymous]
        [Route("/shohna_sader/index")]
        public async Task<IActionResult> Index(long checkRequest_Id, long Committee_ID = 0, byte Committee_Type_Id = 1
            , long EmployeeId = 0, string ISAdmin = null)
        {
            try
            {

                string apiUrl = $"http://10.7.7.250:40/api/Export_CheckRequest_API?CheckRequest_Id={checkRequest_Id}&Committee_Id={Committee_ID}&Committee_Type_Id={Committee_Type_Id}";
                ViewBag.ISAdmin = ISAdmin;
                ViewBag.Committee_Id = Committee_ID;
                ViewBag.EmployeeId = EmployeeId;
                ViewBag.Committee_Type_Id = Committee_Type_Id;

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var model = System.Text.Json.JsonSerializer.Deserialize<Ex_CheckRequest_GetData_Android_V2_VM>(responseBody, options);

                return View("Index", model);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"API Error: {ex.Message}");
            }
        }

        [Route("/shohna_sader/SaveLots")]
        [HttpPost]
        public async Task<IActionResult> SaveLots([FromBody] List<LotDataModel> lots)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(errors);
            }

            if (lots == null || lots.Count == 0)
                return Json(new { success = false, message = "البيانات غير موجودة" });

            try
            {
                long employeeId = lots.FirstOrDefault().EmployeeId;
                double latitude = 0;
                double longitude = 0;
                var committeeId = lots.FirstOrDefault().Committee_ID;

                //update
                //using (DbContext context = new DbContext())
                //{
                //    using (System.Data.Entity.DbContextTransaction trans = context.Database.BeginTransaction())
                //    {



                ////update request status  Ex_request_Committe
                //Ex_RequestCommittee RequestCommittee =  Ex_RequestCommittee().fin(committeeId);
                //        RequestCommittee.Status = true;
                //        RequestCommittee.User_Updation_Id = (short)EmployeeId;
                //        RequestCommittee.User_Updation_Date = DateTime.Now;
                //        //fz IsFinishedAll 
                //        RequestCommittee.IsFinishedAll = IsFinishedAll;
                //        uow.Repository<Ex_RequestCommittee>().Update(RequestCommittee);
                //        uow.SaveChanges();
                ////    }
                ////}



                var groupedLots = lots
                    .GroupBy(l => l.Ex_Request_Item_Id)
                    .Select(group => new CommitteDto
                    {
                        Ex_Request_Item_Id = group.Key,
                        Committee_ID = committeeId,
                        EmployeeId = employeeId,

                        ComResult = group.Select(lot => new CommitteeResultDto
                        {
                            CommitteeResultType_ID = lot.CommitteeResultType_ID,
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            Ex_RequestLotData_ID = lot.LotData_ID,
                            Notes = lot.Notes,
                            QuantitySize = lot.QuantitySize,
                            Weight = lot.Weight,
                            OriginalWeight = lot.OriginalWeight,
                            Latitude = latitude,
                            Longitude = longitude,
                            EmployeeId = employeeId,
                            //Photos = new List<string>(),
                            //infectionData = new List<string>()
                        }).ToList()
                    }).ToList();

                var payload = new CommitteeRequestWrapper
                {
                    Committe_Dto = groupedLots,
                    SampleDto = new List<object>(),
                    Treatment_Dto = new List<object>()
                };

                var client = _httpClientFactory.CreateClient();
                var apiUrl = "http://10.7.7.250:40/API/Ex_CommitteeResult_API?IsFinishedAll=true";
                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, redirectUrl = Url.Action("index", "Home") });
                }
                else
                {
                    return Json(new { success = false, message = $"خطأ في استجابة الـ API: {responseString}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"خطأ غير متوقع: {ex.Message}" });
            }
        }
        //public async Task<IActionResult> SaveLots([FromBody] List<Ex_CheckRequest_GetData_Android_V2_VM> lots)
        //{ 
        //return View();  
        //}
        }

    public class LotDataModel
    {



        //public  bool ExportAbroad { get; set; } //rrrrrr
        public string _ExportAbroad { get; set; } //rrrrrr

        public long Committee_ID { get; set; }
        public long EmployeeId { get; set; }
        public long Ex_Request_Item_Id { get; set; }
        public long LotData_ID { get; set; }
        public decimal QuantitySize { get; set; }
        public decimal Weight { get; set; }
        public decimal OriginalWeight { get; set; }
        public int CommitteeResultType_ID { get; set; }
        public string Notes { get; set; }
        //public string Sample_Data { get; set; }

        public List<SampleDataModel> Sample_Data { get; set; }

    }
    public class SampleDataModel
    {
        public string Sample_dataId { get; set; }
        public string AnalysisType_Name { get; set; }
        public string AnalysisLab_Name { get; set; }
        public string SampleRatio { get; set; }
        public string SampleSize { get; set; }
        public string Sample_BarCode { get; set; }
    }

    public class CommitteeResultDto
    {
        public long EmployeeId { get; set; }

        public int CommitteeResultType_ID { get; set; }
        public string Date { get; set; }
        public long Ex_RequestLotData_ID { get; set; }
        public int ID { get; set; } = 0;
        public bool IS_Total_Android { get; set; } = false;
        public long Item_ID { get; set; } = 0;
        public long Item_ShortName_ID { get; set; } = 0;
        public long Item__OrderID { get; set; } = 0;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Notes { get; set; }
        public List<string> Photos { get; set; } = new();
        public decimal QuantitySize { get; set; }
        public int Result_injuryID { get; set; } = 0;
        public decimal Weight { get; set; }
        public decimal OriginalWeight { get; set; }
        public List<string> infectionData { get; set; } = new();
    }

    public class CommitteDto
    {
        public long Ex_Request_Item_Id { get; set; }
        public long Committee_ID { get; set; }
        public long EmployeeId { get; set; }
        public List<CommitteeResultDto> ComResult { get; set; }
    }

    public class CommitteeRequestWrapper
    {
        public List<CommitteDto> Committe_Dto { get; set; }
        public List<object> SampleDto { get; set; } = new();
        public List<object> Treatment_Dto { get; set; } = new();
    }
}
