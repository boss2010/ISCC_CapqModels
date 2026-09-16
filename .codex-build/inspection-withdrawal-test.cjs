const fs = require('node:fs');
const assert = require('node:assert/strict');
const { chromium } = require('C:/Users/pc/.cache/codex-runtimes/codex-primary-runtime/dependencies/node/node_modules/playwright');
const view = fs.readFileSync('PlantMobile/Views/ImportRequestDetails/Index.cshtml', 'utf8');
const script = view.match(/<script>\s*([\s\S]*?)<\/script>/)[1]
  .replace('@Html.Raw(analysisLabOptionsJson)', JSON.stringify([{AnalysisTypeId:1, AnalysisTypeName:'Analysis', AnalysisLabTypeId:2, AnalysisLabName:'Lab'}]))
  .replace('@Html.Raw(infectionItemsJson)', '[]').replace('@allLots.Count', '1');
(async () => {
 const browser = await chromium.launch({channel:'msedge', headless:true});
 try {
  const page = await browser.newPage();
  const errors=[];
  page.on('pageerror', e => errors.push(e.message));
  page.on('dialog', d => d.dismiss());
  await page.setContent(`<form id="inspectionSaveForm"><input id="performWithdrawal" name="PerformWithdrawal" value="false"><input id="applyToAll" value="false"><input name="Lots[0].Notes" value="Inspection"><button id="openWithdrawal" type="button">withdraw</button><button id="saveInspectionButton">save</button></form>
   <div id="withdrawalSection" style="display:none"><form id="samplesSaveForm"><select id="sampleLot"><option value="3" data-request-item="4" data-short-name="5" data-item-name="Plant" data-lot-number="Lot">Lot</option></select><select id="sampleAnalysisType"><option value="">choose</option></select><select id="sampleAnalysisLab"></select><input id="newSampleSize" type="number" min="0.001" step="0.001"><input id="newSampleSeal"><textarea id="newSampleNotes"></textarea><button id="addSampleAnalysis" type="button">add</button><div id="newSamplesContainer"></div></form></div>`);
  await page.addScriptTag({content:fs.readFileSync('PlantMobile/wwwroot/js/weight-units.js','utf8')});
  await page.addScriptTag({content:script});
  assert.equal(await page.evaluate(() => new FormData(document.getElementById('inspectionSaveForm')).get('PerformWithdrawal')), 'false');
  await page.click('#openWithdrawal');
  // Selecting withdrawal without a sample must not submit the inspection.
  assert.equal(await page.evaluate(() => document.getElementById('inspectionSaveForm').dispatchEvent(new Event('submit',{bubbles:true,cancelable:true}))), false);
  async function addSample(notes) {
   await page.selectOption('#sampleAnalysisType','1'); await page.selectOption('#sampleAnalysisLab','2');
   await page.fill('#newSampleSize','2'); await page.fill('#newSampleSeal','Seal'); await page.fill('#newSampleNotes',notes);
   await page.click('#addSampleAnalysis');
  }
  await addSample('first');
  await page.click('.remove-new-sample');
  await addSample('replacement');
  // Change the externally associated sample weight to grams; submitted value remains kg.
  await page.locator('#newSamplesContainer .weight-unit').last().selectOption('0.001');
  await page.locator('#newSamplesContainer input[name$=".SampleSize"]').fill('500');
  const data = await page.evaluate(() => {
   const form=document.getElementById('inspectionSaveForm');
   form.dispatchEvent(new Event('submit',{bubbles:true,cancelable:true}));
   return Object.fromEntries(new FormData(form));
  });
  assert.equal(data['PerformWithdrawal'],'true');
  assert.equal(data['Lots[0].Notes'],'Inspection');
  assert.equal(data['Samples[0].Notes'],'replacement');
  assert.equal(data['Samples[0].SampleSize'],'0.5');
  assert.equal(data['Samples[0].AnalysisLabTypeId'],'2');
  assert.equal(data['Samples[1].SampleId'],undefined);
  assert.deepEqual(errors,[]);
  await page.click('.remove-new-sample');
  await page.locator('#newSampleSize').locator('..').locator('select').selectOption('count');
  await addSample('count sample');
  assert.equal(await page.locator('#newSamplesContainer .weight-editor select').inputValue(),'count');
  const countData = await page.evaluate(() => {
   const form=document.getElementById('inspectionSaveForm');
   form.dispatchEvent(new Event('submit',{bubbles:true,cancelable:true}));
   return Object.fromEntries(new FormData(form));
  });
  assert.equal(countData['Samples[0].QuantityUnit'],'count');
  assert.equal(countData['Samples[0].SampleSize'],'2');
  await page.locator('#newSamplesContainer input[name$=".SampleSize"]').fill('1.5');
  assert.equal(await page.locator('#newSamplesContainer input[name$=".SampleSize"]').evaluate(x=>x.checkValidity()),false);
  await page.setContent('<form id="normal"><input name="Samples[0].SampleSize" value="5" data-quantity-unit="count" min="0.001" step="0.001" type="number"><input name="Records[0].MaterialAmount" value="1"></form>');
  await page.addScriptTag({content:fs.readFileSync('PlantMobile/wwwroot/js/weight-units.js','utf8')});
  assert.equal(await page.locator('select').first().inputValue(),'count');
  assert.equal(await page.locator('select').last().locator('option[value="count"]').count(),0);
  assert.equal(await page.evaluate(()=>new FormData(document.getElementById('normal')).get('Samples[0].QuantityUnit')),'count');
  console.log('PASS: count selection in inspection, integer validation, ordinary withdrawal count reload, and treatment units unchanged.');
  console.log('PASS: inspection-only data, empty withdrawal blocked, combined payload, removed sample reindexing, and weight conversion.');
 } finally { await browser.close(); }
})().catch(e => {console.error(e); process.exitCode=1;});
