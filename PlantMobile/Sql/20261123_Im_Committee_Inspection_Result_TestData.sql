/*
    20261123_Im_Committee_Inspection_Result_TestData.sql
    DEV / TEST ONLY — بيانات تجريبية لشاشة «نتيجة فحص اللجنة - عرض الكل».

    - يعمل على قاعدة PlantQuarantine_New.
    - آمن لإعادة التنفيذ: يحذف البيانات التجريبية السابقة (Serial 1..20) قبل الإدخال.
    - للحذف الكامل بعد التجربة: نفّذ آخر سطرين في الملف (DELETE المعلّق).
*/

USE [PlantQuarantine_New];
GO

SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DELETE FROM dbo.Im_Committee_Inspection_Result WHERE Serial BETWEEN 1 AND 20;

    DECLARE @now datetime = GETDATE();

    ;WITH src AS
    (
        SELECT * FROM (VALUES
            ( 1, 99999, N'362023101901222461', N'صادر', 1, N'150',  1, 1, 2615, 240, CAST(12.500 AS decimal(18,3)),  1, NULL, 0, N'تم الفحص والرسالة مطابقة لتشريعات الحجر الزراعي', 163, -1),
            ( 2, 99998, N'362023101901185387', N'وارد', 2, N'150',  2, 2, 2975, 134, CAST(25.750 AS decimal(18,3)),  3, 2615, 1, N'تم تحويل الرسالة إلى التبخير', 173, -3),
            ( 3, 99997, N'362023101901153598', N'صادر', 3, N'150',  3, 3, 2465,  40, CAST(50.000 AS decimal(18,3)),  7, NULL, 0, N'إيقاف المعاملة لحين استكمال المستندات', 164, -5),
            ( 4, 99996, N'362023101901121656', N'وارد', 4, N'150',  4, 4, 2964, 155, CAST( 8.250 AS decimal(18,3)),  9, 2975, 1, N'الرسالة غير مطابقة — إعادة تصدير', 155, -8),
            ( 5, 99995, N'362023101901023798', N'صادر', 5, N'150',  5, 1, 2635,  25, CAST(100.000 AS decimal(18,3)), 10, NULL, 0, N'تمت جميع الإجراءات الحجرية', 183, -12),
            ( 6, 99994, N'362023101900572797', N'وارد', 6, N'150',  6, 2, 2038, 234, CAST(15.000 AS decimal(18,3)),  1, 2038, 1, N'تم أخذ عينة إضافية للتأكد', 177, -15),
            ( 7, 99993, N'362023101900533180', N'صادر', 7, N'150', 10, 3, 2576, 167, CAST(33.500 AS decimal(18,3)),  3, NULL, 0, N'الرسالة في انتظار الصرف', 184, -20),
            ( 8, 99992, N'362023101900514886', N'وارد', 1, N'150', 11, 4, 1712, 105, CAST(60.000 AS decimal(18,3)),  7, 1712, 1, N'نقل وتخزين تحت التحفظ', 178, -25),
            ( 9, 99999, N'362023101901222461', N'صادر', 2, N'150',  1, 2, 3138,  40, CAST( 5.500 AS decimal(18,3)),  1, NULL, 0, N'رسالة مطابقة — تم الإفراج', 163, -28),
            (10, 99998, N'362023101901185387', N'وارد', 3, N'150',  2, 1, 2953, 240, CAST(44.000 AS decimal(18,3)),  9, 2953, 1, N'تم التطهير وإعادة الفحص', 173, -32),
            (11, 99997, N'362023101901153598', N'صادر', 4, N'150',  3, 4, 2862, 134, CAST(18.750 AS decimal(18,3)), 10, NULL, 0, N'تمت الإجراءات الحجرية بالكامل', 164, -36),
            (12, 99996, N'362023101901121656', N'وارد', 5, N'150',  4, 3, 2674, 155, CAST(72.250 AS decimal(18,3)),  3, 2674, 1, N'الرسالة تحول إلى التبخير', 155, -40),
            (13, 99995, N'362023101901023798', N'صادر', 6, N'150',  5, 2, 2615,  25, CAST( 9.000 AS decimal(18,3)),  7, NULL, 0, N'لا توجد إصابات — رسالة سليمة', 183, -44),
            (14, 99994, N'362023101900572797', N'وارد', 7, N'150',  6, 1, 2975, 234, CAST(120.000 AS decimal(18,3)), 1, 2975, 1, N'تم سحب عينات للتحليل', 177, -48),
            (15, 99993, N'362023101900533180', N'صادر', 1, N'150', 10, 4, 2465, 167, CAST(27.500 AS decimal(18,3)),  9, NULL, 0, N'إعادة تصدير بناءً على طلب المستورد', 184, -52),
            (16, 99992, N'362023101900514886', N'وارد', 2, N'150', 11, 3, 2964, 105, CAST(64.000 AS decimal(18,3)), 10, 2964, 1, N'تم التحفظ على الرسالة', 178, -56),
            (17, 99999, N'362023101901222461', N'صادر', 3, N'150',  1, 1, 2038,  40, CAST( 3.250 AS decimal(18,3)),  1, NULL, 0, N'رسالة صغيرة — فحص سريع', 163, -60),
            (18, 99998, N'362023101901185387', N'وارد', 4, N'150',  2, 2, 2576, 240, CAST(88.750 AS decimal(18,3)),  3, 2576, 1, N'تم التبخير بنجاح', 173, -64),
            (19, 99997, N'362023101901153598', N'صادر', 5, N'150',  3, 3, 1712, 134, CAST(41.000 AS decimal(18,3)),  7, NULL, 0, N'الرسالة مطابقة — جاهزة للصرف', 164, -68),
            (20, 99996, N'362023101901121656', N'وارد', 6, N'150',  4, 4, 3138, 155, CAST(150.000 AS decimal(18,3)), 9, 3138, 1, N'تم أخذ عينة والنتيجة غير مطابقة', 155, -72)
        ) v (Serial, ReqId, ReqNo, Message, CountryType, Source, ProcedureId, TransportId,
             ItemId, OriginId, Qty, ResultId, InfectionId, AidType, Notes, OutletId, DaysAgo)
    )
    INSERT dbo.Im_Committee_Inspection_Result
    (
        ID, Serial, Im_CheckRequest_ID, CheckRequest_Number, Message, CountryType_ID, Source,
        Procedure_ID, Transport_Mean_ID, Item_ID, Origin_Country_ID, Quantity_Ton,
        Inspection_Result_ID, Infection_Type_Item_ID, AidType_ID, Notes, Outlet_ID,
        Document_Date, User_Creation_Id, User_Creation_Date
    )
    SELECT
        NEXT VALUE FOR dbo.Im_Committee_Inspection_Result_SEQ,
        src.Serial, src.ReqId, src.ReqNo, src.Message, src.CountryType, src.Source,
        src.ProcedureId, src.TransportId, src.ItemId, src.OriginId, src.Qty,
        src.ResultId, src.InfectionId, src.AidType, src.Notes, src.OutletId,
        DATEADD(day, src.DaysAgo, @now), 354, DATEADD(day, src.DaysAgo, @now)
    FROM src;

    COMMIT TRANSACTION;
    SELECT COUNT(*) AS InsertedRows FROM dbo.Im_Committee_Inspection_Result;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

-- للحذف بعد انتهاء التجربة:
-- DELETE FROM dbo.Im_Committee_Inspection_Result WHERE Serial BETWEEN 1 AND 20;
-- GO
