using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static ViewModels.ExportImportActivity_API;

namespace Capqwebsite.Controllers;

public class ImportRequestsController : Controller
{
    private readonly IConfiguration _configuration;

    public ImportRequestsController(IConfiguration configuration) => _configuration = configuration;

    public async Task<IActionResult> Index(DateTime? date)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !long.TryParse(HttpContext.Session.GetString("UserId"), out long userId) ||
            !long.TryParse(HttpContext.Session.GetString("EmployeeId"), out long employeeId))
            return RedirectToAction("Index", "Login");

        DateTime selectedDate = (date ?? DateTime.Today).Date;
        var requests = new List<Ex_Im_CheckRequest_GetAllByUser_DateVM>();

        try
        {
            await using var connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            await connection.OpenAsync();
            const string sql = """
                ;WITH AssignedCommittees AS
                (
                    SELECT DISTINCT
                        request.ID AS CheckRequestId,
                        request.CheckRequest_Number,
                        committee.ID AS CommitteeId,
                        committee.CommitteeType_ID,
                        type.Name_Ar AS CommitteeTypeName,
                        operation.Name_Ar AS OperationTypeName,
                        committee.Status,
                        committee.IsFinishedAll,
                        committee.Is_Start_Android,
                        committee.Is_Cancel,
                        committee.User_Updation_Date,
                        employee.ISAdmin,
                        (SELECT STRING_AGG(CONCAT(COALESCE(NULLIF(privilegeUser.FullName, ''), privilegeUser.LoginName, CONVERT(nvarchar(20), member.Employee_Id)), ' (', CASE WHEN member.ISAdmin = 1 THEN N'أدمن' ELSE N'مساعد' END, ')'), '|')
                         FROM dbo.CommitteeEmployee member
                         LEFT JOIN dbPrivilage_Test.dbo.PR_User privilegeUser ON privilegeUser.Id = member.Employee_Id
                         WHERE member.Committee_ID = committee.ID AND member.OperationType = 74 AND member.User_Deletion_Id IS NULL) AS CommitteeMembers,
                        (SELECT COUNT(*) FROM dbo.Im_CommitteeResult r WHERE r.Committee_ID = committee.ID AND r.CommitteeResultType_ID IS NOT NULL) AS ResultCount,
                        (SELECT COUNT(*) FROM dbo.Im_CommitteeResult r INNER JOIN dbo.Im_CommitteeResult_Confirm c ON c.Im_CommitteeResult_ID = r.ID AND c.EmployeeId = @UserId WHERE r.Committee_ID = committee.ID) AS ConfirmCount
                    FROM dbo.Im_CheckRequest request
                    INNER JOIN dbo.Im_RequestCommittee committee ON committee.ImCheckRequest_ID = request.ID
                    INNER JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID = committee.ID
                        AND employee.Employee_Id = @UserId
                        AND employee.OperationType = 74
                        AND employee.User_Deletion_Id IS NULL
                    INNER JOIN dbo.CommitteeType type ON type.ID = committee.CommitteeType_ID
                    LEFT JOIN dbo.Im_OpertaionType operation ON operation.ID = request.Im_OperationType
                    WHERE request.IsActive = 1
                      AND committee.IsApproved = 1
                      AND committee.User_Deletion_Id IS NULL
                      AND committee.Delegation_Date = @SelectedDate
                      AND committee.CommitteeType_ID IN (11, 13, 14)
                )
                SELECT ROW_NUMBER() OVER (ORDER BY CommitteeId) AS RowNum, *
                FROM AssignedCommittees
                ORDER BY CommitteeId
                """;

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@SelectedDate", selectedDate);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                bool status = GetBool(reader, "Status");
                bool finished = GetBool(reader, "IsFinishedAll");
                bool started = GetBool(reader, "Is_Start_Android");
                bool canceled = !IsDbNull(reader, "Is_Cancel");
                bool isAdmin = GetBool(reader, "ISAdmin");
                int resultCount = Convert.ToInt32(reader["ResultCount"]);
                int confirmCount = Convert.ToInt32(reader["ConfirmCount"]);
                string key = isAdmin
                    ? canceled ? "canceled" : (status || finished) ? "completed" : started || !IsDbNull(reader, "User_Updation_Date") ? "working" : "new"
                    : canceled ? "canceled" : resultCount > 0 && confirmCount >= resultCount ? "completed" : confirmCount > 0 ? "working" : "new";
                string name = key switch { "completed" => "تم الانتهاء", "working" => "جاري العمل", "canceled" => "ملغي", _ => "جديد" };

                requests.Add(new Ex_Im_CheckRequest_GetAllByUser_DateVM
                {
                    Row_Num = Convert.ToByte(reader["RowNum"]),
                    IsExport = 2,
                    CheckRequest_Number = Convert.ToString(reader["CheckRequest_Number"]) ?? string.Empty,
                    checkRequest_Id = Convert.ToInt64(reader["CheckRequestId"]),
                    Committee_ID = Convert.ToInt64(reader["CommitteeId"]),
                    Committee_Type_Name = $"{reader["CommitteeTypeName"]} {reader["OperationTypeName"]}".Trim(),
                    Committee_Type_Id = Convert.ToByte(reader["CommitteeType_ID"]),
                    RequestCommittee_Status = name,
                    RequestCommittee_Status_Id = status ? (byte)1 : (byte)0,
                    WorkStatusKey = key,
                    WorkStatusName = name,
                    IsReadOnly = key is "completed" or "canceled",
                    IsAdmin = isAdmin,
                    CommitteeMembers = reader["CommitteeMembers"] == DBNull.Value ? string.Empty : Convert.ToString(reader["CommitteeMembers"]) ?? string.Empty
                });
            }

            ViewBag.SelectedDate = selectedDate.ToString("yyyy-MM-dd");
            ViewBag.UserId = userId;
            ViewBag.EmployeeId = employeeId;
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
        }
        catch (Exception ex)
        {
            ViewBag.PageError = $"تعذر تحميل طلبات الوارد: {ex.Message}";
            ViewBag.SelectedDate = selectedDate.ToString("yyyy-MM-dd");
        }

        return View(requests);
    }

    private static int Ordinal(SqlDataReader reader, string name) => reader.GetOrdinal(name);
    private static bool IsDbNull(SqlDataReader reader, string name) => reader.IsDBNull(Ordinal(reader, name));
    private static bool GetBool(SqlDataReader reader, string name) => !IsDbNull(reader, name) && Convert.ToBoolean(reader[name]);
}
