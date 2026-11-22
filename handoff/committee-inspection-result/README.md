# Handoff — شاشة «نتيجة فحص اللجنة» (Committee Inspection Result)

تاريخ: 22 نوفمبر 2026. تم نقل الشاشة من `PlantQuarantineNew` إلى هذا المشروع (`PlantMobile`).
هذا الملف لكي يحدّث الشات/المطوّر في هذا الريبو حالته ويعتمد التغييرات.

## 1) القرارات المعتمدة

- **حذف نهائي** لكل الجداول القديمة `Cairo_Suburbs_*` (بدون `sp_rename`، لا ترحيل بيانات).
- الاسم العام الجديد: `dbo.Im_Committee_Inspection_Result` والـ sequence `dbo.Im_Committee_Inspection_Result_SEQ` (قاعدة السليوشن: اسم الجدول + `_SEQ`).
- العمود `Request_Number` أصبح `CheckRequest_Number` (مطابقة لـ `Im_CheckRequest`).
- الجداول الزيادة `Cairo_Suburbs_NegotiationType` / `AidType` / `Procedure` **أُزيلت**؛ القوائم تُقرأ من جداول السليوشن الموجودة والـ Enums:
  - الإجراء: `dbo.Im_CheckRequest_Lot_Result_Status`
  - نتيجة الفحص: `dbo.CommitteeResultType`، النوع: `dbo.Transport_Mean`، الصنف/الإصابة: `dbo.Item`، المنشأ: `dbo.Country`
  - نوع الدولة / الإعانة: نسخة محلية من قيم الـ enums في `CommitteeInspectionResultDefaults` (لأن المشروع لا يرجع `PlantQuar.DTO`).
- الشاشة مفعّلة للأندرويد: `PR_Menu.isEnableAndroid = 1`.
- صلاحية **مصطفى** (EmpId 27338 / login `mostafa` / الاسم؛ `PR_User_id = 354`): View/Add/Edit/Print = 1، Delete = 0، مجمّع/قسم يُنسخ من `Committees/CommitteeType/Index`. عمليًا طلع **Group_Id = 1 / PR_ModuleId = 3** (الـ fallback 8/32 ما استخدمش لأن شاشة المصدر موجودة).

## 2) الملفات المضافة/المعدّلة في هذا الريبو

| الملف | الغرض |
|---|---|
| `PlantMobile/Controllers/ImCommittee_InspectionResultController.cs` | Controller الجديد: `GET Committees/ImCommittee_InspectionResult/Index` و `POST .../Save`. يقرأ القوائم بـ SqlCommand، ويحفظ في الجدول الجديد داخل ترانزاكشن + سجل في `Table_Action_Log_CheckRequest`. |
| `PlantMobile/ViewModels/CommitteeInspectionResultVm.cs` | Page/Save VM + قيم enum محلية. |
| `PlantMobile/Views/ImCommittee_InspectionResult/Index.cshtml` | الواجهة (نفس حقول الشاشة القديمة) + حفظ AJAX مع anti-forgery. |
| `PlantMobile/Sql/20261122_Im_Committee_Inspection_Result_Table.sql` | حذف الجداول القديمة + إنشاء الجدول والـ sequence والفهرس. |
| `PlantMobile/Sql/20261122_Add_Committee_InspectionResult_Screen.sql` | تسجيل الشاشة في `dbPrivilage`، `isEnableAndroid = 1`، ومنح مصطفى الصلاحية. |
| `PlantMobile/appsettings.json` | الاتصالين `DBConnection` و `PrivilegeDBConnection` بقوا على نفس سيرفر المشروع الرئيسي `172.16.45.11` (بدل `DESKTOP-3OTJB3D`)، مطابقين لـ `PlantQuar.WEB/Web.config`. |

الرابط: `Committees/ImCommittee_InspectionResult/Index` (كما في النظام القديم؛ الـ controller يستخدم attribute route بهذا البادئة).

## 3) التنفيذ الفعلي على السيرفر (تم)

السيرفر المستخدم هو نفسه الموجود في `PlantQuar.WEB/Web.config`: `172.16.45.11` (قواعدتا `PlantQuarantine_New` و `dbPrivilage`). اتصال `PlantMobile` اتعدّل له.

1. ✅ `20261122_Im_Committee_Inspection_Result_Table.sql` على `PlantQuarantine_New`: نجح. التحقق النهائي رجّع بس `Im_Committee_Inspection_Result` و `Im_Committee_Inspection_Result_SEQ` — أي كل `Cairo_Suburbs_*` اتحذفت نهائيًا (`OLD_DROPPED`).
2. ✅ `20261122_Add_Committee_InspectionResult_Screen.sql` على `dbPrivilage`: نجح. المنيو **Id = 260**، `MenuURL = Committees/ImCommittee_InspectionResult/Index`، `isEnableAndroid = 1`، `Group_Id = 1`، `PR_ModuleId = 3`. صلاحية `PR_User_id = 354` (mostafa / EmpId 27338) مفعّلة.
   - ملاحظة: كان فيه كمان صفوف صلاحية للمستخدمين 1 و 353 على نفس المنيو (موجودة قبل كده)، لم نلمسها ولم نحذفها.
3. لا حاجة لسكربت `isEnableAndroid` منفصل؛ السكربت بيضيف العمود لو ناقص.

كلا السكربتين idempotent (آمن لإعادة التنفيذ) وينتهيان بـ SELECT للتحقق.

## 4) نتيجة الاختبار (محدّثة)

- `dotnet build PlantMobile.csproj --no-restore`: **Build succeeded، 0 أخطاء و0 تحذيرات** في آخر تشغيل.
- الاتصال بالسيرفر `172.16.45.11` اتختبر بـ `sqlcmd`: نجح (`PlantQuarantine_New / 1`).
- Smoke test للحفظ: تنفيذ SQL الإدراج الخاص بالـ controller داخل `BEGIN TRAN` + إدراج سجل `Table_Action_Log_CheckRequest` ثم `ROLLBACK` → نجح، ورُجع `FormId = 1` و `LogId = 1812782`، وتأكدنا إن أعمدة الجدولين مطابقة للكود. **ما تركّش أي بيانات اختبار**.
- تشغيل التطبيق فعليًا (`dotnet run --no-build --urls http://localhost:5206`) والطلب على `http://localhost:5206/Committees/ImCommittee_InspectionResult/Index` رجّع **302 → `/Login/Index`**: يعني الـ route اتسجّل صح والـ middleware عامل زي باقي الشاشات (محتاجين جلسة مستخدم).
- لم تُجرَ تجربة مستخدم كاملة بحساب مصطفى داخل الموبايل (محتاجين كلمة المرور/الجلسة).

## 5) المطلوب من الشات التاني

1. مراجعة الـ diff وتأكيد group/module الفعليين (1/3، مش 8/32).
2. فتح الشاشة بحساب مصطفى والتأكد من الحفظ الفعلي وظهورها في منيو الأندرويد.
3. لو احتاج الاسم مختلفًا (مثلاً `Im_CommitteeResult_Inspection`)، غيّره في السكربت والـ controller معًا قبل أي إعادة تشغيل للسكربت.

لا توجد كلمات مرور في هذا الملف (الإعدادات في `appsettings.json` فقط).
