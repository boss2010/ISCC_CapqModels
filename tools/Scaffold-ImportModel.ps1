param([switch]$Refresh)
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path $PSScriptRoot -Parent)
$tables = @('Im_CheckRequest','Im_RequestCommittee','CommitteeEmployee','CommitteeType','Im_OpertaionType','Im_CheckRequest_Data','Im_CheckRequest_Items','Im_CheckRequest_Items_Lot_Category','Im_CheckRequset_Shipping_Method','Im_CommitteeResult','Im_CheckRequest_SampleData','Im_CheckRequest_SampleData_Confirm','Im_CommitteeResult_Confirm','Im_CheckRequest_Items_Lot_Result','Im_CheckRequest_Lot_Result_Status','Im_Request_TreatmentData','A_AttachmentData','View_List_Im_PermissionRequest','AnalysisLabType','AnalysisLab','AnalysisType','Item','Item_ShortName','Package_Type','Country','Outlet','CommitteeResultType','Group','Family','Im_CommitteeResult_Infection','Fees_Action')
$scaffoldArgs = @('dbcontext','scaffold','Name=ConnectionStrings:DBConnection','Microsoft.EntityFrameworkCore.SqlServer','--project','EF','--startup-project','PlantMobile','--no-build','--output-dir','Imports/Entities','--context-dir','Imports','--context','ImportDbContext','--namespace','EF.Imports.Entities','--context-namespace','EF.Imports','--no-onconfiguring','--use-database-names','--no-pluralize')
foreach ($table in $tables) { $scaffoldArgs += @('--table', "dbo.$table") }
if ($Refresh) { $scaffoldArgs += '--force' }
& ./.tools/dotnet-ef @scaffoldArgs
if ($LASTEXITCODE -ne 0) { throw 'Import scaffold failed.' }
