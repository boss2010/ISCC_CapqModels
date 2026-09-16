const fs=require('fs');const p='PlantMobile/Views/ImportRequestDetails/Index.cshtml';let s=fs.readFileSync(p,'utf8');s=s.replaceAll('Model.IsReadOnly','isWorkReadOnly');s=s.replace('    var details = Model.Details;','    var details = Model.Details;\n    bool isWorkReadOnly = Model.IsReadOnly || (Model.CommitteeTypeId == 11 && Model.IsAdmin && Model.InspectionComplete);');
s=s.replace('تم الانتهاء من اللجنة.</strong> البيانات معروضة للقراءة فقط ولا يمكن تعديلها.', 'تم الانتهاء من العمل.</strong> بيانات الفحص والسحب معروضة للقراءة فقط.');
let marker='    @if (Model.CommitteeTypeId is 11 or 13)';let a=s.indexOf(marker,s.indexOf('id="inspectionSaveForm"'));
if(a<0)throw Error('missing insertion');
const form=`    @if (Model.CommitteeTypeId == 11 && Model.IsAdmin && !Model.IsReadOnly && Model.InspectionComplete && !Model.HasSamples)
    {
        <form asp-controller="ImportRequestDetails" asp-action="SaveQuarantine" method="post" class="card shadow-sm mb-4" id="inspectionQuarantineForm">
            @Html.AntiForgeryToken()
            <input type="hidden" name="CheckRequestId" value="@Model.CheckRequestId" />
            <input type="hidden" name="CommitteeId" value="@Model.CommitteeId" />
            <input type="hidden" name="CommitteeTypeId" value="@Model.CommitteeTypeId" />
            <div class="card-header"><h4 class="text-primary mb-0">موقف الحجر</h4></div>
            <div class="card-body">
                @{ int quarantineIndex = 0; }
                @foreach (var lot in allLots.GroupBy(x => x.ID).Select(x => x.First()))
                {
                    <div class="inspection-field mb-3">
                        <input type="hidden" name="Lots[@quarantineIndex].LotCategoryId" value="@lot.ID" />
                        <label class="form-label" for="quarantine-@lot.ID">موقف الحجر — اللوط @lot.Lot_Number</label>
                        <select id="quarantine-@lot.ID" class="form-select" name="Lots[@quarantineIndex].QuarantineStatusId" required>
                            <option value="">اختر موقف الحجر</option>
                            @foreach (var status in Model.QuarantineStatuses) { <option value="@status.Value">@status.DisplayText</option> }
                        </select>
                        <label class="form-label mt-2" for="quarantine-notes-@lot.ID">الملاحظات</label>
                        <textarea id="quarantine-notes-@lot.ID" name="Lots[@quarantineIndex].Notes" class="form-control" rows="2"></textarea>
                    </div>
                    quarantineIndex++;
                }
                <button type="submit" class="btn btn-success">حفظ موقف الحجر وإنهاء اللجنة</button>
            </div>
        </form>
    }
`;
s=s.slice(0,a)+form+s.slice(a);fs.writeFileSync(p,s);
