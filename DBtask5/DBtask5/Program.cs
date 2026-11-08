using DBtask5.Data;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using UniversityCafeteria.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//// Database First
builder.Services.AddDbContext<CafeteriaDbContext>(options =>
  options.UseSqlServer(
      builder.Configuration.GetConnectionString(
   "DatabaseFirstConnection")));

// Code First
builder.Services.AddDbContext<CafeteriaEmployeesDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "CodeFirstConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
