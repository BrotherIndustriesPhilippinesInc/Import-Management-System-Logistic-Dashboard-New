using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LogisticDashboard.API.Data;
using LogisticDashboard.API.Controllers;
using LogisticDashboard.API.Mapping;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins(
            "http://apbiphbpswb01:1116",
            "http://localhost:5235"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});
builder.Services.AddHttpClient();

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("LogisticDashboardAPIContext");

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<LogisticDashboardAPIContext>(options =>
    options.UseNpgsql(dataSource)
);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowLocalhost");   // ✅ dito na ilagay, pagkatapos ng UseRouting, bago ang MapControllers

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();