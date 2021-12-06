using System.Reflection;
using Api.Extensions;
using Api.Middleware;
using Microsoft.EntityFrameworkCore;
using SS.Api.Contracts;
using SS.Common;
using SS.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add<LoggingMiddleware>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var contractsXmlFileName = $"{typeof(AuthorDto).Assembly.GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, contractsXmlFileName));
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddDbContext<AuthStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(AppConstants.AuthStoreDbName)));

builder.Services.AddCustomTypes();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.DocumentTitle = "SS API");
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.RunLatestMigrations();

app.Run();

public partial class Program
{
}