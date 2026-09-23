/* Run in the SAME database used by the server's DBConnection, before publishing.
   Database-first EF does not require creating/replacing tables or changing data.
   The unit column is the existing prerequisite; remaining changes are indexes.
   Re-runnable. No DROP, data conversion, UPDATE, or automatic startup migration.
   Index creation takes locks: run during a quiet deployment window.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET ARITHABORT ON;
SET NUMERIC_ROUNDABORT OFF;
SET LOCK_TIMEOUT 10000;

IF OBJECT_ID(N'dbo.Im_RequestCommittee', N'U') IS NULL
    THROW 51000, 'Wrong database: import committee table is missing.', 1;
IF COL_LENGTH(N'dbo.Im_CheckRequest_Items_Lot_Category', N'PackagesCount') IS NULL
    THROW 51001, 'Required existing PackagesCount column is missing. Verify the target database.', 1;
IF COL_LENGTH(N'dbo.Im_CheckRequest_SampleData', N'SampleQuantityUnit') IS NULL
    ALTER TABLE dbo.Im_CheckRequest_SampleData ADD SampleQuantityUnit nvarchar(10) NULL;

-- Start with the current employee's committees, not every committee in the database.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.CommitteeEmployee') AND name=N'IX_PlantMobile_Member_User')
    CREATE INDEX IX_PlantMobile_Member_User ON dbo.CommitteeEmployee(Employee_Id, OperationType, Committee_ID)
        INCLUDE(ISAdmin, User_Deletion_Id) WHERE User_Deletion_Id IS NULL;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Im_RequestCommittee') AND name=N'IX_PlantMobile_Committee_Request')
    CREATE INDEX IX_PlantMobile_Committee_Request ON dbo.Im_RequestCommittee(ImCheckRequest_ID, CommitteeType_ID, Delegation_Date)
        INCLUDE(IsApproved, Status, IsFinishedAll, Is_Start_Android, Is_Cancel, User_Updation_Date, User_Deletion_Id)
        WHERE User_Deletion_Id IS NULL;

-- All result rows remain indexed: legacy assistant confirmation counts include them.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Im_CommitteeResult') AND name=N'IX_PlantMobile_Result_Committee')
    CREATE INDEX IX_PlantMobile_Result_Committee ON dbo.Im_CommitteeResult(Committee_ID, ID)
        INCLUDE(LotData_ID, Im_Request_Item_Id, CommitteeResultType_ID, User_Deletion_Id, IS_Total);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Im_CheckRequest_SampleData') AND name=N'IX_PlantMobile_Sample_Committee')
    CREATE INDEX IX_PlantMobile_Sample_Committee ON dbo.Im_CheckRequest_SampleData(Im_RequestCommittee_ID, ID)
        INCLUDE(LotData_ID, Im_Request_Item_Id, Sample_BarCode, SampleSize, WithdrawDate, IsAccepted, User_Deletion_Id)
        WHERE User_Deletion_Id IS NULL;

-- Never put attachment binary contents in a list-screen index.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.A_AttachmentData') AND name=N'IX_PlantMobile_Attachment_Sample')
    CREATE INDEX IX_PlantMobile_Attachment_Sample ON dbo.A_AttachmentData(RowId, Id DESC)
        INCLUDE(A_AttachmentTableNameId, User_Deletion_Id)
        WHERE A_AttachmentTableNameId=12 AND User_Deletion_Id IS NULL;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Im_CheckRequest_SampleData_Confirm') AND name=N'IX_PlantMobile_SampleOpinion_User')
    CREATE INDEX IX_PlantMobile_SampleOpinion_User ON dbo.Im_CheckRequest_SampleData_Confirm(Im_CheckRequest_SampleData_ID, EmployeeId)
        INCLUDE(IsAccepted);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Im_CommitteeResult_Confirm') AND name=N'IX_PlantMobile_ResultOpinion_User')
    CREATE INDEX IX_PlantMobile_ResultOpinion_User ON dbo.Im_CommitteeResult_Confirm(Im_CommitteeResult_ID, EmployeeId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Im_CheckRequest_Items_Lot_Result') AND name=N'IX_PlantMobile_Lot_ActiveStatus')
    CREATE INDEX IX_PlantMobile_Lot_ActiveStatus ON dbo.Im_CheckRequest_Items_Lot_Result(Im_CheckRequest_Items_Lot_Category_ID, ID DESC)
        INCLUDE(IS_Status, IS_Status_Committee) WHERE IS_Status_Committee=1;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Im_CheckRequset_Shipping_Method') AND name=N'IX_PlantMobile_Shipping_Request')
    CREATE INDEX IX_PlantMobile_Shipping_Request ON dbo.Im_CheckRequset_Shipping_Method(Im_CheckRequest_ID, ID)
        INCLUDE(User_Deletion_Id);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Im_CheckRequest_Items') AND name=N'IX_PlantMobile_Item_Shipping')
    CREATE INDEX IX_PlantMobile_Item_Shipping ON dbo.Im_CheckRequest_Items(Im_CheckRequset_Shipping_Method_ID, ID)
        INCLUDE(Item_ShortName_ID, User_Deletion_Id);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Im_CheckRequest_Items_Lot_Category') AND name=N'IX_PlantMobile_Lot_Item')
    CREATE INDEX IX_PlantMobile_Lot_Item ON dbo.Im_CheckRequest_Items_Lot_Category(Im_CheckRequest_Items_ID, ID)
        INCLUDE(User_Deletion_Id) WHERE User_Deletion_Id IS NULL;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Im_CheckRequest_Data') AND name=N'IX_PlantMobile_RequestData_Request')
    CREATE INDEX IX_PlantMobile_RequestData_Request ON dbo.Im_CheckRequest_Data(Im_CheckRequest_ID, ID DESC)
        INCLUDE(DelegateName, User_Deletion_Id) WHERE User_Deletion_Id IS NULL;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Im_Request_TreatmentData') AND name=N'IX_PlantMobile_Treatment_Committee')
    CREATE INDEX IX_PlantMobile_Treatment_Committee ON dbo.Im_Request_TreatmentData(Im_RequestCommittee_ID)
        INCLUDE(User_Updation_Date, User_Deletion_Id) WHERE User_Deletion_Id IS NULL;

SELECT OBJECT_SCHEMA_NAME(object_id) AS SchemaName, OBJECT_NAME(object_id) AS TableName, name AS IndexName
FROM sys.indexes WHERE name LIKE N'IX[_]PlantMobile[_]%' ORDER BY TableName, IndexName;
