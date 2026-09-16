const fs=require('fs');let p='PlantMobile/Views/ImportRequests/Index.cshtml',s=fs.readFileSync(p,'utf8');s=s.replace('data-barcode="@barcode">نتيجة المعمل</button>','data-barcode="@barcode">@(item.PendingLabBarcodes.Split(\'|\', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Contains(barcode, StringComparer.Ordinal) ? "نتيجة المعمل" : "عرض نتيجة المعمل")</button>');s=s.replace("const body=document.getElementById('laboratoryResultBody');body.innerHTML=", "const body=document.getElementById('laboratoryResultBody');body.dataset.committee=button.dataset.committee;body.dataset.barcode=button.dataset.barcode;body.innerHTML=");s=s.replace(" document.querySelectorAll('.laboratory-result-button')",` document.addEventListener('lab-result-saved', event => {
    const body = event.target.closest('#laboratoryResultBody');
    if (!body) return;
    document.querySelectorAll('.laboratory-result-button').forEach(button => {
        if (button.dataset.committee === body.dataset.committee && button.dataset.barcode === body.dataset.barcode) button.textContent = 'عرض نتيجة المعمل';
    });
 });
 document.querySelectorAll('.laboratory-result-button')`);fs.writeFileSync(p,s);
