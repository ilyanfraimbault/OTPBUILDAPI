using System.Text.Json.Serialization;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Data;

Env.Load();

var server = Environment.GetEnvironmentVariable("DATABASE_HOST");
var uid = Environment.GetEnvironmentVariable("DATABASE_USER");
var pwd = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
var database = Environment.GetEnvironmentVariable("DATABASE_NAME");

var connectionString = $"Server={server};UserID={uid};Password={pwd};Database={database};AllowUserVariables=True;";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Suppression ou modification de la gestion des références
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Ignore les cycles de références
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();