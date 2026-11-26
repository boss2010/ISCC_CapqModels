SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.Ex_CheckRequest_Genshi_Transfer',N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Ex_CheckRequest_Genshi_Transfer
    (
        ID bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_Ex_CheckRequest_Genshi_Transfer PRIMARY KEY,
        Ex_CheckRequest_ID bigint NOT NULL,
        Lot_ID bigint NULL,
        Transfer_Date datetime2(0) NOT NULL CONSTRAINT DF_Ex_CheckRequest_Genshi_Transfer_Date DEFAULT(SYSDATETIME()),
        User_Creation_Id smallint NULL,
        Notes nvarchar(500) NULL,
        IsActive bit NOT NULL CONSTRAINT DF_Ex_CheckRequest_Genshi_Transfer_Active DEFAULT(1),
        CONSTRAINT FK_Ex_CheckRequest_Genshi_Transfer_Request FOREIGN KEY(Ex_CheckRequest_ID) REFERENCES dbo.Ex_CheckRequest(ID)
    );
END;

IF COL_LENGTH(N'dbo.Ex_CheckRequest_Genshi_Transfer',N'Lot_ID') IS NULL
    ALTER TABLE dbo.Ex_CheckRequest_Genshi_Transfer ADD Lot_ID bigint NULL;

IF EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Ex_CheckRequest_Genshi_Transfer') AND name=N'UX_Ex_CheckRequest_Genshi_Transfer_Active')
    DROP INDEX UX_Ex_CheckRequest_Genshi_Transfer_Active ON dbo.Ex_CheckRequest_Genshi_Transfer;

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Ex_CheckRequest_Genshi_Transfer') AND name=N'UX_Ex_CheckRequest_Genshi_Transfer_Lot_Active')
    EXEC(N'CREATE UNIQUE INDEX UX_Ex_CheckRequest_Genshi_Transfer_Lot_Active ON dbo.Ex_CheckRequest_Genshi_Transfer(Ex_CheckRequest_ID,Lot_ID) WHERE IsActive=1 AND Lot_ID IS NOT NULL;');

COMMIT;
