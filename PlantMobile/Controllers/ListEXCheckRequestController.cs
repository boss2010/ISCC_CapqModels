using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ViewModels;

namespace PlantMobile.Controllers;

[Route("Export_CheckRequest/List_EXCheckRequest")]
public class ListEXCheckRequestController : Controller
{
    private readonly string _connectionString;
    private readonly ILogger<ListEXCheckRequestController> _logger;

    public ListEXCheckRequestController(IConfiguration configuration, ILogger<ListEXCheckRequestController> logger)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _logger = logger;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(string? requestNumber, DateTime? fromDate, DateTime? toDate,
        int status = 0, int page = 1, long? stationId = null, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

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

        try { await LoadListAsync(model, cancellationToken); }
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

    private async Task LoadListAsync(ExportInspectionRequestListVm model, CancellationToken ct)
    {
        const string sql = """
        SET NOCOUNT ON;
        SELECT ROW_NUMBER() OVER(ORDER BY Ex_CheckRequest_ID DESC) RowNumber, * INTO #Results
        FROM dbo.Ex_List
        WHERE (@Number=N'' OR ImCheckRequest_Number LIKE N'%'+@Number+N'%')
          AND (@FromDate IS NULL OR Creation_Date>=@FromDate)
          AND (@ToDate IS NULL OR Creation_Date<DATEADD(day,1,@ToDate))
          AND (@StationId IS NULL OR Station_Examination_ID=@StationId)
          AND (@Status=0 OR (@Status=1 AND ISNULL(Final_Result_ID,0)=0 AND ISNULL(Closed_Request,0)=0)
             OR (@Status=2 AND ISNULL(Final_Result_ID,0)>0) OR (@Status=3 AND ISNULL(Closed_Request,0)=1));
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
        OUTER APPLY(SELECT COUNT(DISTINCT p.Ex_CheckRequest_ID) RequestCount FROM dbo.Ex_CheckRequest_Places p WHERE p.Station_Examination_ID=s.ID) requests
        OUTER APPLY(SELECT COUNT(DISTINCT c.ID) CommitteeCount,
          COUNT(DISTINCT CASE WHEN c.CommitteeType_ID=1 THEN c.ID END) InspectionCount,
          COUNT(DISTINCT CASE WHEN c.CommitteeType_ID=3 THEN c.ID END) SamplingCount,
          COUNT(DISTINCT CASE WHEN c.CommitteeType_ID=6 THEN c.ID END) TreatmentCount
          FROM dbo.Ex_CheckRequest_Places p INNER JOIN dbo.Ex_RequestCommittee c ON c.ExCheckRequest_ID=p.Ex_CheckRequest_ID
          WHERE p.Station_Examination_ID=s.ID AND c.CommitteeType_ID IN(1,3,6) AND c.User_Deletion_Id IS NULL) committees
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
          l.ExportCountryName,l.ImporterTypeName,l.ImporterName,l.Outlet_Examination_Name,l.Station_Examination_Name,
          l.Outlet_Genshi_Name,l.Station_Genshi_Name,ISNULL(l.Closed_Request,0) Closed_Request,l.Final_Result_Name,
          d.DelegateName,d.DelegateAddress,d.Ship_Name,sm.Ar_Name ShipmentMean,tm.Ar_Name TransportMean,
          COALESCE(extra.OwnerName,CASE d.ImporterType_Id WHEN 6 THEN company.Owner_Ar WHEN 7 THEN org.Person_Responsible_Name WHEN 8 THEN person.Name END) OwnerName,
          COALESCE(extra.OwnerAddress,CASE d.ImporterType_Id WHEN 6 THEN company.Address_Ar WHEN 7 THEN org.Address_Ar WHEN 8 THEN person.Address END) OwnerAddress,
          extra.ImportCompany,extra.ImporeterCompanyAddress,extra.Reciever_Name,
          shippingPort.Name_Ar ShippingPort,arrivalType.Name_Ar ArrivalPortType,arrivalPort.Name_Ar ArrivalPort,
          transitCountry.Ar_Name TransitCountry,transitType.Name_Ar TransitPortType,transitPort.Name_Ar TransitPort,
          place.Examination_location,gov.Ar_Name ExaminationGovernorate,center.Ar_Name ExaminationCenter,
          place.Station_Examination_ID,station.Ar_Name ExaminationStationName,place.Outlet_Exmainiation_ID,
          fr.Date FinalResultDate
        FROM dbo.Ex_CheckRequest r
        LEFT JOIN dbo.Ex_List l ON l.Ex_CheckRequest_ID=r.ID
        LEFT JOIN dbo.Ex_CheckRequest_Data d ON d.Ex_CheckRequest_ID=r.ID AND d.User_Deletion_Id IS NULL
        LEFT JOIN dbo.Shipment_Mean sm ON sm.ID=d.Shipment_Mean_Id
        LEFT JOIN dbo.Transport_Mean tm ON tm.ID=d.Transport_Mean_Id
        LEFT JOIN dbo.Ex_CheckRequestData_Extra extra ON extra.Ex_CheckRequest_Data_ID=d.ID AND extra.User_Deletion_Id IS NULL
        LEFT JOIN dbo.Company_National company ON company.ID=d.Importer_ID AND d.ImporterType_Id=6
        LEFT JOIN dbo.Public_Organization org ON org.ID=d.Importer_ID AND d.ImporterType_Id=7
        LEFT JOIN dbo.Person person ON person.ID=d.Importer_ID AND d.ImporterType_Id=8
        LEFT JOIN dbo.Ex_CheckRequest_Places place ON place.Ex_CheckRequest_ID=r.ID
        LEFT JOIN dbo.Governate gov ON gov.ID=place.Govern_ID
        LEFT JOIN dbo.Center center ON center.ID=place.Center_ID
        LEFT JOIN dbo.Station station ON station.ID=place.Station_Examination_ID
        LEFT JOIN dbo.Country transitCountry ON transitCountry.ID=d.TransitCountry_Id
        OUTER APPLY(SELECT TOP(1) pn.Name_Ar FROM dbo.Ex_CheckRequest_Port ep LEFT JOIN dbo.PortNational pn ON pn.ID=ep.Port_ID WHERE ep.Ex_CheckRequest_Data_ID=d.ID AND ep.ReqPortType_ID=9 AND ep.User_Deletion_Id IS NULL ORDER BY ep.ID DESC) shippingPort
        OUTER APPLY(SELECT TOP(1) pi.Name_Ar,pi.PortTypeID FROM dbo.Ex_CheckRequest_Port ep LEFT JOIN dbo.Port_International pi ON pi.ID=ep.Port_ID WHERE ep.Ex_CheckRequest_Data_ID=d.ID AND ep.ReqPortType_ID=10 AND ep.User_Deletion_Id IS NULL ORDER BY ep.ID DESC) arrivalPort
        LEFT JOIN dbo.Port_Type arrivalType ON arrivalType.ID=arrivalPort.PortTypeID
        OUTER APPLY(SELECT TOP(1) pi.Name_Ar,pi.PortTypeID FROM dbo.Ex_CheckRequest_Port ep LEFT JOIN dbo.Port_International pi ON pi.ID=ep.Port_ID WHERE ep.Ex_CheckRequest_Data_ID=d.ID AND ep.ReqPortType_ID=11 AND ep.User_Deletion_Id IS NULL ORDER BY ep.ID DESC) transitPort
        LEFT JOIN dbo.Port_Type transitType ON transitType.ID=transitPort.PortTypeID
        OUTER APPLY(SELECT TOP(1) x.Date FROM dbo.Ex_CheckRequest_Final_Result x WHERE x.Ex_CheckRequest_ID=r.ID AND x.User_Deletion_Id IS NULL ORDER BY x.ID DESC) fr
        WHERE r.ID=@Id;

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
        WHERE i.Ex_CheckRequest_ID=@Id AND i.User_Deletion_Id IS NULL ORDER BY i.ID;

        SELECT lot.ID,lot.Ex_CheckRequest_Items_ID,lot.Lot_Number,lot.Package_Count,lot.GrossWeight,lot.Net_Weight,
          COALESCE(pt.Ar_Name,pt.En_Name,N'') PackageType,COALESCE(pm.Ar_Name,pm.En_Name,N'') PackageMaterial,
          lot.Grower_Number,lot.Waybill,lot.IsAccepted,lot.RejectReason,lot.Package_Based_Weight,lot.Package_Net_Weight
        FROM dbo.Ex_CheckRequest_Items_Lot_Category lot
        INNER JOIN dbo.Ex_CheckRequest_Items i ON i.ID=lot.Ex_CheckRequest_Items_ID
        LEFT JOIN dbo.Package_Type pt ON pt.ID=lot.Package_Type_ID
        LEFT JOIN dbo.Package_Material pm ON pm.ID=lot.Package_Material_ID
        WHERE i.Ex_CheckRequest_ID=@Id AND lot.User_Deletion_Id IS NULL ORDER BY lot.ID;

        SELECT a.Id,COALESCE(NULLIF(a.Attachment_TypeName,N''),t.Ar_Name,N'مرفق') AttachmentName,
          a.Attachment_Number,a.AttachmentPath,a.StartDate,a.EndDate
        FROM dbo.A_AttachmentData_Ex_CheckRequest a
        LEFT JOIN dbo.A_AttachmentTableType t ON t.ID=a.A_AttachmentTableType_ID
        WHERE a.Ex_CheckRequest_ID=@Id AND a.User_Deletion_Id IS NULL ORDER BY a.Id;

        SELECT Total_GrossWeight,Fee_Value,Total_Amount,is_Fee_Rounding
        FROM dbo.Ex_CheckRequest_Fees WHERE Ex_CheckRequest_ID=@Id ORDER BY ID;

        SELECT DISTINCT employees.Id,employees.EmpId,employees.FullName FROM(
          SELECT u.Id,u.EmpId,u.FullName
          FROM dbo.Ex_CheckRequest_Places p
          INNER JOIN dbo.Station_Emp se ON se.Station_Id=p.Station_Examination_ID AND se.IsActive=1 AND se.User_Deletion_Id IS NULL
            AND (se.Date_From IS NULL OR se.Date_From<=CAST(GETDATE() AS date)) AND (se.Date_To IS NULL OR se.Date_To>=CAST(GETDATE() AS date))
          INNER JOIN dbPrivilage.dbo.PR_User u ON u.Id=se.Emp_Id AND u.Active=1
          WHERE p.Ex_CheckRequest_ID=@Id AND p.Station_Examination_ID IS NOT NULL
          UNION ALL
          SELECT u.Id,u.EmpId,u.FullName
          FROM dbo.Ex_CheckRequest_Places p INNER JOIN dbo.Outlet o ON o.ID=p.Outlet_Exmainiation_ID
          INNER JOIN dbPrivilage.dbo.PR_User u ON u.Outlet_ID=o.ID_HR AND u.Active=1
          WHERE p.Ex_CheckRequest_ID=@Id AND p.Station_Examination_ID IS NULL AND p.Outlet_Exmainiation_ID IS NOT NULL
        )employees ORDER BY employees.FullName;

        SELECT c.ID,CONVERT(tinyint,c.CommitteeType_ID) CommitteeType_ID,ct.Name_Ar,c.Delegation_Date,
          STRING_AGG(CONVERT(nvarchar(max),u.FullName),N'، ') Employees,
          CASE c.CommitteeType_ID WHEN 1 THEN (SELECT COUNT(*) FROM dbo.Ex_CommitteeResult r WHERE r.Committee_ID=c.ID AND r.User_Deletion_Id IS NULL)
            WHEN 3 THEN (SELECT COUNT(*) FROM dbo.Ex_CheckRequest_SampleData s WHERE s.Ex_RequestCommittee_ID=c.ID AND s.User_Deletion_Id IS NULL)
            WHEN 6 THEN (SELECT COUNT(*) FROM dbo.Ex_Request_TreatmentData x WHERE x.Ex_RequestCommittee_ID=c.ID AND x.User_Deletion_Id IS NULL) ELSE 0 END LotCount
        FROM dbo.Ex_RequestCommittee c INNER JOIN dbo.CommitteeType ct ON ct.ID=c.CommitteeType_ID
        LEFT JOIN dbo.CommitteeEmployee ce ON ce.Committee_ID=c.ID AND ce.OperationType=73 AND ce.User_Deletion_Id IS NULL
        LEFT JOIN dbPrivilage.dbo.PR_User u ON u.Id=ce.Employee_Id
        WHERE c.ExCheckRequest_ID=@Id AND c.CommitteeType_ID IN(1,3,6) AND c.User_Deletion_Id IS NULL
        GROUP BY c.ID,c.CommitteeType_ID,ct.Name_Ar,c.Delegation_Date ORDER BY c.ID DESC;
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
            DelegateName=Text(reader,"DelegateName"),DelegateAddress=Text(reader,"DelegateAddress"),ShipName=Text(reader,"Ship_Name"),
            ShipmentMean=Text(reader,"ShipmentMean"),TransportMean=Text(reader,"TransportMean"),ClosedRequest=Convert.ToInt32(reader["Closed_Request"]),
            ImportCompany=Text(reader,"ImportCompany"),ImportCompanyAddress=Text(reader,"ImporeterCompanyAddress"),ReceiverName=Text(reader,"Reciever_Name"),
            ShippingPort=Text(reader,"ShippingPort"),ArrivalPortType=Text(reader,"ArrivalPortType"),ArrivalPort=Text(reader,"ArrivalPort"),
            TransitCountry=Text(reader,"TransitCountry"),TransitPortType=Text(reader,"TransitPortType"),TransitPort=Text(reader,"TransitPort"),
            ExaminationAddress=Text(reader,"Examination_location"),ExaminationGovernorate=Text(reader,"ExaminationGovernorate"),ExaminationCenter=Text(reader,"ExaminationCenter"),
            ExaminationStationId=reader["Station_Examination_ID"] is DBNull?(reader["Outlet_Exmainiation_ID"] is DBNull?null:0L):Convert.ToInt64(reader["Station_Examination_ID"]),ExaminationStationName=reader["Station_Examination_ID"] is DBNull?Text(reader,"Outlet_Examination_Name"):Text(reader,"ExaminationStationName"),
            ExaminationOutletId=reader["Outlet_Exmainiation_ID"] is DBNull?null:Convert.ToInt64(reader["Outlet_Exmainiation_ID"]),
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
        while(await reader.ReadAsync(ct)) model.Fees.Add(new ExportInspectionFeeVm { TotalGrossWeight=Decimal(reader,"Total_GrossWeight"),FeeValue=Decimal(reader,"Fee_Value"),TotalAmount=Decimal(reader,"Total_Amount"),IsRounded=Bool(reader,"is_Fee_Rounding") });
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.StationEngineers.Add(new ExportStationEngineerVm { UserId=Convert.ToInt64(reader["Id"]),EmployeeId=reader["EmpId"] is DBNull?null:Convert.ToInt64(reader["EmpId"]),Name=Text(reader,"FullName") });
        await reader.NextResultAsync(ct);
        while(await reader.ReadAsync(ct)) model.Committees.Add(new ExportRequestCommitteeVm { Id=Convert.ToInt64(reader["ID"]),CommitteeTypeId=Convert.ToByte(reader["CommitteeType_ID"]),CommitteeType=Text(reader,"Name_Ar"),DelegationDate=Date(reader,"Delegation_Date"),Employees=Text(reader,"Employees"),LotCount=Convert.ToInt32(reader["LotCount"]) });
        return model;
    }

    [HttpPost("Details/{id:long}/Committees")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCommittee(long id,byte committeeTypeId,DateTime delegationDate,TimeSpan startTime,TimeSpan endTime,long adminEmployeeId,List<long> employeeIds,CancellationToken ct)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated")return Unauthorized();
        if(committeeTypeId is not(1 or 3 or 6)){TempData["CommitteeError"]="نوع اللجنة غير صحيح.";return RedirectToAction(nameof(Details),new{id});}
        employeeIds=(employeeIds??[]).Distinct().ToList();if(!employeeIds.Contains(adminEmployeeId))employeeIds.Add(adminEmployeeId);
        if(employeeIds.Count==0||startTime>=endTime){TempData["CommitteeError"]="اختر مهندسًا واحدًا على الأقل وتأكد من توقيت اللجنة.";return RedirectToAction(nameof(Details),new{id});}
        short creator=short.TryParse(HttpContext.Session.GetString("UserId"),out var sessionUser)?sessionUser:(short)0;
        const string sql="""
        SET XACT_ABORT ON;BEGIN TRANSACTION;
        DECLARE @Employees TABLE(Id bigint PRIMARY KEY);
        INSERT @Employees(Id) SELECT DISTINCT TRY_CONVERT(bigint,[value]) FROM OPENJSON(@EmployeesJson) WHERE TRY_CONVERT(bigint,[value]) IS NOT NULL;
        DECLARE @StationId bigint,@OutletId bigint,@OutletHrId bigint;
        SELECT TOP(1) @StationId=Station_Examination_ID,@OutletId=Outlet_Exmainiation_ID FROM dbo.Ex_CheckRequest_Places WHERE Ex_CheckRequest_ID=@RequestId;
        SELECT @OutletHrId=ID_HR FROM dbo.Outlet WHERE ID=@OutletId;
        IF @StationId IS NULL AND @OutletId IS NULL THROW 50010,N'لم يتم تحديد محطة أو منفذ فحص للطلب.',1;
        IF EXISTS(SELECT 1 FROM @Employees e WHERE NOT EXISTS(
          SELECT 1 FROM dbo.Station_Emp se WHERE @StationId IS NOT NULL AND se.Station_Id=@StationId AND se.Emp_Id=e.Id AND se.IsActive=1 AND se.User_Deletion_Id IS NULL AND (se.Date_From IS NULL OR se.Date_From<=@DelegationDate) AND (se.Date_To IS NULL OR se.Date_To>=@DelegationDate)
          UNION ALL SELECT 1 FROM dbPrivilage.dbo.PR_User u WHERE @StationId IS NULL AND u.Id=e.Id AND u.Outlet_ID=@OutletHrId AND u.Active=1))
          THROW 50011,N'أحد الموظفين المختارين غير تابع لمكان الفحص في تاريخ اللجنة.',1;
        IF EXISTS(SELECT 1 FROM dbo.Ex_RequestCommittee WHERE ExCheckRequest_ID=@RequestId AND CommitteeType_ID=@CommitteeTypeId AND Delegation_Date=@DelegationDate AND User_Deletion_Id IS NULL AND Is_Cancel IS NULL)
          THROW 50012,N'توجد لجنة من نفس النوع على الطلب في هذا التاريخ.',1;
        IF NOT EXISTS(SELECT 1 FROM dbo.Ex_CheckRequest_Items i JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.Ex_CheckRequest_Items_ID=i.ID WHERE i.Ex_CheckRequest_ID=@RequestId AND i.User_Deletion_Id IS NULL AND l.User_Deletion_Id IS NULL)
          THROW 50013,N'لا توجد لوطات على الطلب.',1;
        IF @CommitteeTypeId=3 AND NOT EXISTS(SELECT 1 FROM dbo.EX_Choose_SampleData WHERE Ex_CheckRequest_ID=@RequestId AND AnalysisLabType_ID IS NOT NULL) THROW 50014,N'لا توجد تحاليل سحب محددة على الطلب.',1;
        IF @CommitteeTypeId=6 AND NOT EXISTS(SELECT 1 FROM dbo.EX_Choose_Treatment WHERE Ex_CheckRequest_ID=@RequestId AND TreatmentMethods_ID IS NOT NULL) THROW 50015,N'لا توجد طرق معالجة محددة على الطلب.',1;
        DECLARE @CommitteeId bigint=NEXT VALUE FOR dbo.Ex_RequestCommittee_SEQ;
        INSERT dbo.Ex_RequestCommittee(ID,ExCheckRequest_ID,CommitteeType_ID,Delegation_Date,StartTime,EndTime,IsFinishedAll,IsApproved,Status,User_Creation_Id,User_Creation_Date)
        VALUES(@CommitteeId,@RequestId,@CommitteeTypeId,@DelegationDate,@StartTime,@EndTime,0,1,0,@UserId,GETDATE());
        INSERT dbo.CommitteeEmployee(Committee_ID,Employee_Id,ISAdmin,OperationType,User_Creation_Id,User_Creation_Date)
        SELECT @CommitteeId,Id,CASE WHEN Id=@AdminEmployeeId THEN 1 ELSE 0 END,73,@UserId,GETDATE() FROM @Employees;
        IF @CommitteeTypeId=1
          INSERT dbo.Ex_CommitteeResult(ID,Committee_ID,Ex_Request_Item_Id,LotData_ID,IS_Total,Item_ShortName_ID,User_Creation_Id,User_Creation_Date,IS_Total_Android)
          SELECT NEXT VALUE FOR dbo.Ex_CommitteeResult_SEQ,@CommitteeId,i.ID,l.ID,1,i.Item_ShortName_ID,@UserId,GETDATE(),1 FROM dbo.Ex_CheckRequest_Items i JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.Ex_CheckRequest_Items_ID=i.ID WHERE i.Ex_CheckRequest_ID=@RequestId AND i.User_Deletion_Id IS NULL AND l.User_Deletion_Id IS NULL;
        IF @CommitteeTypeId=3
          INSERT dbo.Ex_CheckRequest_SampleData(ID,AnalysisLabType_ID,Ex_RequestCommittee_ID,Ex_Request_Item_Id,LotData_ID,User_Creation_Id,User_Creation_Date,IS_Total,Item_ShortName_ID,IS_Total_Android,IS_From_Android)
          SELECT NEXT VALUE FOR dbo.Ex_CheckRequest_SampleData_SEQ,ch.AnalysisLabType_ID,@CommitteeId,i.ID,l.ID,@UserId,GETDATE(),1,i.Item_ShortName_ID,1,1 FROM dbo.Ex_CheckRequest_Items i JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.Ex_CheckRequest_Items_ID=i.ID JOIN dbo.EX_Choose_SampleData ch ON ch.Ex_CheckRequest_ID=@RequestId AND ch.Item_ShortName_ID=i.Item_ShortName_ID AND ch.AnalysisLabType_ID IS NOT NULL WHERE i.Ex_CheckRequest_ID=@RequestId AND i.User_Deletion_Id IS NULL AND l.User_Deletion_Id IS NULL;
        IF @CommitteeTypeId=6
          INSERT dbo.Ex_Request_TreatmentData(ID,Ex_RequestCommittee_ID,Ex_Request_Item_Id,Ex_Request_LotData_ID,TreatmentMethod_ID,Station_ID,User_Creation_Id,User_Creation_Date,Item_ShortName_ID,IS_Total_Android,IS_From_Android,IS_Total)
          SELECT NEXT VALUE FOR dbo.Ex_Request_TreatmentData_SEQ,@CommitteeId,i.ID,l.ID,ch.TreatmentMethods_ID,@StationId,@UserId,GETDATE(),i.Item_ShortName_ID,1,1,1 FROM dbo.Ex_CheckRequest_Items i JOIN dbo.Ex_CheckRequest_Items_Lot_Category l ON l.Ex_CheckRequest_Items_ID=i.ID JOIN dbo.EX_Choose_Treatment ch ON ch.Ex_CheckRequest_ID=@RequestId AND ch.Item_ShortName_id=i.Item_ShortName_ID AND ch.TreatmentMethods_ID IS NOT NULL WHERE i.Ex_CheckRequest_ID=@RequestId AND i.User_Deletion_Id IS NULL AND l.User_Deletion_Id IS NULL;
        COMMIT;
        """;
        try
        {
            await using var connection=new SqlConnection(_connectionString);await connection.OpenAsync(ct);await using var command=new SqlCommand(sql,connection){CommandTimeout=60};
            command.Parameters.Add("@RequestId",SqlDbType.BigInt).Value=id;command.Parameters.Add("@CommitteeTypeId",SqlDbType.TinyInt).Value=committeeTypeId;command.Parameters.Add("@DelegationDate",SqlDbType.Date).Value=delegationDate.Date;command.Parameters.Add("@StartTime",SqlDbType.Time).Value=startTime;command.Parameters.Add("@EndTime",SqlDbType.Time).Value=endTime;command.Parameters.Add("@AdminEmployeeId",SqlDbType.BigInt).Value=adminEmployeeId;command.Parameters.Add("@UserId",SqlDbType.SmallInt).Value=creator;
            command.Parameters.Add("@EmployeesJson",SqlDbType.NVarChar,-1).Value=System.Text.Json.JsonSerializer.Serialize(employeeIds);
            await command.ExecuteNonQueryAsync(ct);TempData["CommitteeSuccess"]="تم تشكيل اللجنة وتحديد كل لوطات الطلب تلقائيًا.";
        }
        catch(SqlException ex)when(ex.Number is >=50010 and <=50015){TempData["CommitteeError"]=ex.Message;}
        return RedirectToAction(nameof(Details),new{id});
    }

    private static string Text(SqlDataReader r,string name)=>r[name] is DBNull?string.Empty:Convert.ToString(r[name])??string.Empty;
    private static DateTime? Date(SqlDataReader r,string name)=>r[name] is DBNull?null:Convert.ToDateTime(r[name]);
    private static bool? Bool(SqlDataReader r,string name)=>r[name] is DBNull?null:Convert.ToBoolean(r[name]);
    private static decimal? Decimal(SqlDataReader r,string name)=>r[name] is DBNull?null:Convert.ToDecimal(r[name]);
    private static int? Int(SqlDataReader r,string name)=>r[name] is DBNull?null:Convert.ToInt32(r[name]);
    private static string JoinPlace(string outlet,string station)=>string.Join(" - ",new[]{outlet,station}.Where(x=>!string.IsNullOrWhiteSpace(x)));
}
