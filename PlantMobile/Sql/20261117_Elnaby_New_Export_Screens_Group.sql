/*
    إنشاء مجموعة جديدة لشاشات الصادر التي تم تنفيذها في المشروع الجديد
    ومنحها للمستخدم محمد عبد النبي عبدالحميد أحمد.

    المجموعة: إدارة الصادر الجديدة
    القسم: شاشات الصادر الجديدة

    الشاشات:
      1- طلبات الفحص الجديدة - صادر
      2- متابعة محطات الصادر
      3- تسكين الموظفين بمحطات الصادر

    السكربت قابل لإعادة التنفيذ دون تكرار البيانات.
*/

USE [dbPrivilage];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

BEGIN TRANSACTION;

DECLARE @رقم_المجموعة int;
DECLARE @رقم_القسم int;
DECLARE @رقم_المستخدم smallint;
DECLARE @رقم_قائمة_الطلبات int;
DECLARE @رقم_متابعة_المحطات int;
DECLARE @رقم_تسكين_الموظفين int;

/* تحديد حساب عبد النبي بالرقم الوظيفي واسم الدخول. */
SELECT TOP (1) @رقم_المستخدم = CONVERT(smallint, Id)
FROM dbo.PR_User
WHERE EmpId = 27336
  AND LOWER(LoginName) = N'elnaby'
  AND Active = 1
ORDER BY Id;

IF @رقم_المستخدم IS NULL
    THROW 50001, N'حساب عبد النبي بالرقم الوظيفي 27336 غير موجود أو غير مفعل.', 1;

/* إنشاء مجموعة إدارة الصادر الجديدة. */
SELECT TOP (1) @رقم_المجموعة = Id
FROM dbo.PR_Group
WHERE GroupName = N'إدارة الصادر الجديدة'
ORDER BY Id;

IF @رقم_المجموعة IS NULL
BEGIN
    SELECT @رقم_المجموعة = ISNULL(MAX(Id), 0) + 1
    FROM dbo.PR_Group WITH (UPDLOCK, HOLDLOCK);

    INSERT dbo.PR_Group
    (
        Id, GroupName, GroupName_En, Active, CreatedDate,
        LastModifiedDate, Note, PR_ApplicationId,
        PR_ApplicationCategoryId, IsMinistry
    )
    VALUES
    (
        @رقم_المجموعة,
        N'إدارة الصادر الجديدة',
        'New Export Management',
        1, CONVERT(date, GETDATE()), NULL,
        N'شاشات الصادر المنقولة إلى المشروع الجديد',
        1, 1, 1
    );
END
ELSE
BEGIN
    UPDATE dbo.PR_Group
    SET Active = 1,
        GroupName = N'إدارة الصادر الجديدة',
        GroupName_En = 'New Export Management',
        LastModifiedDate = CONVERT(date, GETDATE()),
        PR_ApplicationId = 1,
        PR_ApplicationCategoryId = 1,
        IsMinistry = 1
    WHERE Id = @رقم_المجموعة;
END;

/* إنشاء قسم شاشات الصادر الجديدة. */
SELECT TOP (1) @رقم_القسم = Id
FROM dbo.PR_Module
WHERE ModuleName = N'شاشات الصادر الجديدة'
ORDER BY Id;

IF @رقم_القسم IS NULL
BEGIN
    SELECT @رقم_القسم = ISNULL(MAX(Id), 0) + 1
    FROM dbo.PR_Module WITH (UPDLOCK, HOLDLOCK);

    INSERT dbo.PR_Module
    (
        Id, ModuleName, ModuleName_En, ModuleDescription,
        PR_ApplicationId, PR_ApplicationCategoryId, Active
    )
    VALUES
    (
        @رقم_القسم,
        N'شاشات الصادر الجديدة',
        'New Export Screens',
        N'قائمة طلبات الصادر ومتابعة المحطات وتسكين الموظفين',
        1, 1, 1
    );
END
ELSE
BEGIN
    UPDATE dbo.PR_Module
    SET ModuleName = N'شاشات الصادر الجديدة',
        ModuleName_En = 'New Export Screens',
        ModuleDescription = N'قائمة طلبات الصادر ومتابعة المحطات وتسكين الموظفين',
        PR_ApplicationId = 1,
        PR_ApplicationCategoryId = 1,
        Active = 1
    WHERE Id = @رقم_القسم;
END;

/* شاشة قائمة طلبات فحص الصادر الجديدة: تستخدم السجل الموجود إن وجد. */
SELECT TOP (1) @رقم_قائمة_الطلبات = Id
FROM dbo.PR_Menu
WHERE MenuURL = N'Export_CheckRequest/List_EXCheckRequest/Index'
ORDER BY CASE WHEN Id = 100 THEN 0 ELSE 1 END, Id;

IF @رقم_قائمة_الطلبات IS NULL
BEGIN
    SELECT @رقم_قائمة_الطلبات = ISNULL(MAX(Id), 0) + 1
    FROM dbo.PR_Menu WITH (UPDLOCK, HOLDLOCK);

    INSERT dbo.PR_Menu
    (
        Id, MenuTitle, MenuTitle_En, MenuURL, PR_MenuId, Active,
        PR_ModuleId, PR_ApplicationId, PR_ApplicationCategoryId,
        Group_Id, isEnableAndroid
    )
    VALUES
    (
        @رقم_قائمة_الطلبات,
        N'طلبات الفحص الجديدة - صادر',
        N'New Export Inspection Requests',
        N'Export_CheckRequest/List_EXCheckRequest/Index',
        NULL, 1, @رقم_القسم, 1, 1, @رقم_المجموعة, 1
    );
END
ELSE
BEGIN
    UPDATE dbo.PR_Menu
    SET MenuTitle = N'طلبات الفحص الجديدة - صادر',
        MenuTitle_En = N'New Export Inspection Requests',
        Active = 1,
        isEnableAndroid = 1
    WHERE Id = @رقم_قائمة_الطلبات;
END;

/* شاشة متابعة محطات الصادر. */
SELECT TOP (1) @رقم_متابعة_المحطات = Id
FROM dbo.PR_Menu
WHERE MenuURL = N'Export_CheckRequest/List_EXCheckRequest/Stations'
ORDER BY Id;

IF @رقم_متابعة_المحطات IS NULL
BEGIN
    SELECT @رقم_متابعة_المحطات = ISNULL(MAX(Id), 0) + 1
    FROM dbo.PR_Menu WITH (UPDLOCK, HOLDLOCK);

    INSERT dbo.PR_Menu
    (
        Id, MenuTitle, MenuTitle_En, MenuURL, PR_MenuId, Active,
        PR_ModuleId, PR_ApplicationId, PR_ApplicationCategoryId,
        Group_Id, isEnableAndroid
    )
    VALUES
    (
        @رقم_متابعة_المحطات,
        N'متابعة محطات الصادر',
        N'Export Stations Dashboard',
        N'Export_CheckRequest/List_EXCheckRequest/Stations',
        NULL, 1, @رقم_القسم, 1, 1, @رقم_المجموعة, 1
    );
END
ELSE
BEGIN
    UPDATE dbo.PR_Menu
    SET MenuTitle = N'متابعة محطات الصادر',
        MenuTitle_En = N'Export Stations Dashboard',
        Active = 1,
        isEnableAndroid = 1
    WHERE Id = @رقم_متابعة_المحطات;
END;

/* شاشة تسكين الموظفين بمحطات الصادر. */
SELECT TOP (1) @رقم_تسكين_الموظفين = Id
FROM dbo.PR_Menu
WHERE MenuURL = N'Employee/StationAssignment/Index'
ORDER BY Id;

IF @رقم_تسكين_الموظفين IS NULL
BEGIN
    SELECT @رقم_تسكين_الموظفين = ISNULL(MAX(Id), 0) + 1
    FROM dbo.PR_Menu WITH (UPDLOCK, HOLDLOCK);

    INSERT dbo.PR_Menu
    (
        Id, MenuTitle, MenuTitle_En, MenuURL, PR_MenuId, Active,
        PR_ModuleId, PR_ApplicationId, PR_ApplicationCategoryId,
        Group_Id, isEnableAndroid
    )
    VALUES
    (
        @رقم_تسكين_الموظفين,
        N'تسكين الموظفين بمحطات الصادر',
        N'Export Station Employee Assignment',
        N'Employee/StationAssignment/Index',
        NULL, 1, @رقم_القسم, 1, 1, @رقم_المجموعة, 1
    );
END
ELSE
BEGIN
    UPDATE dbo.PR_Menu
    SET MenuTitle = N'تسكين الموظفين بمحطات الصادر',
        MenuTitle_En = N'Export Station Employee Assignment',
        Active = 1,
        isEnableAndroid = 1
    WHERE Id = @رقم_تسكين_الموظفين;
END;

/* ربط الشاشات بالمجموعة والقسم الجديدين. */
DECLARE @الشاشات TABLE (رقم_الشاشة int PRIMARY KEY, الترتيب int, صلاحية_التعديل bit);
INSERT @الشاشات (رقم_الشاشة, الترتيب, صلاحية_التعديل)
VALUES
    (@رقم_قائمة_الطلبات, 1, 1),
    (@رقم_متابعة_المحطات, 2, 0),
    (@رقم_تسكين_الموظفين, 3, 1);

UPDATE map
SET map.IS_Active = 1,
    map.Order_BY = screens.الترتيب
FROM dbo.PR_GroupModuleMenu map
INNER JOIN @الشاشات screens ON screens.رقم_الشاشة = map.PR_MenuId
WHERE map.PR_GroupId = @رقم_المجموعة
  AND map.PR_ModuleId = @رقم_القسم;

INSERT dbo.PR_GroupModuleMenu
    (PR_GroupId, PR_ModuleId, PR_MenuId, IS_Active, Order_BY)
SELECT @رقم_المجموعة, @رقم_القسم, screens.رقم_الشاشة, 1, screens.الترتيب
FROM @الشاشات screens
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.PR_GroupModuleMenu map
    WHERE map.PR_GroupId = @رقم_المجموعة
      AND map.PR_ModuleId = @رقم_القسم
      AND map.PR_MenuId = screens.رقم_الشاشة
);

/* تحديث أي صلاحية موجودة لعبد النبي داخل المجموعة الجديدة. */
UPDATE privilege
SET privilege.CanView = 1,
    privilege.CanAdd = screens.صلاحية_التعديل,
    privilege.CanEdit = screens.صلاحية_التعديل,
    privilege.CanDelete = screens.صلاحية_التعديل,
    privilege.CanPrint = 0,
    privilege.IS_Active = 1
FROM dbo.PR_GroupModuleMenuPrivilage privilege
INNER JOIN @الشاشات screens ON screens.رقم_الشاشة = privilege.PR_MenuId
WHERE privilege.PR_User_id = @رقم_المستخدم
  AND privilege.PR_GroupId = @رقم_المجموعة
  AND privilege.PR_ModuleId = @رقم_القسم;

/* إضافة الصلاحيات غير الموجودة. */
DECLARE @أول_رقم_صلاحية int;
SELECT @أول_رقم_صلاحية = ISNULL(MAX(Id), 0)
FROM dbo.PR_GroupModuleMenuPrivilage WITH (UPDLOCK, HOLDLOCK);

;WITH الصلاحيات_الناقصة AS
(
    SELECT screens.رقم_الشاشة,
           screens.صلاحية_التعديل,
           ROW_NUMBER() OVER (ORDER BY screens.الترتيب) AS رقم_تسلسلي
    FROM @الشاشات screens
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.PR_GroupModuleMenuPrivilage privilege
        WHERE privilege.PR_User_id = @رقم_المستخدم
          AND privilege.PR_GroupId = @رقم_المجموعة
          AND privilege.PR_ModuleId = @رقم_القسم
          AND privilege.PR_MenuId = screens.رقم_الشاشة
    )
)
INSERT dbo.PR_GroupModuleMenuPrivilage
(
    Id, PR_GroupId, PR_ModuleId, PR_MenuId,
    CanView, CanAdd, CanEdit, CanDelete, CanPrint,
    PR_User_id, IS_Active
)
SELECT @أول_رقم_صلاحية + رقم_تسلسلي,
       @رقم_المجموعة, @رقم_القسم, رقم_الشاشة,
       1, صلاحية_التعديل, صلاحية_التعديل, صلاحية_التعديل, 0,
       @رقم_المستخدم, 1
FROM الصلاحيات_الناقصة;

/* مزامنة Sequence بعد أي إدخالات يدوية سابقة أو حالية. */
DECLARE @رقم_الصلاحية_التالي int =
    (SELECT ISNULL(MAX(Id), 0) + 1 FROM dbo.PR_GroupModuleMenuPrivilage);
DECLARE @أمر_تحديث_التسلسل nvarchar(300) =
    N'ALTER SEQUENCE dbo.PR_GroupModuleMenuPrivilage_SEQ RESTART WITH '
    + CONVERT(nvarchar(20), @رقم_الصلاحية_التالي) + N';';
EXEC sys.sp_executesql @أمر_تحديث_التسلسل;

COMMIT TRANSACTION;
GO

/* نتيجة التنفيذ. */
SELECT [group].Id AS [رقم المجموعة],
       [group].GroupName AS [اسم المجموعة],
       module.Id AS [رقم القسم],
       module.ModuleName AS [اسم القسم],
       menu.Id AS [رقم الشاشة],
       menu.MenuTitle AS [اسم الشاشة],
       menu.MenuURL AS [المسار],
       privilege.CanView AS [عرض],
       privilege.CanAdd AS [إضافة],
       privilege.CanEdit AS [تعديل],
       privilege.CanDelete AS [حذف],
       [user].FullName AS [المستخدم]
FROM dbo.PR_GroupModuleMenuPrivilage privilege
INNER JOIN dbo.PR_Group [group] ON [group].Id = privilege.PR_GroupId
INNER JOIN dbo.PR_Module module ON module.Id = privilege.PR_ModuleId
INNER JOIN dbo.PR_Menu menu ON menu.Id = privilege.PR_MenuId
INNER JOIN dbo.PR_User [user] ON [user].Id = privilege.PR_User_id
WHERE [group].GroupName = N'إدارة الصادر الجديدة'
  AND module.ModuleName = N'شاشات الصادر الجديدة'
  AND [user].EmpId = 27336
ORDER BY menu.MenuTitle;
GO
