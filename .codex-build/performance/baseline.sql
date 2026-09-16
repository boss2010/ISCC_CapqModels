;WITH AssignedCommittees AS
(
    SELECT DISTINCT
        request.ID AS CheckRequestId,
        request.CheckRequest_Number,
        COALESCE(
            NULLIF(permissionImporter.ImporterName COLLATE Arabic_CI_AS, N''),
            NULLIF(importer.DelegateName COLLATE Arabic_CI_AS, N''),
            N'غير محدد') COLLATE Arabic_CI_AS AS CompanyName,
        committee.ID AS CommitteeId,
        committee.CommitteeType_ID,
        type.Name_Ar AS CommitteeTypeName,
        operation.Name_Ar AS OperationTypeName,
        committee.Status,
        committee.IsFinishedAll,
        committee.Is_Start_Android,
        committee.Is_Cancel,
        committee.User_Updation_Date,
        employee.ISAdmin,
        (SELECT STRING_AGG(CONVERT(nvarchar(max), LTRIM(RTRIM(sample.Sample_BarCode))), '|')
                WITHIN GROUP (ORDER BY sample.ID)
         FROM dbo.Im_CheckRequest_SampleData sample
         WHERE sample.Im_RequestCommittee_ID = committee.ID
           AND sample.User_Deletion_Id IS NULL
           AND NULLIF(LTRIM(RTRIM(sample.Sample_BarCode)), '') IS NOT NULL) AS SampleBarcodes,
        (SELECT STRING_AGG(CONCAT(COALESCE(NULLIF(privilegeUser.FullName, ''), privilegeUser.LoginName, CONVERT(nvarchar(20), member.Employee_Id)), ' (', CASE WHEN member.ISAdmin = 1 THEN N'أدمن' ELSE N'مساعد' END, ')'), '|')
         FROM dbo.CommitteeEmployee member
         LEFT JOIN dbPrivilage.dbo.PR_User privilegeUser ON privilegeUser.Id = member.Employee_Id
         WHERE member.Committee_ID = committee.ID AND member.OperationType = 74 AND member.User_Deletion_Id IS NULL) AS CommitteeMembers,
        (SELECT COUNT(*) FROM dbo.Im_CommitteeResult r WHERE r.Committee_ID = committee.ID AND r.CommitteeResultType_ID IS NOT NULL) AS ResultCount,
        (SELECT COUNT(*) FROM dbo.Im_CommitteeResult r INNER JOIN dbo.Im_CommitteeResult_Confirm c ON c.Im_CommitteeResult_ID = r.ID AND c.EmployeeId = @UserId WHERE r.Committee_ID = committee.ID) AS ConfirmCount
        ,(SELECT COUNT(*) FROM dbo.Im_CheckRequest_SampleData s WHERE s.Im_RequestCommittee_ID = committee.ID AND s.Sample_BarCode IS NOT NULL AND s.User_Deletion_Id IS NULL) AS SampleCount
        ,(SELECT COUNT(*) FROM dbo.Im_CheckRequest_SampleData s INNER JOIN dbo.Im_CheckRequest_SampleData_Confirm c ON c.Im_CheckRequest_SampleData_ID = s.ID AND c.EmployeeId = @UserId WHERE s.Im_RequestCommittee_ID = committee.ID AND s.Sample_BarCode IS NOT NULL AND s.User_Deletion_Id IS NULL) AS SampleConfirmCount
        ,CAST(CASE WHEN EXISTS
          (SELECT 1 FROM dbo.Im_CheckRequest_SampleData sample
           INNER JOIN dbo.A_AttachmentData attachment ON attachment.RowId = sample.ID
             AND attachment.A_AttachmentTableNameId = 12
             AND attachment.User_Deletion_Id IS NULL
           WHERE sample.Im_RequestCommittee_ID = committee.ID
             AND sample.User_Deletion_Id IS NULL)
         THEN 1 ELSE 0 END AS bit) AS SampleAttachmentUploaded
        ,CAST(CASE WHEN EXISTS
          (SELECT 1 FROM dbo.Im_CheckRequest_SampleData sample
           WHERE sample.Im_RequestCommittee_ID = committee.ID
             AND sample.User_Deletion_Id IS NULL
             AND sample.Sample_BarCode IS NOT NULL
             AND (sample.IsAccepted IS NULL OR NOT EXISTS
               (SELECT 1 FROM dbo.A_AttachmentData attachment
                WHERE attachment.RowId = sample.ID
                  AND attachment.A_AttachmentTableNameId = 12
                  AND attachment.User_Deletion_Id IS NULL)))
         THEN 1 ELSE 0 END AS bit) AS HasPendingSampleFinalResult
        ,CAST(CASE WHEN EXISTS
          (SELECT 1 FROM dbo.Im_CheckRequest_SampleData labSample
           WHERE labSample.Im_RequestCommittee_ID = committee.ID
             AND labSample.Sample_BarCode IS NOT NULL
             AND labSample.User_Deletion_Id IS NULL
             AND (labSample.IsAccepted IS NULL OR NOT EXISTS
               (SELECT 1 FROM dbo.A_AttachmentData labAttachment
                WHERE labAttachment.RowId = labSample.ID
                  AND labAttachment.A_AttachmentTableNameId = 12
                  AND labAttachment.User_Deletion_Id IS NULL)))
         THEN 1 ELSE 0 END AS bit) AS HasPendingLabResult
        ,CAST(CASE WHEN EXISTS (SELECT 1 FROM dbo.Im_CommitteeResult r WHERE r.Committee_ID=committee.ID AND r.User_Deletion_Id IS NULL)
          AND NOT EXISTS (SELECT 1 FROM dbo.Im_CommitteeResult r WHERE r.Committee_ID=committee.ID AND r.User_Deletion_Id IS NULL AND r.CommitteeResultType_ID IS NULL)
          THEN 1 ELSE 0 END AS bit) AS InspectionComplete
        ,CAST(CASE WHEN EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData s WHERE s.Im_RequestCommittee_ID=committee.ID AND s.User_Deletion_Id IS NULL)
          AND NOT EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData s WHERE s.Im_RequestCommittee_ID=committee.ID AND s.User_Deletion_Id IS NULL AND (s.SampleSize IS NULL OR s.SampleSize<=0))
          THEN 1 ELSE 0 END AS bit) AS WithdrawalComplete
        ,CAST(CASE WHEN committee.IsFinishedAll=1 AND committee.Is_Cancel IS NULL
          AND EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData sample
                      WHERE sample.Im_RequestCommittee_ID=committee.ID AND sample.User_Deletion_Id IS NULL)
          AND NOT EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData sample
                          WHERE sample.Im_RequestCommittee_ID=committee.ID AND sample.User_Deletion_Id IS NULL
                            AND (sample.WithdrawDate IS NULL OR ISNULL(sample.SampleSize,0)<=0))
          AND NOT EXISTS (
              SELECT 1 FROM dbo.CommitteeEmployee assistant
              CROSS JOIN dbo.Im_CheckRequest_SampleData sample
              WHERE assistant.Committee_ID=committee.ID AND assistant.ISAdmin=0
                AND assistant.OperationType=74 AND assistant.User_Deletion_Id IS NULL
                AND sample.Im_RequestCommittee_ID=committee.ID AND sample.User_Deletion_Id IS NULL
                AND NOT EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData_Confirm opinion
                                WHERE opinion.Im_CheckRequest_SampleData_ID=sample.ID
                                  AND opinion.EmployeeId=assistant.Employee_Id AND opinion.IsAccepted IS NOT NULL))
          THEN 1 ELSE 0 END AS bit) AS CanShowLaboratoryResult
        ,(SELECT STRING_AGG(CONVERT(nvarchar(max), LTRIM(RTRIM(pending.Sample_BarCode))), '|')
          FROM dbo.Im_CheckRequest_SampleData pending
          WHERE pending.Im_RequestCommittee_ID=committee.ID AND pending.User_Deletion_Id IS NULL
            AND (pending.IsAccepted IS NULL OR NOT EXISTS
                (SELECT 1 FROM dbo.A_AttachmentData a WHERE a.RowId=pending.ID
                 AND a.A_AttachmentTableNameId=12 AND a.User_Deletion_Id IS NULL))) AS PendingLabBarcodes
    FROM dbo.Im_CheckRequest request
    INNER JOIN dbo.Im_RequestCommittee committee ON committee.ImCheckRequest_ID = request.ID
    INNER JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID = committee.ID
        AND employee.Employee_Id = @UserId
        AND employee.OperationType = 74
        AND employee.User_Deletion_Id IS NULL
    INNER JOIN dbo.CommitteeType type ON type.ID = committee.CommitteeType_ID
    LEFT JOIN dbo.Im_OpertaionType operation ON operation.ID = request.Im_OperationType
    OUTER APPLY
    (
        SELECT TOP (1) permission.ImporterName
        FROM dbo.View_List_Im_PermissionRequest permission
        WHERE permission.Im_CheckRequest_ID = request.ID
        ORDER BY permission.Im_PermissionRequest_ID DESC
    ) permissionImporter
    OUTER APPLY
    (
        SELECT TOP (1) requestData.DelegateName
        FROM dbo.Im_CheckRequest_Data requestData
        WHERE requestData.Im_CheckRequest_ID = request.ID
          AND requestData.User_Deletion_Id IS NULL
        ORDER BY requestData.ID DESC
    ) importer
    WHERE request.IsActive = 1
      AND (@CheckRequestId IS NULL OR request.ID = @CheckRequestId)
      AND committee.IsApproved = 1
      AND committee.User_Deletion_Id IS NULL
      AND committee.CommitteeType_ID IN (11, 13, 14)
      AND
      (
        -- Opening a request shows all its assigned committees, including completed withdrawal.
        @CheckRequestId IS NOT NULL
        OR committee.Delegation_Date = @SelectedDate
        OR (committee.CommitteeType_ID = 13 AND employee.ISAdmin = 1 AND EXISTS
            (SELECT 1 FROM dbo.Im_CheckRequest_SampleData pendingSample
             WHERE pendingSample.Im_RequestCommittee_ID = committee.ID
               AND pendingSample.User_Deletion_Id IS NULL
               AND EXISTS (SELECT 1 FROM dbo.A_AttachmentData pendingAttachment
                           WHERE pendingAttachment.RowId = pendingSample.ID
                             AND pendingAttachment.A_AttachmentTableNameId = 12
                             AND pendingAttachment.User_Deletion_Id IS NULL)
               AND (pendingSample.IsAccepted IS NULL OR pendingSample.Admin_Confirmation IS NULL)))
      )
)
SELECT ROW_NUMBER() OVER (ORDER BY CommitteeId) AS RowNum, *
FROM AssignedCommittees
ORDER BY CommitteeId