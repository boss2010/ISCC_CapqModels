const fs=require('node:fs');
const assert=require('node:assert/strict');
const {chromium}=require('C:/Users/pc/.cache/codex-runtimes/codex-primary-runtime/dependencies/node/node_modules/playwright');
(async()=>{
 const view=fs.readFileSync('PlantMobile/Views/ImportRequests/Index.cshtml','utf8');
 const start=view.indexOf("        document.querySelectorAll('.request-tab').forEach");
 const end=view.indexOf("        quarantineButton?.addEventListener",start);
 const browser=await chromium.launch({channel:'msedge',headless:true});
 try{
  const page=await browser.newPage();
  await page.setContent(`<style>${view.match(/<style>([\s\S]*?)<\/style>/)[1]}</style>`+
   ['today','completed','pending-lab'].map(c=>`<button class="request-tab" data-request-filter="${c}">${c}</button><a class="request-choice" data-category="${c}">${c}</a>`).join(''));
  await page.addScriptTag({content:view.slice(start,end)});
  for(const category of ['today','completed','pending-lab','today']){
   if(category!=='today'||await page.locator('.request-tab.active').getAttribute('data-request-filter')!=='today')
    await page.locator(`[data-request-filter="${category}"]`).click();
   assert.deepEqual(await page.locator('.request-choice:visible').allTextContents(),[category]);
  }
  console.log('PASS: initial today tab and mutually exclusive visible requests in all tabs.');
 }finally{await browser.close();}
})().catch(e=>{console.error(e);process.exitCode=1;});
