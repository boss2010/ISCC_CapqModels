using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ViewModels;

namespace PlantMobile.Controllers;

[Route("Im_Permissions/ListIm_Permission")]
public class ListIm_PermissionController : Controller
{
    private readonly string _connectionString;
    private readonly ILogger<ListIm_PermissionController> _logger;

    public ListIm_PermissionController(IConfiguration configuration, ILogger<ListIm_PermissionController> logger)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _logger = logger;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate, string? permissionNumber,
        long? countryId, long? companyId, long? shortNameId, int status = 5, int page = 1)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated") return RedirectToAction("Index", "Login");
        DateTime today = DateTime.Today;
        var model = new ListImPermissionFilterVm
        {
            FromDate=(fromDate??today.AddDays(-6)).Date, ToDate=(toDate??today).Date,
            PermissionNumber=permissionNumber?.Trim()??string.Empty, CountryId=countryId, CompanyId=companyId,
            ShortNameId=shortNameId, Status=NormalizeStatus(status), Page=Math.Max(1,page), PageSize=15
        };
        if(model.FromDate>model.ToDate) (model.FromDate,model.ToDate)=(model.ToDate,model.FromDate);
        try { model.Results=await LoadPermissionsAsync(model,HttpContext.RequestAborted); }
        catch(Exception ex){_logger.LogError(ex,"Failed to load import permissions; TraceId={TraceId}",HttpContext.TraceIdentifier);ViewBag.PageError=$"تعذر تحميل أذون الاستيراد. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}";}
        return View(model);
    }

    [HttpGet("Countries")]
    public Task<JsonResult> Countries(long? selectedId, CancellationToken ct) => LookupAsync("""
        SELECT ID, COALESCE(NULLIF(Ar_Name,N''),En_Name) FROM dbo.Country
        WHERE @SelectedId IS NULL OR ID=@SelectedId OR IsActive=1 ORDER BY Ar_Name
        """,string.Empty,selectedId,250,ct);

    [HttpGet("Companies")]
    public Task<JsonResult> Companies(string? q,long? selectedId,CancellationToken ct) => LookupAsync("""
        SELECT TOP (@Take) ID,COALESCE(NULLIF(Name_Ar,N''),Name_En) FROM dbo.Company_National
        WHERE (@SelectedId IS NOT NULL AND ID=@SelectedId) OR (@Query=N'' OR Name_Ar LIKE @Prefix OR Name_En LIKE @Prefix)
        ORDER BY CASE WHEN ID=@SelectedId THEN 0 ELSE 1 END,Name_Ar
        """,q,selectedId,50,ct);

    [HttpGet("Items")]
    public Task<JsonResult> Items(string? q,long? selectedId,CancellationToken ct) => LookupAsync("""
        SELECT TOP (@Take) Id,Text FROM (
          SELECT CONVERT(bigint,ID) Id,COALESCE(NULLIF(ShortName_Ar,N''),ShortName_En) Text FROM dbo.Item_ShortName
          UNION ALL SELECT CONVERT(bigint,ID),COALESCE(NULLIF(Name_Ar,N''),Name_En) FROM dbo.QualitativeGroup
        ) x WHERE (@SelectedId IS NOT NULL AND Id=@SelectedId) OR (@Query=N'' OR Text LIKE @Prefix)
        ORDER BY CASE WHEN Id=@SelectedId THEN 0 ELSE 1 END,Text
        """,q,selectedId,50,ct);

    private async Task<JsonResult> LookupAsync(string sql,string? query,long? selectedId,int take,CancellationToken ct)
    {
        var items=new List<PermissionLookupOptionVm>();
        await using var connection=new SqlConnection(_connectionString);await connection.OpenAsync(ct);
        await using var command=new SqlCommand(sql,connection){CommandTimeout=30};
        command.Parameters.Add("@Query",SqlDbType.NVarChar,200).Value=query?.Trim()??string.Empty;
        command.Parameters.Add("@Prefix",SqlDbType.NVarChar,201).Value=(query?.Trim()??string.Empty)+"%";
        command.Parameters.Add("@SelectedId",SqlDbType.BigInt).Value=selectedId??(object)DBNull.Value;
        command.Parameters.Add("@Take",SqlDbType.Int).Value=take;
        await using var reader=await command.ExecuteReaderAsync(ct);
        while(await reader.ReadAsync(ct)) items.Add(new(){Id=Convert.ToInt64(reader.GetValue(0)),Text=Convert.ToString(reader.GetValue(1))??string.Empty});
        return Json(items);
    }

    private async Task<List<ListImPermissionRowVm>> LoadPermissionsAsync(ListImPermissionFilterVm f,CancellationToken ct)
    {
        const string sql="""
        SET NOCOUNT ON;
        SELECT cc.ID PermissionId,rr.ID RequestDataId,cc.ImPermission_Number,cc.Arrival_Date,
               cc.Start_Date,cc.End_Date,cc.IsAcceppted,cc.IsPaid,cc.IS_Print_Ar,cc.IS_Print_EN,
               cc.Renewal_Status,rr.Im_OperationType,rr.ExportCountry_Id,rr.Importer_ID,rr.ImporterType_Id
        INTO #Base
        FROM dbo.Im_PermissionRequest cc
        INNER JOIN dbo.Im_RequestData rr ON rr.Im_PermissionRequest_ID=cc.ID
        WHERE cc.User_Deletion_Id IS NULL AND cc.Arrival_Date>=@FromDate AND cc.Arrival_Date<DATEADD(day,1,@ToDate)
          AND (@CountryId IS NULL OR rr.ExportCountry_Id=@CountryId)
          AND (@CompanyId IS NULL OR rr.Importer_ID=@CompanyId)
          AND (@Permission=N'' OR CONVERT(nvarchar(100),CONVERT(decimal(38,0),cc.ImPermission_Number)) LIKE N'%'+@Permission+N'%')
          AND (@ShortNameId IS NULL OR EXISTS(SELECT 1 FROM dbo.Im_PermissionItems pi INNER JOIN dbo.Im_Initiator i ON i.ID=pi.Im_Initiator_ID WHERE pi.Im_PermissionRequest_ID=cc.ID AND COALESCE(CONVERT(bigint,i.Item_ShortName_ID),CONVERT(bigint,i.QualitativeGroup_Id))=@ShortNameId));

        SELECT ROW_NUMBER() OVER(ORDER BY PermissionId DESC,RequestDataId DESC) RowNumber,* INTO #Filtered
        FROM #Base WHERE @Status=5 OR (@Status=2 AND IsAcceppted=1 AND IS_Print_Ar=1 AND IS_Print_EN=1)
          OR (@Status=3 AND IsAcceppted IS NULL AND IsPaid IS NULL) OR (@Status=4 AND IsAcceppted=0)
          OR (@Status=6 AND IsAcceppted=1 AND ISNULL(IsPaid,0)=0);

        SELECT COUNT(*) TotalResults,
          (SELECT COUNT(*) FROM #Base) AllCount,
          (SELECT COUNT(*) FROM #Base WHERE IsAcceppted=1 AND IS_Print_Ar=1 AND IS_Print_EN=1) ApprovedCount,
          (SELECT COUNT(*) FROM #Base WHERE IsAcceppted IS NULL AND IsPaid IS NULL) PendingCount,
          (SELECT COUNT(*) FROM #Base WHERE IsAcceppted=0) RejectedCount,
          (SELECT COUNT(*) FROM #Base WHERE IsAcceppted=1 AND ISNULL(IsPaid,0)=0) UnpaidCount
        FROM #Filtered;

        SELECT p.RowNumber,p.PermissionId,p.ImPermission_Number,p.Arrival_Date,p.Start_Date,p.End_Date,
          operation.Name_Ar operationTypeName,country.Ar_Name ExportCountryName,
          systemCode.ValueName ImporterTypeName,
          CASE p.ImporterType_Id WHEN 6 THEN company.Name_Ar WHEN 7 THEN organization.Name_Ar WHEN 8 THEN person.Name ELSE N'' END ImporterName,
          item.shortName,p.IsAcceppted,p.IsPaid,p.IS_Print_Ar,p.IS_Print_EN,p.Renewal_Status
        FROM #Filtered p
        INNER JOIN dbo.Im_OpertaionType operation ON operation.ID=p.Im_OperationType
        INNER JOIN dbo.Country country ON country.ID=p.ExportCountry_Id
        INNER JOIN dbo.A_SystemCode systemCode ON systemCode.Id=p.ImporterType_Id
        LEFT JOIN dbo.Company_National company ON company.ID=p.Importer_ID AND p.ImporterType_Id=6
        LEFT JOIN dbo.Public_Organization organization ON organization.ID=p.Importer_ID AND p.ImporterType_Id=7
        LEFT JOIN dbo.Person person ON person.ID=p.Importer_ID AND p.ImporterType_Id=8
        OUTER APPLY(SELECT TOP(1) CASE WHEN sn.ID IS NULL THEN q.Name_Ar ELSE sn.ShortName_Ar END shortName
          FROM dbo.Im_PermissionItems pi INNER JOIN dbo.Im_Initiator i ON i.ID=pi.Im_Initiator_ID
          LEFT JOIN dbo.Item_ShortName sn ON sn.ID=i.Item_ShortName_ID LEFT JOIN dbo.QualitativeGroup q ON q.ID=i.QualitativeGroup_Id
          WHERE pi.Im_PermissionRequest_ID=p.PermissionId ORDER BY pi.ID)item
        WHERE p.RowNumber BETWEEN ((@Page-1)*@PageSize)+1 AND @Page*@PageSize ORDER BY p.RowNumber OPTION(RECOMPILE);
        """;
        var rows=new List<ListImPermissionRowVm>();await using var connection=new SqlConnection(_connectionString);await connection.OpenAsync(ct);
        await using var command=new SqlCommand(sql,connection){CommandTimeout=60};
        command.Parameters.Add("@FromDate",SqlDbType.Date).Value=f.FromDate;command.Parameters.Add("@ToDate",SqlDbType.Date).Value=f.ToDate;
        command.Parameters.Add("@Permission",SqlDbType.NVarChar,100).Value=f.PermissionNumber;AddNullable(command,"@CountryId",f.CountryId);AddNullable(command,"@CompanyId",f.CompanyId);AddNullable(command,"@ShortNameId",f.ShortNameId);
        command.Parameters.Add("@Status",SqlDbType.Int).Value=f.Status;command.Parameters.Add("@Page",SqlDbType.Int).Value=f.Page;command.Parameters.Add("@PageSize",SqlDbType.Int).Value=f.PageSize;
        await using var reader=await command.ExecuteReaderAsync(ct);if(await reader.ReadAsync(ct)){f.TotalCount=reader.GetInt32(0);f.AllCount=reader.GetInt32(1);f.ApprovedCount=reader.GetInt32(2);f.PendingCount=reader.GetInt32(3);f.RejectedCount=reader.GetInt32(4);f.UnpaidCount=reader.GetInt32(5);}await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) rows.Add(new(){RowNumber=Convert.ToInt64(reader["RowNumber"]),Id=Convert.ToInt64(reader["PermissionId"]),PermissionNumber=DbString(reader,"ImPermission_Number").Split('.')[0],ArrivalDate=Convert.ToDateTime(reader["Arrival_Date"]),StartDate=DbDate(reader,"Start_Date"),EndDate=DbDate(reader,"End_Date"),OperationType=DbString(reader,"operationTypeName"),Country=DbString(reader,"ExportCountryName"),ImporterType=DbString(reader,"ImporterTypeName"),Company=DbString(reader,"ImporterName"),ShortName=DbString(reader,"shortName"),IsAccepted=DbBool(reader,"IsAcceppted"),IsPaid=DbBool(reader,"IsPaid"),IsPrintArabic=DbBool(reader,"IS_Print_Ar"),IsPrintEnglish=DbBool(reader,"IS_Print_EN"),RenewalStatus=DbByte(reader,"Renewal_Status")});
        return rows;
    }
    private static void AddNullable(SqlCommand c,string name,long? value)=>c.Parameters.Add(name,SqlDbType.BigInt).Value=value??(object)DBNull.Value;
    private static int NormalizeStatus(int s)=>s is 2 or 3 or 4 or 5 or 6?s:5;
    private static string DbString(SqlDataReader r,string n)=>r[n] is DBNull?string.Empty:Convert.ToString(r[n])??string.Empty;
    private static DateTime? DbDate(SqlDataReader r,string n)=>r[n] is DBNull?null:Convert.ToDateTime(r[n]);
    private static bool? DbBool(SqlDataReader r,string n)=>r[n] is DBNull?null:Convert.ToBoolean(r[n]);
    private static byte? DbByte(SqlDataReader r,string n)=>r[n] is DBNull?null:Convert.ToByte(r[n]);
}
