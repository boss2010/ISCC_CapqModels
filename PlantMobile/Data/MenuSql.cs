namespace PlantMobile.Data;

/// <summary>
/// Single-query replacement for the three nested menu procedures used by the legacy web app
/// (GetGroupsNameByUserAndApplication, GetModulNameByUserAndApplicationandGroup, GetMenuUser).
/// </summary>
internal static class MenuSql
{
    /// <summary>
    /// Builds the menu query. When <paramref name="respectActiveFlags"/> is false the query keeps
    /// the legacy behaviour (menu and module Active flags are ignored), so the result is identical
    /// to the old screens. When <paramref name="onlyEnabledForAndroid"/> is true only menu rows
    /// flagged <c>isEnableAndroid = 1</c> are returned (false hides nothing).
    /// </summary>
    public static string Build(bool respectActiveFlags, bool onlyEnabledForAndroid) =>
        Template
            .Replace("{{ACTIVE_FILTER}}", respectActiveFlags ? StrictActiveFilter : string.Empty)
            .Replace("{{ANDROID_FILTER}}", onlyEnabledForAndroid ? OnlyAndroidFilter : string.Empty);

    private const string StrictActiveFilter = """
          AND ISNULL(mod.Active, 1) = 1
          AND ISNULL(mn.Active, 1) = 1
        """;

    private const string OnlyAndroidFilter = "  AND mn.isEnableAndroid = 1";

    private const string Template = """
        WITH UserMenus AS (
            SELECT prv.PR_GroupId, prv.PR_ModuleId, prv.PR_MenuId,
                   CAST(MAX(CAST(prv.CanView   AS tinyint)) AS bit) AS CanView,
                   CAST(MAX(CAST(prv.CanAdd    AS tinyint)) AS bit) AS CanAdd,
                   CAST(MAX(CAST(prv.CanEdit   AS tinyint)) AS bit) AS CanEdit,
                   CAST(MAX(CAST(prv.CanDelete AS tinyint)) AS bit) AS CanDelete
            FROM dbo.PR_GroupModuleMenuPrivilage prv
            WHERE prv.PR_User_id = @UserId
              AND prv.IS_Active = 1
            GROUP BY prv.PR_GroupId, prv.PR_ModuleId, prv.PR_MenuId
        ),
        MenuOrder AS (
            SELECT PR_GroupId, PR_ModuleId, PR_MenuId, MIN(Order_BY) AS Order_BY
            FROM dbo.PR_GroupModuleMenu
            GROUP BY PR_GroupId, PR_ModuleId, PR_MenuId
        )
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
        FROM UserMenus um
        JOIN dbo.PR_Group g ON g.Id = um.PR_GroupId
        JOIN dbo.PR_Application app ON app.Id = g.PR_ApplicationId
        JOIN dbo.PR_Module mod ON mod.Id = um.PR_ModuleId
        JOIN dbo.PR_Menu mn ON mn.Id = um.PR_MenuId
        JOIN MenuOrder mo
          ON mo.PR_GroupId = um.PR_GroupId
         AND mo.PR_ModuleId = um.PR_ModuleId
         AND mo.PR_MenuId = um.PR_MenuId
        WHERE g.Active = 1
          AND app.PR_ApplicationCategoryId = @ApplicationCategoryId
        {{ACTIVE_FILTER}}
        {{ANDROID_FILTER}}
        ORDER BY g.Id, mod.Id, ISNULL(mo.Order_BY, 2147483647), mn.Id;
        """;
}
