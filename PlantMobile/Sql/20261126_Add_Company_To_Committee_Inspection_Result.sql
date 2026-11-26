/*
    20261126_Add_Company_To_Committee_Inspection_Result.sql
    يضيف عمودي الشركة (ImporterType_Id / Importer_ID) لجدول نتيجة فحص اللجنة.
    آمن لإعادة التنفيذ.
*/

USE [PlantQuarantine_New];
GO

SET NOCOUNT ON;
GO

IF COL_LENGTH(N'dbo.Im_Committee_Inspection_Result', N'ImporterType_Id') IS NULL
    ALTER TABLE dbo.Im_Committee_Inspection_Result ADD ImporterType_Id int NULL;
GO

IF COL_LENGTH(N'dbo.Im_Committee_Inspection_Result', N'Importer_ID') IS NULL
    ALTER TABLE dbo.Im_Committee_Inspection_Result ADD Importer_ID bigint NULL;
GO

SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = N'Im_Committee_Inspection_Result'
  AND COLUMN_NAME IN (N'ImporterType_Id', N'Importer_ID')
ORDER BY COLUMN_NAME;
GO
