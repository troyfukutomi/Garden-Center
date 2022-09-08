using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using GardenCenter.Models;
using System.Reflection;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Data") ?? "Data Source=Data.db";

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSqlite<DatabaseContext>(connectionString);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "GardenCenter",
            Description = "Final capstone for dotnet module",
            TermsOfService = new Uri("https://go.microsoft.com/fwlink/?LinkID=206977"),
            Contact = new OpenApiContact
            {
                Name = "Troy Fukutomi",
                Email = string.Empty,
                Url = new Uri("https://www.microsoft.com/learn")
            }
        });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    }

);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
