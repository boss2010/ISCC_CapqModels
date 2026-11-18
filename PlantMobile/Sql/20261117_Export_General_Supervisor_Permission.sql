/*
    إضافة شاشة وصلاحية: الإشراف العام على طلبات الصادر.
    هذه الشاشة للعرض فقط، ولا تمنح الإضافة أو التعديل أو الحذف.
    بعد تنفيذ السكربت يمكن منحها لأي موظف من شاشة صلاحيات المستخدمين.
*/
USE [dbPrivilage];
SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRANSACTION;

DECLARE @رقم_المجموعة int;
DECLARE @رقم_القسم int;
DECLARE @رقم_الشاشة int;

SELECT TOP (1) @رقم_المجموعة = Id
FROM dbo.PR_Group
WHERE GroupName = N'إدارة الصادر الجديدة';

IF @رقم_المجموعة IS NULL
    THROW 51001, N'مجموعة إدارة الصادر الجديدة غير موجودة. نفّذ سكربت مجموعة شاشات الصادر أولاً.', 1;

SELECT TOP (1) @رقم_القسم = Id
FROM dbo.PR_Module
WHERE ModuleName = N'شاشات الصادر الجديدة';

IF @رقم_القسم IS NULL
    THROW 51002, N'قسم شاشات الصادر الجديدة غير موجود. نفّذ سكربت مجموعة شاشات الصادر أولاً.', 1;

SELECT TOP (1) @رقم_الشاشة = Id
FROM dbo.PR_Menu
WHERE LOWER(REPLACE(REPLACE(LTRIM(RTRIM(MenuURL)), N'~', N''), N'\', N'/'))
    = N'export_checkrequest/list_excheckrequest/supervisor';

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
        N'الإشراف العام على طلبات الصادر',
        N'Export Requests General Supervision',
        N'Export_CheckRequest/List_EXCheckRequest/Supervisor',
        NULL, 1, @رقم_القسم, 1, 1, @رقم_المجموعة, 1
    );
END
ELSE
BEGIN
    UPDATE dbo.PR_Menu
    SET MenuTitle = N'الإشراف العام على طلبات الصادر',
        MenuTitle_En = N'Export Requests General Supervision',
        Active = 1,
        PR_ModuleId = @رقم_القسم,
        Group_Id = @رقم_المجموعة,
        isEnableAndroid = 1
    WHERE Id = @رقم_الشاشة;
END;

UPDATE dbo.PR_GroupModuleMenu
SET IS_Active = 1, Order_BY = 4
WHERE PR_GroupId = @رقم_المجموعة
  AND PR_ModuleId = @رقم_القسم
  AND PR_MenuId = @رقم_الشاشة;

IF NOT EXISTS
(
    SELECT 1 FROM dbo.PR_GroupModuleMenu
    WHERE PR_GroupId = @رقم_المجموعة
      AND PR_ModuleId = @رقم_القسم
      AND PR_MenuId = @رقم_الشاشة
)
BEGIN
    INSERT dbo.PR_GroupModuleMenu
        (PR_GroupId, PR_ModuleId, PR_MenuId, IS_Active, Order_BY)
    VALUES
        (@رقم_المجموعة, @رقم_القسم, @رقم_الشاشة, 1, 4);
END;

/* حماية أي صلاحيات موجودة للشاشة: تظل عرض فقط دائمًا. */
UPDATE dbo.PR_GroupModuleMenuPrivilage
SET CanAdd = 0, CanEdit = 0, CanDelete = 0, CanPrint = 0
WHERE PR_MenuId = @رقم_الشاشة;

COMMIT TRANSACTION;

SELECT @رقم_الشاشة AS [رقم الشاشة],
       N'الإشراف العام على طلبات الصادر' AS [اسم الشاشة],
       N'Export_CheckRequest/List_EXCheckRequest/Supervisor' AS [المسار],
       N'امنح عرض فقط للموظف من شاشة صلاحيات المستخدمين' AS [طريقة المنح];
GO
