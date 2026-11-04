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
