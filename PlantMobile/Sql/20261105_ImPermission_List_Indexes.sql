/*
    Optional performance indexes for:
    /Im_Permissions/ListIm_Permission/Index

    Safe to run more than once. It creates only missing indexes and does not
    change or delete application data.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Im_PermissionRequest', N'U') IS NULL
    THROW 50001, 'Table dbo.Im_PermissionRequest was not found in the selected database.', 1;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Im_PermissionRequest')
      AND name = N'IX_Im_PermissionRequest_ArrivalDate_List'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Im_PermissionRequest_ArrivalDate_List
        ON dbo.Im_PermissionRequest (User_Deletion_Id, Arrival_Date DESC, ID DESC)
        INCLUDE
        (
            ImPermission_Number, IsAcceppted, IsPaid, IS_Print_Ar, IS_Print_EN,
            Start_Date, End_Date, Renewal_Status
        );
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Im_RequestData')
      AND name = N'IX_Im_RequestData_Permission_List'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Im_RequestData_Permission_List
        ON dbo.Im_RequestData (Im_PermissionRequest_ID)
        INCLUDE (ID, Im_OperationType, ExportCountry_Id, Importer_ID, ImporterType_Id);
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Im_PermissionItems')
      AND name = N'IX_Im_PermissionItems_Permission_List'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Im_PermissionItems_Permission_List
        ON dbo.Im_PermissionItems (Im_PermissionRequest_ID, ID)
        INCLUDE (Im_Initiator_ID);
END;

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Im_Initiator')
      AND name = N'IX_Im_Initiator_Permission_Filter'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Im_Initiator_Permission_Filter
        ON dbo.Im_Initiator (ID)
        INCLUDE (Item_ShortName_ID, QualitativeGroup_Id);
END;

SELECT
    i.name AS IndexName,
    OBJECT_NAME(i.object_id) AS TableName
FROM sys.indexes AS i
WHERE i.name IN
(
    N'IX_Im_PermissionRequest_ArrivalDate_List',
    N'IX_Im_RequestData_Permission_List',
    N'IX_Im_PermissionItems_Permission_List',
    N'IX_Im_Initiator_Permission_Filter'
)
ORDER BY TableName, IndexName;
