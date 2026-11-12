/*
    إضافة شاشة تعديل تاريخ شحن طلب الوارد إلى نظام الصلاحيات.

    ملاحظات:
      - السكربت قابل لإعادة التنفيذ دون تكرار الشاشة.
      - لا يمنح الشاشة لأي موظف تلقائياً.
      - بعد التنفيذ تُمنح صلاحية العرض للموظفين المطلوبين من شاشة إدارة الصلاحيات.
      - الملف مكتوب بترميز UTF-8 للحفاظ على الاسم العربي.
*/

USE [dbPrivilage];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;

DECLARE @MenuId int;

SELECT TOP (1) @MenuId = Id
FROM dbo.PR_Menu
WHERE LOWER(REPLACE(REPLACE(LTRIM(RTRIM(MenuURL)), N'~', N''), N'\', N'/'))
    = LOWER(N'Im_CheckRequests/ShippingDate/Index')
ORDER BY Id;

IF @MenuId IS NULL
BEGIN
    SELECT @MenuId = ISNULL(MAX(Id), 0) + 1
    FROM dbo.PR_Menu WITH (UPDLOCK, HOLDLOCK);

    INSERT dbo.PR_Menu
    (
        Id, MenuTitle, MenuTitle_En, MenuURL, PR_MenuId, Active,
        PR_ModuleId, PR_ApplicationId, PR_ApplicationCategoryId,
        Group_Id, isEnableAndroid
    )
    VALUES
    (
        @MenuId,
        N'تعديل تاريخ شحن طلب وارد',
        N'Edit Import Request Shipping Date',
        N'Im_CheckRequests/ShippingDate/Index',
        NULL, 1, 22, 1, 1, 1, 1
    );
END
ELSE
BEGIN
    UPDATE dbo.PR_Menu
    SET MenuTitle = N'تعديل تاريخ شحن طلب وارد',
        MenuTitle_En = N'Edit Import Request Shipping Date',
        MenuURL = N'Im_CheckRequests/ShippingDate/Index',
        Active = 1,
        PR_ModuleId = 22,
        PR_ApplicationId = 1,
        PR_ApplicationCategoryId = 1,
        Group_Id = 1,
        isEnableAndroid = 1
    WHERE Id = @MenuId;
END;

IF EXISTS
(
    SELECT 1
    FROM dbo.PR_GroupModuleMenu
    WHERE PR_GroupId = 8
      AND PR_ModuleId = 33
      AND PR_MenuId = @MenuId
)
BEGIN
    UPDATE dbo.PR_GroupModuleMenu
    SET IS_Active = 1,
        Order_BY = 90
    WHERE PR_GroupId = 8
      AND PR_ModuleId = 33
      AND PR_MenuId = @MenuId;
END
ELSE
BEGIN
    INSERT dbo.PR_GroupModuleMenu
        (PR_GroupId, PR_ModuleId, PR_MenuId, IS_Active, Order_BY)
    VALUES
        (8, 33, @MenuId, 1, 90);
END;

COMMIT TRANSACTION;
GO

SELECT menu.Id AS [رقم الشاشة],
       menu.MenuTitle AS [اسم الشاشة],
       menu.MenuURL AS [مسار الشاشة],
       groupMenu.PR_GroupId AS [رقم المجموعة],
       groupMenu.PR_ModuleId AS [رقم القسم],
       groupMenu.Order_BY AS [ترتيب الشاشة]
FROM dbo.PR_Menu menu
INNER JOIN dbo.PR_GroupModuleMenu groupMenu ON groupMenu.PR_MenuId = menu.Id
WHERE LOWER(REPLACE(REPLACE(LTRIM(RTRIM(menu.MenuURL)), N'~', N''), N'\', N'/'))
    = LOWER(N'Im_CheckRequests/ShippingDate/Index');
GO
