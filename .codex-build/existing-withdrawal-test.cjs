const fs=require('node:fs'),assert=require('node:assert/strict');
const {chromium}=require('C:/Users/pc/.cache/codex-runtimes/codex-primary-runtime/dependencies/node/node_modules/playwright');
const view=fs.readFileSync('PlantMobile/Views/ImportRequestDetails/Index.cshtml','utf8');
const script=view.match(/<script>\s*([\s\S]*?)<\/script>/)[1].replace('@Html.Raw(analysisLabOptionsJson)','[]').replace('@Html.Raw(infectionItemsJson)','[]').replace('@allLots.Count','2');
(async()=>{const browser=await chromium.launch({channel:'msedge',headless:true});try{
 const page=await browser.newPage(),errors=[];page.on('pageerror',e=>errors.push(e.message));
 await page.setContent(`<form id="samplesSaveForm"><div id="existingWithdrawalMode"><input type="radio" name="WithdrawalModeChoice" value="partial" checked><input type="radio" name="WithdrawalModeChoice" value="total"><div id="sharedWithdrawalFields"><input id="newSampleSize" type="number" min="0.001" step="0.001"><input id="newSampleSeal"><textarea id="newSampleNotes"></textarea></div></div>${[0,1].map(i=>`<div class="inspection-panel sample-result-card" data-sample-attached="false"><input type="hidden" name="Samples[${i}].SampleId" value="${i+10}"><input type="hidden" name="Samples[${i}].IsTotal" value="false"><input name="Samples[${i}].SampleSize" type="number" value="${i+1}" min="0.001" step="0.001"><input name="Samples[${i}].SealNumber"><textarea name="Samples[${i}].Notes">note</textarea></div>`).join('')}<button id="saveSamplesButton">save</button></form>`);
 await page.addScriptTag({content:fs.readFileSync('PlantMobile/wwwroot/js/weight-units.js','utf8')});await page.addScriptTag({content:script});
 assert.equal(await page.locator('#sharedWithdrawalFields').isVisible(),false);
 await page.locator('[value="total"]').check();
 await page.locator('#newSampleSize').locator('..').locator('select').selectOption('count');
 await page.fill('#newSampleSize','7');await page.fill('#newSampleSeal','seal');await page.fill('#newSampleNotes','shared');
 const data=await page.evaluate(()=>Object.fromEntries(new FormData(document.getElementById('samplesSaveForm'))));
 for(let i=0;i<2;i++){assert.equal(data[`Samples[${i}].SampleId`],String(i+10));assert.equal(data[`Samples[${i}].SampleSize`],'7');assert.equal(data[`Samples[${i}].QuantityUnit`],'count');assert.equal(data[`Samples[${i}].Notes`],'shared');assert.equal(data[`Samples[${i}].IsTotal`],'true');}
 await page.locator('[value="partial"]').check();await page.locator('[name="Samples[0].SampleSize"]').fill('3');
 assert.equal(await page.locator('[name="Samples[1].SampleSize"]').inputValue(),'7');assert.deepEqual(errors,[]);
 console.log('PASS: shared total values update existing sample IDs, preserve count units, and partial editing remains independent.');
}finally{await browser.close();}})().catch(e=>{console.error(e);process.exitCode=1;});
