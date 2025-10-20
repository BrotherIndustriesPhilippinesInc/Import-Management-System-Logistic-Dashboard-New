using LogisticDashboard.Web.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// 💡 Get the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 💡 Build a custom data source with dynamic JSON enabled
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.EnableDynamicJson(); // 👈 This is the key
var dataSource = dataSourceBuilder.Build();

// 💡 Use the data source for EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(dataSource)); // <-- use the dataSource here, NOT the connection string!

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
