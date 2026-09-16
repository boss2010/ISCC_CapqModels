const fs=require('fs');let p='PlantMobile/Views/ImportRequests/Index.cshtml';let s=fs.readFileSync(p,'utf8');s=s.replace('@(item.Committee_Type_Id == 13 && item.IsAdmin && !item.HasPendingLabResult ? "عرض اللجنة" : item.IsReadOnly ? "عرض اللجنة" : item.IsAdmin ? "بدء / استكمال" : "إبداء الرأي")','@item.ActionText');fs.writeFileSync(p,s);
p='PlantMobile/Views/ImportRequestDetails/Index.cshtml';s=fs.readFileSync(p,'utf8').replaceAll('\r\n','\n');
s=s.replace('    var completedInspectionLots = inspectionResultsByLot.Keys.ToHashSet();',`    var completedInspectionLots = inspectionResultsByLot.Keys.ToHashSet();
    foreach (var owner in details.Item_Data ?? Enumerable.Empty<ViewModels._x0040_Item_Data>())
        if (totalInspectionResultsByItem.ContainsKey(owner.ID))
            completedInspectionLots.UnionWith((owner.Lot_Data ?? Enumerable.Empty<ViewModels._x0040_temp_table_Lot>()).Select(x => x.ID));`);
let a=s.indexOf('                    <div class="inspection-panel shadow-sm mb-3">',s.indexOf('var sampleLot ='));let b=s.indexOf('                    sampleIndex++;',a);let block=s.slice(a,b);
block=block.replace('<div class="inspection-panel shadow-sm mb-3">','<details class="inspection-panel sample-result-card shadow-sm mb-3" open="@(sample.SampleSize == null || sample.SampleSize <= 0)">');
const title='<div class="inspection-panel-title">@item.Item_Name — @(string.IsNullOrWhiteSpace(sample.Lotnum) ? "سحب كلي" : $"اللوط {sample.Lotnum}")</div>';
block=block.replace(title,'');block=block.replace('<input type="hidden" name="Samples[@sampleIndex].SampleId"', '<summary class="inspection-panel-title">@item.Item_Name — @(string.IsNullOrWhiteSpace(sample.Lotnum) ? "سحب كلي" : $"اللوط {sample.Lotnum}") <span class="status-badge">@(sample.SampleSize > 0 ? "تم السحب — عرض البيانات" : "استكمال السحب")</span></summary>\n                        <input type="hidden" name="Samples[@sampleIndex].SampleId"');
let end=block.lastIndexOf('</div>');block=block.slice(0,end)+'</details>'+block.slice(end+6);s=s.slice(0,a)+block+s.slice(b);
// Open the relevant saved sample when validation needs a field inside it.
s=s.replace("            samplesForm?.addEventListener('submit', () => {",`            samplesForm?.addEventListener('invalid', event => event.target.closest('details')?.setAttribute('open', 'open'), true);
            samplesForm?.addEventListener('submit', () => {`);
fs.writeFileSync(p,s);
