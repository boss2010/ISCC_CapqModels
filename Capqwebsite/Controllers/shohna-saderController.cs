using EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            dbContext.Database.UseTransaction(transaction.GetDbTransaction());
            try
            {

                

                if ((lots.FirstOrDefault().Committee_Type_Id == 1) || (lots.FirstOrDefault().Committee_Type_Id == 2))
                {

                    foreach (var lot in lots)
                    {
                        //////////////////////////////////Ex_RequestCommittees/////////////////////////////////////////
                        if (lots.FirstOrDefault().ISAdmin == 1)
                        {
                            var committee_Id = lot.Committee_ID;
                        var updatedRow1 = dbContext.Ex_RequestCommittees.Where(a => a.ID == committee_Id).FirstOrDefault();
                        //var updatedRow1 = dbContext.Ex_RequestCommittees.Select(a => a.ID == committee_Id).FirstOrDefault();
                        updatedRow1.IsFinishedAll = true;
                        updatedRow1.Status = true;
                        updatedRow1.User_Updation_Date = DateTime.Now;
                        dbContext.SaveChanges();

                        
                            /////////////////////////update in Ex_CommitteeResults>>>>>admin///////////////////////////////////
                            var updatedRow2 = dbContext.Ex_CommitteeResults
          .Where(a => a.Committee_ID == lot.Committee_ID && a.LotData_ID == lot.LotData_ID && a.EmployeeId==lot.EmployeeId && a.Ex_Request_Item_Id== lot.Ex_Request_Item_Id)
          .FirstOrDefault();
                            updatedRow2.EmployeeId = lot.EmployeeId;
                            updatedRow2.CommitteeResultType_ID = lot.CommitteeResultType_ID;

                            updatedRow2.QuantitySize = lot.QuantitySize;
                            updatedRow2.Weight = lot.Weight;
                            updatedRow2.WeightOld = lot.OriginalWeight;
                            updatedRow2.Notes = lot.Notes;
                            updatedRow2.AdminFinalResult_Note = lot.Notes;
                            updatedRow2.User_Updation_Date = DateTime.Now;
                            updatedRow2.Date = DateTime.Now;

                        }
                        else
                        {
                            /////////////////////////insert in Ex_CommitteeResult_Confirm>>>>>مساعد////////Ex_CommitteeResult_Confirm_SEQ///////////////////////////


                            var Ex_CommitteeResult_Confirm_ID = GetSequenceFromTable("Ex_CommitteeResult_Confirm_SEQ");

                            var Ex_CommitteeResult_ConfirmnewRowObj = new Ex_CommitteeResult_Confirm();

                            Ex_CommitteeResult_ConfirmnewRowObj.ID = Ex_CommitteeResult_Confirm_ID;

                            Ex_CommitteeResult_ConfirmnewRowObj.Ex_CommitteeResult_ID = dbContext.Ex_CommitteeResults
          .Where(a => a.Committee_ID == lot.Committee_ID && a.LotData_ID == lot.LotData_ID && a.Ex_Request_Item_Id == lot.Ex_Request_Item_Id)
          .FirstOrDefault().ID;
                            Ex_CommitteeResult_ConfirmnewRowObj.Date = DateTime.Now;
                            Ex_CommitteeResult_ConfirmnewRowObj.EmployeeId = lots.FirstOrDefault().EmployeeId;

                            dbContext.Add(Ex_CommitteeResult_ConfirmnewRowObj);
                            dbContext.SaveChanges();

                        }
                    }
                }










                dbContext.SaveChanges();

                transaction.Commit();





                // جاشنى مقبول >>>>> CommitteeResultType_ID=1&&&& مرفوض CommitteeResultType_ID=3






                return Json(new { success = true, redirectUrl = Url.Action("index", "Home") });
            }
            catch (Exception ex)
            {

                // transaction.Rollback(); // Rollback FIRST ✅
                throw new Exception("rehab errorr", ex); // Then throw
            }
            return Json(new { success = false, });
        }
        public long GetSequenceFromTable(string seqName)
        {
            using var dbContext = new AgricultureDBContext();

            var nextVal = dbContext.Database
                .SqlQueryRaw<long>(
                    @"UPDATE Sequences
              SET LastValue = LastValue + 1
              OUTPUT INSERTED.LastValue
              WHERE SeqName = {0}", seqName)
                .AsEnumerable()
                .First();

            return nextVal;
        }
    }

    public class LotDataModel
    {
        
        public short? itemID { get; set; }
        public short? Committee_Type_Id { get; set; }
        public short? ISAdmin { get; set; }

        //public  bool ExportAbroad { get; set; } //rrrrrr
        public string _ExportAbroad { get; set; } //rrrrrr

        public long Committee_ID { get; set; }
        public long EmployeeId { get; set; }
        public long Ex_Request_Item_Id { get; set; }
        public long LotData_ID { get; set; }
        public double? QuantitySize { get; set; }
        public double? Weight { get; set; }
        public double? OriginalWeight { get; set; }
        public byte? CommitteeResultType_ID { get; set; }
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
