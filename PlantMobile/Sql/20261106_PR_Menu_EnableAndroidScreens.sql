-- 20261106_PR_Menu_EnableAndroidScreens.sql
-- Enables the migrated screens for the Android client (dbPrivilage.dbo.PR_Menu).
--
-- State captured from the live server (172.16.45.11 / dbPrivilage) on 2026-11-06,
-- after the owner enabled them from SSMS. Running this script on any server
-- reproduces exactly that state (idempotent: re-running is a no-op).
--
-- Screens that are isEnableAndroid = 1:
--   84   ListIm_Permission                        Im_Permissions/ListIm_Permission/index
--   134  Im_CheckRequests outlet                  Im_CheckRequests/List_ImCheckRequestOutlet/Index
--   197  Im_CheckRequests outlet (group variant)  Im_CheckRequests/List_ImCheckRequestOutlet/Index
--   198  Im_checkRequest_Update_Port              Im_CheckRequests/Im_checkRequest_Update_Port
--   200  Im_CheckRequests outlet (group variant)  Im_CheckRequests/List_ImCheckRequestOutlet/Index
--   201  Im_checkRequest_Update_Port (variant)    Im_CheckRequests/Im_checkRequest_Update_Port
--   203  Im_CheckRequests outlet (group variant)  Im_CheckRequests/List_ImCheckRequestOutlet/Index
--   204  Im_checkRequest_Update_Port (variant)    Im_CheckRequests/Im_checkRequest_Update_Port
--   206  Im_CheckRequests outlet (group variant)  Im_CheckRequests/List_ImCheckRequestOutlet/Index
--   207  Im_checkRequest_Update_Port (variant)    Im_CheckRequests/Im_checkRequest_Update_Port
--   255  ImportRequests (root screen)             ImportRequests
--   256  LaboratoryResult (root screen)           LaboratoryResult
--
-- Prerequisite: 20261104_PR_Menu_isEnableAndroid.sql (adds the isEnableAndroid column).

UPDATE dbo.PR_Menu
SET isEnableAndroid = 1
WHERE Id IN (
     84   -- ListIm_Permission
    ,134  -- Im_CheckRequests outlet (Im_CheckRequests/List_ImCheckRequestOutlet/Index)
    ,197  -- Im_CheckRequests outlet (group variant)
    ,198  -- Im_checkRequest_Update_Port
    ,200  -- Im_CheckRequests outlet (group variant)
    ,201  -- Im_checkRequest_Update_Port (group variant)
    ,203  -- Im_CheckRequests outlet (group variant)
    ,204  -- Im_checkRequest_Update_Port (group variant)
    ,206  -- Im_CheckRequests outlet (group variant)
    ,207  -- Im_checkRequest_Update_Port (group variant)
    ,255  -- ImportRequests (root screen)
    ,256  -- LaboratoryResult (root screen)
);
GO

-- Verification: must return the same 12 rows with isEnableAndroid = 1
SELECT Id, MenuTitle, MenuTitle_En, MenuURL, isEnableAndroid
FROM dbo.PR_Menu
WHERE isEnableAndroid = 1
ORDER BY Id;
GO
