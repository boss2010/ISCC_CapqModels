using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using PlantMobile.Controllers;
using ViewModels;

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
