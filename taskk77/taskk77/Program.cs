using Microsoft.EntityFrameworkCore;
using taskk77.Models;

var builder = WebApplication.CreateBuilder(args);

// إضافة خدمات MVC
builder.Services.AddControllersWithViews();

// الاتصال بقاعدة البيانات
builder.Services.AddDbContext<Task7Context>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Connection string 'DefaultConnection' was not found.")
    ));

var app = builder.Build();

// قراءة العملاء عند بدء تشغيل التطبيق
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<Task7Context>();

    var customers = await db.Customers
        .AsNoTracking()
        .ToListAsync();

    foreach (var customer in customers)
    {
        Console.WriteLine(
            $"ID: {customer.CustomerID} | Name: {customer.FullName}");
    }
}

// إعداد تشغيل الموقع
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