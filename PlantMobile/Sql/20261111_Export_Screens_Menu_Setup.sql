/*
    إعداد شاشات الصادر الجديدة في قاعدة الصلاحيات.

    الشاشات:
      1) طلبات الفحص الجديدة
      2) تسكين الموظفين بالمحطات
      3) متابعة محطات الصادر

    ملاحظات:
      - السكربت قابل لإعادة التشغيل دون تكرار السجلات.
      - شاشة التسكين تستخدم جدول PlantQuarantine_New.dbo.Station_Emp الأصلي.
      - لا يحتاج المشروع إلى إنشاء جدول تسكين جديد.
*/

USE [dbPrivilage];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;

/* 1. تحويل عنصر قائمة طلبات فحص الصادر الحالي إلى الشاشة الجديدة. */
DECLARE @ExportRequestsMenuId int;

SELECT TOP (1) @ExportRequestsMenuId = Id
FROM dbo.PR_Menu
WHERE MenuURL = N'Export_CheckRequest/List_EXCheckRequest/Index'
ORDER BY CASE WHEN Id = 100 THEN 0 ELSE 1 END, Id;

IF @ExportRequestsMenuId IS NULL
BEGIN
    SELECT @ExportRequestsMenuId = ISNULL(MAX(Id), 0) + 1
    FROM dbo.PR_Menu WITH (UPDLOCK, HOLDLOCK);

    INSERT dbo.PR_Menu
    (
        Id, MenuTitle, MenuTitle_En, MenuURL, PR_MenuId, Active,
        PR_ModuleId, PR_ApplicationId, PR_ApplicationCategoryId,
        Group_Id, isEnableAndroid
    )
    VALUES
    (
        @ExportRequestsMenuId,
        N'طلبات الفحص الجديدة',
        N'New Inspection Requests',
        N'Export_CheckRequest/List_EXCheckRequest/Index',
        NULL, 1, 38, 1, 1, 7, 1
    );
END
ELSE
BEGIN
    UPDATE dbo.PR_Menu
    SET MenuTitle = N'طلبات الفحص الجديدة',
        MenuTitle_En = N'New Inspection Requests',
        MenuURL = N'Export_CheckRequest/List_EXCheckRequest/Index',
        Active = 1,
        isEnableAndroid = 1
    WHERE Id = @ExportRequestsMenuId;
END;

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.PR_GroupModuleMenu
    WHERE PR_GroupId = 7
      AND PR_ModuleId = 38
      AND PR_MenuId = @ExportRequestsMenuId
)
BEGIN
    INSERT dbo.PR_GroupModuleMenu
        (PR_GroupId, PR_ModuleId, PR_MenuId, IS_Active, Order_BY)
    VALUES
        (7, 38, @ExportRequestsMenuId, 1, 77);
END;

/* 2. شاشة تسكين الموظفين بالمحطات. */
DECLARE @StationAssignmentMenuId int;

SELECT TOP (1) @StationAssignmentMenuId = Id
FROM dbo.PR_Menu
WHERE MenuURL = N'Employee/StationAssignment/Index';

IF @StationAssignmentMenuId IS NULL
BEGIN
    SELECT @StationAssignmentMenuId = ISNULL(MAX(Id), 0) + 1
    FROM dbo.PR_Menu WITH (UPDLOCK, HOLDLOCK);

    INSERT dbo.PR_Menu
    (
        Id, MenuTitle, MenuTitle_En, MenuURL, PR_MenuId, Active,
        PR_ModuleId, PR_ApplicationId, PR_ApplicationCategoryId,
        Group_Id, isEnableAndroid
    )
    VALUES
    (
        @StationAssignmentMenuId,
        N'تسكين الموظفين بالمحطات',
        N'Employee Station Assignment',
        N'Employee/StationAssignment/Index',
        NULL, 1, 30, 1, 1, 16, 1
    );
END
ELSE
BEGIN
    UPDATE dbo.PR_Menu
    SET MenuTitle = N'تسكين الموظفين بالمحطات',
        MenuTitle_En = N'Employee Station Assignment',
        Active = 1,
        isEnableAndroid = 1
    WHERE Id = @StationAssignmentMenuId;
END;

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.PR_GroupModuleMenu
    WHERE PR_GroupId = 16
      AND PR_ModuleId = 30
      AND PR_MenuId = @StationAssignmentMenuId
)
BEGIN
    INSERT dbo.PR_GroupModuleMenu
        (PR_GroupId, PR_ModuleId, PR_MenuId, IS_Active, Order_BY)
    VALUES
        (16, 30, @StationAssignmentMenuId, 1, 116);
END;

/* نسخ مستخدمي شاشة "الموظفين داخل منفذ" رقم 128 إلى شاشة التسكين. */
DECLARE @PrivilegeBase int;

SELECT @PrivilegeBase = ISNULL(MAX(Id), 0)
FROM dbo.PR_GroupModuleMenuPrivilage WITH (UPDLOCK, HOLDLOCK);

;WITH MissingAssignmentPrivileges AS
(
    SELECT
        MIN(p.Id) AS SourcePrivilegeId,
        p.PR_User_id,
        CAST(MAX(CAST(p.CanView AS tinyint)) AS bit) AS CanView,
        CAST(MAX(CAST(p.CanAdd AS tinyint)) AS bit) AS CanAdd,
        CAST(MAX(CAST(p.CanEdit AS tinyint)) AS bit) AS CanEdit,
        CAST(MAX(CAST(p.CanDelete AS tinyint)) AS bit) AS CanDelete,
        CAST(MAX(CAST(ISNULL(p.CanPrint, 0) AS tinyint)) AS bit) AS CanPrint
    FROM dbo.PR_GroupModuleMenuPrivilage p
    WHERE p.PR_GroupId = 16
      AND p.PR_ModuleId = 30
      AND p.PR_MenuId = 128
      AND p.IS_Active = 1
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.PR_GroupModuleMenuPrivilage currentPrivilege
          WHERE currentPrivilege.PR_GroupId = 16
            AND currentPrivilege.PR_ModuleId = 30
            AND currentPrivilege.PR_MenuId = @StationAssignmentMenuId
            AND currentPrivilege.PR_User_id = p.PR_User_id
      )
    GROUP BY p.PR_User_id
)
INSERT dbo.PR_GroupModuleMenuPrivilage
(
    Id, PR_GroupId, PR_ModuleId, PR_MenuId,
    CanView, CanAdd, CanEdit, CanDelete, CanPrint,
    PR_User_id, IS_Active
)
SELECT
    @PrivilegeBase + ROW_NUMBER() OVER (ORDER BY SourcePrivilegeId),
    16, 30, @StationAssignmentMenuId,
    CanView, CanAdd, CanEdit, CanDelete, CanPrint,
    PR_User_id, 1
FROM MissingAssignmentPrivileges;

/* منح مصطفى أحمد محمد طلبه صلاحيات شاشة التسكين كاملة. */
DECLARE @MostafaUserId smallint;

SELECT TOP (1) @MostafaUserId = CONVERT(smallint, Id)
FROM dbo.PR_User
WHERE Active = 1
  AND
  (
      EmpId = 27338
      OR LoginName = N'mostafa'
      OR FullName = N'مصطفى احمد محمد طلبه'
  )
ORDER BY CASE WHEN EmpId = 27338 THEN 0 WHEN LoginName = N'mostafa' THEN 1 ELSE 2 END;

IF @MostafaUserId IS NOT NULL
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM dbo.PR_GroupModuleMenuPrivilage
        WHERE PR_GroupId = 16
          AND PR_ModuleId = 30
          AND PR_MenuId = @StationAssignmentMenuId
          AND PR_User_id = @MostafaUserId
    )
    BEGIN
        UPDATE dbo.PR_GroupModuleMenuPrivilage
        SET CanView = 1,
            CanAdd = 1,
            CanEdit = 1,
            CanDelete = 1,
            CanPrint = 0,
            IS_Active = 1
        WHERE PR_GroupId = 16
          AND PR_ModuleId = 30
          AND PR_MenuId = @StationAssignmentMenuId
          AND PR_User_id = @MostafaUserId;
    END
    ELSE
    BEGIN
        DECLARE @MostafaPrivilegeId int;

        SELECT @MostafaPrivilegeId = ISNULL(MAX(Id), 0) + 1
        FROM dbo.PR_GroupModuleMenuPrivilage WITH (UPDLOCK, HOLDLOCK);

        INSERT dbo.PR_GroupModuleMenuPrivilage
        (
            Id, PR_GroupId, PR_ModuleId, PR_MenuId,
            CanView, CanAdd, CanEdit, CanDelete, CanPrint,
            PR_User_id, IS_Active
        )
        VALUES
        (
            @MostafaPrivilegeId, 16, 30, @StationAssignmentMenuId,
            1, 1, 1, 1, 0, @MostafaUserId, 1
        );
    END;
END;

/* 3. شاشة متابعة محطات الصادر. */
DECLARE @StationsDashboardMenuId int;

SELECT TOP (1) @StationsDashboardMenuId = Id
FROM dbo.PR_Menu
WHERE MenuURL = N'Export_CheckRequest/List_EXCheckRequest/Stations';

IF @StationsDashboardMenuId IS NULL
BEGIN
    SELECT @StationsDashboardMenuId = ISNULL(MAX(Id), 0) + 1
    FROM dbo.PR_Menu WITH (UPDLOCK, HOLDLOCK);

    INSERT dbo.PR_Menu
    (
        Id, MenuTitle, MenuTitle_En, MenuURL, PR_MenuId, Active,
        PR_ModuleId, PR_ApplicationId, PR_ApplicationCategoryId,
        Group_Id, isEnableAndroid
    )
    VALUES
    (
        @StationsDashboardMenuId,
        N'متابعة محطات الصادر',
        N'Export Stations Dashboard',
        N'Export_CheckRequest/List_EXCheckRequest/Stations',
        NULL, 1, 38, 1, 1, 7, 1
    );
END
ELSE
BEGIN
    UPDATE dbo.PR_Menu
    SET MenuTitle = N'متابعة محطات الصادر',
        MenuTitle_En = N'Export Stations Dashboard',
        Active = 1,
        isEnableAndroid = 1
    WHERE Id = @StationsDashboardMenuId;
END;

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.PR_GroupModuleMenu
    WHERE PR_GroupId = 7
      AND PR_ModuleId = 38
      AND PR_MenuId = @StationsDashboardMenuId
)
BEGIN
    INSERT dbo.PR_GroupModuleMenu
        (PR_GroupId, PR_ModuleId, PR_MenuId, IS_Active, Order_BY)
    VALUES
        (7, 38, @StationsDashboardMenuId, 1, 76);
END;

/* نسخ صلاحية العرض من قائمة طلبات فحص الصادر إلى شاشة متابعة المحطات. */
SELECT @PrivilegeBase = ISNULL(MAX(Id), 0)
FROM dbo.PR_GroupModuleMenuPrivilage WITH (UPDLOCK, HOLDLOCK);

;WITH MissingDashboardPrivileges AS
(
    SELECT
        MIN(p.Id) AS SourcePrivilegeId,
        p.PR_User_id
    FROM dbo.PR_GroupModuleMenuPrivilage p
    WHERE p.PR_GroupId = 7
      AND p.PR_ModuleId = 38
      AND p.PR_MenuId = @ExportRequestsMenuId
      AND p.IS_Active = 1
      AND p.CanView = 1
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.PR_GroupModuleMenuPrivilage currentPrivilege
          WHERE currentPrivilege.PR_GroupId = 7
            AND currentPrivilege.PR_ModuleId = 38
            AND currentPrivilege.PR_MenuId = @StationsDashboardMenuId
            AND currentPrivilege.PR_User_id = p.PR_User_id
      )
    GROUP BY p.PR_User_id
)
INSERT dbo.PR_GroupModuleMenuPrivilage
(
    Id, PR_GroupId, PR_ModuleId, PR_MenuId,
    CanView, CanAdd, CanEdit, CanDelete, CanPrint,
    PR_User_id, IS_Active
)
SELECT
    @PrivilegeBase + ROW_NUMBER() OVER (ORDER BY SourcePrivilegeId),
    7, 38, @StationsDashboardMenuId,
    1, 0, 0, 0, 0,
    PR_User_id, 1
FROM MissingDashboardPrivileges;

COMMIT TRANSACTION;
GO

/* نتيجة تنفيذ مختصرة. */
SELECT
    menu.Id,
    menu.MenuTitle,
    menu.MenuURL,
    groupMenu.PR_GroupId,
    groupMenu.PR_ModuleId,
    groupMenu.Order_BY,
    COUNT(DISTINCT CASE
        WHEN privilege.IS_Active = 1 AND privilege.CanView = 1
        THEN privilege.PR_User_id
    END) AS UsersWithViewAccess
FROM dbo.PR_Menu menu
LEFT JOIN dbo.PR_GroupModuleMenu groupMenu
    ON groupMenu.PR_MenuId = menu.Id
LEFT JOIN dbo.PR_GroupModuleMenuPrivilage privilege
    ON privilege.PR_MenuId = menu.Id
WHERE menu.MenuURL IN
(
    N'Export_CheckRequest/List_EXCheckRequest/Index',
    N'Employee/StationAssignment/Index',
    N'Export_CheckRequest/List_EXCheckRequest/Stations'
)
GROUP BY
    menu.Id, menu.MenuTitle, menu.MenuURL,
    groupMenu.PR_GroupId, groupMenu.PR_ModuleId, groupMenu.Order_BY
ORDER BY menu.Id;
GO
