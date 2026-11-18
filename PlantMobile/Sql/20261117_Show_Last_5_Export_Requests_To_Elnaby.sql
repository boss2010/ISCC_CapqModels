/*
  تحديث آخر خمسة طلبات صادر لها لجان وإسناد لجانها لعبد النبي.
  المستخدم: elnaby - رقم المستخدم 353.
  التاريخ الجديد: 17/11/2026.

  القيم الأصلية للرجوع:
  الطلب 22593: 11/12/2025 - اللجان 37421=13/12/2025، 37422 و37423=14/12/2025
  الطلب 22591: 29/11/2025 - اللجان 37419=29/11/2025، 37420=30/11/2025
  الطلب 22590: 27/11/2025 - اللجنة 37418=29/11/2025
  الطلب 22589: 24/11/2025 - اللجنتان 37413 و37414=24/11/2025
  الطلب 22588: 23/11/2025 - اللجنة 37415=25/11/2025
*/
USE [PlantQuarantine_New];
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET ARITHABORT ON;
SET NUMERIC_ROUNDABORT OFF;
BEGIN TRANSACTION;

DECLARE @تاريخ_العرض date='2026-11-17';
DECLARE @رقم_عبد_النبي smallint=353;

DECLARE @الطلبات TABLE (رقم_الطلب bigint PRIMARY KEY);
INSERT @الطلبات VALUES (22593),(22591),(22590),(22589),(22588);

/* تغيير يوم إنشاء الطلب مع الاحتفاظ بوقت الإنشاء الأصلي. */
UPDATE request
SET User_Creation_Date=DATEADD(day,DATEDIFF(day,request.User_Creation_Date,@تاريخ_العرض),request.User_Creation_Date)
FROM dbo.Ex_CheckRequest request
JOIN @الطلبات selectedRequest ON selectedRequest.رقم_الطلب=request.ID;

/* تاريخ كل اللجان النشطة التابعة للطلبات الخمسة. */
UPDATE committee
SET Delegation_Date=@تاريخ_العرض
FROM dbo.Ex_RequestCommittee committee
JOIN @الطلبات selectedRequest ON selectedRequest.رقم_الطلب=committee.ExCheckRequest_ID
WHERE committee.User_Deletion_Id IS NULL AND ISNULL(committee.Is_Cancel,0)=0;

/* إعادة تفعيل عضوية قديمة لعبد النبي إن وجدت. */
UPDATE employee
SET User_Deletion_Id=NULL,User_Deletion_Date=NULL
FROM dbo.CommitteeEmployee employee
JOIN dbo.Ex_RequestCommittee committee ON committee.ID=employee.Committee_ID
JOIN @الطلبات selectedRequest ON selectedRequest.رقم_الطلب=committee.ExCheckRequest_ID
WHERE employee.Employee_Id=@رقم_عبد_النبي;

/* إضافته كمساعد في أي لجنة لم يكن عضوًا فيها. */
INSERT dbo.CommitteeEmployee
    (Committee_ID,Employee_Id,ISAdmin,OperationType,User_Creation_Id,User_Creation_Date)
SELECT committee.ID,@رقم_عبد_النبي,0,73,@رقم_عبد_النبي,GETDATE()
FROM dbo.Ex_RequestCommittee committee
JOIN @الطلبات selectedRequest ON selectedRequest.رقم_الطلب=committee.ExCheckRequest_ID
WHERE committee.User_Deletion_Id IS NULL AND ISNULL(committee.Is_Cancel,0)=0
  AND NOT EXISTS
  (
      SELECT 1 FROM dbo.CommitteeEmployee employee
      WHERE employee.Committee_ID=committee.ID
        AND employee.Employee_Id=@رقم_عبد_النبي
        AND employee.User_Deletion_Id IS NULL
  );

COMMIT TRANSACTION;

SELECT request.ID AS [رقم الطلب الداخلي],request.CheckRequest_Number AS [رقم الطلب],
       request.User_Creation_Date AS [تاريخ الطلب],committee.ID AS [رقم اللجنة],
       committee.Delegation_Date AS [تاريخ اللجنة],type.Name_Ar AS [نوع اللجنة],
       employee.ISAdmin AS [رئيس اللجنة]
FROM @الطلبات selectedRequest
JOIN dbo.Ex_CheckRequest request ON request.ID=selectedRequest.رقم_الطلب
JOIN dbo.Ex_RequestCommittee committee ON committee.ExCheckRequest_ID=request.ID
JOIN dbo.CommitteeType type ON type.ID=committee.CommitteeType_ID
JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID=committee.ID
    AND employee.Employee_Id=@رقم_عبد_النبي AND employee.User_Deletion_Id IS NULL
ORDER BY request.ID DESC,committee.ID;
GO
