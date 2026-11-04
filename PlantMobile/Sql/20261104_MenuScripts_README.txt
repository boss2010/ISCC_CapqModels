20261104 PlantMobile — Menu scripts (combined)
===============================================

تاريخ التجميع: 2026-11-04 (الأربعاء)
المشروع: PlantMobile (ISCC) — boss2010/ISCC_CapqModels @ master 788bd54
المكان: C:\Users\Nabila\Desktop\20261104_Menu_Scripts\

الملف: 20261104_PlantMobile_Menu_Scripts_COMBINED.sql
يحتوي على:
  PART 1 = 20261104_Menu_GetByUser.sql         (مرجع الاستعلام + مقارنة مع الإصدار القديم)
  PART 2 = 20261104_PR_Menu_isEnableAndroid.sql (إضافة عمود PR_Menu.isEnableAndroid)
  PART 3 = 20261104_PR_Menu_AddRootScreens.sql  (إضافة 255 «طلبات الوارد» و256 «نتيجة المعمل» + موديولين 57/58 + صلاحيات مصطفى 354)
  PART 4 = خطوات التحقق (Verification queries)
  PART 5 = Rollback + بلوك توقيع/اعتماد

حالة التنفيذ:
  - 172.16.45.11 (WIN-272BMNKC9OI) هي الداتا بيز الحالية للتطبيق الجديد (PlantMobile).
    تم تنفيذ PART 2 وPART 3 عليها بنجاح، والتحقق موجود في PART 4 (والنتيجة كاملة قبل تنفيذها موجودة بالمشروع).
    الوضع النهائي: كل الصفوف isEnableAndroid = 0 ما عدا 255 و256 (= 1).
  - 10.7.7.225 (DESKTOP-044HRJM) هي الداتا بيز الحالية للتطبيق القديم (PlantQuar.Web).
    لم ينفذ عليها أي شيء — العمود غير موجود هناك والشاشتين غير موجودتين.
    لو أراد التطبيق القديم الالتزام بنفس الفلتر (isEnableAndroid) لازم PART 2 تنفذ هناك
    وتُعدّل الـ3 Procedures الخاصة بمنيو الويب القديم لتستثني isEnableAndroid = 1.
    انتبه: أي انتقال للقديم على 11 لازم يستنى الموافقة (لم يتغيّر Web.config).

ماذا تحتاج قبل تشغيلك أي حاجة؟
  - نسخة احتياطية / SSMS مفتوح على dbPrivilage.
  - Scripts كلها Additive (مفيش DELETE/UPDATE) ما عدا سطور Enable/EnableAll المُعلّمة في PART 2.
  - PART 2 وPART 3 Idempotent (ممكن تشغلهما تاني بأمان).

التحقق: راجع PART 4.
للتراجع: راجع PART 5 (أوامر ROLLBACK جاهزة).

Notes for the owner:
  - الـAndroid app يعرض حاليًا: الوارد ← «طلبات الوارد» (255) وتحاليل ← «نتيجة المعمل» (256) فقط.
  - لتفعيل شاشات تانية لاحقًا: UPDATE dbo.PR_Menu SET isEnableAndroid = 1 WHERE Id IN (...);
  - لتعطيلها: UPDATE ... SET isEnableAndroid = 0 WHERE Id IN (...);
  - أي تغيير محتاج إعادة تشغيل التطبيق (بيانات المنيو Cached 15 دقيقة).

===============================================
Prepared by: OpenCode
Date: 2026-11-04
