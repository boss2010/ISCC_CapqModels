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
