-- 20261106_PR_Menu_EnablePermissionScreens.sql
-- Enables the two permission screens for the Android client (dbPrivilage.dbo.PR_Menu) and
-- repairs a trailing space that was stored in the Stopping_Permission menu URL.
--
-- Screens (menu rows):
--   Im_Permissions/IM_Permission_Active_Print  : 133, 185, 188, 191, 194
--   Im_Permissions/Stopping_Permission         : 141, 186, 189, 192, 195  (141 had a trailing space)
--
-- Applied on the test server 172.16.45.11 / dbPrivilage on 2026-11-06.
-- Idempotent: re-running is a no-op.

-- 1) Remove leading/trailing spaces from menu URLs.
--    NOTE: SQL Server ignores trailing spaces when comparing strings, so DATALENGTH is used.
UPDATE dbo.PR_Menu
SET MenuURL = LTRIM(RTRIM(MenuURL))
WHERE DATALENGTH(MenuURL) <> DATALENGTH(LTRIM(RTRIM(MenuURL)));
GO

-- 2) Enable both screens for the Android client (all group variants).
UPDATE dbo.PR_Menu
SET isEnableAndroid = 1
WHERE Id IN (
     133, 185, 188, 191, 194,   -- Im_Permissions/IM_Permission_Active_Print
     141, 186, 189, 192, 195    -- Im_Permissions/Stopping_Permission
);
GO

-- Verification: must return the 10 rows with isEnableAndroid = 1 and trimmed URLs.
SELECT Id, MenuTitle, MenuTitle_En, '[' + MenuURL + ']' AS MenuURL, isEnableAndroid
FROM dbo.PR_Menu
WHERE Id IN (133, 185, 188, 191, 194, 141, 186, 189, 192, 195)
ORDER BY Id;
GO
