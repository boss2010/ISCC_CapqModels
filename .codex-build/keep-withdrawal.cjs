const fs=require('fs');const p='PlantMobile/Controllers/ImportRequestsController.cs';let s=fs.readFileSync(p,'utf8');const a=s.indexOf('                      AND\n                      (',s.indexOf('AND committee.CommitteeType_ID IN (11, 13, 14)'));const b=s.indexOf('                )\n                SELECT ROW_NUMBER()',a);if(a<0||b<0)throw Error('query boundaries missing');s=s.slice(0,a)+`                      AND
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
`+s.slice(b);fs.writeFileSync(p,s);
