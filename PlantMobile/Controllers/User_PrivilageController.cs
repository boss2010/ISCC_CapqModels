using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PlantMobile.Data;
using PlantMobile.Infrastructure;
using ViewModels;

namespace PlantMobile.Controllers;

[Route("EX_User/User_Privilage")]
public sealed class User_PrivilageController : Controller
{
    private readonly string _applicationConnectionString;
    private readonly string _privilegeConnectionString;
    private readonly MenuAccessReader _accessReader;
    private readonly IMenuService _menuService;
    private readonly ILogger<User_PrivilageController> _logger;

    public User_PrivilageController(
        IConfiguration configuration,
        MenuAccessReader accessReader,
        IMenuService menuService,
        ILogger<User_PrivilageController> logger)
    {
        _applicationConnectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _privilegeConnectionString = configuration.GetConnectionString("PrivilegeDBConnection")
            ?? throw new InvalidOperationException("PrivilegeDBConnection is not configured.");
        _accessReader = accessReader;
        _menuService = menuService;
        _logger = logger;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        if (!TryCurrentUser(out short currentUserId))
            return RedirectToAction("Index", "Login");
        if (!await CanManageAsync(currentUserId, cancellationToken).ConfigureAwait(false))
            return Forbid();

        var model = new UserPrivilegePageVm
        {
            Outlets = await ReadOptionsAsync(
                _applicationConnectionString,
                """
                SELECT ID_HR, Ar_Name
                FROM dbo.Outlet
                WHERE IsActive = 1
                  AND User_Deletion_Id IS NULL
                  AND ID_HR IS NOT NULL
                ORDER BY Ar_Name;
                """, cancellationToken).ConfigureAwait(false),
            Groups = await ReadOptionsAsync(
                _privilegeConnectionString,
                """
                SELECT DISTINCT [group].Id, [group].GroupName
                FROM dbo.PR_Group [group]
                INNER JOIN dbo.PR_GroupModuleMenu map ON map.PR_GroupId = [group].Id
                WHERE [group].Active = 1
                  AND map.IS_Active = 1
                ORDER BY [group].GroupName;
                """, cancellationToken).ConfigureAwait(false)
        };
        return View(model);
    }

    [HttpGet("Users")]
    public async Task<IActionResult> Users(long outletId, CancellationToken cancellationToken)
    {
        if (!await IsAuthorizedAsync(cancellationToken).ConfigureAwait(false)) return Forbid();
        const string sql = """
            SELECT Id, EmpId, COALESCE(FullName, N''), COALESCE(LoginName, N'')
            FROM dbo.PR_User
            WHERE Outlet_ID = @OutletId
              AND Active = 1
              AND LoginName IS NOT NULL
              AND Password IS NOT NULL
            ORDER BY FullName;
            """;
        var result = new List<UserPrivilegeEmployeeVm>();
        await using var connection = new SqlConnection(_privilegeConnectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@OutletId", SqlDbType.BigInt).Value = outletId;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new UserPrivilegeEmployeeVm
            {
                Id = reader.GetInt16(0),
                EmployeeId = reader.GetInt64(1),
                Name = reader.GetString(2),
                LoginName = reader.GetString(3)
            });
        }
        return Json(result);
    }

    [HttpGet("Modules")]
    public async Task<IActionResult> Modules(int groupId, CancellationToken cancellationToken)
    {
        if (!await IsAuthorizedAsync(cancellationToken).ConfigureAwait(false)) return Forbid();
        const string sql = """
            SELECT DISTINCT module.Id, module.ModuleName
            FROM dbo.PR_Module module
            INNER JOIN dbo.PR_GroupModuleMenu map ON map.PR_ModuleId = module.Id
            WHERE map.PR_GroupId = @GroupId
              AND map.IS_Active = 1
              AND module.Active = 1
            ORDER BY module.ModuleName;
            """;
        return Json(await ReadOptionsAsync(
            _privilegeConnectionString, sql, cancellationToken,
            new SqlParameter("@GroupId", SqlDbType.Int) { Value = groupId }).ConfigureAwait(false));
    }

    [HttpGet("Menus")]
    public async Task<IActionResult> Menus(
        short userId,
        int groupId,
        int moduleId,
        CancellationToken cancellationToken)
    {
        if (!await IsAuthorizedAsync(cancellationToken).ConfigureAwait(false)) return Forbid();
        const string sql = """
            WITH CurrentPrivilege AS
            (
                SELECT PR_MenuId,
                       MAX(CONVERT(tinyint, CanView)) AS CanView,
                       MAX(CONVERT(tinyint, CanAdd)) AS CanAdd,
                       MAX(CONVERT(tinyint, CanEdit)) AS CanEdit,
                       MAX(CONVERT(tinyint, CanDelete)) AS CanDelete
                FROM dbo.PR_GroupModuleMenuPrivilage
                WHERE PR_User_id = @UserId
                  AND PR_GroupId = @GroupId
                  AND PR_ModuleId = @ModuleId
                  AND IS_Active = 1
                GROUP BY PR_MenuId
            )
            SELECT menu.Id, menu.MenuTitle, COALESCE(menu.MenuURL, N''),
                   CONVERT(bit, ISNULL(privilege.CanView, 0)),
                   CONVERT(bit, ISNULL(privilege.CanAdd, 0)),
                   CONVERT(bit, ISNULL(privilege.CanEdit, 0)),
                   CONVERT(bit, ISNULL(privilege.CanDelete, 0))
            FROM dbo.PR_GroupModuleMenu map
            INNER JOIN dbo.PR_Menu menu ON menu.Id = map.PR_MenuId
            LEFT JOIN CurrentPrivilege privilege ON privilege.PR_MenuId = menu.Id
            WHERE map.PR_GroupId = @GroupId
              AND map.PR_ModuleId = @ModuleId
              AND map.IS_Active = 1
              AND menu.Active = 1
            ORDER BY ISNULL(map.Order_BY, 2147483647), menu.MenuTitle;
            """;
        var result = new List<UserPrivilegeMenuVm>();
        await using var connection = new SqlConnection(_privilegeConnectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@UserId", SqlDbType.SmallInt).Value = userId;
        command.Parameters.Add("@GroupId", SqlDbType.Int).Value = groupId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = moduleId;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new UserPrivilegeMenuVm
            {
                MenuId = reader.GetInt32(0), MenuTitle = reader.GetString(1), MenuUrl = reader.GetString(2),
                CanView = reader.GetBoolean(3), CanAdd = reader.GetBoolean(4),
                CanEdit = reader.GetBoolean(5), CanDelete = reader.GetBoolean(6)
            });
        }
        return Json(result);
    }

    [HttpPost("Save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(
        [FromBody] UserPrivilegeSaveVm model,
        CancellationToken cancellationToken)
    {
        if (!await IsAuthorizedAsync(cancellationToken).ConfigureAwait(false)) return Forbid();
        if (model.UserId <= 0 || model.GroupId <= 0 || model.ModuleId <= 0 || model.Menus.Count == 0)
            return BadRequest(new { message = "بيانات الحفظ غير مكتملة." });

        const string validateSql = """
            SELECT COUNT(DISTINCT map.PR_MenuId)
            FROM dbo.PR_GroupModuleMenu map
            WHERE map.PR_GroupId = @GroupId
              AND map.PR_ModuleId = @ModuleId
              AND map.IS_Active = 1
              AND map.PR_MenuId IN (SELECT value FROM STRING_SPLIT(@MenuIds, ','));
            """;
        const string saveSql = """
            UPDATE dbo.PR_GroupModuleMenuPrivilage
            SET CanView = @CanView, CanAdd = @CanAdd, CanEdit = @CanEdit,
                CanDelete = @CanDelete, CanPrint = 0, IS_Active = @IsActive
            WHERE PR_User_id = @UserId AND PR_GroupId = @GroupId
              AND PR_ModuleId = @ModuleId AND PR_MenuId = @MenuId;

            IF @@ROWCOUNT = 0 AND @IsActive = 1
            BEGIN
                INSERT dbo.PR_GroupModuleMenuPrivilage
                (Id, PR_GroupId, PR_ModuleId, PR_MenuId, CanView, CanAdd,
                 CanEdit, CanDelete, CanPrint, PR_User_id, IS_Active)
                VALUES
                (NEXT VALUE FOR dbo.PR_GroupModuleMenuPrivilage_SEQ,
                 @GroupId, @ModuleId, @MenuId, @CanView, @CanAdd,
                 @CanEdit, @CanDelete, 0, @UserId, 1);
            END;
            """;

        await using var connection = new SqlConnection(_privilegeConnectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            string menuIds = string.Join(',', model.Menus.Select(row => row.MenuId).Distinct());
            await using (var validate = new SqlCommand(validateSql, connection, (SqlTransaction)transaction))
            {
                validate.Parameters.Add("@GroupId", SqlDbType.Int).Value = model.GroupId;
                validate.Parameters.Add("@ModuleId", SqlDbType.Int).Value = model.ModuleId;
                validate.Parameters.Add("@MenuIds", SqlDbType.VarChar, -1).Value = menuIds;
                int validCount = Convert.ToInt32(await validate.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false));
                if (validCount != model.Menus.Select(row => row.MenuId).Distinct().Count())
                    throw new InvalidOperationException("إحدى الشاشات لا تتبع المجموعة والقسم المختارين.");
            }

            foreach (var row in model.Menus.GroupBy(item => item.MenuId).Select(group => group.Last()))
            {
                bool isActive = row.CanView || row.CanAdd || row.CanEdit || row.CanDelete;
                await using var save = new SqlCommand(saveSql, connection, (SqlTransaction)transaction);
                save.Parameters.Add("@UserId", SqlDbType.SmallInt).Value = model.UserId;
                save.Parameters.Add("@GroupId", SqlDbType.Int).Value = model.GroupId;
                save.Parameters.Add("@ModuleId", SqlDbType.Int).Value = model.ModuleId;
                save.Parameters.Add("@MenuId", SqlDbType.Int).Value = row.MenuId;
                save.Parameters.Add("@CanView", SqlDbType.Bit).Value = row.CanView;
                save.Parameters.Add("@CanAdd", SqlDbType.Bit).Value = row.CanAdd;
                save.Parameters.Add("@CanEdit", SqlDbType.Bit).Value = row.CanEdit;
                save.Parameters.Add("@CanDelete", SqlDbType.Bit).Value = row.CanDelete;
                save.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;
                await save.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            _menuService.Invalidate(model.UserId);
            return Json(new { success = true, message = "تم حفظ صلاحيات الموظف بنجاح." });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogError(ex, "Failed to save privileges for user {UserId}", model.UserId);
            return BadRequest(new { message = ex is InvalidOperationException ? ex.Message : "تعذر حفظ الصلاحيات." });
        }
    }

    private async Task<bool> IsAuthorizedAsync(CancellationToken cancellationToken) =>
        TryCurrentUser(out short userId) && await CanManageAsync(userId, cancellationToken).ConfigureAwait(false);

    private Task<bool> CanManageAsync(short userId, CancellationToken cancellationToken) =>
        _accessReader.CanViewAsync(userId, MenuAccessReader.UserPrivilegeMenuUrl, cancellationToken);

    private bool TryCurrentUser(out short userId)
    {
        userId = 0;
        return HttpContext.Session.GetString("UserSession") == "Authenticated"
            && short.TryParse(HttpContext.Session.GetString("UserId"), out userId);
    }

    private static async Task<List<UserPrivilegeOptionVm>> ReadOptionsAsync(
        string connectionString,
        string sql,
        CancellationToken cancellationToken,
        params SqlParameter[] parameters)
    {
        var result = new List<UserPrivilegeOptionVm>();
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        if (parameters.Length > 0) command.Parameters.AddRange(parameters);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            result.Add(new UserPrivilegeOptionVm { Id = Convert.ToInt64(reader.GetValue(0)), Text = reader.GetString(1) });
        return result;
    }
}
