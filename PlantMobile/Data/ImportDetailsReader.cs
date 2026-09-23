using System.Globalization;
using EF.Imports;
using Microsoft.EntityFrameworkCore;
using ViewModels;

namespace PlantMobile.Data;

public sealed class ImportDetailsReader(IDbContextFactory<ImportDbContext> factory)
{
    public async Task<Ex_CheckRequest_GetData_Android_V2_VM> LoadAsync(long requestId, long committeeId, byte typeId, CancellationToken ct = default)
    {
        await using var db = await factory.CreateDbContextAsync(ct);
        db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        var header = await (from request in db.Im_CheckRequest
                            join committee in db.Im_RequestCommittee on request.ID equals committee.ImCheckRequest_ID
                            from data in db.Im_CheckRequest_Data.Where(d => d.Im_CheckRequest_ID == request.ID && d.User_Deletion_Id == null).DefaultIfEmpty()
                            from country in db.Country.Where(c => c.ID == data.ExportCountry_Id).DefaultIfEmpty()
                            from outlet in db.Outlet.Where(o => o.ID == request.Outlet_ID).DefaultIfEmpty()
                            from type in db.CommitteeType.Where(t => t.ID == committee.CommitteeType_ID).DefaultIfEmpty()
                            from operation in db.Im_OpertaionType.Where(o => o.ID == request.Im_OperationType).DefaultIfEmpty()
                            where request.ID == requestId && committee.ID == committeeId && committee.CommitteeType_ID == typeId && committee.User_Deletion_Id == null
                            select new {
                                request.ID, request.CheckRequest_Number, request.IsAccepted,
                                ImporterType = (int?)data.ImporterType_Id, data.DelegateName, data.DelegateAddress,
                                Country = country.Ar_Name, Outlet = outlet.Ar_Name, Address = outlet.Address_Ar,
                                Type = type.Name_Ar, committee.Delegation_Date, committee.Is_Cancel, committee.Status,
                                committee.IsFinishedAll, committee.Is_Start_Android, committee.User_Updation_Date,
                                OperationId = (int?)operation.ID, Operation = operation.Name_Ar, data.Ship_Name,
                                Importer = db.View_List_Im_PermissionRequest.Where(p => p.Im_CheckRequest_ID == request.ID)
                                    .OrderByDescending(p => p.Im_PermissionRequest_ID).Select(p => p.ImporterName).FirstOrDefault()
                            }).FirstOrDefaultAsync(ct)
            ?? throw new InvalidOperationException("الطلب غير موجود أو غير مسموح بعرض هذه اللجنة.");
        var details = new Ex_CheckRequest_GetData_Android_V2_VM {
            CheckRequest_Id = header.ID, CheckRequest_Number = header.CheckRequest_Number,
            IsAcceppted = header.IsAccepted, ImporterType_Id = header.ImporterType,
            Reciever_Name = !string.IsNullOrWhiteSpace(header.Importer) ? header.Importer : !string.IsNullOrWhiteSpace(header.DelegateName) ? header.DelegateName : "غير محدد",
            ImportCompany_Address = header.DelegateAddress ?? "", ExportCountry_Name = header.Country ?? "",
            Outlet_Name = header.Outlet ?? "", Outlet_Address = header.Address ?? "", Committee_Type = header.Type ?? "",
            Check_Date = header.Delegation_Date?.ToDateTime(TimeOnly.MinValue),
            RequestCommittee_Status = header.Is_Cancel != null ? "ملغي" : header.Status == true || header.IsFinishedAll == true ? "تم الانتهاء"
                : header.Is_Start_Android == true || header.User_Updation_Date != null ? "جاري العمل" : "جديد",
            Opreration_type_Id = header.OperationId, Opreration_type_Name = header.Operation ?? "", Ship_Name = header.Ship_Name ?? "", IsExport = 2
        };
        details.Item_Data = await (from item in db.Im_CheckRequest_Items
                                  join shipping in db.Im_CheckRequset_Shipping_Method on item.Im_CheckRequset_Shipping_Method_ID equals shipping.ID
                                  from shortName in db.Item_ShortName.Where(n => n.ID == item.Item_ShortName_ID).DefaultIfEmpty()
                                  from plant in db.Item.Where(p => p.ID == shortName.Item_ID).DefaultIfEmpty()
                                  where shipping.Im_CheckRequest_ID == requestId && shipping.User_Deletion_Id == null && item.User_Deletion_Id == null
                                  orderby item.ID
                                  select new _x0040_Item_Data {
                                      ID = item.ID, Item_ShortName_id = item.Item_ShortName_ID ?? 0,
                                      Item_Type_ID = shortName.Item_Type_ID, Item_Name = plant.Name_Ar ?? shortName.ShortName_Ar ?? "",
                                      Scientific_Name = plant.Scientific_Name ?? "", Item_ShortName_Name = shortName.ShortName_Ar ?? "",
                                      Im_CheckRequset_Shipping_Method_ID = item.Im_CheckRequset_Shipping_Method_ID, IsExport = 2
                                  }).ToListAsync(ct);
        foreach (var item in details.Item_Data) {
            item.Lot_Data = new List<_x0040_temp_table_Lot>();
            item.Sample_Data = new List<Itemsample_data>();
            item.TreatmentLot = new List<TreatmentDataDTO>();
        }
        var itemMap = details.Item_Data.ToDictionary(i => i.ID);
        var itemIds = itemMap.Keys.ToArray();
        var lots = await (from lot in db.Im_CheckRequest_Items_Lot_Category
                          join item in db.Im_CheckRequest_Items on lot.Im_CheckRequest_Items_ID equals item.ID
                          from package in db.Package_Type.Where(p => p.ID == (lot.Package_Type_ID ?? item.Package_Type_ID)).DefaultIfEmpty()
                          where itemIds.Contains(item.ID) && lot.User_Deletion_Id == null
                              && (typeId != 11 || db.Im_CommitteeResult.Any(r => r.Committee_ID == committeeId && r.User_Deletion_Id == null
                                  && (r.LotData_ID == lot.ID || (r.LotData_ID == null && r.Im_Request_Item_Id == item.ID))))
                              && (typeId != 13 || db.Im_CheckRequest_SampleData.Any(s => s.Im_RequestCommittee_ID == committeeId && s.User_Deletion_Id == null
                                  && (s.LotData_ID == lot.ID || (s.LotData_ID == null && s.Im_Request_Item_Id == item.ID))))
                          orderby lot.ID
                          select new {
                              ItemId = item.ID, lot.PackagesCount,
                              Lot = new _x0040_temp_table_Lot {
                                  ID = lot.ID, Lot_ID = lot.ID, Lot_Number = lot.Lot_Number ?? lot.ID.ToString(),
                                  Net_Weight = lot.Net_Weight ?? item.Net_Weight, Gross_Weight = lot.GrossWeight ?? item.GrossWeight,
                                  Package_Weight = lot.Package_Weight ?? item.Package_Weight, Units_Number = lot.Units_Number ?? item.Units_Number,
                                  Size = lot.Size, Package_Type_Name = package.Ar_Name ?? "", Grower_Number = lot.Grower_Number ?? "",
                                  Number_Wooden_Package = lot.Number_Wooden_Package ?? "",
                                  is_Total = db.Im_CommitteeResult.Where(r => r.Committee_ID == committeeId && (r.LotData_ID == lot.ID || r.Im_Request_Item_Id == item.ID))
                                      .OrderByDescending(r => r.ID).Select(r => r.IS_Total).FirstOrDefault() == true
                              }
                          }).ToListAsync(ct);
        foreach (var row in lots) {
            // Database-first mapping keeps this legacy nvarchar column as text.
            // Parse only at the display boundary, preserving fractional values.
            row.Lot.Package_Count = string.IsNullOrWhiteSpace(row.PackagesCount) ? 0
                : decimal.Parse(row.PackagesCount, NumberStyles.Number, CultureInfo.InvariantCulture);
            ((List<_x0040_temp_table_Lot>)itemMap[row.ItemId].Lot_Data).Add(row.Lot);
        }
        var sampleRows = await (from s in db.Im_CheckRequest_SampleData
                                from link in db.AnalysisLabType.Where(l => l.ID == s.AnalysisLabType_ID).DefaultIfEmpty()
                                from lab in db.AnalysisLab.Where(l => l.ID == link.AnalysisLabID).DefaultIfEmpty()
                                from analysis in db.AnalysisType.Where(a => a.ID == link.AnalysisTypeID).DefaultIfEmpty()
                                from lot in db.Im_CheckRequest_Items_Lot_Category.Where(l => l.ID == s.LotData_ID).DefaultIfEmpty()
                                where s.Im_RequestCommittee_ID == committeeId && s.User_Deletion_Id == null
                                orderby s.ID
                                select new {
                                    ItemId = s.Im_Request_Item_Id,
                                    Sample = new Itemsample_data {
                                        Sample_dataId = s.ID, LotData_ID = s.LotData_ID, AnalysisLab_Name = lab.Name_Ar ?? "", AnalysisType_Name = analysis.Name_Ar ?? "",
                                        Sample_BarCode = s.Sample_BarCode ?? "", SampleRatio = s.SampleRatio, SampleSize = s.SampleSize,
                                        IS_From_Android = s.IS_From_Android, IS_Total = s.IS_Total, Lotnum = lot.Lot_Number ?? "",
                                        Syl_ALkhatima_Number = s.Syl_ALkhatima_Number ?? "", Notes_Ar = s.Notes_Ar ?? "", AnalysisLabTypeId = s.AnalysisLabType_ID
                                    }
                                }).ToListAsync(ct);
        foreach (var row in sampleRows)
            if (itemMap.TryGetValue(row.ItemId, out var item)) ((List<Itemsample_data>)item.Sample_Data).Add(row.Sample);
        return details;
    }
}
