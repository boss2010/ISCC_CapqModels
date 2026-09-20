const fs=require('node:fs'), assert=require('node:assert/strict');
const {chromium}=require('C:/Users/pc/.cache/codex-runtimes/codex-primary-runtime/dependencies/node/node_modules/playwright');
const view=fs.readFileSync('PlantMobile/Views/ImportRequestDetails/Index.cshtml','utf8');
const script=view.match(/<script>\s*([\s\S]*?)<\/script>/)[1]
 .replace('@Html.Raw(analysisLabOptionsJson)',JSON.stringify([{AnalysisTypeId:1,AnalysisTypeName:'Analysis',AnalysisLabTypeId:2,AnalysisLabName:'Lab'}]))
 .replace('@Html.Raw(infectionItemsJson)','[]').replace('@allLots.Count','2');
(async()=>{
 const browser=await chromium.launch({channel:'msedge',headless:true});
 try { for(const inspection of [true,false]) {
  const page=await browser.newPage(), errors=[];
  page.on('pageerror',e=>errors.push(e.message));
  page.on('dialog',d=>{errors.push(d.message());d.dismiss();});
  await page.setContent(`${inspection?'<form id="inspectionSaveForm"><input id="performWithdrawal" name="PerformWithdrawal" value="true"><input id="applyToAll" value="false"><button id="saveInspectionButton">save</button></form>':''}
  <div id="withdrawalSection"><form id="samplesSaveForm">
  <input type="radio" name="WithdrawalModeChoice" value="partial" checked><input type="radio" name="WithdrawalModeChoice" value="total">
  <select id="sampleLot"><option value="3" data-request-item="4" data-short-name="5" data-item-name="A" data-lot-number="3">3</option><option value="6" data-request-item="7" data-short-name="8" data-item-name="B" data-lot-number="6">6</option></select>
  <select id="sampleAnalysisType"><option value="">choose</option></select><select id="sampleAnalysisLab"></select>
  <input id="newSampleSize" type="number" min="0.001" step="0.001"><input id="newSampleSeal"><textarea id="newSampleNotes"></textarea><button type="button" id="addSampleAnalysis">add</button>
  <div id="newSamplesContainer"></div>
  <details class="inspection-panel" data-sample-key="3-2" data-sample-attached="false"><input name="Samples[0].SampleId" value="50" type="hidden"><input name="Samples[0].LotDataId" value="3" type="hidden"><input name="Samples[0].AnalysisLabTypeId" value="2" type="hidden"><input name="Samples[0].IsTotal" value="false" type="hidden"><input name="Samples[0].SampleSize" value="1" type="number"><input name="Samples[0].SealNumber" value="old"><textarea name="Samples[0].Notes">old</textarea></details><button id="saveSamplesButton">save</button></form></div>`);
  await page.addScriptTag({content:fs.readFileSync('PlantMobile/wwwroot/js/weight-units.js','utf8')});
  await page.addScriptTag({content:script});
  async function add(note) {await page.selectOption('#sampleAnalysisType','1');await page.selectOption('#sampleAnalysisLab','2');await page.fill('#newSampleSize','2');await page.fill('#newSampleSeal','seal');await page.fill('#newSampleNotes',note);await page.click('#addSampleAnalysis');}
  await page.selectOption('#sampleLot','6');await add('partial');
  assert.equal(await page.locator('#newSamplesContainer .inspection-panel').count(),1);
  assert.equal(await page.locator('[data-sample-key="3-2"] textarea').inputValue(),'old');
  await page.locator('[name="WithdrawalModeChoice"][value="total"]').check();
  assert.equal(await page.locator('#sampleLot').isDisabled(),true);
  await add('total');
  assert.equal(await page.locator('#samplesSaveForm .inspection-panel').count(),2);
  const payload=await page.evaluate(inspection=>{const form=document.getElementById(inspection?'inspectionSaveForm':'samplesSaveForm');form.dispatchEvent(new Event('submit',{bubbles:true,cancelable:true}));return Object.fromEntries(new FormData(form));},inspection);
  assert.deepEqual([payload['Samples[0].LotDataId'],payload['Samples[1].LotDataId']].sort(),['3','6']);
  for(let i=0;i<2;i++){assert.equal(payload[`Samples[${i}].IsTotal`],'true');assert.equal(payload[`Samples[${i}].Notes`],'total');}
  assert.deepEqual(errors,[]);await page.close();
 }
 console.log('PASS: inspection and ordinary withdrawal, partial targeting, total committee coverage, existing sample reuse, and combined serialization.');
 } finally {await browser.close();}
})().catch(e=>{console.error(e);process.exitCode=1;});
