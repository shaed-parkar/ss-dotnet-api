using System.Reflection;
using Api.Extensions;
using Api.Middleware;
using Api.Middleware.Validation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SS.Api.Contracts;
using SS.Common;
using SS.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomTypes();

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString(AppConstants.AuthStoreDbName));

builder.Services.AddControllers(options => options.Filters.Add<LoggingActionFilter>());
builder.Services.AddControllers(options => options.Filters.Add<RequestModelValidatorFilter>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IRequestModelValidatorService>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var contractsXmlFileName = $"{typeof(AuthorDto).Assembly.GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, contractsXmlFileName));
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["ApplicationInsights:InstrumentationKey"]);

builder.Services.AddDbContext<AuthStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(AppConstants.AuthStoreDbName)));

var app = builder.Build();

app.RunLatestMigrations();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI(options => options.DocumentTitle = "SS API");
// }

// app.UseHttpsRedirection();
app.UseRouting().UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        AllowCachingResponses = false,
        ResponseWriter = WriteHealthCheckResponse
    });
});
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<LogResponseBodyMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.Run();

public partial class Program
{
    private static Task WriteHealthCheckResponse(HttpContext context, HealthReport result)
    {
        context.Response.ContentType = "application/json";

        var json = new JObject(
            new JProperty("status", result.Status.ToString()),
            new JProperty("results", new JObject(result.Entries.Select(pair =>
                new JProperty(pair.Key, new JObject(
                    new JProperty("status", pair.Value.Status.ToString()),
                    new JProperty("description", pair.Value.Description),
                    new JProperty("data", new JObject(pair.Value.Data.Select(
                        p => new JProperty(p.Key, p.Value))))))))));

        return context.Response.WriteAsync(
            json.ToString(Formatting.Indented));
    }
}