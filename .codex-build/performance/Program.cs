using System.Data;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using PlantMobile.Infrastructure;

var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
using var settings = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, "PlantMobile/appsettings.json")));
var connectionString = settings.RootElement.GetProperty("ConnectionStrings").GetProperty("DBConnection").GetString();
var baseline = File.ReadAllText(Path.Combine(root, ".codex-build/performance/baseline.sql"));
long user = 137;
var date = new DateTime(2026, 9, 15);
try
{
    var before = await Run(false, null);
    var after = await Run(true, null);
    Compare(before, after, "list");
    foreach (var id in before.Select(x => Convert.ToInt64(x["CheckRequestId"])).Distinct().Take(2))
        Compare(await Run(false, id), await Run(true, id), "committees");
    Compare(await Run(false, -1), await Run(true, -1), "missing request");
    user = -1;
    Compare(await Run(false, null), await Run(true, null), "unassigned user");
}
catch (SqlException ex)
{
    Console.WriteLine($"SQL validation failed: Number={ex.Number}; Class={ex.Class}; {ex.Message.Split('\n')[0]}");
    Environment.ExitCode = 1;
}

async Task<List<SortedDictionary<string, object?>>> Run(bool optimized, long? id)
{
    await using var connection = new SqlConnection(connectionString);
    var watch = Stopwatch.StartNew();
    await connection.OpenAsync();
    var openMs = watch.ElapsedMilliseconds;
    string sql = optimized ? ImportRequestsSql.Build(id.HasValue) : baseline;
    if (!optimized && !id.HasValue)
    {
        var start = sql.IndexOf("(SELECT STRING_AGG", StringComparison.Ordinal);
        var end = sql.IndexOf("FROM dbo.Im_CheckRequest request", start, StringComparison.Ordinal);
        var summary = (string)typeof(ImportRequestsSql).GetField("ListSummary", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!.GetRawConstantValue()!;
        sql = sql[..start] + summary + "\n" + sql[end..];
    }
    await using var command = new SqlCommand(sql, connection) { CommandTimeout = 90 };
    command.Parameters.AddWithValue("@UserId", user);
    command.Parameters.AddWithValue("@SelectedDate", date);
    command.Parameters.Add("@CheckRequestId", SqlDbType.BigInt).Value = (object?)id ?? DBNull.Value;
    var rows = new List<SortedDictionary<string, object?>>();
    await using var reader = await command.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        var row = new SortedDictionary<string, object?>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            var name = reader.GetName(i);
            if (name == "RowNum") continue;
            object? value = reader.IsDBNull(i) ? null : reader.GetValue(i);
            if (value is string text && name is "CommitteeMembers" or "PendingLabBarcodes")
                value = string.Join('|', text.Split('|').OrderBy(x => x, StringComparer.Ordinal));
            row[name] = value;
        }
        rows.Add(row);
    }
    Console.WriteLine($"{(optimized ? "optimized" : "baseline")} {(id.HasValue ? "committees" : "list")}: connection={openMs}ms total={watch.ElapsedMilliseconds}ms rows={rows.Count}");
    return rows;
}

static void Compare(List<SortedDictionary<string, object?>> before, List<SortedDictionary<string, object?>> after, string label)
{
    var a = before.Select(x => JsonSerializer.Serialize(x)).OrderBy(x => x).ToArray();
    var b = after.Select(x => JsonSerializer.Serialize(x)).OrderBy(x => x).ToArray();
    if (!a.SequenceEqual(b)) throw new InvalidOperationException($"Result mismatch: {label}");
    Console.WriteLine($"PASS identical results: {label}");
}
