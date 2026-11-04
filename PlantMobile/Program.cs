using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using PlantMobile.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddPooledDbContextFactory<EF.Imports.ImportDbContext>(options =>
    PlantMobile.Data.ImportDatabase.Configure(options, builder.Configuration));
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<PlantMobile.Data.MenuReader>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Logging.AddProvider(new DailyFileLoggerProvider(
    Path.Combine(builder.Environment.ContentRootPath, "Logs")));
/////////////////rrr8-5-2025/////////////////////////
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
//cookies - caches - session
builder.Services.AddDistributedMemoryCache(); //For developent
// builder.Services.AddStackExchangeRedisCache(...); // For production
/////////////////rrr8-5-2025/////////////////////////

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Or Always in production
    options.Cookie.SameSite = SameSiteMode.Lax;
});

//builder.Services.AddSession();

//builder.Services.AddPersistence(builder.Configuration);
////////////////////////
// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Replace raw 404 responses for page requests with a friendly "ask the admin" page.
// Static assets (paths with a file extension) and pages that already produced a
// response keep the real 404. Registered first so it observes the final status.
app.Use(async (context, next) =>
{
    await next();

    var response = context.Response;
    if (response.StatusCode != StatusCodes.Status404NotFound || response.HasStarted)
    {
        return;
    }

    var path = context.Request.Path.Value ?? string.Empty;

    if (Path.HasExtension(path) ||
        path.StartsWith("/Unavailable", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/Home/Error", StringComparison.OrdinalIgnoreCase) ||
        response.ContentLength is > 0)
    {
        return;
    }

    response.Redirect("/Unavailable?code=404");
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Add this
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseMiddleware<RequestLoggingMiddleware>();
app.Use(async (context, next) =>
{
    bool isLoginRequest = context.Request.Path.StartsWithSegments("/Login");
    bool isErrorRequest = context.Request.Path.StartsWithSegments("/Home/Error");
    bool isAuthenticated = context.Session.GetString("UserSession") == "Authenticated";

    if (!isAuthenticated && !isLoginRequest && !isErrorRequest)
    {
        context.Response.Redirect("/Login/Index");
        return;
    }

    await next();
});
app.Use(async (context, next) =>
{
    // These legacy screens depend on external APIs and are intentionally disabled.
    string path = context.Request.Path.Value ?? string.Empty;
    string[] disabledApiScreens = ["/Test", "/shohna_sader"];

    if (disabledApiScreens.Any(prefix => path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
    {
        context.Response.StatusCode = StatusCodes.Status410Gone;
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync("تم إلغاء هذه الشاشة لأنها كانت تعتمد على API خارجي.");
        return;
    }

    await next();
});
app.UseCors("AllowAll");
app.UseCookiePolicy();
app.UseAuthorization();




//app.UseEndpoints(endpoints => {
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");
//});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "AgricultureLaw",
    pattern: "AgricultureLaw/{action=Index}/{id?}",
    defaults: new { controller = "AgricultureLaw" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.Run();











