using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PlantMobile.Controllers;

public class LaboratoryResultController : Controller
{
    private const int MaxAttachmentBytes = 10 * 1024 * 1024;
    private readonly string _connectionString;
    private readonly string _attachmentRoot;

    public LaboratoryResultController(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _attachmentRoot = Path.Combine(environment.ContentRootPath, "App_Data", "LaboratoryResults");
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> Find(string code, long? committeeId = null)
    {
        if (!TryUser(out var userId)) return Unauthorized("انتهت جلسة الدخول.");
        code = (code ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(code)) return BadRequest("أدخل باركود أو QR Code صحيحًا.");

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT TOP 1 sample.ID, sample.Sample_BarCode, request.CheckRequest_Number,
                   committee.ID, COALESCE(plant.Name_Ar, shortName.ShortName_Ar, N'غير محدد'),
                   COALESCE(lot.Lot_Number, CONVERT(nvarchar(30), sample.LotData_ID), N'سحب كلي'),
                   COALESCE(analysisType.Name_Ar, N'غير محدد'), COALESCE(lab.Name_Ar, N'غير محدد'),
                   sample.IsAccepted, sample.Notes_Ar,
                   attachment.Id, COALESCE(attachment.Attachment_Number, N'')
            FROM dbo.Im_CheckRequest_SampleData sample
            JOIN dbo.Im_RequestCommittee committee ON committee.ID=sample.Im_RequestCommittee_ID AND committee.User_Deletion_Id IS NULL
            JOIN dbo.Im_CheckRequest request ON request.ID=committee.ImCheckRequest_ID
            JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID=committee.ID AND employee.Employee_Id=@UserId
              AND employee.ISAdmin=1 AND employee.OperationType=74 AND employee.User_Deletion_Id IS NULL
            LEFT JOIN dbo.Im_CheckRequest_Items_Lot_Category lot ON lot.ID=sample.LotData_ID
            LEFT JOIN dbo.Im_CheckRequest_Items requestItem ON requestItem.ID=sample.Im_Request_Item_Id
            LEFT JOIN dbo.Item_ShortName shortName ON shortName.ID=requestItem.Item_ShortName_ID
            LEFT JOIN dbo.Item plant ON plant.ID=shortName.Item_ID
            LEFT JOIN dbo.AnalysisLabType labType ON labType.ID=sample.AnalysisLabType_ID
            LEFT JOIN dbo.AnalysisType analysisType ON analysisType.ID=labType.AnalysisTypeID
            LEFT JOIN dbo.AnalysisLab lab ON lab.ID=labType.AnalysisLabID
            OUTER APPLY (SELECT TOP 1 a.Id,a.Attachment_Number FROM dbo.A_AttachmentData a
                         WHERE a.RowId=sample.ID AND a.A_AttachmentTableNameId=12 AND a.User_Deletion_Id IS NULL ORDER BY a.Id DESC) attachment
            WHERE sample.User_Deletion_Id IS NULL AND LTRIM(RTRIM(sample.Sample_BarCode))=@Code
              AND (@CommitteeId IS NULL OR committee.ID=@CommitteeId)
            ORDER BY CASE WHEN sample.IsAccepted IS NULL OR attachment.Id IS NULL THEN 0 ELSE 1 END, sample.ID
            """;
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@Code", code);
        command.Parameters.AddWithValue("@CommitteeId", (object?)committeeId ?? DBNull.Value);
        LaboratoryResultVm? model = null;
        await using (var reader = await command.ExecuteReaderAsync())
        {
            if (await reader.ReadAsync())
                model = new LaboratoryResultVm
                {
                    SampleId=reader.GetInt64(0), Barcode=reader.GetString(1), RequestNumber=Convert.ToString(reader.GetValue(2)) ?? "",
                    CommitteeId=reader.GetInt64(3), PlantName=reader.GetString(4), LotNumber=reader.GetString(5),
                    AnalysisType=reader.GetString(6), Laboratory=reader.GetString(7),
                    Accepted=reader.IsDBNull(8)?null:reader.GetBoolean(8), Notes=reader.IsDBNull(9)?"":reader.GetString(9),
                    AttachmentId=reader.IsDBNull(10)?null:reader.GetInt64(10), AttachmentName=reader.IsDBNull(11)?"":reader.GetString(11)
                };
        }
        if (model == null) return NotFound("لم يتم العثور على عينة مسندة لك بهذا الكود.");
        // Readiness applies to direct barcode access as well as the committee list.
        if (model != null)
        {
            var blocked = await GetReadinessErrorAsync(connection, null, model.SampleId);
            if (blocked != null) return BadRequest(blocked);
        }
        return PartialView("_ResultForm", model);
    }

    [HttpGet]
    public async Task<IActionResult> ByCommittee(long committeeId)
    {
        if (!TryUser(out var userId)) return Unauthorized("انتهت جلسة الدخول.");
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT TOP 1 sample.Sample_BarCode FROM dbo.Im_CheckRequest_SampleData sample
            JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID=sample.Im_RequestCommittee_ID
              AND employee.Employee_Id=@UserId AND employee.ISAdmin=1 AND employee.OperationType=74 AND employee.User_Deletion_Id IS NULL
            WHERE sample.Im_RequestCommittee_ID=@CommitteeId AND sample.User_Deletion_Id IS NULL AND NULLIF(LTRIM(RTRIM(sample.Sample_BarCode)),'') IS NOT NULL
            ORDER BY CASE WHEN sample.IsAccepted IS NULL THEN 0 ELSE 1 END,sample.ID
            """;
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@CommitteeId", committeeId);
        var code = Convert.ToString(await command.ExecuteScalarAsync());
        if (string.IsNullOrWhiteSpace(code)) return NotFound("لا توجد عينة مسحوبة جاهزة لتسجيل نتيجة المعمل.");
        return await Find(code, committeeId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(LaboratoryResultSaveVm input)
    {
        if (!TryUser(out var userId)) return Unauthorized("انتهت جلسة الدخول.");
        if (input.SampleId<=0 || input.Accepted==null || string.IsNullOrWhiteSpace(input.Notes))
            return BadRequest("استكمل نتيجة المعمل والملاحظات.");
        if (input.Attachment == null || input.Attachment.Length == 0) return BadRequest("يجب إرفاق صورة نتيجة المعمل.");
        if (input.Attachment.Length > MaxAttachmentBytes) return BadRequest("حجم المرفق يجب ألا يتجاوز 10 ميجابايت.");
        if (!input.Attachment.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) return BadRequest("المسموح هو ملفات الصور فقط.");
        string extension = input.Attachment.ContentType.ToLowerInvariant() switch
        {
            "image/jpeg" => ".jpg", "image/png" => ".png", "image/gif" => ".gif",
            "image/webp" => ".webp", "image/bmp" => ".bmp", _ => ".img"
        };
        string storedName = $"{Guid.NewGuid():N}{extension}";
        Directory.CreateDirectory(_attachmentRoot);
        string storedPath = Path.Combine(_attachmentRoot, storedName);
        await using (var stream = System.IO.File.Create(storedPath)) await input.Attachment.CopyToAsync(stream);

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction=(SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            var blocked = await GetReadinessErrorAsync(connection, transaction, input.SampleId);
            if (blocked != null) throw new InvalidOperationException(blocked);
            const string accessSql="""
                SELECT CAST(CASE WHEN sample.IsAccepted IS NOT NULL AND EXISTS
                  (SELECT 1 FROM dbo.A_AttachmentData attachment WHERE attachment.RowId=sample.ID
                   AND attachment.A_AttachmentTableNameId=12 AND attachment.User_Deletion_Id IS NULL)
                  THEN 1 ELSE 0 END AS bit)
                FROM dbo.Im_CheckRequest_SampleData sample
                JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID=sample.Im_RequestCommittee_ID
                  AND employee.Employee_Id=@UserId AND employee.ISAdmin=1 AND employee.OperationType=74 AND employee.User_Deletion_Id IS NULL
                WHERE sample.ID=@SampleId AND sample.User_Deletion_Id IS NULL
                """;
            await using (var access=new SqlCommand(accessSql,connection,transaction))
            {
                access.Parameters.AddWithValue("@UserId",userId); access.Parameters.AddWithValue("@SampleId",input.SampleId);
                var value=await access.ExecuteScalarAsync();
                if(value==null||value==DBNull.Value) throw new InvalidOperationException("العينة غير موجودة أو غير مسموح بتعديلها.");
                if(Convert.ToBoolean(value)) throw new InvalidOperationException("تم حفظ نتيجة المعمل من قبل وأصبحت متاحة للعرض فقط.");
            }
            const string saveSql="""
                DECLARE @Targets TABLE (ID bigint PRIMARY KEY);
                INSERT @Targets(ID)
                SELECT related.ID
                FROM dbo.Im_CheckRequest_SampleData source
                JOIN dbo.Im_CheckRequest_SampleData related WITH (UPDLOCK, HOLDLOCK)
                  ON related.Im_RequestCommittee_ID=source.Im_RequestCommittee_ID
                 AND LTRIM(RTRIM(related.Sample_BarCode))=LTRIM(RTRIM(source.Sample_BarCode))
                WHERE source.ID=@SampleId AND related.User_Deletion_Id IS NULL
                  AND NULLIF(LTRIM(RTRIM(source.Sample_BarCode)), '') IS NOT NULL;
                IF NOT EXISTS (SELECT 1 FROM @Targets) THROW 50001, N'لا توجد عينات بهذا الباركود.', 1;
                IF EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData s
                           JOIN @Targets t ON t.ID=s.ID
                           WHERE s.IsAccepted IS NOT NULL AND s.IsAccepted<>@Accepted)
                    THROW 50001, N'توجد نتيجة مختلفة محفوظة لنفس الباركود. راجع نتائج العينات قبل الحفظ.', 1;
                UPDATE dbo.Im_CheckRequest_SampleData SET IsAccepted=@Accepted,
                  Notes_Ar=@Notes, User_Updation_Id=@UserId,User_Updation_Date=GETDATE()
                WHERE ID IN (SELECT ID FROM @Targets);
                UPDATE dbo.A_AttachmentData SET User_Deletion_Id=@UserId,User_Deletion_Date=GETDATE()
                WHERE RowId IN (SELECT ID FROM @Targets) AND A_AttachmentTableNameId=12 AND User_Deletion_Id IS NULL;
                INSERT dbo.A_AttachmentData(Id,A_AttachmentTableNameId,RowId,Attachment_Number,Attachment_TypeName,AttachmentPath,User_Creation_Id,User_Creation_Date)
                SELECT NEXT VALUE FOR dbo.A_AttachmentData_SEQ,12,ID,@FileName,@ContentType,@Path,@UserId,GETDATE() FROM @Targets;
                """;
            await using var save=new SqlCommand(saveSql,connection,transaction);
            save.Parameters.AddWithValue("@Accepted",input.Accepted.Value);
            save.Parameters.AddWithValue("@Notes",input.Notes.Trim()); save.Parameters.AddWithValue("@UserId",userId); save.Parameters.AddWithValue("@SampleId",input.SampleId);
            save.Parameters.AddWithValue("@FileName",Path.GetFileName(input.Attachment.FileName)); save.Parameters.AddWithValue("@ContentType",input.Attachment.ContentType);
            save.Parameters.AddWithValue("@Path",storedPath);
            await save.ExecuteNonQueryAsync(); await transaction.CommitAsync();
            return Ok(new { saved=true, sampleId=input.SampleId, message="تم حفظ نتيجة المعمل بنجاح." });
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();
            if (System.IO.File.Exists(storedPath)) System.IO.File.Delete(storedPath);
            return BadRequest(ex.Message);
        }
    }

    internal static async Task<string?> GetReadinessErrorAsync(SqlConnection connection, SqlTransaction? transaction, long sampleId)
    {
        const string sql = """
            SELECT CASE
              WHEN committee.Is_Cancel IS NOT NULL OR ISNULL(committee.IsFinishedAll,0)=0 OR EXISTS
                (SELECT 1 FROM dbo.Im_CheckRequest_SampleData related
                 WHERE related.Im_RequestCommittee_ID=sample.Im_RequestCommittee_ID
                   AND related.User_Deletion_Id IS NULL
                   AND (related.WithdrawDate IS NULL OR ISNULL(related.SampleSize,0)<=0))
                THEN N'لا يمكن إدخال نتيجة المعمل قبل تسجيل بيانات السحب وإنهاء السحب.'
              WHEN EXISTS
                (SELECT 1 FROM dbo.CommitteeEmployee assistant
                 CROSS JOIN dbo.Im_CheckRequest_SampleData pending
                 WHERE assistant.Committee_ID=committee.ID AND assistant.ISAdmin=0
                   AND assistant.OperationType=74 AND assistant.User_Deletion_Id IS NULL
                   AND pending.Im_RequestCommittee_ID=committee.ID AND pending.User_Deletion_Id IS NULL
                   AND NOT EXISTS
                     (SELECT 1 FROM dbo.Im_CheckRequest_SampleData_Confirm opinion
                      WHERE opinion.Im_CheckRequest_SampleData_ID=pending.ID
                        AND opinion.EmployeeId=assistant.Employee_Id AND opinion.IsAccepted IS NOT NULL))
                THEN N'لا يمكن إدخال نتيجة المعمل قبل انتهاء جميع مساعدي اللجنة من تسجيل آرائهم على عينات السحب.'
              ELSE NULL END
            FROM dbo.Im_CheckRequest_SampleData sample
            JOIN dbo.Im_RequestCommittee committee ON committee.ID=sample.Im_RequestCommittee_ID
            WHERE sample.ID=@SampleId AND sample.User_Deletion_Id IS NULL AND committee.User_Deletion_Id IS NULL
            """;
        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@SampleId", sampleId);
        var result = await command.ExecuteScalarAsync();
        return result == null ? "العينة أو اللجنة غير موجودة." : result == DBNull.Value ? null : Convert.ToString(result);
    }

    private bool TryUser(out short userId)
    {
        userId = 0;
        return HttpContext.Session.GetString("UserSession")=="Authenticated"
            && short.TryParse(HttpContext.Session.GetString("UserId"),out userId);
    }
}

public class LaboratoryResultVm
{
    public long SampleId {get;set;} public long CommitteeId {get;set;} public string Barcode {get;set;}=""; public string RequestNumber {get;set;}="";
    public string PlantName {get;set;}=""; public string LotNumber {get;set;}=""; public string AnalysisType {get;set;}=""; public string Laboratory {get;set;}="";
    public bool? Accepted {get;set;} public string Notes {get;set;}=""; public long? AttachmentId {get;set;} public string AttachmentName {get;set;}="";
    public bool IsCompleted => Accepted != null && AttachmentId != null;
}
public class LaboratoryResultSaveVm { public long SampleId {get;set;} public bool? Accepted {get;set;} public string Notes {get;set;}=""; public IFormFile? Attachment {get;set;} }
