const fs=require('fs');const p='PlantMobile/Views/ImportRequestDetails/Index.cshtml';let s=fs.readFileSync(p,'utf8');
s=s.replace('<div class="inspection-mode shadow-sm mb-3">','<div class="inspection-mode-inline">').replace('<div class="form-label fw-bold text-info mb-2">طريقة تنفيذ الفحص</div>','').replace('فحص كلي — نتيجة واحدة لكل نبات','فحص كلي').replace('فحص جزئي — نتيجة لكل لوط','فحص جزئي');
const old='            @foreach (var item in details.Item_Data)\n            {\n                var itemLots = (item.Lot_Data ?? Enumerable.Empty<ViewModels._x0040_temp_table_Lot>()).ToList();';
s=s.replaceAll('\r\n','\n');if(!s.includes(old))throw Error('plant loop missing');
s=s.replace(old,`            @foreach (var plantGroup in details.Item_Data.GroupBy(x => x.Item_ShortName_id > 0 ? $"id:{x.Item_ShortName_id}" : $"name:{x.Item_Name}|{x.Item_ShortName_Name}"))
            {
                var item = plantGroup.First();
                var lotOwners = plantGroup.SelectMany(owner => (owner.Lot_Data ?? Enumerable.Empty<ViewModels._x0040_temp_table_Lot>()).Select(lot => new { Lot = lot, Owner = owner })).GroupBy(x => x.Lot.ID).ToDictionary(x => x.Key, x => x.First().Owner.ID);
                var itemLots = plantGroup.SelectMany(x => x.Lot_Data ?? Enumerable.Empty<ViewModels._x0040_temp_table_Lot>()).GroupBy(x => x.ID).Select(x => x.First()).ToList();`);
s=s.replace('name="Lots[@lotIndex].RequestItemId" value="@item.ID"','name="Lots[@lotIndex].RequestItemId" value="@lotOwners[lot.ID]"');
s=s.replace('<input type="hidden" name="PlantTotals[@plantIndex].RequestItemId" value="@item.ID" />',`<input type="hidden" name="PlantTotals[@plantIndex].RequestItemId" value="@item.ID" />
                        @foreach (var owner in plantGroup) { <input type="hidden" name="PlantTotals[@plantIndex].RequestItemIds" value="@owner.ID" /> }`);
s=s.replace("                applyToAll.value = isTotal ? 'true' : 'false';",`                applyToAll.value = isTotal ? 'true' : 'false';
                form.classList.toggle('total-mode', isTotal);
                if (isTotal) form.querySelectorAll('.lot-card').forEach(card => card.open = false);`);
s=s.replace("            modes.forEach(x => x.addEventListener('change', setInspectionMode));",`            form.querySelectorAll('.lot-card > summary').forEach(summary => summary.addEventListener('click', event => {
                if (currentMode() === 'total' && !event.target.closest('.weight-unit')) event.preventDefault();
            }));
            modes.forEach(x => x.addEventListener('change', setInspectionMode));`);
fs.writeFileSync(p,s);
