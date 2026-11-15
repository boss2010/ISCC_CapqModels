/*
    السكربت النهائي لشاشة: تعديل تاريخ شحن طلب وارد

    قاعدة البيانات: dbPrivilage

    يقوم السكربت بالآتي:
      1- إضافة الشاشة إلى جدول القوائم إن لم تكن موجودة.
      2- تحديث الاسم العربي والمسار والإعدادات إذا كانت موجودة.
      3- ربط الشاشة بمجموعة الوارد وقسم طلبات الفحص الحالية - وارد.

    ملاحظات مهمة:
      - السكربت قابل لإعادة التنفيذ دون تكرار السجلات.
      - لا يمنح الصلاحية لأي موظف بصورة تلقائية.
      - منح صلاحية العرض أو التعديل يتم من شاشة صلاحيات المستخدمين.
      - لا توجد تعديلات مطلوبة على جداول PlantQuarantine_New؛
        الشاشة تستخدم الحقل الموجود dbo.Im_CheckRequest_Customs_Message.Shipment_Date.
      - الملف مكتوب بترميز UTF-8 للحفاظ على الاسم العربي.
*/

USE [dbPrivilage];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @رقم_الشاشة int;
    DECLARE @مسار_الشاشة nvarchar(500) = N'Im_CheckRequests/ShippingDate/Index';

    SELECT TOP (1) @رقم_الشاشة = Id
    FROM dbo.PR_Menu
    WHERE LOWER(REPLACE(REPLACE(LTRIM(RTRIM(MenuURL)), N'~', N''), N'\', N'/'))
        = LOWER(@مسار_الشاشة)
    ORDER BY Id;

    IF @رقم_الشاشة IS NULL
    BEGIN
        SELECT @رقم_الشاشة = ISNULL(MAX(Id), 0) + 1
        FROM dbo.PR_Menu WITH (UPDLOCK, HOLDLOCK);

        INSERT dbo.PR_Menu
        (
            Id, MenuTitle, MenuTitle_En, MenuURL, PR_MenuId, Active,
            PR_ModuleId, PR_ApplicationId, PR_ApplicationCategoryId,
            Group_Id, isEnableAndroid
        )
        VALUES
        (
            @رقم_الشاشة,
            N'تعديل تاريخ شحن طلب وارد',
            N'Edit Import Request Shipping Date',
            @مسار_الشاشة,
            NULL, 1, 22, 1, 1, 1, 1
        );
    END
    ELSE
    BEGIN
        UPDATE dbo.PR_Menu
        SET MenuTitle = N'تعديل تاريخ شحن طلب وارد',
            MenuTitle_En = N'Edit Import Request Shipping Date',
            MenuURL = @مسار_الشاشة,
            Active = 1,
            PR_ModuleId = 22,
            PR_ApplicationId = 1,
            PR_ApplicationCategoryId = 1,
            Group_Id = 1,
            isEnableAndroid = 1
        WHERE Id = @رقم_الشاشة;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.PR_GroupModuleMenu
        WHERE PR_GroupId = 8
          AND PR_ModuleId = 33
          AND PR_MenuId = @رقم_الشاشة
    )
    BEGIN
        UPDATE dbo.PR_GroupModuleMenu
        SET IS_Active = 1,
            Order_BY = 90
        WHERE PR_GroupId = 8
          AND PR_ModuleId = 33
          AND PR_MenuId = @رقم_الشاشة;
    END
    ELSE
    BEGIN
        INSERT dbo.PR_GroupModuleMenu
            (PR_GroupId, PR_ModuleId, PR_MenuId, IS_Active, Order_BY)
        VALUES
            (8, 33, @رقم_الشاشة, 1, 90);
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO

/* عرض نتيجة التنفيذ للتأكد من ظهور الاسم العربي. */
SELECT menu.Id AS [رقم الشاشة],
       menu.MenuTitle AS [اسم الشاشة],
       menu.MenuURL AS [مسار الشاشة],
       groupMenu.PR_GroupId AS [رقم مجموعة الوارد],
       groupMenu.PR_ModuleId AS [رقم قسم طلبات الفحص],
       groupMenu.Order_BY AS [ترتيب الشاشة],
       menu.Active AS [الشاشة مفعلة],
       menu.isEnableAndroid AS [تظهر في النظام الجديد]
FROM dbo.PR_Menu menu
INNER JOIN dbo.PR_GroupModuleMenu groupMenu
    ON groupMenu.PR_MenuId = menu.Id
WHERE LOWER(REPLACE(REPLACE(LTRIM(RTRIM(menu.MenuURL)), N'~', N''), N'\', N'/'))
    = LOWER(N'Im_CheckRequests/ShippingDate/Index');
GO
