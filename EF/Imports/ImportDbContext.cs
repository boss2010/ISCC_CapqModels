using System;
using System.Collections.Generic;
using EF.Imports.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF.Imports;

public partial class ImportDbContext : DbContext
{
    public ImportDbContext(DbContextOptions<ImportDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<A_AttachmentData> A_AttachmentData { get; set; }

    public virtual DbSet<AnalysisLab> AnalysisLab { get; set; }

    public virtual DbSet<AnalysisLabType> AnalysisLabType { get; set; }

    public virtual DbSet<AnalysisType> AnalysisType { get; set; }

    public virtual DbSet<CommitteeEmployee> CommitteeEmployee { get; set; }

    public virtual DbSet<CommitteeResultType> CommitteeResultType { get; set; }

    public virtual DbSet<CommitteeType> CommitteeType { get; set; }

    public virtual DbSet<Country> Country { get; set; }

    public virtual DbSet<Family> Family { get; set; }

    public virtual DbSet<Fees_Action> Fees_Action { get; set; }

    public virtual DbSet<Group> Group { get; set; }

    public virtual DbSet<Im_CheckRequest> Im_CheckRequest { get; set; }

    public virtual DbSet<Im_CheckRequest_Data> Im_CheckRequest_Data { get; set; }

    public virtual DbSet<Im_CheckRequest_Items> Im_CheckRequest_Items { get; set; }

    public virtual DbSet<Im_CheckRequest_Items_Lot_Category> Im_CheckRequest_Items_Lot_Category { get; set; }

    public virtual DbSet<Im_CheckRequest_Items_Lot_Result> Im_CheckRequest_Items_Lot_Result { get; set; }

    public virtual DbSet<Im_CheckRequest_Lot_Result_Status> Im_CheckRequest_Lot_Result_Status { get; set; }

    public virtual DbSet<Im_CheckRequest_SampleData> Im_CheckRequest_SampleData { get; set; }

    public virtual DbSet<Im_CheckRequest_SampleData_Confirm> Im_CheckRequest_SampleData_Confirm { get; set; }

    public virtual DbSet<Im_CheckRequset_Shipping_Method> Im_CheckRequset_Shipping_Method { get; set; }

    public virtual DbSet<Im_CommitteeResult> Im_CommitteeResult { get; set; }

    public virtual DbSet<Im_CommitteeResult_Confirm> Im_CommitteeResult_Confirm { get; set; }

    public virtual DbSet<Im_CommitteeResult_Infection> Im_CommitteeResult_Infection { get; set; }

    public virtual DbSet<Im_OpertaionType> Im_OpertaionType { get; set; }

    public virtual DbSet<Im_RequestCommittee> Im_RequestCommittee { get; set; }

    public virtual DbSet<Im_Request_TreatmentData> Im_Request_TreatmentData { get; set; }

    public virtual DbSet<Item> Item { get; set; }

    public virtual DbSet<Item_ShortName> Item_ShortName { get; set; }

    public virtual DbSet<Outlet> Outlet { get; set; }

    public virtual DbSet<Package_Type> Package_Type { get; set; }

    public virtual DbSet<View_List_Im_PermissionRequest> View_List_Im_PermissionRequest { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CI_AS");

        modelBuilder.Entity<A_AttachmentData>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("باص الملفات"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AttachmentPath_Binary)
                .HasMaxLength(7000)
                .IsFixedLength();
            entity.Property(e => e.Attachment_TypeName).HasComment("نوع المرفق");
            entity.Property(e => e.EndDate)
                .HasComment("تاريخ النهاية")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.StartDate)
                .HasComment("تاريخ البداية")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Date)
                .HasComment("null-> for user , value -> if the admin add the row")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Id).HasComment("null-> for user , value -> if the admin add the row");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<AnalysisLab>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("معامل التحاليل"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Addrees_En).IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fax).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone).HasColumnType("numeric(11, 0)");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<AnalysisLabType>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("جدول ربط التحاليل بالمعامل"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.AnalysisLab).WithMany(p => p.AnalysisLabType)
                .HasForeignKey(d => d.AnalysisLabID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnalysisLabType_AnalysisLab");

            entity.HasOne(d => d.AnalysisType).WithMany(p => p.AnalysisLabType)
                .HasForeignKey(d => d.AnalysisTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnalysisLabType_AnalysisType");
        });

        modelBuilder.Entity<AnalysisType>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("التحاليل التي تتم على العينة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsRejectedAll).HasComment("هل يرفض الطلب كله في حالة وجود إصابة");
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<CommitteeEmployee>(entity =>
        {
            entity.HasKey(e => new { e.Committee_ID, e.Employee_Id, e.OperationType });

            entity.ToTable(tb => tb.HasComment("أعضاء اللجنة"));

            entity.Property(e => e.OperationType).HasComment("from system code 20 (export :73 , Import 74, Farm 78) ");
            entity.Property(e => e.ISAdmin).HasComment("is admin for the current committee");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<CommitteeResultType>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_CommitteeResult");

            entity.ToTable(tb => tb.HasComment("مرفوض/ مقبول .........."));

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar).HasMaxLength(20);
            entity.Property(e => e.Name_En)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<CommitteeType>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_CommitteePurpose");

            entity.ToTable(tb => tb.HasComment("الغرض من اللجنة"));

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.type_Id).HasDefaultValue(93);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_LK_Country");

            entity.ToTable(tb => tb.HasComment("الدول"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Is_IPPC).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Family>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("العائله"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Fees_Action>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_FixedFeesAmount");

            entity.ToTable(tb => tb.HasComment("تفاصيل الاجراءات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Account_Type).HasComment("نوع الحساب من  system code رقم 33");
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.IsPaidBefore).HasComment("هل تدفع عند الطلب ام بعده");
            entity.Property(e => e.MinAmount)
                .HasComment("الحد الادنى")
                .HasColumnType("money");
            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.WeightTo).HasComment("الوزن");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("المجموعة الزراعيه"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_CheckRequest>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("طلب الفحص الوارد"));

            entity.HasIndex(e => new { e.CheckRequest_Number, e.Outlet_ID }, "IX_Im_CheckRequest_Number_Outlet");

            entity.HasIndex(e => new { e.Outlet_ID, e.User_Creation_Date }, "IX_Im_CheckRequest_Outlet_CreationDate");

            entity.HasIndex(e => e.CheckRequest_Number, "UQ_CheckRequest_Number").IsUnique();

            entity.HasIndex(e => new { e.User_Creation_Date, e.CheckRequest_Number, e.Outlet_ID, e.IsAccepted, e.ID }, "_dta_index_Im_CheckRequest_15_2006454372__K11_K3_K2_K8_K1");

            entity.HasIndex(e => new { e.ID, e.Outlet_ID }, "_dta_index_Im_CheckRequest_15_2006454372__K1_K2");

            entity.HasIndex(e => e.Outlet_ID, "_dta_index_Im_CheckRequest_15_2006454372__K2");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.CheckRequest_Number)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("رقم طلب الفحص");
            entity.Property(e => e.Completion_Request_Date).HasColumnType("datetime");
            entity.Property(e => e.Completion_Response_Date).HasColumnType("datetime");
            entity.Property(e => e.ExportCompany)
                .HasMaxLength(250)
                .HasComment("الشركة المصدرة");
            entity.Property(e => e.ExportCompanyAddress).HasComment("عنوان الشركة المصدرة");
            entity.Property(e => e.Im_OperationType).HasComment("نوع إذن الاستراد");
            entity.Property(e => e.IsAccepted_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Outlet).WithMany(p => p.Im_CheckRequest)
                .HasForeignKey(d => d.Outlet_ID)
                .HasConstraintName("FK_Im_CheckRequest_Outlet");
        });

        modelBuilder.Entity<Im_CheckRequest_Data>(entity =>
        {
            entity.HasIndex(e => e.Im_CheckRequest_ID, "IX_ImCheckRequestData_Request");

            entity.HasIndex(e => new { e.Importer_ID, e.ExportCountry_Id }, "_dta_index_Im_CheckRequest_Data_15_948406648__K2_K4_3_13");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.DelegateAddress)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DelegateName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Ship_Name)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TransitCountry_Id).HasComment("دولة العبور");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.ExportCountry).WithMany(p => p.Im_CheckRequest_Data)
                .HasForeignKey(d => d.ExportCountry_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_Data_Country");

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_CheckRequest_Data)
                .HasForeignKey(d => d.Im_CheckRequest_ID)
                .HasConstraintName("FK_Im_CheckRequest_Data_Im_CheckRequest");
        });

        modelBuilder.Entity<Im_CheckRequest_Items>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("نباتات طلب الفحص الوارد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Accept_User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Accept_User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Fees).HasColumnType("money");
            entity.Property(e => e.Fees_Actual).HasColumnType("money");
            entity.Property(e => e.GrossWeight).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.Item_Permission_Number)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Net_Weight)
                .HasComment("الوزن الصافي")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Order_Text).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Package_Weight).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CheckRequset_Shipping_Method).WithMany(p => p.Im_CheckRequest_Items)
                .HasForeignKey(d => d.Im_CheckRequset_Shipping_Method_ID)
                .HasConstraintName("FK_Im_CheckRequest_Items_IM_CheckRequset_shippingmethod");
        });

        modelBuilder.Entity<Im_CheckRequest_Items_Lot_Category>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_CheckRequest_Items_Category");

            entity.ToTable(tb => tb.HasComment("تقسيم لوط وااصناف"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Based_Weight)
                .HasComment("مش مستخدم")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.DistinctiveMark)
                .HasMaxLength(250)
                .HasComment("علامة مميزة");
            entity.Property(e => e.GrossWeight)
                .HasComment("اجمالى الوزن القائم لللوطات")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Grower_Number)
                .HasMaxLength(50)
                .HasComment("رقم المزرعه من الصادر ,وبعض الحالات من الوارد");
            entity.Property(e => e.IsAccepted).HasComment("مقبول = 1 / مرفوض =0");
            entity.Property(e => e.Lot_Number)
                .HasMaxLength(50)
                .HasComment("رقم اللوط");
            entity.Property(e => e.Net_Weight)
                .HasComment("الوزن الصافي لللوطات")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Number_Wooden_Package)
                .HasMaxLength(50)
                .HasComment("عدد وحدات التعبئه الخشبيه ");
            entity.Property(e => e.Order_Text).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Package_Based_Weight)
                .HasComment("وزن العبوة القائم")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Package_Count).HasComment("عدد العبوات");
            entity.Property(e => e.Package_Material_ID).HasComment("مادة العبوة");
            entity.Property(e => e.Package_Net_Weight)
                .HasComment("وزن العبوة الفارغ")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Package_Type_ID).HasComment("نوع العبوة");
            entity.Property(e => e.Package_Weight)
                .HasComment("الوزن العبوه الصافي")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.PackagesCount)
                .HasMaxLength(250)
                .HasComment("عدد الطرود");
            entity.Property(e => e.Reason_Entry)
                .HasComment("سبب الدخول")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.RejectReason)
                .HasMaxLength(300)
                .HasComment("اسباب الرفض");
            entity.Property(e => e.ShipName)
                .HasMaxLength(250)
                .HasComment("علامة مميزة");
            entity.Property(e => e.TripDate).HasComment("تاريخ الرحلة");
            entity.Property(e => e.Units_Number).HasComment("عدد الوحدات");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Waybill)
                .HasMaxLength(100)
                .HasComment("رقم بوليصه الشحن");
            entity.Property(e => e.containers_type_ID).HasComment("عبوات او بدون");

            entity.HasOne(d => d.Im_CheckRequest_Items).WithMany(p => p.Im_CheckRequest_Items_Lot_Category)
                .HasForeignKey(d => d.Im_CheckRequest_Items_ID)
                .HasConstraintName("FK_Im_CheckRequest_Items_Category_Im_CheckRequest_Items");
        });

        modelBuilder.Entity<Im_CheckRequest_Items_Lot_Result>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("نتيجه اللوط"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IS_Status).HasComment("الموقف مقبول او مرفوض");
            entity.Property(e => e.IS_Status_Committee).HasComment("الموقف مقبول او مرفوض");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.IS_StatusNavigation).WithMany(p => p.Im_CheckRequest_Items_Lot_Result)
                .HasForeignKey(d => d.IS_Status)
                .HasConstraintName("FK_Im_CheckRequest_Items_Lot_Result_Im_CheckRequest_Lot_Result_Status");
        });

        modelBuilder.Entity<Im_CheckRequest_Lot_Result_Status>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Is_Continue).HasComment("0 عدم استكمال الاعمال\r\nلا يمكن استكمال الاعمال 1");
        });

        modelBuilder.Entity<Im_CheckRequest_SampleData>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("نتيجه سحب عينه للوارد (admin)"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Admin_Confirmation).HasComment("موقف الحجر");
            entity.Property(e => e.Admin_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Admin_User).HasComment("ادمن الحجر");
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.Fees_Actual).HasColumnType("money");
            entity.Property(e => e.IS_From_Android)
                .HasDefaultValue(false)
                .HasComment("مين رمي row (system or android)");
            entity.Property(e => e.IS_Total).HasComment("(0) in the case of all,(1) in the case of the part   في حاله الجزئي او الكلي");
            entity.Property(e => e.IS_Total_Android).HasComment("في حاله الفحص لو كلي واتحول الي جزئي");
            entity.Property(e => e.Im_RequestCommittee_ID).HasComment("كود اساسيات اللجنه");
            entity.Property(e => e.IsAccepted).HasComment("0 if rejected else 1 \r\nموافقه المعمل");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.Item_ShortName_ID).HasComment("الاسم المختصر ");
            entity.Property(e => e.LotData_ID).HasComment("null for the whole request  /  كود بيانات الدفعه");
            entity.Property(e => e.Notes_Ar)
                .HasMaxLength(300)
                .HasComment("ملاحظات الاندريد");
            entity.Property(e => e.Notes_En)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.RejectReason_Ar)
                .HasMaxLength(150)
                .HasComment("سبب الرفض للمعمل ar\r\n");
            entity.Property(e => e.RejectReason_En)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("سبب الرفض للمعمل en");
            entity.Property(e => e.SampleQuantityUnit).HasMaxLength(10);
            entity.Property(e => e.SampleRatio).HasComment("نسبة اخذ العينة");
            entity.Property(e => e.SampleSize).HasComment("حجم العينة");
            entity.Property(e => e.Sample_BarCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("البار كود");
            entity.Property(e => e.Syl_ALkhatima_Number).HasComment("رقم الختامه والسيل الملاحي");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.WithdrawDate).HasComment("تاريخ سحب العينة");

            entity.HasOne(d => d.AnalysisLabType).WithMany(p => p.Im_CheckRequest_SampleData)
                .HasForeignKey(d => d.AnalysisLabType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_SampleData_AnalysisLabType");

            entity.HasOne(d => d.Im_RequestCommittee).WithMany(p => p.Im_CheckRequest_SampleData)
                .HasForeignKey(d => d.Im_RequestCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_SampleData_Im_RequestCommittee");
        });

        modelBuilder.Entity<Im_CheckRequest_SampleData_Confirm>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("نتيجه سحب عينه للمساعد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Im_CheckRequest_SampleData).WithMany(p => p.Im_CheckRequest_SampleData_Confirm)
                .HasForeignKey(d => d.Im_CheckRequest_SampleData_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_SampleData_Confirm_Im_CheckRequest_SampleData");
        });

        modelBuilder.Entity<Im_CheckRequset_Shipping_Method>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_IM_CheckRequset_shippingmethod");

            entity.ToTable(tb => tb.HasComment("اساليب الشحن لطلب الفحص الوارد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ContainerNumber)
                .HasMaxLength(50)
                .HasComment("رقم الحاوية");
            entity.Property(e => e.NavigationalNumber)
                .HasMaxLength(50)
                .HasComment("رقم السيل الملاحي");
            entity.Property(e => e.ShipholdNumber)
                .HasMaxLength(50)
                .HasComment("رقم عنبر السفينة");
            entity.Property(e => e.Total_Weight).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.containers_ID).HasComment("حاوية او صب");
            entity.Property(e => e.containers_type_ID).HasComment("عبوات او بدون");

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_CheckRequset_Shipping_Method)
                .HasForeignKey(d => d.Im_CheckRequest_ID)
                .HasConstraintName("FK_IM_CheckRequset_shippingmethod_Im_CheckRequest");
        });

        modelBuilder.Entity<Im_CommitteeResult>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("النتيجه"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.AdminFinalResult_Note).HasComment("Admin Note");
            entity.Property(e => e.Committee_ID).HasComment("كود اساسيات اللجنه");
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IS_Total).HasComment("(0) in the case of all,(1) in the case of the part   في حاله الجزئي او الكلي");
            entity.Property(e => e.IS_Total_Android).HasComment("في حاله الفحص لو كلي واتحول الي جزئي");
            entity.Property(e => e.IsAdminResult).HasComment("null->exporter not take action 0 if exporter doesn't accept else 1");
            entity.Property(e => e.Item_ShortName_ID).HasComment("الاسم المختصر");
            entity.Property(e => e.LotData_ID).HasComment("null for the whole request  /  كود بيانات الدفعه");
            entity.Property(e => e.QuantitySize).HasComment("العدد");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Weight).HasComment("الوزن");

            entity.HasOne(d => d.CommitteeResultType).WithMany(p => p.Im_CommitteeResult)
                .HasForeignKey(d => d.CommitteeResultType_ID)
                .HasConstraintName("FK_Im_CommitteeResult_CommitteeResultType");

            entity.HasOne(d => d.Committee).WithMany(p => p.Im_CommitteeResult)
                .HasForeignKey(d => d.Committee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CommitteeResult_Im_RequestCommittee");

            entity.HasOne(d => d.Item_ShortName).WithMany(p => p.Im_CommitteeResult)
                .HasForeignKey(d => d.Item_ShortName_ID)
                .HasConstraintName("FK_Im_CommitteeResult_Item_ShortName");
        });

        modelBuilder.Entity<Im_CommitteeResult_Confirm>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("راي المساعد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);

            entity.HasOne(d => d.Im_CommitteeResult).WithMany(p => p.Im_CommitteeResult_Confirm)
                .HasForeignKey(d => d.Im_CommitteeResult_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CommitteeResult_Confirm_Im_CommitteeResult");
        });

        modelBuilder.Entity<Im_CommitteeResult_Infection>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("الاصابه للفحص"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CommitteeResult).WithMany(p => p.Im_CommitteeResult_Infection)
                .HasForeignKey(d => d.Im_CommitteeResult_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CommitteeResult_Infection_Im_CommitteeResult");

            entity.HasOne(d => d.Item).WithMany(p => p.Im_CommitteeResult_Infection)
                .HasForeignKey(d => d.Item_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CommitteeResult_Infection_Item");
        });

        modelBuilder.Entity<Im_OpertaionType>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_ImportOpertaionType");

            entity.ToTable(tb => tb.HasComment("أنواع عمليات الوارد"));

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.withPermission).HasComment("1 = permission request / 2 = check request / 3 = both permission and check request");
        });

        modelBuilder.Entity<Im_RequestCommittee>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("اساسيات اللجان"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.CommitteeType_ID).HasComment("كود الغرض من اللجنه");
            entity.Property(e => e.Delegation_Date).HasComment("تاريخ الانتداب");
            entity.Property(e => e.EndTime).HasComment("انتهاء ساعة الفحص");
            entity.Property(e => e.ImCommitteeCheckLocation_ID).HasComment("كود اماكن الفحص");
            entity.Property(e => e.IsApproved)
                .HasDefaultValue(false)
                .HasComment("0 if exporter doesn't accept else 1 خاص ب نتيجه الفحص");
            entity.Property(e => e.IsFinishedAll)
                .HasDefaultValue(false)
                .HasComment("0 if exporter doesn't accept else 1 خاص ب شغل موظف الحجر في فحص الشحنه");
            entity.Property(e => e.IsPaid)
                .HasDefaultValue(false)
                .HasComment("تم الانتهاء من الدفع");
            entity.Property(e => e.Is_Cancel).HasComment("لايقاف او حذف اللجنة مربوط مع  A_SystemCode رقم 31\r\n");
            entity.Property(e => e.Is_Start_Android).HasComment("تعزر عمل اللجنه");
            entity.Property(e => e.StartTime).HasComment(" بداية ساعة الفحص ");
            entity.Property(e => e.Status)
                .HasDefaultValue(false)
                .HasComment("0 if not done, 1 if investigation is done تم نزول اللجنه 1 ,0 وافق علي معاد اللجنه , null خاص ب العميل عدم الرد عل معاد اللجنه , ");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CommitteeType).WithMany(p => p.Im_RequestCommittee)
                .HasForeignKey(d => d.CommitteeType_ID)
                .HasConstraintName("FK_Im_RequestCommittee_CommitteeType");

            entity.HasOne(d => d.ImCheckRequest).WithMany(p => p.Im_RequestCommittee)
                .HasForeignKey(d => d.ImCheckRequest_ID)
                .HasConstraintName("FK_Im_RequestCommittee_Im_CheckRequest");
        });

        modelBuilder.Entity<Im_Request_TreatmentData>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.Company_ID).HasComment("شركة المعالجة");
            entity.Property(e => e.Fees_Actual).HasColumnType("money");
            entity.Property(e => e.IS_From_Android)
                .HasDefaultValue(false)
                .HasComment("مين رمي row (system or android)");
            entity.Property(e => e.IS_Total).HasComment("(0) in the case of all,(1) in the case of the part في حاله الجزئي او الكلي");
            entity.Property(e => e.IS_Total_Android).HasComment("في حاله الفحص لو كلي واتحول الي جزئي");
            entity.Property(e => e.Im_RequestCommittee_ID).HasComment("لجنة المعالجة");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.Item_ShortName_ID).HasComment("الاسم المختصر ");
            entity.Property(e => e.Size)
                .HasComment("حجم الرسالة (متر مكعب / سم مكعب)")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Station_ID).HasComment("مكان المعالجة(محطة معتمدة)");
            entity.Property(e => e.Temperature)
                .HasComment("درجة الحرارة")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TheDose)
                .HasComment("الجرعة")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.ThermalSealNumber)
                .HasComment("رقم الختم الحراري")
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.TreatmentMat_Amount)
                .HasComment("كمية المادة المستخدمة في المعالجة")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.TreatmentMat_ID).HasComment("مادة المعالجة");
            entity.Property(e => e.TreatmentMethod_ID).HasComment("طريقة المعالجة");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_RequestCommittee).WithMany(p => p.Im_Request_TreatmentData)
                .HasForeignKey(d => d.Im_RequestCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_Request_TreatmentData_Im_RequestCommittee");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Plant");

            entity.ToTable(tb => tb.HasComment("النبات و المنتجات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Descreption_Ar).HasComment("وصف أو تنويه");
            entity.Property(e => e.Descreption_En)
                .IsUnicode(false)
                .HasComment("وصف أو تنويه");
            entity.Property(e => e.IsForbidden).HasComment("0 مسموح به\r\n1 ممنوع \r\n");
            entity.Property(e => e.IsPermissionRequest)
                .HasDefaultValue(false)
                .HasComment("هل له اذن استيراد -خاص بالوارد");
            entity.Property(e => e.Is_known_item).HasComment("معروف وغير معروف");
            entity.Property(e => e.Item_Code).HasMaxLength(3);
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(200)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(200)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.Picture).IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Family).WithMany(p => p.Item)
                .HasForeignKey(d => d.Family_ID)
                .HasConstraintName("FK_Plant_Family");

            entity.HasOne(d => d.Group).WithMany(p => p.Item)
                .HasForeignKey(d => d.Group_ID)
                .HasConstraintName("FK_Plant_Group");
        });

        modelBuilder.Entity<Item_ShortName>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Plant_ShortName");

            entity.ToTable(tb => tb.HasComment("المسمى المختصر"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ExportStatus).HasComment("الموقف من التصدير");
            entity.Property(e => e.IS_ShortName).HasComment("له اسم مختصر ام لا");
            entity.Property(e => e.ImportStatus).HasComment("الموقف من الاستيراد");
            entity.Property(e => e.Is_ImportTaxFree)
                .HasDefaultValue(false)
                .HasComment("معفي من اذن الاستيراد");
            entity.Property(e => e.Item_ID).HasComment("الصنف");
            entity.Property(e => e.Item_Purpose_ID).HasComment("الغرض");
            entity.Property(e => e.Item_Status_ID).HasComment("الحالة");
            entity.Property(e => e.Product_ID).HasComment("المنتج");
            entity.Property(e => e.QualitativeGroup_Id).HasComment("مجموعة نوعية");
            entity.Property(e => e.Reason).HasComment("سبب الايقاف");
            entity.Property(e => e.ShortName_Ar).HasComment("الاسم العربى");
            entity.Property(e => e.ShortName_En)
                .IsUnicode(false)
                .HasComment("الاسم الاجنبى");
            entity.Property(e => e.SubPart_ID).HasComment("الجزء النباتى او الطور الحيوى");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Item).WithMany(p => p.Item_ShortName)
                .HasForeignKey(d => d.Item_ID)
                .HasConstraintName("FK_Plant_ShortName_Plant");
        });

        modelBuilder.Entity<Outlet>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("المنفذ"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address_Ar).HasComment("العنوان بالعربية");
            entity.Property(e => e.Address_En)
                .IsUnicode(false)
                .HasComment("العنوان بالانجليزية");
            entity.Property(e => e.Ar_Name).HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.GrAdmin_ID).HasComment("الادارة العامة");
            entity.Property(e => e.ID_HR).HasComment("رقم المنفذ بالنسبة لل HR");
            entity.Property(e => e.IsDisplay)
                .HasDefaultValue((byte)1)
                .HasComment("from system code 21\r\nصادر	/وارد	/صادر+ وارد");
            entity.Property(e => e.IsExport).HasComment("from system code 21\r\nصادر	/وارد	/صادر+ وارد");
            entity.Property(e => e.Supervisor_ID).HasComment("مشرف الفرع\r\nFrom HR employee table");
        });

        modelBuilder.Entity<Package_Type>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("نوع العبوة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<View_List_Im_PermissionRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_List_Im_PermissionRequest");

            entity.Property(e => e.ExportCountryName).HasMaxLength(100);
            entity.Property(e => e.ImPermission_Number).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.ImporterTypeName).HasMaxLength(150);
            entity.Property(e => e.operationTypeName).HasMaxLength(50);
        });
        modelBuilder.HasSequence("A__plant_Error_Save_SEQ")
            .StartsAt(15031L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A__User_Login_SEQ")
            .StartsAt(327878L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A_AttachmentData_Ex_CheckRequest_SEQ")
            .StartsAt(480L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A_AttachmentData_Ex_CommitteeResult_Infection_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A_AttachmentData_Im_CommitteeResult_Infection_SEQ")
            .StartsAt(21621L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A_AttachmentData_SEQ")
            .StartsAt(672638L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A_AttachmentData_Station_SEQ")
            .StartsAt(2387L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("A_AttachmentTableName_SEQ")
            .StartsAt(27L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("A_AttachmentTableType_SEQ")
            .StartsAt(31L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("A_SystemCode_SEQ")
            .StartsAt(142L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("A_SystemCodeType_SEQ")
            .StartsAt(34L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("AnalysisLab_SEQ")
            .StartsAt(13L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("AnalysisLabType_SEQ")
            .StartsAt(23L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("AnalysisType_SEQ")
            .StartsAt(22L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Andriod_Location_SEQ")
            .StartsAt(442871L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Andriod_Operation_SEQ")
            .StartsAt(23L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Biological_Phase_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Center_SEQ")
            .StartsAt(324L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("CommitteeEmployee_SEQ")
            .StartsAt(155218L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("CommitteeResultType_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("CommitteeType_SEQ")
            .StartsAt(15L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Company_National_SEQ")
            .StartsAt(32288L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("CompanyAccreditation_Committee_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("CompanyAccreditation_Payment_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("CompanyAccreditation_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("CompanyActivity_SEQ")
            .StartsAt(35829L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("CompanyActivityType_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("ContactType_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Continents_SEQ")
            .StartsAt(12L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Country_SEQ")
            .StartsAt(235L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Enrollment_type_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificateAddtion_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificateAddtionUser_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_CertificatesNewCountry_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificatesRequests_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificatesRequestsFiles_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificatesRequestsLotData_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificatesRequestsPayments_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificatesRequestsPaymentsDetailes_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Ex_CertificatesRequestsPaymentsType_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_CheckRequest_Customs_Message_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Data_SEQ")
            .StartsAt(22609L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Fees_SEQ")
            .StartsAt(22214L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Final_Result_SEQ")
            .StartsAt(24567L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Items_Lot_Category_SEQ")
            .StartsAt(83664L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Items_Lot_Result_SEQ")
            .StartsAt(76995L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Items_SEQ")
            .StartsAt(46667L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_CheckRequest_Lot_Result_Status_SEQ")
            .StartsAt(12L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Organization_Distribution_Detials_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Organization_Distribution_Master_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Organization_Distribution_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Places_SEQ")
            .StartsAt(22611L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_CheckRequest_Port_SEQ")
            .StartsAt(54563L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_RefuseReason_SEQ")
            .StartsAt(180L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_SampleData_Confirm_SEQ")
            .StartsAt(954L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_SampleData_SEQ")
            .StartsAt(2330L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_SEQ")
            .StartsAt(22614L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Visa_SEQ")
            .StartsAt(21281L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequestData_Extra_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequset_Shipping_Method_SEQ")
            .StartsAt(38918L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("EX_Choose_SampleData_SEQ")
            .StartsAt(2067L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("EX_Choose_Treatment_SEQ")
            .StartsAt(262L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Ex_CommitteeCheckLocation_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CommitteeResult_Confirm_SEQ")
            .StartsAt(34054L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CommitteeResult_Infection_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CommitteeResult_SEQ")
            .StartsAt(91259L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("EX_Constrain_Country_Item_SEQ")
            .StartsAt(54L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("EX_Constrain_Text_SEQ")
            .StartsAt(185L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("EX_Constrain_Type_SEQ")
            .StartsAt(13L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_ContactData_SEQ")
            .StartsAt(77423L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CountryConstrain_AnalysisLabType_SEQ")
            .StartsAt(154L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CountryConstrain_ArrivalPort_SEQ")
            .StartsAt(5L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CountryConstrain_SEQ")
            .StartsAt(1596L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CountryConstrain_Text_SEQ")
            .StartsAt(5568L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CountryConstrain_Treatment_SEQ")
            .StartsAt(245L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("EX_Fees_Type_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_Final_Result_SEQ")
            .StartsAt(19L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_OpertaionType_SEQ")
            .StartsAt(4L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_Request_TreatmentData_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_Request_TreatmentData_SEQ")
            .StartsAt(25L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_RequestCommittee_Fees_ENG_SEQ")
            .StartsAt(6813L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_RequestCommittee_SEQ")
            .StartsAt(37424L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_RequestCommittee_Shift_SEQ")
            .StartsAt(22811L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_Visa_SEQ")
            .StartsAt(3L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Family_SEQ")
            .StartsAt(2507L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_CheckList_SEQ")
            .StartsAt(24L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_CheckList_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_CheckList_SEQ")
            .StartsAt(11761L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_Constrain_SEQ")
            .StartsAt(16196L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_Examination_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_Examination_SEQ")
            .StartsAt(71L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_Final_Result_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_SEQ")
            .StartsAt(57L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_Shift_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Company_SEQ")
            .StartsAt(403L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Constrain_SEQ")
            .StartsAt(3337L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Constrain_Text_SEQ")
            .StartsAt(27L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Country_CheckList_SEQ")
            .StartsAt(1975L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Country_SEQ")
            .StartsAt(33162L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Farm_Fees_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_ItemCategories_SEQ")
            .StartsAt(608L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Request_ItemCategories_SEQ")
            .StartsAt(500L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Request_Refuse_Reason_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Request_SEQ")
            .StartsAt(436L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Farm_Request_Type_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_SampleData_Confirm_Item_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_SampleData_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_SampleData_Item_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_SampleData_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farms_Organization_Distribution_Detials_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farms_Organization_Distribution_Master_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("FarmsData_SEQ")
            .StartsAt(349L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("FarmStop_SEQ")
            .StartsAt(194L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Action_SEQ")
            .StartsAt(2518L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Altahsil_Detiles_SEQ")
            .StartsAt(90L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Altahsil_SEQ")
            .StartsAt(56L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Certificates_Payment_Detiles_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Fees_Money_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Fees_process_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Fees_TableName_SEQ")
            .StartsAt(11L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Transactions_Detiles_SEQ")
            .StartsAt(304530L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Transactions_Payment_Detiles_SEQ")
            .StartsAt(4664L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Transactions_SEQ")
            .StartsAt(266484L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Fees_Type_Action_SEQ")
            .StartsAt(14L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("FeesAmount_Fixed_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("FeesType_SEQ")
            .StartsAt(36L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("FreeZone_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("FumigationUnit_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Gas_ImportCompany_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("General_Admin_SEQ")
            .StartsAt(63L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Governate_SEQ")
            .StartsAt(36L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Group_SEQ")
            .StartsAt(100L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("HagrContact_SEQ")
            .StartsAt(11L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_CheckRequest_Customs_Message_SEQ")
            .StartsAt(319957L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Data_SEQ")
            .StartsAt(325993L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Distribution_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Final_Result_SEQ")
            .StartsAt(128485L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Items_Lot_Category_SEQ")
            .StartsAt(627556L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Items_Lot_Result_SEQ")
            .StartsAt(161452L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Items_SEQ")
            .StartsAt(577898L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_CheckRequest_Lot_Result_Status_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Manafest_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_CheckRequest_Port_SEQ")
            .StartsAt(669453L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_RefuseReason_SEQ")
            .StartsAt(49002L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_SampleData_Confirm_SEQ")
            .StartsAt(10438L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_SampleData_SEQ")
            .StartsAt(14625L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_SEQ")
            .StartsAt(326067L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Visa_SEQ")
            .StartsAt(122089L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequestData_Extra_SEQ")
            .StartsAt(322593L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequset_Shipping_Method_SEQ")
            .StartsAt(535065L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_Committee_CustodyPlace_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_CommitteeCheckLocation_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CommitteeResult_Confirm_SEQ")
            .StartsAt(34768L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CommitteeResult_Infection_SEQ")
            .StartsAt(72L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CommitteeResult_SEQ")
            .StartsAt(206132L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Constrain_Initiator_Text_SEQ")
            .StartsAt(73225L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_Constrain_Type_SEQ")
            .StartsAt(71L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Constrains_Special_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CountryConstrain_ArrivalPort_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CountryConstrain_Text_SEQ")
            .StartsAt(167L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CustodyPlace_CheckRequest_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CustodyPlace_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_CustodyPlaceType_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Execution_Items_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Execution_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_Final_Result_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Fumigation_Distribution_Inspection_SEQ");
        modelBuilder.HasSequence("Im_Fumigation_Distribution_Result_SEQ");
        modelBuilder.HasSequence("Im_Fumigation_Distribution_SEQ");
        modelBuilder.HasSequence("Im_Fumigation_Release_Request_SEQ");
        modelBuilder.HasSequence("Im_Initiator_SEQ")
            .StartsAt(129703L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ItemsLotDivision_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Manafest_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_OpertaionType_SEQ")
            .StartsAt(17L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionItem_Division_Custody_DismissCommittee_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionItem_Division_Custody_ReceiveCommittee_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionItem_Division_Custody_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionItems_Category_SEQ")
            .StartsAt(4972L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionItems_SEQ")
            .StartsAt(465534L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionRequest_RefuseReason_SEQ")
            .StartsAt(2280L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionRequest_SEQ")
            .StartsAt(486170L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_ProcedureType_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_Request_Port_SEQ")
            .StartsAt(930648L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Request_TreatmentData_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Request_TreatmentData_SEQ")
            .StartsAt(11391L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_RequestCommittee_Procedure_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_RequestCommittee_SEQ")
            .StartsAt(155243L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_RequestCommittee_Shift_SEQ")
            .StartsAt(162073L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_RequestDat_Extra_SEQ")
            .StartsAt(465676L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_RequestData_SEQ")
            .StartsAt(465550L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ScientificResearch_ItemPlant_Inseket_Lieble_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ScientificResearch_ItemPlant_Product_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ScientificResearch_Organization_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ScientificResearch_Person_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ScientificResearch_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_Stores_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_SubDivission_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_TransUnderCustodyReason_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Visa_SEQ")
            .StartsAt(11L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_Warehouses_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("InternationalTransportation_SEQ")
            .StartsAt(9639L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Item_Purpose_SEQ")
            .StartsAt(39L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Item_SEQ")
            .StartsAt(3291L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Item_ShortName_SEQ")
            .StartsAt(2126L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Item_Status_SEQ")
            .StartsAt(66L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Item_Type_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("ItemCategories_Group_SEQ")
            .StartsAt(170L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("ItemCategories_SEQ")
            .StartsAt(9197L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("ItemCategories_Type_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("ItemPart_SEQ")
            .StartsAt(2764L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Kingdom_SEQ")
            .StartsAt(16L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Level_SEQ")
            .StartsAt(12L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("LiableItems_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("LiableItems_ShortName_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("LiableItems_Status_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("MainCalssification_SEQ")
            .StartsAt(23L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Order_SEQ")
            .StartsAt(447L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Outlet_Employee_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Outlet_SEQ")
            .StartsAt(245L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Package_Material_SEQ")
            .StartsAt(12L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Package_Type_SEQ")
            .StartsAt(21L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Pallet_Data_Ex_CheckRequest_Distribution_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Pallet_Data_Organization__Distribution_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Person_SEQ")
            .StartsAt(1237L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PhylumSubphylum_SEQ")
            .StartsAt(74L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Port_International_SEQ")
            .StartsAt(22930L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Port_Type_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PortNational_SEQ")
            .StartsAt(35L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PortOrganization_SEQ")
            .StartsAt(5L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("pos_information_SEQ")
            .StartsAt(32L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Public_Organization_SEQ")
            .StartsAt(705L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PublicOrganization_Type_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("QualitativeGroup_SEQ")
            .StartsAt(60L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Refuse_Reason_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Regional_Area_SEQ")
            .StartsAt(27L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Regions_SEQ")
            .StartsAt(59L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("SecondaryClassification_SEQ")
            .StartsAt(38L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("ShiftTiming_SEQ")
            .StartsAt(10L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Shipment_Mean_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("ShippingAgencies_SEQ")
            .StartsAt(484L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("ShippingCompanies_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_CheckList_SEQ")
            .StartsAt(472L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_CheckList_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_CheckList_SEQ")
            .StartsAt(22758L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_Final_Result_SEQ")
            .StartsAt(813L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_Imge_SEQ")
            .StartsAt(903L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_SEQ")
            .StartsAt(1244L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_Shift_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Data_Country_SEQ")
            .StartsAt(4340L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Data_Item_ShortName_SEQ")
            .StartsAt(85L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Data_SEQ")
            .StartsAt(39L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Request_Fees_ENG_SEQ")
            .StartsAt(5L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Request_Fees_SEQ")
            .StartsAt(904L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Request_SEQ")
            .StartsAt(1376L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Station_Accreditation_Request_Type_SEQ")
            .StartsAt(4L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_SEQ")
            .StartsAt(600L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_CheckList_SEQ")
            .StartsAt(164L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Station_Constrain_Country_Item_SEQ")
            .StartsAt(17L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Station_Constrain_Type_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Emp_SEQ")
            .StartsAt(2240L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Station_Fees_Type_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Managing_Director_SEQ")
            .StartsAt(995L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_SEQ")
            .StartsAt(584L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("StationActivityType_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("StationCompany_SEQ")
            .StartsAt(618L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("StationContacts_SEQ")
            .StartsAt(670L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("SteamingCompany_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("SubPart_SEQ")
            .StartsAt(127L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("SubPart_Type_SEQ")
            .StartsAt(16L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Table_Action_Log_CheckRequest_SEQ")
            .StartsAt(1818521L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Table_Action_Log_EX_SEQ")
            .StartsAt(18628L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Table_Action_Log_Farm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Table_Action_Log_SEQ")
            .StartsAt(815763L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Table_Action_Log_Station_SEQ")
            .StartsAt(2664L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Table_Action_SEQ")
            .StartsAt(65L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Transport_Mean_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("TreatmentMainType_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("TreatmentMaterial_SEQ")
            .StartsAt(15L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("TreatmentMethods_SEQ")
            .StartsAt(14L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("TreatmentType_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Union_Country_SEQ")
            .StartsAt(92L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Union_SEQ")
            .StartsAt(11L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("UnitType_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Village_SEQ")
            .StartsAt(4411L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Websitetype_SEQ")
            .StartsAt(17L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("WebsiteTypeDetail_SEQ")
            .StartsAt(271L)
            .HasMin(1L)
            .IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
