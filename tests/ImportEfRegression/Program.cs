using System.Diagnostics;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PlantMobile.Data;
using PlantMobile.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging.Abstractions;

var config = new ConfigurationBuilder().SetBasePath(Path.GetFullPath("PlantMobile")).AddJsonFile("appsettings.json").Build();
var factory = ImportDatabase.CreateFactory(config);
var efReader = new ImportRequestReader(factory);
await using var connection = new SqlConnection(config.GetConnectionString("DBConnection"));
await connection.OpenAsync();
var legacyType = typeof(ImportRequestReader).Assembly.GetType("PlantMobile.Infrastructure.ImportRequestsSql")!;
foreach (long? requestId in new long?[] { null, 331324, 326745 })
{
    var clock = Stopwatch.StartNew();
    await using var command = new SqlCommand((string)legacyType.GetMethod("Build")!.Invoke(null, new object[] { requestId.HasValue })!, connection) { CommandTimeout = 120 };
    command.Parameters.AddWithValue("@UserId", 354L);
    command.Parameters.AddWithValue("@SelectedDate", new DateTime(2026, 9, 22));
    command.Parameters.AddWithValue("@CheckRequestId", (object?)requestId ?? DBNull.Value);
    var legacy = new List<ImportCommitteeRow>();
    await using (var reader = await command.ExecuteReaderAsync())
        while (await reader.ReadAsync())
        {
            var row = new ImportCommitteeRow();
            foreach (var prop in typeof(ImportCommitteeRow).GetProperties())
            {
                var value = reader[prop.Name];
                if (value != DBNull.Value) prop.SetValue(row, Convert.ChangeType(value, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
            }
            legacy.Add(row);
        }
    double legacyMs = clock.Elapsed.TotalMilliseconds;
    clock.Restart();
    var actual = await efReader.LoadAsync(354, new DateTime(2026, 9, 22), requestId);
    double efMs = clock.Elapsed.TotalMilliseconds;
    if (legacy.Count != actual.Count) throw new Exception($"Row count mismatch for {requestId}: {legacy.Count} vs {actual.Count}.");
    for (int i = 0; i < legacy.Count; i++)
        foreach (var prop in typeof(ImportCommitteeRow).GetProperties())
        {
            object? expected = prop.GetValue(legacy[i]), observed = prop.GetValue(actual[i]);
            if (prop.Name is "CommitteeMembers" or "PendingLabBarcodes")
            {
                expected = string.Join('|', ((string?)expected ?? "").Split('|').Order());
                observed = string.Join('|', ((string?)observed ?? "").Split('|').Order());
            }
            if (!Equals(expected, observed)) throw new Exception($"Committee {legacy[i].CommitteeId}: {prop.Name} differs: '{expected}' vs '{observed}'.");
        }
    Console.WriteLine($"PASS request={requestId?.ToString() ?? "list"}, rows={actual.Count}, legacy={legacyMs:F0}ms, EF={efMs:F0}ms; every field matches.");
}
Console.WriteLine("Read-only parity checks passed. No database records were changed.");

var warmClock = Stopwatch.StartNew();
await efReader.LoadAsync(354, new DateTime(2026, 9, 22), null);
Console.WriteLine($"Warm EF list: {warmClock.Elapsed.TotalMilliseconds:F0}ms.");
var context = new DefaultHttpContext { Session = new TestSession() };
context.Session.SetString("UserSession", "Authenticated");
context.Session.SetString("UserId", "354");
var controller = new ImportRequestDetailsController(config, NullLogger<ImportRequestDetailsController>.Instance, factory)
{
    ControllerContext = new ControllerContext { HttpContext = context },
    TempData = new TempDataDictionary(context, new TestTempData())
};
var action = await controller.Index(331324, 155292, 11);
if (action is not ViewResult { Model: ImportRequestDetailsPageVm page })
    throw new Exception($"Inspection screen redirected: {controller.TempData["PageError"]}");
var lots = page.Details.Item_Data.SelectMany(i => i.Lot_Data).ToList();
if (lots.Count != 6 || lots.Count(l => l.Package_Count == 796.8m) != 5 || lots.Sum(l => l.Package_Count) != 4005m)
    throw new Exception("Inspection lot quantities changed.");
if (page.CommitteeResultTypes.Count == 0) throw new Exception("Inspection results dropdown is empty.");
Console.WriteLine("PASS: full inspection action returns its screen, 6 lots, decimal quantities and result choices.");

var validate = typeof(ImportRequestDetailsController).GetMethod("ValidateSampleScopeAsync", BindingFlags.Instance | BindingFlags.NonPublic)!;
var firstItem = page.Details.Item_Data.First(i => i.Lot_Data.Any());
var firstLot = firstItem.Lot_Data.First();
var allowed = new ImportSampleSaveVm { LotDataId = firstLot.ID, RequestItemId = firstItem.ID, ItemShortNameId = firstItem.Item_ShortName_id, AnalysisLabTypeId = 1 };
await (Task)validate.Invoke(controller, new object[] { 331324L, 155292L, (byte)11, new List<ImportSampleSaveVm> { allowed } })!;
async Task Reject(ImportSampleSaveVm sample)
{
    try { await (Task)validate.Invoke(controller, new object[] { 331324L, 155292L, (byte)11, new List<ImportSampleSaveVm> { sample } })!; }
    catch (InvalidOperationException) { return; }
    throw new Exception("Out-of-scope or incomplete total withdrawal was accepted.");
}
await Reject(new ImportSampleSaveVm { LotDataId = -1, RequestItemId = firstItem.ID, ItemShortNameId = firstItem.Item_ShortName_id, AnalysisLabTypeId = 1 });
allowed.IsTotal = true;
await Reject(allowed);
Console.WriteLine("PASS: assigned partial withdrawal accepted; foreign lot and incomplete total withdrawal rejected.");

// Exercise the sample merge branch without calling the barcode-generating GET path.
await using (var sampleCase = new SqlCommand("""
    SELECT TOP (1) c.ImCheckRequest_ID, c.ID, s.ID, COALESCE(s.SampleQuantityUnit, N'kg')
    FROM dbo.Im_CheckRequest_SampleData s
    JOIN dbo.Im_RequestCommittee c ON c.ID=s.Im_RequestCommittee_ID
    JOIN dbo.Im_CheckRequest_Items item ON item.ID=s.Im_Request_Item_Id
    JOIN dbo.Im_CheckRequset_Shipping_Method shipping ON shipping.ID=item.Im_CheckRequset_Shipping_Method_ID
    WHERE c.CommitteeType_ID=13 AND c.User_Deletion_Id IS NULL AND s.User_Deletion_Id IS NULL
      AND item.User_Deletion_Id IS NULL AND shipping.User_Deletion_Id IS NULL
      AND shipping.Im_CheckRequest_ID=c.ImCheckRequest_ID
    ORDER BY s.ID DESC
    """, connection))
{
    long requestId, committeeId, sampleId;
    string unit;
    await using (var sampleRow = await sampleCase.ExecuteReaderAsync())
    {
        if (!await sampleRow.ReadAsync()) throw new Exception("Withdrawal fixture is missing.");
        requestId = sampleRow.GetInt64(0); committeeId = sampleRow.GetInt64(1); sampleId = sampleRow.GetInt64(2); unit = sampleRow.GetString(3);
    }
    var loadSamples = typeof(ImportRequestDetailsController).GetMethod("LoadDetailsWithSamplesAsync", BindingFlags.Instance | BindingFlags.NonPublic)!;
    var detail = await (Task<ViewModels.Ex_CheckRequest_GetData_Android_V2_VM>)loadSamples.Invoke(controller, new object[] { requestId, committeeId, (byte)13 })!;
    if (detail.Item_Data.SelectMany(i => i.Sample_Data).Single(s => s.Sample_dataId == sampleId).QuantityUnit != unit)
        throw new Exception("Withdrawal sample unit changed.");
    Console.WriteLine("PASS: withdrawal details and saved sample metadata loaded through EF without modifying barcodes.");
}

await using (var parseOn = new SqlCommand("SET PARSEONLY ON", connection)) await parseOn.ExecuteNonQueryAsync();
try
{
    await using var script = new SqlCommand(await File.ReadAllTextAsync("PlantMobile/Sql/20260922_ImportEfIndexes.sql"), connection);
    await script.ExecuteNonQueryAsync();
}
finally
{
    await using var parseOff = new SqlCommand("SET PARSEONLY OFF", connection);
    await parseOff.ExecuteNonQueryAsync();
}
Console.WriteLine("PASS: database script parsed by SQL Server without executing it.");

sealed class TestSession : ISession
{
    private readonly Dictionary<string, byte[]> values = new();
    public bool IsAvailable => true;
    public string Id => "ef-regression";
    public IEnumerable<string> Keys => values.Keys;
    public void Clear() => values.Clear();
    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public void Remove(string key) => values.Remove(key);
    public void Set(string key, byte[] value) => values[key] = value;
    public bool TryGetValue(string key, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out byte[]? value) => values.TryGetValue(key, out value);
}
sealed class TestTempData : ITempDataProvider
{
    public IDictionary<string, object> LoadTempData(HttpContext context) => new Dictionary<string, object>();
    public void SaveTempData(HttpContext context, IDictionary<string, object> values) { }
}
