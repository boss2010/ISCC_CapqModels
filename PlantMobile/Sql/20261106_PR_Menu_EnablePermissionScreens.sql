-- 20261106_PR_Menu_EnablePermissionScreens.sql
-- Enables the two permission screens for the Android client (dbPrivilage.dbo.PR_Menu) and
-- repairs trailing spaces that were stored in the Im_Permissions menu URLs
-- (menu 141 was 'Im_Permissions/Stopping_Permission ' and broke routing).
--
-- Only the rows that belong to a user who actually has the privilege are enabled:
-- target user = mostafa (PR_User_id 354), group 8 / module 32.
--
-- Screens (menu rows):
--   Im_Permissions/IM_Permission_Active_Print  : 133
--   Im_Permissions/Stopping_Permission         : 141
--
-- Other group variants (185/186/188/189/191/192/194/195) are intentionally NOT enabled.
--
-- Applied on the test server 172.16.45.11 / dbPrivilage on 2026-11-06.
-- Idempotent: re-running is a no-op.

-- 1) Remove leading/trailing spaces from menu URLs.
--    NOTE: SQL Server ignores trailing spaces when comparing strings, so DATALENGTH is used.
UPDATE dbo.PR_Menu
SET MenuURL = LTRIM(RTRIM(MenuURL))
WHERE DATALENGTH(MenuURL) <> DATALENGTH(LTRIM(RTRIM(MenuURL)));
GO

-- 2) Enable the two screens for the Android client (only the rows with real privileges).
UPDATE dbo.PR_Menu
SET isEnableAndroid = 1
WHERE Id IN (
     133,   -- Im_Permissions/IM_Permission_Active_Print  (group 8 / module 32)
     141    -- Im_Permissions/Stopping_Permission         (group 8 / module 32)
);
GO

-- Verification: must return the 2 rows with isEnableAndroid = 1 and trimmed URLs.
SELECT Id, MenuTitle, MenuTitle_En, '[' + MenuURL + ']' AS MenuURL, isEnableAndroid
FROM dbo.PR_Menu
WHERE Id IN (133, 141)
ORDER BY Id;
GO
