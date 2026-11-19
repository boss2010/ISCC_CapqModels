using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ViewModels;
using PlantMobile.Data;
using PlantMobile.Infrastructure;

namespace PlantMobile.Controllers;

[Route("Export_CheckRequest/List_EXCheckRequest")]
public class ListEXCheckRequestController : Controller
{
    private readonly string _connectionString;
    private readonly ILogger<ListEXCheckRequestController> _logger;
    private readonly MenuAccessReader _menuAccess;
    private readonly IConfiguration _configuration;
    private readonly string _attachmentServer;

    public ListEXCheckRequestController(IConfiguration configuration, ILogger<ListEXCheckRequestController> logger, MenuAccessReader menuAccess)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _logger = logger;
        _menuAccess = menuAccess;
        _configuration=configuration;
        _attachmentServer=configuration["Path_NetworkShare"] is {Length:>0} share?@"\\"+share.Trim().Trim('\\','/'):string.Empty;
    }

    [HttpGet("Supervisor")]
    public async Task<IActionResult> Supervisor(string? requestNumber,DateTime? fromDate,DateTime? toDate,int status=0,int page=1,CancellationToken cancellationToken=default)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated")return RedirectToAction("Index","Login");
        if(!await HasSupervisorAccessAsync(cancellationToken))return Forbid();
        var model=new ExportInspectionRequestListVm{IsSupervisorReadOnly=true,RequestNumber=requestNumber?.Trim()??string.Empty,FromDate=fromDate?.Date,ToDate=toDate?.Date,Status=status is >=0 and <=3?status:0,Page=Math.Max(1,page)};
        if(model.FromDate.HasValue&&model.ToDate.HasValue&&model.FromDate>model.ToDate)(model.FromDate,model.ToDate)=(model.ToDate,model.FromDate);
        try{await LoadListAsync(model,0,false,cancellationToken);}catch(Exception ex){_logger.LogError(ex,"Failed to load export supervisor requests; TraceId={TraceId}",HttpContext.TraceIdentifier);ViewBag.PageError=$"تعذر تحميل طلبات الصادر. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}";}
        return View("Index",model);
    }

    [HttpGet("Supervisor/Details/{id:long}")]
    public async Task<IActionResult> SupervisorDetails(long id,CancellationToken cancellationToken)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated")return RedirectToAction("Index","Login");
        if(!await HasSupervisorAccessAsync(cancellationToken))return Forbid();
        var model=await LoadDetailsAsync(id,cancellationToken);if(model is null)return NotFound();model.IsSupervisorReadOnly=true;return View("Details",model);
    }

    [HttpGet("DelegateAttachment/{id:long}")]
    public async Task<IActionResult> DelegateAttachment(long id,CancellationToken ct)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated")return Unauthorized();
        await using var connection=new SqlConnection(_connectionString);await connection.OpenAsync(ct);
        const string sql="SELECT AttachmentPath_Binary,AttachmentPath,Attachment_TypeName,Attachment_Number FROM dbo.A_AttachmentData WHERE Id=@Id AND User_Deletion_Id IS NULL";
        await using var command=new SqlCommand(sql,connection);command.Parameters.Add("@Id",SqlDbType.BigInt).Value=id;
        await using var reader=await command.ExecuteReaderAsync(ct);if(!await reader.ReadAsync(ct))return NotFound();
        var path=reader.IsDBNull(1)?string.Empty:reader.GetString(1);var name=reader.IsDBNull(3)?string.Empty:reader.GetString(3);
        var contentType=ImportAttachmentFiles.ContentType(string.IsNullOrWhiteSpace(path)?name:path,reader.IsDBNull(2)?null:reader.GetString(2));
        if(!reader.IsDBNull(0)&&((byte[])reader[0]).Length>0)return File((byte[])reader[0],contentType);
        await reader.CloseAsync();if(Uri.TryCreate(path,UriKind.Absolute,out var uri)&&uri.Scheme is "http" or "https")return Redirect(path);
        if(string.IsNullOrWhiteSpace(path)||string.IsNullOrWhiteSpace(_attachmentServer))return NotFound();
        var resolved=ImportAttachmentFiles.ResolvePath(path,_attachmentServer);
        try{AttachmentShareConnection.Connect(resolved,_configuration);return File(await System.IO.File.ReadAllBytesAsync(resolved,ct),contentType);}
        catch(Exception ex)when(ex is IOException or UnauthorizedAccessException){_logger.LogError(ex,"Unable to open export delegate attachment {AttachmentId}",id);return NotFound("تعذر فتح صورة المندوب من السيرفر.");}
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(string? requestNumber, DateTime? fromDate, DateTime? toDate,
        int status = 0, int page = 1, long? stationId = null, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");
        if (!await HasOperationalAccessAsync(cancellationToken))
            return Forbid();

        var model = new ExportInspectionRequestListVm
        {
            RequestNumber = requestNumber?.Trim() ?? string.Empty,
            FromDate = fromDate?.Date,
            ToDate = toDate?.Date,
            Status = status is >= 0 and <= 3 ? status : 0,
            StationId = stationId,
            Page = Math.Max(1, page)
        };
        if (model.FromDate.HasValue && model.ToDate.HasValue && model.FromDate > model.ToDate)
            (model.FromDate, model.ToDate) = (model.ToDate, model.FromDate);

        short currentUserId=short.TryParse(HttpContext.Session.GetString("UserId"),out var listUserId)?listUserId:(short)0;
        try { await LoadListAsync(model, currentUserId, true, cancellationToken); }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load export inspection requests; TraceId={TraceId}", HttpContext.TraceIdentifier);
            ViewBag.PageError = $"تعذر تحميل طلبات الفحص. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}";
        }
        return View(model);
    }

    [HttpGet("Details/{id:long}")]
    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");
        if (!await HasOperationalAccessAsync(cancellationToken))
        {
            if (await HasSupervisorAccessAsync(cancellationToken))
                return RedirectToAction(nameof(SupervisorDetails), new { id });
            return Forbid();
        }
        short currentUserId=short.TryParse(HttpContext.Session.GetString("UserId"),out var detailsUserId)?detailsUserId:(short)0;
        if(!await CanAccessRequestAsync(id,currentUserId,cancellationToken))return Forbid();
        try
        {
            var model = await LoadDetailsAsync(id, cancellationToken);
            return model is null ? NotFound() : View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load export inspection request {RequestId}; TraceId={TraceId}", id, HttpContext.TraceIdentifier);
            TempData["Error"] = $"تعذر تحميل تفاصيل الطلب. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}";
            return RedirectToAction(nameof(Index));
        }
    }

    // Keeps links/bookmarks from the original export screen working in the new application.
    [HttpGet("/Export_CheckRequest/EX_CheckRequestDetails/Index")]
    public async Task<IActionResult> OriginalDetails(string? ImCheckRequest_Number, long? Ex_CheckRequest_ID,
        CancellationToken cancellationToken)
    {
        if (Ex_CheckRequest_ID.HasValue)
            return RedirectToAction(nameof(Details), new { id = Ex_CheckRequest_ID.Value });
        if (string.IsNullOrWhiteSpace(ImCheckRequest_Number))
            return RedirectToAction(nameof(Index));

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(
            "SELECT TOP(1) Ex_CheckRequest_ID FROM dbo.Ex_List WHERE ImCheckRequest_Number=@Number ORDER BY Ex_CheckRequest_ID DESC",
            connection);
        command.Parameters.Add("@Number", SqlDbType.NVarChar, 100).Value = ImCheckRequest_Number.Trim();
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is null or DBNull
            ? NotFound()
            : RedirectToAction(nameof(Details), new { id = Convert.ToInt64(result) });
    }

    private async Task LoadListAsync(ExportInspectionRequestListVm model, short userId, bool restrictToUser, CancellationToken ct)
    {
        const string sql = """
        SET NOCOUNT ON;
        CREATE TABLE #AllowedRequests(Ex_CheckRequest_ID bigint NOT NULL PRIMARY KEY);
        IF @RestrictToUser=1
        BEGIN
          INSERT #AllowedRequests(Ex_CheckRequest_ID)
          SELECT allowed.Ex_CheckRequest_ID
          FROM
          (
            SELECT p.Ex_CheckRequest_ID
            FROM dbo.Ex_CheckRequest_Places p
            INNER JOIN dbo.Station_Emp se ON se.Emp_Id=@UserId AND se.IsActive=1 AND se.User_Deletion_Id IS NULL
              AND (se.Date_From IS NULL OR se.Date_From<=CAST(GETDATE() AS date))
              AND (se.Date_To IS NULL OR se.Date_To>=CAST(GETDATE() AS date))
              AND se.Station_Id IN(p.Station_Examination_ID,p.Station_Genshi_ID)
            UNION
            SELECT p.Ex_CheckRequest_ID
            FROM dbPrivilage.dbo.PR_User currentUser
            INNER JOIN dbo.Outlet accessOutlet ON accessOutlet.ID_HR=currentUser.Outlet_ID
            INNER JOIN dbo.Ex_CheckRequest_Places p ON accessOutlet.ID IN(p.Outlet_Exmainiation_ID,p.Outlet_Genshi_ID)
            WHERE currentUser.Id=@UserId AND currentUser.Active=1
            UNION
            SELECT committee.ExCheckRequest_ID
            FROM dbo.CommitteeEmployee employee
            INNER JOIN dbo.Ex_RequestCommittee committee ON committee.ID=employee.Committee_ID AND committee.User_Deletion_Id IS NULL
            WHERE employee.Employee_Id=@UserId AND employee.OperationType=73 AND employee.User_Deletion_Id IS NULL
          ) allowed
          GROUP BY allowed.Ex_CheckRequest_ID;
        END;
        ELSE
          INSERT #AllowedRequests(Ex_CheckRequest_ID)
          SELECT ID FROM dbo.Ex_CheckRequest WHERE User_Deletion_Id IS NULL;
        SELECT ROW_NUMBER() OVER(ORDER BY request.ID DESC) RowNumber,
          request.ID Ex_CheckRequest_ID,request.CheckRequest_Number ImCheckRequest_Number,
          request.User_Creation_Date Creation_Date,country.Ar_Name ExportCountryName,
          systemCode.ValueName ImporterTypeName,
          CASE requestData.ImporterType_Id WHEN 6 THEN company.Name_Ar WHEN 7 THEN organization.Name_Ar WHEN 8 THEN person.Name ELSE N'' END ImporterName,
          COALESCE(examinationOutlet.Ar_Name,examinationCenterOutlet.Ar_Name) Outlet_Examination_Name,
          examinationStation.Ar_Name Station_Examination_Name,
          COALESCE(genshiOutlet.Ar_Name,genshiPortOutlet.Ar_Name) Outlet_Genshi_Name,
          genshiStation.Ar_Name Station_Genshi_Name,
          place.Station_Examination_ID,request.IsAccepted,request.IsPaid,
          ISNULL(finalResult.ResultCount,0) Closed_Request,ISNULL(finalResult.Ex_Final_Result_ID,0) Final_Result_ID,
          ISNULL(finalResult.Final_Result_Name,N'') Final_Result_Name
        INTO #Results
        FROM #AllowedRequests allowed
        INNER JOIN dbo.Ex_CheckRequest request ON request.ID=allowed.Ex_CheckRequest_ID
        OUTER APPLY(SELECT TOP(1) data.* FROM dbo.Ex_CheckRequest_Data data WHERE data.Ex_CheckRequest_ID=request.ID AND data.User_Deletion_Id IS NULL ORDER BY data.ID DESC) requestData
        LEFT JOIN dbo.Country country ON country.ID=requestData.ExportCountry_Id
        LEFT JOIN dbo.A_SystemCode systemCode ON systemCode.Id=requestData.ImporterType_Id
        LEFT JOIN dbo.Company_National company ON company.ID=requestData.Importer_ID AND requestData.ImporterType_Id=6
        LEFT JOIN dbo.Public_Organization organization ON organization.ID=requestData.Importer_ID AND requestData.ImporterType_Id=7
        LEFT JOIN dbo.Person person ON person.ID=requestData.Importer_ID AND requestData.ImporterType_Id=8
        OUTER APPLY(SELECT TOP(1) p.* FROM dbo.Ex_CheckRequest_Places p WHERE p.Ex_CheckRequest_ID=request.ID ORDER BY p.ID DESC) place
        LEFT JOIN dbo.Outlet examinationOutlet ON examinationOutlet.ID=place.Outlet_Exmainiation_ID
        LEFT JOIN dbo.Center examinationCenter ON examinationCenter.ID=place.Center_ID
        LEFT JOIN dbo.Outlet examinationCenterOutlet ON examinationCenterOutlet.ID=examinationCenter.Outlet_ID
        LEFT JOIN dbo.Station examinationStation ON examinationStation.ID=place.Station_Examination_ID
        LEFT JOIN dbo.Outlet genshiOutlet ON genshiOutlet.ID=place.Outlet_Genshi_ID
        LEFT JOIN dbo.Outlet genshiPortOutlet ON genshiPortOutlet.PortNational_ID=place.PortNational_ID
        LEFT JOIN dbo.Station genshiStation ON genshiStation.ID=place.Station_Genshi_ID
        OUTER APPLY
        (
          SELECT TOP(1) resultRow.Ex_Final_Result_ID,resultType.Ar_Name Final_Result_Name,
            COUNT(*) OVER() ResultCount
          FROM dbo.Ex_CheckRequest_Final_Result resultRow
          LEFT JOIN dbo.Ex_Final_Result resultType ON resultType.ID=resultRow.Ex_Final_Result_ID AND resultType.IsActive=1
          WHERE resultRow.Ex_CheckRequest_ID=request.ID AND resultRow.User_Deletion_Id IS NULL
          ORDER BY resultRow.ID DESC
        ) finalResult
        WHERE (@Number=N'' OR request.CheckRequest_Number LIKE N'%'+@Number+N'%')
          AND (@FromDate IS NULL OR request.User_Creation_Date>=@FromDate)
          AND (@ToDate IS NULL OR request.User_Creation_Date<DATEADD(day,1,@ToDate))
          AND (@StationId IS NULL OR place.Station_Examination_ID=@StationId)
          AND (@Status=0 OR (@Status=1 AND ISNULL(finalResult.Ex_Final_Result_ID,0)=0 AND ISNULL(finalResult.ResultCount,0)=0)
             OR (@Status=2 AND ISNULL(finalResult.Ex_Final_Result_ID,0)>0) OR (@Status=3 AND ISNULL(finalResult.ResultCount,0)>0));
        SELECT COUNT(*) FROM #Results;
        SELECT RowNumber,Ex_CheckRequest_ID,ImCheckRequest_Number,Creation_Date,ExportCountryName,
          ImporterTypeName,ImporterName,Outlet_Examination_Name,Station_Examination_Name,
          Outlet_Genshi_Name,Station_Genshi_Name,IsAccepted,IsPaid,ISNULL(Closed_Request,0) Closed_Request,
          ISNULL(Final_Result_ID,0) Final_Result_ID,Final_Result_Name
        FROM #Results WHERE RowNumber BETWEEN ((@Page-1)*@PageSize)+1 AND @Page*@PageSize
        ORDER BY RowNumber OPTION(RECOMPILE);
        """;
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 60 };
        command.Parameters.Add("@Number", SqlDbType.NVarChar, 100).Value = model.RequestNumber;
        command.Parameters.Add("@FromDate", SqlDbType.Date).Value = model.FromDate ?? (object)DBNull.Value;
        command.Parameters.Add("@ToDate", SqlDbType.Date).Value = model.ToDate ?? (object)DBNull.Value;
        command.Parameters.Add("@Status", SqlDbType.Int).Value = model.Status;
        command.Parameters.Add("@StationId",SqlDbType.BigInt).Value=model.StationId??(object)DBNull.Value;
        command.Parameters.Add("@UserId",SqlDbType.SmallInt).Value=userId;
        command.Parameters.Add("@RestrictToUser",SqlDbType.Bit).Value=restrictToUser;
        command.Parameters.Add("@Page", SqlDbType.Int).Value = model.Page;
        command.Parameters.Add("@PageSize", SqlDbType.Int).Value = model.PageSize;
        await using var reader = await command.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct)) model.TotalCount = reader.GetInt32(0);
        if (model.Page > model.TotalPages) model.Page = model.TotalPages;
        await reader.NextResultAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            model.Results.Add(new ExportInspectionRequestRowVm
            {
                RowNumber = Convert.ToInt64(reader["RowNumber"]), Id = Convert.ToInt64(reader["Ex_CheckRequest_ID"]),
                RequestNumber = Text(reader, "ImCheckRequest_Number"), CreationDate = Date(reader, "Creation_Date"),
                Country = Text(reader, "ExportCountryName"), ImporterType = Text(reader, "ImporterTypeName"),
                ImporterName = Text(reader, "ImporterName"), IsAccepted = Bool(reader, "IsAccepted"), IsPaid = Bool(reader, "IsPaid"),
                ExaminationPlace = JoinPlace(Text(reader, "Outlet_Examination_Name"), Text(reader, "Station_Examination_Name")),
                GenshiPlace = JoinPlace(Text(reader, "Outlet_Genshi_Name"), Text(reader, "Station_Genshi_Name")),
                ClosedRequest = Convert.ToInt32(reader["Closed_Request"]), FinalResultId = Convert.ToInt32(reader["Final_Result_ID"]),
                FinalResultName = Text(reader, "Final_Result_Name")
            });
        }
        await reader.CloseAsync();
        if(model.StationId.HasValue)
        {
            await using var stationCommand=new SqlCommand("SELECT COALESCE(NULLIF(Ar_Name,N''),En_Name) FROM dbo.Station WHERE ID=@Id",connection);
            stationCommand.Parameters.Add("@Id",SqlDbType.BigInt).Value=model.StationId.Value;
            model.StationName=Convert.ToString(await stationCommand.ExecuteScalarAsync(ct))??string.Empty;
        }
    }

    [HttpGet("Stations")]
    public async Task<IActionResult> Stations(string? search,int page=1,CancellationToken ct=default)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated")return RedirectToAction("Index","Login");
        var model=new ExportStationDashboardVm{Search=search?.Trim()??string.Empty,Page=Math.Max(1,page)};
        const string sql="""
        SET NOCOUNT ON;
        SELECT ROW_NUMBER() OVER(ORDER BY s.Ar_Name,s.ID) RowNumber,s.ID,s.Ar_Name,s.StationCode,g.Ar_Name Governorate
        INTO #Stations FROM dbo.Station s
        LEFT JOIN dbo.Governate g ON g.ID=s.Gov_Id
        WHERE s.IsActive=1 AND s.User_Deletion_Id IS NULL AND (@Search=N'' OR s.Ar_Name LIKE N'%'+@Search+N'%' OR s.StationCode LIKE N'%'+@Search+N'%')
        OPTION(RECOMPILE);
        SELECT COUNT(*) FROM #Stations;
        SELECT s.RowNumber,s.ID,s.Ar_Name,s.StationCode,s.Governorate,
          ISNULL(requests.RequestCount,0) RequestCount,ISNULL(committees.CommitteeCount,0) CommitteeCount,
          ISNULL(committees.InspectionCount,0) InspectionCount,ISNULL(committees.SamplingCount,0) SamplingCount,
          ISNULL(committees.TreatmentCount,0) TreatmentCount,ISNULL(engineers.CurrentEngineerCount,0) CurrentEngineerCount
        FROM #Stations s
        OUTER APPLY(SELECT COUNT(DISTINCT p.Ex_CheckRequest_ID) RequestCount FROM dbo.Ex_CheckRequest_Places p WHERE p.Station_Examination_ID=s.ID OR p.Station_Genshi_ID=s.ID) requests
        OUTER APPLY(SELECT COUNT(DISTINCT c.ID) CommitteeCount,
          COUNT(DISTINCT CASE WHEN c.CommitteeType_ID=1 THEN c.ID END) InspectionCount,
          COUNT(DISTINCT CASE WHEN c.CommitteeType_ID=3 THEN c.ID END) SamplingCount,
          COUNT(DISTINCT CASE WHEN c.CommitteeType_ID=6 THEN c.ID END) TreatmentCount
          FROM dbo.Ex_CheckRequest_Places p INNER JOIN dbo.Ex_RequestCommittee c ON c.ExCheckRequest_ID=p.Ex_CheckRequest_ID
          WHERE ((c.CommitteeType_ID=2 AND p.Station_Genshi_ID=s.ID) OR (c.CommitteeType_ID IN(1,3,6) AND p.Station_Examination_ID=s.ID)) AND c.User_Deletion_Id IS NULL) committees
        OUTER APPLY(SELECT COUNT(DISTINCT se.Emp_Id) CurrentEngineerCount FROM dbo.Station_Emp se WHERE se.Station_Id=s.ID AND se.IsActive=1 AND se.User_Deletion_Id IS NULL
          AND (se.Date_From IS NULL OR se.Date_From<=CAST(GETDATE() AS date)) AND (se.Date_To IS NULL OR se.Date_To>=CAST(GETDATE() AS date))) engineers
        WHERE s.RowNumber BETWEEN ((@Page-1)*@PageSize)+1 AND @Page*@PageSize ORDER BY s.RowNumber OPTION(RECOMPILE);
        """;
        await using var connection=new SqlConnection(_connectionString);await connection.OpenAsync(ct);await using var command=new SqlCommand(sql,connection){CommandTimeout=60};
        command.Parameters.Add("@Search",SqlDbType.NVarChar,200).Value=model.Search;command.Parameters.Add("@Page",SqlDbType.Int).Value=model.Page;command.Parameters.Add("@PageSize",SqlDbType.Int).Value=model.PageSize;
        await using var reader=await command.ExecuteReaderAsync(ct);if(await reader.ReadAsync(ct))model.TotalCount=reader.GetInt32(0);await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct))model.Stations.Add(new(){RowNumber=Convert.ToInt64(reader["RowNumber"]),StationId=Convert.ToInt64(reader["ID"]),StationName=Text(reader,"Ar_Name"),StationCode=Text(reader,"StationCode"),Governorate=Text(reader,"Governorate"),RequestCount=Convert.ToInt32(reader["RequestCount"]),CommitteeCount=Convert.ToInt32(reader["CommitteeCount"]),InspectionCount=Convert.ToInt32(reader["InspectionCount"]),SamplingCount=Convert.ToInt32(reader["SamplingCount"]),TreatmentCount=Convert.ToInt32(reader["TreatmentCount"]),CurrentEngineerCount=Convert.ToInt32(reader["CurrentEngineerCount"])});
        return View(model);
    }

    private async Task<ExportInspectionRequestDetailsVm?> LoadDetailsAsync(long id, CancellationToken ct)
    {
        const string sql = """
        SET NOCOUNT ON;
        SELECT TOP(1) r.ID,r.CheckRequest_Number,r.User_Creation_Date,r.ExportCompany,r.ExportCompanyAddress,
          r.NationalIDCompanyOwner,r.IsAccepted,r.IsPaid,r.Amount,r.Notes_Reject,
          country.Ar_Name ExportCountryName,systemCode.ValueName ImporterTypeName,
          CASE d.ImporterType_Id WHEN 6 THEN company.Name_Ar WHEN 7 THEN org.Name_Ar WHEN 8 THEN person.Name ELSE N'' END ImporterName,
          examinationOutlet.Ar_Name Outlet_Examination_Name,station.Ar_Name Station_Examination_Name,
          genshiOutlet.Ar_Name Outlet_Genshi_Name,genshiStation.Ar_Name Station_Genshi_Name,
          ISNULL(fr.ResultCount,0) Closed_Request,ISNULL(fr.Final_Result_Name,N'') Final_Result_Name,
          COALESCE(NULLIF(d.DelegateName,N''),delegateData.DelegateName) DelegateName,
          d.DelegateAddress,delegateData.NationalID DelegateNationalId,d.Ship_Name,
          COALESCE(NULLIF(sm.Ar_Name COLLATE DATABASE_DEFAULT,N''),NULLIF(international.Ar_Name COLLATE DATABASE_DEFAULT,N''),international.TransferMethod COLLATE DATABASE_DEFAULT) ShipmentMean,
          COALESCE(NULLIF(tm.Ar_Name COLLATE DATABASE_DEFAULT,N''),NULLIF(international.TransferMethod COLLATE DATABASE_DEFAULT,N''),international.Ar_Name COLLATE DATABASE_DEFAULT) TransportMean,
          COALESCE(extra.OwnerName,CASE d.ImporterType_Id WHEN 6 THEN company.Owner_Ar WHEN 7 THEN org.Person_Responsible_Name WHEN 8 THEN person.Name END) OwnerName,
          COALESCE(extra.OwnerAddress,CASE d.ImporterType_Id WHEN 6 THEN company.Address_Ar WHEN 7 THEN org.Address_Ar WHEN 8 THEN person.Address END) OwnerAddress,
          COALESCE(NULLIF(extra.ImportCompany,N''),NULLIF(d.Receiver_Name,N'')) ImportCompany,
          COALESCE(NULLIF(extra.ImporeterCompanyAddress,N''),NULLIF(d.Receiver_Address,N'')) ImporeterCompanyAddress,
          COALESCE(NULLIF(d.Receiver_Name,N''),NULLIF(extra.Reciever_Name,N'')) Reciever_Name,
          delegateAttachment.Id DelegateAttachmentId,delegateAttachment.AttachmentPath DelegateAttachmentPath,
          shippingPort.Name_Ar ShippingPort,arrivalType.Name_Ar ArrivalPortType,arrivalPort.Name_Ar ArrivalPort,
          transitCountry.Ar_Name TransitCountry,transitType.Name_Ar TransitPortType,transitPort.Name_Ar TransitPort,
          place.Examination_location,gov.Ar_Name ExaminationGovernorate,center.Ar_Name ExaminationCenter,
          place.Station_Examination_ID,station.Ar_Name ExaminationStationName,place.Outlet_Exmainiation_ID,
          place.Station_Genshi_ID,genshiStation.Ar_Name GenshiStationName,place.Outlet_Genshi_ID,
          fr.Date FinalResultDate
        FROM dbo.Ex_CheckRequest r
        OUTER APPLY(SELECT TOP(1) requestData.* FROM dbo.Ex_CheckRequest_Data requestData WHERE requestData.Ex_CheckRequest_ID=r.ID AND requestData.User_Deletion_Id IS NULL ORDER BY requestData.ID DESC) d
        LEFT JOIN dbo.Country country ON country.ID=d.ExportCountry_Id
        LEFT JOIN dbo.A_SystemCode systemCode ON systemCode.Id=d.ImporterType_Id
        LEFT JOIN dbo.Shipment_Mean sm ON sm.ID=d.Shipment_Mean_Id
        LEFT JOIN dbo.Transport_Mean tm ON tm.ID=d.Transport_Mean_Id
        LEFT JOIN dbo.InternationalTransportation international ON international.ID=d.InternationalTransportation_ID
        LEFT JOIN dbo.Ex_CheckRequestData_Extra extra ON extra.Ex_CheckRequest_Data_ID=d.ID AND extra.User_Deletion_Id IS NULL
        LEFT JOIN dbo.Company_National company ON company.ID=d.Importer_ID AND d.ImporterType_Id=6
        LEFT JOIN dbo.Public_Organization org ON org.ID=d.Importer_ID AND d.ImporterType_Id=7
        LEFT JOIN dbo.Person person ON person.ID=d.Importer_ID AND d.ImporterType_Id=8
        OUTER APPLY(SELECT TOP(1) x.ID,x.DelegateName,x.NationalID FROM dbo.Delegates x
          WHERE x.EntityTypeID=d.ImporterType_Id AND x.EntityID=d.Importer_ID AND x.IsActive=1 AND x.User_Deletion_Date IS NULL
          ORDER BY CASE WHEN LTRIM(RTRIM(x.DelegateName))=LTRIM(RTRIM(d.DelegateName)) THEN 0 ELSE 1 END,x.ID DESC) delegateData
        OUTER APPLY(SELECT TOP(1) a.Id,a.AttachmentPath FROM dbo.A_AttachmentData a
          WHERE a.RowId=d.Importer_ID AND a.User_Deletion_Id IS NULL
            AND a.A_AttachmentTableNameId=CASE d.ImporterType_Id WHEN 6 THEN 1 WHEN 7 THEN 7 WHEN 8 THEN 6 ELSE -1 END
            AND (a.Attachment_TypeName LIKE N'%صورة%' OR a.Attachment_TypeName LIKE N'%بطاق%' OR a.AttachmentPath LIKE '%.jpg' OR a.AttachmentPath LIKE '%.jpeg' OR a.AttachmentPath LIKE '%.png')
          ORDER BY CASE WHEN a.Attachment_TypeName LIKE N'%صورة بطاقة%' THEN 0 ELSE 1 END,a.Id DESC) delegateAttachment
        LEFT JOIN dbo.Ex_CheckRequest_Places place ON place.Ex_CheckRequest_ID=r.ID
        LEFT JOIN dbo.Outlet examinationOutlet ON examinationOutlet.ID=place.Outlet_Exmainiation_ID
        LEFT JOIN dbo.Outlet genshiOutlet ON genshiOutlet.ID=place.Outlet_Genshi_ID
        LEFT JOIN dbo.Governate gov ON gov.ID=place.Govern_ID
        LEFT JOIN dbo.Center center ON center.ID=place.Center_ID
        LEFT JOIN dbo.Station station ON station.ID=place.Station_Examination_ID
        LEFT JOIN dbo.Station genshiStation ON genshiStation.ID=place.Station_Genshi_ID
        LEFT JOIN dbo.Country transitCountry ON transitCountry.ID=d.TransitCountry_Id
        OUTER APPLY(SELECT TOP(1) pn.Name_Ar FROM dbo.Ex_CheckRequest_Port ep LEFT JOIN dbo.PortNational pn ON pn.ID=ep.Port_ID WHERE ep.Ex_CheckRequest_Data_ID=d.ID AND ep.ReqPortType_ID=9 AND ep.User_Deletion_Id IS NULL ORDER BY ep.ID DESC) shippingPort
        OUTER APPLY(SELECT TOP(1) pi.Name_Ar,pi.PortTypeID FROM dbo.Ex_CheckRequest_Port ep LEFT JOIN dbo.Port_International pi ON pi.ID=ep.Port_ID WHERE ep.Ex_CheckRequest_Data_ID=d.ID AND ep.ReqPortType_ID=10 AND ep.User_Deletion_Id IS NULL ORDER BY ep.ID DESC) arrivalPort
        LEFT JOIN dbo.Port_Type arrivalType ON arrivalType.ID=arrivalPort.PortTypeID
        OUTER APPLY(SELECT TOP(1) pi.Name_Ar,pi.PortTypeID FROM dbo.Ex_CheckRequest_Port ep LEFT JOIN dbo.Port_International pi ON pi.ID=ep.Port_ID WHERE ep.Ex_CheckRequest_Data_ID=d.ID AND ep.ReqPortType_ID=11 AND ep.User_Deletion_Id IS NULL ORDER BY ep.ID DESC) transitPort
        LEFT JOIN dbo.Port_Type transitType ON transitType.ID=transitPort.PortTypeID
        OUTER APPLY(SELECT TOP(1) x.Date,resultType.Ar_Name Final_Result_Name,COUNT(*) OVER() ResultCount FROM dbo.Ex_CheckRequest_Final_Result x LEFT JOIN dbo.Ex_Final_Result resultType ON resultType.ID=x.Ex_Final_Result_ID WHERE x.Ex_CheckRequest_ID=r.ID AND x.User_Deletion_Id IS NULL ORDER BY x.ID DESC) fr
        WHERE r.ID=@Id OPTION(RECOMPILE);

        SELECT i.ID,COALESCE(sn.ShortName_Ar,sn.ShortName_En,N'') ShortName,
          COALESCE(ic.Name_Ar,ic.Name_En,N'') Category,COALESCE(ig.Name_Ar,ig.Name_En,N'') CategoryGroup,
          i.GrossWeight,i.Net_Weight,i.Package_Count,COALESCE(pt.Ar_Name,pt.En_Name,N'') PackageType,
          COALESCE(pm.Ar_Name,pm.En_Name,N'') PackageMaterial,i.IsAccepted,fd.FarmCode_14 FarmCode,fd.Name_Ar FarmName
        FROM dbo.Ex_CheckRequest_Items i
        LEFT JOIN dbo.Item_ShortName sn ON sn.ID=i.Item_ShortName_ID
        LEFT JOIN dbo.ItemCategories ic ON ic.ID=i.ItemCategory_ID
        LEFT JOIN dbo.ItemCategories_Group ig ON ig.ID=i.ItemCategories_Group_ID
        LEFT JOIN dbo.Package_Type pt ON pt.ID=i.Package_Type_ID
        LEFT JOIN dbo.Package_Material pm ON pm.ID=i.Package_Material_ID
        LEFT JOIN dbo.FarmsData fd ON fd.ID=i.FarmsData_ID
        WHERE i.Ex_CheckRequest_ID=@Id AND i.User_Deletion_Id IS NULL ORDER BY i.ID OPTION(RECOMPILE);

        SELECT lot.ID,lot.Ex_CheckRequest_Items_ID,lot.Lot_Number,lot.Package_Count,lot.GrossWeight,lot.Net_Weight,
          COALESCE(pt.Ar_Name,pt.En_Name,N'') PackageType,COALESCE(pm.Ar_Name,pm.En_Name,N'') PackageMaterial,
          lot.Grower_Number,lot.Waybill,lot.IsAccepted,lot.RejectReason,lot.Package_Based_Weight,lot.Package_Net_Weight
        FROM dbo.Ex_CheckRequest_Items_Lot_Category lot
        INNER JOIN dbo.Ex_CheckRequest_Items i ON i.ID=lot.Ex_CheckRequest_Items_ID
        LEFT JOIN dbo.Package_Type pt ON pt.ID=lot.Package_Type_ID
        LEFT JOIN dbo.Package_Material pm ON pm.ID=lot.Package_Material_ID
        WHERE i.Ex_CheckRequest_ID=@Id AND lot.User_Deletion_Id IS NULL ORDER BY lot.ID OPTION(RECOMPILE);

        SELECT a.Id,COALESCE(NULLIF(a.Attachment_TypeName,N''),t.Ar_Name,N'مرفق') AttachmentName,
          a.Attachment_Number,a.AttachmentPath,a.StartDate,a.EndDate
        FROM dbo.A_AttachmentData_Ex_CheckRequest a
        LEFT JOIN dbo.A_AttachmentTableType t ON t.ID=a.A_AttachmentTableType_ID
        WHERE a.Ex_CheckRequest_ID=@Id AND a.User_Deletion_Id IS NULL ORDER BY a.Id OPTION(RECOMPILE);

        SELECT N'رسوم طلب الفحص الصادر' FeeType,N'الطلب' FeeSource,CAST(NULL AS nvarchar(100)) OrderNumber,
          r.IsPaid,fee.Total_GrossWeight,fee.Fee_Value,fee.Total_Amount,fee.is_Fee_Rounding,fee.ID SortId
        FROM dbo.Ex_CheckRequest_Fees fee INNER JOIN dbo.Ex_CheckRequest r ON r.ID=fee.Ex_CheckRequest_ID
        WHERE fee.Ex_CheckRequest_ID=@Id
        UNION ALL
        SELECT COALESCE(NULLIF(action.Name_Ar COLLATE DATABASE_DEFAULT,N''),NULLIF(feeType.Name_Ar COLLATE DATABASE_DEFAULT,N''),N'رسوم لجنة صادر'),
          COALESCE(NULLIF(committeeType.Name_Ar COLLATE DATABASE_DEFAULT,N''),N'لجنة صادر'),transactionData.OrderNumber COLLATE DATABASE_DEFAULT,
          transactionData.IsSuccess_Bank,CAST(NULL AS decimal(18,6)),CAST(NULL AS money),
          COALESCE(detail.Amount,transactionData.Amount_Total),CAST(NULL AS bit),transactionData.ID
        FROM dbo.Fees_Transactions transactionData
        LEFT JOIN dbo.Fees_Transactions_Detiles detail ON detail.Fees_Transactions_ID=transactionData.ID AND detail.User_Deletion_Id IS NULL
        LEFT JOIN dbo.Fees_Action action ON action.ID=detail.Fees_Action_ID
        LEFT JOIN dbo.FeesType feeType ON feeType.ID=action.FeesType_Id
        LEFT JOIN dbo.Ex_RequestCommittee committee ON committee.ID=transactionData.Table_ID AND transactionData.TableName_ID=9
        LEFT JOIN dbo.CommitteeType committeeType ON committeeType.ID=committee.CommitteeType_ID
        WHERE transactionData.User_Deletion_Id IS NULL
          AND ((transactionData.TableName_ID=8 AND transactionData.Table_ID=@Id)
            OR (transactionData.TableName_ID=9 AND committee.ExCheckRequest_ID=@Id))
        ORDER BY SortId OPTION(RECOMPILE);

        SELECT DISTINCT employees.Id,employees.EmpId,employees.FullName,employees.Date_From,employees.Date_To FROM(
          SELECT u.Id,u.EmpId,u.FullName,se.Date_From,se.Date_To
          FROM dbo.Ex_CheckRequest_Places p
          INNER JOIN dbo.Station_Emp se ON se.Station_Id=p.Station_Examination_ID AND se.IsActive=1 AND se.User_Deletion_Id IS NULL
          INNER JOIN dbPrivilage.dbo.PR_User u ON u.Id=se.Emp_Id AND u.Active=1
          WHERE p.Ex_CheckRequest_ID=@Id AND p.Station_Examination_ID IS NOT NULL
          UNION ALL
          SELECT u.Id,u.EmpId,u.FullName,CAST(NULL AS date) Date_From,CAST(NULL AS date) Date_To
          FROM dbo.Ex_CheckRequest_Places p INNER JOIN dbo.Outlet o ON o.ID=p.Outlet_Exmainiation_ID
          INNER JOIN dbPrivilage.dbo.PR_User u ON u.Outlet_ID=o.ID_HR AND u.Active=1
          WHERE p.Ex_CheckRequest_ID=@Id AND p.Station_Examination_ID IS NULL AND p.Outlet_Exmainiation_ID IS NOT NULL
        )employees ORDER BY employees.FullName OPTION(RECOMPILE);

        SELECT DISTINCT employees.Id,employees.EmpId,employees.FullName,employees.Date_From,employees.Date_To FROM(
          SELECT u.Id,u.EmpId,u.FullName,se.Date_From,se.Date_To
          FROM dbo.Ex_CheckRequest_Places p
          INNER JOIN dbo.Station_Emp se ON se.Station_Id=p.Station_Genshi_ID AND se.IsActive=1 AND se.User_Deletion_Id IS NULL
          INNER JOIN dbPrivilage.dbo.PR_User u ON u.Id=se.Emp_Id AND u.Active=1
          WHERE p.Ex_CheckRequest_ID=@Id AND p.Station_Genshi_ID IS NOT NULL
          UNION ALL
          SELECT u.Id,u.EmpId,u.FullName,CAST(NULL AS date) Date_From,CAST(NULL AS date) Date_To
          FROM dbo.Ex_CheckRequest_Places p INNER JOIN dbo.Outlet o ON o.ID=p.Outlet_Genshi_ID
          INNER JOIN dbPrivilage.dbo.PR_User u ON u.Outlet_ID=o.ID_HR AND u.Active=1
          WHERE p.Ex_CheckRequest_ID=@Id AND p.Station_Genshi_ID IS NULL AND p.Outlet_Genshi_ID IS NOT NULL
        )employees ORDER BY employees.FullName OPTION(RECOMPILE);

        SELECT DISTINCT o.ID,o.Ar_Name
        FROM dbo.Outlet o
        INNER JOIN dbPrivilage.dbo.PR_User u ON u.Outlet_ID=o.ID_HR AND u.Active=1
        WHERE o.IsActive=1 AND o.User_Deletion_Id IS NULL
        ORDER BY o.Ar_Name;

        SELECT DISTINCT u.Id,u.EmpId,u.FullName,o.ID OutletId,o.Ar_Name OutletName
        FROM dbo.Outlet o
        INNER JOIN dbPrivilage.dbo.PR_User u ON u.Outlet_ID=o.ID_HR AND u.Active=1
        WHERE o.IsActive=1 AND o.User_Deletion_Id IS NULL
        ORDER BY o.Ar_Name,u.FullName;

        SELECT alt.ID,at.Name_Ar AnalysisName,al.Name_Ar LabName
        FROM dbo.AnalysisLabType alt
        INNER JOIN dbo.AnalysisType at ON at.ID=alt.AnalysisTypeID AND at.IsActive=1
        INNER JOIN dbo.AnalysisLab al ON al.ID=alt.AnalysisLabID AND al.IsActive=1
        WHERE alt.User_Deletion_Id IS NULL AND at.User_Deletion_Id IS NULL AND al.User_Deletion_Id IS NULL
        ORDER BY at.Name_Ar,al.Name_Ar;

        SELECT ID,Name_Ar,CONVERT(decimal(18,2),ISNULL([count],0)) Amount
        FROM dbo.ShiftTiming
        WHERE IsActive=1 AND User_Deletion_Id IS NULL
        ORDER BY ID;

        SELECT c.ID,CONVERT(tinyint,c.CommitteeType_ID) CommitteeType_ID,ct.Name_Ar,c.Delegation_Date,
          STRING_AGG(CONVERT(nvarchar(max),u.FullName),N'، ') Employees,
          CASE WHEN c.CommitteeType_ID IN(1,2) THEN (SELECT COUNT(*) FROM dbo.Ex_CommitteeResult r WHERE r.Committee_ID=c.ID AND r.User_Deletion_Id IS NULL)
            WHEN c.CommitteeType_ID=3 THEN (SELECT COUNT(*) FROM dbo.Ex_CheckRequest_SampleData s WHERE s.Ex_RequestCommittee_ID=c.ID AND s.User_Deletion_Id IS NULL)
            WHEN c.CommitteeType_ID=6 THEN (SELECT COUNT(*) FROM dbo.Ex_Request_TreatmentData x WHERE x.Ex_RequestCommittee_ID=c.ID AND x.User_Deletion_Id IS NULL) ELSE 0 END LotCount
        FROM dbo.Ex_RequestCommittee c INNER JOIN dbo.CommitteeType ct ON ct.ID=c.CommitteeType_ID
        LEFT JOIN dbo.CommitteeEmployee ce ON ce.Committee_ID=c.ID AND ce.OperationType=73 AND ce.User_Deletion_Id IS NULL
        LEFT JOIN dbPrivilage.dbo.PR_User u ON u.Id=ce.Employee_Id
        WHERE c.ExCheckRequest_ID=@Id AND c.CommitteeType_ID IN(1,2,3,6) AND c.User_Deletion_Id IS NULL
        GROUP BY c.ID,c.CommitteeType_ID,ct.Name_Ar,c.Delegation_Date ORDER BY c.ID DESC OPTION(RECOMPILE);
        """;
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 60 };
        command.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;
        await using var reader = await command.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct)) return null;
        var model = new ExportInspectionRequestDetailsVm
        {
            Id=id, RequestNumber=Text(reader,"CheckRequest_Number"), CreationDate=Date(reader,"User_Creation_Date"),
            ExportCompany=Text(reader,"ExportCompany"), ExportCompanyAddress=Text(reader,"ExportCompanyAddress"),
            NationalId=Text(reader,"NationalIDCompanyOwner"), IsAccepted=Bool(reader,"IsAccepted"), IsPaid=Bool(reader,"IsPaid"),
            Amount=Decimal(reader,"Amount"), RejectNotes=Text(reader,"Notes_Reject"), Country=Text(reader,"ExportCountryName"),
            ImporterType=Text(reader,"ImporterTypeName"), ImporterName=Text(reader,"ImporterName"),
            OwnerName=Text(reader,"OwnerName"),OwnerAddress=Text(reader,"OwnerAddress"),
            ExaminationPlace=JoinPlace(Text(reader,"Outlet_Examination_Name"),Text(reader,"Station_Examination_Name")),
            GenshiPlace=JoinPlace(Text(reader,"Outlet_Genshi_Name"),Text(reader,"Station_Genshi_Name")),
            DelegateName=Text(reader,"DelegateName"),DelegateAddress=Text(reader,"DelegateAddress"),DelegateNationalId=Text(reader,"DelegateNationalId"),
            DelegateAttachmentId=reader["DelegateAttachmentId"] is DBNull?null:Convert.ToInt64(reader["DelegateAttachmentId"]),DelegateAttachmentPath=Text(reader,"DelegateAttachmentPath"),ShipName=Text(reader,"Ship_Name"),
            ShipmentMean=Text(reader,"ShipmentMean"),TransportMean=Text(reader,"TransportMean"),ClosedRequest=Convert.ToInt32(reader["Closed_Request"]),
            ImportCompany=Text(reader,"ImportCompany"),ImportCompanyAddress=Text(reader,"ImporeterCompanyAddress"),ReceiverName=Text(reader,"Reciever_Name"),
            ShippingPort=Text(reader,"ShippingPort"),ArrivalPortType=Text(reader,"ArrivalPortType"),ArrivalPort=Text(reader,"ArrivalPort"),
            TransitCountry=Text(reader,"TransitCountry"),TransitPortType=Text(reader,"TransitPortType"),TransitPort=Text(reader,"TransitPort"),
            ExaminationAddress=Text(reader,"Examination_location"),ExaminationGovernorate=Text(reader,"ExaminationGovernorate"),ExaminationCenter=Text(reader,"ExaminationCenter"),
            ExaminationStationId=reader["Station_Examination_ID"] is DBNull?(reader["Outlet_Exmainiation_ID"] is DBNull?null:0L):Convert.ToInt64(reader["Station_Examination_ID"]),ExaminationStationName=reader["Station_Examination_ID"] is DBNull?Text(reader,"Outlet_Examination_Name"):Text(reader,"ExaminationStationName"),
            ExaminationOutletId=reader["Outlet_Exmainiation_ID"] is DBNull?null:Convert.ToInt64(reader["Outlet_Exmainiation_ID"]),
            GenshiStationId=reader["Station_Genshi_ID"] is DBNull?null:Convert.ToInt64(reader["Station_Genshi_ID"]),
            GenshiStationName=reader["Station_Genshi_ID"] is DBNull?Text(reader,"Outlet_Genshi_Name"):Text(reader,"GenshiStationName"),
            GenshiOutletId=reader["Outlet_Genshi_ID"] is DBNull?null:Convert.ToInt64(reader["Outlet_Genshi_ID"]),
            FinalResult=Text(reader,"Final_Result_Name"),FinalResultDate=Date(reader,"FinalResultDate")
        };
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.Items.Add(new ExportInspectionItemVm
        {
            Id=Convert.ToInt64(reader["ID"]),ShortName=Text(reader,"ShortName"),Category=Text(reader,"Category"),
            CategoryGroup=Text(reader,"CategoryGroup"),GrossWeight=Decimal(reader,"GrossWeight"),NetWeight=Decimal(reader,"Net_Weight"),
            PackageCount=Int(reader,"Package_Count"),PackageType=Text(reader,"PackageType"),PackageMaterial=Text(reader,"PackageMaterial"),
            IsAccepted=Convert.ToBoolean(reader["IsAccepted"]),FarmCode=Text(reader,"FarmCode"),FarmName=Text(reader,"FarmName")
        });
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct))
        {
            var item=model.Items.FirstOrDefault(x=>x.Id==Convert.ToInt64(reader["Ex_CheckRequest_Items_ID"]));
            item?.Lots.Add(new ExportInspectionLotVm { Id=Convert.ToInt64(reader["ID"]),LotNumber=Text(reader,"Lot_Number"),PackageCount=Int(reader,"Package_Count"),GrossWeight=Decimal(reader,"GrossWeight"),NetWeight=Decimal(reader,"Net_Weight"),PackageGrossWeight=Decimal(reader,"Package_Based_Weight"),PackageNetWeight=Decimal(reader,"Package_Net_Weight"),PackageType=Text(reader,"PackageType"),PackageMaterial=Text(reader,"PackageMaterial"),GrowerNumber=Text(reader,"Grower_Number"),Waybill=Text(reader,"Waybill"),IsAccepted=Bool(reader,"IsAccepted"),RejectReason=Text(reader,"RejectReason") });
        }
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.Attachments.Add(new ExportInspectionAttachmentVm { Id=Convert.ToInt64(reader["Id"]),Name=Text(reader,"AttachmentName"),Number=Text(reader,"Attachment_Number"),Path=Text(reader,"AttachmentPath"),StartDate=Date(reader,"StartDate"),EndDate=Date(reader,"EndDate") });
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.Fees.Add(new ExportInspectionFeeVm { FeeType=Text(reader,"FeeType"),Source=Text(reader,"FeeSource"),OrderNumber=Text(reader,"OrderNumber"),IsPaid=Bool(reader,"IsPaid"),TotalGrossWeight=Decimal(reader,"Total_GrossWeight"),FeeValue=Decimal(reader,"Fee_Value"),TotalAmount=Decimal(reader,"Total_Amount"),IsRounded=Bool(reader,"is_Fee_Rounding") });
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.StationEngineers.Add(new ExportStationEngineerVm { UserId=Convert.ToInt64(reader["Id"]),EmployeeId=reader["EmpId"] is DBNull?null:Convert.ToInt64(reader["EmpId"]),Name=Text(reader,"FullName"),AssignmentFrom=Date(reader,"Date_From"),AssignmentTo=Date(reader,"Date_To") });
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.GenshiEngineers.Add(new ExportStationEngineerVm { UserId=Convert.ToInt64(reader["Id"]),EmployeeId=reader["EmpId"] is DBNull?null:Convert.ToInt64(reader["EmpId"]),Name=Text(reader,"FullName"),AssignmentFrom=Date(reader,"Date_From"),AssignmentTo=Date(reader,"Date_To") });
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.CommitteeOutlets.Add(new ExportCommitteeOutletVm { Id=Convert.ToInt64(reader["ID"]),Name=Text(reader,"Ar_Name") });
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.OutletEngineers.Add(new ExportStationEngineerVm { UserId=Convert.ToInt64(reader["Id"]),EmployeeId=reader["EmpId"] is DBNull?null:Convert.ToInt64(reader["EmpId"]),Name=Text(reader,"FullName"),OutletId=Convert.ToInt64(reader["OutletId"]),OutletName=Text(reader,"OutletName") });
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.AnalysisLabs.Add(new ExportAnalysisLabVm { Id=Convert.ToInt32(reader["ID"]),AnalysisName=Text(reader,"AnalysisName"),LabName=Text(reader,"LabName") });
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.ShiftTimings.Add(new ExportShiftTimingVm { Id=Convert.ToByte(reader["ID"]),Name=Text(reader,"Name_Ar"),Amount=Convert.ToDecimal(reader["Amount"]) });
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.Committees.Add(new ExportRequestCommitteeVm { Id=Convert.ToInt64(reader["ID"]),CommitteeTypeId=Convert.ToByte(reader["CommitteeType_ID"]),CommitteeType=Text(reader,"Name_Ar"),DelegationDate=Date(reader,"Delegation_Date"),Employees=Text(reader,"Employees"),LotCount=Convert.ToInt32(reader["LotCount"]) });
        return model;
    }

    [HttpPost("Details/{id:long}/Committees")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCommittee(long id,byte committeeTypeId,DateTime delegationDate,long adminEmployeeId,List<long> employeeIds,long? selectedOutletId,List<long> selectedLotIds,List<int> selectedAnalysisLabTypeIds,List<byte> shiftTimingIds,List<byte> shiftCounts,CancellationToken ct)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated")return Unauthorized();
        if(!short.TryParse(HttpContext.Session.GetString("UserId"),out var accessUser)||!await _menuAccess.CanModifyAsync(accessUser,MenuAccessReader.ExportRequestMenuUrl,ct))return Forbid();
        if(committeeTypeId is not(1 or 2 or 3 or 6)){TempData["CommitteeError"]="نوع اللجنة غير صحيح.";return RedirectToAction(nameof(Details),new{id});}
        employeeIds=(employeeIds??[]).Where(x=>x>0).Distinct().ToList();if(adminEmployeeId>0&&!employeeIds.Contains(adminEmployeeId))employeeIds.Add(adminEmployeeId);
        selectedLotIds=(selectedLotIds??[]).Distinct().ToList();selectedAnalysisLabTypeIds=(selectedAnalysisLabTypeIds??[]).Distinct().ToList();shiftTimingIds??=[];shiftCounts??=[];
        if(adminEmployeeId<=0||employeeIds.Count==0){TempData["CommitteeError"]="اختر مهندسًا واحدًا على الأقل وحدد رئيس اللجنة.";return RedirectToAction(nameof(Details),new{id});}
        short creator=short.TryParse(HttpContext.Session.GetString("UserId"),out var sessionUser)?sessionUser:(short)0;
        const string sql="""
        SET XACT_ABORT ON;BEGIN TRANSACTION;
        DECLARE @Employees TABLE(Id bigint PRIMARY KEY);
        INSERT @Employees(Id) SELECT DISTINCT TRY_CONVERT(bigint,[value]) FROM OPENJSON(@EmployeesJson) WHERE TRY_CONVERT(bigint,[value]) IS NOT NULL;
        DECLARE @SelectedLots TABLE(Id bigint PRIMARY KEY);
        DECLARE @SelectedLabs TABLE(Id int PRIMARY KEY);
        INSERT @SelectedLots(Id) SELECT DISTINCT TRY_CONVERT(bigint,[value]) FROM OPENJSON(@LotsJson) WHERE TRY_CONVERT(bigint,[value]) IS NOT NULL;
        INSERT @SelectedLabs(Id) SELECT DISTINCT TRY_CONVERT(int,[value]) FROM OPENJSON(@LabsJson) WHERE TRY_CONVERT(int,[value]) IS NOT NULL;
        DECLARE @StationId bigint,@OutletId bigint,@OutletHrId bigint,@HasStationEmployees bit=0;
        SELECT TOP(1)
          @StationId=CASE WHEN @CommitteeTypeId=2 THEN Station_Genshi_ID ELSE Station_Examination_ID END,
          @OutletId=CASE WHEN @CommitteeTypeId=2 THEN Outlet_Genshi_ID ELSE Outlet_Exmainiation_ID END
        FROM dbo.Ex_CheckRequest_Places WHERE Ex_CheckRequest_ID=@RequestId;
        SELECT @OutletHrId=ID_HR FROM dbo.Outlet WHERE ID=@OutletId;
        IF @StationId IS NULL AND @OutletId IS NULL THROW 50010,N'لم يتم تحديد محطة أو منفذ للمرحلة المطلوبة.',1;
        IF @StationId IS NOT NULL AND EXISTS(SELECT 1 FROM dbo.Station_Emp se WHERE se.Station_Id=@StationId AND se.IsActive=1 AND se.User_Deletion_Id IS NULL AND (se.Date_From IS NULL OR se.Date_From<=@DelegationDate) AND (se.Date_To IS NULL OR se.Date_To>=@DelegationDate)) SET @HasStationEmployees=1;
        IF @StationId IS NOT NULL AND @HasStationEmployees=0
        BEGIN
          IF @SelectedOutletId IS NULL THROW 50016,N'لا يوجد موظفون مسكنون على المحطة في تاريخ اللجنة؛ اختر منفذًا لعرض موظفيه.',1;
          SELECT @OutletHrId=ID_HR FROM dbo.Outlet WHERE ID=@SelectedOutletId AND IsActive=1 AND User_Deletion_Id IS NULL;
          IF @OutletHrId IS NULL THROW 50017,N'المنفذ المختار غير صحيح.',1;
        END;
        IF EXISTS(SELECT 1 FROM @Employees e WHERE NOT EXISTS(
          SELECT 1 FROM dbo.Station_Emp se WHERE @StationId IS NOT NULL AND @HasStationEmployees=1 AND se.Station_Id=@StationId AND se.Emp_Id=e.Id AND se.IsActive=1 AND se.User_Deletion_Id IS NULL AND (se.Date_From IS NULL OR se.Date_From<=@DelegationDate) AND (se.Date_To IS NULL OR se.Date_To>=@DelegationDate)
          UNION ALL SELECT 1 FROM dbPrivilage.dbo.PR_User u WHERE (@StationId IS NULL OR @HasStationEmployees=0) AND u.Id=e.Id AND u.Outlet_ID=@OutletHrId AND u.Active=1))
          THROW 50011,N'أحد الموظفين المختارين غير تابع لمكان المرحلة في تاريخ اللجنة.',1;
        IF EXISTS(SELECT 1 FROM dbo.Ex_RequestCommittee WHERE ExCheckRequest_ID=@RequestId AND CommitteeType_ID=@CommitteeTypeId AND Delegation_Date=@DelegationDate AND User_Deletion_Id IS NULL AND Is_Cancel IS NULL)
          THROW 50012,N'توجد لجنة من نفس النوع على الطلب في هذا التاريخ.',1;
        IF NOT EXISTS(SELECT 1 FROM dbo.Ex_CheckRequest_Items i JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.Ex_CheckRequest_Items_ID=i.ID WHERE i.Ex_CheckRequest_ID=@RequestId AND i.User_Deletion_Id IS NULL AND l.User_Deletion_Id IS NULL)
          THROW 50013,N'لا توجد لوطات على الطلب.',1;
        IF NOT EXISTS(SELECT 1 FROM @SelectedLots selectedLot JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.ID=selectedLot.Id AND l.User_Deletion_Id IS NULL JOIN dbo.Ex_CheckRequest_Items i ON i.ID=l.Ex_CheckRequest_Items_ID AND i.Ex_CheckRequest_ID=@RequestId AND i.User_Deletion_Id IS NULL) THROW 50014,N'اختر لوطًا واحدًا على الأقل لعمل اللجنة.',1;
        IF @CommitteeTypeId=3 AND NOT EXISTS(SELECT 1 FROM @SelectedLabs) THROW 50014,N'اختر تحليلًا ومعملًا واحدًا على الأقل للجنة السحب.',1;
        IF @CommitteeTypeId=6 AND NOT EXISTS(SELECT 1 FROM dbo.EX_Choose_Treatment WHERE Ex_CheckRequest_ID=@RequestId AND TreatmentMethods_ID IS NOT NULL) THROW 50015,N'لا توجد طرق معالجة محددة على الطلب.',1;
        DECLARE @CommitteeId bigint=NEXT VALUE FOR dbo.Ex_RequestCommittee_SEQ;
        INSERT dbo.Ex_RequestCommittee(ID,ExCheckRequest_ID,CommitteeType_ID,Delegation_Date,StartTime,EndTime,IsFinishedAll,IsApproved,Status,User_Creation_Id,User_Creation_Date)
        VALUES(@CommitteeId,@RequestId,@CommitteeTypeId,@DelegationDate,CAST('00:00' AS time),CAST('00:01' AS time),0,1,0,@UserId,GETDATE());
        INSERT dbo.CommitteeEmployee(Committee_ID,Employee_Id,ISAdmin,OperationType,User_Creation_Id,User_Creation_Date)
        SELECT @CommitteeId,Id,CASE WHEN Id=@AdminEmployeeId THEN 1 ELSE 0 END,73,@UserId,GETDATE() FROM @Employees;
        IF @CommitteeTypeId IN(1,2)
          INSERT dbo.Ex_CommitteeResult(ID,Committee_ID,Ex_Request_Item_Id,LotData_ID,IS_Total,Item_ShortName_ID,User_Creation_Id,User_Creation_Date,IS_Total_Android)
          SELECT NEXT VALUE FOR dbo.Ex_CommitteeResult_SEQ,@CommitteeId,i.ID,l.ID,1,i.Item_ShortName_ID,@UserId,GETDATE(),1 FROM dbo.Ex_CheckRequest_Items i JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.Ex_CheckRequest_Items_ID=i.ID JOIN @SelectedLots selectedLot ON selectedLot.Id=l.ID WHERE i.Ex_CheckRequest_ID=@RequestId AND i.User_Deletion_Id IS NULL AND l.User_Deletion_Id IS NULL;
        IF @CommitteeTypeId=3
          INSERT dbo.Ex_CheckRequest_SampleData(ID,AnalysisLabType_ID,Ex_RequestCommittee_ID,Ex_Request_Item_Id,LotData_ID,User_Creation_Id,User_Creation_Date,IS_Total,Item_ShortName_ID,IS_Total_Android,IS_From_Android)
          SELECT NEXT VALUE FOR dbo.Ex_CheckRequest_SampleData_SEQ,lab.Id,@CommitteeId,i.ID,l.ID,@UserId,GETDATE(),1,i.Item_ShortName_ID,1,1 FROM dbo.Ex_CheckRequest_Items i JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.Ex_CheckRequest_Items_ID=i.ID JOIN @SelectedLots selectedLot ON selectedLot.Id=l.ID CROSS JOIN @SelectedLabs lab WHERE i.Ex_CheckRequest_ID=@RequestId AND i.User_Deletion_Id IS NULL AND l.User_Deletion_Id IS NULL;
        INSERT dbo.Ex_RequestCommittee_Shift(ID,Ex_RequestCommittee_ID,ShiftTiming_ID,[Count],Amount,IsPaid,User_Creation_Id,User_Creation_Date)
          SELECT NEXT VALUE FOR dbo.Ex_RequestCommittee_Shift_SEQ,@CommitteeId,shiftData.Id,shiftData.ShiftCount,CONVERT(money,ISNULL(timing.[count],0)*shiftData.ShiftCount),0,@UserId,GETDATE()
          FROM OPENJSON(@ShiftsJson) WITH(Id tinyint '$.Id',ShiftCount tinyint '$.Count') shiftData
          INNER JOIN dbo.ShiftTiming timing ON timing.ID=shiftData.Id AND timing.IsActive=1 AND timing.User_Deletion_Id IS NULL
          WHERE shiftData.ShiftCount>0;
        IF @CommitteeTypeId=6
          INSERT dbo.Ex_Request_TreatmentData(ID,Ex_RequestCommittee_ID,Ex_Request_Item_Id,Ex_Request_LotData_ID,TreatmentMethod_ID,Station_ID,User_Creation_Id,User_Creation_Date,Item_ShortName_ID,IS_Total_Android,IS_From_Android,IS_Total)
          SELECT NEXT VALUE FOR dbo.Ex_Request_TreatmentData_SEQ,@CommitteeId,i.ID,l.ID,ch.TreatmentMethods_ID,@StationId,@UserId,GETDATE(),i.Item_ShortName_ID,1,1,1 FROM dbo.Ex_CheckRequest_Items i JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.Ex_CheckRequest_Items_ID=i.ID JOIN @SelectedLots selectedLot ON selectedLot.Id=l.ID JOIN dbo.EX_Choose_Treatment ch ON ch.Ex_CheckRequest_ID=@RequestId AND ch.Item_ShortName_id=i.Item_ShortName_ID AND ch.TreatmentMethods_ID IS NOT NULL WHERE i.Ex_CheckRequest_ID=@RequestId AND i.User_Deletion_Id IS NULL AND l.User_Deletion_Id IS NULL;
        UPDATE dbo.Ex_CheckRequest
          SET IsAccepted=1,IsActive=1,Notes_Reject=NULL,IsAccepted_Date=GETDATE(),User_Updation_Id=@UserId,User_Updation_Date=GETDATE()
        WHERE ID=@RequestId AND User_Deletion_Id IS NULL;
        IF @@ROWCOUNT=0 THROW 50018,N'طلب الفحص غير موجود أو تم حذفه.',1;
        COMMIT;
        """;
        try
        {
            await using var connection=new SqlConnection(_connectionString);await connection.OpenAsync(ct);await using var command=new SqlCommand(sql,connection){CommandTimeout=60};
            command.Parameters.Add("@RequestId",SqlDbType.BigInt).Value=id;command.Parameters.Add("@CommitteeTypeId",SqlDbType.TinyInt).Value=committeeTypeId;command.Parameters.Add("@DelegationDate",SqlDbType.Date).Value=delegationDate.Date;command.Parameters.Add("@AdminEmployeeId",SqlDbType.BigInt).Value=adminEmployeeId;command.Parameters.Add("@SelectedOutletId",SqlDbType.BigInt).Value=selectedOutletId??(object)DBNull.Value;command.Parameters.Add("@UserId",SqlDbType.SmallInt).Value=creator;
            command.Parameters.Add("@EmployeesJson",SqlDbType.NVarChar,-1).Value=System.Text.Json.JsonSerializer.Serialize(employeeIds);
            command.Parameters.Add("@LotsJson",SqlDbType.NVarChar,-1).Value=System.Text.Json.JsonSerializer.Serialize(selectedLotIds);command.Parameters.Add("@LabsJson",SqlDbType.NVarChar,-1).Value=System.Text.Json.JsonSerializer.Serialize(selectedAnalysisLabTypeIds);
            var shifts=shiftTimingIds.Select((shiftId,index)=>new{Id=shiftId,Count=index<shiftCounts.Count?Math.Max((byte)1,shiftCounts[index]):(byte)1}).ToList();command.Parameters.Add("@ShiftsJson",SqlDbType.NVarChar,-1).Value=System.Text.Json.JsonSerializer.Serialize(shifts);
            await command.ExecuteNonQueryAsync(ct);TempData["CommitteeSuccess"]="تمت الموافقة على الطلب وتشكيل اللجنة على اللوطات المختارة وحفظ بدلات المهندسين.";
        }
        catch(SqlException ex)when(ex.Number is >=50010 and <=50018){TempData["CommitteeError"]=ex.Message;}
        catch(Exception ex){_logger.LogError(ex,"Failed to create export committee for request {RequestId}; TraceId={TraceId}",id,HttpContext.TraceIdentifier);TempData["CommitteeError"]=$"تعذر تشكيل اللجنة. رقم التتبع: {HttpContext.TraceIdentifier}";}
        return RedirectToAction(nameof(Details),new{id});
    }

    [HttpPost("Details/{id:long}/Decision")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDecision(long id,string decision,string? notes,CancellationToken ct)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated")return Unauthorized();
        if(!short.TryParse(HttpContext.Session.GetString("UserId"),out var userId)||!await _menuAccess.CanModifyAsync(userId,MenuAccessReader.ExportRequestMenuUrl,ct))return Forbid();
        if(!await CanAccessRequestAsync(id,userId,ct))return Forbid();
        decision=(decision??string.Empty).Trim().ToLowerInvariant();notes=notes?.Trim();
        if(decision=="approve"){TempData["DecisionError"]="تتم الموافقة عند حفظ بيانات تشكيل اللجنة.";return RedirectToAction(nameof(Details),new{id});}
        if(decision is not("reject" or "complete")){TempData["DecisionError"]="الحالة المختارة غير صحيحة.";return RedirectToAction(nameof(Details),new{id});}
        if((decision is "reject" or "complete")&&string.IsNullOrWhiteSpace(notes)){TempData["DecisionError"]="اكتب سبب الرفض أو البيانات المطلوب استكمالها.";return RedirectToAction(nameof(Details),new{id});}
        const string sql="""
        UPDATE dbo.Ex_CheckRequest
        SET IsAccepted=CASE WHEN @Decision='approve' THEN 1 WHEN @Decision='reject' THEN 0 ELSE NULL END,
            IsActive=CASE WHEN @Decision='reject' THEN 0 ELSE 1 END,
            Notes_Reject=CASE WHEN @Decision='approve' THEN NULL ELSE @Notes END,
            IsAccepted_Date=CASE WHEN @Decision IN('approve','reject') THEN GETDATE() ELSE NULL END,
            User_Updation_Id=@UserId,User_Updation_Date=GETDATE()
        WHERE ID=@Id AND User_Deletion_Id IS NULL;
        """;
        try
        {
            await using var connection=new SqlConnection(_connectionString);await connection.OpenAsync(ct);
            await using var command=new SqlCommand(sql,connection);command.Parameters.Add("@Id",SqlDbType.BigInt).Value=id;command.Parameters.Add("@Decision",SqlDbType.VarChar,20).Value=decision;command.Parameters.Add("@Notes",SqlDbType.NVarChar,-1).Value=(object?)notes??DBNull.Value;command.Parameters.Add("@UserId",SqlDbType.SmallInt).Value=userId;
            if(await command.ExecuteNonQueryAsync(ct)==0)return NotFound();
            TempData["DecisionSuccess"]=decision switch{"approve"=>"تمت الموافقة على الطلب.","reject"=>"تم رفض الطلب.",_=>"تم إرسال الطلب لاستكمال البيانات."};
        }
        catch(Exception ex){_logger.LogError(ex,"Failed to update export request decision {RequestId}; TraceId={TraceId}",id,HttpContext.TraceIdentifier);TempData["DecisionError"]=$"تعذر حفظ موقف الطلب. رقم التتبع: {HttpContext.TraceIdentifier}";}
        return RedirectToAction(nameof(Details),new{id});
    }

    private static string Text(SqlDataReader r,string name)=>r[name] is DBNull?string.Empty:Convert.ToString(r[name])??string.Empty;
    private static DateTime? Date(SqlDataReader r,string name)=>r[name] is DBNull?null:Convert.ToDateTime(r[name]);
    private static bool? Bool(SqlDataReader r,string name)=>r[name] is DBNull?null:Convert.ToBoolean(r[name]);
    private static decimal? Decimal(SqlDataReader r,string name)=>r[name] is DBNull?null:Convert.ToDecimal(r[name]);
    private static int? Int(SqlDataReader r,string name)=>r[name] is DBNull?null:Convert.ToInt32(r[name]);
    private static string JoinPlace(string outlet,string station)=>string.Join(" - ",new[]{outlet,station}.Where(x=>!string.IsNullOrWhiteSpace(x)));
    private async Task<bool> HasSupervisorAccessAsync(CancellationToken ct)=>short.TryParse(HttpContext.Session.GetString("UserId"),out var userId)&&await _menuAccess.CanViewAsync(userId,MenuAccessReader.ExportSupervisorMenuUrl,ct);
    private async Task<bool> HasOperationalAccessAsync(CancellationToken ct)=>short.TryParse(HttpContext.Session.GetString("UserId"),out var userId)&&await _menuAccess.CanViewAsync(userId,MenuAccessReader.ExportRequestMenuUrl,ct);
    private async Task<bool> CanAccessRequestAsync(long requestId,short userId,CancellationToken ct)
    {
        const string sql="""
        SELECT CASE WHEN EXISTS
        (
          SELECT 1 FROM dbo.Ex_CheckRequest_Places p
          WHERE p.Ex_CheckRequest_ID=@RequestId AND
          (
            EXISTS(SELECT 1 FROM dbo.Station_Emp se WHERE se.Emp_Id=@UserId AND se.IsActive=1 AND se.User_Deletion_Id IS NULL
              AND (se.Date_From IS NULL OR se.Date_From<=CAST(GETDATE() AS date)) AND (se.Date_To IS NULL OR se.Date_To>=CAST(GETDATE() AS date))
              AND se.Station_Id IN(p.Station_Examination_ID,p.Station_Genshi_ID))
            OR EXISTS(SELECT 1 FROM dbPrivilage.dbo.PR_User u INNER JOIN dbo.Outlet o ON o.ID_HR=u.Outlet_ID
              AND o.ID IN(p.Outlet_Exmainiation_ID,p.Outlet_Genshi_ID) WHERE u.Id=@UserId AND u.Active=1)
            OR EXISTS(SELECT 1 FROM dbo.Ex_RequestCommittee c INNER JOIN dbo.CommitteeEmployee e ON e.Committee_ID=c.ID
              AND e.Employee_Id=@UserId AND e.OperationType=73 AND e.User_Deletion_Id IS NULL WHERE c.ExCheckRequest_ID=@RequestId AND c.User_Deletion_Id IS NULL)
          )
        ) THEN 1 ELSE 0 END;
        """;
        await using var connection=new SqlConnection(_connectionString);await connection.OpenAsync(ct);
        await using var command=new SqlCommand(sql,connection){CommandTimeout=30};
        command.Parameters.Add("@RequestId",SqlDbType.BigInt).Value=requestId;
        command.Parameters.Add("@UserId",SqlDbType.SmallInt).Value=userId;
        return Convert.ToInt32(await command.ExecuteScalarAsync(ct))==1;
    }
}
