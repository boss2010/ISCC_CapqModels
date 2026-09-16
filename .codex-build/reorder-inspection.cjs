const fs=require('fs');
const p='PlantMobile/Views/ImportRequestDetails/Index.cshtml';let s=fs.readFileSync(p,'utf8').replaceAll('\r\n','\n');
let a=s.indexOf('    @if (Model.CommitteeTypeId is 11 or 13 && allLots.Count > 0)'),b=s.indexOf('    @if (details.Item_Data == null',a);s=s.slice(0,a)+s.slice(b);
a=s.indexOf('            @if (!Model.IsReadOnly && !Model.InspectionComplete)');b=s.indexOf('            @foreach (var item in details.Item_Data)',a);
let mode=s.slice(a,b).replace('@if (!Model.IsReadOnly && !Model.InspectionComplete)','@if (Model.CommitteeTypeId == 11 && Model.IsAdmin && !Model.IsReadOnly && !Model.InspectionComplete)').replaceAll('name="InspectionModeChoice"','name="InspectionModeChoice" form="inspectionSaveForm"');s=s.slice(0,a)+s.slice(b);
let marker='<div class="col-md-4 mb-3"><strong>المنفذ</strong><div>@details.Outlet_Name</div></div>';
s=s.replace(marker,marker+`<div class="col-md-6 mb-3"><strong>إجمالي الوزن القائم</strong><div>@await Html.PartialAsync("_Weight", (decimal?)totalGrossWeight)</div></div><div class="col-md-6 mb-3"><strong>إجمالي عدد الطرود</strong><div>@totalPackageCount</div></div><div class="col-12">${mode}</div>`);
a=s.indexOf('            <div class="card shadow-sm mb-4" id="withdrawalDecisionCard">');b=s.indexOf('            <div class="lots-heading mb-3">',a);s=s.slice(0,a)+'<input type="hidden" name="PerformWithdrawal" id="performWithdrawal" value="@hasSavedWithdrawal.ToString().ToLowerInvariant()" />\n'+s.slice(b);
s=s.replace('@if (!Model.IsReadOnly && !Model.InspectionComplete)\n            {\n            <div class="card shadow-sm mb-4"><div','@if (!Model.IsReadOnly)\n            {\n            <div class="card shadow-sm mb-4"><div');
s=s.replace('<button type="submit" class="btn btn-success px-5" id="saveInspectionButton">حفظ نتيجة الفحص</button>','<div class="d-flex flex-wrap" style="gap:10px"><button type="button" class="btn btn-outline-primary" id="openWithdrawal" aria-controls="withdrawalSection">سحب عينة</button>@if (!Model.InspectionComplete) { <button type="submit" class="btn btn-success px-5" id="saveInspectionButton">حفظ نتيجة الفحص</button> }</div>');
a=s.indexOf('            const updateWithdrawalDecision =');b=s.indexOf('            const samplesForm =',a);s=s.slice(0,a)+`document.getElementById('openWithdrawal')?.addEventListener('click', () => {
 document.getElementById('performWithdrawal').value = 'true';
 if (withdrawalSection) { withdrawalSection.style.display = ''; withdrawalSection.scrollIntoView({behavior:'smooth'}); document.getElementById('sampleLot')?.focus({preventScroll:true}); }
});\n`+s.slice(b);
marker='<div class="inspection-field d-flex align-items-end"><button';s=s.replace(marker,`<div class="inspection-field"><label for="newSampleSize">وزن العينة</label><input id="newSampleSize" class="form-control" type="number" min="0.001" step="0.001" /></div><div class="inspection-field"><label for="newSampleSeal">رقم الختم</label><input id="newSampleSeal" class="form-control" /></div><div class="inspection-field inspection-field-notes"><label for="newSampleNotes">الملاحظات</label><textarea id="newSampleNotes" class="form-control" rows="3"></textarea></div>`+marker);
marker='                const key = `${lotOption.value}-${labOption.value}`;';s=s.replace(marker,`const draftSize=document.getElementById('newSampleSize'),draftSeal=document.getElementById('newSampleSeal'),draftNotes=document.getElementById('newSampleNotes');
if (!draftSize.value || !draftSize.checkValidity() || !draftSeal.value.trim() || !draftNotes.value.trim()) { alert('استكمل وزن العينة ورقم الختم والملاحظات قبل الإضافة.'); return; }
const sampleKg=Number(draftSize.value)*Number(draftSize.dataset.weightFactor || 1);
`+marker);
s=s.replace('                newSamplesContainer.appendChild(card);',`card.querySelector('input[name$=".SampleSize"]').value=Number(sampleKg.toFixed(12)).toString();
card.querySelector('input[name$=".SealNumber"]').value=draftSeal.value.trim();
card.querySelector('textarea[name$=".Notes"]').value=draftNotes.value.trim();
newSamplesContainer.appendChild(card);
draftSize.value='';draftSeal.value='';draftNotes.value='';`);
s=s.replaceAll('حجم العينة','وزن العينة');fs.writeFileSync(p,s);
