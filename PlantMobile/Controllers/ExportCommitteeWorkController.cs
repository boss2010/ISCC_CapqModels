using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ViewModels;

namespace PlantMobile.Controllers;

[Route("ExportRequests/Committees")]
public sealed class ExportCommitteeWorkController(IConfiguration configuration,ILogger<ExportCommitteeWorkController> logger):Controller
{
    private readonly string _connectionString=configuration.GetConnectionString("DBConnection")??throw new InvalidOperationException("DBConnection is not configured.");

    [HttpGet("{committeeId:long}")]
    public async Task<IActionResult> Index(long committeeId,CancellationToken ct)
    {
        if(!TryUser(out var userId))return RedirectToAction("Index","Login");
        var model=await LoadAsync(committeeId,userId,ct);return model is null?Forbid():View(model);
    }

    [HttpPost("Save")]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(25_000_000)]
    public async Task<IActionResult> Save([FromForm]ExportCommitteeSaveVm input,CancellationToken ct)
    {
        if(!TryUser(out var userId))return Unauthorized();
        await using var connection=new SqlConnection(_connectionString);await connection.OpenAsync(ct);
        var access=await ReadAccessAsync(connection,input.CommitteeId,userId,ct);if(access is null)return Forbid();
        var(type,isAdmin,isFinished)=access.Value;if(isFinished){TempData["WorkError"]="تم إنهاء اللجنة ولا يمكن تعديلها.";return RedirectToAction(nameof(Index),new{committeeId=input.CommitteeId});}
        if(type==6&&!isAdmin){TempData["WorkError"]="تسجيل المعالجة متاح لرئيس اللجنة فقط.";return RedirectToAction(nameof(Index),new{committeeId=input.CommitteeId});}
        await using var transaction=await connection.BeginTransactionAsync(ct);
        try
        {
            foreach(var row in input.Rows.Where(x=>x.Id>0))
            {
                if(type is 1 or 2)
                {
                    if(isAdmin)
                    {
                        const string update="""UPDATE dbo.Ex_CommitteeResult SET EmployeeId=@UserId,CommitteeResultType_ID=@ResultType,QuantitySize=@Quantity,Weight=@Weight,Notes=@Notes,AdminFinalResult_Note=@Notes,[Date]=GETDATE(),User_Updation_Id=@UserId,User_Updation_Date=GETDATE() WHERE ID=@Id AND Committee_ID=@CommitteeId AND User_Deletion_Id IS NULL;""";
                        await ExecuteAsync(connection,(SqlTransaction)transaction,update,row,input.CommitteeId,userId,ct);
                        if(row.InfectionPhoto is {Length:>0})
                        {
                            await using var stream=new MemoryStream();await row.InfectionPhoto.CopyToAsync(stream,ct);
                            const string photo="""INSERT dbo.A_AttachmentData_Ex_CommitteeResult_Infection(Id,Ex_CommitteeResult_id,Infection_Comment,AttachmentPath_Binary,User_Creation_Id,User_Creation_Date) VALUES(NEXT VALUE FOR dbo.A_AttachmentData_Ex_CommitteeResult_Infection_SEQ,@Id,@Notes,@Photo,@UserId,GETDATE());""";
                            await using var pc=new SqlCommand(photo,connection,(SqlTransaction)transaction);pc.Parameters.Add("@Id",SqlDbType.BigInt).Value=row.Id;pc.Parameters.Add("@Notes",SqlDbType.NVarChar,-1).Value=(object?)row.Notes??DBNull.Value;pc.Parameters.Add("@Photo",SqlDbType.VarBinary,-1).Value=stream.ToArray();pc.Parameters.Add("@UserId",SqlDbType.SmallInt).Value=userId;await pc.ExecuteNonQueryAsync(ct);
                        }
                    }
                    else
                    {
                        const string confirm="""IF NOT EXISTS(SELECT 1 FROM dbo.Ex_CommitteeResult_Confirm WHERE Ex_CommitteeResult_ID=@Id AND EmployeeId=@UserId) INSERT dbo.Ex_CommitteeResult_Confirm(ID,Ex_CommitteeResult_ID,[Date],EmployeeId,Notes,IsAccepted) VALUES(NEXT VALUE FOR dbo.Ex_CommitteeResult_Confirm_SEQ,@Id,GETDATE(),@UserId,@Notes,CASE WHEN @ResultType=1 THEN 1 WHEN @ResultType IS NULL THEN NULL ELSE 0 END);""";
                        await ExecuteAsync(connection,(SqlTransaction)transaction,confirm,row,input.CommitteeId,userId,ct);
                    }
                }
                else if(type==3)
                {
                    if(isAdmin)
                    {
                        const string sample="""UPDATE dbo.Ex_CheckRequest_SampleData SET WithdrawDate=@WithdrawDate,Sample_BarCode=@Barcode,SampleSize=@SampleSize,SampleRatio=@SampleRatio,IsAccepted=@Accepted,Notes_Ar=@Notes,Admin_Confirmation=1,Admin_User=@UserId,Admin_Date=GETDATE(),User_Updation_Id=@UserId,User_Updation_Date=GETDATE() WHERE ID=@Id AND Ex_RequestCommittee_ID=@CommitteeId AND User_Deletion_Id IS NULL;""";
                        await ExecuteAsync(connection,(SqlTransaction)transaction,sample,row,input.CommitteeId,userId,ct);
                    }
                    else
                    {
                        const string confirm="""IF NOT EXISTS(SELECT 1 FROM dbo.Ex_CheckRequest_SampleData_Confirm WHERE Ex_CheckRequest_SampleData_ID=@Id AND EmployeeId=@UserId) INSERT dbo.Ex_CheckRequest_SampleData_Confirm(ID,Ex_CheckRequest_SampleData_ID,[Date],EmployeeId,Notes,IsAccepted) VALUES(NEXT VALUE FOR dbo.Ex_CheckRequest_SampleData_Confirm_SEQ,@Id,GETDATE(),@UserId,@Notes,@Accepted);""";
                        await ExecuteAsync(connection,(SqlTransaction)transaction,confirm,row,input.CommitteeId,userId,ct);
                    }
                }
                else if(type==6)
                {
                    const string treatment="""UPDATE dbo.Ex_Request_TreatmentData SET Size=@TreatmentSize,TreatmentMat_Amount=@MaterialAmount,TheDose=@Dose,Exposure_Minute=@ExposureMinute,Exposure_Hour=@ExposureHour,Exposure_Day=@ExposureDay,Temperature=@Temperature,Procedures=@Procedures,Note=@Notes,User_Updation_Id=@UserId,User_Updation_Date=GETDATE() WHERE ID=@Id AND Ex_RequestCommittee_ID=@CommitteeId AND User_Deletion_Id IS NULL;""";
                    await ExecuteAsync(connection,(SqlTransaction)transaction,treatment,row,input.CommitteeId,userId,ct);
                }
            }
            if(isAdmin&&input.FinishCommittee)
            {
                await using var finish=new SqlCommand("UPDATE dbo.Ex_RequestCommittee SET IsFinishedAll=1,Status=1,User_Updation_Id=@UserId,User_Updation_Date=GETDATE() WHERE ID=@CommitteeId",connection,(SqlTransaction)transaction);finish.Parameters.Add("@UserId",SqlDbType.SmallInt).Value=userId;finish.Parameters.Add("@CommitteeId",SqlDbType.BigInt).Value=input.CommitteeId;await finish.ExecuteNonQueryAsync(ct);
            }
            await transaction.CommitAsync(ct);TempData["WorkSuccess"]=isAdmin?"تم حفظ بيانات اللجنة بنجاح.":"تم تسجيل رأيك على بيانات اللجنة.";
        }
        catch(Exception ex){await transaction.RollbackAsync(ct);logger.LogError(ex,"Failed to save export committee {CommitteeId}",input.CommitteeId);TempData["WorkError"]="تعذر حفظ بيانات اللجنة.";}
        return RedirectToAction(nameof(Index),new{committeeId=input.CommitteeId});
    }

    private async Task<ExportCommitteeWorkVm?> LoadAsync(long committeeId,short userId,CancellationToken ct)
    {
        await using var c=new SqlConnection(_connectionString);await c.OpenAsync(ct);var access=await ReadAccessAsync(c,committeeId,userId,ct);if(access is null)return null;
        const string head="""SELECT c.ExCheckRequest_ID,r.CheckRequest_Number,c.CommitteeType_ID,t.Name_Ar FROM dbo.Ex_RequestCommittee c JOIN dbo.Ex_CheckRequest r ON r.ID=c.ExCheckRequest_ID JOIN dbo.CommitteeType t ON t.ID=c.CommitteeType_ID WHERE c.ID=@Id AND c.User_Deletion_Id IS NULL;SELECT ID,Name_Ar FROM dbo.CommitteeResultType WHERE IsActive=1 AND User_Deletion_Id IS NULL ORDER BY ID;""";
        var model=new ExportCommitteeWorkVm{CommitteeId=committeeId,CommitteeTypeId=access.Value.Type,IsAdmin=access.Value.IsAdmin,IsFinished=access.Value.IsFinished};
        await using(var cmd=new SqlCommand(head,c)){cmd.Parameters.Add("@Id",SqlDbType.BigInt).Value=committeeId;await using var rd=await cmd.ExecuteReaderAsync(ct);if(!await rd.ReadAsync(ct))return null;model.RequestId=rd.GetInt64(0);model.RequestNumber=Convert.ToString(rd[1])??"";model.CommitteeTypeId=Convert.ToByte(rd[2]);model.CommitteeType=Convert.ToString(rd[3])??"";await rd.NextResultAsync(ct);while(await rd.ReadAsync(ct))model.ResultTypes.Add(new(){Id=Convert.ToByte(rd[0]),Name=Convert.ToString(rd[1])??""});}
        string rows=model.CommitteeTypeId is 1 or 2?InspectionSql:model.CommitteeTypeId==3?SampleSql:TreatmentSql;
        await using(var cmd=new SqlCommand(rows,c)){cmd.Parameters.Add("@Id",SqlDbType.BigInt).Value=committeeId;await using var rd=await cmd.ExecuteReaderAsync(ct);while(await rd.ReadAsync(ct))model.Rows.Add(ReadRow(rd,model.CommitteeTypeId));}
        return model;
    }

    private static readonly string InspectionSql="""SELECT x.ID,x.Ex_Request_Item_Id ItemId,x.LotData_ID LotId,s.ShortName_Ar ItemName,l.Lot_Number,l.Package_Count,l.Package_Based_Weight,x.CommitteeResultType_ID,x.QuantitySize,x.Weight,x.Notes FROM dbo.Ex_CommitteeResult x JOIN dbo.Ex_CheckRequest_Items i ON i.ID=x.Ex_Request_Item_Id LEFT JOIN dbo.Item_ShortName s ON s.ID=i.Item_ShortName_ID LEFT JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.ID=x.LotData_ID WHERE x.Committee_ID=@Id AND x.User_Deletion_Id IS NULL ORDER BY x.ID;""";
    private static readonly string SampleSql="""SELECT x.ID,x.Ex_Request_Item_Id ItemId,x.LotData_ID LotId,s.ShortName_Ar ItemName,l.Lot_Number,l.Package_Count,l.Package_Based_Weight,x.WithdrawDate,x.Sample_BarCode,x.SampleSize,x.SampleRatio,x.IsAccepted,x.Notes_Ar Notes,COALESCE(at.Name_Ar,N'') AnalysisName FROM dbo.Ex_CheckRequest_SampleData x JOIN dbo.Ex_CheckRequest_Items i ON i.ID=x.Ex_Request_Item_Id LEFT JOIN dbo.Item_ShortName s ON s.ID=i.Item_ShortName_ID LEFT JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.ID=x.LotData_ID LEFT JOIN dbo.AnalysisLabType alt ON alt.ID=x.AnalysisLabType_ID LEFT JOIN dbo.AnalysisType at ON at.ID=alt.AnalysisTypeID WHERE x.Ex_RequestCommittee_ID=@Id AND x.User_Deletion_Id IS NULL ORDER BY x.ID;""";
    private static readonly string TreatmentSql="""SELECT x.ID,x.Ex_Request_Item_Id ItemId,x.Ex_Request_LotData_ID LotId,s.ShortName_Ar ItemName,l.Lot_Number,l.Package_Count,l.Package_Based_Weight,x.Note Notes,COALESCE(tm.Ar_Name,N'') TreatmentMethod,x.Size,x.TreatmentMat_Amount,x.TheDose,x.Exposure_Minute,x.Exposure_Hour,x.Exposure_Day,x.Temperature,x.Procedures FROM dbo.Ex_Request_TreatmentData x JOIN dbo.Ex_CheckRequest_Items i ON i.ID=x.Ex_Request_Item_Id LEFT JOIN dbo.Item_ShortName s ON s.ID=i.Item_ShortName_ID LEFT JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.ID=x.Ex_Request_LotData_ID LEFT JOIN dbo.TreatmentMethods tm ON tm.ID=x.TreatmentMethod_ID WHERE x.Ex_RequestCommittee_ID=@Id AND x.User_Deletion_Id IS NULL ORDER BY x.ID;""";

    private static ExportCommitteeWorkRowVm ReadRow(SqlDataReader r,byte type)=>new(){Id=Convert.ToInt64(r["ID"]),ItemId=Convert.ToInt64(r["ItemId"]),LotId=r["LotId"] is DBNull?0:Convert.ToInt64(r["LotId"]),ItemName=Convert.ToString(r["ItemName"])??"",LotNumber=Convert.ToString(r["Lot_Number"])??"",PackageCount=r["Package_Count"] is DBNull?null:Convert.ToInt32(r["Package_Count"]),OriginalWeight=r["Package_Based_Weight"] is DBNull?null:Convert.ToDecimal(r["Package_Based_Weight"]),Notes=Convert.ToString(r["Notes"])??"",ResultTypeId=type is 1 or 2?(r["CommitteeResultType_ID"] is DBNull?null:Convert.ToByte(r["CommitteeResultType_ID"])):null,Quantity=type is 1 or 2?(r["QuantitySize"] is DBNull?null:Convert.ToDecimal(r["QuantitySize"])):null,Weight=type is 1 or 2?(r["Weight"] is DBNull?null:Convert.ToDecimal(r["Weight"])):null,WithdrawDate=type==3?(r["WithdrawDate"] is DBNull?null:Convert.ToDateTime(r["WithdrawDate"])):null,Barcode=type==3?Convert.ToString(r["Sample_BarCode"])??"":"",SampleSize=type==3?(r["SampleSize"] is DBNull?null:Convert.ToDouble(r["SampleSize"])):null,SampleRatio=type==3?(r["SampleRatio"] is DBNull?null:Convert.ToDouble(r["SampleRatio"])):null,IsAccepted=type==3?(r["IsAccepted"] is DBNull?null:Convert.ToBoolean(r["IsAccepted"])):null,AnalysisName=type==3?Convert.ToString(r["AnalysisName"])??"":"",TreatmentMethod=type==6?Convert.ToString(r["TreatmentMethod"])??"":"",TreatmentSize=type==6?(r["Size"] is DBNull?null:Convert.ToDecimal(r["Size"])):null,MaterialAmount=type==6?(r["TreatmentMat_Amount"] is DBNull?null:Convert.ToDecimal(r["TreatmentMat_Amount"])):null,Dose=type==6?(r["TheDose"] is DBNull?null:Convert.ToDecimal(r["TheDose"])):null,ExposureMinute=type==6?(r["Exposure_Minute"] is DBNull?null:Convert.ToInt32(r["Exposure_Minute"])):null,ExposureHour=type==6?(r["Exposure_Hour"] is DBNull?null:Convert.ToInt32(r["Exposure_Hour"])):null,ExposureDay=type==6?(r["Exposure_Day"] is DBNull?null:Convert.ToInt32(r["Exposure_Day"])):null,Temperature=type==6?(r["Temperature"] is DBNull?null:Convert.ToDecimal(r["Temperature"])):null,Procedures=type==6?Convert.ToString(r["Procedures"])??"":""};

    private static async Task<(byte Type,bool IsAdmin,bool IsFinished)?> ReadAccessAsync(SqlConnection c,long committeeId,short userId,CancellationToken ct){const string sql="""SELECT TOP(1) rc.CommitteeType_ID,CONVERT(bit,ISNULL(ce.ISAdmin,0)),CONVERT(bit,ISNULL(rc.IsFinishedAll,0)) FROM dbo.Ex_RequestCommittee rc JOIN dbo.CommitteeEmployee ce ON ce.Committee_ID=rc.ID AND ce.Employee_Id=@UserId AND ce.User_Deletion_Id IS NULL WHERE rc.ID=@Id AND rc.User_Deletion_Id IS NULL AND ISNULL(rc.Is_Cancel,0)=0;""";await using var cmd=new SqlCommand(sql,c);cmd.Parameters.Add("@Id",SqlDbType.BigInt).Value=committeeId;cmd.Parameters.Add("@UserId",SqlDbType.SmallInt).Value=userId;await using var r=await cmd.ExecuteReaderAsync(ct);return await r.ReadAsync(ct)?(Convert.ToByte(r[0]),Convert.ToBoolean(r[1]),Convert.ToBoolean(r[2])):null;}
    private static async Task ExecuteAsync(SqlConnection c,SqlTransaction tx,string sql,ExportCommitteeSaveRowVm row,long committeeId,short userId,CancellationToken ct){await using var cmd=new SqlCommand(sql,c,tx);cmd.Parameters.Add("@Id",SqlDbType.BigInt).Value=row.Id;cmd.Parameters.Add("@CommitteeId",SqlDbType.BigInt).Value=committeeId;cmd.Parameters.Add("@UserId",SqlDbType.SmallInt).Value=userId;cmd.Parameters.Add("@ResultType",SqlDbType.TinyInt).Value=(object?)row.ResultTypeId??DBNull.Value;cmd.Parameters.Add("@Quantity",SqlDbType.Decimal).Value=(object?)row.Quantity??DBNull.Value;cmd.Parameters.Add("@Weight",SqlDbType.Decimal).Value=(object?)row.Weight??DBNull.Value;cmd.Parameters.Add("@Notes",SqlDbType.NVarChar,-1).Value=(object?)row.Notes??DBNull.Value;cmd.Parameters.Add("@WithdrawDate",SqlDbType.Date).Value=(object?)row.WithdrawDate?.Date??DBNull.Value;cmd.Parameters.Add("@Barcode",SqlDbType.VarChar,50).Value=(object?)row.Barcode??DBNull.Value;cmd.Parameters.Add("@SampleSize",SqlDbType.Float).Value=(object?)row.SampleSize??DBNull.Value;cmd.Parameters.Add("@SampleRatio",SqlDbType.Float).Value=(object?)row.SampleRatio??DBNull.Value;cmd.Parameters.Add("@Accepted",SqlDbType.Bit).Value=(object?)row.IsAccepted??DBNull.Value;cmd.Parameters.Add("@TreatmentSize",SqlDbType.Decimal).Value=(object?)row.TreatmentSize??DBNull.Value;cmd.Parameters.Add("@MaterialAmount",SqlDbType.Decimal).Value=(object?)row.MaterialAmount??DBNull.Value;cmd.Parameters.Add("@Dose",SqlDbType.Decimal).Value=(object?)row.Dose??DBNull.Value;cmd.Parameters.Add("@ExposureMinute",SqlDbType.Int).Value=(object?)row.ExposureMinute??DBNull.Value;cmd.Parameters.Add("@ExposureHour",SqlDbType.Int).Value=(object?)row.ExposureHour??DBNull.Value;cmd.Parameters.Add("@ExposureDay",SqlDbType.Int).Value=(object?)row.ExposureDay??DBNull.Value;cmd.Parameters.Add("@Temperature",SqlDbType.Decimal).Value=(object?)row.Temperature??DBNull.Value;cmd.Parameters.Add("@Procedures",SqlDbType.NVarChar,-1).Value=(object?)row.Procedures??DBNull.Value;await cmd.ExecuteNonQueryAsync(ct);}
    private bool TryUser(out short id)=>short.TryParse(HttpContext.Session.GetString("UserId"),out id)&&HttpContext.Session.GetString("UserSession")=="Authenticated";
}

public sealed class ExportCommitteeSaveVm
{
    public long CommitteeId { get; set; }
    public bool FinishCommittee { get; set; }
    public List<ExportCommitteeSaveRowVm> Rows { get; set; }=[];
}

public sealed class ExportCommitteeSaveRowVm
{
    public long Id { get; set; }
    public byte? ResultTypeId { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? Weight { get; set; }
    public string? Notes { get; set; }
    public DateTime? WithdrawDate { get; set; }
    public string? Barcode { get; set; }
    public double? SampleSize { get; set; }
    public double? SampleRatio { get; set; }
    public bool? IsAccepted { get; set; }
    public decimal? TreatmentSize { get; set; }
    public decimal? MaterialAmount { get; set; }
    public decimal? Dose { get; set; }
    public int? ExposureMinute { get; set; }
    public int? ExposureHour { get; set; }
    public int? ExposureDay { get; set; }
    public decimal? Temperature { get; set; }
    public string? Procedures { get; set; }
    public IFormFile? InfectionPhoto { get; set; }
}
