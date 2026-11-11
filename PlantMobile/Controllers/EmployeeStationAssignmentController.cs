using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ViewModels;

namespace PlantMobile.Controllers;

[Route("Employee/StationAssignment")]
public class EmployeeStationAssignmentController : Controller
{
    private readonly string _db;
    private readonly string _privilegeDb;

    public EmployeeStationAssignmentController(IConfiguration configuration)
    {
        _db=configuration.GetConnectionString("DBConnection")??throw new InvalidOperationException("DBConnection is missing.");
        _privilegeDb=configuration.GetConnectionString("PrivilegeDBConnection")??throw new InvalidOperationException("PrivilegeDBConnection is missing.");
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(long? outletId,CancellationToken ct)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated") return RedirectToAction("Index","Login");
        var model=new EmployeeStationAssignmentPageVm{OutletId=outletId};
        await using var connection=new SqlConnection(_db);await connection.OpenAsync(ct);
        await using var command=new SqlCommand("""
            SELECT ID,COALESCE(NULLIF(Ar_Name,N''),En_Name) Name FROM dbo.Outlet WHERE IsActive=1 AND User_Deletion_Id IS NULL ORDER BY Ar_Name;
            SELECT ID,COALESCE(NULLIF(Ar_Name,N''),En_Name) Name FROM dbo.Station WHERE IsActive=1 AND User_Deletion_Id IS NULL ORDER BY Ar_Name;
            SELECT a.Id,a.Station_Id,s.Ar_Name StationName,a.Emp_Id PR_User_ID,u.EmpId Employee_ID,u.FullName Employee_Name,
              a.Date_From From_Date,a.Date_To To_Date,o.ID Outlet_ID,o.Ar_Name OutletName
            FROM dbo.Station_Emp a INNER JOIN dbo.Station s ON s.ID=a.Station_Id
            INNER JOIN dbPrivilage.dbo.PR_User u ON u.Id=a.Emp_Id
            LEFT JOIN dbo.Outlet o ON o.ID_HR=u.Outlet_ID
            WHERE a.IsActive=1 AND a.User_Deletion_Id IS NULL AND (@OutletId IS NULL OR o.ID=@OutletId)
            ORDER BY a.Date_From DESC,a.Id DESC;
            """,connection);
        command.Parameters.Add("@OutletId",SqlDbType.BigInt).Value=outletId??(object)DBNull.Value;
        await using var reader=await command.ExecuteReaderAsync(ct);
        while(await reader.ReadAsync(ct))model.Outlets.Add(new(){Id=Convert.ToInt64(reader[0]),Text=Convert.ToString(reader[1])??string.Empty});
        await reader.NextResultAsync(ct);while(await reader.ReadAsync(ct))model.Stations.Add(new(){Id=Convert.ToInt64(reader[0]),Text=Convert.ToString(reader[1])??string.Empty});
        await reader.NextResultAsync(ct);while(await reader.ReadAsync(ct))model.Assignments.Add(new(){Id=Convert.ToInt64(reader["ID"]),OutletId=reader["Outlet_ID"] is DBNull?0:Convert.ToInt64(reader["Outlet_ID"]),OutletName=Convert.ToString(reader["OutletName"])??string.Empty,StationId=Convert.ToInt64(reader["Station_ID"]),StationName=Convert.ToString(reader["StationName"])??string.Empty,UserId=Convert.ToInt16(reader["PR_User_ID"]),EmployeeId=reader["Employee_ID"] is DBNull?null:Convert.ToInt64(reader["Employee_ID"]),EmployeeName=Convert.ToString(reader["Employee_Name"])??string.Empty,FromDate=reader["From_Date"] is DBNull?DateTime.MinValue:Convert.ToDateTime(reader["From_Date"]),ToDate=reader["To_Date"] is DBNull?DateTime.MaxValue:Convert.ToDateTime(reader["To_Date"])});
        return View(model);
    }

    [HttpGet("Employees")]
    public async Task<IActionResult> Employees(long outletId,CancellationToken ct)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated")return Unauthorized();
        long? hrId;
        await using(var db=new SqlConnection(_db)){await db.OpenAsync(ct);await using var command=new SqlCommand("SELECT ID_HR FROM dbo.Outlet WHERE ID=@Id AND IsActive=1",db);command.Parameters.Add("@Id",SqlDbType.BigInt).Value=outletId;var value=await command.ExecuteScalarAsync(ct);hrId=value is null or DBNull?null:Convert.ToInt64(value);}
        if(!hrId.HasValue)return Json(Array.Empty<object>());
        var employees=new List<object>();await using var connection=new SqlConnection(_privilegeDb);await connection.OpenAsync(ct);
        await using var employeeCommand=new SqlCommand("SELECT Id,EmpId,FullName FROM dbo.PR_User WHERE Outlet_ID=@OutletId AND Active=1 ORDER BY FullName",connection);employeeCommand.Parameters.Add("@OutletId",SqlDbType.BigInt).Value=hrId.Value;
        await using var reader=await employeeCommand.ExecuteReaderAsync(ct);while(await reader.ReadAsync(ct))employees.Add(new{id=Convert.ToInt16(reader["Id"]),employeeId=reader["EmpId"] is DBNull?null:(long?)Convert.ToInt64(reader["EmpId"]),name=Convert.ToString(reader["FullName"])??string.Empty});
        return Json(employees);
    }

    [HttpGet("EmployeeHistory")]
    public async Task<IActionResult> EmployeeHistory(short employeeUserId,CancellationToken ct)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated")return Unauthorized();
        await using var connection=new SqlConnection(_db);await connection.OpenAsync(ct);
        await using var command=new SqlCommand("""
            SELECT a.ID,a.Employee_ID,a.Employee_Name,a.From_Date,a.To_Date,a.IsActive,
                   COALESCE(NULLIF(o.Ar_Name,N''),o.En_Name) OutletName,
                   COALESCE(NULLIF(s.Ar_Name,N''),s.En_Name) StationName
            FROM dbo.Employee_Station_Assignment a
            INNER JOIN dbo.Outlet o ON o.ID=a.Outlet_ID
            INNER JOIN dbo.Station s ON s.ID=a.Station_ID
            WHERE a.PR_User_ID=@EmployeeUserId
            ORDER BY a.From_Date DESC,a.ID DESC;
            """,connection);
        command.Parameters.Add("@EmployeeUserId",SqlDbType.SmallInt).Value=employeeUserId;
        var history=new List<object>();
        await using var reader=await command.ExecuteReaderAsync(ct);
        while(await reader.ReadAsync(ct))history.Add(new
        {
            id=Convert.ToInt64(reader["ID"]),
            employeeId=reader["Employee_ID"] is DBNull?null:(long?)Convert.ToInt64(reader["Employee_ID"]),
            employeeName=Convert.ToString(reader["Employee_Name"])??string.Empty,
            outletName=Convert.ToString(reader["OutletName"])??string.Empty,
            stationName=Convert.ToString(reader["StationName"])??string.Empty,
            fromDate=Convert.ToDateTime(reader["From_Date"]).ToString("yyyy-MM-dd"),
            toDate=Convert.ToDateTime(reader["To_Date"]).ToString("yyyy-MM-dd"),
            isActive=Convert.ToBoolean(reader["IsActive"])
        });
        return Json(history);
    }

    [HttpPost("Save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(long outletId,long stationId,short employeeUserId,DateTime fromDate,DateTime toDate,CancellationToken ct)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated")return Unauthorized();
        if(fromDate.Date>toDate.Date){TempData["Error"]="تاريخ البداية يجب أن يسبق تاريخ النهاية.";return RedirectToAction(nameof(Index),new{outletId});}
        long? employeeId;string employeeName;
        await using(var privilege=new SqlConnection(_privilegeDb)){await privilege.OpenAsync(ct);await using var command=new SqlCommand("SELECT EmpId,FullName FROM dbo.PR_User WHERE Id=@Id AND Active=1",privilege);command.Parameters.Add("@Id",SqlDbType.SmallInt).Value=employeeUserId;await using var reader=await command.ExecuteReaderAsync(ct);if(!await reader.ReadAsync(ct)){TempData["Error"]="الموظف غير موجود أو غير نشط.";return RedirectToAction(nameof(Index),new{outletId});}employeeId=reader["EmpId"] is DBNull?null:Convert.ToInt64(reader["EmpId"]);employeeName=Convert.ToString(reader["FullName"])??string.Empty;}
        await using var connection=new SqlConnection(_db);await connection.OpenAsync(ct);await using var command2=new SqlCommand("""
            IF EXISTS(SELECT 1 FROM dbo.Station_Emp WHERE Emp_Id=@EmployeeUserId AND IsActive=1 AND User_Deletion_Id IS NULL AND @FromDate<=Date_To AND @ToDate>=Date_From)
                THROW 50002,N'يوجد تسكين آخر للموظف يتداخل مع هذه الفترة.',1;
            INSERT dbo.Station_Emp(Id,Station_Id,Emp_Id,IsActive,User_Creation_Id,User_Creation_Date,Date_From,Date_To)
            VALUES(NEXT VALUE FOR dbo.Station_Emp_SEQ,@StationId,@EmployeeUserId,1,@CreatedBy,GETDATE(),@FromDate,@ToDate);
            """,connection);
        command2.Parameters.Add("@OutletId",SqlDbType.BigInt).Value=outletId;command2.Parameters.Add("@StationId",SqlDbType.BigInt).Value=stationId;command2.Parameters.Add("@EmployeeUserId",SqlDbType.SmallInt).Value=employeeUserId;command2.Parameters.Add("@EmployeeId",SqlDbType.BigInt).Value=employeeId??(object)DBNull.Value;command2.Parameters.Add("@EmployeeName",SqlDbType.NVarChar,250).Value=employeeName;command2.Parameters.Add("@FromDate",SqlDbType.Date).Value=fromDate.Date;command2.Parameters.Add("@ToDate",SqlDbType.Date).Value=toDate.Date;command2.Parameters.Add("@CreatedBy",SqlDbType.BigInt).Value=long.TryParse(HttpContext.Session.GetString("UserId"),out var userId)?userId:(object)DBNull.Value;
        try{await command2.ExecuteNonQueryAsync(ct);TempData["Success"]="تم تسكين الموظف بنجاح.";}catch(SqlException ex)when(ex.Number==50002){TempData["Error"]=ex.Message;}
        return RedirectToAction(nameof(Index),new{outletId});
    }

    [HttpPost("Delete/{id:long}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id,long? outletId,CancellationToken ct)
    {
        if(HttpContext.Session.GetString("UserSession")!="Authenticated")return Unauthorized();
        await using var connection=new SqlConnection(_db);await connection.OpenAsync(ct);await using var command=new SqlCommand("UPDATE dbo.Station_Emp SET IsActive=0,User_Deletion_Id=@UserId,User_Deletion_Date=GETDATE() WHERE Id=@Id",connection);command.Parameters.Add("@Id",SqlDbType.BigInt).Value=id;command.Parameters.Add("@UserId",SqlDbType.SmallInt).Value=short.TryParse(HttpContext.Session.GetString("UserId"),out var userId)?userId:(object)DBNull.Value;await command.ExecuteNonQueryAsync(ct);TempData["Success"]="تم إلغاء التسكين.";return RedirectToAction(nameof(Index),new{outletId});
    }
}
