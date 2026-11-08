-- ============================================================
-- ISCC PlantQuarantine / PlantMobile
-- Menu scripts combined - 2026-11-04 (Wednesday)
-- Tareekh: 2026-11-04
-- ============================================================
-- Prepared by: OpenCode (PlantMobile epic) - then approved by owner
-- Project: PlantMobile (ISCC) - path: PlantMobile/Sql
-- git: boss2010/ISCC_CapqModels @ master (788bd54)
--
-- IMPLEMENTED / EXECUTED ON: DESKTOP-3OTJB3D (WIN-272BMNKC9OI) - dbPrivilage
--   the current database used by the NEW PlantMobile application
-- NOT EXECUTED ON: 10.7.7.225 (DESKTOP-044HRJM) - dbPrivilage
--   the current database used by the LEGACY PlantQuar web app
--   (still reachable; no schema changes were applied there)
--
-- What is in this file:
--   PART 0 - How to read this file + status of each script
--   PART 1 - 20261104_Menu_GetByUser.sql        (reference query + parity check - read-only)
--   PART 2 - 20261104_PR_Menu_isEnableAndroid.sql (add PR_Menu.isEnableAndroid bit col, default 0)
--   PART 3 - 20261104_PR_Menu_AddRootScreens.sql (add 255/256 menus + modules 57/58 + privileges for user 354)
--   PART 4 - Verification queries (after execution)
--   PART 5 - Rollback hints and sign-off block
--
-- SAFETY
--   - All DDL is additive. No rows are deleted or updated.
--   - Parts 2 and 3 are idempotent guarded.
--   - Execute in SSMS with a transaction-aware session if you re-run them.
-- ============================================================

-- ============================================================
-- PART 1 - 20261104_Menu_GetByUser.sql
-- ============================================================

/*
    Menu query for PlantMobile — reference and parity check.

    The application uses Data/MenuSql.cs (same query, parameterised).
    This file exists so the query can be reviewed and compared against the legacy
    three-procedure menu in SSMS before/after deployment.

    Notes
    -----
    * @ApplicationCategoryId = 1 is the legacy web application (PR_Application.PR_ApplicationCategoryId).
    * Menu/module Active flags are ignored by default, exactly like the legacy screens.
      Add the two lines marked STRICT to reproduce Menu:RespectActiveFlags = true.
    * Only rows flagged isEnableAndroid = 1 appear by default (Menu:OnlyEnabledForAndroid = true).
      Flip @OnlyAndroidEnabled to 0 to see every permitted screen again.
    * The legacy screen also hides Im_CheckRequests/Im_Fumigation/ReleaseSupervisorTasks
      unconditionally (DrawMenuController). The application removes it in
      MenuService (Menu:ExcludedUrls), so the raw counts below can differ by one row
      when that menu is active.
*/

DECLARE @UserId smallint = 1;
DECLARE @IsArabic bit = 1;
DECLARE @ApplicationCategoryId int = 1;
DECLARE @OnlyAndroidEnabled bit = 1; -- 1 = only isEnableAndroid rows (matches Menu:OnlyEnabledForAndroid)

/* ------------------------- New single query ------------------------- */
CREATE TABLE #NewMenu
(
    GroupId int, GroupName nvarchar(200),
    ModuleId int, ModuleName nvarchar(200),
    MenuId int, MenuTitle nvarchar(200), MenuUrl nvarchar(400),
    CanView bit, CanAdd bit, CanEdit bit, CanDelete bit
);

INSERT INTO #NewMenu
SELECT
    g.Id AS GroupId,
    CASE WHEN @IsArabic = 1 THEN g.GroupName
         ELSE COALESCE(NULLIF(g.GroupName_En, ''), g.GroupName) END AS GroupName,
    mod.Id AS ModuleId,
    CASE WHEN @IsArabic = 1 THEN mod.ModuleName
         ELSE COALESCE(NULLIF(mod.ModuleName_En, ''), mod.ModuleName) END AS ModuleName,
    mn.Id AS MenuId,
    CASE WHEN @IsArabic = 1 THEN mn.MenuTitle
         ELSE COALESCE(NULLIF(mn.MenuTitle_En, ''), mn.MenuTitle) END AS MenuTitle,
    mn.MenuURL AS MenuUrl,
    um.CanView, um.CanAdd, um.CanEdit, um.CanDelete
FROM (
    SELECT prv.PR_GroupId, prv.PR_ModuleId, prv.PR_MenuId,
           CAST(MAX(CAST(prv.CanView   AS tinyint)) AS bit) AS CanView,
           CAST(MAX(CAST(prv.CanAdd    AS tinyint)) AS bit) AS CanAdd,
           CAST(MAX(CAST(prv.CanEdit   AS tinyint)) AS bit) AS CanEdit,
           CAST(MAX(CAST(prv.CanDelete AS tinyint)) AS bit) AS CanDelete
    FROM dbo.PR_GroupModuleMenuPrivilage prv
    WHERE prv.PR_User_id = @UserId
      AND prv.IS_Active = 1
    GROUP BY prv.PR_GroupId, prv.PR_ModuleId, prv.PR_MenuId
) um
JOIN dbo.PR_Group g ON g.Id = um.PR_GroupId
JOIN dbo.PR_Application app ON app.Id = g.PR_ApplicationId
JOIN dbo.PR_Module mod ON mod.Id = um.PR_ModuleId
JOIN dbo.PR_Menu mn ON mn.Id = um.PR_MenuId
JOIN (
    SELECT PR_GroupId, PR_ModuleId, PR_MenuId, MIN(Order_BY) AS Order_BY
    FROM dbo.PR_GroupModuleMenu
    GROUP BY PR_GroupId, PR_ModuleId, PR_MenuId
) mo
  ON mo.PR_GroupId = um.PR_GroupId
 AND mo.PR_ModuleId = um.PR_ModuleId
 AND mo.PR_MenuId = um.PR_MenuId
WHERE g.Active = 1
  AND app.PR_ApplicationCategoryId = @ApplicationCategoryId
  AND (@OnlyAndroidEnabled = 0 OR mn.isEnableAndroid = 1)
-- STRICT (uncomment to match Menu:RespectActiveFlags = true)
--AND ISNULL(mod.Active, 1) = 1
--AND ISNULL(mn.Active, 1) = 1
ORDER BY g.Id, mod.Id, ISNULL(mo.Order_BY, 2147483647), mn.Id;

/* ------------------ Legacy equivalent (three procedures) ------------------ */
CREATE TABLE #LegacyMenu
(
    Id int, MenuTitle nvarchar(200), MenuURL nvarchar(400),
    CanView bit, CanAdd bit, CanEdit bit, CanDelete bit
);

DECLARE @GroupId int, @ModuleId int;
DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
    SELECT DISTINCT prv.PR_GroupId, prv.PR_ModuleId
    FROM dbo.PR_GroupModuleMenuPrivilage prv
    JOIN dbo.PR_Group g ON g.Id = prv.PR_GroupId
    JOIN dbo.PR_Application app ON app.Id = g.PR_ApplicationId
    WHERE prv.PR_User_id = @UserId
      AND prv.IS_Active = 1
      AND g.Active = 1
      AND app.PR_ApplicationCategoryId = @ApplicationCategoryId;

OPEN cur;
FETCH NEXT FROM cur INTO @GroupId, @ModuleId;
WHILE @@FETCH_STATUS = 0
BEGIN
    INSERT INTO #LegacyMenu (Id, MenuTitle, MenuURL, CanView, CanAdd, CanEdit, CanDelete)
    EXEC dbo.GetMenuUser
        @PR_ApplicationCategoryId = @ApplicationCategoryId,
        @PR_ModuleId = @ModuleId,
        @PR_UserId = @UserId,
        @GroupID = @GroupId,
        @Language = @IsArabic;

    FETCH NEXT FROM cur INTO @GroupId, @ModuleId;
END
CLOSE cur;
DEALLOCATE cur;

/* ------------------------------- Compare ------------------------------- */
SELECT 'Legacy' AS Source, COUNT(*) AS MenuRows FROM #LegacyMenu
UNION ALL
SELECT 'New', COUNT(*) FROM #NewMenu;

SELECT COALESCE(l.Id, n.MenuId) AS MenuId,
       l.Id AS InLegacy,
       n.MenuId AS InNew
FROM #LegacyMenu l
FULL OUTER JOIN #NewMenu n ON n.MenuId = l.Id
WHERE l.Id IS NULL OR n.MenuId IS NULL;

DROP TABLE #NewMenu, #LegacyMenu;


-- ============================================================
-- PART 2 - 20261104_PR_Menu_isEnableAndroid.sql
-- ============================================================

-- 20261104_PR_Menu_isEnableAndroid.sql
-- Adds isEnableAndroid to dbPrivilage.dbo.PR_Menu (idempotent).
-- Default 0 for all existing rows: the Android client shows only menus flagged 1.
-- Safe additive change; the legacy web application ignores the column.

IF COL_LENGTH('dbo.PR_Menu', 'isEnableAndroid') IS NULL
BEGIN
    ALTER TABLE dbo.PR_Menu
        ADD isEnableAndroid BIT NOT NULL
            CONSTRAINT DF_PR_Menu_isEnableAndroid DEFAULT (0);
END;
GO

-- Review current state
SELECT COUNT(*) AS TotalRows,
       SUM(CASE WHEN isEnableAndroid = 1 THEN 1 ELSE 0 END) AS EnabledForAndroid
FROM dbo.PR_Menu;
GO

-- Enable specific menus (example — replace with the chosen IDs):
-- UPDATE dbo.PR_Menu SET isEnableAndroid = 1 WHERE Id IN (166, 231);
-- GO

-- Enable everything (not recommended initially):
-- UPDATE dbo.PR_Menu SET isEnableAndroid = 1;
-- GO


-- ============================================================
-- PART 3 - 20261104_PR_Menu_AddRootScreens.sql
-- ============================================================

-- 20261104_PR_Menu_AddRootScreens.sql
-- Adds the two application root screens to the menu tree in dbPrivilage (DESKTOP-3OTJB3D):
--   255  «طلبات الوارد»   ImportRequests      (Android: enabled)
--   256  «نتيجة المعمل»   LaboratoryResult    (Android: enabled)
-- One module per screen, under its own group (the layers the app reads):
--   57   «طلبات الوارد»   group 8  (الوارد)
--   58   «نتيجة المعمل»   group 15 (تحاليل)
-- Privileges are granted to user 354 (Mostafa) only — extend later as needed.
-- Additive only; runs inside one transaction with automatic rollback on error.
-- Verify with the final SELECTs.

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF EXISTS (SELECT 1 FROM dbo.PR_Module WHERE Id IN (57, 58))
        THROW 51057, 'Module Id 57 or 58 already exists.', 1;
    IF EXISTS (SELECT 1 FROM dbo.PR_Menu WHERE Id IN (255, 256))
        THROW 51255, 'Menu Id 255 or 256 already exists.', 1;

    INSERT INTO dbo.PR_Module (Id, ModuleName, ModuleName_En, ModuleDescription,
                               PR_ApplicationId, PR_ApplicationCategoryId, Active)
    VALUES (57, N'طلبات الوارد',  'Import Requests',  NULL, 1, 1, 1),
           (58, N'نتيجة المعمل', 'Laboratory Result', NULL, 1, 1, 1);

    INSERT INTO dbo.PR_Menu (Id, MenuTitle, MenuTitle_En, MenuURL, PR_MenuId, Active,
                             PR_ModuleId, PR_ApplicationId, PR_ApplicationCategoryId, Group_Id, isEnableAndroid)
    VALUES (255, N'طلبات الوارد',  'Import Requests',  'ImportRequests',   255, 1, 57, 1, 1, 1, 1),
           (256, N'نتيجة المعمل', 'Laboratory Result', 'LaboratoryResult', 256, 1, 58, 1, 1, 1, 1);

    INSERT INTO dbo.PR_GroupModuleMenu (PR_GroupId, PR_ModuleId, PR_MenuId, IS_Active, Order_BY)
    VALUES (8,  57, 255, 1, 1),
           (15, 58, 256, 1, 1);

    DECLARE @privId int = (SELECT MAX(Id) + 1 FROM dbo.PR_GroupModuleMenuPrivilage);

    INSERT INTO dbo.PR_GroupModuleMenuPrivilage
        (Id, PR_GroupId, PR_ModuleId, PR_MenuId,
         CanView, CanAdd, CanEdit, CanDelete, CanPrint, PR_User_id, IS_Active)
    VALUES (@privId,      8, 57, 255, 1, 1, 1, 1, 1, 354, 1),
           (@privId + 1, 15, 58, 256, 1, 1, 1, 1, 1, 354, 1);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;

-- Verification
SELECT Id, MenuTitle, MenuURL, isEnableAndroid, PR_ModuleId, PR_ApplicationCategoryId
FROM dbo.PR_Menu
WHERE Id IN (255, 256);

SELECT gmm.*, g.GroupName, m.ModuleName
FROM dbo.PR_GroupModuleMenu gmm
JOIN dbo.PR_Group g ON g.Id = gmm.PR_GroupId
JOIN dbo.PR_Module m ON m.Id = gmm.PR_ModuleId
WHERE gmm.PR_MenuId IN (255, 256);

SELECT *
FROM dbo.PR_GroupModuleMenuPrivilage
WHERE PR_MenuId IN (255, 256)
ORDER BY PR_MenuId;
GO


-- ============================================================
-- PART 4 - Verification queries (run after execution)
-- ============================================================
-- 1) Column present?
SELECT COL_LENGTH('dbo.PR_Menu','isEnableAndroid') AS ColLen;
-- 2) Flags summary
SELECT COUNT(*) AS Rows, SUM(CAST(isEnableAndroid AS int)) AS Enabled FROM dbo.PR_Menu;
-- 3) New root screens present?
SELECT Id, MenuTitle, MenuURL, Active, isEnableAndroid FROM dbo.PR_Menu WHERE Id IN (255, 256);
-- 4) Menu locations linked?
SELECT PR_GroupId, PR_ModuleId, PR_MenuId, IS_Active FROM dbo.PR_GroupModuleMenu WHERE PR_MenuId IN (255, 256);
-- 5) Mostfa (354) menu visibility
SELECT PR_MenuId FROM dbo.PR_GroupModuleMenuPrivilage WHERE PR_User_id = 354 AND PR_MenuId IN (255, 256) AND IS_Active = 1;

-- ============================================================
-- PART 5 - Rollback hints and sign-off
-- ============================================================
-- Nothing was destructive. To revert:
--   - DELETE FROM dbo.PR_GroupModuleMenu WHERE PR_MenuId IN (255, 256);
--   - DELETE FROM dbo.PR_GroupModuleMenuPrivilage WHERE PR_MenuId IN (255, 256);
--   - DELETE FROM dbo.PR_Menu WHERE Id IN (255, 256);
--   - DELETE FROM dbo.PR_Module WHERE Id IN (57, 58);
--   - ALTER TABLE dbo.PR_Menu DROP CONSTRAINT DF_PR_Menu_isEnableAndroid;
--     ALTER TABLE dbo.PR_Menu DROP COLUMN isEnableAndroid;   -- only if you want the column gone
-- Sign / approval block
-- Date:        2026-11-04
-- Prepared by: OpenCode (automatic) / PlantMobile project
-- Approved by: ____________________  Date: ____________
-- Executed by: ____________________  Date: ____________
-- Verified by: ____________________  Date: ____________
