using CoreStorage.StorageContext;
using ElmahCore.Mvc;
using Hangfire;
using LicenseProject.StartupModuleServices.ApplicationServices;
using LicenseProject.StartupModuleServices.AuthenticationService;
using LicenseProject.StartupModuleServices.ElmahSqlService;
using LicenseProject.StartupModuleServices.Hangfire;
using LicenseProject.StartupModuleServices.SqlSeedService;
using LicenseProject.StartupModuleServices.SqlStorageService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.RunAppService();
builder.Services.RunSqlServerService(builder.Configuration);
builder.Services.AddResponseCompression();
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.RunElmahSqlService(builder.Configuration);
builder.Services.RunHangfireService(builder.Configuration);
builder.Services.RunAuthentication();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// وقتی ادمین قراره داخل دیتابیس سید بشه .

app.RunInitialScope();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseElmah();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Client}/{action=ValidateClient}/{id?}");

app.Run();