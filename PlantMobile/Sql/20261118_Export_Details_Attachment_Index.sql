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
