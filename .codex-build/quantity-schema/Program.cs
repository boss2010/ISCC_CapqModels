using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
var root = Path.GetFullPath("PlantMobile");
var config = new ConfigurationBuilder().SetBasePath(root).AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json", optional: true).AddEnvironmentVariables().Build();
await using var connection = new SqlConnection(config.GetConnectionString("DBConnection"));
await connection.OpenAsync();
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
