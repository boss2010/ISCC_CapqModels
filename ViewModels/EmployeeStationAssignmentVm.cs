namespace ViewModels;

public class EmployeeStationAssignmentPageVm
{
    public long? OutletId { get; set; }
    public DateTime FromDate { get; set; } = DateTime.Today;
    public DateTime ToDate { get; set; } = DateTime.Today;
    public List<EmployeeStationLookupVm> Outlets { get; set; } = [];
    public List<EmployeeStationLookupVm> Stations { get; set; } = [];
    public List<EmployeeStationAssignmentRowVm> Assignments { get; set; } = [];
}

public class EmployeeStationLookupVm
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class EmployeeStationAssignmentRowVm
{
    public long Id { get; set; }
    public long OutletId { get; set; }
    public string OutletName { get; set; } = string.Empty;
    public long StationId { get; set; }
    public string StationName { get; set; } = string.Empty;
    public short UserId { get; set; }
    public long? EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}
