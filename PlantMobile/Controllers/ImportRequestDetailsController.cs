using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ViewModels;

namespace PlantMobile.Controllers;

public class ImportRequestDetailsController : Controller
{
    private readonly string _connectionString;
    private readonly ILogger<ImportRequestDetailsController> _logger;

    public ImportRequestDetailsController(
        IConfiguration configuration,
        ILogger<ImportRequestDetailsController> logger)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
    }

    private static async Task SaveSampleRowsAsync(ImportSamplesSaveVm input, short userId, SqlConnection connection, SqlTransaction transaction, bool regenerateBarcodes = false)
    {
            if (input.Samples.Any(sample => sample.QuantityUnit is not ("kg" or "count")
                || !double.IsFinite(sample.SampleSize) || sample.SampleSize <= 0
                || (sample.QuantityUnit == "count" && sample.SampleSize != Math.Truncate(sample.SampleSize))))
                throw new InvalidOperationException("أدخل كمية عينة صحيحة؛ وحدة عدد تتطلب عددًا صحيحًا موجبًا.");
            const string updateSql = """
                UPDATE dbo.Im_CheckRequest_SampleData
                SET WithdrawDate = CONVERT(date, GETDATE()),
                    Sample_BarCode = CASE WHEN @RegenerateBarcode = 1 THEN @Barcode ELSE COALESCE(NULLIF(Sample_BarCode, ''), @Barcode) END,
                    SampleSize = @SampleSize, SampleRatio = @SampleRatio, SampleQuantityUnit = @QuantityUnit, IS_Total = @IsTotal,
                    Notes_Ar = @Notes, Syl_ALkhatima_Number = @SealNumber,
                    IsAccepted = CASE WHEN @HasAttachment = 1 THEN @LabResultAccepted ELSE IsAccepted END,
                    Admin_Confirmation = CASE WHEN @HasAttachment = 1 THEN (SELECT Is_Continue FROM dbo.Im_CheckRequest_Lot_Result_Status WHERE ID = @QuarantineStatusId) ELSE Admin_Confirmation END,
                    Admin_User = CASE WHEN @HasAttachment = 1 THEN @UserId ELSE Admin_User END,
                    Admin_Date = CASE WHEN @HasAttachment = 1 THEN GETDATE() ELSE Admin_Date END,
                    IS_From_Android = 1, User_Updation_Id = @UserId, User_Updation_Date = GETDATE()
                WHERE ID = @SampleId AND Im_RequestCommittee_ID = @CommitteeId
                  AND (LotData_ID = @LotDataId OR (LotData_ID IS NULL AND @LotDataId IS NULL))
                  AND User_Deletion_Id IS NULL
                """;
            foreach (var group in input.Samples.GroupBy(x => x.LotDataId))
            {
                foreach (var resultSample in group.Where(x => x.HasAttachment))
                {
                    var blocked = await LaboratoryResultController.GetReadinessErrorAsync(connection, transaction, resultSample.SampleId);
                    if (blocked != null) throw new InvalidOperationException(blocked);
                }
                string barcode = await GenerateImportSampleBarcodeAsync(connection, transaction);
                foreach (var sample in group)
                {
                    if (sample.SampleId <= 0)
                    {
                        const string insertSql = """
                            INSERT INTO dbo.Im_CheckRequest_SampleData
                                (ID, AnalysisLabType_ID, Im_RequestCommittee_ID, Im_Request_Item_Id,
                                 LotData_ID, WithdrawDate, Sample_BarCode, SampleSize, SampleRatio,
                                 Notes_Ar, User_Creation_Id, User_Creation_Date, IS_Total,
                                 Item_ShortName_ID, IS_From_Android, Syl_ALkhatima_Number, Amount, SampleQuantityUnit)
                            SELECT NEXT VALUE FOR dbo.Im_CheckRequest_SampleData_SEQ,
                                   @AnalysisLabTypeId, @CommitteeId, @RequestItemId, @LotDataId,
                                   CONVERT(date, GETDATE()), @Barcode, @SampleSize, @SampleRatio,
                                   @Notes, @UserId, GETDATE(), @IsTotal, @ItemShortNameId, 1, @SealNumber,
                                   (SELECT TOP 1 Amount FROM dbo.Fees_Action WHERE ID = 9), @QuantityUnit
                            WHERE EXISTS
                                (SELECT 1 FROM dbo.AnalysisLabType
                                 WHERE ID = @AnalysisLabTypeId AND User_Deletion_Id IS NULL)
                            """;
                        await using var insert = new SqlCommand(insertSql, connection, transaction);
                        insert.Parameters.AddWithValue("@AnalysisLabTypeId", sample.AnalysisLabTypeId);
                        insert.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                        insert.Parameters.AddWithValue("@RequestItemId", sample.RequestItemId);
                        insert.Parameters.AddWithValue("@LotDataId", sample.LotDataId!.Value);
                        insert.Parameters.AddWithValue("@Barcode", barcode);
                        insert.Parameters.AddWithValue("@SampleSize", sample.SampleSize);
                        insert.Parameters.AddWithValue("@QuantityUnit", sample.QuantityUnit);
                        insert.Parameters.AddWithValue("@IsTotal", sample.IsTotal);
                        insert.Parameters.AddWithValue("@SampleRatio", sample.SampleRatio);
                        insert.Parameters.AddWithValue("@Notes", (object?)sample.Notes?.Trim() ?? DBNull.Value);
                        insert.Parameters.AddWithValue("@UserId", userId);
                        insert.Parameters.AddWithValue("@ItemShortNameId", sample.ItemShortNameId);
                        insert.Parameters.AddWithValue("@SealNumber", (object?)sample.SealNumber?.Trim() ?? DBNull.Value);
                        if (await insert.ExecuteNonQueryAsync() != 1)
                            throw new InvalidOperationException("تعذر إضافة بيانات السحبة الجديدة.");
                        continue;
                    }

                    await using var command = new SqlCommand(updateSql, connection, transaction);
                    command.Parameters.AddWithValue("@Barcode", barcode);
                    command.Parameters.AddWithValue("@RegenerateBarcode", regenerateBarcodes);
                    command.Parameters.AddWithValue("@SampleSize", sample.SampleSize);
                    command.Parameters.AddWithValue("@QuantityUnit", sample.QuantityUnit);
                    command.Parameters.AddWithValue("@IsTotal", sample.IsTotal);
                    command.Parameters.AddWithValue("@SampleRatio", sample.SampleRatio);
                    command.Parameters.AddWithValue("@Notes", (object?)sample.Notes?.Trim() ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SealNumber", (object?)sample.SealNumber?.Trim() ?? DBNull.Value);
                    command.Parameters.AddWithValue("@HasAttachment", sample.HasAttachment);
                    command.Parameters.AddWithValue("@LabResultAccepted", (object?)sample.LabResultAccepted ?? DBNull.Value);
                    command.Parameters.AddWithValue("@QuarantineStatusId", (object?)sample.QuarantineStatusId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@SampleId", sample.SampleId);
                    command.Parameters.AddWithValue("@LotDataId", (object?)sample.LotDataId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                    if (await command.ExecuteNonQueryAsync() != 1)
                        throw new InvalidOperationException($"تعذر حفظ بيانات العينة رقم {sample.SampleId}.");
                }

                var attachedSamples = group.Where(x => x.HasAttachment).ToList();
                if (group.Key != null && attachedSamples.Count > 0)
                {
                    if (attachedSamples.Select(x => x.QuarantineStatusId).Distinct().Count() != 1)
                        throw new InvalidOperationException("يجب اختيار موقف حجر واحد لكل تحاليل اللوط نفسه.");
                    const string lotStatusSql = """
                        UPDATE dbo.Im_CheckRequest_Items_Lot_Result SET IS_Status_Committee = 0
                        WHERE Im_CheckRequest_Items_Lot_Category_ID = @LotId;
                        INSERT INTO dbo.Im_CheckRequest_Items_Lot_Result
                            (ID, Im_CheckRequest_Items_Lot_Category_ID, Nots, User_Creation_Id, User_Creation_Date, IS_Status, IS_Status_Committee)
                        VALUES (NEXT VALUE FOR dbo.Im_CheckRequest_Items_Lot_Result_SEQ, @LotId, @Notes, @UserId, GETDATE(), @StatusId, 1);
                        """;
                    await using var statusCommand = new SqlCommand(lotStatusSql, connection, transaction);
                    statusCommand.Parameters.AddWithValue("@LotId", group.Key.Value);
                    statusCommand.Parameters.AddWithValue("@Notes", (object?)attachedSamples[0].Notes?.Trim() ?? DBNull.Value);
                    statusCommand.Parameters.AddWithValue("@UserId", userId);
                    statusCommand.Parameters.AddWithValue("@StatusId", attachedSamples[0].QuarantineStatusId!.Value);
                    await statusCommand.ExecuteNonQueryAsync();
                }
            }
            if (input.CommitteeTypeId == 11 && input.IsFinishedAll)
            {
                const string incompleteInspectionSql = """
                    SELECT COUNT(1)
                    FROM dbo.Im_CommitteeResult
                    WHERE Committee_ID = @CommitteeId AND User_Deletion_Id IS NULL
                      AND CommitteeResultType_ID IS NULL
                    """;
                await using var incompleteInspection = new SqlCommand(incompleteInspectionSql, connection, transaction);
                incompleteInspection.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                if (Convert.ToInt32(await incompleteInspection.ExecuteScalarAsync()) > 0)
                    throw new InvalidOperationException("لا يمكن إنهاء اللجنة قبل استكمال نتائج الفحص لجميع اللوطات.");
            }

            const string committeeSql = """
                UPDATE dbo.Im_RequestCommittee
                SET Status = CASE WHEN CommitteeType_ID = 11 THEN Status ELSE @Finished END,
                    IsFinishedAll = @Finished, Is_Start_Android = 1,
                    User_Updation_Id = @UserId, User_Updation_Date = GETDATE()
                WHERE ID = @CommitteeId AND ImCheckRequest_ID = @CheckRequestId
                  AND CommitteeType_ID = @CommitteeTypeId AND User_Deletion_Id IS NULL
                """;
            await using var committeeCommand = new SqlCommand(committeeSql, connection, transaction);
            committeeCommand.Parameters.AddWithValue("@Finished", input.IsFinishedAll);
            committeeCommand.Parameters.AddWithValue("@UserId", userId);
            committeeCommand.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
            committeeCommand.Parameters.AddWithValue("@CheckRequestId", input.CheckRequestId);
            committeeCommand.Parameters.AddWithValue("@CommitteeTypeId", input.CommitteeTypeId);
            if (await committeeCommand.ExecuteNonQueryAsync() != 1) throw new InvalidOperationException("تعذر تحديث حالة لجنة السحب.");
    }

    [HttpGet]
    public async Task<IActionResult> Index(long checkRequestId, long committeeId, byte committeeTypeId)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        if (checkRequestId <= 0 || committeeId <= 0 || committeeTypeId <= 0)
        {
            TempData["PageError"] = "بيانات طلب الوارد غير مكتملة.";
            return RedirectToAction("Index", "ImportRequests");
        }

        try
        {
            return View(await LoadPageAsync(checkRequestId, committeeId, committeeTypeId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to load import request details. CheckRequestId={CheckRequestId}, CommitteeId={CommitteeId}, CommitteeTypeId={CommitteeTypeId}; TraceId={TraceId}",
                checkRequestId, committeeId, committeeTypeId, HttpContext.TraceIdentifier);
            TempData["PageError"] = $"تعذر تحميل تفاصيل طلب الوارد: {ex.Message}";
            return RedirectToAction("Index", "ImportRequests");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveInspection(ImportInspectionSaveVm input)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        if (input.CommitteeTypeId != 11)
        {
            TempData["SaveError"] = "حفظ نتيجة الفحص وموقف الحجر متاح للجنة الفحص فقط.";
            return RedirectToAction(nameof(Index), RouteValues(input));
        }

        var access = await GetCommitteeAccessAsync(input.CheckRequestId, input.CommitteeId);
        if (input.CommitteeTypeId == 11) {
            var workflow = await LoadWorkflowStateAsync(input.CommitteeId);
            if (workflow.InspectionCount > 0 && workflow.IncompleteInspectionCount == 0) {
                TempData["SaveError"] = "اكتمل الفحص؛ البيانات للعرض فقط. موقف الحجر متاح من النافذة المنبثقة في قائمة اللجان.";
                return RedirectToAction(nameof(Index), new { checkRequestId=input.CheckRequestId, committeeId=input.CommitteeId, committeeTypeId=input.CommitteeTypeId });
            }
        }

        if (!access.IsAdmin)
        {
            TempData["SaveError"] = "المساعد لا يمكنه تعديل نتيجة الأدمن؛ المتاح له تسجيل الرأي فقط.";
            return RedirectToAction(nameof(Index), RouteValues(input));
        }
        if (access.IsReadOnly)
        {
            TempData["SaveError"] = "تم الانتهاء من هذه اللجنة وأصبحت متاحة للعرض فقط.";
            return RedirectToAction(nameof(Index), RouteValues(input));
        }

        if (input.ApplyToAll)
        {
            if (input.PlantTotals.Count > 0)
            {
                var totalsByItem = input.PlantTotals
                    .SelectMany(x => (x.RequestItemIds.Count > 0 ? x.RequestItemIds : new List<long> { x.RequestItemId })
                        .Select(id => new { Id = id, Result = x }))
                    .GroupBy(x => x.Id).ToDictionary(x => x.Key, x => x.First().Result);
                foreach (var lot in input.Lots)
                {
                    if (!totalsByItem.TryGetValue(lot.RequestItemId, out var plant)) continue;
                    lot.CommitteeResultTypeId = plant.CommitteeResultTypeId;
                    lot.Notes = plant.Notes;
                    lot.InfectionItemId = plant.InfectionItemId;
                }
            }
            else
            {
                foreach (var lot in input.Lots)
                {
                    lot.CommitteeResultTypeId = input.SharedCommitteeResultTypeId;
                    lot.Notes = input.SharedNotes;
                    lot.InfectionItemId = input.SharedInfectionItemId;
                }
            }
        }

        if (input.Lots.Count == 0 || input.Lots.Any(x => x.LotCategoryId <= 0 || x.CommitteeResultTypeId == 0
            || string.IsNullOrWhiteSpace(x.Notes)))
        {
            TempData["SaveError"] = "يجب استكمال نتيجة الفحص والملاحظات.";
            return RedirectToAction(nameof(Index), RouteValues(input));
        }

        if (!short.TryParse(HttpContext.Session.GetString("UserId"), out short userId))
        {
            TempData["SaveError"] = "تعذر تحديد رقم المستخدم. برجاء تسجيل الدخول مرة أخرى.";
            return RedirectToAction("Index", "Login");
        }

        try
        {
            var lotItems = await ResolveLotItemsAsync(input.CheckRequestId, input.Lots.Select(x => x.LotCategoryId));
            if (lotItems.Count != input.Lots.Count)
                throw new InvalidOperationException("أحد اللوطات لا يتبع طلب الوارد الحالي أو بياناته غير مكتملة.");

            if (input.PerformWithdrawal)
            {
                await ValidateSampleScopeAsync(input.CheckRequestId, input.CommitteeId, input.CommitteeTypeId, input.Samples);
                if (input.Samples.Count == 0 || input.Samples.Any(x => !double.IsFinite(x.SampleSize)
                    || x.SampleSize <= 0 || string.IsNullOrWhiteSpace(x.Notes) || x.HasAttachment))
                    throw new InvalidOperationException("أضف بيانات السحب واستكمل كمية العينة والملاحظات قبل الحفظ.");
                var newSamples = input.Samples.Where(x => x.SampleId <= 0).ToList();
                if (newSamples.Any(x => x.LotDataId.GetValueOrDefault() <= 0 || x.AnalysisLabTypeId <= 0))
                    throw new InvalidOperationException("اختر اللوط ونوع التحليل ومعمل التحليل لكل عينة.");
                var sampleLots = await ResolveLotItemsAsync(input.CheckRequestId, newSamples.Select(x => x.LotDataId!.Value));
                if (newSamples.Any(x => !sampleLots.TryGetValue(x.LotDataId!.Value, out var item)
                    || item.RequestItemId != x.RequestItemId || item.ItemShortNameId != x.ItemShortNameId))
                    throw new InvalidOperationException("أحد لوطات السحب لا يتبع طلب الوارد الحالي.");
            }

            await SaveInspectionTransactionAsync(input, userId);
            TempData["SaveSuccess"] = input.PerformWithdrawal
                ? "تم حفظ الفحص والسحب وإنشاء لجنة السحب وتوليد الباركود الجديد بنجاح."
                : "تم حفظ نتيجة الفحص بنجاح.";
            if (input.IsFinishedAll)
                return RedirectToAction("AfterCompletion", "ImportRequests", new { checkRequestId = input.CheckRequestId });
        }
        catch (Exception ex)
        {
            TempData["SaveError"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), RouteValues(input));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveQuarantine(ImportQuarantineSaveVm input)
    {
        var route = new { checkRequestId = input.CheckRequestId, committeeId = input.CommitteeId, committeeTypeId = input.CommitteeTypeId };
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !short.TryParse(HttpContext.Session.GetString("UserId"), out short userId))
            return RedirectToAction("Index", "Login");
        var access = await GetCommitteeAccessAsync(input.CheckRequestId, input.CommitteeId);
        if (input.CommitteeTypeId != 11 || !access.IsAdmin || access.IsReadOnly)
        {
            TempData["SaveError"] = "تسجيل موقف الحجر غير متاح لهذه اللجنة.";
            return RedirectToAction(nameof(Index), route);
        }
        if (input.Lots.Count == 0 || input.Lots.Any(x => x.LotCategoryId <= 0 || x.QuarantineStatusId <= 0))
        {
            TempData["SaveError"] = "يجب تحديد موقف الحجر لكل لوط.";
            return RedirectToAction(nameof(Index), route);
        }

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            const string readinessSql = """
                SELECT
                    (SELECT COUNT(*) FROM dbo.Im_CommitteeResult
                     WHERE Committee_ID=@CommitteeId AND User_Deletion_Id IS NULL
                       AND CommitteeResultType_ID IS NULL),
                    (SELECT COUNT(*) FROM dbo.Im_CheckRequest_SampleData
                     WHERE Im_RequestCommittee_ID=@CommitteeId AND User_Deletion_Id IS NULL
                       AND (SampleSize IS NULL OR SampleSize<=0))
                """;
            await using (var readiness = new SqlCommand(readinessSql, connection, transaction))
            {
                readiness.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                await using var reader = await readiness.ExecuteReaderAsync();
                await reader.ReadAsync();
                if (reader.GetInt32(0) > 0) throw new InvalidOperationException("يجب إنهاء فحص جميع اللوطات أولًا.");
                if (reader.GetInt32(1) > 0) throw new InvalidOperationException("لا يمكن تسجيل موقف الحجر قبل اكتمال جميع بيانات السحب.");
            }

            var assignedLots = await ResolveLotItemsAsync(input.CheckRequestId, input.Lots.Select(x => x.LotCategoryId));
            if (assignedLots.Count != input.Lots.Count)
                throw new InvalidOperationException("يوجد لوط لا يتبع الطلب الحالي.");

            const string statusSql = """
                UPDATE dbo.Im_CheckRequest_Items_Lot_Result SET IS_Status_Committee=0
                WHERE Im_CheckRequest_Items_Lot_Category_ID=@LotId;
                INSERT INTO dbo.Im_CheckRequest_Items_Lot_Result
                    (ID, Im_CheckRequest_Items_Lot_Category_ID, Nots, User_Creation_Id,
                     User_Creation_Date, IS_Status, IS_Status_Committee)
                VALUES (NEXT VALUE FOR dbo.Im_CheckRequest_Items_Lot_Result_SEQ,
                        @LotId, @Notes, @UserId, GETDATE(), @StatusId, 1);
                """;
            foreach (var lot in input.Lots)
            {
                await using var status = new SqlCommand(statusSql, connection, transaction);
                status.Parameters.AddWithValue("@LotId", lot.LotCategoryId);
                status.Parameters.AddWithValue("@Notes", (object?)lot.Notes?.Trim() ?? DBNull.Value);
                status.Parameters.AddWithValue("@UserId", userId);
                status.Parameters.AddWithValue("@StatusId", lot.QuarantineStatusId);
                await status.ExecuteNonQueryAsync();
            }
            const string finishSql = """
                UPDATE dbo.Im_RequestCommittee
                SET Status=1, IsFinishedAll=1, Is_Start_Android=1,
                    User_Updation_Id=@UserId, User_Updation_Date=GETDATE()
                WHERE ID=@CommitteeId AND ImCheckRequest_ID=@CheckRequestId
                  AND CommitteeType_ID=11 AND User_Deletion_Id IS NULL
                """;
            await using var finish = new SqlCommand(finishSql, connection, transaction);
            finish.Parameters.AddWithValue("@UserId", userId);
            finish.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
            finish.Parameters.AddWithValue("@CheckRequestId", input.CheckRequestId);
            if (await finish.ExecuteNonQueryAsync() != 1) throw new InvalidOperationException("تعذر إنهاء اللجنة.");
            await transaction.CommitAsync();
            TempData["SaveSuccess"] = "تم حفظ موقف الحجر وإنهاء اللجنة بنجاح.";
            return RedirectToAction("AfterCompletion", "ImportRequests", new { checkRequestId = input.CheckRequestId });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            TempData["SaveError"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), route);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveSamples(ImportSamplesSaveVm input)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !short.TryParse(HttpContext.Session.GetString("UserId"), out short userId))
            return RedirectToAction("Index", "Login");

        var route = new { checkRequestId = input.CheckRequestId, committeeId = input.CommitteeId, committeeTypeId = input.CommitteeTypeId };
        if (input.CommitteeTypeId != 13)
        {
            TempData["SaveError"] = "حفظ بيانات السحب متاح للجنة سحب العينات فقط.";
            return RedirectToAction(nameof(Index), route);
        }
        var access = await GetCommitteeAccessAsync(input.CheckRequestId, input.CommitteeId);
        if (input.CommitteeTypeId == 11) {
            var workflow = await LoadWorkflowStateAsync(input.CommitteeId);
            if (workflow.InspectionCount > 0 && workflow.IncompleteInspectionCount == 0) {
                TempData["SaveError"] = "اكتمل الفحص؛ البيانات للعرض فقط. موقف الحجر متاح من النافذة المنبثقة في قائمة اللجان.";
                return RedirectToAction(nameof(Index), new { checkRequestId=input.CheckRequestId, committeeId=input.CommitteeId, committeeTypeId=input.CommitteeTypeId });
            }
        }

        if (!access.IsAdmin || access.IsReadOnly)
        {
            TempData["SaveError"] = access.IsReadOnly ? "تم الانتهاء من لجنة السحب والبيانات للعرض فقط." : "تعديل بيانات السحب متاح لأدمن اللجنة فقط.";
            return RedirectToAction(nameof(Index), route);
        }
        input.Samples.ForEach(x => x.SampleRatio = 0);
        if (input.Samples.Any(x => x.SampleId <= 0))
        {
            TempData["SaveError"] = "لجنة السحب تسجل بيانات العينات المسندة إليها فقط؛ إضافة التحاليل متاحة من الفحص.";
            return RedirectToAction(nameof(Index), route);
        }
        if (input.Samples.Count == 0 || input.Samples.Any(x => (x.SampleId <= 0 &&
                (x.AnalysisLabTypeId <= 0 || x.LotDataId <= 0 || x.RequestItemId <= 0 || x.ItemShortNameId <= 0))
            || x.SampleSize <= 0
            || string.IsNullOrWhiteSpace(x.Notes)
            || (x.HasAttachment && (x.LabResultAccepted == null || x.QuarantineStatusId <= 0))))
        {
            TempData["SaveError"] = "يجب استكمال كمية العينة والملاحظات، واختيار نتيجة التحليل وموقف الحجر للعينات التي تم رفع مرفق لها.";
            return RedirectToAction(nameof(Index), route);
        }

        var newSamples = input.Samples.Where(x => x.SampleId <= 0).ToList();
        try
        {
            await ValidateSampleScopeAsync(input.CheckRequestId, input.CommitteeId, input.CommitteeTypeId, input.Samples);
        }
        catch (InvalidOperationException ex)
        {
            TempData["SaveError"] = ex.Message;
            return RedirectToAction(nameof(Index), route);
        }
        if (newSamples.Count > 0)
        {
            var lotItems = await ResolveLotItemsAsync(input.CheckRequestId, newSamples.Select(x => x.LotDataId!.Value));
            if (newSamples.Any(x => !lotItems.TryGetValue(x.LotDataId!.Value, out var item)
                || item.RequestItemId != x.RequestItemId || item.ItemShortNameId != x.ItemShortNameId))
            {
                TempData["SaveError"] = "أحد اللوطات المختارة لا يتبع طلب الوارد الحالي.";
                return RedirectToAction(nameof(Index), route);
            }
        }

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            await SaveSampleRowsAsync(input, userId, connection, transaction);
            await transaction.CommitAsync();
            TempData["SaveSuccess"] = "تم حفظ بيانات سحب العينات وتوليد الباركود تلقائيًا.";
            if (input.IsFinishedAll)
                return RedirectToAction("AfterCompletion", "ImportRequests", new { checkRequestId = input.CheckRequestId });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            TempData["SaveError"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), route);
    }

    [HttpGet]
    public async Task<IActionResult> ViewSampleAttachment(long id)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !long.TryParse(HttpContext.Session.GetString("UserId"), out long userId))
            return RedirectToAction("Index", "Login");

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT attachment.AttachmentPath_Binary, attachment.AttachmentPath,
                   attachment.Attachment_TypeName, attachment.Attachment_Number
            FROM dbo.A_AttachmentData attachment
            INNER JOIN dbo.Im_CheckRequest_SampleData sample ON sample.ID = attachment.RowId
            INNER JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID = sample.Im_RequestCommittee_ID
              AND employee.Employee_Id = @UserId AND employee.OperationType = 74
              AND employee.User_Deletion_Id IS NULL
            WHERE attachment.Id = @Id AND attachment.A_AttachmentTableNameId = 12
              AND attachment.User_Deletion_Id IS NULL AND sample.User_Deletion_Id IS NULL
            """;
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@UserId", userId);
        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return NotFound("المرفق غير موجود أو غير مسموح بعرضه.");

        string contentType = reader.IsDBNull(2) ? "application/octet-stream" : reader.GetString(2);
        if (!contentType.Contains('/')) contentType = contentType.ToLowerInvariant() switch
        {
            "jpg" or "jpeg" => "image/jpeg", "png" => "image/png", "pdf" => "application/pdf", _ => "application/octet-stream"
        };
        if (!reader.IsDBNull(0)) return File((byte[])reader[0], contentType);
        string path = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
        if (Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.Scheme is "http" or "https") return Redirect(path);
        if (!string.IsNullOrWhiteSpace(path) && System.IO.File.Exists(path)) return PhysicalFile(path, contentType);
        return NotFound("ملف المرفق غير متاح على الخادم.");
    }

    private static async Task<string> GenerateImportSampleBarcodeAsync(
        SqlConnection connection, SqlTransaction transaction)
    {
        DateTime now = DateTime.Now;
        string prefix = $"74{now:yyMM}";
        const string sql = """
            DECLARE @LockResult int;
            EXEC @LockResult = sys.sp_getapplock
                @Resource = @LockResource,
                @LockMode = 'Exclusive',
                @LockOwner = 'Transaction',
                @LockTimeout = 10000;

            IF @LockResult < 0
                THROW 50001, N'تعذر حجز رقم باركود جديد. حاول مرة أخرى.', 1;

            SELECT ISNULL(MAX(TRY_CONVERT(int, RIGHT(Sample_BarCode, 5))), 0) + 1
            FROM dbo.Im_CheckRequest_SampleData WITH (UPDLOCK, HOLDLOCK)
            WHERE Sample_BarCode LIKE @Prefix + '_____'
              AND LEN(Sample_BarCode) = 11;
            """;
        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@LockResource", $"ImportSampleBarcode:{prefix}");
        command.Parameters.AddWithValue("@Prefix", prefix);
        int counter = Convert.ToInt32(await command.ExecuteScalarAsync());
        if (counter > 99999)
            throw new InvalidOperationException("تم استنفاد أرقام باركودات العينات للشهر الحالي.");
        return $"{prefix}{counter:D5}";
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveSampleAssistantOpinion(SampleAssistantOpinionSaveVm input)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !short.TryParse(HttpContext.Session.GetString("UserId"), out short userId))
            return RedirectToAction("Index", "Login");
        var route = new { checkRequestId = input.CheckRequestId, committeeId = input.CommitteeId, committeeTypeId = input.CommitteeTypeId };
        var access = await GetCommitteeAccessAsync(input.CheckRequestId, input.CommitteeId);
        var opinions = input.Opinions.Where(x => x.SampleId > 0 && x.IsAccepted != null).ToList();
        if (input.CommitteeTypeId is not (11 or 13) || access.IsAdmin || opinions.Count == 0)
        {
            TempData["SaveError"] = "يجب تحديد موافق أو غير موافق لكل عينة.";
            return RedirectToAction(nameof(Index), route);
        }

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            const string sql = """
                INSERT INTO dbo.Im_CheckRequest_SampleData_Confirm
                    (ID, Im_CheckRequest_SampleData_ID, [Date], EmployeeId, Notes, IsAccepted)
                SELECT NEXT VALUE FOR dbo.Im_CheckRequest_SampleData_Confirm_SEQ,
                       sample.ID, GETDATE(), @UserId, @Notes, @IsAccepted
                FROM dbo.Im_CheckRequest_SampleData sample
                WHERE sample.ID = @SampleId AND sample.Im_RequestCommittee_ID = @CommitteeId
                  AND sample.Sample_BarCode IS NOT NULL
                  AND sample.SampleSize > 0
                  AND sample.User_Deletion_Id IS NULL
                  AND NOT EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData_Confirm confirm
                                  WHERE confirm.Im_CheckRequest_SampleData_ID = sample.ID AND confirm.EmployeeId = @UserId)
                """;
            foreach (var opinion in opinions)
            {
                await using var command = new SqlCommand(sql, connection, transaction);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Notes", (object?)opinion.Notes?.Trim() ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsAccepted", opinion.IsAccepted!.Value);
                command.Parameters.AddWithValue("@SampleId", opinion.SampleId);
                command.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                if (await command.ExecuteNonQueryAsync() != 1)
                    throw new InvalidOperationException("تعذر حفظ رأي المساعد أو تم تسجيله من قبل.");
            }
            await transaction.CommitAsync();
            TempData["SaveSuccess"] = "تم حفظ موافقة المساعد على بيانات السحب بنجاح.";
            return RedirectToAction("AfterCompletion", "ImportRequests", new { checkRequestId = input.CheckRequestId });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            TempData["SaveError"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), route);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveAssistantOpinion(AssistantOpinionSaveVm input)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !short.TryParse(HttpContext.Session.GetString("UserId"), out short userId))
            return RedirectToAction("Index", "Login");

        var access = await GetCommitteeAccessAsync(input.CheckRequestId, input.CommitteeId);
        var submittedOpinions = input.Opinions.Where(x => x.ResultId > 0 && x.IsAccepted != null).ToList();
        if (access.IsAdmin || access.IsReadOnly || submittedOpinions.Count == 0)
        {
            TempData["SaveError"] = access.IsReadOnly ? "تم تسجيل رأيك على هذه اللجنة من قبل." : "يجب تحديد موافق أو غير موافق لكل نتيجة.";
            return RedirectToAction(nameof(Index), new { checkRequestId = input.CheckRequestId, committeeId = input.CommitteeId, committeeTypeId = input.CommitteeTypeId });
        }

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            const string sql = """
                INSERT INTO dbo.Im_CommitteeResult_Confirm
                    (ID, Im_CommitteeResult_ID, [Date], EmployeeId, Notes, IsAccepted)
                SELECT NEXT VALUE FOR dbo.Im_CommitteeResult_Confirm_SEQ,
                       result.ID, GETDATE(), @UserId, @Notes, @IsAccepted
                FROM dbo.Im_CommitteeResult result
                WHERE result.ID = @ResultId AND result.Committee_ID = @CommitteeId
                  AND result.CommitteeResultType_ID IS NOT NULL
                  AND NOT EXISTS
                    (SELECT 1 FROM dbo.Im_CommitteeResult_Confirm confirm
                     WHERE confirm.Im_CommitteeResult_ID = result.ID AND confirm.EmployeeId = @UserId)
                """;
            foreach (var opinion in submittedOpinions)
            {
                await using var command = new SqlCommand(sql, connection, transaction);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Notes", (object?)opinion.Notes?.Trim() ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsAccepted", opinion.IsAccepted!.Value);
                command.Parameters.AddWithValue("@ResultId", opinion.ResultId);
                command.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                if (await command.ExecuteNonQueryAsync() != 1)
                    throw new InvalidOperationException("تعذر حفظ الرأي أو تم تسجيله من قبل.");
            }
            await transaction.CommitAsync();
            TempData["SaveSuccess"] = "تم حفظ رأي المساعد بنجاح.";
            return RedirectToAction("AfterCompletion", "ImportRequests", new { checkRequestId = input.CheckRequestId });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            TempData["SaveError"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { checkRequestId = input.CheckRequestId, committeeId = input.CommitteeId, committeeTypeId = input.CommitteeTypeId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveTreatment(ImportTreatmentSaveVm input)
    {
        var route = new { checkRequestId = input.CheckRequestId, committeeId = input.CommitteeId, committeeTypeId = input.CommitteeTypeId };
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !long.TryParse(HttpContext.Session.GetString("UserId"), out long userId))
            return RedirectToAction("Index", "Login");

        if (input.CommitteeTypeId != 14)
        {
            TempData["SaveError"] = "حفظ بيان المعالجة متاح للجنة المعالجة فقط.";
            return RedirectToAction(nameof(Index), route);
        }

        var access = await GetCommitteeAccessAsync(input.CheckRequestId, input.CommitteeId);
        if (!access.IsAdmin || access.IsReadOnly)
        {
            TempData["SaveError"] = access.IsReadOnly
                ? "تم الانتهاء من لجنة المعالجة والبيانات متاحة للعرض فقط."
                : "تسجيل بيان المعالجة متاح لأدمن اللجنة فقط.";
            return RedirectToAction(nameof(Index), route);
        }

        if (input.Records.Count == 0 || input.Records.Any(x => x.TreatmentDataId <= 0 || x.Size <= 0 ||
            x.MaterialAmount <= 0 || x.Dose <= 0 || x.ExposureDay < 0 || x.ExposureHour < 0 ||
            x.ExposureHour > 23 || x.ExposureMinute < 0 || x.ExposureMinute > 59))
        {
            TempData["SaveError"] = "يرجى استكمال حجم الرسالة وكمية المادة والجرعة ومدة التعرض بصورة صحيحة.";
            return RedirectToAction(nameof(Index), route);
        }

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            const string updateSql = """
                UPDATE dbo.Im_Request_TreatmentData
                SET Size = @Size, TreatmentMat_Amount = @MaterialAmount, TheDose = @Dose,
                    Temperature = @Temperature, ThermalSealNumber = @ThermalSealNumber,
                    Exposure_Day = @ExposureDay, Exposure_Hour = @ExposureHour,
                    Exposure_Minute = @ExposureMinute, Note = @Notes,
                    IS_Total = @IsTotal, IS_Total_Android = @IsTotal, IS_From_Android = 1,
                    User_Updation_Id = @UserId, User_Updation_Date = GETDATE()
                WHERE ID = @TreatmentDataId AND Im_RequestCommittee_ID = @CommitteeId
                  AND User_Deletion_Id IS NULL
                """;
            foreach (var record in input.Records)
            {
                await using var command = new SqlCommand(updateSql, connection, transaction);
                command.Parameters.AddWithValue("@Size", record.Size);
                command.Parameters.AddWithValue("@MaterialAmount", record.MaterialAmount);
                command.Parameters.AddWithValue("@Dose", record.Dose);
                command.Parameters.AddWithValue("@Temperature", (object?)record.Temperature ?? DBNull.Value);
                command.Parameters.AddWithValue("@ThermalSealNumber", (object?)record.ThermalSealNumber ?? DBNull.Value);
                command.Parameters.AddWithValue("@ExposureDay", record.ExposureDay);
                command.Parameters.AddWithValue("@ExposureHour", record.ExposureHour);
                command.Parameters.AddWithValue("@ExposureMinute", record.ExposureMinute);
                command.Parameters.AddWithValue("@Notes", (object?)record.Notes?.Trim() ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsTotal", input.ApplyToAll);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@TreatmentDataId", record.TreatmentDataId);
                command.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                if (await command.ExecuteNonQueryAsync() != 1)
                    throw new InvalidOperationException($"تعذر حفظ سجل المعالجة رقم {record.TreatmentDataId}.");
            }

            const string committeeSql = """
                UPDATE dbo.Im_RequestCommittee
                SET Status = @Finished, IsFinishedAll = @Finished, Is_Start_Android = 1,
                    User_Updation_Id = @UserId, User_Updation_Date = GETDATE()
                WHERE ID = @CommitteeId AND ImCheckRequest_ID = @CheckRequestId
                  AND CommitteeType_ID = 14 AND User_Deletion_Id IS NULL
                """;
            await using var committee = new SqlCommand(committeeSql, connection, transaction);
            committee.Parameters.AddWithValue("@Finished", input.IsFinishedAll);
            committee.Parameters.AddWithValue("@UserId", userId);
            committee.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
            committee.Parameters.AddWithValue("@CheckRequestId", input.CheckRequestId);
            if (await committee.ExecuteNonQueryAsync() != 1)
                throw new InvalidOperationException("تعذر تحديث حالة لجنة المعالجة.");

            await transaction.CommitAsync();
            TempData["SaveSuccess"] = "تم حفظ بيان المعالجة بنجاح.";
            if (input.IsFinishedAll)
                return RedirectToAction("AfterCompletion", "ImportRequests", new { checkRequestId = input.CheckRequestId });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            TempData["SaveError"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), route);
    }

    private async Task<ImportRequestDetailsPageVm> LoadPageAsync(long checkRequestId, long committeeId, byte committeeTypeId)
    {
        var access = await GetCommitteeAccessAsync(checkRequestId, committeeId);
        if (committeeTypeId == 13 && access.IsAdmin && !access.IsReadOnly)
            await EnsureSampleBarcodesAsync(committeeId);

        var detailsTask = LoadDetailsWithSamplesAsync(checkRequestId, committeeId, committeeTypeId);
#if false // Replaced by direct SQL loading.
        var client = _httpClientFactory.CreateClient();
        var detailsResponse = await client.GetAsync(
            $"{ImportApi}?CheckRequest_Id={checkRequestId}&Committee_Id={committeeId}&Committee_Type_Id={committeeTypeId}");
        detailsResponse.EnsureSuccessStatusCode();
        var details = await detailsResponse.Content.ReadFromJsonAsync<Ex_CheckRequest_GetData_Android_V2_VM>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("لم يتم إرجاع بيانات للطلب.");

#endif
#if false // Replaced by direct SQL loading.
        if (committeeTypeId is 11 or 13)
        {
            using var statusRequest = new HttpRequestMessage(HttpMethod.Get, FinalResultApi);
            statusRequest.Headers.Add("lang", "1");
            var statusResponse = await client.SendAsync(statusRequest);
            statusResponse.EnsureSuccessStatusCode();
            statuses = await statusResponse.Content.ReadFromJsonAsync<List<QuarantineStatusVm>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            statuses = statuses.Where(x => x.Value > 0).ToList();
        }

#endif
        var statusesTask = committeeTypeId is 11 or 13
            ? LoadQuarantineStatusesAsync(committeeTypeId)
            : Task.FromResult(new List<QuarantineStatusVm>());

        // These read-only lookups use separate connections and can run together.
        var sampleStatusesTask = committeeTypeId == 13
            ? statusesTask
            : committeeTypeId == 11 ? LoadQuarantineStatusesAsync(13) : Task.FromResult(new List<QuarantineStatusVm>());
        var analysisLabsTask = committeeTypeId is 11 or 13
            ? LoadAnalysisLabOptionsAsync() : Task.FromResult(new List<AnalysisLabOptionVm>());
        var infectionItemsTask = committeeTypeId == 11 && access.IsAdmin
            ? LoadInfectionItemsAsync() : Task.FromResult(new List<InfectionItemVm>());
        var workflowTask = LoadWorkflowStateAsync(committeeId);
        var resultTypesTask = committeeTypeId == 11
            ? LoadCommitteeResultTypesAsync() : Task.FromResult(new List<CommitteeResultTypeOptionVm>());
        var assistantResultsTask = LoadAssistantResultsAsync(committeeId, long.Parse(HttpContext.Session.GetString("UserId")!));
        await Task.WhenAll(detailsTask, statusesTask, sampleStatusesTask, analysisLabsTask, infectionItemsTask, workflowTask, resultTypesTask, assistantResultsTask);
        var workflow = await workflowTask;
        return new ImportRequestDetailsPageVm
        {
            Details = await detailsTask,
            CheckRequestId = checkRequestId,
            CommitteeId = committeeId,
            CommitteeTypeId = committeeTypeId,
            UserId = HttpContext.Session.GetString("UserId") ?? string.Empty,
            UserName = HttpContext.Session.GetString("UserName") ?? string.Empty,
            QuarantineStatuses = await statusesTask,
            SampleQuarantineStatuses = await sampleStatusesTask,
            AnalysisLabOptions = await analysisLabsTask,
            IsReadOnly = access.IsReadOnly,
            IsAdmin = access.IsAdmin,
            InspectionComplete = workflow.InspectionCount > 0 && workflow.IncompleteInspectionCount == 0,
            HasSamples = workflow.SampleCount > 0,
            SamplesComplete = workflow.SampleCount > 0 && workflow.IncompleteSampleCount == 0,
            InfectionItems = await infectionItemsTask,
            CommitteeResultTypes = await resultTypesTask,
            AssistantResults = await assistantResultsTask
        };
    }

    private async Task<Ex_CheckRequest_GetData_Android_V2_VM> LoadDetailsWithSamplesAsync(
        long checkRequestId, long committeeId, byte committeeTypeId)
    {
        var details = await LoadDetailsFromDatabaseAsync(checkRequestId, committeeId, committeeTypeId);
        if (committeeTypeId is 11 or 13)
            await MergeSavedSampleDataAsync(details, committeeId);
        return details;
    }

    private async Task<CommitteeWorkflowState> LoadWorkflowStateAsync(long committeeId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT
                (SELECT COUNT(*) FROM dbo.Im_CommitteeResult WHERE Committee_ID=@CommitteeId AND User_Deletion_Id IS NULL),
                (SELECT COUNT(*) FROM dbo.Im_CommitteeResult WHERE Committee_ID=@CommitteeId AND User_Deletion_Id IS NULL AND CommitteeResultType_ID IS NULL),
                (SELECT COUNT(*) FROM dbo.Im_CheckRequest_SampleData WHERE Im_RequestCommittee_ID=@CommitteeId AND User_Deletion_Id IS NULL),
                (SELECT COUNT(*) FROM dbo.Im_CheckRequest_SampleData WHERE Im_RequestCommittee_ID=@CommitteeId AND User_Deletion_Id IS NULL AND (SampleSize IS NULL OR SampleSize<=0))
            """;
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CommitteeId", committeeId);
        await using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        return new CommitteeWorkflowState(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3));
    }

    private async Task<Ex_CheckRequest_GetData_Android_V2_VM> LoadDetailsFromDatabaseAsync(
        long checkRequestId, long committeeId, byte committeeTypeId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string headerSql = """
            SELECT request.ID, request.CheckRequest_Number, request.IsAccepted,
                   data.ImporterType_Id,
                   COALESCE(
                       NULLIF(permissionImporter.ImporterName COLLATE Arabic_CI_AS, N''),
                       NULLIF(data.DelegateName COLLATE Arabic_CI_AS, N''),
                       N'غير محدد') COLLATE Arabic_CI_AS,
                   COALESCE(data.DelegateAddress, N''),
                   COALESCE(country.Ar_Name, N''), COALESCE(outlet.Ar_Name, N''),
                   COALESCE(outlet.Address_Ar, N''), COALESCE(type.Name_Ar, N''),
                   committee.Delegation_Date,
                   CASE WHEN committee.Is_Cancel IS NOT NULL THEN N'ملغي'
                        WHEN committee.Status = 1 OR committee.IsFinishedAll = 1 THEN N'تم الانتهاء'
                        WHEN committee.Is_Start_Android = 1 OR committee.User_Updation_Date IS NOT NULL THEN N'جاري العمل'
                        ELSE N'جديد' END,
                   operation.ID, COALESCE(operation.Name_Ar, N''), COALESCE(data.Ship_Name, N'')
            FROM dbo.Im_CheckRequest request
            INNER JOIN dbo.Im_RequestCommittee committee ON committee.ID = @CommitteeId
                AND committee.ImCheckRequest_ID = request.ID AND committee.User_Deletion_Id IS NULL
            LEFT JOIN dbo.Im_CheckRequest_Data data ON data.Im_CheckRequest_ID = request.ID
                AND data.User_Deletion_Id IS NULL
            LEFT JOIN dbo.Country country ON country.ID = data.ExportCountry_Id
            LEFT JOIN dbo.Outlet outlet ON outlet.ID = request.Outlet_ID
            LEFT JOIN dbo.CommitteeType type ON type.ID = committee.CommitteeType_ID
            LEFT JOIN dbo.Im_OpertaionType operation ON operation.ID = request.Im_OperationType
            OUTER APPLY
            (
                SELECT TOP (1) permission.ImporterName
                FROM dbo.View_List_Im_PermissionRequest permission
                WHERE permission.Im_CheckRequest_ID = request.ID
                ORDER BY permission.Im_PermissionRequest_ID DESC
            ) permissionImporter
            WHERE request.ID = @CheckRequestId AND committee.CommitteeType_ID = @CommitteeTypeId
            """;
        var details = new Ex_CheckRequest_GetData_Android_V2_VM();
        await using (var command = new SqlCommand(headerSql, connection))
        {
            command.Parameters.AddWithValue("@CheckRequestId", checkRequestId);
            command.Parameters.AddWithValue("@CommitteeId", committeeId);
            command.Parameters.AddWithValue("@CommitteeTypeId", committeeTypeId);
            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                throw new InvalidOperationException("الطلب غير موجود أو غير مسموح بعرض هذه اللجنة.");

            details.CheckRequest_Id = reader.GetInt64(0);
            details.CheckRequest_Number = DbString(reader, 1);
            details.IsAcceppted = reader.IsDBNull(2) ? null : reader.GetBoolean(2);
            details.ImporterType_Id = reader.IsDBNull(3) ? null : reader.GetInt32(3);
            details.Reciever_Name = DbString(reader, 4);
            details.ImportCompany_Address = DbString(reader, 5);
            details.ExportCountry_Name = DbString(reader, 6);
            details.Outlet_Name = DbString(reader, 7);
            details.Outlet_Address = DbString(reader, 8);
            details.Committee_Type = DbString(reader, 9);
            details.Check_Date = reader.IsDBNull(10) ? null : Convert.ToDateTime(reader.GetValue(10));
            details.RequestCommittee_Status = DbString(reader, 11);
            details.Opreration_type_Id = reader.IsDBNull(12) ? null : Convert.ToInt32(reader.GetValue(12));
            details.Opreration_type_Name = DbString(reader, 13);
            details.Ship_Name = DbString(reader, 14);
            details.IsExport = 2;
        }

        const string itemsSql = """
            SELECT item.ID, item.Item_ShortName_ID, shortName.Item_Type_ID,
                   COALESCE(plant.Name_Ar, shortName.ShortName_Ar, N''),
                   COALESCE(plant.Scientific_Name, N''), COALESCE(shortName.ShortName_Ar, N''),
                   item.Im_CheckRequset_Shipping_Method_ID
            FROM dbo.Im_CheckRequest_Items item
            INNER JOIN dbo.Im_CheckRequset_Shipping_Method shipping
                ON shipping.ID = item.Im_CheckRequset_Shipping_Method_ID
            LEFT JOIN dbo.Item_ShortName shortName ON shortName.ID = item.Item_ShortName_ID
            LEFT JOIN dbo.Item plant ON plant.ID = shortName.Item_ID
            WHERE shipping.Im_CheckRequest_ID = @CheckRequestId
              AND shipping.User_Deletion_Id IS NULL AND item.User_Deletion_Id IS NULL
            ORDER BY item.ID
            """;
        var itemMap = new Dictionary<long, _x0040_Item_Data>();
        await using (var command = new SqlCommand(itemsSql, connection))
        {
            command.Parameters.AddWithValue("@CheckRequestId", checkRequestId);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var item = new _x0040_Item_Data
                {
                    ID = reader.GetInt64(0),
                    Item_ShortName_id = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                    Item_Type_ID = reader.IsDBNull(2) ? null : reader.GetByte(2),
                    Item_Name = DbString(reader, 3),
                    Scientific_Name = DbString(reader, 4),
                    Item_ShortName_Name = DbString(reader, 5),
                    Im_CheckRequset_Shipping_Method_ID = reader.IsDBNull(6) ? null : reader.GetInt64(6),
                    IsExport = 2,
                    Lot_Data = new List<_x0040_temp_table_Lot>(),
                    Sample_Data = new List<Itemsample_data>(),
                    TreatmentLot = new List<TreatmentDataDTO>()
                };
                itemMap[item.ID] = item;
                details.Item_Data.Add(item);
            }
        }

        const string lotsSql = """
            SELECT lot.ID, lot.Im_CheckRequest_Items_ID, COALESCE(lot.Lot_Number, CONVERT(nvarchar(30), lot.ID)),
                   COALESCE(CONVERT(decimal(28, 10), NULLIF(LTRIM(RTRIM(lot.PackagesCount)), N'')), 0),
                   COALESCE(lot.Net_Weight, item.Net_Weight), COALESCE(lot.GrossWeight, item.GrossWeight),
                   COALESCE(lot.Package_Weight, item.Package_Weight), COALESCE(lot.Units_Number, item.Units_Number),
                   lot.Size, COALESCE(packageType.Ar_Name, N''), COALESCE(lot.Grower_Number, N''),
                   COALESCE(lot.Number_Wooden_Package, N''),
                   CAST(CASE WHEN result.IS_Total = 1 THEN 1 ELSE 0 END AS bit)
            FROM dbo.Im_CheckRequest_Items_Lot_Category lot
            INNER JOIN dbo.Im_CheckRequest_Items item ON item.ID = lot.Im_CheckRequest_Items_ID
            INNER JOIN dbo.Im_CheckRequset_Shipping_Method shipping
                ON shipping.ID = item.Im_CheckRequset_Shipping_Method_ID
            LEFT JOIN dbo.Package_Type packageType ON packageType.ID = COALESCE(lot.Package_Type_ID, item.Package_Type_ID)
            OUTER APPLY (SELECT TOP 1 r.IS_Total FROM dbo.Im_CommitteeResult r
                         WHERE r.Committee_ID = @CommitteeId AND (r.LotData_ID = lot.ID OR r.Im_Request_Item_Id = item.ID)
                         ORDER BY r.ID DESC) result
            WHERE shipping.Im_CheckRequest_ID = @CheckRequestId
              AND lot.User_Deletion_Id IS NULL AND item.User_Deletion_Id IS NULL
              AND (@CommitteeTypeId <> 11 OR EXISTS
                  (SELECT 1
                   FROM dbo.Im_CommitteeResult assignedResult
                   WHERE assignedResult.Committee_ID = @CommitteeId
                     AND assignedResult.User_Deletion_Id IS NULL
                     AND (assignedResult.LotData_ID = lot.ID
                          OR (assignedResult.LotData_ID IS NULL
                              AND assignedResult.Im_Request_Item_Id = item.ID))))
              AND (@CommitteeTypeId <> 13 OR EXISTS
                  (SELECT 1 FROM dbo.Im_CheckRequest_SampleData assignedSample
                   WHERE assignedSample.Im_RequestCommittee_ID = @CommitteeId AND assignedSample.User_Deletion_Id IS NULL
                     AND (assignedSample.LotData_ID = lot.ID OR (assignedSample.LotData_ID IS NULL AND assignedSample.Im_Request_Item_Id = item.ID))))
            ORDER BY lot.ID
            """;
        await using (var command = new SqlCommand(lotsSql, connection))
        {
            command.Parameters.AddWithValue("@CheckRequestId", checkRequestId);
            command.Parameters.AddWithValue("@CommitteeId", committeeId);
            command.Parameters.AddWithValue("@CommitteeTypeId", committeeTypeId);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                long requestItemId = reader.GetInt64(1);
                if (!itemMap.TryGetValue(requestItemId, out var item)) continue;
                ((List<_x0040_temp_table_Lot>)item.Lot_Data).Add(new _x0040_temp_table_Lot
                {
                    ID = reader.GetInt64(0), Lot_ID = reader.GetInt64(0), Lot_Number = DbString(reader, 2),
                    Package_Count = reader.GetDecimal(3), Net_Weight = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                    Gross_Weight = reader.IsDBNull(5) ? null : reader.GetDecimal(5),
                    Package_Weight = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                    Units_Number = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                    Size = reader.IsDBNull(8) ? null : Convert.ToDouble(reader.GetValue(8)),
                    Package_Type_Name = DbString(reader, 9), Grower_Number = DbString(reader, 10),
                    Number_Wooden_Package = DbString(reader, 11), is_Total = reader.GetBoolean(12)
                });
            }
        }

        await LoadSamplesFromDatabaseAsync(connection, committeeId, itemMap);
        await LoadTreatmentsFromDatabaseAsync(connection, committeeId, itemMap);
        return details;
    }

    private async Task LoadSamplesFromDatabaseAsync(
        SqlConnection connection, long committeeId, Dictionary<long, _x0040_Item_Data> itemMap)
    {
        const string sql = """
            SELECT sample.ID, sample.Im_Request_Item_Id, sample.LotData_ID,
                   COALESCE(lab.Name_Ar, N''), COALESCE(analysis.Name_Ar, N''),
                   COALESCE(sample.Sample_BarCode, N''), sample.SampleRatio, sample.SampleSize,
                   sample.IS_From_Android, sample.IS_Total, COALESCE(lot.Lot_Number, N''),
                   COALESCE(sample.Syl_ALkhatima_Number, N''), COALESCE(sample.Notes_Ar, N''), sample.AnalysisLabType_ID
            FROM dbo.Im_CheckRequest_SampleData sample
            LEFT JOIN dbo.AnalysisLabType labType ON labType.ID = sample.AnalysisLabType_ID
            LEFT JOIN dbo.AnalysisLab lab ON lab.ID = labType.AnalysisLabID
            LEFT JOIN dbo.AnalysisType analysis ON analysis.ID = labType.AnalysisTypeID
            LEFT JOIN dbo.Im_CheckRequest_Items_Lot_Category lot ON lot.ID = sample.LotData_ID
            WHERE sample.Im_RequestCommittee_ID = @CommitteeId AND sample.User_Deletion_Id IS NULL
            ORDER BY sample.ID
            """;
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CommitteeId", committeeId);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            long requestItemId = reader.GetInt64(1);
            if (!itemMap.TryGetValue(requestItemId, out var item)) continue;
            ((List<Itemsample_data>)item.Sample_Data).Add(new Itemsample_data
            {
                Sample_dataId = reader.GetInt64(0), LotData_ID = reader.IsDBNull(2) ? null : reader.GetInt64(2),
                AnalysisLab_Name = DbString(reader, 3), AnalysisType_Name = DbString(reader, 4),
                Sample_BarCode = DbString(reader, 5), SampleRatio = reader.IsDBNull(6) ? null : Convert.ToDouble(reader.GetValue(6)),
                SampleSize = reader.IsDBNull(7) ? null : Convert.ToDouble(reader.GetValue(7)),
                IS_From_Android = reader.IsDBNull(8) ? null : reader.GetBoolean(8),
                IS_Total = reader.IsDBNull(9) ? null : reader.GetBoolean(9), Lotnum = DbString(reader, 10),
                Syl_ALkhatima_Number = DbString(reader, 11), Notes_Ar = DbString(reader, 12), AnalysisLabTypeId = reader.GetInt32(13)
            });
        }
    }

    private async Task LoadTreatmentsFromDatabaseAsync(
        SqlConnection connection, long committeeId, Dictionary<long, _x0040_Item_Data> itemMap)
    {
        const string sql = """
            SELECT treatment.ID, treatment.Im_Request_Item_Id, treatment.Im_Request_LotData_ID,
                   COALESCE(lot.Lot_Number, N''), treatment.TreatmentMethod_ID,
                   COALESCE(method.Ar_Name, N''), COALESCE(type.Ar_Name, N''),
                   COALESCE(materialItem.Name_Ar, N''), treatment.TreatmentMat_Amount,
                   treatment.Item_ShortName_ID, treatment.Size, treatment.TheDose,
                   treatment.Exposure_Hour, treatment.Exposure_Minute, treatment.Exposure_Day,
                   treatment.Temperature, treatment.ThermalSealNumber, treatment.IS_Total,
                   treatment.IS_Total_Android, treatment.IS_From_Android, COALESCE(treatment.Note, N''),
                   COALESCE(treatment.Procedures, N'')
            FROM dbo.Im_Request_TreatmentData treatment
            LEFT JOIN dbo.Im_CheckRequest_Items_Lot_Category lot ON lot.ID = treatment.Im_Request_LotData_ID
            LEFT JOIN dbo.TreatmentMethods method ON method.ID = treatment.TreatmentMethod_ID
            LEFT JOIN dbo.TreatmentType type ON type.ID = treatment.TreatmentType_ID
            LEFT JOIN dbo.TreatmentMaterial material ON material.ID = treatment.TreatmentMat_ID
            LEFT JOIN dbo.Item materialItem ON materialItem.ID = material.Item_ID
            WHERE treatment.Im_RequestCommittee_ID = @CommitteeId AND treatment.User_Deletion_Id IS NULL
            ORDER BY treatment.ID
            """;
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CommitteeId", committeeId);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            long requestItemId = reader.GetInt64(1);
            if (!itemMap.TryGetValue(requestItemId, out var item)) continue;
            ((List<TreatmentDataDTO>)item.TreatmentLot).Add(new TreatmentDataDTO
            {
                TreatmentDataID = reader.GetInt64(0), ID = reader.GetInt64(0),
                RequestLotData_ID = reader.IsDBNull(2) ? null : reader.GetInt64(2), lot_ID = reader.IsDBNull(2) ? 0 : reader.GetInt64(2),
                lot_Number = DbString(reader, 3), treatment_MethodId = reader.GetByte(4), treatment_MethodName = DbString(reader, 5),
                treatment_TypeName = DbString(reader, 6), treatmentMaterial_Name = DbString(reader, 7),
                treatmentMat_Amount = reader.IsDBNull(8) ? null : reader.GetDecimal(8),
                Item_ShortName_ID = reader.IsDBNull(9) ? null : reader.GetInt64(9), size = reader.IsDBNull(10) ? null : reader.GetDecimal(10),
                dose = reader.IsDBNull(11) ? null : reader.GetDecimal(11), exposure_Hour = reader.IsDBNull(12) ? null : reader.GetInt32(12),
                exposure_Minute = reader.IsDBNull(13) ? null : reader.GetInt32(13), exposure_Day = reader.IsDBNull(14) ? null : reader.GetInt32(14),
                temperature = reader.IsDBNull(15) ? null : reader.GetDecimal(15), thermalSealNumber = reader.IsDBNull(16) ? null : reader.GetDecimal(16),
                IS_Total = reader.IsDBNull(17) ? null : reader.GetBoolean(17), IS_Total_Android = reader.IsDBNull(18) ? null : reader.GetBoolean(18),
                IS_From_Android = reader.IsDBNull(19) ? null : reader.GetBoolean(19), Notes = DbString(reader, 20), Procedures = DbString(reader, 21),
                RequestCommittee_ID = committeeId
            });
        }
    }

    private async Task<List<QuarantineStatusVm>> LoadQuarantineStatusesAsync(byte committeeTypeId)
    {
        var statuses = new List<QuarantineStatusVm>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT ID, Is_Continue, COALESCE(Name_AR, Name_En, CONVERT(nvarchar(20), ID))
            FROM dbo.Im_CheckRequest_Lot_Result_Status
            WHERE ID > 0 AND ISNULL(IsActive, 1) = 1
            ORDER BY ID
            """;
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            statuses.Add(new QuarantineStatusVm
            {
                Value = reader.GetInt32(0),
                Value2 = reader.IsDBNull(1) ? null : reader.GetBoolean(1),
                DisplayText = DbString(reader, 2)
            });
        return statuses;
    }

    private async Task<List<AnalysisLabOptionVm>> LoadAnalysisLabOptionsAsync()
    {
        var options = new List<AnalysisLabOptionVm>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT link.ID, analysis.ID, COALESCE(analysis.Name_Ar, analysis.Name_En, N''),
                   lab.ID, COALESCE(lab.Name_Ar, lab.Name_En, N'')
            FROM dbo.AnalysisLabType link
            INNER JOIN dbo.AnalysisType analysis ON analysis.ID = link.AnalysisTypeID
            INNER JOIN dbo.AnalysisLab lab ON lab.ID = link.AnalysisLabID
            WHERE link.User_Deletion_Id IS NULL AND analysis.User_Deletion_Id IS NULL
              AND lab.User_Deletion_Id IS NULL AND analysis.IsActive = 1 AND lab.IsActive = 1
            ORDER BY analysis.Name_Ar, lab.Name_Ar
            """;
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            options.Add(new AnalysisLabOptionVm
            {
                AnalysisLabTypeId = reader.GetInt32(0), AnalysisTypeId = reader.GetInt32(1),
                AnalysisTypeName = DbString(reader, 2), AnalysisLabId = reader.GetInt32(3),
                AnalysisLabName = DbString(reader, 4)
            });
        return options;
    }

    private static string DbString(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? string.Empty : Convert.ToString(reader.GetValue(ordinal)) ?? string.Empty;

    private async Task MergeSavedSampleDataAsync(Ex_CheckRequest_GetData_Android_V2_VM details, long committeeId)
    {
        var samples = details.Item_Data?
            .SelectMany(x => x.Sample_Data ?? Enumerable.Empty<Itemsample_data>())
            .ToDictionary(x => x.Sample_dataId) ?? new Dictionary<long, Itemsample_data>();
        if (samples.Count == 0) return;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT sample.ID, sample.Sample_BarCode, sample.SampleSize, sample.SampleRatio,
                   sample.Syl_ALkhatima_Number, sample.Notes_Ar, sample.LotData_ID, sample.WithdrawDate,
                   confirm.IsAccepted, confirm.Notes,
                   CAST(CASE WHEN attachment.Id IS NULL THEN 0 ELSE 1 END AS bit),
                   sample.IsAccepted, sample.Admin_Confirmation,
                   attachment.Id, COALESCE(attachment.Attachment_Number, attachment.Attachment_TypeName),
                   lotStatus.IS_Status, lotStatus.Name_AR, COALESCE(sample.SampleQuantityUnit, N'kg')
            FROM dbo.Im_CheckRequest_SampleData sample
            LEFT JOIN dbo.Im_CheckRequest_SampleData_Confirm confirm
              ON confirm.Im_CheckRequest_SampleData_ID = sample.ID AND confirm.EmployeeId = @UserId
            OUTER APPLY (SELECT TOP 1 a.Id, a.Attachment_Number, a.Attachment_TypeName
                         FROM dbo.A_AttachmentData a
                         WHERE a.RowId = sample.ID AND a.A_AttachmentTableNameId = 12
                           AND a.User_Deletion_Id IS NULL ORDER BY a.Id DESC) attachment
            OUTER APPLY (SELECT TOP 1 lr.IS_Status, status.Name_AR
                         FROM dbo.Im_CheckRequest_Items_Lot_Result lr
                         LEFT JOIN dbo.Im_CheckRequest_Lot_Result_Status status ON status.ID = lr.IS_Status
                         WHERE lr.Im_CheckRequest_Items_Lot_Category_ID = sample.LotData_ID
                           AND lr.IS_Status_Committee = 1 ORDER BY lr.ID DESC) lotStatus
            WHERE sample.Im_RequestCommittee_ID = @CommitteeId AND sample.User_Deletion_Id IS NULL
            """;
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CommitteeId", committeeId);
        command.Parameters.AddWithValue("@UserId", long.TryParse(HttpContext.Session.GetString("UserId"), out long currentUserId) ? currentUserId : 0);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            long id = reader.GetInt64(0);
            if (!samples.TryGetValue(id, out var sample)) continue;
            sample.Sample_BarCode = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            sample.SampleSize = reader.IsDBNull(2) ? null : reader.GetDouble(2);
            sample.QuantityUnit = reader.GetString(17);
            sample.SampleRatio = reader.IsDBNull(3) ? null : reader.GetDouble(3);
            sample.Syl_ALkhatima_Number = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
            sample.Notes_Ar = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
            sample.LotData_ID = reader.IsDBNull(6) ? null : reader.GetInt64(6);
            sample.AssistantAccepted = reader.IsDBNull(8) ? null : reader.GetBoolean(8);
            sample.AssistantNotes = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
            sample.HasAttachment = reader.GetBoolean(10);
            sample.LabResultAccepted = reader.IsDBNull(11) ? null : reader.GetBoolean(11);
            sample.QuarantineAccepted = reader.IsDBNull(12) ? null : reader.GetBoolean(12);
            sample.AttachmentId = reader.IsDBNull(13) ? null : reader.GetInt64(13);
            sample.AttachmentName = reader.IsDBNull(14) ? string.Empty : reader.GetString(14);
            sample.QuarantineStatusId = reader.IsDBNull(15) ? null : reader.GetInt32(15);
            sample.QuarantineStatusName = reader.IsDBNull(16) ? string.Empty : reader.GetString(16);
        }
    }

    private async Task EnsureSampleBarcodesAsync(long committeeId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        try
        {
            var sampleGroups = new List<(long SampleId, long? LotId)>();
            const string selectSql = """
                SELECT ID, LotData_ID
                FROM dbo.Im_CheckRequest_SampleData WITH (UPDLOCK, HOLDLOCK)
                WHERE Im_RequestCommittee_ID = @CommitteeId
                  AND User_Deletion_Id IS NULL
                  AND (NULLIF(LTRIM(RTRIM(Sample_BarCode)), '') IS NULL OR Sample_BarCode LIKE '72%')
                ORDER BY ID
                """;
            await using (var select = new SqlCommand(selectSql, connection, transaction))
            {
                select.Parameters.AddWithValue("@CommitteeId", committeeId);
                await using var reader = await select.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    sampleGroups.Add((reader.GetInt64(0), reader.IsDBNull(1) ? null : reader.GetInt64(1)));
            }

            const string updateSql = """
                UPDATE dbo.Im_CheckRequest_SampleData
                SET Sample_BarCode = @Barcode, IS_From_Android = 1,
                    User_Updation_Id = @UserId, User_Updation_Date = GETDATE()
                WHERE ID = @SampleId AND Im_RequestCommittee_ID = @CommitteeId
                  AND (NULLIF(LTRIM(RTRIM(Sample_BarCode)), '') IS NULL OR Sample_BarCode LIKE '72%')
                """;
            short.TryParse(HttpContext.Session.GetString("UserId"), out short userId);
            foreach (var group in sampleGroups.GroupBy(x => x.LotId))
            {
                string barcode = await GenerateImportSampleBarcodeAsync(connection, transaction);
                foreach (var sample in group)
                {
                    await using var update = new SqlCommand(updateSql, connection, transaction);
                    update.Parameters.AddWithValue("@Barcode", barcode);
                    update.Parameters.AddWithValue("@UserId", userId);
                    update.Parameters.AddWithValue("@SampleId", sample.SampleId);
                    update.Parameters.AddWithValue("@CommitteeId", committeeId);
                    await update.ExecuteNonQueryAsync();
                }
            }
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<List<CommitteeResultTypeOptionVm>> LoadCommitteeResultTypesAsync()
    {
        var options = new List<CommitteeResultTypeOptionVm>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT ID, COALESCE(NULLIF(Name_Ar, N''), NULLIF(Name_En, N''), CONVERT(nvarchar(10), ID))
            FROM dbo.CommitteeResultType
            WHERE IsActive = 1 AND User_Deletion_Id IS NULL AND ID > 0
            ORDER BY ID
            """;
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            options.Add(new CommitteeResultTypeOptionVm { Id = reader.GetByte(0), Name = reader.GetString(1) });
        return options;
    }
    private async Task<List<InfectionItemVm>> LoadInfectionItemsAsync()
    {
        var items = new List<InfectionItemVm>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT item.ID, item.Name_Ar, item.Scientific_Name,
                   item.Group_ID, grp.Name_Ar AS GroupName,
                   item.Family_ID, family.Name_Ar AS FamilyName
            FROM dbo.Item item
            LEFT JOIN dbo.[Group] grp ON grp.ID = item.Group_ID
            LEFT JOIN dbo.Family family ON family.ID = item.Family_ID
            WHERE item.User_Deletion_Id IS NULL
              AND NULLIF(LTRIM(RTRIM(item.Name_Ar)), '') IS NOT NULL
            ORDER BY grp.Name_Ar, family.Name_Ar, item.Name_Ar
            """;
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            items.Add(new InfectionItemVm
            {
                ItemId = reader.GetInt64(0),
                ItemName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                ScientificName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                GroupId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                GroupName = reader.IsDBNull(4) ? "غير محدد" : reader.GetString(4),
                FamilyId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                FamilyName = reader.IsDBNull(6) ? "غير محدد" : reader.GetString(6)
            });
        return items;
    }

    private static object RouteValues(ImportInspectionSaveVm input) => new
    {
        checkRequestId = input.CheckRequestId,
        committeeId = input.CommitteeId,
        committeeTypeId = input.CommitteeTypeId
    };

    private async Task<Dictionary<long, LotItemIdentity>> ResolveLotItemsAsync(
        long checkRequestId, IEnumerable<long> lotIds)
    {
        var result = new Dictionary<long, LotItemIdentity>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = """
            SELECT lot.ID, item.ID AS RequestItemId, item.Item_ShortName_ID
            FROM dbo.Im_CheckRequest_Items_Lot_Category lot
            INNER JOIN dbo.Im_CheckRequest_Items item ON item.ID = lot.Im_CheckRequest_Items_ID
            INNER JOIN dbo.Im_CheckRequset_Shipping_Method shipping
                ON shipping.ID = item.Im_CheckRequset_Shipping_Method_ID
            WHERE lot.ID = @LotId AND shipping.Im_CheckRequest_ID = @CheckRequestId
            """;

        foreach (long lotId in lotIds.Distinct())
        {
            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@LotId", lotId);
            command.Parameters.AddWithValue("@CheckRequestId", checkRequestId);
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                result[lotId] = new LotItemIdentity(reader.GetInt64(1), reader.GetInt64(2));
        }

        return result;
    }

    private async Task ValidateSampleScopeAsync(long checkRequestId, long committeeId, byte committeeTypeId, List<ImportSampleSaveVm> samples)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT lot.ID, item.ID, item.Item_ShortName_ID
            FROM dbo.Im_CheckRequest_Items_Lot_Category lot
            JOIN dbo.Im_CheckRequest_Items item ON item.ID=lot.Im_CheckRequest_Items_ID AND item.User_Deletion_Id IS NULL
            JOIN dbo.Im_CheckRequset_Shipping_Method shipping ON shipping.ID=item.Im_CheckRequset_Shipping_Method_ID AND shipping.User_Deletion_Id IS NULL
            WHERE shipping.Im_CheckRequest_ID=@RequestId AND lot.User_Deletion_Id IS NULL
              AND ((@Type=11 AND EXISTS (SELECT 1 FROM dbo.Im_CommitteeResult r
                    WHERE r.Committee_ID=@CommitteeId AND r.User_Deletion_Id IS NULL
                      AND (r.LotData_ID=lot.ID OR (r.LotData_ID IS NULL AND r.Im_Request_Item_Id=item.ID))))
                OR (@Type=13 AND EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData s
                    WHERE s.Im_RequestCommittee_ID=@CommitteeId AND s.User_Deletion_Id IS NULL
                      AND (s.LotData_ID=lot.ID OR (s.LotData_ID IS NULL AND s.Im_Request_Item_Id=item.ID)))))
            """;
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@RequestId", checkRequestId);
        command.Parameters.AddWithValue("@CommitteeId", committeeId);
        command.Parameters.AddWithValue("@Type", committeeTypeId);
        var allowed = new Dictionary<long, LotItemIdentity>();
        await using (var reader = await command.ExecuteReaderAsync())
            while (await reader.ReadAsync()) allowed[reader.GetInt64(0)] = new(reader.GetInt64(1), reader.GetInt64(2));
        await using var existingCommand = new SqlCommand("SELECT ID, LotData_ID, Im_Request_Item_Id, AnalysisLabType_ID FROM dbo.Im_CheckRequest_SampleData WHERE Im_RequestCommittee_ID=@CommitteeId AND User_Deletion_Id IS NULL", connection);
        existingCommand.Parameters.AddWithValue("@CommitteeId", committeeId);
        var existingSamples = new Dictionary<long, (long? LotId, long ItemId, int AnalysisId)>();
        await using (var reader = await existingCommand.ExecuteReaderAsync())
            while (await reader.ReadAsync()) existingSamples[reader.GetInt64(0)] = (reader.IsDBNull(1) ? null : reader.GetInt64(1), reader.GetInt64(2), reader.GetInt32(3));
        foreach (var sample in samples.Where(s => s.SampleId > 0))
        {
            if (!existingSamples.TryGetValue(sample.SampleId, out var existing) || existing.LotId != sample.LotDataId)
                throw new InvalidOperationException("العينة أو اللوط لا يتبع لجنة السحب الحالية.");
            sample.RequestItemId = existing.ItemId;
            sample.AnalysisLabTypeId = existing.AnalysisId;
        }
        foreach (var sample in samples.Where(s => s.SampleId <= 0))
            if (!allowed.TryGetValue(sample.LotDataId ?? 0, out var item)
                || item.RequestItemId != sample.RequestItemId || item.ItemShortNameId != sample.ItemShortNameId)
                throw new InvalidOperationException("السحب متاح فقط على اللوطات المسندة إلى هذه اللجنة.");
        foreach (var analysis in samples.Where(s => s.IsTotal && s.SampleId <= 0).GroupBy(s => s.AnalysisLabTypeId))
        {
            var covered = samples.Where(s => s.IsTotal && s.AnalysisLabTypeId == analysis.Key && s.LotDataId.HasValue)
                .Select(s => s.LotDataId!.Value).ToHashSet();
            foreach (var wholeItem in samples.Where(s => s.IsTotal && s.AnalysisLabTypeId == analysis.Key && s.SampleId > 0 && !s.LotDataId.HasValue))
                covered.UnionWith(allowed.Where(lot => lot.Value.RequestItemId == wholeItem.RequestItemId).Select(lot => lot.Key));
            if (!covered.SetEquals(allowed.Keys))
                throw new InvalidOperationException("السحب الكلي يجب أن يشمل جميع لوطات اللجنة للتحليل المختار.");
        }
    }

    private async Task<CommitteeAccess> GetCommitteeAccessAsync(long checkRequestId, long committeeId)
    {
        long.TryParse(HttpContext.Session.GetString("UserId"), out long userId);
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT employee.ISAdmin,
              CASE WHEN committee.Is_Cancel IS NOT NULL THEN 1
                   WHEN employee.ISAdmin = 1 AND committee.CommitteeType_ID <> 13
                        AND ((committee.CommitteeType_ID = 11 AND (committee.Status = 1 OR committee.IsFinishedAll = 1))
                             OR (committee.CommitteeType_ID <> 11 AND (committee.Status = 1 OR committee.IsFinishedAll = 1))) THEN 1
                   WHEN employee.ISAdmin = 1 AND committee.CommitteeType_ID = 13
                        AND (committee.Status = 1 OR committee.IsFinishedAll = 1)
                   THEN 1
                   WHEN employee.ISAdmin = 0 AND NOT EXISTS
                     (SELECT 1 FROM dbo.Im_CheckRequest_SampleData sample
                      WHERE committee.CommitteeType_ID = 13 AND sample.Im_RequestCommittee_ID = committee.ID
                        AND sample.Sample_BarCode IS NOT NULL AND sample.User_Deletion_Id IS NULL
                        AND NOT EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData_Confirm confirm
                                        WHERE confirm.Im_CheckRequest_SampleData_ID = sample.ID AND confirm.EmployeeId = @UserId))
                     AND committee.CommitteeType_ID = 13
                     AND EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData sample WHERE sample.Im_RequestCommittee_ID = committee.ID AND sample.Sample_BarCode IS NOT NULL AND sample.User_Deletion_Id IS NULL)
                   THEN 1
                   WHEN employee.ISAdmin = 0 AND committee.CommitteeType_ID <> 13 AND NOT EXISTS
                     (SELECT 1 FROM dbo.Im_CommitteeResult result
                      WHERE result.Committee_ID = committee.ID AND result.CommitteeResultType_ID IS NOT NULL
                        AND NOT EXISTS (SELECT 1 FROM dbo.Im_CommitteeResult_Confirm confirm
                                        WHERE confirm.Im_CommitteeResult_ID = result.ID AND confirm.EmployeeId = @UserId))
                     AND EXISTS (SELECT 1 FROM dbo.Im_CommitteeResult result WHERE result.Committee_ID = committee.ID AND result.CommitteeResultType_ID IS NOT NULL)
                     AND NOT EXISTS
                       (SELECT 1 FROM dbo.Im_CheckRequest_SampleData sample
                        WHERE committee.CommitteeType_ID = 11 AND sample.Im_RequestCommittee_ID = committee.ID
                          AND sample.Sample_BarCode IS NOT NULL AND sample.User_Deletion_Id IS NULL
                          AND NOT EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData_Confirm confirm
                                          WHERE confirm.Im_CheckRequest_SampleData_ID = sample.ID AND confirm.EmployeeId = @UserId))
                   THEN 1 ELSE 0 END AS IsReadOnly
            FROM dbo.Im_RequestCommittee committee
            INNER JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID = committee.ID
              AND employee.Employee_Id = @UserId AND employee.OperationType = 74 AND employee.User_Deletion_Id IS NULL
            WHERE committee.ID = @CommitteeId AND committee.ImCheckRequest_ID = @CheckRequestId
            """;
        await using var command = new SqlCommand(PlantMobile.Infrastructure.ImportCommitteeEligibility.Filter(sql), connection);
        command.Parameters.AddWithValue("@CommitteeId", committeeId);
        command.Parameters.AddWithValue("@CheckRequestId", checkRequestId);
        command.Parameters.AddWithValue("@UserId", userId);
        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync()
            ? new CommitteeAccess(Convert.ToBoolean(reader[0]), Convert.ToBoolean(reader[1]))
            : new CommitteeAccess(false, true);
    }

    private async Task<List<AssistantResultVm>> LoadAssistantResultsAsync(long committeeId, long userId)
    {
        var results = new List<AssistantResultVm>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT result.ID, result.LotData_ID, lot.Lot_Number, type.Name_Ar,
                   result.Notes, result.IS_Total, confirm.IsAccepted, confirm.Notes AS ConfirmNotes,
                    lotStatus.Name_AR AS QuarantineStatus,
                    CAST(CASE
                        WHEN ISNULL(result.IS_Total, 0) = 1 THEN
                            CASE WHEN
                                EXISTS (
                                    SELECT 1 FROM dbo.Im_RequestCommittee inspectionCommittee
                                    WHERE inspectionCommittee.ImCheckRequest_ID = requestCommittee.ImCheckRequest_ID
                                      AND inspectionCommittee.CommitteeType_ID = 11
                                      AND inspectionCommittee.User_Deletion_Id IS NULL
                                      AND inspectionCommittee.IsFinishedAll = 1)
                                AND EXISTS (
                                    SELECT 1 FROM dbo.Im_RequestCommittee withdrawalCommittee
                                    WHERE withdrawalCommittee.ImCheckRequest_ID = requestCommittee.ImCheckRequest_ID
                                      AND withdrawalCommittee.CommitteeType_ID IN (11, 13)
                                      AND withdrawalCommittee.User_Deletion_Id IS NULL
                                      AND withdrawalCommittee.IsFinishedAll = 1
                                      AND EXISTS (
                                          SELECT 1 FROM dbo.Im_CheckRequest_SampleData completedSample
                                          WHERE completedSample.Im_RequestCommittee_ID = withdrawalCommittee.ID
                                            AND completedSample.Sample_BarCode IS NOT NULL
                                            AND completedSample.SampleSize > 0
                                            AND completedSample.User_Deletion_Id IS NULL))
                                THEN 1 ELSE 0 END
                        ELSE
                            CASE WHEN requestCommittee.IsFinishedAll = 1
                                AND EXISTS (
                                    SELECT 1
                                    FROM dbo.Im_RequestCommittee withdrawalCommittee
                                    INNER JOIN dbo.Im_CheckRequest_SampleData completedSample
                                      ON completedSample.Im_RequestCommittee_ID = withdrawalCommittee.ID
                                     AND completedSample.LotData_ID = result.LotData_ID
                                     AND completedSample.Sample_BarCode IS NOT NULL
                                     AND completedSample.SampleSize > 0
                                     AND completedSample.User_Deletion_Id IS NULL
                                    WHERE withdrawalCommittee.ImCheckRequest_ID = requestCommittee.ImCheckRequest_ID
                                      AND withdrawalCommittee.CommitteeType_ID IN (11, 13)
                                      AND withdrawalCommittee.User_Deletion_Id IS NULL
                                      AND withdrawalCommittee.IsFinishedAll = 1)
                                THEN 1 ELSE 0 END
                    END AS bit) AS CanShowQuarantine,
                   result.Im_Request_Item_Id AS RequestItemId
            FROM dbo.Im_CommitteeResult result
            INNER JOIN dbo.Im_RequestCommittee requestCommittee ON requestCommittee.ID = result.Committee_ID
            LEFT JOIN dbo.Im_CheckRequest_Items_Lot_Category lot ON lot.ID = result.LotData_ID
            LEFT JOIN dbo.CommitteeResultType type ON type.ID = result.CommitteeResultType_ID
            LEFT JOIN dbo.Im_CommitteeResult_Confirm confirm ON confirm.Im_CommitteeResult_ID = result.ID AND confirm.EmployeeId = @UserId
            OUTER APPLY (SELECT TOP 1 status.Name_AR FROM dbo.Im_CheckRequest_Items_Lot_Result lr
                         INNER JOIN dbo.Im_CheckRequest_Lot_Result_Status status ON status.ID = lr.IS_Status
                         WHERE lr.Im_CheckRequest_Items_Lot_Category_ID = result.LotData_ID ORDER BY lr.ID DESC) lotStatus
            WHERE result.Committee_ID = @CommitteeId AND result.CommitteeResultType_ID IS NOT NULL
            ORDER BY result.ID
            """;
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CommitteeId", committeeId);
        command.Parameters.AddWithValue("@UserId", userId);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            results.Add(new AssistantResultVm
            {
                ResultId = reader.GetInt64(0), LotId = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                LotNumber = reader.IsDBNull(2) ? "—" : reader.GetString(2), ResultName = reader.IsDBNull(3) ? "—" : reader.GetString(3),
                AdminNotes = reader.IsDBNull(4) ? string.Empty : reader.GetString(4), IsTotal = !reader.IsDBNull(5) && reader.GetBoolean(5),
                ExistingAccepted = reader.IsDBNull(6) ? null : reader.GetBoolean(6), ExistingNotes = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                QuarantineStatus = reader.IsDBNull(8) ? "—" : reader.GetString(8),
                CanShowQuarantine = reader.GetBoolean(9),
                RequestItemId = reader.IsDBNull(10) ? 0 : reader.GetInt64(10)
            });
        return results;
    }

    private static async Task CreateInspectionWithdrawalAsync(ImportInspectionSaveVm input, short userId,
        SqlConnection connection, SqlTransaction transaction)
    {
        const string createSql = """
            DECLARE @WithdrawalId bigint = NEXT VALUE FOR dbo.Im_RequestCommittee_SEQ;
            INSERT INTO dbo.Im_RequestCommittee
                (ID, ImCheckRequest_ID, CommitteeType_ID, ImCommitteeCheckLocation_ID,
                 Delegation_Date, StartTime, EndTime, IsFinishedAll, IsApproved, Status,
                 Is_Start_Android, User_Creation_Id, User_Creation_Date)
            SELECT @WithdrawalId, ImCheckRequest_ID, 13, ImCommitteeCheckLocation_ID,
                   CONVERT(date, GETDATE()), StartTime, EndTime, 0, IsApproved, 0,
                   1, @UserId, GETDATE()
            FROM dbo.Im_RequestCommittee
            WHERE ID = @CommitteeId AND ImCheckRequest_ID = @CheckRequestId
              AND CommitteeType_ID = 11 AND User_Deletion_Id IS NULL AND Is_Cancel IS NULL;
            IF @@ROWCOUNT <> 1 THROW 50001, N'تعذر إنشاء لجنة السحب من لجنة الفحص الحالية.', 1;
            INSERT INTO dbo.CommitteeEmployee
                (Committee_ID, Employee_Id, ISAdmin, OperationType, User_Creation_Id, User_Creation_Date)
            SELECT @WithdrawalId, Employee_Id, ISAdmin, OperationType, @UserId, GETDATE()
            FROM dbo.CommitteeEmployee
            WHERE Committee_ID = @CommitteeId AND OperationType = 74 AND User_Deletion_Id IS NULL;
            SELECT @WithdrawalId;
            """;
        await using var create = new SqlCommand(createSql, connection, transaction);
        create.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
        create.Parameters.AddWithValue("@CheckRequestId", input.CheckRequestId);
        create.Parameters.AddWithValue("@UserId", userId);
        long withdrawalId = Convert.ToInt64(await create.ExecuteScalarAsync());

        // Existing inspection samples retain their analysis metadata, but get a new
        // barcode and belong to the new withdrawal committee after this transaction.
        const string moveSql = """
            UPDATE dbo.Im_CheckRequest_SampleData
            SET Im_RequestCommittee_ID = @WithdrawalId
            WHERE ID = @SampleId AND Im_RequestCommittee_ID = @CommitteeId
              AND (LotData_ID = @LotDataId OR (LotData_ID IS NULL AND @LotDataId IS NULL))
              AND User_Deletion_Id IS NULL AND IsAccepted IS NULL
              AND NOT EXISTS (SELECT 1 FROM dbo.A_AttachmentData attachment
                              WHERE attachment.RowId = @SampleId AND attachment.A_AttachmentTableNameId = 12
                                AND attachment.User_Deletion_Id IS NULL);
            """;
        foreach (var sample in input.Samples.Where(x => x.SampleId > 0))
        {
            await using var move = new SqlCommand(moveSql, connection, transaction);
            move.Parameters.AddWithValue("@WithdrawalId", withdrawalId);
            move.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
            move.Parameters.AddWithValue("@SampleId", sample.SampleId);
            move.Parameters.AddWithValue("@LotDataId", (object?)sample.LotDataId ?? DBNull.Value);
            if (await move.ExecuteNonQueryAsync() != 1)
                throw new InvalidOperationException("تعذر نقل العينة إلى لجنة السحب الجديدة.");
        }
        await using var remaining = new SqlCommand("SELECT COUNT(*) FROM dbo.Im_CheckRequest_SampleData WHERE Im_RequestCommittee_ID = @CommitteeId AND User_Deletion_Id IS NULL", connection, transaction);
        remaining.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
        if (Convert.ToInt32(await remaining.ExecuteScalarAsync()) != 0)
            throw new InvalidOperationException("يجب استكمال بيانات جميع عينات السحب قبل حفظ الفحص والسحب.");

        input.Samples.ForEach(x => x.SampleRatio = 0);
        await SaveSampleRowsAsync(new ImportSamplesSaveVm
        {
            CheckRequestId = input.CheckRequestId, CommitteeId = withdrawalId,
            CommitteeTypeId = 13, IsFinishedAll = true, Samples = input.Samples
        }, userId, connection, transaction, regenerateBarcodes: true);
    }

    private async Task SaveInspectionTransactionAsync(ImportInspectionSaveVm input, short userId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

        try
        {
            // Serialize saves for this inspection so retries cannot create duplicate committees.
            const string lockSql = """
                SELECT COUNT(*) FROM dbo.Im_RequestCommittee committee WITH (UPDLOCK, HOLDLOCK)
                WHERE committee.ID = @CommitteeId AND committee.ImCheckRequest_ID = @CheckRequestId
                  AND committee.CommitteeType_ID = 11 AND committee.User_Deletion_Id IS NULL
                  AND committee.Is_Cancel IS NULL AND ISNULL(committee.IsFinishedAll, 0) = 0
                  AND ISNULL(committee.Status, 0) = 0
                  AND EXISTS (SELECT 1 FROM dbo.CommitteeEmployee employee
                              WHERE employee.Committee_ID = committee.ID AND employee.Employee_Id = @UserId
                                AND employee.OperationType = 74 AND employee.ISAdmin = 1
                                AND employee.User_Deletion_Id IS NULL)
                  AND EXISTS (SELECT 1 FROM dbo.Im_CommitteeResult result
                              WHERE result.Committee_ID = committee.ID AND result.User_Deletion_Id IS NULL
                                AND result.CommitteeResultType_ID IS NULL);
                """;
            await using var guard = new SqlCommand(lockSql, connection, transaction);
            guard.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
            guard.Parameters.AddWithValue("@CheckRequestId", input.CheckRequestId);
            guard.Parameters.AddWithValue("@UserId", userId);
            if (Convert.ToInt32(await guard.ExecuteScalarAsync()) != 1)
                throw new InvalidOperationException("تم حفظ الفحص بالفعل أو لم تعد اللجنة متاحة للتعديل.");

            foreach (byte resultTypeId in input.Lots.Select(x => x.CommitteeResultTypeId).Distinct())
            {
                const string validTypeSql = "SELECT COUNT(*) FROM dbo.CommitteeResultType WHERE ID = @Id AND ID > 0 AND IsActive = 1 AND User_Deletion_Id IS NULL";
                await using var validType = new SqlCommand(validTypeSql, connection, transaction);
                validType.Parameters.AddWithValue("@Id", resultTypeId);
                if (Convert.ToInt32(await validType.ExecuteScalarAsync()) != 1)
                    throw new InvalidOperationException("نتيجة الفحص المختارة غير متاحة. أعد تحميل الصفحة واختر نتيجة فعالة.");
            }
            const string updateResultSql = """
                UPDATE dbo.Im_CommitteeResult
                SET CommitteeResultType_ID = @ResultTypeId,
                    EmployeeId = @UserId,
                    [Date] = GETDATE(),
                    QuantitySize = @QuantitySize,
                    [Weight] = @Weight,
                    Notes = @Notes,
                    IS_Total_Android = @IsTotal,
                    User_Updation_Id = @UserId,
                    User_Updation_Date = GETDATE()
                WHERE Committee_ID = @CommitteeId
                  AND (
                      LotData_ID = @LotId
                      OR (
                          @IsTotal = 1
                          AND LotData_ID IS NULL
                          AND Im_Request_Item_Id = (
                              SELECT lot.Im_CheckRequest_Items_ID
                              FROM dbo.Im_CheckRequest_Items_Lot_Category lot
                              WHERE lot.ID = @LotId)
                          AND NOT EXISTS (
                              SELECT 1 FROM dbo.Im_CommitteeResult exactResult
                              WHERE exactResult.Committee_ID = @CommitteeId
                                AND exactResult.LotData_ID = @LotId
                                AND exactResult.User_Deletion_Id IS NULL)
                      )
                  )
                  AND User_Deletion_Id IS NULL
                """;

            const string closeOldStatusesSql = """
                UPDATE dbo.Im_CheckRequest_Items_Lot_Result
                SET IS_Status_Committee = 0
                WHERE Im_CheckRequest_Items_Lot_Category_ID = @LotId
                """;

            const string insertStatusSql = """
                INSERT INTO dbo.Im_CheckRequest_Items_Lot_Result
                    (ID, Im_CheckRequest_Items_Lot_Category_ID, Nots,
                     User_Creation_Id, User_Creation_Date, IS_Status, IS_Status_Committee)
                VALUES
                    (NEXT VALUE FOR dbo.Im_CheckRequest_Items_Lot_Result_SEQ,
                     @LotId, @Notes, @UserId, GETDATE(), @StatusId, 1)
                """;

            const string clearInfectionsSql = """
                UPDATE infection
                SET User_Deletion_Id = @UserId, User_Deletion_Date = GETDATE()
                FROM dbo.Im_CommitteeResult_Infection infection
                INNER JOIN dbo.Im_CommitteeResult result ON result.ID = infection.Im_CommitteeResult_ID
                WHERE result.Committee_ID = @CommitteeId
                  AND (result.LotData_ID = @LotId
                       OR (@IsTotal = 1 AND result.LotData_ID IS NULL
                           AND result.Im_Request_Item_Id = (
                               SELECT lot.Im_CheckRequest_Items_ID
                               FROM dbo.Im_CheckRequest_Items_Lot_Category lot
                               WHERE lot.ID = @LotId)
                           AND NOT EXISTS (
                               SELECT 1 FROM dbo.Im_CommitteeResult exactResult
                               WHERE exactResult.Committee_ID = @CommitteeId
                                 AND exactResult.LotData_ID = @LotId
                                 AND exactResult.User_Deletion_Id IS NULL)))
                  AND infection.User_Deletion_Id IS NULL
                """;

            const string insertInfectionSql = """
                INSERT INTO dbo.Im_CommitteeResult_Infection
                    (ID, Im_CommitteeResult_ID, Item_ID, User_Creation_Id, User_Creation_Date)
                SELECT NEXT VALUE FOR dbo.Im_CommitteeResult_Infection_SEQ,
                       result.ID, @InfectionItemId, @UserId, GETDATE()
                FROM dbo.Im_CommitteeResult result
                WHERE result.Committee_ID = @CommitteeId
                  AND (result.LotData_ID = @LotId
                       OR (@IsTotal = 1 AND result.LotData_ID IS NULL
                           AND result.Im_Request_Item_Id = (
                               SELECT lot.Im_CheckRequest_Items_ID
                               FROM dbo.Im_CheckRequest_Items_Lot_Category lot
                               WHERE lot.ID = @LotId)
                           AND NOT EXISTS (
                               SELECT 1 FROM dbo.Im_CommitteeResult exactResult
                               WHERE exactResult.Committee_ID = @CommitteeId
                                 AND exactResult.LotData_ID = @LotId
                                 AND exactResult.User_Deletion_Id IS NULL)))
                  AND result.User_Deletion_Id IS NULL
                """;

            foreach (var lot in input.Lots)
            {
                await using var updateResult = new SqlCommand(updateResultSql, connection, transaction);
                updateResult.Parameters.AddWithValue("@ResultTypeId", lot.CommitteeResultTypeId);
                updateResult.Parameters.AddWithValue("@UserId", userId);
                updateResult.Parameters.AddWithValue("@QuantitySize", (object?)lot.QuantitySize ?? DBNull.Value);
                updateResult.Parameters.AddWithValue("@Weight", (object?)lot.Weight ?? DBNull.Value);
                updateResult.Parameters.AddWithValue("@Notes", lot.Notes.Trim());
                updateResult.Parameters.AddWithValue("@IsTotal", lot.IsTotal);
                updateResult.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                updateResult.Parameters.AddWithValue("@LotId", lot.LotCategoryId);
                int updatedResults = await updateResult.ExecuteNonQueryAsync();
                if (updatedResults != 1)
                    throw new InvalidOperationException($"لم يتم العثور على سجل نتيجة الفحص للوط {lot.LotCategoryId} داخل اللجنة {input.CommitteeId}.");

                if (lot.QuarantineStatusId > 0)
                {
                    await using var closeStatuses = new SqlCommand(closeOldStatusesSql, connection, transaction);
                    closeStatuses.Parameters.AddWithValue("@LotId", lot.LotCategoryId);
                    await closeStatuses.ExecuteNonQueryAsync();

                    await using var insertStatus = new SqlCommand(insertStatusSql, connection, transaction);
                    insertStatus.Parameters.AddWithValue("@LotId", lot.LotCategoryId);
                    insertStatus.Parameters.AddWithValue("@Notes", lot.Notes.Trim());
                    insertStatus.Parameters.AddWithValue("@UserId", userId);
                    insertStatus.Parameters.AddWithValue("@StatusId", lot.QuarantineStatusId);
                    if (await insertStatus.ExecuteNonQueryAsync() != 1)
                        throw new InvalidOperationException($"تعذر إدخال موقف الحجر للوط {lot.LotCategoryId}.");
                }

                await using var clearInfections = new SqlCommand(clearInfectionsSql, connection, transaction);
                clearInfections.Parameters.AddWithValue("@UserId", userId);
                clearInfections.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                clearInfections.Parameters.AddWithValue("@LotId", lot.LotCategoryId);
                clearInfections.Parameters.AddWithValue("@IsTotal", lot.IsTotal);
                if (lot.CommitteeResultTypeId != 3 || lot.InfectionItemId > 0)
                    await clearInfections.ExecuteNonQueryAsync();

                if (lot.CommitteeResultTypeId == 3 && lot.InfectionItemId > 0)
                {
                    await using var insertInfection = new SqlCommand(insertInfectionSql, connection, transaction);
                    insertInfection.Parameters.AddWithValue("@InfectionItemId", lot.InfectionItemId);
                    insertInfection.Parameters.AddWithValue("@UserId", userId);
                    insertInfection.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                    insertInfection.Parameters.AddWithValue("@LotId", lot.LotCategoryId);
                    insertInfection.Parameters.AddWithValue("@IsTotal", lot.IsTotal);
                    if (await insertInfection.ExecuteNonQueryAsync() != 1)
                        throw new InvalidOperationException($"تعذر حفظ نوع الإصابة للوط {lot.LotCategoryId}.");
                }
            }

            if (input.PerformWithdrawal)
                await CreateInspectionWithdrawalAsync(input, userId, connection, transaction);

            if (input.IsFinishedAll)
            {
                const string incompleteSamplesSql = """
                    SELECT COUNT(1)
                    FROM dbo.Im_CommitteeResult
                    WHERE Committee_ID = @CommitteeId
                      AND User_Deletion_Id IS NULL
                      AND CommitteeResultType_ID IS NULL
                    """;
                await using var incompleteSamples = new SqlCommand(incompleteSamplesSql, connection, transaction);
                incompleteSamples.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                if (Convert.ToInt32(await incompleteSamples.ExecuteScalarAsync()) > 0)
                    throw new InvalidOperationException("لا يمكن إنهاء اللجنة قبل استكمال نتائج الفحص لجميع اللوطات.");
            }

            const string updateCommitteeSql = """
                UPDATE dbo.Im_RequestCommittee
                SET Status = @IsFinishedAll,
                    IsFinishedAll = @IsFinishedAll,
                    User_Updation_Id = @UserId,
                    User_Updation_Date = GETDATE()
                WHERE ID = @CommitteeId
                  AND ImCheckRequest_ID = @CheckRequestId
                  AND User_Deletion_Id IS NULL
                """;
            await using var updateCommittee = new SqlCommand(updateCommitteeSql, connection, transaction);
            updateCommittee.Parameters.AddWithValue("@IsFinishedAll", input.IsFinishedAll);
            updateCommittee.Parameters.AddWithValue("@UserId", userId);
            updateCommittee.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
            updateCommittee.Parameters.AddWithValue("@CheckRequestId", input.CheckRequestId);
            if (await updateCommittee.ExecuteNonQueryAsync() != 1)
                throw new InvalidOperationException("لم يتم العثور على اللجنة التابعة للطلب الحالي.");

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

public record LotItemIdentity(long RequestItemId, long ItemShortNameId);
public record CommitteeAccess(bool IsAdmin, bool IsReadOnly);
public record CommitteeWorkflowState(int InspectionCount, int IncompleteInspectionCount, int SampleCount, int IncompleteSampleCount);

public class CommitteeResultTypeOptionVm
{
    public byte Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class ImportRequestDetailsPageVm
{
    public Ex_CheckRequest_GetData_Android_V2_VM Details { get; set; } = new();
    public long CheckRequestId { get; set; }
    public long CommitteeId { get; set; }
    public byte CommitteeTypeId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public List<QuarantineStatusVm> QuarantineStatuses { get; set; } = new();
    public List<QuarantineStatusVm> SampleQuarantineStatuses { get; set; } = new();
    public List<AnalysisLabOptionVm> AnalysisLabOptions { get; set; } = new();
    public List<InfectionItemVm> InfectionItems { get; set; } = new();
    public List<CommitteeResultTypeOptionVm> CommitteeResultTypes { get; set; } = new();
    public bool IsReadOnly { get; set; }
    public bool IsAdmin { get; set; }
    public bool InspectionComplete { get; set; }
    public bool HasSamples { get; set; }
    public bool SamplesComplete { get; set; }
    public List<AssistantResultVm> AssistantResults { get; set; } = new();
}

public class InfectionItemVm
{
    public long ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ScientificName { get; set; } = string.Empty;
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int FamilyId { get; set; }
    public string FamilyName { get; set; } = string.Empty;
}

public class AssistantResultVm
{
    public long ResultId { get; set; }
    public long LotId { get; set; }
    public long RequestItemId { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public string ResultName { get; set; } = string.Empty;
    public string AdminNotes { get; set; } = string.Empty;
    public string QuarantineStatus { get; set; } = string.Empty;
    public bool CanShowQuarantine { get; set; }
    public bool IsTotal { get; set; }
    public bool? ExistingAccepted { get; set; }
    public string ExistingNotes { get; set; } = string.Empty;
}

public class AssistantOpinionSaveVm
{
    public long CheckRequestId { get; set; }
    public long CommitteeId { get; set; }
    public byte CommitteeTypeId { get; set; }
    public List<AssistantLotOpinionVm> Opinions { get; set; } = new();
}

public class AssistantLotOpinionVm
{
    public long ResultId { get; set; }
    public bool? IsAccepted { get; set; }
    public string? Notes { get; set; }
}

public class QuarantineStatusVm
{
    public int Value { get; set; }
    public bool? Value2 { get; set; }
    public string DisplayText { get; set; } = string.Empty;
}

public class AnalysisLabOptionVm
{
    public int AnalysisLabTypeId { get; set; }
    public int AnalysisTypeId { get; set; }
    public string AnalysisTypeName { get; set; } = string.Empty;
    public int AnalysisLabId { get; set; }
    public string AnalysisLabName { get; set; } = string.Empty;
}

public class ImportInspectionSaveVm
{
    public long CheckRequestId { get; set; }
    public long CommitteeId { get; set; }
    public byte CommitteeTypeId { get; set; }
    public bool IsFinishedAll { get; set; }
    public bool PerformWithdrawal { get; set; }
    public bool ApplyToAll { get; set; }
    public byte SharedCommitteeResultTypeId { get; set; }
    public int SharedQuarantineStatusId { get; set; }
    public string SharedNotes { get; set; } = string.Empty;
    public long SharedInfectionItemId { get; set; }
    public List<ImportInspectionPlantVm> PlantTotals { get; set; } = new();
    public List<ImportInspectionLotVm> Lots { get; set; } = new();
    public List<ImportSampleSaveVm> Samples { get; set; } = new();
}

public class ImportInspectionPlantVm
{
    public List<long> RequestItemIds { get; set; } = new();
    public long RequestItemId { get; set; }
    public byte CommitteeResultTypeId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public long InfectionItemId { get; set; }
}

public class ImportInspectionLotVm
{
    public long LotCategoryId { get; set; }
    public long RequestItemId { get; set; }
    public bool IsTotal { get; set; }
    public decimal? Weight { get; set; }
    public decimal? QuantitySize { get; set; }
    public byte CommitteeResultTypeId { get; set; }
    public int QuarantineStatusId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public long InfectionItemId { get; set; }
}

public class ImportQuarantineSaveVm
{
    public long CheckRequestId { get; set; }
    public long CommitteeId { get; set; }
    public byte CommitteeTypeId { get; set; }
    public List<ImportQuarantineLotVm> Lots { get; set; } = new();
}

public class ImportQuarantineLotVm
{
    public long LotCategoryId { get; set; }
    public long RequestItemId { get; set; }
    public int QuarantineStatusId { get; set; }
    public string? Notes { get; set; }
}

public class ImportSamplesSaveVm
{
    public long CheckRequestId { get; set; }
    public long CommitteeId { get; set; }
    public byte CommitteeTypeId { get; set; }
    public bool IsFinishedAll { get; set; }
    public List<ImportSampleSaveVm> Samples { get; set; } = new();
}

public class ImportSampleSaveVm
{
    public bool IsTotal { get; set; }
    public long SampleId { get; set; }
    public int AnalysisLabTypeId { get; set; }
    public long RequestItemId { get; set; }
    public long ItemShortNameId { get; set; }
    public long? LotDataId { get; set; }
    public double SampleSize { get; set; }
    public string QuantityUnit { get; set; } = "kg";
    public double SampleRatio { get; set; }
    public string? SealNumber { get; set; }
    public string? Notes { get; set; }
    public bool HasAttachment { get; set; }
    public bool? LabResultAccepted { get; set; }
    public bool? QuarantineAccepted { get; set; }
    public int? QuarantineStatusId { get; set; }
}

public class SampleAssistantOpinionSaveVm
{
    public long CheckRequestId { get; set; }
    public long CommitteeId { get; set; }
    public byte CommitteeTypeId { get; set; }
    public List<SampleAssistantOpinionVm> Opinions { get; set; } = new();
}

public class SampleAssistantOpinionVm
{
    public long SampleId { get; set; }
    public bool? IsAccepted { get; set; }
    public string? Notes { get; set; }
}

public class ImportTreatmentSaveVm
{
    public long CheckRequestId { get; set; }
    public long CommitteeId { get; set; }
    public byte CommitteeTypeId { get; set; }
    public bool ApplyToAll { get; set; }
    public bool IsFinishedAll { get; set; }
    public List<ImportTreatmentRecordVm> Records { get; set; } = new();
}

public class ImportTreatmentRecordVm
{
    public long TreatmentDataId { get; set; }
    public decimal Size { get; set; }
    public decimal MaterialAmount { get; set; }
    public decimal Dose { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? ThermalSealNumber { get; set; }
    public int ExposureDay { get; set; }
    public int ExposureHour { get; set; }
    public int ExposureMinute { get; set; }
    public string? Notes { get; set; }
}
