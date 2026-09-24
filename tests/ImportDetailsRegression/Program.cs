using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using PlantMobile.Controllers;
using ViewModels;
using PlantMobile.Infrastructure;

if (args.Contains("--share-paths"))
{
    foreach (string path in new[] { @"\\10.7.7.13\plant\Import\SampleData\test.jpg", "//10.7.7.13/plant/Import/test.jpg", @"\\10.7.7.13\plant" })
        if (AttachmentShareConnection.GetSharePath(path) != @"\\10.7.7.13\plant")
            throw new Exception("Connection must target the share root, not an attachment subdirectory.");
    if (AttachmentShareConnection.GetSharePath(@"C:\images\test.jpg") != null)
        throw new Exception("Local files must not create network connections.");
    try
    {
        AttachmentShareConnection.GetSharePath(@"\\server");
        throw new Exception("Invalid share path accepted.");
    }
    catch (IOException) { }
    Console.WriteLine("PASS: 5 share connection path cases. No network accessed.");
    return;
}

if (args.Contains("--share-connection"))
{
    var shareConfig = new ConfigurationBuilder().SetBasePath(Path.GetFullPath("PlantMobile"))
        .AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
    string sharePath = ImportAttachmentFiles.ResolvePath(@"\plant", shareConfig["Path_NetworkShare"]!);
    AttachmentShareConnection.Connect(sharePath, shareConfig);
    File.GetAttributes(sharePath);
    Console.WriteLine("PASS: Connected to the configured share and read directory attributes. No files written.");
    return;
}

if (args.Contains("--attachment-paths"))
{
    foreach (string server in new[] { "10.7.7.13", @"\\10.7.7.13", "//10.7.7.13/", " 10.7.7.13 " })
        foreach (string path in new[] { @"plant\Import\test.JPG", @"\plant\Import\test.JPG", "plant/Import/test.JPG" })
            if (ImportAttachmentFiles.ResolvePath(path, server) != @"\\10.7.7.13\plant\Import\test.JPG")
                throw new Exception("Incorrect attachment share or relative path.");
    const string unc = @"\\existing-server\plant\test.pdf";
    if (ImportAttachmentFiles.ResolvePath(unc, "10.7.7.13") != unc) throw new Exception("Existing UNC path changed.");
    foreach (var test in new[] { ("test.PDF", "", "application/pdf"), ("test.JPG", "", "image/jpeg"), ("test.png", "application/octet-stream", "image/png"), ("no-extension", "jpg", "image/jpeg"), ("test.unknown", "", "application/octet-stream") })
        if (ImportAttachmentFiles.ContentType(test.Item1, test.Item2) != test.Item3)
            throw new Exception("Incorrect attachment content type.");
    Console.WriteLine("PASS: 18 attachment path and content type cases. No database accessed.");
    return;
}

if (args.Contains("--quarantine-readiness"))
{
    var lot = new RequestQuarantineLotVm { InspectionComplete = true, WithdrawalComplete = true };
    if (!lot.IsReady) throw new Exception("Completed inspection without withdrawal should be ready.");
    lot.InspectionUnavailable = true;
    if (lot.IsReady) throw new Exception("Unavailable inspection must close quarantine even without samples.");
    lot.HasWithdrawal = true;
    lot.LaboratoryResultComplete = true;
    if (lot.IsReady) throw new Exception("Completed laboratory results must not reopen an unavailable inspection.");
    lot.InspectionUnavailable = false;
    if (!lot.IsReady) throw new Exception("Completed withdrawal and laboratory results should be ready.");
    lot.LaboratoryResultComplete = false;
    if (lot.IsReady) throw new Exception("Pending laboratory results must block quarantine.");
    lot.LaboratoryResultComplete = true;
    lot.WithdrawalComplete = false;
    if (lot.IsReady) throw new Exception("Pending withdrawal must block quarantine.");
    Console.WriteLine("PASS: 6 quarantine readiness regression cases. No database accessed.");
    return;
}

// Read-only integration regression. Run from the repository root against a
// database containing the request from the reported failure; never saves data.
var config = new ConfigurationBuilder().SetBasePath(Path.GetFullPath("PlantMobile"))
    .AddJsonFile("appsettings.json").Build();
var controller = new ImportRequestDetailsController(config,
    NullLogger<ImportRequestDetailsController>.Instance);
var load = typeof(ImportRequestDetailsController).GetMethod("LoadDetailsFromDatabaseAsync",
    BindingFlags.Instance | BindingFlags.NonPublic)!;
var details = await (Task<Ex_CheckRequest_GetData_Android_V2_VM>)load.Invoke(controller,
    new object[] { 331324L, 155292L, (byte)11 })!;
var lots = details.Item_Data.SelectMany(item => item.Lot_Data).ToList();
if (lots.Count != 6 || lots.Count(lot => lot.Package_Count == 796.8m) != 5
    || lots.Single(lot => lot.ID == 635522).Package_Count != 21m
    || lots.Sum(lot => lot.Package_Count) != 4005m)
    throw new Exception("Lot quantities or total changed unexpectedly.");
Console.WriteLine("PASS: affected inspection details loaded; 6 lots, decimal values preserved, total 4005. No data modified.");
