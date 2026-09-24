using EF.Imports;
using Microsoft.EntityFrameworkCore;

namespace PlantMobile.Data;

public sealed class ImportRequestReader(IDbContextFactory<ImportDbContext> factory)
{
    public async Task<List<ImportCommitteeRow>> LoadAsync(long userId, DateTime selectedDate, long? requestId, CancellationToken ct = default)
    {
        await using var db = await factory.CreateDbContextAsync(ct);
        db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        var date = DateOnly.FromDateTime(selectedDate);
        var samples = db.Im_CheckRequest_SampleData.Where(s => s.User_Deletion_Id == null);
        var attachments = db.A_AttachmentData.Where(a => a.A_AttachmentTableNameId == 12 && a.User_Deletion_Id == null);
        var pending = samples.Where(s => s.Sample_BarCode != null && (s.IsAccepted == null || !attachments.Any(a => a.RowId == s.ID)));
        var query = from c in db.EligibleCommittees()
                    join r in db.Im_CheckRequest on c.ImCheckRequest_ID equals r.ID
                    join e in db.CommitteeEmployee on c.ID equals e.Committee_ID
                    join t in db.CommitteeType on c.CommitteeType_ID equals t.ID
                    join o in db.Im_OpertaionType on r.Im_OperationType equals o.ID into operations
                    from o in operations.DefaultIfEmpty()
                    where e.Employee_Id == userId && e.OperationType == 74 && e.User_Deletion_Id == null
                        && r.IsActive == true && c.IsApproved == true && c.User_Deletion_Id == null
                        && (c.CommitteeType_ID == 11 || c.CommitteeType_ID == 13 || c.CommitteeType_ID == 14)
                        && !(c.CommitteeType_ID == 13
                            && samples.Any(s => s.Im_RequestCommittee_ID == c.ID && s.Sample_BarCode != null)
                            && !pending.Any(s => s.Im_RequestCommittee_ID == c.ID))
                        && (c.CommitteeType_ID != 13 || c.Delegation_Date == date
                            || (c.Delegation_Date < date && c.Is_Cancel == null
                                && (c.Is_Start_Android == true || c.Status == true || c.IsFinishedAll == true
                                    || samples.Any(s => s.Im_RequestCommittee_ID == c.ID && s.WithdrawDate != null && s.SampleSize > 0))
                                && pending.Any(s => s.Im_RequestCommittee_ID == c.ID)))
                        && (requestId.HasValue ? r.ID == requestId.Value : c.CommitteeType_ID == 13 || c.Delegation_Date == date)
                    select new ImportCommitteeRow
                    {
                        CheckRequestId = r.ID, CheckRequest_Number = r.CheckRequest_Number,
                        CommitteeId = c.ID, CommitteeType_ID = c.CommitteeType_ID!.Value,
                        CommitteeTypeName = t.Name_Ar, OperationTypeName = o.Name_Ar,
                        Status = c.Status, IsFinishedAll = c.IsFinishedAll, Is_Start_Android = c.Is_Start_Android,
                        Is_Cancel = c.Is_Cancel, User_Updation_Date = c.User_Updation_Date, ISAdmin = e.ISAdmin
                    };
        var rows = await query.Distinct().OrderBy(c => c.CommitteeId).TagWith("Import list: authorized committees").ToListAsync(ct);
        if (rows.Count == 0) return rows;
        var ids = rows.Select(c => c.CommitteeId).Distinct().ToArray();
        var requestIds = rows.Select(c => c.CheckRequestId).Distinct().ToArray();

        // Fixed number of narrow, bounded reads. Never load attachment binary data
        // or run one query per committee/lot. Every collection is scoped first.
        var completionCommittees = await db.EligibleCommittees()
            .Where(c => c.User_Deletion_Id == null && requestIds.Contains(c.ImCheckRequest_ID!.Value))
            .Select(c => new { c.ID, c.ImCheckRequest_ID, c.CommitteeType_ID, c.IsFinishedAll }).ToListAsync(ct);
        var allIds = completionCommittees.Select(c => c.ID).Concat(ids).Distinct().ToArray();
        var resultRows = await db.Im_CommitteeResult.Where(r => allIds.Contains(r.Committee_ID))
            .Select(r => new { r.ID, r.Committee_ID, r.CommitteeResultType_ID, r.User_Deletion_Id, r.LotData_ID, r.Im_Request_Item_Id }).ToListAsync(ct);
        var sampleRows = await samples.Where(s => allIds.Contains(s.Im_RequestCommittee_ID))
            .Select(s => new { s.ID, s.Im_RequestCommittee_ID, s.Sample_BarCode, s.IsAccepted, s.SampleSize, s.WithdrawDate,
                s.LotData_ID, s.Im_Request_Item_Id, HasAttachment = attachments.Any(a => a.RowId == s.ID) }).ToListAsync(ct);
        var selectedSamples = samples.Where(s => ids.Contains(s.Im_RequestCommittee_ID));
        var opinions = await db.Im_CheckRequest_SampleData_Confirm
            .Where(c => selectedSamples.Any(s => s.ID == c.Im_CheckRequest_SampleData_ID))
            .Select(c => new { c.Im_CheckRequest_SampleData_ID, c.EmployeeId, c.IsAccepted }).ToListAsync(ct);
        var resultOpinions = await db.Im_CommitteeResult_Confirm.Where(c => c.EmployeeId == userId
                && db.Im_CommitteeResult.Any(r => r.ID == c.Im_CommitteeResult_ID && ids.Contains(r.Committee_ID)))
            .Select(c => c.Im_CommitteeResult_ID).ToListAsync(ct);
        var members = await db.CommitteeEmployee.Where(e => ids.Contains(e.Committee_ID) && e.OperationType == 74 && e.User_Deletion_Id == null)
            .Select(e => new { e.Committee_ID, e.Employee_Id, e.ISAdmin }).ToListAsync(ct);
        var memberIds = members.Select(m => m.Employee_Id).Distinct().ToArray();
        // The existing authorization database is separate; retain its qualified
        // read through EF's unmapped SQL query, with the ID filter parameterized.
        var names = await db.Database.SqlQuery<PrivilegeName>($"SELECT CONVERT(bigint, Id) AS Id, FullName, LoginName FROM dbPrivilage.dbo.PR_User")
            .Where(u => memberIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id, ct);
        var companies = await db.Im_CheckRequest.Where(r => requestIds.Contains(r.ID))
            .Select(r => new {
                r.ID,
                PermissionName = db.View_List_Im_PermissionRequest.Where(p => p.Im_CheckRequest_ID == r.ID)
                    .OrderByDescending(p => p.Im_PermissionRequest_ID).Select(p => p.ImporterName).FirstOrDefault(),
                DelegateName = db.Im_CheckRequest_Data.Where(d => d.Im_CheckRequest_ID == r.ID && d.User_Deletion_Id == null)
                    .OrderByDescending(d => d.ID).Select(d => d.DelegateName).FirstOrDefault()
            }).ToDictionaryAsync(r => r.ID, ct);
        var lots = await (from lot in db.Im_CheckRequest_Items_Lot_Category
                          join item in db.Im_CheckRequest_Items on lot.Im_CheckRequest_Items_ID equals item.ID
                          join shipping in db.Im_CheckRequset_Shipping_Method on item.Im_CheckRequset_Shipping_Method_ID equals shipping.ID
                          where requestIds.Contains(shipping.Im_CheckRequest_ID!.Value) && lot.User_Deletion_Id == null
                              && item.User_Deletion_Id == null && shipping.User_Deletion_Id == null
                          select new { lot.ID, ItemId = item.ID, RequestId = shipping.Im_CheckRequest_ID!.Value,
                              Status = (from lr in db.Im_CheckRequest_Items_Lot_Result
                                        join status in db.Im_CheckRequest_Lot_Result_Status on lr.IS_Status equals status.ID
                                        where lr.Im_CheckRequest_Items_Lot_Category_ID == lot.ID && lr.IS_Status_Committee == true
                                        orderby lr.ID descending
                                        select new LotStatus { ID = lr.ID, Continues = status.Is_Continue }).FirstOrDefault()
                          }).ToListAsync(ct);

        var resultLookup = resultRows.ToLookup(r => r.Committee_ID);
        var sampleLookup = sampleRows.ToLookup(s => s.Im_RequestCommittee_ID);
        var opinionLookup = opinions.ToLookup(o => o.Im_CheckRequest_SampleData_ID);
        var resultOpinionLookup = resultOpinions.ToLookup(id => id);
        var memberLookup = members.ToLookup(m => m.Committee_ID);
        var unavailableResultTypes = (await db.CommitteeResultType.AsNoTracking()
            .Where(t => t.Name_Ar != null && t.Name_Ar.Contains("تعذر"))
            .Select(t => t.ID).ToListAsync(ct)).ToHashSet();
        var completion = new Dictionary<long, (bool Complete, bool Pending)>();
        foreach (long id in requestIds)
        {
            var committees = completionCommittees.Where(c => c.ImCheckRequest_ID == id).ToDictionary(c => c.ID);
            var inspections = committees.Values.Where(c => c.CommitteeType_ID == 11)
                .SelectMany(c => resultLookup[c.ID]).Where(r => r.User_Deletion_Id == null).ToList();
            var withdrawal = committees.Values.Where(c => c.CommitteeType_ID is 11 or 13)
                .SelectMany(c => sampleLookup[c.ID]).ToList();
            bool complete = true, any = false, pendingQuarantine = false;
            foreach (var lot in lots.Where(l => l.RequestId == id))
            {
                var inspection = inspections.Where(r => r.LotData_ID == lot.ID || (r.LotData_ID == null && r.Im_Request_Item_Id == lot.ItemId))
                    .OrderByDescending(r => r.ID).FirstOrDefault();
                if (lot.Status == null && inspection == null) continue;
                any = true;
                if (inspection?.CommitteeResultType_ID is byte resultType && unavailableResultTypes.Contains(resultType))
                    continue;
                complete &= lot.Status?.Continues == false;
                bool pendingWithdrawal = withdrawal.Any(s => (s.LotData_ID == lot.ID || (s.LotData_ID == null && s.Im_Request_Item_Id == lot.ItemId))
                    && (s.SampleSize.GetValueOrDefault() <= 0 || committees[s.Im_RequestCommittee_ID].IsFinishedAll != true || s.IsAccepted == null || !s.HasAttachment));
                pendingQuarantine |= lot.Status == null && inspection?.CommitteeResultType_ID != null && !pendingWithdrawal;
            }
            completion[id] = (any && complete, pendingQuarantine);
        }
        for (int index = 0; index < rows.Count; index++)
        {
            var row = rows[index];
            row.RowNum = index + 1;
            var company = companies[row.CheckRequestId];
            row.CompanyName = !string.IsNullOrWhiteSpace(company.PermissionName) ? company.PermissionName
                : !string.IsNullOrWhiteSpace(company.DelegateName) ? company.DelegateName : "غير محدد";
            (row.RequestQuarantineComplete, row.HasPendingQuarantine) = completion[row.CheckRequestId];
            var committeeSamples = sampleLookup[row.CommitteeId].OrderBy(s => s.ID).ToList();
            var committeeResults = resultLookup[row.CommitteeId].ToList();
            var activeResults = committeeResults.Where(r => r.User_Deletion_Id == null).ToList();
            var committeeMembers = memberLookup[row.CommitteeId].ToList();
            var pendingSamples = committeeSamples.Where(s => s.IsAccepted == null || !s.HasAttachment).ToList();
            row.SampleBarcodes = string.Join('|', committeeSamples.Where(s => !string.IsNullOrWhiteSpace(s.Sample_BarCode)).Select(s => s.Sample_BarCode!.Trim()));
            row.PendingLabBarcodes = string.Join('|', pendingSamples.Where(s => s.Sample_BarCode != null).Select(s => s.Sample_BarCode!.Trim()));
            row.CommitteeMembers = string.Join('|', committeeMembers.Select(m => {
                names.TryGetValue(m.Employee_Id, out var name);
                return $"{(!string.IsNullOrWhiteSpace(name?.FullName) ? name.FullName : name?.LoginName ?? m.Employee_Id.ToString())} ({(m.ISAdmin ? "أدمن" : "مساعد")})";
            }));
            row.ResultCount = committeeResults.Count(r => r.CommitteeResultType_ID != null);
            row.ConfirmCount = committeeResults.Sum(r => resultOpinionLookup[r.ID].Count());
            row.SampleCount = committeeSamples.Count(s => s.Sample_BarCode != null);
            row.SampleConfirmCount = committeeSamples.Where(s => s.Sample_BarCode != null).Sum(s => opinionLookup[s.ID].Count(o => o.EmployeeId == userId));
            row.SampleAttachmentUploaded = committeeSamples.Any(s => s.HasAttachment);
            row.HasPendingSampleFinalResult = pendingSamples.Any(s => s.Sample_BarCode != null);
            row.HasPendingLabResult = row.Is_Cancel == null && pendingSamples.Any(s => s.Sample_BarCode != null && s.WithdrawDate != null && s.SampleSize > 0);
            row.InspectionComplete = activeResults.Count > 0 && activeResults.All(r => r.CommitteeResultType_ID != null);
            row.WithdrawalComplete = committeeSamples.Count > 0 && committeeSamples.All(s => s.SampleSize > 0);
            row.CanShowLaboratoryResult = row.IsFinishedAll == true && row.Is_Cancel == null && committeeSamples.Count > 0
                && committeeSamples.All(s => s.WithdrawDate != null && s.SampleSize > 0)
                && committeeMembers.Where(m => !m.ISAdmin).All(m => committeeSamples.All(s => opinionLookup[s.ID].Any(o => o.EmployeeId == m.Employee_Id)));
        }
        return rows;
    }

    public sealed class PrivilegeName { public long Id { get; set; } public string? FullName { get; set; } public string? LoginName { get; set; } }
    private sealed class LotStatus { public long ID { get; set; } public bool? Continues { get; set; } }
}
