const fs=require('fs');const p='PlantMobile/Controllers/ImportRequestDetailsController.cs';let s=fs.readFileSync(p,'utf8');
const start=s.indexOf('(committee.CommitteeType_ID = 11 AND (');const end=s.indexOf('                             OR (committee.CommitteeType_ID <> 11',start);
s=s.slice(0,start)+'(committee.CommitteeType_ID = 11 AND (committee.Status = 1 OR committee.IsFinishedAll = 1))\n'+s.slice(end);
// Preserve the lock on inspection and withdrawal, independently of quarantine.
for(const method of ['SaveInspection(ImportInspectionSaveVm input)','SaveSamples(ImportSamplesSaveVm input)']){
 const a=s.indexOf(method),b=s.indexOf('        var access = await GetCommitteeAccessAsync(input.CheckRequestId, input.CommitteeId);',a);
 const pos=b+'        var access = await GetCommitteeAccessAsync(input.CheckRequestId, input.CommitteeId);'.length;
 s=s.slice(0,pos)+`\n        if (input.CommitteeTypeId == 11) {
            var workflow = await LoadWorkflowStateAsync(input.CommitteeId);
            if (workflow.InspectionCount > 0 && workflow.IncompleteInspectionCount == 0) {
                TempData["SaveError"] = "اكتمل الفحص؛ البيانات للعرض فقط. المتاح هو استكمال موقف الحجر.";
                return RedirectToAction(nameof(Index), new { checkRequestId=input.CheckRequestId, committeeId=input.CommitteeId, committeeTypeId=input.CommitteeTypeId });
            }
        }\n`+s.slice(pos);
}
fs.writeFileSync(p,s);
const q='PlantMobile/Controllers/ImportRequestsController.cs';s=fs.readFileSync(q,'utf8').replace('status || finished || (Convert.ToByte(reader["CommitteeType_ID"]) == 11 && GetBool(reader, "InspectionComplete"))','status || finished');fs.writeFileSync(q,s);
