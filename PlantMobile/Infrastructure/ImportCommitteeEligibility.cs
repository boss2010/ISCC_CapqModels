namespace PlantMobile.Infrastructure;

internal static class ImportCommitteeEligibility
{
    // Status and preallocated barcodes are not evidence of actual work.
    // Keep undated/future/today committees; disregard only past, untouched ones.
    internal const string EligibleTable = """
        (SELECT eligible.* FROM dbo.Im_RequestCommittee eligible
         WHERE eligible.Delegation_Date IS NULL
            OR eligible.Delegation_Date >= CONVERT(date, GETDATE())
            OR eligible.Is_Start_Android = 1 OR eligible.IsFinishedAll = 1
            OR EXISTS (SELECT 1 FROM dbo.Im_CommitteeResult workedResult
                       WHERE workedResult.Committee_ID = eligible.ID
                         AND workedResult.User_Deletion_Id IS NULL
                         AND workedResult.CommitteeResultType_ID IS NOT NULL)
            OR EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData workedSample
                       WHERE workedSample.Im_RequestCommittee_ID = eligible.ID
                         AND workedSample.User_Deletion_Id IS NULL
                         AND workedSample.WithdrawDate IS NOT NULL AND workedSample.SampleSize > 0)
            OR EXISTS (SELECT 1 FROM dbo.Im_Request_TreatmentData workedTreatment
                       WHERE workedTreatment.Im_RequestCommittee_ID = eligible.ID
                         AND workedTreatment.User_Deletion_Id IS NULL
                         AND workedTreatment.User_Updation_Date IS NOT NULL))
        """;

    // Used only for read queries with explicitly aliased committee tables.
    internal static string Filter(string sql) => sql.Replace("dbo.Im_RequestCommittee ", EligibleTable + " ", StringComparison.Ordinal);
}
