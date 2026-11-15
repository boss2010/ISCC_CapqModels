/*
    منح شاشة تعديل تاريخ شحن طلب وارد للمستخدم Elnaby.

    يتم تحديد المستخدم باسم الدخول والرقم الوظيفي معاً
    لتجنب منح الصلاحية لمستخدم آخر يحمل اسماً مشابهاً.
*/

USE [dbPrivilage];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;

DECLARE @رقم_المستخدم smallint;
DECLARE @رقم_الشاشة int;

SELECT TOP (1) @رقم_المستخدم = CONVERT(smallint, Id)
FROM dbo.PR_User
WHERE LoginName = N'Elnaby'
  AND EmpId = 27336
  AND Active = 1;

IF @رقم_المستخدم IS NULL
    THROW 50001, N'المستخدم Elnaby بالرقم الوظيفي 27336 غير موجود أو غير مفعل.', 1;

SELECT TOP (1) @رقم_الشاشة = Id
FROM dbo.PR_Menu
WHERE MenuURL = N'Im_CheckRequests/ShippingDate/Index'
  AND Active = 1
ORDER BY Id DESC;

IF @رقم_الشاشة IS NULL
    THROW 50002, N'شاشة تعديل تاريخ شحن طلب وارد غير موجودة في نظام الصلاحيات.', 1;

IF EXISTS
(
    SELECT 1
    FROM dbo.PR_GroupModuleMenuPrivilage
    WHERE PR_User_id = @رقم_المستخدم
      AND PR_GroupId = 8
      AND PR_ModuleId = 33
      AND PR_MenuId = @رقم_الشاشة
)
BEGIN
    UPDATE dbo.PR_GroupModuleMenuPrivilage
    SET CanView = 1,
        CanAdd = 0,
        CanEdit = 1,
        CanDelete = 0,
        CanPrint = 0,
        IS_Active = 1
    WHERE PR_User_id = @رقم_المستخدم
      AND PR_GroupId = 8
      AND PR_ModuleId = 33
      AND PR_MenuId = @رقم_الشاشة;
END
ELSE
BEGIN
    DECLARE @رقم_الصلاحية int;

    SELECT @رقم_الصلاحية = ISNULL(MAX(Id), 0) + 1
    FROM dbo.PR_GroupModuleMenuPrivilage WITH (UPDLOCK, HOLDLOCK);

    INSERT dbo.PR_GroupModuleMenuPrivilage
    (
        Id, PR_GroupId, PR_ModuleId, PR_MenuId,
        CanView, CanAdd, CanEdit, CanDelete, CanPrint,
        PR_User_id, IS_Active
    )
    VALUES
    (
        @رقم_الصلاحية, 8, 33, @رقم_الشاشة,
        1, 0, 1, 0, 0,
        @رقم_المستخدم, 1
    );
END;

COMMIT TRANSACTION;
GO

SELECT [user].Id AS [رقم المستخدم],
       [user].LoginName AS [اسم الدخول],
       [user].FullName AS [اسم الموظف],
       menu.MenuTitle AS [اسم الشاشة],
       privilege.CanView AS [صلاحية العرض],
       privilege.CanEdit AS [صلاحية التعديل],
       privilege.IS_Active AS [الصلاحية مفعلة]
FROM dbo.PR_GroupModuleMenuPrivilage privilege
INNER JOIN dbo.PR_User [user] ON [user].Id = privilege.PR_User_id
INNER JOIN dbo.PR_Menu menu ON menu.Id = privilege.PR_MenuId
WHERE [user].LoginName = N'Elnaby'
  AND [user].EmpId = 27336
  AND menu.MenuURL = N'Im_CheckRequests/ShippingDate/Index';
GO
