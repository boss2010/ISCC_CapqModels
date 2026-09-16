namespace PlantMobile.Infrastructure;

/// <summary>Filter authorized committees before loading company and workflow data.</summary>
internal static class ImportRequestsSql
{
    public static string Build(bool committeePage) => ImportCommitteeEligibility.Filter(Selection) + "\n" + WithdrawalVisibility + "\n" +
        (committeePage ? "AND request.ID = @CheckRequestId\n" : DateFilter + "\n") +
        PrepareSelection + "\n" + WorkflowData + "\n" + ImportCommitteeEligibility.Filter(RequestCompletion) + "\n" +
        Companies + "\n" + Details + "\n" + Result +
        "\nDROP TABLE #Samples, #Attachments, #Results, #SampleConfirmations, #ResultConfirmations, #Members, #RequestCompletion;";

    private const string RequestCompletion = """
        SELECT shipping.Im_CheckRequest_ID AS CheckRequestId,
               CAST(MIN(CASE WHEN latestStatus.Is_Continue = 0 THEN 1 ELSE 0 END) AS bit) AS RequestQuarantineComplete
        INTO #RequestCompletion
        FROM dbo.Im_CheckRequest_Items_Lot_Category lot
        JOIN dbo.Im_CheckRequest_Items item ON item.ID = lot.Im_CheckRequest_Items_ID AND item.User_Deletion_Id IS NULL
        JOIN dbo.Im_CheckRequset_Shipping_Method shipping ON shipping.ID = item.Im_CheckRequset_Shipping_Method_ID AND shipping.User_Deletion_Id IS NULL
        OUTER APPLY (
            SELECT TOP (1) status.Is_Continue, lr.ID
            FROM dbo.Im_CheckRequest_Items_Lot_Result lr
            JOIN dbo.Im_CheckRequest_Lot_Result_Status status ON status.ID = lr.IS_Status
            WHERE lr.Im_CheckRequest_Items_Lot_Category_ID = lot.ID AND lr.IS_Status_Committee = 1
            ORDER BY lr.ID DESC
        ) latestStatus
        WHERE lot.User_Deletion_Id IS NULL
          AND EXISTS (SELECT 1 FROM #SelectedCommittees selected WHERE selected.CheckRequestId = shipping.Im_CheckRequest_ID)
          AND (latestStatus.ID IS NOT NULL OR EXISTS (
              SELECT 1 FROM dbo.Im_CommitteeResult inspection
              JOIN dbo.Im_RequestCommittee assigned ON assigned.ID = inspection.Committee_ID
              WHERE assigned.ImCheckRequest_ID = shipping.Im_CheckRequest_ID
                AND assigned.CommitteeType_ID = 11 AND assigned.User_Deletion_Id IS NULL
                AND inspection.User_Deletion_Id IS NULL
                AND (inspection.LotData_ID = lot.ID OR (inspection.LotData_ID IS NULL AND inspection.Im_Request_Item_Id = item.ID))))
        GROUP BY shipping.Im_CheckRequest_ID;
        """;

    private const string Selection = """
        SET NOCOUNT ON;
        SELECT DISTINCT request.ID AS CheckRequestId, request.CheckRequest_Number,
               committee.ID AS CommitteeId, committee.CommitteeType_ID,
               type.Name_Ar AS CommitteeTypeName, operation.Name_Ar AS OperationTypeName,
               committee.Status, committee.IsFinishedAll, committee.Is_Start_Android,
               committee.Is_Cancel, committee.User_Updation_Date, employee.ISAdmin
        INTO #SelectedCommittees
        FROM dbo.Im_CheckRequest request
            INNER JOIN dbo.Im_RequestCommittee committee ON committee.ImCheckRequest_ID = request.ID
            INNER JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID = committee.ID
                AND employee.Employee_Id = @UserId
                AND employee.OperationType = 74
                AND employee.User_Deletion_Id IS NULL
            INNER JOIN dbo.CommitteeType type ON type.ID = committee.CommitteeType_ID
            LEFT JOIN dbo.Im_OpertaionType operation ON operation.ID = request.Im_OperationType
            WHERE request.IsActive = 1
              AND committee.IsApproved = 1
              AND committee.User_Deletion_Id IS NULL
              AND committee.CommitteeType_ID IN (11, 13, 14)
              AND NOT (committee.CommitteeType_ID = 13
                AND EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData completedSample
                            WHERE completedSample.Im_RequestCommittee_ID = committee.ID
                              AND completedSample.User_Deletion_Id IS NULL
                              AND completedSample.Sample_BarCode IS NOT NULL)
                AND NOT EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData incompleteSample
                                WHERE incompleteSample.Im_RequestCommittee_ID = committee.ID
                                  AND incompleteSample.User_Deletion_Id IS NULL
                                  AND incompleteSample.Sample_BarCode IS NOT NULL
                                  AND (incompleteSample.IsAccepted IS NULL OR NOT EXISTS
                                      (SELECT 1 FROM dbo.A_AttachmentData resultAttachment
                                       WHERE resultAttachment.RowId = incompleteSample.ID
                                         AND resultAttachment.A_AttachmentTableNameId = 12
                                         AND resultAttachment.User_Deletion_Id IS NULL))))
        
        """;
    private const string DateFilter = """
              AND (committee.CommitteeType_ID = 13 OR committee.Delegation_Date = @SelectedDate)
        """;

    // Apply the withdrawal rule to both the request list and its committees.
    // A preassigned barcode alone does not mean that withdrawal work has started.
    private const string WithdrawalVisibility = """
              AND
              (
                committee.CommitteeType_ID <> 13
                OR committee.Delegation_Date = @SelectedDate
                OR (committee.Delegation_Date < @SelectedDate
                    AND committee.Is_Cancel IS NULL
                    AND (committee.Is_Start_Android = 1 OR committee.Status = 1 OR committee.IsFinishedAll = 1
                         OR EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData startedSample
                                    WHERE startedSample.Im_RequestCommittee_ID = committee.ID
                                      AND startedSample.User_Deletion_Id IS NULL
                                      AND startedSample.WithdrawDate IS NOT NULL
                                      AND startedSample.SampleSize > 0))
                    AND EXISTS
                    (SELECT 1 FROM dbo.Im_CheckRequest_SampleData pendingSample
                     WHERE pendingSample.Im_RequestCommittee_ID = committee.ID
                       AND pendingSample.User_Deletion_Id IS NULL
                       AND pendingSample.Sample_BarCode IS NOT NULL
                       AND (pendingSample.IsAccepted IS NULL OR NOT EXISTS
                                  (SELECT 1 FROM dbo.A_AttachmentData pendingAttachment
                                   WHERE pendingAttachment.RowId = pendingSample.ID
                                     AND pendingAttachment.A_AttachmentTableNameId = 12
                                     AND pendingAttachment.User_Deletion_Id IS NULL))))
              )
        """;
    private const string PrepareSelection = """
        OPTION (RECOMPILE);
        CREATE CLUSTERED INDEX IX_SelectedCommittees ON #SelectedCommittees (CommitteeId);
        """;

    // Materialize only this user's selected committees. Repeated status checks then
    // use small indexed tables instead of revisiting the full operational tables.
    private const string WorkflowData = """
        SELECT s.ID, s.Im_RequestCommittee_ID, s.User_Deletion_Id,
               s.Sample_BarCode, s.IsAccepted, s.SampleSize, s.WithdrawDate
        INTO #Samples
        FROM dbo.Im_CheckRequest_SampleData s
        WHERE s.User_Deletion_Id IS NULL
          AND EXISTS (SELECT 1 FROM #SelectedCommittees c WHERE c.CommitteeId = s.Im_RequestCommittee_ID)
        OPTION (RECOMPILE);
        CREATE UNIQUE CLUSTERED INDEX IX_Samples ON #Samples (ID);
        CREATE INDEX IX_Samples_Committee ON #Samples (Im_RequestCommittee_ID);

        SELECT a.RowId, a.A_AttachmentTableNameId, a.User_Deletion_Id
        INTO #Attachments
        FROM dbo.A_AttachmentData a
        WHERE a.A_AttachmentTableNameId = 12 AND a.User_Deletion_Id IS NULL
          AND EXISTS (SELECT 1 FROM #Samples s WHERE s.ID = a.RowId)
        OPTION (RECOMPILE);
        CREATE CLUSTERED INDEX IX_Attachments ON #Attachments (RowId);

        SELECT r.ID, r.Committee_ID, r.CommitteeResultType_ID, r.User_Deletion_Id
        INTO #Results
        FROM dbo.Im_CommitteeResult r
        WHERE EXISTS (SELECT 1 FROM #SelectedCommittees c WHERE c.CommitteeId = r.Committee_ID)
        OPTION (RECOMPILE);
        CREATE UNIQUE CLUSTERED INDEX IX_Results ON #Results (ID);
        CREATE INDEX IX_Results_Committee ON #Results (Committee_ID);

        SELECT c.Im_CheckRequest_SampleData_ID, c.EmployeeId, c.IsAccepted
        INTO #SampleConfirmations
        FROM dbo.Im_CheckRequest_SampleData_Confirm c
        WHERE EXISTS (SELECT 1 FROM #Samples s WHERE s.ID = c.Im_CheckRequest_SampleData_ID)
        OPTION (RECOMPILE);
        CREATE CLUSTERED INDEX IX_SampleConfirmations ON #SampleConfirmations (Im_CheckRequest_SampleData_ID, EmployeeId);

        SELECT c.Im_CommitteeResult_ID, c.EmployeeId
        INTO #ResultConfirmations
        FROM dbo.Im_CommitteeResult_Confirm c
        WHERE c.EmployeeId = @UserId
          AND EXISTS (SELECT 1 FROM #Results r WHERE r.ID = c.Im_CommitteeResult_ID)
        OPTION (RECOMPILE);
        CREATE CLUSTERED INDEX IX_ResultConfirmations ON #ResultConfirmations (Im_CommitteeResult_ID);

        SELECT m.Committee_ID, m.Employee_Id, m.ISAdmin, m.OperationType, m.User_Deletion_Id
        INTO #Members
        FROM dbo.CommitteeEmployee m
        WHERE m.OperationType = 74 AND m.User_Deletion_Id IS NULL
          AND EXISTS (SELECT 1 FROM #SelectedCommittees c WHERE c.CommitteeId = m.Committee_ID)
        OPTION (RECOMPILE);
        CREATE CLUSTERED INDEX IX_Members ON #Members (Committee_ID);
        """;

    private const string Companies = """
        SELECT request.CheckRequestId,
               COALESCE(NULLIF(permissionImporter.ImporterName COLLATE Arabic_CI_AS, N''),
                        NULLIF(importer.DelegateName COLLATE Arabic_CI_AS, N''),
                        N'غير محدد') COLLATE Arabic_CI_AS AS CompanyName
        INTO #RequestCompanies
        FROM (SELECT DISTINCT CheckRequestId FROM #SelectedCommittees) request
            OUTER APPLY
            (
                SELECT TOP (1) permission.ImporterName
                FROM dbo.View_List_Im_PermissionRequest permission
                WHERE permission.Im_CheckRequest_ID = request.CheckRequestId
                ORDER BY permission.Im_PermissionRequest_ID DESC
            ) permissionImporter
            OUTER APPLY
            (
                SELECT TOP (1) requestData.DelegateName
                FROM dbo.Im_CheckRequest_Data requestData
                WHERE requestData.Im_CheckRequest_ID = request.CheckRequestId
                  AND requestData.User_Deletion_Id IS NULL
                ORDER BY requestData.ID DESC
            ) importer
        
        OPTION (RECOMPILE);
        CREATE UNIQUE CLUSTERED INDEX IX_RequestCompanies ON #RequestCompanies (CheckRequestId);
        
        SELECT ROW_NUMBER() OVER (ORDER BY committee.CommitteeId) AS RowNum,
               committee.*, company.CompanyName, ISNULL(completion.RequestQuarantineComplete, CAST(0 AS bit)) AS RequestQuarantineComplete,
        """;
    private const string Details = """
        (SELECT STRING_AGG(CONVERT(nvarchar(max), LTRIM(RTRIM(sample.Sample_BarCode))), '|')
                        WITHIN GROUP (ORDER BY sample.ID)
                 FROM #Samples sample
                 WHERE sample.Im_RequestCommittee_ID = committee.CommitteeId
                   AND sample.User_Deletion_Id IS NULL
                   AND NULLIF(LTRIM(RTRIM(sample.Sample_BarCode)), '') IS NOT NULL) AS SampleBarcodes,
                (SELECT STRING_AGG(CONCAT(COALESCE(NULLIF(privilegeUser.FullName, ''), privilegeUser.LoginName, CONVERT(nvarchar(20), member.Employee_Id)), ' (', CASE WHEN member.ISAdmin = 1 THEN N'أدمن' ELSE N'مساعد' END, ')'), '|')
                 FROM #Members member
                 LEFT JOIN dbPrivilage.dbo.PR_User privilegeUser ON privilegeUser.Id = member.Employee_Id
                 WHERE member.Committee_ID = committee.CommitteeId AND member.OperationType = 74 AND member.User_Deletion_Id IS NULL) AS CommitteeMembers,
                (SELECT COUNT(*) FROM #Results r WHERE r.Committee_ID = committee.CommitteeId AND r.CommitteeResultType_ID IS NOT NULL) AS ResultCount,
                (SELECT COUNT(*) FROM #Results r INNER JOIN #ResultConfirmations c ON c.Im_CommitteeResult_ID = r.ID AND c.EmployeeId = @UserId WHERE r.Committee_ID = committee.CommitteeId) AS ConfirmCount
                ,(SELECT COUNT(*) FROM #Samples s WHERE s.Im_RequestCommittee_ID = committee.CommitteeId AND s.Sample_BarCode IS NOT NULL AND s.User_Deletion_Id IS NULL) AS SampleCount
                ,(SELECT COUNT(*) FROM #Samples s INNER JOIN #SampleConfirmations c ON c.Im_CheckRequest_SampleData_ID = s.ID AND c.EmployeeId = @UserId WHERE s.Im_RequestCommittee_ID = committee.CommitteeId AND s.Sample_BarCode IS NOT NULL AND s.User_Deletion_Id IS NULL) AS SampleConfirmCount
                ,CAST(CASE WHEN EXISTS
                  (SELECT 1 FROM #Samples sample
                   INNER JOIN #Attachments attachment ON attachment.RowId = sample.ID
                     AND attachment.A_AttachmentTableNameId = 12
                     AND attachment.User_Deletion_Id IS NULL
                   WHERE sample.Im_RequestCommittee_ID = committee.CommitteeId
                     AND sample.User_Deletion_Id IS NULL)
                 THEN 1 ELSE 0 END AS bit) AS SampleAttachmentUploaded
                ,CAST(CASE WHEN EXISTS
                  (SELECT 1 FROM #Samples sample
                   WHERE sample.Im_RequestCommittee_ID = committee.CommitteeId
                     AND sample.User_Deletion_Id IS NULL
                     AND sample.Sample_BarCode IS NOT NULL
                     AND (sample.IsAccepted IS NULL OR NOT EXISTS
                       (SELECT 1 FROM #Attachments attachment
                        WHERE attachment.RowId = sample.ID
                          AND attachment.A_AttachmentTableNameId = 12
                          AND attachment.User_Deletion_Id IS NULL)))
                 THEN 1 ELSE 0 END AS bit) AS HasPendingSampleFinalResult
                ,CAST(CASE WHEN EXISTS
                  (SELECT 1 FROM #Samples labSample
                   WHERE labSample.Im_RequestCommittee_ID = committee.CommitteeId
                     AND committee.Is_Cancel IS NULL
                     AND labSample.WithdrawDate IS NOT NULL AND labSample.SampleSize > 0
                     AND labSample.Sample_BarCode IS NOT NULL
                     AND labSample.User_Deletion_Id IS NULL
                     AND (labSample.IsAccepted IS NULL OR NOT EXISTS
                       (SELECT 1 FROM #Attachments labAttachment
                        WHERE labAttachment.RowId = labSample.ID
                          AND labAttachment.A_AttachmentTableNameId = 12
                          AND labAttachment.User_Deletion_Id IS NULL)))
                 THEN 1 ELSE 0 END AS bit) AS HasPendingLabResult
                ,CAST(CASE WHEN EXISTS (SELECT 1 FROM #Results r WHERE r.Committee_ID=committee.CommitteeId AND r.User_Deletion_Id IS NULL)
                  AND NOT EXISTS (SELECT 1 FROM #Results r WHERE r.Committee_ID=committee.CommitteeId AND r.User_Deletion_Id IS NULL AND r.CommitteeResultType_ID IS NULL)
                  THEN 1 ELSE 0 END AS bit) AS InspectionComplete
                ,CAST(CASE WHEN EXISTS (SELECT 1 FROM #Samples s WHERE s.Im_RequestCommittee_ID=committee.CommitteeId AND s.User_Deletion_Id IS NULL)
                  AND NOT EXISTS (SELECT 1 FROM #Samples s WHERE s.Im_RequestCommittee_ID=committee.CommitteeId AND s.User_Deletion_Id IS NULL AND (s.SampleSize IS NULL OR s.SampleSize<=0))
                  THEN 1 ELSE 0 END AS bit) AS WithdrawalComplete
                ,CAST(CASE WHEN committee.IsFinishedAll=1 AND committee.Is_Cancel IS NULL
                  AND EXISTS (SELECT 1 FROM #Samples sample
                              WHERE sample.Im_RequestCommittee_ID=committee.CommitteeId AND sample.User_Deletion_Id IS NULL)
                  AND NOT EXISTS (SELECT 1 FROM #Samples sample
                                  WHERE sample.Im_RequestCommittee_ID=committee.CommitteeId AND sample.User_Deletion_Id IS NULL
                                    AND (sample.WithdrawDate IS NULL OR ISNULL(sample.SampleSize,0)<=0))
                  AND NOT EXISTS (
                      SELECT 1 FROM #Members assistant
                      CROSS JOIN #Samples sample
                      WHERE assistant.Committee_ID=committee.CommitteeId AND assistant.ISAdmin=0
                        AND assistant.OperationType=74 AND assistant.User_Deletion_Id IS NULL
                        AND sample.Im_RequestCommittee_ID=committee.CommitteeId AND sample.User_Deletion_Id IS NULL
                        AND NOT EXISTS (SELECT 1 FROM #SampleConfirmations opinion
                                        WHERE opinion.Im_CheckRequest_SampleData_ID=sample.ID
                                          AND opinion.EmployeeId=assistant.Employee_Id AND opinion.IsAccepted IS NOT NULL))
                  THEN 1 ELSE 0 END AS bit) AS CanShowLaboratoryResult
                ,(SELECT STRING_AGG(CONVERT(nvarchar(max), LTRIM(RTRIM(pending.Sample_BarCode))), '|')
                  FROM #Samples pending
                  WHERE pending.Im_RequestCommittee_ID=committee.CommitteeId AND pending.User_Deletion_Id IS NULL
                    AND (pending.IsAccepted IS NULL OR NOT EXISTS
                        (SELECT 1 FROM #Attachments a WHERE a.RowId=pending.ID
                         AND a.A_AttachmentTableNameId=12 AND a.User_Deletion_Id IS NULL))) AS PendingLabBarcodes
        """;
    private const string ListSummary = """
        CAST(NULL AS nvarchar(max)) AS SampleBarcodes,
        CAST(NULL AS nvarchar(max)) AS CommitteeMembers,
        0 AS ResultCount, 0 AS ConfirmCount, 0 AS SampleCount, 0 AS SampleConfirmCount,
        CAST(0 AS bit) AS SampleAttachmentUploaded,
        CAST(0 AS bit) AS HasPendingSampleFinalResult,
        CAST(0 AS bit) AS HasPendingLabResult, CAST(0 AS bit) AS InspectionComplete, CAST(0 AS bit) AS WithdrawalComplete, CAST(0 AS bit) AS CanShowLaboratoryResult, CAST(NULL AS nvarchar(max)) AS PendingLabBarcodes
        """;
    private const string Result = """
        FROM #SelectedCommittees committee
        JOIN #RequestCompanies company ON company.CheckRequestId = committee.CheckRequestId
        LEFT JOIN #RequestCompletion completion ON completion.CheckRequestId = committee.CheckRequestId
        ORDER BY committee.CommitteeId
        OPTION (RECOMPILE);
        DROP TABLE #RequestCompanies;
        DROP TABLE #SelectedCommittees;
        """;
}
