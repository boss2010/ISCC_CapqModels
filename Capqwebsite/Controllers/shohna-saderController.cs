using EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            AgricultureDBContext dbContext = new AgricultureDBContext();
            using var transaction = dbContext.Database.BeginTransaction();
            try 
            {

           
            


                dbContext.Database.UseTransaction(transaction.GetDbTransaction());

                try
                {

                    foreach (var lot in lots)
            {
                //////////////////////////////////Ex_RequestCommittees/////////////////////////////////////////

                var committee_Id = lot.Committee_ID;
                var updatedRow1 = dbContext.Ex_RequestCommittees.Find(committee_Id);
                //var updatedRow1 = dbContext.Ex_RequestCommittees.Select(a => a.ID == committee_Id).FirstOrDefault();
                updatedRow1.IsFinishedAll = true;
                updatedRow1.Status = true;
                 
                    //////////////////////////////////Ex_CommitteeResult/////////////////////////////////////////
                    var updatedRow2 = dbContext.Ex_CommitteeResults.FirstOrDefault(a => a.Committee_ID == committee_Id);
                updatedRow2.Weight = (float)lot.Weight;
                updatedRow2.Notes = lot.Notes;
                updatedRow2.WeightOld = (float)lot.OriginalWeight;
                updatedRow2.Weight = (float)lot.Weight;
                updatedRow2.LotData_ID = lot.LotData_ID;
                updatedRow2.EmployeeId = lot.EmployeeId;
                updatedRow2.QuantitySize = (double)lot.QuantitySize;
                updatedRow2.User_Updation_Date = DateTime.Now;  

              
                }

                    dbContext.SaveChanges();

                    transaction.Commit();

                }
                catch (Exception ex2)
                {
                    transaction.Rollback();
                    
                }



                // جاشنى مقبول >>>>> CommitteeResultType_ID=1&&&& مرفوض CommitteeResultType_ID=3






                return Json(new { success = true, redirectUrl = Url.Action("index", "Home") });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    
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
