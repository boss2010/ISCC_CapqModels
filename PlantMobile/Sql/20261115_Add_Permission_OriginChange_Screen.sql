/*
    إضافة شاشة تعديل منشأ إذن الاستيراد إلى نظام الصلاحيات.
    السكربت لا يمنح الصلاحية لموظف محدد، ويمكن إعادة تنفيذه بأمان.
*/

USE [dbPrivilage];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;

DECLARE @رقم_الشاشة int;
DECLARE @مسار_الشاشة nvarchar(500) = N'Im_Permissions/OriginChange/Index';

SELECT TOP (1) @رقم_الشاشة = Id
FROM dbo.PR_Menu
WHERE LOWER(LTRIM(RTRIM(MenuURL))) = LOWER(@مسار_الشاشة)
ORDER BY Id;

IF @رقم_الشاشة IS NULL
BEGIN
    SELECT @رقم_الشاشة = ISNULL(MAX(Id), 0) + 1
    FROM dbo.PR_Menu WITH (UPDLOCK, HOLDLOCK);

    INSERT dbo.PR_Menu
    (
        Id, MenuTitle, MenuTitle_En, MenuURL, PR_MenuId, Active,
        PR_ModuleId, PR_ApplicationId, PR_ApplicationCategoryId,
        Group_Id, isEnableAndroid
    )
    VALUES
    (
        @رقم_الشاشة,
        N'تعديل منشأ إذن الاستيراد',
        N'Change Import Permission Origin',
        @مسار_الشاشة,
        NULL, 1, 26, 1, 1, 1, 1
    );
END
ELSE
BEGIN
    UPDATE dbo.PR_Menu
    SET MenuTitle = N'تعديل منشأ إذن الاستيراد',
        MenuTitle_En = N'Change Import Permission Origin',
        MenuURL = @مسار_الشاشة,
        Active = 1,
        PR_ModuleId = 26,
        PR_ApplicationId = 1,
        PR_ApplicationCategoryId = 1,
        Group_Id = 1,
        isEnableAndroid = 1
    WHERE Id = @رقم_الشاشة;
END;

IF EXISTS
(
    SELECT 1 FROM dbo.PR_GroupModuleMenu
    WHERE PR_GroupId = 8 AND PR_ModuleId = 32 AND PR_MenuId = @رقم_الشاشة
)
BEGIN
    UPDATE dbo.PR_GroupModuleMenu
    SET IS_Active = 1, Order_BY = 86
    WHERE PR_GroupId = 8 AND PR_ModuleId = 32 AND PR_MenuId = @رقم_الشاشة;
END
ELSE
BEGIN
    INSERT dbo.PR_GroupModuleMenu
        (PR_GroupId, PR_ModuleId, PR_MenuId, IS_Active, Order_BY)
    VALUES
        (8, 32, @رقم_الشاشة, 1, 86);
END;

COMMIT TRANSACTION;
GO

SELECT menu.Id AS [رقم الشاشة], menu.MenuTitle AS [اسم الشاشة],
       menu.MenuURL AS [المسار], groupMenu.PR_GroupId AS [رقم المجموعة],
       groupMenu.PR_ModuleId AS [رقم القسم], groupMenu.Order_BY AS [الترتيب]
FROM dbo.PR_Menu menu
INNER JOIN dbo.PR_GroupModuleMenu groupMenu ON groupMenu.PR_MenuId = menu.Id
WHERE menu.MenuURL = N'Im_Permissions/OriginChange/Index';
GO
