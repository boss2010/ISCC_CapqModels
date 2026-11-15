/*
    تفعيل شاشة صلاحيات المستخدمين داخل المشروع الجديد.
    لا يضيف أو يحذف صلاحيات أي موظف.
*/

USE [dbPrivilage];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;

DECLARE @رقم_الشاشة int;

SELECT TOP (1) @رقم_الشاشة = Id
FROM dbo.PR_Menu
WHERE MenuURL = N'EX_User/User_Privilage'
ORDER BY CASE WHEN Id = 144 THEN 0 ELSE 1 END, Id;

IF @رقم_الشاشة IS NULL
    THROW 50001, N'شاشة صلاحيات المستخدمين غير موجودة في جدول القوائم.', 1;

UPDATE dbo.PR_Menu
SET MenuTitle = N'صلاحيات المستخدمين',
    MenuTitle_En = N'User Privileges',
    MenuURL = N'EX_User/User_Privilage',
    Active = 1,
    isEnableAndroid = 1
WHERE Id = @رقم_الشاشة;

UPDATE dbo.PR_GroupModuleMenu
SET IS_Active = 1
WHERE PR_MenuId = @رقم_الشاشة;

COMMIT TRANSACTION;
GO

SELECT menu.Id AS [رقم الشاشة],
       menu.MenuTitle AS [اسم الشاشة],
       menu.MenuURL AS [المسار],
       menu.Active AS [مفعلة],
       menu.isEnableAndroid AS [تظهر في المشروع الجديد],
       groupMenu.PR_GroupId AS [رقم المجموعة],
       groupMenu.PR_ModuleId AS [رقم القسم]
FROM dbo.PR_Menu menu
LEFT JOIN dbo.PR_GroupModuleMenu groupMenu ON groupMenu.PR_MenuId = menu.Id
WHERE menu.MenuURL = N'EX_User/User_Privilage';
GO
