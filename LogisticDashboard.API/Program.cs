using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LogisticDashboard.API.Data;
using LogisticDashboard.API.Controllers;
using LogisticDashboard.API.Mapping;

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

builder.Services.AddDbContext<LogisticDashboardAPIContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("LogisticDashboardAPIContext")
        ?? throw new InvalidOperationException("Connection string not found.")
    )
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowLocalhost");

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
