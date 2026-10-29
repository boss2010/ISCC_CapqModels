# تسليم كود موقف الحجر — 28 سبتمبر 2026

هذه نسخة من الملفات الحالية لتسليمها لمطور يعمل على موقف الحجر والتحقق منه وحفظه في طلبات الوارد. الملفات من نسخة العمل الحالية، بما فيها التعديلات غير المرفوعة في ImportRequestDetailsController.cs. هذه حزمة مرجعية للدمج داخل المشروع وليست تطبيقًا مستقلًا جاهزًا للتشغيل. لم يتم تغيير منطق المشروع أو تنفيذ حفظ على قاعدة البيانات.

## ابدأ من هنا

1. `PlantMobile/Controllers/ImportRequestsController.cs`: الدالة `QuarantineLots` لقراءة البيانات والتحقق من إتاحتها، والدالة `SaveQuarantineLots` للتحقق على السيرفر والحفظ. موديلات `RequestQuarantine*` موجودة في نهاية الملف، ومنها شرط `IsReady`.
2. `PlantMobile/Views/ImportRequests/_QuarantineLots.cshtml`: اختيار موقف الحجر والملاحظات لكل لوط جاهز، وإظهار سبب عدم الإتاحة.
3. `PlantMobile/Views/ImportRequests/Index.cshtml`: زر `openQuarantine` والنافذة `quarantineModal` وتحميل المحتوى وإرسال `FormData` بالحفظ.
4. `PlantMobile/Infrastructure/ImportCommitteeEligibility.cs`: فلتر SQL المستخدم لاستبعاد اللجان القديمة التي لم يبدأ العمل الفعلي عليها.
5. `PlantMobile/Data/ImportRequestReader.cs` و`ImportCommitteeRow.cs`: حساب حالة الطلب المعروضة في القائمة. ملف `ImportRequestsSql.cs` مرفق كمرجع SQL أيضًا؛ القراءة في `LoadRequests` تستدعي `ImportRequestReader` حاليًا.

## مسار العرض والتحقق

- `GET /ImportRequests/QuarantineLots?checkRequestId=...` يتأكد من الجلسة وعضوية المستخدم في لجنة للطلب مع `OperationType=74`.
- قراءة نتيجة فحص من لجنة نوعها `11` يكون المستخدم أدمن فيها، على مستوى اللوط أو الصنف عند غياب `LotData_ID`.
- اللوط جاهز إذا اكتمل الفحص ولم تكن النتيجة تحتوي على «تعذر»، واكتمل السحب إن وجد، وسُجلت نتيجة المعمل مع مرفق لكل عينة.
- اكتمال السحب يعتمد على `SampleSize > 0` و`IsFinishedAll` للجنة. نتيجة المعمل تعتمد على `IsAccepted IS NOT NULL` ووجود مرفق غير محذوف من نوع `A_AttachmentTableNameId=12`.
- عند وجود موقف محفوظ تعرض الواجهة الموقف فقط. `Is_Continue=false` يعرض انتهاء الإجراءات على اللوط.
- قائمة المواقف من `Im_CheckRequest_Lot_Result_Status` مع `ID > 0` و`ISNULL(IsActive,1)=1`.

## بيانات الحفظ

`POST /ImportRequests/SaveQuarantineLots` مع AntiForgeryToken والجلسة الحالية:

```text
CheckRequestId=...
Lots[0].LotId=...
Lots[0].StatusId=...
Lots[0].Notes=...
__RequestVerificationToken=...
```

يعيد السيرفر فحص جاهزية كل لوط وتبعيته للطلب وصلاحية المستخدم. داخل Transaction واحدة يغيّر `IS_Status_Committee` للسجلات السابقة إلى `0`، ثم يضيف سجلًا جديدًا بقيمة `1` في `Im_CheckRequest_Items_Lot_Result`. المفتاح من `Im_CheckRequest_Items_Lot_Result_SEQ`، ويُحفظ المستخدم والتاريخ والملاحظات. نجاح كل اللوطات يؤدي إلى Commit، وأي خطأ يؤدي إلى Rollback.

## مسار آخر يجب مراجعته عند التعديل

`PlantMobile/Controllers/ImportRequestDetailsController.cs` يتضمن `SaveQuarantine(ImportQuarantineSaveVm)` وشروطه ومساعداته داخل الملف نفسه، كما يتضمن حفظ بيانات العينات وما يرتبط بموقف الحجر.

`SaveQuarantine` يستخدم `GetCommitteeAccessAsync` و`ResolveLotItemsAsync` و`EnsureLotInspectionAvailableAsync`، ثم يحفظ المواقف وينهي اللجنة. شروطه تختلف عن مسار النافذة في `ImportRequestsController`؛ لا تفترض أن تعديل أحدهما يعدّل الآخر.

## نقاط مراجعة ظاهرة من الكود الحالي

- `SaveQuarantineLots` يتحقق من أن `StatusId` موجب، لكنه لا يعيد التحقق صراحة من نشاط الموقف ووجوده في قائمة المواقف المتاحة.
- إخفاء التعديل عند وجود موقف محفوظ يتم في الواجهة؛ مسار الحفظ لا يمنع صراحة إعادة إرسال لوط له موقف سابق، بما فيه موقف ينهي الإجراءات.
- شرط `OperationType=74` ظاهر في قراءة عضوية المستخدم، لكنه غير موجود في شرط عضوية المستخدم داخل `readySql` للحفظ.
- لا يوجد تحقق صريح من تكرار `LotId` في نفس الإرسال، ولا قفل صريح خاص بمنع حفظين متزامنين لنفس اللوط.
- سكربت تغيير تحذير انتهاء الإجراءات موجود داخل الـPartial، بينما تحميلها يتم باستخدام `innerHTML`. يجب مراجعة ربط حدث `change` لأن سكربتات HTML المحقونة بهذه الطريقة لا تُنفذ تلقائيًا.

هذه ملاحظات مراجعة للكود المنقول، ولم تُطبق إصلاحات في هذه الحزمة.

## متطلبات الدمج والتحقق

يلزم مشروع ASP.NET Core MVC الحالي، وMicrosoft.Data.SqlClient، وEF الخاص بالمشروع، والجلسات وRazor Tag Helpers وAntiForgery وBootstrap/jQuery المستخدمين بالواجهة، وقاعدة SQL Server بالجداول والـSequence المشار إليها في الاستعلامات. لم تُرفق إعدادات الاتصال أو كلمات المرور.

يُراجع المطور حالات: عدم وجود صلاحية، عدم اكتمال الفحص، تعذر الفحص، سحب غير مكتمل، نتيجة معمل بلا مرفق، لوط جاهز، موقف محفوظ مسبقًا، موقف غير نشط، تكرار الإرسال، وفشل أحد اللوطات مع التأكد من تراجع المعاملة كاملة. لم يتم تشغيل هذه السيناريوهات على قاعدة بيانات ضمن تجهيز التسليم.
