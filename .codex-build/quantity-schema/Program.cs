using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
var root = Path.GetFullPath("PlantMobile");
var config = new ConfigurationBuilder().SetBasePath(root).AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json", optional: true).AddEnvironmentVariables().Build();
await using var connection = new SqlConnection(config.GetConnectionString("DBConnection"));
await connection.OpenAsync();
if (args.Contains("--old-check"))
{
    await using var query = new SqlCommand("""
        SELECT c.ID,c.Delegation_Date,c.Is_Start_Android,c.Status,c.IsFinishedAll,
               r.LotData_ID,r.Im_Request_Item_Id,r.CommitteeResultType_ID,r.User_Updation_Date
        FROM dbo.Im_RequestCommittee c LEFT JOIN dbo.Im_CommitteeResult r ON r.Committee_ID=c.ID AND r.User_Deletion_Id IS NULL
        WHERE c.ImCheckRequest_ID=326745 AND c.User_Deletion_Id IS NULL AND c.CommitteeType_ID=11 ORDER BY c.ID
        """,connection);
    await using var reader=await query.ExecuteReaderAsync();
    while(await reader.ReadAsync()) Console.WriteLine(string.Join(" | ",Enumerable.Range(0,reader.FieldCount).Select(i=>$"{reader.GetName(i)}={reader[i]}")));
    return;
}
if (args.Contains("--completion-check"))
{
    var queryType = typeof(PlantMobile.Controllers.ImportRequestsController).Assembly.GetType("PlantMobile.Infrastructure.ImportRequestsSql")!;
    string sql = (string)queryType.GetMethod("Build")!.Invoke(null, new object[] {false})!;
    await using var query = new SqlCommand(sql, connection) { CommandTimeout = 90 };
    query.Parameters.AddWithValue("@UserId", 354);
    query.Parameters.AddWithValue("@SelectedDate", DateTime.Today);
    query.Parameters.AddWithValue("@CheckRequestId", DBNull.Value);
    await using var rows = await query.ExecuteReaderAsync();
    while (await rows.ReadAsync())
        Console.WriteLine($"Request={rows["CheckRequest_Number"]}, Committee={rows["CommitteeId"]}, FinalQuarantine={rows["RequestQuarantineComplete"]}");
    await rows.CloseAsync();
    await using var lots = new SqlCommand("""
        SELECT lot.ID, lot.Lot_Number, latest.Name_AR, latest.Is_Continue
        FROM dbo.Im_CheckRequest_Items_Lot_Category lot
        JOIN dbo.Im_CheckRequest_Items item ON item.ID=lot.Im_CheckRequest_Items_ID
        JOIN dbo.Im_CheckRequset_Shipping_Method shipping ON shipping.ID=item.Im_CheckRequset_Shipping_Method_ID
        OUTER APPLY (SELECT TOP 1 status.Name_AR,status.Is_Continue FROM dbo.Im_CheckRequest_Items_Lot_Result lr
          JOIN dbo.Im_CheckRequest_Lot_Result_Status status ON status.ID=lr.IS_Status
          WHERE lr.Im_CheckRequest_Items_Lot_Category_ID=lot.ID AND lr.IS_Status_Committee=1 ORDER BY lr.ID DESC) latest
        WHERE shipping.Im_CheckRequest_ID=326745 AND lot.User_Deletion_Id IS NULL AND item.User_Deletion_Id IS NULL;
        """, connection);
    await using var lotRows = await lots.ExecuteReaderAsync();
    while (await lotRows.ReadAsync()) Console.WriteLine($"Lot={lotRows[0]}, Number={lotRows[1]}, Status={lotRows[2]}, Continues={lotRows[3]}");
    return;
}
if (args.Contains("--eligibility-check"))
{
    var policy = typeof(PlantMobile.Controllers.ImportRequestsController).Assembly.GetType("PlantMobile.Infrastructure.ImportCommitteeEligibility")!;
    var filter = policy.GetMethod("Filter", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)!;
    const string sql = """
        SELECT c.ID, c.CommitteeType_ID, c.Delegation_Date,
          (SELECT COUNT(*) FROM dbo.Im_CheckRequest_SampleData s WHERE s.Im_RequestCommittee_ID=c.ID AND s.User_Deletion_Id IS NULL) AS Samples
        FROM dbo.Im_RequestCommittee c
        WHERE c.ImCheckRequest_ID=326745 AND c.User_Deletion_Id IS NULL ORDER BY c.ID
        """;
    foreach (var version in new[] { ("Before", sql), ("Eligible", (string)filter.Invoke(null, new object[] {sql})!) })
    {
        Console.WriteLine(version.Item1);
        await using var query = new SqlCommand(version.Item2, connection);
        await using var rows = await query.ExecuteReaderAsync();
        while (await rows.ReadAsync()) Console.WriteLine($"Committee={rows[0]}, Type={rows[1]}, Date={rows[2]}, Samples={rows[3]}");
    }
    return;
}
await using var command = new SqlCommand(await File.ReadAllTextAsync(Path.Combine(root, "Sql", "20260916_SampleQuantityUnit.sql")), connection);
await command.ExecuteNonQueryAsync();
await using var verify = new SqlCommand("SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Im_CheckRequest_SampleData') AND name = 'SampleQuantityUnit'", connection);
if (Convert.ToInt32(await verify.ExecuteScalarAsync()) != 1) throw new Exception("Quantity unit schema verification failed.");
Console.WriteLine("Sample quantity unit column is ready; existing records were not modified.");
