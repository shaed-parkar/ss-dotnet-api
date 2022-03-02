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
builder.Services.AddSwagger();

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
app.UseExceptionMapper();
app.UseMiddleware<LogResponseBodyMiddleware>();
app.AddHealthCheck();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}