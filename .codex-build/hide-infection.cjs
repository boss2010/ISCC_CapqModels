const fs=require('fs');let p='PlantMobile/Views/ImportRequestDetails/Index.cshtml';let s=fs.readFileSync(p,'utf8');let a=s.indexOf('            const updateInfectionPanels = () => {');let b=s.indexOf('            const updateProgress =',a);if(a<0||b<0)throw Error('handler missing');s=s.slice(0,a)+`            const updateInfectionPanels = () => {
                document.querySelectorAll('[data-infection-panel]').forEach(panel => {
                    panel.hidden = true;
                    panel.classList.remove('visible');
                    panel.querySelectorAll('select').forEach(field => { field.required = false; field.disabled = true; });
                });
            };

`+s.slice(b);fs.writeFileSync(p,s);
