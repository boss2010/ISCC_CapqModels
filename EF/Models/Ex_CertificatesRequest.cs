using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// بيانات الشهادة
/// </summary>
public partial class Ex_CertificatesRequest
{
    public long ID { get; set; }

    public long? Ex_CheckRequest_ID { get; set; }

    /// <summary>
    /// رقم بوليصة الشحن
    /// </summary>
    public string? ShippingPolicyNumber { get; set; }

    public string? ShippingCompanyName { get; set; }

    /// <summary>
    ///  وسيلة الشحن
    /// </summary>
    public long? ShipmentmeanID { get; set; }

    public DateOnly? ShippingDate { get; set; }

    public string? ShippingAgency { get; set; }

    /// <summary>
    /// رقم الشركة
    /// </summary>
    public long Importer_Id { get; set; }

    public int ExporterId { get; set; }

    public long ExporterTypeID { get; set; }

    public string? Details { get; set; }

    public string? CertificateNumber { get; set; }

    public int CertificateNo { get; set; }

    public int ArrivePortID { get; set; }

    public bool? ISAccepted { get; set; }

    public bool ISPrint { get; set; }

    /// <summary>
    /// لغة الشهادة
    /// </summary>
    public bool? ISEnglish { get; set; }

    public long? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// لوط
    /// </summary>
    public bool? IS_Lot { get; set; }

    /// <summary>
    /// حاويات
    /// </summary>
    public bool? IS_Containers { get; set; }

    /// <summary>
    /// معالجه
    /// </summary>
    public bool? IS_Treatment { get; set; }

    public bool? IS_Additional_Declaretion { get; set; }

    public bool? IsPaid { get; set; }

    /// <summary>
    /// طــريقة النقل
    /// </summary>
    public byte? Transport_Mean_Id { get; set; }

    /// <summary>
    /// تفاصيل أو رقم الرحلة
    /// </summary>
    public string? Ship_Name { get; set; }

    /// <summary>
    /// وسـائل النقل الدولية
    /// </summary>
    public long? InternationalTransportation_ID { get; set; }

    /// <summary>
    /// أسم الشركة المستوردة باللغه الأنجيلزية
    /// </summary>
    public string? ImportCompany_Name_EN { get; set; }

    /// <summary>
    /// عنوان الشركة المستوردة باللغه الأنجيلزية
    /// </summary>
    public string? ImporeterCompanyAddress_EN { get; set; }

    /// <summary>
    /// أسم الشركة المستوردة باللغه العربية
    /// </summary>
    public string? ImportCompany_Name_AR { get; set; }

    /// <summary>
    /// عنوان الشركة المستوردة باللغه العربية
    /// </summary>
    public string? ImporeterCompanyAddress_AR { get; set; }

    /// <summary>
    /// ميناء الشحن
    /// </summary>
    public int? Shipping_Port { get; set; }

    /// <summary>
    /// الدولة المستوردة
    /// </summary>
    public short Importing_Country { get; set; }

    /// <summary>
    /// نوع الميناء الدوله المستورده
    /// </summary>
    public byte Port_Type_Importing_Country { get; set; }

    /// <summary>
    /// ميناء الوصول
    /// </summary>
    public int? Port_Access { get; set; }

    /// <summary>
    /// دولة العبور
    /// </summary>
    public short? Transit_Country { get; set; }

    /// <summary>
    /// نوع الميناء دوله العبور
    /// </summary>
    public byte? Port_Type_Transit_Country { get; set; }

    /// <summary>
    /// ميناء العبور
    /// </summary>
    public int? Transit_Port { get; set; }

    public string? RejectReason { get; set; }

    public virtual ICollection<Ex_CertificateAddtionUser> Ex_CertificateAddtionUsers { get; set; } = new List<Ex_CertificateAddtionUser>();

    public virtual ICollection<Ex_CertificateAddtion> Ex_CertificateAddtions { get; set; } = new List<Ex_CertificateAddtion>();

    public virtual ICollection<Ex_CertificatesRequestsFile> Ex_CertificatesRequestsFiles { get; set; } = new List<Ex_CertificatesRequestsFile>();

    public virtual ICollection<Ex_CertificatesRequestsLotDatum> Ex_CertificatesRequestsLotData { get; set; } = new List<Ex_CertificatesRequestsLotDatum>();

    public virtual ICollection<Ex_CertificatesRequestsPayment> Ex_CertificatesRequestsPayments { get; set; } = new List<Ex_CertificatesRequestsPayment>();

    public virtual Ex_CheckRequest? Ex_CheckRequest { get; set; }

    public virtual ICollection<Ex_CheckRequest_Customs_Message> Ex_CheckRequest_Customs_Messages { get; set; } = new List<Ex_CheckRequest_Customs_Message>();

    public virtual ICollection<Fees_Certificates_Payment_Detile> Fees_Certificates_Payment_Detiles { get; set; } = new List<Fees_Certificates_Payment_Detile>();
}
