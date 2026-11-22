/*
    20261122_Im_Committee_Inspection_Result_Table.sql
    شاشة «نتيجة فحص اللجنة»: حذف الجداول القديمة نهائيًا وإنشاء جدول الحفظ
    باسم عام متوافق مع تسمية السليوشن (Im_ + _SEQ).

    ملاحظة: السكربت يحذف Cairo_Suburbs_* نهائيًا (بدون sp_rename).
    يُنفّذ على قاعدة PlantQuarantine_New. آمن لإعادة التنفيذ.
*/

USE [PlantQuarantine_New];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    -- 1) حذف الجداول القديمة نهائيًا (لا ترحيل بيانات)
    IF OBJECT_ID(N'dbo.Cairo_Suburbs_Form', N'U') IS NOT NULL
        DROP TABLE dbo.Cairo_Suburbs_Form;

    IF OBJECT_ID(N'dbo.Cairo_Suburbs_NegotiationType', N'U') IS NOT NULL
        DROP TABLE dbo.Cairo_Suburbs_NegotiationType;

    IF OBJECT_ID(N'dbo.Cairo_Suburbs_AidType', N'U') IS NOT NULL
        DROP TABLE dbo.Cairo_Suburbs_AidType;

    IF OBJECT_ID(N'dbo.Cairo_Suburbs_Procedure', N'U') IS NOT NULL
        DROP TABLE dbo.Cairo_Suburbs_Procedure;

    IF OBJECT_ID(N'dbo.Cairo_Suburbs_Form_SEQ', N'SO') IS NOT NULL
        DROP SEQUENCE dbo.Cairo_Suburbs_Form_SEQ;

    -- 2) sequence الجدول الجديد: اسم الجدول + _SEQ (قاعدة السليوشن)
    IF OBJECT_ID(N'dbo.Im_Committee_Inspection_Result_SEQ', N'SO') IS NULL
        CREATE SEQUENCE dbo.Im_Committee_Inspection_Result_SEQ AS bigint START WITH 1 INCREMENT BY 1;

    -- 3) جدول الحفظ
    IF OBJECT_ID(N'dbo.Im_Committee_Inspection_Result', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.Im_Committee_Inspection_Result
        (
            ID bigint NOT NULL CONSTRAINT PK_Im_Committee_Inspection_Result PRIMARY KEY,
            Serial bigint NULL,
            Im_CheckRequest_ID bigint NULL,
            CheckRequest_Number nvarchar(50) NULL,
            Message nvarchar(50) NULL,
            CountryType_ID bigint NULL,
            Source nvarchar(100) NULL,
            Procedure_ID bigint NULL,
            Transport_Mean_ID bigint NULL,
            Item_ID bigint NULL,
            Origin_Country_ID bigint NULL,
            Quantity_Ton decimal(18,3) NULL,
            Inspection_Result_ID bigint NULL,
            Infection_Type_Item_ID bigint NULL,
            AidType_ID bigint NULL,
            Notes nvarchar(max) NULL,
            Outlet_ID bigint NULL,
            Document_Date datetime NULL,
            User_Creation_Id smallint NULL,
            User_Creation_Date datetime NULL,
            User_Updation_Id smallint NULL,
            User_Updation_Date datetime NULL
        );
    END;

    -- 4) فهرس البحث حسب الطلب (غير موجود في السكربت القديم)
    IF NOT EXISTS
    (
        SELECT 1 FROM sys.indexes
        WHERE object_id = OBJECT_ID(N'dbo.Im_Committee_Inspection_Result')
          AND name = N'IX_Im_Committee_Inspection_Result_Request'
    )
        CREATE INDEX IX_Im_Committee_Inspection_Result_Request
            ON dbo.Im_Committee_Inspection_Result (Im_CheckRequest_ID);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

-- تحقق
SELECT name FROM sys.objects
WHERE name LIKE N'Im_Committee_Inspection_Result%' OR name LIKE N'Cairo_Suburbs_%'
ORDER BY name;
GO
