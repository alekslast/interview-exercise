using BusinessLogic.Implementations;
using BusinessLogic.Interfaces;
using InterviewExercise.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


builder.Services.AddControllersWithViews();


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();


services.AddTransient<GlobalExceptionHandlingMiddleware>();

services.AddScoped<IItemCheckService, ItemCheckService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();