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
