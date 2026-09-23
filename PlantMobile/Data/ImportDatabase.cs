using EF.Imports;
using EF.Imports.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PlantMobile.Data;

public static class ImportDatabase
{
    public static void Configure(DbContextOptionsBuilder options, IConfiguration configuration) =>
        options.UseSqlServer(configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured."), sql => sql.CommandTimeout(90));

    public static IDbContextFactory<ImportDbContext> CreateFactory(IConfiguration configuration)
    {
        var options = new DbContextOptionsBuilder<ImportDbContext>();
        Configure(options, configuration);
        return new PooledDbContextFactory<ImportDbContext>(options.Options);
    }

    // Keep exactly the existing rule: a barcode or Status alone is not work.
    public static IQueryable<Im_RequestCommittee> EligibleCommittees(this ImportDbContext db) =>
        db.Im_RequestCommittee.AsNoTracking().Where(c =>
            c.Delegation_Date == null || c.Delegation_Date >= DateOnly.FromDateTime(DateTime.Today)
            || c.Is_Start_Android == true || c.IsFinishedAll == true
            || db.Im_CommitteeResult.Any(r => r.Committee_ID == c.ID && r.User_Deletion_Id == null && r.CommitteeResultType_ID != null)
            || db.Im_CheckRequest_SampleData.Any(s => s.Im_RequestCommittee_ID == c.ID && s.User_Deletion_Id == null && s.WithdrawDate != null && s.SampleSize > 0)
            || db.Im_Request_TreatmentData.Any(t => t.Im_RequestCommittee_ID == c.ID && t.User_Deletion_Id == null && t.User_Updation_Date != null));
}
