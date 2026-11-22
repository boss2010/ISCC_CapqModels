/*
    20261122_Add_Committee_InspectionResult_Screen.sql
    تسجيل شاشة «نتيجة فحص اللجنة» في dbPrivilage:
      - تفعيل isEnableAndroid = 1 (تظهر في المشروع الجديد)
      - ربطها بالمجموعة/القسم
      - منح المستخدم مصطفى صلاحية الشاشة

    المسار: Committees/ImCommittee_InspectionResult/Index
    آمن لإعادة التنفيذ. الصلاحيات: View/Add/Edit/Print = 1، Delete = 0.
*/

USE [dbPrivilage];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

-- العمود مضاف أصلاً بواسطة 20261104_PR_Menu_isEnableAndroid.sql؛ نضيفه هنا لو ناقص.
IF COL_LENGTH(N'dbo.PR_Menu', N'isEnableAndroid') IS NULL
BEGIN
    ALTER TABLE dbo.PR_Menu
        ADD isEnableAndroid bit NOT NULL CONSTRAINT DF_PR_Menu_isEnableAndroid DEFAULT (0);
END;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Url nvarchar(300) = N'Committees/ImCommittee_InspectionResult/Index';
    DECLARE @MenuId int;

    -- grup/module الأصل: ننسخهم من شاشة تشكيل اللجنة لو موجودة، وإلا group 8 / module 32
    DECLARE @SourceGroup int, @SourceModule int, @SourceApp int, @SourceCategory int, @SourceParent int;
    SELECT TOP (1)
        @SourceGroup = Group_Id,
        @SourceModule = PR_ModuleId,
        @SourceApp = PR_ApplicationId,
        @SourceCategory = PR_ApplicationCategoryId,
        @SourceParent = PR_MenuId
    FROM dbo.PR_Menu
    WHERE LOWER(LTRIM(RTRIM(MenuURL))) = N'committees/committeetype/index';

    SELECT @MenuId = MAX(Id)
    FROM dbo.PR_Menu
    WHERE LOWER(LTRIM(RTRIM(MenuURL))) = LOWER(@Url);

    IF @MenuId IS NULL
    BEGIN
        SELECT @MenuId = ISNULL(MAX(Id), 0) + 1
        FROM dbo.PR_Menu WITH (UPDLOCK, HOLDLOCK);

        INSERT dbo.PR_Menu
            (Id, MenuTitle, MenuTitle_En, MenuURL, PR_MenuId, Active,
             PR_ModuleId, PR_ApplicationId, PR_ApplicationCategoryId, Group_Id, isEnableAndroid)
        VALUES
            (@MenuId, N'نتيجة فحص اللجنة', N'Committee Inspection Result', @Url,
             @SourceParent, 1,
             ISNULL(@SourceModule, 32), ISNULL(@SourceApp, 1),
             ISNULL(@SourceCategory, 1), ISNULL(@SourceGroup, 8), 1);
    END
    ELSE
    BEGIN
        UPDATE dbo.PR_Menu
        SET MenuTitle = N'نتيجة فحص اللجنة',
            MenuTitle_En = N'Committee Inspection Result',
            Active = 1,
            isEnableAndroid = 1,
            PR_MenuId = @SourceParent,
            PR_ModuleId = ISNULL(@SourceModule, PR_ModuleId),
            PR_ApplicationId = ISNULL(@SourceApp, PR_ApplicationId),
            PR_ApplicationCategoryId = ISNULL(@SourceCategory, PR_ApplicationCategoryId),
            Group_Id = ISNULL(@SourceGroup, Group_Id)
        WHERE Id = @MenuId;
    END;

    DECLARE @GroupId int = ISNULL(@SourceGroup, 8);
    DECLARE @ModuleId int = ISNULL(@SourceModule, 32);

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

    -- مصطفى:EmpId 27338 / login mostafa / الاسم؛ fallback للمعرّف 354
    DECLARE @MostafaUserId smallint;
    SELECT TOP (1) @MostafaUserId = CONVERT(smallint, Id)
    FROM dbo.PR_User
    WHERE Active = 1
      AND (EmpId = 27338 OR LoginName = N'mostafa' OR FullName = N'مصطفى احمد محمد طلبه')
    ORDER BY CASE WHEN EmpId = 27338 THEN 0 WHEN LoginName = N'mostafa' THEN 1 ELSE 2 END;

    IF @MostafaUserId IS NULL SET @MostafaUserId = 354;

    IF EXISTS
    (
        SELECT 1 FROM dbo.PR_GroupModuleMenuPrivilage
        WHERE PR_GroupId = @GroupId AND PR_ModuleId = @ModuleId
          AND PR_MenuId = @MenuId AND PR_User_id = @MostafaUserId
    )
        UPDATE dbo.PR_GroupModuleMenuPrivilage
        SET CanView = 1, CanAdd = 1, CanEdit = 1, CanDelete = 0, CanPrint = 1, IS_Active = 1
        WHERE PR_GroupId = @GroupId AND PR_ModuleId = @ModuleId
          AND PR_MenuId = @MenuId AND PR_User_id = @MostafaUserId;
    ELSE
        INSERT dbo.PR_GroupModuleMenuPrivilage
            (Id, PR_GroupId, PR_ModuleId, PR_MenuId,
             CanView, CanAdd, CanEdit, CanDelete, CanPrint, PR_User_id, IS_Active)
        VALUES
            (NEXT VALUE FOR dbo.PR_GroupModuleMenuPrivilage_SEQ,
             @GroupId, @ModuleId, @MenuId, 1, 1, 1, 0, 1, @MostafaUserId, 1);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

-- تحقق
SELECT Id, MenuTitle, MenuTitle_En, MenuURL, isEnableAndroid, Group_Id, PR_ModuleId
FROM dbo.PR_Menu
WHERE LOWER(LTRIM(RTRIM(MenuURL))) = N'committees/imcommittee_inspectionresult/index';

SELECT p.PR_User_id, u.FullName, p.PR_GroupId, p.PR_ModuleId, p.PR_MenuId,
       p.CanView, p.CanAdd, p.CanEdit, p.CanDelete, p.CanPrint, p.IS_Active
FROM dbo.PR_GroupModuleMenuPrivilage p
LEFT JOIN dbo.PR_User u ON u.Id = p.PR_User_id
WHERE p.PR_MenuId IN (SELECT Id FROM dbo.PR_Menu WHERE MenuURL = N'Committees/ImCommittee_InspectionResult/Index');
GO
