namespace PlantMobile.Data;

// Typed result shared by the EF reader and the legacy/EF parity test.
public sealed class ImportCommitteeRow
{
    public long RowNum { get; set; }
    public long CheckRequestId { get; set; }
    public string? CheckRequest_Number { get; set; }
    public long CommitteeId { get; set; }
    public byte CommitteeType_ID { get; set; }
    public string? CommitteeTypeName { get; set; }
    public string? OperationTypeName { get; set; }
    public bool? Status { get; set; }
    public bool? IsFinishedAll { get; set; }
    public bool? Is_Start_Android { get; set; }
    public byte? Is_Cancel { get; set; }
    public DateTime? User_Updation_Date { get; set; }
    public bool ISAdmin { get; set; }
    public string CompanyName { get; set; } = "غير محدد";
    public bool RequestQuarantineComplete { get; set; }
    public bool HasPendingQuarantine { get; set; }
    public string SampleBarcodes { get; set; } = "";
    public string CommitteeMembers { get; set; } = "";
    public int ResultCount { get; set; }
    public int ConfirmCount { get; set; }
    public int SampleCount { get; set; }
    public int SampleConfirmCount { get; set; }
    public bool SampleAttachmentUploaded { get; set; }
    public bool HasPendingSampleFinalResult { get; set; }
    public bool HasPendingLabResult { get; set; }
    public bool InspectionComplete { get; set; }
    public bool WithdrawalComplete { get; set; }
    public bool CanShowLaboratoryResult { get; set; }
    public string PendingLabBarcodes { get; set; } = "";
}
