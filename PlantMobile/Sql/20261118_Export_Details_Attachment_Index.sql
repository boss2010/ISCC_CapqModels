USE [PlantQuarantine_New];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.A_AttachmentData')
      AND name = N'IX_A_AttachmentData_Entity_Active'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_A_AttachmentData_Entity_Active
        ON dbo.A_AttachmentData
        (
            RowId,
            A_AttachmentTableNameId,
            User_Deletion_Id,
            Id DESC
        )
        INCLUDE
        (
            Attachment_TypeName,
            AttachmentPath,
            Attachment_Number
        );
END;
GO

/* فهارس شاشة تفاصيل طلب الفحص الصادر */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Ex_CheckRequest_Data') AND name=N'IX_Ex_CheckRequest_Data_Request_Active')
    CREATE NONCLUSTERED INDEX IX_Ex_CheckRequest_Data_Request_Active
        ON dbo.Ex_CheckRequest_Data(Ex_CheckRequest_ID,User_Deletion_Id,ID DESC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Ex_CheckRequest_Items') AND name=N'IX_Ex_CheckRequest_Items_Request_Active')
    CREATE NONCLUSTERED INDEX IX_Ex_CheckRequest_Items_Request_Active
        ON dbo.Ex_CheckRequest_Items(Ex_CheckRequest_ID,User_Deletion_Id,ID);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Ex_CheckRequest_Items_Lot_Category') AND name=N'IX_Ex_Request_Lots_Item_Active')
    CREATE NONCLUSTERED INDEX IX_Ex_Request_Lots_Item_Active
        ON dbo.Ex_CheckRequest_Items_Lot_Category(Ex_CheckRequest_Items_ID,User_Deletion_Id,ID);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Ex_CheckRequest_Places') AND name=N'IX_Ex_CheckRequest_Places_Request')
    CREATE NONCLUSTERED INDEX IX_Ex_CheckRequest_Places_Request
        ON dbo.Ex_CheckRequest_Places(Ex_CheckRequest_ID);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Ex_RequestCommittee') AND name=N'IX_Ex_RequestCommittee_Request_Active')
    CREATE NONCLUSTERED INDEX IX_Ex_RequestCommittee_Request_Active
        ON dbo.Ex_RequestCommittee(ExCheckRequest_ID,User_Deletion_Id,CommitteeType_ID,ID DESC);
GO
