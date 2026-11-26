/*
    20261125_Grant_Committee_InspectionResult_To_Elnaby.sql
    يضيف شاشة «نتيجة فحص اللجنة» ليوزر alnaby / Elnaby.
    آمن لإعادة التنفيذ (UPSERT): لا يعمل duplicate صلاحيات.
    الصلاحيات: View / Add / Edit / Print = 1، Delete = 0.

    المسار: Committees/ImCommittee_InspectionResult/Index
*/

USE [dbPrivilage];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Url nvarchar(300) = N'Committees/ImCommittee_InspectionResult/Index';
    DECLARE @MenuId int;

    SELECT TOP (1) @MenuId = Id
    FROM dbo.PR_Menu
    WHERE LOWER(LTRIM(RTRIM(MenuURL))) = LOWER(@Url)
    ORDER BY Id DESC;

    IF @MenuId IS NULL
        THROW 50001, 'الشاشة غير موجودة. نفّذ 20261122_Add_Committee_InspectionResult_Screen.sql أولاً.', 1;

    -- group / module من ربط الشاشة، وال fallback من تعريف الشاشة نفسها
    DECLARE @GroupId int, @ModuleId int;
    SELECT TOP (1)
        @GroupId = PR_GroupId,
        @ModuleId = PR_ModuleId
    FROM dbo.PR_GroupModuleMenu
    WHERE PR_MenuId = @MenuId
    ORDER BY PR_GroupId, PR_ModuleId;

    IF @GroupId IS NULL
        SELECT @GroupId = Group_Id FROM dbo.PR_Menu WHERE Id = @MenuId;
    IF @ModuleId IS NULL
        SELECT @ModuleId = PR_ModuleId FROM dbo.PR_Menu WHERE Id = @MenuId;

    -- يوزر alnaby: LoginName = Elnaby / EmpId = 27336 / Id = 353
    DECLARE @UserId smallint;
    SELECT TOP (1) @UserId = CONVERT(smallint, Id)
    FROM dbo.PR_User
    WHERE Active = 1
      AND (EmpId = 27336 OR LOWER(LTRIM(RTRIM(LoginName))) = N'elnaby')
    ORDER BY CASE WHEN EmpId = 27336 THEN 0 ELSE 1 END;

    IF @UserId IS NULL
        THROW 50002, 'لم يتم العثور على يوزر Elnaby (EmpId 27336 / LoginName Elnaby).', 1;

    -- تأكد إن الشاشة مفعّلة في المشروع الجديد
    UPDATE dbo.PR_Menu
    SET Active = 1, isEnableAndroid = 1
    WHERE Id = @MenuId;

    -- ظهور الشاشة داخل المجموعة / الوحدة
    IF EXISTS
    (
        SELECT 1 FROM dbo.PR_GroupModuleMenu
        WHERE PR_GroupId = @GroupId AND PR_ModuleId = @ModuleId AND PR_MenuId = @MenuId
    )
        UPDATE dbo.PR_GroupModuleMenu
        SET IS_Active = 1, Order_BY = 95
        WHERE PR_GroupId = @GroupId AND PR_ModuleId = @ModuleId AND PR_MenuId = @MenuId;
    ELSE
        INSERT dbo.PR_GroupModuleMenu (PR_GroupId, PR_ModuleId, PR_MenuId, IS_Active, Order_BY)
        VALUES (@GroupId, @ModuleId, @MenuId, 1, 95);

    -- صلاحية اليوزر
    IF EXISTS
    (
        SELECT 1 FROM dbo.PR_GroupModuleMenuPrivilage
        WHERE PR_GroupId = @GroupId AND PR_ModuleId = @ModuleId
          AND PR_MenuId = @MenuId AND PR_User_id = @UserId
    )
        UPDATE dbo.PR_GroupModuleMenuPrivilage
        SET CanView = 1, CanAdd = 1, CanEdit = 1, CanDelete = 0, CanPrint = 1, IS_Active = 1
        WHERE PR_GroupId = @GroupId AND PR_ModuleId = @ModuleId
          AND PR_MenuId = @MenuId AND PR_User_id = @UserId;
    ELSE
        INSERT dbo.PR_GroupModuleMenuPrivilage
            (Id, PR_GroupId, PR_ModuleId, PR_MenuId,
             CanView, CanAdd, CanEdit, CanDelete, CanPrint, PR_User_id, IS_Active)
        VALUES
            (NEXT VALUE FOR dbo.PR_GroupModuleMenuPrivilage_SEQ,
             @GroupId, @ModuleId, @MenuId, 1, 1, 1, 0, 1, @UserId, 1);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

-- تحقق
SELECT Id, LoginName, EmpId, FullName
FROM dbo.PR_User
WHERE Id = 353;

SELECT m.Id, m.MenuTitle, m.MenuURL, m.isEnableAndroid,
       p.PR_GroupId, p.PR_ModuleId,
       p.CanView, p.CanAdd, p.CanEdit, p.CanDelete, p.CanPrint, p.IS_Active
FROM dbo.PR_GroupModuleMenuPrivilage p
JOIN dbo.PR_Menu m ON m.Id = p.PR_MenuId
WHERE p.PR_User_id = 353
  AND LOWER(LTRIM(RTRIM(m.MenuURL))) = N'committees/imcommittee_inspectionresult/index';
GO
