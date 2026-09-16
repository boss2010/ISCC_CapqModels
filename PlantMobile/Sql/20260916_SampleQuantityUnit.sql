-- Existing sample amounts are kilograms; count amounts must retain their unit.
IF COL_LENGTH('dbo.Im_CheckRequest_SampleData', 'SampleQuantityUnit') IS NULL
    ALTER TABLE dbo.Im_CheckRequest_SampleData ADD SampleQuantityUnit nvarchar(10) NULL;
