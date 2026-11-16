/*
    تحسين سرعة شاشة تفاصيل إذن الاستيراد.

    السبب الرئيسي للبطء:
    جدول المرفقات يحتوي مئات الآلاف من السجلات، والفهرس الموجود
    مخصص لمرفقات نتيجة المعمل رقم 12 فقط، بينما تفاصيل إذن الاستيراد
    تبحث عن نوع المرفق رقم 4 فتضطر إلى فحص الجدول بالكامل.
*/

USE [PlantQuarantine_New];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.A_AttachmentData')
      AND name = N'IX_A_AttachmentData_ImportPermission_Active'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_A_AttachmentData_ImportPermission_Active
        ON dbo.A_AttachmentData (RowId, Id)
        WHERE A_AttachmentTableNameId = 4
          AND User_Deletion_Id IS NULL;
END;
GO

/* عرض الفهرس بعد التنفيذ. */
SELECT name AS [اسم الفهرس],
       filter_definition AS [شرط الفهرس],
       is_disabled AS [متوقف]
FROM sys.indexes
WHERE object_id = OBJECT_ID(N'dbo.A_AttachmentData')
  AND name = N'IX_A_AttachmentData_ImportPermission_Active';
GO
